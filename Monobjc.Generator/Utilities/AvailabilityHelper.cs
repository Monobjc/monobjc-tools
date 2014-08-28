using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.Utilities
{
	public static class AvailabilityHelper
	{
        private static readonly IDictionary<string, string> AVAILABILITIES_STRINGS = CreateAvailabilityStrings ();
        private static readonly IDictionary<string, Version> AVAILABILITIES_VERSIONS = CreateAvailabilityVersions ();
		
		private static readonly Regex MIN_MAX = new Regex(@"\(Available in (OS X v10\.\d+) through (OS X v10\.\d+)");
		
		private static readonly Regex DEPRECATED = new Regex(@"\((Deprecated\sin\s)(OS\sX\sv\s?10\.\d+(?:\.\d+)?)(?:\.| and later\.)(.*)\)");

        /// <summary>
        ///   Gets the define string for the given version of OS.
        /// </summary>
        public static String GetDefine (String availability)
        {
            if (String.IsNullOrEmpty (availability)) {
                return null;
            }
            if (AVAILABILITIES_STRINGS.ContainsKey (availability)) {
                return AVAILABILITIES_STRINGS [availability];
            }
            //Console.WriteLine("Unknown availability => " + availability);
            return null;
        }

        /// <summary>
        ///   Gets the define string for the given version of OS.
        /// </summary>
        public static Version GetVersion (String availability, Version defaultVersion)
        {
            if (String.IsNullOrEmpty (availability)) {
                return defaultVersion;
            }
            if (AVAILABILITIES_VERSIONS.ContainsKey (availability)) {
                return AVAILABILITIES_VERSIONS [availability];
            }
            //Console.WriteLine("Unknown availability => " + availability);
            return defaultVersion;
        }

        /// <summary>
        /// Adds the availability mention.
        /// </summary>
		public static bool AddMention(BaseEntity baseEntity, String line)
		{
			bool modified = false;
			Match m = MIN_MAX.Match(line);
			if (m.Success)
			{
				String v1 = m.Groups[1].Value;
				String v2 = m.Groups[2].Value;

				if (!String.Equals(baseEntity.MinAvailability, v1))
				{
					baseEntity.MinAvailability = v1;
					modified |= true;
				}
				
				switch (v2)
				{
				case "OS X v10.0":
					v2 = "OS X v10.1";
					break;
				case "OS X v10.1":
					v2 = "OS X v10.2";
					break;
				case "OS X v10.2":
					v2 = "OS X v10.3";
					break;
				case "OS X v10.3":
					v2 = "OS X v10.4";
					break;
				case "OS X v10.4":
					v2 = "OS X v10.5";
					break;
				case "OS X v10.5":
					v2 = "OS X v10.6";
					break;
				case "OS X v10.6":
					v2 = "OS X v10.7";
					break;
                case "OS X v10.7":
                    v2 = "OS X v10.8";
                    break;
                case "OS X v10.8":
                    v2 = "OS X v10.9";
                    break;
                case "OS X v10.9":
                    v2 = "OS X v10.10";
                    break;
				default:
					break;
				}
				
				if (!String.Equals(baseEntity.MaxAvailability, v2))
				{
					baseEntity.MaxAvailability = v2;
					modified |= true;
				}
			}
			
			m = DEPRECATED.Match(line);
			if (m.Success)
			{
				String v2 = m.Groups[2].Value;
				String v3 = m.Groups[3].Value.Trim();

				if (!String.Equals(baseEntity.MaxAvailability, v2))
				{
					baseEntity.MaxAvailability = v2;
					modified |= true;
				}
				if (!String.Equals(baseEntity.Obsolete, v3))
				{
					baseEntity.Obsolete = v3;
					modified |= true;
				}
			}
			
			return modified;
		}

        /// <summary>
        ///   Set the define to use for each version of OS.
        /// </summary>
        private static IDictionary<string, string> CreateAvailabilityStrings ()
        {
            IDictionary<string, string> result = new Dictionary<string, string> ();
            result.Add ("OS X v10.0", "");
            result.Add ("OS X v10.1", "");
            result.Add ("OS X v10.2", "");
            result.Add ("OS X v10.3", "");
            result.Add ("OS X v10.3.9", "");
            result.Add ("OS X v10.4", "");
            result.Add ("OS X v10.5", "");
            result.Add ("OS X v10.6", "MACOSX_10_6");
            result.Add ("OS X v10.7", "MACOSX_10_7");
            result.Add ("OS X v10.8", "MACOSX_10_8");
            result.Add ("OS X v10.9", "MACOSX_10_9");
            result.Add ("Sparkle 1.0", "");
            result.Add ("Sparkle 1.5", "");
            return result;
        }

        /// <summary>
        ///   Set the define to use for each version of OS.
        /// </summary>
        private static IDictionary<string, Version> CreateAvailabilityVersions ()
        {
            IDictionary<string, Version> result = new Dictionary<string, Version> ();
            result.Add ("OS X v10.0", new Version(10, 0));
            result.Add ("OS X v10.1", new Version(10, 1));
            result.Add ("OS X v10.2", new Version(10, 2));
            result.Add ("OS X v10.3", new Version(10, 3));
            result.Add ("OS X v10.3.9", new Version(10, 3, 9));
            result.Add ("OS X v10.4", new Version(10, 4));
            result.Add ("OS X v10.5", new Version(10, 5));
            result.Add ("OS X v10.6", new Version(10, 6));
            result.Add ("OS X v10.7", new Version(10, 7));
            result.Add ("OS X v10.8", new Version(10, 8));
            result.Add ("OS X v10.9", new Version(10, 9));
            result.Add ("Sparkle 1.0", new Version(1, 0));
            result.Add ("Sparkle 1.5", new Version(1, 5));
            return result;
        }
	}
}
