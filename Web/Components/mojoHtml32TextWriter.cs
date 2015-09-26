using System;
using System.IO;
using System.Web.UI;

namespace mojoPortal.Web
{
    class MojoHtml32TextWriter : Html32TextWriter
    {
        private string formAction;
        private const string actionAttribute = "action";

        public MojoHtml32TextWriter(TextWriter writer)
            : base(writer)
        {
            
        }

        public MojoHtml32TextWriter(TextWriter writer, string action) : base(writer)
		{
			formAction = action;
		}

		public override void RenderBeginTag(string tagName)
		{
			base.RenderBeginTag(tagName);
		}

		public override void WriteAttribute(string name, string value, bool fEncode)
		{

            if (string.Equals(name, actionAttribute))
            {
                base.WriteAttribute(name, formAction, fEncode);
            }
            else
            {
                base.WriteAttribute(name, value, fEncode);
            }

			//if (string.Compare(name, "action", true) == 0)
            //if (string.Equals(name, "action", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    value = formAction;
            //}
            //base.WriteAttribute (name, value, fEncode);
		}

    }
}
