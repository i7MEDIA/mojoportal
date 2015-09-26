using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;


namespace mojoPortal.Web.Framework
{
    public class CombineScripts
    {
        private static Regex _FindScriptTags = new Regex(@"<script\s*src\s*=\s*""(?<url>.[^""]+)"".[^>]*>\s*</script>", RegexOptions.Compiled);
        //private static readonly string SCRIPT_VERSION_NO = ConfigurationManager.AppSettings["ScriptVersionNo"];


        /// <summary>
        /// Combine script references using file sets defined in a configuration file.
        /// It will replace multiple script references using one 
        /// </summary>
        public static string CombineScriptBlocks(string scripts)
        {
            List<UrlMapSet> sets = LoadSets();
            string output = scripts;

            foreach (UrlMapSet mapSet in sets)
            {
                int setStartPos = -1;
                List<string> names = new List<string>();

                output = _FindScriptTags.Replace(output, new MatchEvaluator(delegate(Match match)
                {
                    string url = match.Groups["url"].Value;

                    UrlMap urlMatch = mapSet.Urls.Find(
                        new Predicate<UrlMap>(
                            delegate(UrlMap map)
                            {
                                return map.Url == url;
                            }));

                    if (null != urlMatch)
                    {
                        // Remember the first script tag that matched in this UrlMapSet because
                        // this is where the combined script tag will be inserted
                        if (setStartPos < 0) setStartPos = match.Index;

                        names.Add(urlMatch.Name);
                        return string.Empty;
                    }
                    else
                    {
                        return match.Value;
                    }

                }));

                if (setStartPos >= 0)
                {
                    string setName = string.Empty;
                    // if the set says always include all urls within it whenever a single match is found,
                    // then generate the full set
                    if (mapSet.IsIncludeAll)
                    {
                        // No need send the individual url names when the full set needs to be included
                        setName = string.Empty;
                    }
                    else
                    {
                        names.Sort();
                        setName = string.Join(",", names.ToArray());
                    }

                    string urlPrefix = HttpContext.Current.Request.Path.Substring(0, HttpContext.Current.Request.Path.LastIndexOf('/') + 1);
                    //string newScriptTag = "<script type=\"text/javascript\" src=\"scripthandler.ashx?" 
                    //    + HttpUtility.UrlEncode(mapSet.Name) 
                    //    + "=" + HttpUtility.UrlEncode(setName) 
                    //    + "&" + HttpUtility.UrlEncode(urlPrefix) 
                    //    + "&" + HttpUtility.UrlEncode(SCRIPT_VERSION_NO) + "\"></script>";

                    string newScriptTag = "<script type=\"text/javascript\" src=\"scripthandler.ashx?"
                        + HttpUtility.UrlEncode(mapSet.Name)
                        + "=" + HttpUtility.UrlEncode(setName)
                        + "&amp;" + HttpUtility.UrlEncode(urlPrefix)
                        + "\"></script>";

                    output = output.Insert(setStartPos, newScriptTag);
                }
            }

            return output;
        }

        public static List<UrlMapSet> LoadSets()
        {
            List<UrlMapSet> sets = new List<UrlMapSet>();

            string fileName = "ScriptFileSets.xml";
            if (ConfigurationManager.AppSettings["NameOfScriptConfigFileInApp_DataFolder"] != null)
            {
                fileName = ConfigurationManager.AppSettings["NameOfScriptConfigFileInApp_DataFolder"];
            }

            string fullPathToFile = HttpContext.Current.Server.MapPath("~/App_Data/ScriptFileSets.xml");
            if (!File.Exists(fullPathToFile)) { return sets; }

            using (XmlReader reader = new XmlTextReader(new StreamReader(fullPathToFile)))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if ("set" == reader.Name)
                    {
                        string setName = reader.GetAttribute("name");
                        string isIncludeAll = reader.GetAttribute("includeAll");

                        UrlMapSet mapSet = new UrlMapSet();
                        mapSet.Name = setName;
                        if (isIncludeAll == "true")
                            mapSet.IsIncludeAll = true;

                        while (reader.Read())
                        {
                            if ("url" == reader.Name)
                            {
                                string urlName = reader.GetAttribute("name");
                                string url = reader.ReadElementContentAsString();
                                mapSet.Urls.Add(new UrlMap(urlName, url));
                            }
                            else if ("set" == reader.Name)
                                break;
                        }

                        sets.Add(mapSet);
                    }
                }
            }

            return sets;
        }
    }

    public class UrlMapSet
    {
        public string Name;
        public bool IsIncludeAll;
        public List<UrlMap> Urls = new List<UrlMap>();
    }

    public class UrlMap
    {
        public string Name;
        public string Url;

        public UrlMap(string name, string url)
        {
            this.Name = name;
            this.Url = url;
        }
    }
}
