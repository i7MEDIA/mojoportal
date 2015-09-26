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
	/// Author:				Marek Habersack
	/// Created:			2008-02-27
	/// Last Modified:		2008-02-27
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
	
	public class MonoCompilationSection : ISectionSettingsMapper
	{
		public object MapSection (object section, List <SettingsMappingWhat> whats)
		{
			if(!(section is CompilationSection))
				return section;
			
			CompilationSection compilationConfig = section as CompilationSection;
			if (compilationConfig == null)
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
					case SettingsMappingWhatOperation.Remove:
						ProcessRemove (val, compilationConfig, item);
						break;
					}
				}
			}
			
			return compilationConfig;
		}
		
		void ProcessRemove (string val, CompilationSection section, SettingsMappingWhatContents how)
		{
			if (val == "assemblies")
				RemoveAssembly (section, how);
		}
		
		void RemoveAssembly (CompilationSection section, SettingsMappingWhatContents how)
		{
			Dictionary <string, string> attrs = how.Attributes;
			if (attrs == null || attrs.Count == 0)
				return;
			
			string assemblyName;
			if (!attrs.TryGetValue ("assembly", out assemblyName))
				return;
			
			section.Assemblies.Remove (assemblyName);
		}
	}
}
#endif
