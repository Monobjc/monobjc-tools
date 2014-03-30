//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2014 - Laurent Etiemble
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

		/// <summary>
		///   Initializes a new instance of the <see cref = "ProcessHelper" /> class.
		/// </summary>
		/// <param name = "command">The command.</param>
		/// <param name = "arguments">The arguments.</param>
		public ProcessHelper (String command, String arguments)
		{
			this.Logger = new NullLogger ();

			this.process = new Process {EnableRaisingEvents = true};
			this.process.ErrorDataReceived += this.ProcessErrorDataReceived;
			this.process.OutputDataReceived += this.ProcessOutputDataReceived;

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
		/// Gets or sets the output writer.
		/// </summary>
		/// <value>The output writer.</value>
		public TextWriter OutputWriter { get; set; }

		/// <summary>
		/// Gets or sets the error writer.
		/// </summary>
		/// <value>The error writer.</value>
		public TextWriter ErrorWriter { get; set; }

		/// <summary>
		///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose ()
		{
		}

		/// <summary>
		///   Executes this external process.
		/// </summary>
		public int Execute ()
		{
			bool internalOutput = false;
			bool internalError = false;

			try {
				if (this.OutputWriter == null) {
					this.OutputWriter = new StringWriter ();
					internalOutput = true;
				}
				if (this.ErrorWriter == null) {
					this.ErrorWriter = new StringWriter ();
					internalError = true;
				}

				this.process.Start ();
				this.process.BeginErrorReadLine ();
				this.process.BeginOutputReadLine ();
				this.process.WaitForExit ();

				return this.process.ExitCode;
			} catch (Exception ex) {
				if (this.Logger != null) {
					this.Logger.LogError (ex.ToString ());
				}
				return -1;
			} finally {
				if (internalOutput) {
					this.OutputWriter.Dispose ();
				}
				if (internalError) {
					this.ErrorWriter.Dispose ();
				}
			}
		}

		/// <summary>
		///   Executes this external process.
		/// </summary>
		public String ExecuteAndReturnOutput()
		{
			using(StringWriter writer = new StringWriter()) {
				this.OutputWriter = writer;
				this.Execute();
				return writer.ToString();
			}
		}

		/// <summary>
		///   Handles the OutputDataReceived event of the Process control.
		/// </summary>
		/// <param name = "sender">The source of the event.</param>
		/// <param name = "e">The <see cref = "System.Diagnostics.DataReceivedEventArgs" /> instance containing the event data.</param>
		private void ProcessOutputDataReceived (object sender, DataReceivedEventArgs e)
		{
			this.OutputWriter.WriteLine (e.Data);
			if (this.Logger != null) {
				this.Logger.LogInfo (e.Data);
			}
		}

		/// <summary>
		///   Handles the ErrorDataReceived event of the Process control.
		/// </summary>
		/// <param name = "sender">The source of the event.</param>
		/// <param name = "e">The <see cref = "System.Diagnostics.DataReceivedEventArgs" /> instance containing the event data.</param>
		private void ProcessErrorDataReceived (object sender, DataReceivedEventArgs e)
		{
			this.ErrorWriter.WriteLine (e.Data);
			if (this.Logger != null) {
				this.Logger.LogError (e.Data);
			}
		}
	}
}