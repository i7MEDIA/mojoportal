/*

NeatHtml- Fighting XSS with JavaScript Judo
Copyright (C) 2007  Dean Brettle

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.ComponentModel;
using System.IO;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Brettle.Web.NeatHtml;

/// <summary>
/// Renders it's content using NeatHtml.js to fight XSS and other attacks.
/// </summary>
/// <remarks>
/// Tables that are not at the top-level of the content may cause the content to not display properly
/// for users without javascript.
/// </remarks>
//[AspNetHostingPermissionAttribute (SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
[ParseChildren(false)]
[PersistChildren(true)]
//#if NET_2_0
//	[Designer(typeof(System.Web.UI.Design.ContainerControlDesigner))]
//#else
//	[Designer(typeof(System.Web.UI.Design.ReadWriteControlDesigner))]
//#endif
public class UntrustedContent : Control
{
	/// <summary>
	/// The name of the filter in client-side script.
	/// </summary>
	/// <remarks>
	/// You can create and configure multiple filters in client-side script.
	/// </remarks>
	[DefaultValue("NeatHtml.DefaultFilter")]
	public string ClientSideFilterName
	{
		get => ViewState["ClientSideFilterName"] != null ? (string)ViewState["ClientSideFilterName"] : "NeatHtml.DefaultFilter";
		set => ViewState["ClientSideFilterName"] = value;
	}

	/// <summary>
	/// Enables/disables support for displaying TABLE elements to users that have JavaScript disabled.
	/// </summary>
	/// <remarks>
	/// Due to the complexity of supporting tables for no-script users, enabling this support could be a
	/// security risk.  Only set this to true if you need well-formed tables in the untrusted content to
	/// be displayed properly to no-script users.
	/// </remarks>
	[DefaultValue(false)]
	public bool SupportNoScriptTables
	{
		get => ViewState["SupportNoScriptTables"] != null && (bool)ViewState["SupportNoScriptTables"];
		set => ViewState["SupportNoScriptTables"] = value;
	}

	/// <summary>
	/// The maximum number of "<" characters (including in tags), "&" characters (including entities), 
	/// attributes, and style properties, combined, that are allowed in the untrusted content.
	/// </summary>
	/// <remarks>
	/// This limits the effectiveness of Denial of Service attacks that use pathological untrusted content
	/// to increase processing time.  If you expect benign content to contain many entities,
	/// or complex markup, you might want to increase this value.  If you expect benign content to contain 
	/// relatively few entities and little markup, you might want to decrease this value.
	/// </remarks>
	[DefaultValue(10000)]
	public int MaxComplexity
	{
		get => ViewState["MaxComplexity"] != null ? (int)ViewState["MaxComplexity"] : 10000;
		set => ViewState["MaxComplexity"] = value;
	}

	private readonly bool IsDesignTime = HttpContext.Current == null;

	// This is used to ensure that the browser gets the latest NeatHtml.js each time this assembly is
	// reloaded.  Strictly speaking the browser only needs to get the latest when NeatHtml.js changes,
	// but computing a hash on that file everytime this assembly is loaded strikes me as overkill.
	private static Guid CacheBustingGuid = Guid.NewGuid();

	/// <summary>
	/// URL of NeatHtml.js.</summary>
	/// <remarks>
	/// If the URL starts with "~", the "~" will be replaced with the web application root as returned by
	/// <see cref="HttpRequest.ApplicationPath"/>.  By default, "~/NeatHtml/NeatHtml.js" will be used.</remarks>
	//[Editor(typeof(UrlEditor), typeof(UITypeEditor)), Bindable(true), DefaultValue("~/NeatHtml/NeatHtml.js")]
	public string ClientScriptUrl
	{
		get => (string)ViewState["ClientScriptUrl"];
		set => ViewState["ClientScriptUrl"] = value;
	}

	/// <summary>
	/// The regular expression pattern that a trusted image URL must match.
	/// </summary>
	[DefaultValue("^$")]
	public string TrustedImageUrlPattern
	{
		get => (string)ViewState["TrustedImageUrlPattern"];
		set
		{
			ViewState["TrustedImageUrlPattern"] = value;
			_TrustedImageUrlRegex = new Regex(value);
		}
	}

	private Regex _TrustedImageUrlRegex = null;
	private Regex TrustedImageUrlRegex
	{
		get
		{
			if (_TrustedImageUrlRegex == null && TrustedImageUrlPattern != null)
			{
				_TrustedImageUrlRegex = new Regex(TrustedImageUrlPattern);
			}

			return _TrustedImageUrlRegex;
		}
	}

	/// <summary>
	/// The regular expression pattern that a link URL must match in order for it to void being marked with
	/// rel="nofollow"
	/// </summary>
	[DefaultValue("^$")]
	public string SpamFreeLinkUrlPattern
	{
		get => (string)ViewState["SpamFreeLinkUrlPattern"];
		set
		{
			ViewState["SpamFreeLinkUrlPattern"] = value;
			_SpamFreeLinkUrlRegex = new Regex(value);
		}
	}

	private Regex _SpamFreeLinkUrlRegex = null;
	private Regex SpamFreeLinkUrlRegex
	{
		get
		{
			if (_SpamFreeLinkUrlRegex == null && SpamFreeLinkUrlPattern != null)
			{
				_SpamFreeLinkUrlRegex = new Regex(SpamFreeLinkUrlPattern);
			}

			return _SpamFreeLinkUrlRegex;
		}
	}


	protected override void OnPreRender(EventArgs e)
	{
		if (!IsDesignTime)
		{
			ClientScriptUrl ??= "~/NeatHtml/NeatHtml.js";

			if (!Page.ClientScript.IsClientScriptBlockRegistered("NeatHtmlJs"))
			{
				Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "NeatHtmlJs", $"<script data-loader=\"NeatHtmlUntrustedContentControl\" src=\"{Helpers.ApplyAppPathModifier(ClientScriptUrl)}?guid={CacheBustingGuid}\"></script>");
			}
		}
		base.OnPreRender(e);
	}


	protected override void Render(HtmlTextWriter writer)
	{
		// Render the content of this control to a string
		var sw = new StringWriter();
		System.Reflection.ConstructorInfo constructor = writer.GetType().GetConstructor([sw.GetType()]);
		HtmlTextWriter htw = constructor.Invoke([sw]) as HtmlTextWriter;
		base.RenderChildren(htw);
		htw.Close();

		// Filter the string and write the result
		var filter = new Filter
		{
			ClientSideFilterName = ClientSideFilterName,
			SupportNoScriptTables = SupportNoScriptTables,
			MaxComplexity = MaxComplexity,
			TrustedImageUrlRegex = TrustedImageUrlRegex,
			SpamFreeLinkUrlRegex = SpamFreeLinkUrlRegex
		};
		writer.Write(filter.FilterUntrusted(sw.ToString()));
	}
}
