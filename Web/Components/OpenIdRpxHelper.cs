// Created:       2009-05-14
// Last Modified: 2018-09-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using log4net;
using mojoPortal.Business;

namespace mojoPortal.Web
{
	/// <summary>
	/// Helper methods based on https://rpxnow.com/examples/Rpx.cs
	/// https://rpxnow.com/docs
	/// </summary>
	public class OpenIdRpxHelper
	{
		private string apiKey = string.Empty;
		private string baseUrl = "https://rpxnow.com";
		private readonly ILog log = LogManager.GetLogger(typeof(OpenIdRpxHelper));

		#region Constructors

		public OpenIdRpxHelper(string apiKey)
		{
			if (string.IsNullOrEmpty(apiKey)) { throw new ArgumentException("apiKey is null or empty"); }

			this.apiKey = apiKey;
		}

		public OpenIdRpxHelper(string apiKey, string baseUrl)
		{
			if (string.IsNullOrEmpty(apiKey)) { throw new ArgumentException("apiKey is null or empty"); }
			if (string.IsNullOrEmpty(baseUrl)) { throw new ArgumentException("baseUrl is null or empty"); }

			while (baseUrl.EndsWith("/"))
				baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);

			this.apiKey = apiKey;
			this.baseUrl = baseUrl;
		}

		#endregion


		public OpenIdRpxAuthInfo AuthInfo(string token, string tokenUrl)
		{
			Dictionary<string, string> query = new Dictionary<string, string>();
			query.Add("token", token);
			query.Add("tokenUrl", tokenUrl);
			query.Add("extended", "true");
			try
			{
				XmlElement authElement = ApiCall("auth_info", query);
				if (authElement != null) { return new OpenIdRpxAuthInfo(authElement); }
			}
			catch (SystemException) { }

			return null;

		}

		public ArrayList GetMappings(string primaryKey)
		{
			Dictionary<string, string> query = new Dictionary<string, string>();
			query.Add("primaryKey", primaryKey);
			XmlElement rsp = ApiCall("mappings", query);
			XmlElement oids = (XmlElement)rsp.FirstChild;

			ArrayList result = new ArrayList();

			for (int i = 0; i < oids.ChildNodes.Count; i++)
			{
				result.Add(oids.ChildNodes[i].InnerText);
			}

			return result;
		}

		public Dictionary<string, ArrayList> GetAllMappings()
		{
			Dictionary<string, string> query = new Dictionary<string, string>();
			XmlElement rsp = ApiCall("all_mappings", query);

			Dictionary<string, ArrayList> result = new Dictionary<string, ArrayList>();
			XPathNavigator nav = rsp.CreateNavigator();

			XPathNodeIterator mappings = (XPathNodeIterator)nav.Evaluate("/rsp/mappings/mapping");
			foreach (XPathNavigator m in mappings)
			{
				string remote_key = GetContents("./primaryKey/text()", m);
				XPathNodeIterator ident_nodes = (XPathNodeIterator)m.Evaluate("./identifiers/identifier");
				ArrayList identifiers = new ArrayList();
				foreach (XPathNavigator i in ident_nodes)
				{
					identifiers.Add(i.ToString());
				}

				result.Add(remote_key, identifiers);
			}

			return result;
		}

		private string GetContents(string xpath_expr, XPathNavigator nav)
		{
			XPathNodeIterator rk_nodes = (XPathNodeIterator)nav.Evaluate(xpath_expr);
			while (rk_nodes.MoveNext())
			{
				return rk_nodes.Current.ToString();
			}
			return null;
		}

		public void Map(string identifier, string primaryKey)
		{
			Dictionary<string, string> query = new Dictionary<string, string>();
			query.Add("identifier", identifier);
			query.Add("primaryKey", primaryKey);
			try
			{
				ApiCall("map", query);
			}
			catch (SystemException) { }
		}

