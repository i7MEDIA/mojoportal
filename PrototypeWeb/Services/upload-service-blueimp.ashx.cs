

using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using log4net;

namespace PrototypeWeb.Services
{
    /// <summary>
    /// Summary description for upload_service_blueimp
    /// </summary>
    public class upload_service_blueimp : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(upload_service_blueimp));

        private HttpRequest Request;
        private HttpResponse Response;
        private HttpServerUtility Server;
        private string foo = string.Empty;
        private bool useSub = false;

        public void ProcessRequest(HttpContext context)
        {
            Request = context.Request;
            Response = context.Response;
            Server = context.Server;

            // just to prove that other form elements are also posted
            if (Request.Params["hdnFoo"] != null)
            {
                foo = Request.Params["hdnFoo"];
                log.Info("parameter hdnFoo was " + foo);
            }

            // the 2nd instance on the upload-blueimp.aspx page passes this 
            // param in the url
            if (Request.Params["sub"] != null)
            {
                if (Request.Params["sub"] == "1") { useSub = true; }
            }
            

            context.Response.ContentType = "text/plain";//"application/json";
            var r = new System.Collections.Generic.List<ViewDataUploadFilesResult>();
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (Request.Files.Count > 0)
            {
                for (int f = 0; f < Request.Files.Count; f++ )
                {
                    HttpPostedFile file = Request.Files[f];

                    string fileName = string.Empty;
                   
                    fileName = Path.GetFileName(file.FileName);

                    if (useSub)
                    {
                        file.SaveAs(Server.MapPath("~/Files/Sub/" + fileName));
                    }
                    else
                    {
                        file.SaveAs(Server.MapPath("~/Files/" + fileName));
                    }

                    r.Add(new ViewDataUploadFilesResult()
                    {
                        //Thumbnail_url = savedFileName,
                        Name = fileName,
                        Length = file.ContentLength,
                        Type = file.ContentType
                    });

                }

                var uploadedFiles = new
                {
                    files = r.ToArray()
                };
                var jsonObj = js.Serialize(uploadedFiles);
                //jsonObj.ContentEncoding = System.Text.Encoding.UTF8;
                //jsonObj.ContentType = "application/json;";
                context.Response.Write(jsonObj.ToString());

            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    // example of expected json result
    // https://github.com/blueimp/jQuery-File-Upload/wiki/Setup

    public class ViewDataUploadFilesResult
    {
        public string Thumbnail_url { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
    }
}