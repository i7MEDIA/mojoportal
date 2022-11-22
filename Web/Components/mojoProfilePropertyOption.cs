namespace mojoPortal.Web.Configuration
{
	public class mojoProfilePropertyOption
	{
		private string textResourceKey = string.Empty;
		private string optionValue = string.Empty;


		public mojoProfilePropertyOption()
		{ }


		public string TextResourceKey
		{
			get { return textResourceKey; }
			set { textResourceKey = value; }
		}


		public string Value
		{
			get { return optionValue; }
			set { optionValue = value; }
		}
	}
}
