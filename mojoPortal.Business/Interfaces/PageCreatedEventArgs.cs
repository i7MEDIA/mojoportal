using System;
using System.Collections.Generic;
using System.Text;

namespace mojoPortal.Business
{
    public delegate void PageCreatedEventHandler(object sender, PageCreatedEventArgs e);

    public class PageCreatedEventArgs : EventArgs
    {

    }
}
