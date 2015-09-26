using System;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace mojoPortal.Web
{
    public class GCheckoutBasicAuthenticationModule : IHttpModule
    {
        private string _userName = string.Empty;

        /// <summary>
        /// The UserName performing the request
        /// </summary>
        protected string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                if (value != null)
                    _userName = value;
            }
        }

        private string _password = string.Empty;

        /// <summary>
        /// The Password of the User performing the call.
        /// </summary>
        protected string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (value != null)
                    _password = value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BasicAuthenticationModule"/> 
        /// class. This is done by IIS.
        /// </summary>
        public GCheckoutBasicAuthenticationModule()
        {
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the
        /// module that implements <see langword="IHttpModule."/>
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Inits the specified application. This will be called by the system.
        /// </summary>
        /// <param name="Application">The HTTP application.</param>
        public void Init(HttpApplication Application)
        {
            Application.AuthenticateRequest +=
              new EventHandler(this.OnAuthenticateRequest);
            Application.EndRequest += new EventHandler(this.OnEndRequest);
        }

        /// <summary>
        /// Called when IIS asks to authenticate an HTTP request.
        /// </summary>
        /// <param name="source">The calling HttpApplication.</param>
        /// <param name="eventArgs">
        /// The <see cref="System.EventArgs"/> instance 
        /// containing the event data.
        /// </param>
        public void OnAuthenticateRequest(object source, EventArgs eventArgs)
        {
            
            HttpApplication App = (HttpApplication)source;

            if (!App.Request.Path.Contains("GCheckoutNotificationHandler.ashx")) return;

            string AuthHeader = App.Request.Headers["Authorization"];
            UserName = GetUserName(AuthHeader);
            Password = GetPassword(AuthHeader);
            if (UserHasAccess(UserName, Password))
            {
                App.Context.User = new GenericPrincipal(
                  new GenericIdentity(UserName, "Google.Checkout.Basic"),
                  new string[1] { "User" });
            }
            else
            {
                App.Response.StatusCode = 401;
                App.Response.StatusDescription = "Access Denied";
                App.Response.Write("401 Access Denied");
                App.CompleteRequest();
            }
        }

        /// <summary>
        /// Called by the system when the HTTP request ends.
        /// </summary>
        /// <param name="source">The calling HttpApplication.</param>
        /// <param name="eventArgs">
        /// The <see cref="System.EventArgs"/> instance 
        /// containing the event data.
        /// </param>
        public void OnEndRequest(object source, EventArgs eventArgs)
        {
            HttpApplication app = (HttpApplication)source;

            if (!app.Request.Path.Contains("GCheckoutNotificationHandler.ashx")) return;

            if (app.Response.StatusCode == 401)
            {
                app.Response.AppendHeader(
                  "WWW-Authenticate", "Basic Realm=\"CheckoutCallbackRealm\"");
            }
        }

        /// <summary>
        /// Gets the name of the user from the "Authorization" HTTP header. That header has
        /// user name and password (as typed by the user) in a Base64-encoded form.
        /// </summary>
        /// <param name="AuthHeader">
        /// The value of the "Authorization" HTTP header.
        /// </param>
        /// <returns>The name of the user as typed by the user in the browser.</returns>
        public static string GetUserName(string AuthHeader)
        {
            return GetDecodedAndSplitAuthorizatonHeader(AuthHeader)[0];
        }

        /// <summary>
        /// Gets the password from the "Authorization" HTTP header. That header has
        /// user name and password (as typed by the user) in a Base64-encoded form.
        /// </summary>
        /// <param name="AuthHeader">
        /// The value of the "Authorization" HTTP header.
        /// </param>
        /// <returns>The password as typed by the user in the browser.</returns>
        public static string GetPassword(string AuthHeader)
        {
            return GetDecodedAndSplitAuthorizatonHeader(AuthHeader)[1];
        }

        private static string[] GetDecodedAndSplitAuthorizatonHeader(
          string AuthHeader)
        {
            string[] RetVal = new string[2] { string.Empty, string.Empty };
            if (AuthHeader != null && AuthHeader.StartsWith("Basic "))
            {
                try
                {
                    string EncodedString = AuthHeader.Substring(6);
                    byte[] DecodedBytes = Convert.FromBase64String(EncodedString);
                    string DecodedString = new ASCIIEncoding().GetString(DecodedBytes);
                    RetVal = DecodedString.Split(new char[] { ':' });
                }
                catch
                {
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Verify if the user has access to perform the callback
        /// </summary>
        /// <param name="UserName">The UserName making the call.</param>
        /// <param name="Password">The Password of the user.</param>
        /// <returns>True or false if the user is able to perform the callback.</returns>
        protected virtual bool UserHasAccess(string UserName, string Password)
        {
            
            CommerceConfiguration commerceConfig = SiteUtils.GetCommerceConfig();

            return (UserName == commerceConfig.GoogleMerchantID && Password == commerceConfig.GoogleMerchantKey);

            //return true;
        }
    }
}
