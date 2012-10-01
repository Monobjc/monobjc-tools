using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("clean-classic-documents")]
	public class CleanClassicDocumentsTask : PatchDocumentsTask
	{
		private const String DISCUSSION_BEGIN = "<!-- begin discussion -->";
		private const String DISCUSSION_END = "<!-- end discussion -->";
		private static readonly Regex DISCUSSION_OPENING_REGEX = new Regex (@"(?m-isnx:)(<p>)", RegexOptions.Multiline);
		private static readonly Regex DISCUSSION_CLOSING_REGEX = new Regex (@"(?m-isnx:)(</p>)", RegexOptions.Multiline);
		private static PatchReplace[] replacements = new[]
		{
			new PatchReplace {Source = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\" \"http://www.w3.org/TR/1998/REC-html40-19980424/loose.dtd\">", With = "<!DOCTYPE html>"},
			new PatchReplace {Source = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"", With = "<!DOCTYPE html>"},
			new PatchReplace {Source = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Frameset//EN\"", With = "<!DOCTYPE html>"},
			new PatchReplace {Source = "\"http://www.w3.org/TR/1998/REC-html40-19980424/loose.dtd\">", With = String.Empty},
			new PatchReplace {Source = "\"http://www.w3.org/TR/1999/REC-html401-19991224/frameset.dtd\">", With = String.Empty},
			new PatchReplace {Source = "<hr>", With = "<hr/>"},
			new PatchReplace {Source = "<br>", With = "<br/>"},
			new PatchReplace {Source = "<p><!-- begin abstract --><p>", With = "<p>"},
			new PatchReplace {Source = "</p>\n<!-- end abstract --></p>", With = "</p>"},
			new PatchReplace {Source = "<hr class=\"betweenAPIEntries\">", With = "<hr class=\"betweenAPIEntries\"/>"},
			new PatchReplace {Source = "<hr class=\"afterName\">", With = "<hr class=\"afterName\"/>"},
		};

		protected override void ExecuteTask ()
		{
			String baseFolder = this.CreateBaseDir ();
			DocSet docSet = this.CreateDocSet ();
			IEnumerable<Framework> frameworks = this.CreateFrameworks (docSet);

			foreach (var f in frameworks) {
				if (f.style != PageStyle.Classic) {
					continue;
				}
				foreach (var e in f.GetEntities()) {
					this.Patch (baseFolder, e, replacements);
					this.PatchDiscussion (baseFolder, docSet, f, e);
				}
			}
		}

		protected void PatchDiscussion (String baseFolder, DocSet docSet, Framework f, FrameworkEntity entity)
		{
			String path = entity.GetPath (baseFolder, this.Type);
			if (path == null || !File.Exists(path)) {
				return;
			}
			
			String contents = File.ReadAllText (path);
			bool modified = false;
				
			// Search for discussion parts
			int index = 0;
			int beginning = 0;
			int ending = 0;
			while ((beginning = contents.IndexOf(DISCUSSION_BEGIN, index)) != -1) {
				ending = contents.IndexOf (DISCUSSION_END, beginning);
				String content = contents.Substring (beginning, ending + DISCUSSION_END.Length - beginning);
				String replacement = content;
					
				// Count opening and closing paragraphs
				int opening = DISCUSSION_OPENING_REGEX.Matches (replacement).Count;
				int closing = DISCUSSION_CLOSING_REGEX.Matches (replacement).Count;
					
				if (opening < closing) {
					throw new NotSupportedException ();
				}
				if (opening > closing) {
					replacement = replacement.Replace ("<!-- begin discussion -->", String.Empty);
					replacement = replacement.Replace ("<!-- end discussion -->", String.Empty);
					for (int i = closing; i < opening; i++) {
						replacement += "</p>";
					}
					replacement = "<!-- begin discussion -->" + replacement;
					replacement = replacement + "<!-- end discussion -->";
						
					contents = contents.Replace (content, replacement);
					modified |= true;
				}
					
				index = beginning + replacement.Length;
			}
			
			if (modified) {
				this.Log (Level.Info, String.Format ("Patching {0} for '{1}'...", this.Type, entity.name));
				File.WriteAllText (path, contents);
			} else {
				this.Log (Level.Debug, String.Format ("Skipping {0} for '{1}'...", this.Type, entity.name));
			}
		}
	}
}
