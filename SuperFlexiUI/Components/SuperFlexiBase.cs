using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperFlexiUI
{
    public interface ISuperFlexiBase
    {
        Guid DefGuid { get; }
        String Name { get; }
        
        int SortOrder { get; set; }
        


    }

    public abstract partial class SuperFlexiField
    {
        public String Name { get; set; }
        public String Label { get; set; }
        public bool Required { get; set; }
        public int SortOrder { get; set; }
    }
}