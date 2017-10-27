// Author:             
// Created:            2010-08-19
// Last Modified:      2017-10-04

using System.Collections;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;
using mojoPortal.Business;
using System.Collections.Generic;
namespace mojoPortal.Web
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class mojoThemeVirtualDirectory : VirtualDirectory
    {
        
        VirtualDirectory _requestedDirectory;
        private ArrayList _children = new ArrayList();
        private ArrayList _directories = new ArrayList();
        private ArrayList _files = new ArrayList();

       
        public override IEnumerable Children 
        {
          get { return _children; }
        }
 
        public override IEnumerable Directories 
        {
          get { return _directories; }
        }
 
        public override IEnumerable Files 
        {
          get { return _files; }
        }

        public mojoThemeVirtualDirectory(VirtualDirectory requestedDirectory)
            : base(requestedDirectory.VirtualPath) 
        {
            _requestedDirectory = requestedDirectory;
            
            BuildChildren();
        }

        private void BuildChildren() 
        {
            // fool it into thinking the theme.skin file exists in the requested path
            mojoThemeVirtualFile themeFile = new mojoThemeVirtualFile(_requestedDirectory.VirtualPath + "theme.skin");

			List<string> moduleSkinFileNames = ModuleDefinition.GetAllModuleSkinFileNames();

            foreach (VirtualFile f in _requestedDirectory.Files)
            {
                string fileName = VirtualPathUtility.GetFileName(f.VirtualPath);
                if (fileName == "theme.skin.css" || moduleSkinFileNames.Contains(fileName))
                {
                    _files.Add(f);
                    _children.Add(f);
                }
            }

            _files.Add(themeFile);
            _children.Add(themeFile);
        }

      }

    
}