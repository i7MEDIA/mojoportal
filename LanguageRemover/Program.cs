

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;

namespace LanguageRemover
{
    /// <summary>
    /// The purpose of this console app is to improve performance when working with mojoPortal in Visual Studio 
    /// by removing the extra languages .resx files from the Web/App_GlobalResources folder.
    /// each of those extra languages adds to the time it takes for the ASP.NET compiler to compile the app on
    /// the first web request. Since we now have so many translations it has become a major slow down.
    /// So by building this project after building the other projects in the solution we can remove all except for the
    /// English resource files.
    /// 
    /// DO NOT RUN this manually ie using the debugger, it will just throw errors
    /// To run it just right click it and choose build. A post build event will execute it and pass in the file system path
    /// to Web/App_GlobalResources
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string appGlobaleResourcesFolderPath = args[0];

            Console.WriteLine("App_GlobalResources Path: " + appGlobaleResourcesFolderPath);

            string languagesNotToRemove = GetConfigSetting("LanguagesNotToRemove");

            Console.WriteLine("Languages to keep: " + languagesNotToRemove);

            string[] langsToKeep = languagesNotToRemove.Split(',');

            string[] files = Directory.GetFiles(appGlobaleResourcesFolderPath);

            //English files have one dot ie Resource.resx
            // other languages have 2 dots ie Resource.ru.resx

            foreach (string file in files)
            {
                if (!ShouldKeep(langsToKeep, file)) { File.Delete(file); }
                Console.WriteLine("Deleted file: " + file);
            }

        }

        private static bool ShouldKeep(string[] langsToKeep, string fileName)
        {
            if (fileName.IndexOf('.') == fileName.LastIndexOf('.')) { return true; }

            // these are under source control in the /Web/App_GlobalResources folder so probably should not delete them 
            // otherwise it may cause problems on commit

            string diskFileName = Path.GetFileName(fileName);
            // one dot means it is an English file
            if (diskFileName.IndexOf('.') == diskFileName.LastIndexOf('.')) { return true; }

            if (diskFileName.StartsWith("Resource.")) { return true; }

            if (fileName.IndexOf(".cs") > -1) { return true; } //leave source code

            if (!RemoveCoreResources())
            {
                if (fileName.IndexOf("AccessKeys") > -1) { return true; }
                if (fileName.IndexOf("CountryISOCode2Resources") > -1) { return true; }
                if (fileName.IndexOf("DevTools") > -1) { return true; }
                if (fileName.IndexOf("ProfileResource") > -1) { return true; }
                if (fileName.IndexOf("SetupResource") > -1) { return true; }
                if (fileName.IndexOf("TimeZoneResources") > -1) { return true; }
            }

            foreach (string lang in langsToKeep)
            {
                string marker = "." + lang.Trim() + ".";
                if (fileName.IndexOf(marker) > -1) { return true; }
            }

            return false;
        }

        private static bool RemoveCoreResources()
        {
            return Convert.ToBoolean(GetConfigSetting("RemoveCoreFiles"));
        }

        private static string GetConfigSetting(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigurationManager.AppSettings[key];
            }

            return string.Empty;

        }
    }
}
