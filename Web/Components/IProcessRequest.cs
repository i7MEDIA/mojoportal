using System;
using System.Web;

namespace mojoPortal.Web
{
    public interface IProcessRequest
    {
        void ProcessRequest(HttpContext context);
    }
}
