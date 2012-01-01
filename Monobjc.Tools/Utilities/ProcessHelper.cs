//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2012 - Laurent Etiemble
//
// Monobjc is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// Monobjc is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Monobjc.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Monobjc.Tools.Utilities
{
    /// <summary>
    ///   Helper class used to launch an external process and to capture its output.
    /// </summary>
    internal class ProcessHelper : IDisposable
    {
        private readonly Process process;
        private readonly ProcessStartInfo processInfo;
        private readonly StringWriter writer;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ProcessHelper" /> class.
        /// </summary>
        /// <param name = "command">The command.</param>
        /// <param name = "arguments">The arguments.</param>
        public ProcessHelper(String command, String arguments)
        {
            this.Logger = new NullLogger();

            this.writer = new StringWriter(CultureInfo.InvariantCulture);
            this.process = new Process {EnableRaisingEvents = true};
            this.process.ErrorDataReceived += this.Process_ErrorDataReceived;
            this.process.OutputDataReceived += this.Process_OutputDataReceived;

            this.processInfo = new ProcessStartInfo
                                   {
                                       FileName = command,
                                       Arguments = arguments,
                                       RedirectStandardError = true,
                                       RedirectStandardInput = true,
                                       RedirectStandardOutput = true,
                                       UseShellExecute = false,
                                       CreateNoWindow = true
                                   };

            this.process.StartInfo = this.processInfo;
        }

        /// <summary>
        ///   Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public IExecutionLogger Logger { get; set; }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.writer.Dispose();
        }

        /// <summary>
        ///   Executes this external process.
        /// </summary>
        /// <returns>The complete ouput of the process</returns>
        public String Execute()
        {
            try
            {
                this.process.Start();
                this.process.BeginErrorReadLine();
                this.process.BeginOutputReadLine();
                this.process.WaitForExit();
            }
            catch (Exception ex)
            {
                if (this.Logger != null)
                {
                    this.Logger.LogError(ex.ToString());
                }
            }
            return this.writer.ToString();
        }

        /// <summary>
        ///   Handles the OutputDataReceived event of the Process control.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.Diagnostics.DataReceivedEventArgs" /> instance containing the event data.</param>
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.writer.WriteLine(e.Data);
            if (this.Logger != null)
            {
                this.Logger.LogInfo(e.Data);
            }
        }

        /// <summary>
        ///   Handles the ErrorDataReceived event of the Process control.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.Diagnostics.DataReceivedEventArgs" /> instance containing the event data.</param>
        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.writer.WriteLine(e.Data);
            if (this.Logger != null)
            {
                this.Logger.LogError(e.Data);
            }
        }
    }
}