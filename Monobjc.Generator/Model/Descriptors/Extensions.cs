using System;
using System.IO;
using System.Collections.Generic;

namespace Monobjc.Tools.Generator.Model
{
	public static class FrameworkExtensions
	{
		public static IEnumerable<FrameworkEntity> GetEntities (this Framework framework)
		{
			if (framework == null) {
				throw new ArgumentNullException ("framework");
			}
			if (framework.Types != null) {
				foreach (var e in framework.Types) {
					e.Framework = framework;
					yield return e;
				}
			}
			if (framework.Classes != null) {
				foreach (var e in framework.Classes) {
					e.Framework = framework;
					yield return e;
				}
			}
			if (framework.Protocols != null) {
				foreach (var e in framework.Protocols) {
					e.Framework = framework;
					yield return e;
				}
			}
			if (framework.Structures != null) {
				foreach (var e in framework.Structures) {
					e.Framework = framework;
					yield return e;
				}
			}
			if (framework.Enumerations != null) {
				foreach (var e in framework.Enumerations) {
					e.Framework = framework;
					yield return e;
				}
			}
		}
	}

	public static class FrameworkEntityExtensions
	{
		public static void CopyDocument (this FrameworkEntity entity, String baseFolder, DocumentType sourceType, DocumentType destinationType)
		{
			if (entity == null) {
				throw new ArgumentNullException ("entity");
			}

			String sourcePath = entity.GetPath (baseFolder, sourceType);
			String destintationPath = entity.GetPath (baseFolder, destinationType);

			if (!File.Exists (sourcePath)) {
				// Do nothing
			} else if (!File.Exists (destintationPath)) {
				File.Copy (sourcePath, destintationPath, true);
			} else if (File.GetLastWriteTimeUtc (destintationPath) < File.GetLastWriteTimeUtc (sourcePath)) {
				File.Copy (sourcePath, destintationPath, true);
			}
		}
	}
}
