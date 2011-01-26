//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2011 - Laurent Etiemble
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
using System.Globalization;
using Monobjc.Tools.External;
using Monobjc.Tools.Properties;
using Monobjc.Tools.PropertyList;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Generators
{
    /// <summary>
    ///   Wrapper for XIB file compilation.
    /// </summary>
    public class XibCompiler
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XibCompiler" /> class.
        /// </summary>
        public XibCompiler()
        {
            this.Logger = new NullLogger();
        }

        /// <summary>
        ///   Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public IExecutionLogger Logger { get; set; }

        /// <summary>
        ///   Compiles the specified XIB file.
        /// </summary>
        /// <param name = "xibFile">The XIB file.</param>
        /// <param name = "directory">The directory.</param>
        /// <returns><code>true</code> if the compilation is successful, <code>false</code> otherwise.</returns>
        public bool Compile(String xibFile, String directory)
        {
            PListDocument result = XibTool.Compile(xibFile, directory);
            if (result == null)
            {
                this.Logger.LogInfo(String.Format(CultureInfo.CurrentCulture, Resources.IBFileUpToDate, xibFile));
                return true;
            }

            PList root = result.Root;
            PListDict dict = root.Dict;
            if (dict == null)
            {
                this.Logger.LogError(Resources.NoDictionaryFound);
                return false;
            }

            // Global errors
            if (dict.ContainsKey(XibTool.ERRORS))
            {
                PListArray errors = (PListArray) dict[XibTool.ERRORS];
                foreach (PListItemBase item in errors)
                {
                    PListDict error = item as PListDict;
                    if (error == null)
                    {
                        continue;
                    }
                    PListString description = error[XibTool.KEY_DESCRIPTION] as PListString;
                    if (description != null)
                    {
                        this.Logger.LogError(description.Value);
                    }
                }

                // If there was global errors, return
                if (errors.Count > 0)
                {
                    return false;
                }
            }

            // Document errors
            PListDict documentErrors = (PListDict) dict[XibTool.DOCUMENT_ERRORS];
            foreach (String key in documentErrors.Keys)
            {
                PListArray elementErrors = documentErrors[key] as PListArray;
                if (elementErrors == null)
                {
                    continue;
                }
                foreach (PListItemBase item in elementErrors)
                {
                    PListDict error = item as PListDict;
                    PListString type = error[XibTool.KEY_TYPE] as PListString;
                    PListString message = error[XibTool.KEY_MESSAGE] as PListString;
                    if (type != null && message != null)
                    {
                        this.Logger.LogError(String.Format(CultureInfo.CurrentCulture, Resources.ValueDescriptionFormat, type.Value, message.Value));
                    }
                }
            }

            // If there was document errors, return
            if (documentErrors.Count > 0)
            {
                return false;
            }

            // Document warnings
            PListDict documentWarnings = (PListDict) dict[XibTool.DOCUMENT_WARNINGS];
            foreach (String key in documentWarnings.Keys)
            {
                PListArray elementWarnings = documentWarnings[key] as PListArray;
                if (elementWarnings == null)
                {
                    continue;
                }
                foreach (PListItemBase item in elementWarnings)
                {
                    PListDict error = item as PListDict;
                    PListString type = error[XibTool.KEY_TYPE] as PListString;
                    PListString message = error[XibTool.KEY_MESSAGE] as PListString;
                    if (type != null && message != null)
                    {
                        this.Logger.LogWarning(String.Format(CultureInfo.CurrentCulture, Resources.ValueDescriptionFormat, type.Value, message.Value));
                    }
                }
            }

            // Document notices
            PListDict documentNotices = (PListDict) dict[XibTool.DOCUMENT_NOTICES];
            foreach (String key in documentNotices.Keys)
            {
                PListArray elementNotices = documentNotices[key] as PListArray;
                if (elementNotices == null)
                {
                    continue;
                }
                foreach (PListItemBase item in elementNotices)
                {
                    PListDict error = item as PListDict;
                    PListString type = error[XibTool.KEY_TYPE] as PListString;
                    PListString message = error[XibTool.KEY_MESSAGE] as PListString;
                    if (type != null && message != null)
                    {
                        this.Logger.LogInfo(String.Format(CultureInfo.CurrentCulture, Resources.ValueDescriptionFormat, type.Value, message.Value));
                    }
                }
            }

            return true;
        }
    }
}