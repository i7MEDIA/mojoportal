
namespace mojoPortal.Web.UI
{
	public class FormGroupPanel : BasePanel
	{
		private string cssClass = "settingrow";
		public override string CssClass
		{
			get { return cssClass; }
			set { cssClass = value; }
		}

		private bool renderId = false;
		public override bool RenderId
		{
			get { return renderId; }
			set { renderId = value; }
		}
	}
}