using System;
using System.Collections.Generic;
using System.Text;

namespace mojoPortal.Business
{
    

    public interface IIndexableContent
    {

        event ContentChangedEventHandler ContentChanged;
        

    }
}
