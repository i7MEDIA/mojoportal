#if USESETTINGMAP

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Mono.Web.Util;
using System.Web.Configuration;
 
namespace mojoPortal.Web
{
	/// <summary>
	/// Author:				
	/// Created:			2007-12-13
	/// Last Modified:		2007-12-15
	/// 
	/// The use and distribution terms for this software are covered by the 
	/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
	/// which can be found in the file CPL.TXT at the root of this distribution.
	/// By using this software in any fashion, you are agreeing to be bound by 
	/// the terms of this license.
	///
	/// You must not remove this notice, or any other, from this software.
	/// 
	/// This is only used to avoid having to modify Web.config for mono
	/// </summary>
	public class MonoPagesConfigSection : ISectionSettingsMapper
	{
		public object MapSection (object section, List <SettingsMappingWhat> whats)
		{
			if(!(section is PagesSection))
				return section;
			
			PagesSection pageConfig = section as PagesSection;
			if (pageConfig == null)
				return section;
			
			List <SettingsMappingWhatContents> contents;
			string val;
			
			foreach (SettingsMappingWhat what in whats) {
				contents = what.Contents;
				if (contents == null || contents.Count == 0)
					continue;

				val = what.Value;
				foreach (SettingsMappingWhatContents item in contents) {
					switch (item.Operation) {
					case SettingsMappingWhatOperation.Add:
						ProcessAdd (val, pageConfig, item);
						break;
						
					case SettingsMappingWhatOperation.Clear:
						ProcessClear (val, pageConfig, item);
						break;
					}
				}
			}
			
			return pageConfig;
		}
		
		void ProcessAdd (string val, PagesSection section, SettingsMappingWhatContents how)
		{
			if (val == "controls")
				AddControl (section, how);
		}
		
		void AddControl (PagesSection section, SettingsMappingWhatContents how)
		{
			Dictionary <string, string> attrs = how.Attributes;
			if (attrs == null || attrs.Count == 0)
				return;
			
			string tagPrefix, nameSpace, asm, tagName, source;
			if (!attrs.TryGetValue ("tagPrefix", out tagPrefix))
				tagPrefix = String.Empty;
			if (!attrs.TryGetValue ("namespace", out nameSpace))
				nameSpace = String.Empty;
			if (!attrs.TryGetValue ("assembly", out asm))
				asm = String.Empty;
			if (!attrs.TryGetValue ("tagName", out tagName))
				tagName = String.Empty;
			if (!attrs.TryGetValue ("src", out source))
				source = String.Empty;
			
			TagPrefixInfo info = new TagPrefixInfo (tagPrefix, nameSpace, asm, tagName, source);
			section.Controls.Add (info);
		}
		
		void ProcessClear (string val, PagesSection section, SettingsMappingWhatContents how)
		{
			if (val == "controls")
				section.Controls.Clear ();
			else if (val == "tagMapping")
				section.TagMapping.Clear ();
		}
		
		private bool IsIgnoredControlNamespace(string nameSpace)
		{
			bool result = false;
			
			if(nameSpace == "Microsoft.Web.Preview.UI")return true;
			
			return result;
			
		}
		
		private bool IsIgnoredTagType(string tagType)
		{
			bool result = false;
			
			if(tagType == "System.Web.UI.WebControls.WebParts.WebPartManager")return true;
			if(tagType == "System.Web.UI.WebControls.WebParts.WebPartZone")return true;
			
			return result;
			
		}
 
	}
}

#endif
