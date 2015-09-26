//	Created:			    2012-03-09
//	Last Modified:		    2012-03-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// A control designed to render shema.org markup for an organization
    /// http://schema.org/Organization
    /// </summary>
    public class OrganizationControl : WebControl
    {
        private bool loadFromSiteSettings = false;

        public bool LoadFromSiteSettings
        {
            get { return loadFromSiteSettings; }
            set { loadFromSiteSettings = value; }
        }

        private string organizationName = string.Empty;

        public string OrganizationName
        {
            get { return organizationName; }
            set { organizationName = value; }
        }

        private string streetAddress = string.Empty;

        public string StreetAddress
        {
            get { return streetAddress; }
            set { streetAddress = value; }
        }

        private string streetAddress2 = string.Empty;

        public string StreetAddress2
        {
            get { return streetAddress2; }
            set { streetAddress2 = value; }
        }

        private string addressLocality = string.Empty;

        public string AddressLocality
        {
            get { return addressLocality; }
            set { addressLocality = value; }
        }

        private string addressRegion = string.Empty;

        public string AddressRegion
        {
            get { return addressRegion; }
            set { addressRegion = value; }
        }

        private string postalCode = string.Empty;

        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        private string addressCountry = string.Empty;

        public string AddressCountry
        {
            get { return addressCountry; }
            set { addressCountry = value; }
        }

        private string telephone = string.Empty;

        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        private string telephoneLabel = string.Empty;

        public string TelephoneLabel
        {
            get { return telephoneLabel; }
            set { telephoneLabel = value; }
        }

        private string faxNumber = string.Empty;

        public string FaxNumber
        {
            get { return faxNumber; }
            set { faxNumber = value; }
        }

        private string faxNumberLabel = string.Empty;

        public string FaxNumberLabel
        {
            get { return faxNumberLabel; }
            set { faxNumberLabel = value; }
        }

        private string email = string.Empty;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private bool useMailToForEmail = true;

        public bool UseMailToForEmail
        {
            get { return useMailToForEmail; }
            set { useMailToForEmail = value; }
        }

        private string emailLabel = string.Empty;

        public string EmailLabel
        {
            get { return emailLabel; }
            set { emailLabel = value; }
        }

        private bool useCommaAfterLocality = true;

        public bool UseCommaAfterLocality
        {
            get { return useCommaAfterLocality; }
            set { useCommaAfterLocality = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (loadFromSiteSettings)
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings == null) { return; }

                organizationName = siteSettings.CompanyName;
                streetAddress = siteSettings.CompanyStreetAddress;
                streetAddress2 = siteSettings.CompanyStreetAddress2;
                addressLocality = siteSettings.CompanyLocality;
                addressRegion = siteSettings.CompanyRegion;
                postalCode = siteSettings.CompanyPostalCode;
                addressCountry = siteSettings.CompanyCountry;
                telephone = siteSettings.CompanyPhone;
                faxNumber = siteSettings.CompanyFax;
                email = siteSettings.CompanyPublicEmail;

            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (organizationName.Length == 0) { return; }

            writer.Write("<div class='org' itemscope itemtype=\"http://schema.org/Organization\">");

            writer.Write("<span class='orgname' itemprop=\"name\">");
            writer.Write(organizationName);
            writer.Write("</span>");

            if (streetAddress.Length > 0)
            {
                writer.Write("<div class='addresswrap' itemprop=\"address\" itemscope itemtype=\"http://schema.org/PostalAddress\">");

                writer.Write("\n<span itemprop=\"streetAddress\">");
                writer.Write(streetAddress);

                if (streetAddress2.Length > 0)
                {
                    writer.Write("<br />");
                    writer.Write(streetAddress2);
                }

                writer.Write("</span>");

                if (addressLocality.Length > 0)
                {
                    writer.Write("\n<span itemprop=\"addressLocality\">");
                    writer.Write(addressLocality);
                    writer.Write("</span>");
                }

                if (useCommaAfterLocality)
                {
                    writer.Write(", ");
                }

                if (addressRegion.Length > 0)
                {
                    writer.Write("\n<span itemprop=\"addressRegion\">");
                    writer.Write(addressRegion);
                    writer.Write("</span>");
                }

                if (postalCode.Length > 0)
                {
                    writer.Write("\n<span itemprop=\"postalCode\">");
                    writer.Write(postalCode);
                    writer.Write("</span>");
                }

                if (addressCountry.Length > 0)
                {
                    writer.Write("\n<span itemprop=\"addressCountry\">");
                    writer.Write(addressCountry);
                    writer.Write("</span>");
                }


                writer.Write("</div>");
            }// end address

            if (telephone.Length > 0)
            {
                if (telephoneLabel.Length > 0)
                {
                    writer.Write(telephoneLabel);
                }
                writer.Write("\n<span itemprop=\"telephone\">");
                writer.Write(telephone);
                writer.Write("</span>");

            }

            if (faxNumber.Length > 0)
            {
                if (faxNumberLabel.Length > 0)
                {
                    writer.Write(faxNumberLabel);
                }
                writer.Write("\n<span itemprop=\"faxNumber\">");
                writer.Write(faxNumber);
                writer.Write("</span>");

            }

            if (email.Length > 0)
            {
                if (emailLabel.Length > 0)
                {
                    writer.Write(emailLabel);
                }
                writer.Write("\n<span itemprop=\"email\">");

                if (useMailToForEmail)
                {
                    writer.Write("<a href='mailto:");
                    writer.Write(email);
                    writer.Write("'>");
                    writer.Write(email);
                    writer.Write("</a>");
                }
                else
                {
                    writer.Write(email);
                }

                writer.Write("</span>");

            }



            writer.Write("\n</div>");
        }

    }
}