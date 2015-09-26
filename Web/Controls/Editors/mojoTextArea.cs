


using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class mojoTextArea : TextBox, ISettingControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            TextMode = TextBoxMode.MultiLine;
            Rows = 20;
            this.Columns = 80;
        }

        #region ISettingControl

        public string GetValue()
        {
            return Text;
        }

        public void SetValue(string val)
        {
            Text = val;
        }

        #endregion


    }
}