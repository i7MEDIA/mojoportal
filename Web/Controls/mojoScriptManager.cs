using System;
using System.Linq;
using System.Web.UI;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// http://stackoverflow.com/questions/12065228/asp-net-4-5-web-forms-unobtrusive-validation-jquery-issue/12628170#12628170
    /// see where we add an empty jquery script reference in global.asax.cs
    /// this is to remove that
    /// we already render our own jquery in the head
    /// </summary>
    public class mojoScriptManager : ScriptManager
    {
        protected override void OnInit(EventArgs e)
        {
            Page.PreRenderComplete += Page_PreRenderComplete;
            base.OnInit(e);
        }

        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            var jqueryReferences = Scripts.Where(s => s.Name.Equals("jquery", StringComparison.OrdinalIgnoreCase)).ToList();
            if (jqueryReferences.Count > 0)
            {
                // Remove the jquery references as we're rendering it manually in the master page <head>
                foreach (var reference in jqueryReferences)
                {
                    Scripts.Remove(reference);
                }
            }
        }
    }
}