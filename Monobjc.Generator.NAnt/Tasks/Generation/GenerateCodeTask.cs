using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Generators;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("generate-code")]
	public class GenerateCodeTask : BaseTask
	{
		private String license;
		private GenerationStatistics statistics;
		private IDictionary<String, String> mixedTypes;
		private TypeManager typeManager;

		/// <summary>
		/// Gets or sets the mapping file.
		/// </summary>
		[TaskAttribute("license", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public FileInfo LicenseFile { get; set; }
		
		/// <summary>
		/// Gets or sets the mixed types file.
		/// </summary>
		[TaskAttribute("mixed-types", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public FileInfo MixedTypesFile { get; set; }

		/// <summary>
		/// Executes the task.
		/// </summary>
		protected override void ExecuteTask ()
		{
			String baseFolder = this.CreateBaseDir ();
			DocSet docSet = this.CreateDocSet ();
			IEnumerable<Framework> frameworks = this.CreateFrameworks (docSet);
			IList<FrameworkEntity> entities = frameworks.SelectMany (f => f.GetEntities ()).ToList ();

			this.statistics = new GenerationStatistics ();
			this.license = this.LoadLicense ();
			this.mixedTypes = this.LoadMixedTypes (baseFolder, entities);

			this.typeManager = new TypeManager ();
			Func<FrameworkEntity, bool> selector = delegate(FrameworkEntity e) {
				return e.type == FrameworkEntityType.C || e.type == FrameworkEntityType.P;
			};
			var names = entities.Where (selector).Select (e => e.name);
			this.typeManager.SetClasses (names);

			foreach (var f in frameworks) {
				foreach (var e in f.GetEntities()) {
					if (e.name.EndsWith (".Deprecated")) {
						continue;
					}

					this.Log (Level.Verbose, String.Format ("Processing '{0}'...", e.name));
					
					String sourcePath = e.GetPath (baseFolder, DocumentType.Model);
					String destinationPath = e.GetPath (baseFolder, DocumentType.Generated);

					if (sourcePath == null || !File.Exists (sourcePath)) {
						continue;
					}

					String file = null;
					if (e.type == FrameworkEntityType.P) {
						file = destinationPath + ".Protocol.cs";
					} else {
						file = destinationPath + ".cs";
					}

					if (sourcePath.IsYoungerThan (file)) {
						this.Generate (baseFolder, entities, f, e, sourcePath, destinationPath);
					} else {
						this.Log (Level.Debug, String.Format ("Skipping '{0}'...", e.name));
					}
				}
			}

			this.Log (Level.Info, statistics.ToString ());
		}

		private void Generate (String baseFolder, IList<FrameworkEntity> entities, Framework f, FrameworkEntity e, String sourcePath, String destinationPath)
		{
			switch (e.type) {
			case FrameworkEntityType.T:
				this.GenerateTypedEntity (baseFolder, entities, f, e, sourcePath, destinationPath);
				break;
			case FrameworkEntityType.C:
				this.GenerateClassEntity (baseFolder, entities, f, e, sourcePath, destinationPath);
				break;
			case FrameworkEntityType.P:
				this.GenerateProtocolEntity (baseFolder, entities, f, e, sourcePath, destinationPath);
				break;
			case FrameworkEntityType.E:
				this.GenerateEnumerationEntity (baseFolder, entities, f, e, sourcePath, destinationPath);
				break;
			default:
				break;
			}
		}

		private void GenerateTypedEntity (String baseFolder, IList<FrameworkEntity> entities, Framework f, FrameworkEntity e, String sourcePath, String destinationPath)
		{
			String file;
			TypedEntity entity = BaseEntity.LoadFrom<TypedEntity> (sourcePath);
			if (!entity.Generate) {
				return;
			}

			// Make sure that availability is ready
			entity.AdjustAvailability ();
			
			this.Log (Level.Info, String.Format ("Generating '{0}'...", e.name));
			
			file = destinationPath + ".cs";
			this.Generate<TypedGenerator> (f, entity, file);

//			if (entity.HasOpaqueFunctions) {
//				file = destinationPath + ".Wrap.cs";
//				this.Generate<OpaqueTypedGenerator> (f, entity, file);
//			}
		}

		private void GenerateClassEntity (String baseFolder, IList<FrameworkEntity> entities, Framework f, FrameworkEntity e, String sourcePath, String destinationPath)
		{
			String file;
			ClassEntity entity = BaseEntity.LoadFrom<ClassEntity> (sourcePath);
			if (!entity.Generate) {
				return;
			}
			
			// Make sure that availability is ready
			entity.AdjustAvailability ();
			
			this.Log (Level.Info, String.Format ("Generating '{0}'...", e.name));
			
			this.LoadClassDependencies (baseFolder, entities, entity);
			
			if (entity.ExtendedClass == null) {
				file = destinationPath + ".cs";
				this.Generate<ClassDefinitionGenerator> (f, entity, file);

				file = destinationPath + ".Class.cs";
				this.Generate<ClassMembersGenerator> (f, entity, file);

				if (entity.HasConstructors) {
					file = destinationPath + ".Constructors.cs";
					this.Generate<ClassConstructorsGenerator> (f, entity, file);
				}

				if (entity.HasConstants || entity.HasEnumerations) {
					file = destinationPath + ".Constants.cs";
					this.Generate<ClassConstantsGenerator> (f, entity, file);
				}

				if (entity.Protocols != null && entity.Protocols.Count > 0) {
					file = destinationPath + ".Protocols.cs";
					this.Generate<ClassProtocolsGenerator> (f, entity, file);
				}
			} else {
				file = destinationPath + ".cs";
				this.Generate<ClassAdditionsGenerator> (f, entity, file);

				if (entity.HasConstants || entity.HasEnumerations) {
					file = destinationPath + ".Constants.cs";
					this.Generate<ClassConstantsGenerator> (f, entity, file);
				}
			}
		}

		private void GenerateProtocolEntity (String baseFolder, IList<FrameworkEntity> entities, Framework f, FrameworkEntity e, String sourcePath, String destinationPath)
		{
			String file;
			ProtocolEntity entity = BaseEntity.LoadFrom<ProtocolEntity> (sourcePath);
			if (!entity.Generate) {
				return;
			}
			
			// Make sure that availability is ready
			entity.AdjustAvailability ();
			
			this.Log (Level.Info, String.Format ("Generating '{0}'...", e.name));
			
			this.LoadProtocolDependencies (baseFolder, entities, entity);

			file = destinationPath + ".Protocol.cs";
			this.Generate<ProtocolGenerator> (f, entity, file);

			if (entity.DelegatorEntity != null) {
				String property = entity.DelegateProperty ?? "Delegate";
				String dir = Path.GetDirectoryName (destinationPath);
				file = Path.Combine (dir, entity.DelegatorEntity.Name + "." + property + ".cs");
				this.Generate<ClassDelegateGenerator> (f, entity, file);
			}

			if (entity.HasConstants || entity.HasEnumerations) {
				file = destinationPath + ".Constants.cs";
				this.Generate<ClassConstantsGenerator> (f, entity, file);
			}
		}

		private void GenerateEnumerationEntity (String baseFolder, IList<FrameworkEntity> entities, Framework f, FrameworkEntity e, String sourcePath, String destinationPath)
		{
			String file;
			EnumerationEntity entity = BaseEntity.LoadFrom<EnumerationEntity> (sourcePath);
			if (!entity.Generate) {
				return;
			}
			
			// Make sure that availability is ready
			entity.AdjustAvailability ();
			
			this.Log (Level.Info, String.Format ("Generating '{0}'...", e.name));
			
			file = destinationPath + ".cs";
			this.Generate<EnumerationGenerator> (f, entity, file);
		}

		private void Generate<T> (Framework f, BaseEntity entity, String file) where T : BaseGenerator, new()
		{
			using (StreamWriter writer = new StreamWriter(file)) {
				T generator = new T ();
				generator.Writer = writer;
				generator.Statistics = this.statistics;
				generator.Usings = f.UsingList;
				generator.License = this.license;
				generator.MixedTypes = this.mixedTypes;
				generator.TypeManager = this.typeManager;
				generator.Generate (entity);
			}
		}
		
		private String LoadLicense ()
		{
			return File.ReadAllText (this.LicenseFile.ToString ());
		}

		private void LoadClasses(TypeManager manager, IList<FrameworkEntity> entities)
		{
			IEnumerable<String> names = entities.Where(e => e.type == FrameworkEntityType.C || e.type == FrameworkEntityType.P).Select(e => e.name);
			manager.SetClasses(names);
		}
		
		private Dictionary<String, String> LoadMixedTypes (String baseFolder, IList<FrameworkEntity> entities)
		{
			this.Log (Level.Info, "Loading mixed types...");
			String mixedTypesFile = this.MixedTypesFile.ToString ();
			Dictionary<String, String> table = new Dictionary<String, String> ();
			
			String[] lines = File.ReadAllLines (mixedTypesFile);
			foreach (String line in lines) {
				String[] parts = line.Split ('=');
				if (parts.Length != 2) {
					continue;
				}
				this.AddMixedType (table, parts [0], parts [1]);
			}
			this.Log (Level.Info, "Loaded {0} mixed types", table.Count);

			return table;
		}
		
		private void AddMixedType (Dictionary<String, String> table, String name, String mixedType)
		{
			if (String.IsNullOrWhiteSpace (mixedType)) {
				return;
			}
			this.Log (Level.Info, String.Format ("Adding MixedType {0}={1}", name, mixedType));
			table [name] = mixedType;
		}

		private void LoadClassDependencies (String baseFolder, IList<FrameworkEntity> entities, ClassEntity classEntity)
		{
			if (classEntity == null) {
				return;
			}
			
			this.Log (Level.Verbose, "Loading class dependencies for " + classEntity.Name);

			// Retrieve super class
			ClassEntity baseTypeEntity = this.FindAndLoad<ClassEntity> (baseFolder, entities, FrameworkEntityType.C, classEntity.BaseType);
			if (baseTypeEntity != null) {
				this.LoadClassDependencies (baseFolder, entities, baseTypeEntity);
				classEntity.SuperClass = baseTypeEntity;
			}

			// Retrieve protocols
			if (!String.IsNullOrEmpty (classEntity.ConformsTo)) {
				String[] parts = classEntity.ConformsTo.Split (new []{','}, StringSplitOptions.RemoveEmptyEntries);
				classEntity.Protocols = new List<ProtocolEntity> ();
				foreach (var p in parts) {
					ProtocolEntity protocolEntity = this.FindAndLoad<ProtocolEntity> (baseFolder, entities, FrameworkEntityType.P, p);
					if (protocolEntity == null) {
						continue;
					}
					classEntity.Protocols.Add (protocolEntity);
				}
			}

			// Retrieve extended class
			ClassEntity additionForEntity = this.FindAndLoad<ClassEntity> (baseFolder, entities, FrameworkEntityType.C, classEntity.AdditionFor);
			if (additionForEntity != null) {
				this.LoadClassDependencies (baseFolder, entities, additionForEntity);
				classEntity.ExtendedClass = additionForEntity;
			}
		}
	
		private void LoadProtocolDependencies (String baseFolder, IList<FrameworkEntity> entities, ProtocolEntity protocolEntity)
		{
			if (protocolEntity == null) {
				return;
			}

			this.Log (Level.Verbose, "Loading protocol dependencies for " + protocolEntity.Name);

			this.LoadClassDependencies (baseFolder, entities, protocolEntity);
			
			// Retrieve delegator class
			ClassEntity delegateForEntity = this.FindAndLoad<ClassEntity> (baseFolder, entities, FrameworkEntityType.C, protocolEntity.DelegateFor);
			if (delegateForEntity != null) {
				this.LoadClassDependencies (baseFolder, entities, delegateForEntity);
				protocolEntity.DelegatorEntity = delegateForEntity;
			}
		}
	}
}
