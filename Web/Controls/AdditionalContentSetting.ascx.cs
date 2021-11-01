using System;
using System.Collections.Generic;
using System.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Models;
using mojoPortal.Web.UI;
using Newtonsoft.Json;
namespace mojoPortal.Web.Controls
{
	public partial class AdditionalContentSetting : UserControl, ICustomField
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(AdditionalContentSetting));

		protected void Page_Load(object sender, EventArgs e)
		{
			AdditionalContentList model = new AdditionalContentList
			{
				GlobalContent = Module.GetGlobalContent(CacheHelper.GetCurrentSiteSettings().SiteId),
				ChosenContent = JsonConvert.DeserializeObject(hdnChosenModules.Value) as Dictionary<string, int>,
				LocationOptions = new Dictionary<string, int>
				{
					{ "AbovePosts", 0 },
					{ "BelowPosts", 1 },
					{ "AboveNav", 2 },
					{ "BelowNav", 3 }
				}
			};

			try
			{
				litGlobalContentList.Text = RazorBridge.RenderPartialToString("_AdditionalContentList", model, "Admin");
			}
			catch (System.Web.HttpException ex)
			{
				log.Error($"layout (_AdditionalContentList) was not found in skin {SiteUtils.GetSkinBaseUrl(true, Page)}. perhaps it is in a different skin. Error was: {ex}");
			}
		}

		#region ICustomField

		public string GetValue()
		{
			// send the value from the control back to mojo
			return hdnChosenModules.Value;
		}

		public void SetValue(string val)
		{
			// set the control to the value from the database 
			hdnChosenModules.Value = val;
		}

		public void Attributes(IDictionary<string, string> attribs)
		{

			//foreach (KeyValuePair<string, string> pair in attribs)
			//{
			//	if (pair.Key == "type")
			//	{
			//		//type = pair.Value;
			//		browse.BrowserType = pair.Value;
			//		if (pair.Value == "folder")
			//		{
			//			browse.Editor = "folderpicker";
			//		}
			//	}
			//	else if (pair.Key == "startFolder")
			//	{
			//		browse.StartFolder = pair.Value;
			//	}
			//	else if (pair.Key == "returnFullPath" && pair.Value.ToLower() == "false")
			//	{
			//		browse.ReturnFullPath = false;
			//	}
			//	else
			//	{
			//		txtPath.Attributes.Add(pair.Key, pair.Value);
			//	}
			//}
		}
		#endregion

	}

	public class AdditionalContentList
	{
		public List<Module.GlobalContent> GlobalContent;
		public Dictionary<string, int> ChosenContent;
		public Dictionary<string, int> LocationOptions;
	}
}