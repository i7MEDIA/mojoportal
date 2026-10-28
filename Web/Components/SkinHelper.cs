using log4net;
using mojoPortal.Business;
using mojoPortal.Web.Controls.Editors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

namespace mojoPortal.Web.Components;

public class SkinHelper
{
	private static readonly ILog log = LogManager.GetLogger(typeof(SkinHelper));


	#region Static Methods

	public static string GetStyleJson(Guid siteGuid)
	{
		var styles = new List<EditorStyle>();
		using IDataReader reader = ContentStyle.GetAllActive(siteGuid);
		while (reader.Read())
		{
			styles.Add(new EditorStyle
			{
				Name = reader["Name"].ToString(),
				Element = [reader["Element"].ToString()],
				Attributes = new Dictionary<string, string>() { { "class", reader["CssClass"].ToString() } }
			});
		}

		return JsonConvert.SerializeObject(styles);
	}


	public static void ImportStyles(Stream stylesXmlStream, Guid siteGuid)
	{
		try
		{
			var stylesXmlDoc = Core.Helpers.XmlHelper.GetXmlDocument(stylesXmlStream);

			if (stylesXmlDoc.DocumentElement.Name != "ContentStyles") { return; }
			foreach (XmlNode node in stylesXmlDoc.DocumentElement.ChildNodes)
			{
				if (node.Name == "Style")
				{
					ContentStyle style = ContentStyle.GetNew(siteGuid);
					SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
					if (currentUser != null) { style.LastModBy = currentUser.UserGuid; }
					style.Name = node.Attributes["Name"].Value;
					style.Element = node.Attributes["Element"].Value;
					style.CssClass = node.Attributes["CssClass"].Value;
					style.IsActive = Convert.ToBoolean(node.Attributes["IsActive"].Value);
					style.Save();
				}
			}
		}
		catch (UnauthorizedAccessException ex)
		{
			log.Error(ex);
		}
		catch (ArgumentException ex)
		{
			log.Error(ex);
		}
	}


	public static bool ImportStyleJson(Stream stream, Guid siteGuid)
	{
		var editorStyles = EditorStyle.GetEditorStyles(stream);
		if (editorStyles == null)
		{
			log.Error("invalid file uploaded for Style import");
			return false;
		}

		bool addedStyle = false;

		foreach (var editorStyle in editorStyles)
		{
			ContentStyle contentStyle = ContentStyle.GetNew(siteGuid);
			SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
			if (currentUser != null) { contentStyle.LastModBy = currentUser.UserGuid; }
			contentStyle.Name = editorStyle.Name;
			contentStyle.Element = string.Join(" ", [.. editorStyle.Element]);

			string cssClasses = string.Empty;
			if (editorStyle.Attributes.ContainsKey("class"))
			{
				contentStyle.CssClass = editorStyle.Attributes["class"];
			}
			contentStyle.IsActive = true;
			if (contentStyle.Save())
			{
				addedStyle = true;
			}
		}
		if (addedStyle)
		{
			Global.SkinConfigManager.ClearAll();
		}
		return addedStyle;
	}

	#endregion
}
