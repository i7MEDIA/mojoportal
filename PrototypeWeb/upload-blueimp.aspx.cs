using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrototypeWeb
{
    public partial class upload_blueimp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            up1.UploadButtonClientId = btnUpload.ClientID;
            up2.UploadButtonClientId = btnUpload2.ClientID;

            lblTime.Text = DateTime.Now.ToString();

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // this is fall back implementation
            // should only fire if javascript is disabled 
            // with javascript enabled we use the jquery file upload and a service page to receive the file

            if (up1.HasFile)
            {
                string fileName = string.Empty;

                fileName = Path.GetFileName(up1.FileName);

                up1.SaveAs(Server.MapPath("~/Files/" + fileName));


            }
        }

        protected void btnUpload2_Click(object sender, EventArgs e)
        {
            // this is fall back implementation
            // should only fire if javascript is disabled 
            // with javascript enabled we use the jquery file upload and a service page to receive the file

            if (up2.HasFile)
            {
                string fileName = string.Empty;

                fileName = Path.GetFileName(up2.FileName);

                up2.SaveAs(Server.MapPath("~/Files/Sub/" + fileName));


            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
            pnlUpldate.Update();

        }
    }
}