using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web
{
    public class CultureHelperHttpModule : IHttpModule
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CultureHelperHttpModule));


        public void Init(HttpApplication application)
        {
            application.Error += new EventHandler(this.Error);
            application.BeginRequest += new EventHandler(BeginRequest);
            application.EndRequest += new EventHandler(this.EndRequest);
        }


        private void BeginRequest(object sender, EventArgs e)
        {

            HttpApplication app = (HttpApplication)sender;

            if (WebUtils.IsRequestForStaticFile(app.Request.Path)) { return; }
            if (app.Request.Path.ContainsCaseInsensitive("csshandler.ashx")) { return; }
            if (app.Request.Path.ContainsCaseInsensitive("thumbnailservice.ashx")) { return; }
            if (app.Request.Path.ContainsCaseInsensitive("GCheckoutNotificationHandler.ashx")) { return; }

            
            // 2006-12-29 
            // CultureInfo for the executing thread is automatically set to the 
            // preferred culture of the user's browser by this web.config setting:
            // <globalization 
            //   culture="auto:en-US" 
            //   uiCulture="auto:en" 
            //   requestEncoding="utf-8" 
            //   responseEncoding="utf-8" 
            //   fileEncoding="iso-8859-15" />
            //
            // the "auto" tells the runtime to use the browser preference
            // and the :en-US tells the runtime to fall back to en-US as the default culture for 
            // missing resource keys or if no resource file exists for the preferred culture
            // you can specify a different default culture by replacing en-US with your preferred
            // culture, but you should make sure that the resource file for the default culture 
            // has no missing keys or runtime errors could occur

            //by default the culture of the executing thread is set to that of the browser preferred language setting
            // and this causes the use of the language specific resource if available else it falls back to the default en-US
            //as defined in Web.config globalization section
            //below we are using a config setting to change the default behavior to force the thread to use a specific culture
            //however if there are missing keys in the specified culture it can still fall back to en-US as it should
            if (WebConfigSettings.UseCultureOverride)
            {
                CultureInfo siteCulture;
                CultureInfo siteUICulture;
                try
                {
                    siteCulture = SiteUtils.GetDefaultCulture();
                    siteUICulture = SiteUtils.GetDefaultUICulture();
                }
                catch (InvalidOperationException) { return; }
                catch (System.Data.Common.DbException) { return; }

                if (siteCulture.IsNeutralCulture)
                {
                    log.Info("cannot use culture " + siteCulture.Name + " because it is a neutral culture. It cannot be used in formatting and parsing and therefore cannot be set as the thread's current culture.");
                }
                else
                {
                    try
                    {
                        Thread.CurrentThread.CurrentCulture = siteCulture;
                        if (WebConfigSettings.SetUICultureWhenSettingCulture)
                        {
                            Thread.CurrentThread.CurrentUICulture = siteUICulture;
                        }
                        
                    }
                    catch (ArgumentException ex)
                    {
                        log.Info("swallowed error in culture helper", ex);
                    }
                    catch (NotSupportedException ex)
                    {
                        log.Info("swallowed error in culture helper", ex);
                    }
                }

            }


            // below we are overriding only to handle culture specific workarounds, for most cultures
            // we don't need to do anything here as the runtime handles it correctly
            // but there are some cultures which are either poorly or incorrectly implemented
            // in the .NET runtime

            if (WebConfigSettings.UseCustomHandlingForPersianCulture)
            {

                if (
                    (CultureInfo.CurrentCulture.Name == "fa-IR")
                    || (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fa")
                    )
                {
                    try
                    {
                        CultureInfo PersianCulture = CultureHelper.GetPersianCulture();
                        Thread.CurrentThread.CurrentCulture = PersianCulture;
                        Thread.CurrentThread.CurrentUICulture = PersianCulture;
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        //can happen in medium trust
                        log.Error(ex);
                    }
                    catch (ArgumentException ex)
                    {
                        log.Info("swallowed error in culture helper", ex);
                    }

                }
            }
            

            //this doesn't work but was a nice idea
            //http://weblogs.asp.net/abdullaabdelhaq/archive/2009/06/27/displaying-arabic-number.aspx
            //has the only real solution
            //if ((CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar") && (!CultureInfo.CurrentCulture.IsNeutralCulture))
            //{
            //    CultureInfo arabic = new CultureInfo(CultureInfo.CurrentCulture.Name);
            //    arabic.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
            //    string[] arabicDigits = new string[] { "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩" };
            //    arabic.NumberFormat.NativeDigits = arabicDigits;
            //    Thread.CurrentThread.CurrentCulture = arabic;
            //    Thread.CurrentThread.CurrentUICulture = arabic;
            //}

            

        }


        private void Error(object sender, EventArgs e)
        {

        }


        private void EndRequest(object sender, EventArgs e)
        {

        }




        public void Dispose() { }

    }

}
