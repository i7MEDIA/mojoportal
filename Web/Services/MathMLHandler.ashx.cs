/// Author:					
/// Created:				2007-12-04
/// Last Modified:			2007-12-04
///		
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace mojoPortal.Web.Services
{
    /// <summary>
    /// This will be a handler for MathML .mml files.
    /// The plan is to support Amaya editor using http put
    /// 
    /// This is not yet implemented just some initial scaffolding
    /// 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MathMLHandler : IHttpHandler
    {
        const string GetMethod = "GET";
        const string PostMethod = "POST";
        const string PutMethod = "PUT";

        // 200 = ok
        // 201 = created
        // 204 = no content
        // 501 = not implemented


        // problem by default IIS 7 doesn't allow put, haven't tested IIS6

        public void ProcessRequest(HttpContext context)
        {
            

            switch(context.Request.HttpMethod)
            {
                

                case PostMethod:
                    HandlePost(context);
                    break;

                case GetMethod:
                
                    HandleGet(context);
                    break;

                case PutMethod:
                default:
                    HandlePut(context);
                    break;

            }
        }

        private void HandleGet(HttpContext context)
        {
            context.Response.ContentType = "application/mathml+xml";
            //context.Response.Write("Hello World");

            string downloadPath = context.Server.MapPath("~/Data/Sites/1/mathml/math3.mml");
                    
            if (File.Exists(downloadPath))
            {
                //FileInfo fileInfo = new System.IO.FileInfo(downloadPath);
                //Page.Response.AppendHeader("Content-Length", fileInfo.Length.ToString(CultureInfo.InvariantCulture));
                context.Response.Buffer = false;
                context.Response.BufferOutput = false;
                context.Response.TransmitFile(downloadPath);
                context.Response.End();
            }


        }

        private void HandlePut(HttpContext context)
        {

            string downloadPath = context.Server.MapPath("~/Data/Sites/1/mathml/math3.mml");
            if (File.Exists(downloadPath))
            {
                File.Delete(downloadPath);

                Stream requestStream = context.Request.InputStream;
                StreamReader rdr = new StreamReader(requestStream);

                StreamWriter replacementFile = File.CreateText(downloadPath);
                replacementFile.Write(rdr.ReadToEnd());
                replacementFile.Close();

                context.Response.StatusCode = 200;


            }

        }

        private void HandlePost(HttpContext context)
        {

            HandlePut(context);

        }


        public void ProcessRequestExample(HttpContext context)
        {
            // Access the request stream and wrap in a StreamReader
            Stream requestStream = context.Request.InputStream;
            StreamReader rdr = new StreamReader(requestStream);

            int linesRead = 0;
            // Read data from the client line-by-line
            string inLine = rdr.ReadLine();
            while (inLine != null)
            {
                // perform line processing
                linesRead += 1;
                inLine = rdr.ReadLine();
            }

            // Attach to the response stream and wrap in a StreamWriter
            Stream responseStream = context.Response.OutputStream;
            StreamWriter wrtr = new StreamWriter(responseStream);

            // Write the response message
            wrtr.WriteLine("LinesRead=");
            wrtr.WriteLine(linesRead.ToString());
            wrtr.WriteLine("Processing Successful");

            wrtr.Flush();
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
