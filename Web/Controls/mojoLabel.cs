using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// this extends ASP.NET Label and changes the behavior so that if the text property is empty the control does not render.
    /// The ASP.NET Label will render an empty span but we don't always want that
    /// </summary>
    public class mojoLabel : Label
    {
		public string Format { get; set; } = string.Empty;
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (HttpContext.Current == null)
			{
				writer.Write("[" + this.ID + "]");
				return;
			}

			if (Text.Length > 0)
			{
				if (string.IsNullOrWhiteSpace(Format))
				{
					base.Render(writer);
				}
				else
				{
					writer.Write(string.Format(Format, Text));
				}
			}
		}
    }

    /// <summary>
    /// a hidden field that supports validation
    /// http://stackoverflow.com/questions/6607984/asprequiredfieldvalidator-does-not-validate-hidden-fields
    /// </summary>
    [ValidationProperty("Value")]
    public class mojoHiddenField : HiddenField
    {

    }
}