		public void Unmap(string identifier, string primaryKey)
		{
			Dictionary<string, string> query = new Dictionary<string, string>();
			query.Add("identifier", identifier);
			query.Add("primaryKey", primaryKey);
			try
			{
				ApiCall("unmap", query);
			}
			catch (SystemException) { }
		}

		public void UnmapAll(string primaryKey)
		{
			ArrayList allMappings = GetMappings(primaryKey);
			foreach (string identifier in allMappings)
			{
				Dictionary<string, string> query = new Dictionary<string, string>();
				query.Add("identifier", identifier);
				query.Add("primaryKey", primaryKey);
				try
				{
					ApiCall("unmap", query);
				}
				catch (SystemException) { }
			}

		}

		private XmlElement ApiCall(string methodName, Dictionary<string, string> partialQuery)
		{
			Dictionary<string, string> query = new Dictionary<string, string>(partialQuery);
			query.Add("format", "xml");
			query.Add("apiKey", apiKey);

			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, string> e in query)
			{
				if (sb.Length > 0)
				{
					sb.Append('&');
				}

				sb.Append(System.Web.HttpUtility.UrlEncode(e.Key, Encoding.UTF8));
				sb.Append('=');
				sb.Append(HttpUtility.UrlEncode(e.Value, Encoding.UTF8));
			}
			string data = sb.ToString();

			Uri url = new Uri(baseUrl + "/api/v2/" + methodName);

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = data.Length;

			using (StreamWriter stOut = new StreamWriter(request.GetRequestStream(), Encoding.ASCII))
			{
				stOut.Write(data);
			}

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			/*
			 response will never be null
            if (response == null) { return null; }
			*/

			Stream dataStream = response.GetResponseStream();
			var doc = Core.Helpers.XmlHelper.GetXmlDocument(dataStream);

			XmlElement resp = doc.DocumentElement;

			log.Debug($"openid-debug: response from {url.ToString()} is \r\n {resp.Value} ");

			if (!resp.GetAttribute("stat").Equals("ok"))
			{
				throw new SystemException("Unexpected API error");
			}

			return resp;
		}


		public static string GetPluginApiRedirectUrl(HttpContext context, string siteRoot, string returnUrl, string requestId)
		{
			return "https://rpxnow.com/plugin/create_rp?base=" + context.Server.UrlEncode(siteRoot)
				+ "&return=" + context.Server.UrlEncode(returnUrl)
				+ "&requestId=" + context.Server.UrlEncode(requestId) + "&utm_source=mojoportal&utm_medium=partner&utm_campaign=mojoportalreferral";

		}

		public static OpenIdRpxAccountInfo LookupRpxAccount(string tokenOrApiKey, bool isToken)
		{
			OpenIdRpxAccountInfo account = null;
			Dictionary<string, string> query = new Dictionary<string, string>();
			//query.Add("format", "xml");
			if (isToken)
			{
				query.Add("token", tokenOrApiKey);
			}
			else
			{
				query.Add("apiKey", tokenOrApiKey);
			}

			query.Add("demographics", "mojoportal " + DatabaseHelper.DBCodeVersion().ToString());

			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, string> e in query)
			{
				if (sb.Length > 0)
				{
					sb.Append('&');
				}

				sb.Append(System.Web.HttpUtility.UrlEncode(e.Key, Encoding.UTF8));
				sb.Append('=');
				sb.Append(HttpUtility.UrlEncode(e.Value, Encoding.UTF8));
			}
			string data = sb.ToString();

			Uri url = new Uri("https://rpxnow.com/plugin/lookup_rp");

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = data.Length;

			using (StreamWriter stOut = new StreamWriter(request.GetRequestStream(), Encoding.ASCII))
			{
				stOut.Write(data);
			}

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			if (response == null) { return null; }

			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(OpenIdRpxAccountInfo));

			//result is a json dictionary
			using (Stream stream = response.GetResponseStream())
			{
				// deserialize the json into an object
				account = serializer.ReadObject(stream) as OpenIdRpxAccountInfo;
			}

			return account;
		}
	}
}