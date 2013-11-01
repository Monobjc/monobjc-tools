//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2013 - Laurent Etiemble
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
using System.IO;

namespace Monobjc.Tools.Utilities
{
    public class NativeContext
    {
        private const string PPC = "-arch ppc";
        private const string PPC64 = "-arch ppc64";
        private const string X86 = "-arch i386";
        private const string X8664 = "-arch x86_64";
        private const String SDK_PATTERN = "-mmacosx-version-min={0}";

        private readonly MacOSVersion version;
        private readonly MacOSArchitecture architecture;

        public NativeContext(MacOSVersion version, MacOSArchitecture architecture)
        {
            this.version = version;
            this.architecture = architecture;
        }

        /// <summary>
        ///   Writes the support header.
        /// </summary>
        /// <param name = "directory">The directory.</param>
        public void WriteHeader(String directory)
        {
            String file = Path.Combine(directory, "monobjc.h");
            FileProvider.CopyFile(this.version, "monobjc.h", file);
        }

        /// <summary>
        ///   Writes the support library.
        /// </summary>
        /// <param name = "directory">The directory.</param>
        public void WriteLibrary(String directory)
        {
            String file = Path.Combine(directory, "libmonobjc.dylib");
            FileProvider.CopyFile(this.version, "libmonobjc.dylib", file);
        }

        /// <summary>
        ///   Gets the compiler.
        /// </summary>
        /// <value>The compiler.</value>
        public String Compiler
        {
            get
            {
                switch (this.version)
                {
                    case MacOSVersion.MacOS105:
                    case MacOSVersion.MacOS106:
                        return "cc";
                    case MacOSVersion.MacOS107:
                    case MacOSVersion.MacOS108:
                    case MacOSVersion.MacOS109:
                        return "clang";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///   Gets the SDK flags.
        /// </summary>
        /// <value>The SDK flags.</value>
        public String SDKFlags
        {
            get
            {
                switch (this.version)
                {
                    case MacOSVersion.MacOS105:
                        return String.Format(CultureInfo.CurrentCulture, SDK_PATTERN, "10.5");
                    case MacOSVersion.MacOS106:
                        return String.Format(CultureInfo.CurrentCulture, SDK_PATTERN, "10.6");
                    case MacOSVersion.MacOS107:
                        return String.Format(CultureInfo.CurrentCulture, SDK_PATTERN, "10.7");
                    case MacOSVersion.MacOS108:
                        return String.Format(CultureInfo.CurrentCulture, SDK_PATTERN, "10.8");
                    case MacOSVersion.MacOS109:
                        return String.Format(CultureInfo.CurrentCulture, SDK_PATTERN, "10.9");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///   Gets the architecture flags.
        /// </summary>
        /// <value>The architecture flags.</value>
        public String ArchitectureFlags
        {
            get
            {
                switch (this.architecture)
                {
                    case MacOSArchitecture.PPC:
                        return PPC;
                    case MacOSArchitecture.PPC64:
                        return PPC64;
                    case MacOSArchitecture.X86:
                        return X86;
                    case MacOSArchitecture.X8664:
                        return X8664;
                    case MacOSArchitecture.Intel:
                        return String.Join(" ", new[] {X86, X8664});
                    case MacOSArchitecture.Universal32:
                        return String.Join(" ", new[] {X86, PPC});
                    case MacOSArchitecture.Universal3264:
                        return String.Join(" ", new[] {X86, PPC, X8664});
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}