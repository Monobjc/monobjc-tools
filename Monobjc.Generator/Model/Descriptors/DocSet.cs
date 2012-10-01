using System;
using System.IO;

namespace Monobjc.Tools.Generator.Model
{
	public partial class DocSet
	{
		private String absolutePath;

		public DocSet (String name, String path)
		{
			this.Name = name;
			this.Path = path;
		}

		public String Name {
			get;
			private set;
		}

		public String Path {
			get;
			private set;
		}

		/// <summary>
		/// Gets the absolute path of the doc set.
		/// </summary>
		public String AbsolutePath {
			get {
				if (this.absolutePath == null) {
					String value = this.Path;
					if (value.Contains("~")) {
						value = value.Replace("~", Environment.GetEnvironmentVariable("HOME"));
					}
					this.absolutePath = value;
				}
				return this.absolutePath;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this doc set is found on the system.
		/// </summary>
		public bool IsInstalled
		{
			get {
				return Directory.Exists(this.AbsolutePath);
			}
		}
	}
}
