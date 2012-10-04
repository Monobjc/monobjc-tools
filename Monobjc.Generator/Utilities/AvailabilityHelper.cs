using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.Utilities
{
	public static class AvailabilityHelper
	{
		private static readonly IDictionary<string, string> AVAILABILITIES = CreateAvailabilities ();
		
		private static readonly Regex MIN_MAX = new Regex(@"\(Available in (OS X v10\.\d) through (OS X v10\.\d)");
		
		private static readonly Regex DEPRECATED = new Regex(@"\((Deprecated\sin\s)(OS\sX\sv\s?10\.\d(?:\.\d\d)?)(?:\.| and later\.)(.*)\)");

		/// <summary>
		///   Gets the define string for the given version of OS.
		/// </summary>
		/// <param name = "availability">The availability.</param>
		/// <returns></returns>
		public static String GetDefine (String availability)
		{
			if (String.IsNullOrEmpty (availability)) {
				return null;
			}
			if (AVAILABILITIES.ContainsKey (availability)) {
				return AVAILABILITIES [availability];
			}
			//Console.WriteLine("Unknown availability => " + availability);
			return null;
		}
		
		/// <summary>
		///   Set the define to use for each version of OS.
		/// </summary>
		public static IDictionary<string, string> CreateAvailabilities ()
		{
			IDictionary<string, string> result = new Dictionary<string, string> ();
			result.Add ("OS X v10.0", "");
			result.Add ("OS X v10.1", "");
			result.Add ("OS X v10.2", "");
			result.Add ("OS X v10.3", "");
			result.Add ("OS X v10.3.9", "");
			result.Add ("OS X v10.4", "");
			result.Add ("OS X v10.5", "MACOSX_10_5");
			result.Add ("OS X v10.6", "MACOSX_10_6");
			result.Add ("OS X v10.7", "MACOSX_10_7");
			result.Add ("OS X v10.8", "MACOSX_10_8");
			result.Add ("Sparkle 1.5", "");
			return result;
		}

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
	}
}
