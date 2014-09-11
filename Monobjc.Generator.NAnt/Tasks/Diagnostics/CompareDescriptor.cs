using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Monobjc.Tools.Generator.Model;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Monobjc.Tools.Generator.NAnt
{
    [TaskName("compare-descriptor")]
    public class CompareDescriptor : Task
    {
        /// <summary>
        /// Gets or sets the first file.
        /// </summary>
        [TaskAttribute("file1", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public FileInfo File1 { get; set; }

        /// <summary>
        /// Gets or sets the second file.
        /// </summary>
        [TaskAttribute("file2", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public FileInfo File2 { get; set; }

        /// <summary>
        /// Gets or sets the mapping file.
        /// </summary>
        [TaskAttribute("report", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public FileInfo Report { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        protected override void ExecuteTask()
        {
            Framework descriptor1 = Framework.LoadFromFile(this.File1.ToString());
            Framework descriptor2 = Framework.LoadFromFile(this.File2.ToString());

            TextWriter writer = null;
            if (this.Report != null) {
                writer = new StreamWriter(this.Report.ToString());
            }

            writer.WriteLine("Types");
            Dump(writer, descriptor1.Types, descriptor2.Types);
            writer.WriteLine("Classes");
            Dump(writer, descriptor1.Classes, descriptor2.Classes);
            writer.WriteLine("Protocols");
            Dump(writer, descriptor1.Protocols, descriptor2.Protocols);

            if (this.Report != null) {
                writer.Close();
                writer.Dispose();
            }
        }

        private void Dump(TextWriter writer, IEnumerable<FrameworkEntity> entities1, IEnumerable<FrameworkEntity> entities2)
        {
            IEnumerable<String> names1 = entities1.Select(e => e.name);
            IEnumerable<String> names2 = entities2.Select(e => e.name);

            foreach (String s in names2.Except(names1)) {
                writer.WriteLine(s);
            }
        }
    }
}
