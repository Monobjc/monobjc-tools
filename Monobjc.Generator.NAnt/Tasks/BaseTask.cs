using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.NAnt
{
	public abstract class BaseTask : Task
	{
		/// <summary>
		/// Gets or sets the base dir.
		/// </summary>
		[TaskAttribute("dir", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public DirectoryInfo BaseDir { get; set; }

		/// <summary>
		/// Gets or sets the document set.
		/// </summary>
		[BuildElement("docset", Required = false)]
		public DocSetElement DocSet { get; set; }

		/// <summary>
		/// Gets or sets the framework set.
		/// </summary>
		[BuildElement("framework-set", Required = true)]
		public FrameworkSetElement FrameworkSet { get; set; }

		protected String CreateBaseDir()
		{
			return this.BaseDir.ToString();
		}

		protected DocSet CreateDocSet()
		{
			if (this.DocSet != null) {
				return this.DocSet.CreateDocSet();
			}
			return null;
		}
		
		protected IEnumerable<Framework> CreateFrameworks(DocSet docSet)
		{
			List<Framework> frameworks = this.FrameworkSet.CreateFrameworks().ToList();
			foreach(var f in frameworks.Where(f => f.source == DocumentOrigin.DocSet)) {
				f.DocSet = docSet;
			}
			return frameworks;
		}
	}
}
