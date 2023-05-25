using System;
using System.Collections.Generic;

namespace mojoPortal.Web.UI
{
    public class mojoMenuItem
    {
        public int PageId { get; set; }
        public string Name { get; set; }
        public string TitleOverride { get; set; }
        //public string Heading { get; set; }
        //public bool ShowHeading { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string CssClass { get; set; }
        public string Rel { get; set; }
        public bool Clickable { get; set; }
        public bool OpenInNewTab { get; set; }
        public int PublishMode { get; set; }
        //public string Skin { get; set; }
        public DateTime LastModDate { get; set; }
        public bool Current { get; set; }
        public List<mojoMenuItem> Children { get; set; } = new();
    }
}