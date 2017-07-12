// Author:		        
// Created:             2009-05-15
// Last Modified:       2009-05-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Xml;
using System.Xml.XPath;

namespace mojoPortal.Web
{
    /// <summary>
    /// A wrapper class to encaspsulate the xml response from rpxnow.com
    /// https://rpxnow.com/docs
    /// </summary>
    public class OpenIdRpxAuthInfo
    {
        public OpenIdRpxAuthInfo(XmlElement authElement)
        {
            auth = authElement;
            xpathNavigator = auth.CreateNavigator();

        }


        private string GetNodeValue(string xpathEpression)
        {
            try
            {
                XPathNodeIterator nodes = (XPathNodeIterator)xpathNavigator.Evaluate(xpathEpression);
                while (nodes.MoveNext())
                {
                    return nodes.Current.ToString();
                }
            }
            catch (ArgumentException) { }
            catch (XPathException) { }

            return string.Empty;
        }

        private XmlElement auth = null;
        private XPathNavigator xpathNavigator = null;

        /// <summary>
        /// returns true if the response has an identifier
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (Identifier.Length == 0) { return false; }
                
                return true;
            }
        }

        /// <summary>
        /// The user's OpenID URL. Use this value to sign the user in to your website. This field is always present.
        /// </summary>
        public string Identifier
        {
            get { return GetNodeValue("/rsp/profile/identifier"); }
        }

        /// <summary>
        /// A human-readable name of the authentication provider that was used for this authentication. 
        /// For well-known providers, RPX sends values such as "Google", "Facebook", and "MySpace"; 
        /// "Other" is sent for other providers. New provider names are added over time.
        /// </summary>
        public string ProviderName
        {
            get { return GetNodeValue("/rsp/profile/providerName"); }
        }

        /// <summary>
        /// Primary key of the user in your database. Only present if you are using the mappings API.
        /// </summary>
        public string PrimaryKey
        {
            get { return GetNodeValue("/rsp/profile/primaryKey"); }
        }

        /// <summary>
        /// The name of this Contact, suitable for display to end-users.
        /// SREG(fullname, nickname), hCard(nickname, fn, givenName), FB(name)
        /// </summary>
        public string DisplayName
        {
            get { return GetNodeValue("/rsp/profile/displayName"); }
        }

        /// <summary>
        /// The preferred username of this contact on sites that ask for a username.
        /// SREG(nickname)
        /// </summary>
        public string PreferredUsername
        {
            get { return GetNodeValue("/rsp/profile/preferredUsername"); }
        }

        /// <summary>
        /// The full name, including all middle names, titles, and suffixes as appropriate, formatted for display (e.g. Mr. Joseph Robert Smarr, Esq.). 
        /// This is the Primary Sub-Field for this field, for the purposes of sorting and filtering.
        /// </summary>
        public string FormattedName
        {
            get { return GetNodeValue("/rsp/profile/name/formatted"); }
        }

        /// <summary>
        /// The family name of this Contact, or "Last Name" in most Western languages (e.g. Smarr given the full name Mr. Joseph Robert Smarr, Esq.).
        /// hCard(family_name), FB(last_name)
        /// </summary>
        public string FamilyName
        {
            get { return GetNodeValue("/rsp/profile/name/familyName"); }
        }

        /// <summary>
        /// The given name of this Contact, or "First Name" in most Western languages (e.g. Joseph given the full name Mr. Joseph Robert Smarr, Esq.).
        /// hCard(given_name), FB(first_name)
        /// </summary>
        public string GivenName
        {
            get { return GetNodeValue("/rsp/profile/name/givenName"); }
        }

        /// <summary>
        /// The middle name(s) of this Contact (e.g. Robert given the full name Mr. Joseph Robert Smarr, Esq.).
        /// </summary>
        public string MiddleName
        {
            get { return GetNodeValue("/rsp/profile/name/middleName"); }
        }

        /// <summary>
        /// The honorific prefix(es) of this Contact, or "Title" in most Western languages (e.g. Mr. given the full name Mr. Joseph Robert Smarr, Esq.).
        /// hCard(honorific_title)
        /// </summary>
        public string HonorificPrefix
        {
            get { return GetNodeValue("/rsp/profile/name/honorificPrefix"); }
        }

        /// <summary>
        /// The honorific suffix(es) of this Contact, or "Suffix" in most Western languages (e.g. Esq. given the full name Mr. Joseph Robert Smarr, Esq.).
        /// hCard(honorific_title)
        /// </summary>
        public string HonorificSuffix
        {
            get { return GetNodeValue("/rsp/profile/name/honorificSuffix"); }
        }

        /// <summary>
        /// The full mailing address, formatted for display or use with a mailing label.
        /// </summary>
        public string FormattedAddress
        {
            get { return GetNodeValue("/rsp/profile/address/formatted"); }
        }

        /// <summary>
        /// The full street address component, which may include house number, street name, PO BOX, and multi-line extended street address information.
        /// </summary>
        public string StreetAddress
        {
            get { return GetNodeValue("/rsp/profile/address/streetAddress"); }
        }

        /// <summary>
        /// The city or locality component.
        /// </summary>
        public string Locality
        {
            get { return GetNodeValue("/rsp/profile/address/locality"); }
        }

        /// <summary>
        /// The state or region component.
        /// </summary>
        public string Region
        {
            get { return GetNodeValue("/rsp/profile/address/region"); }
        }

        /// <summary>
        /// Postal code or zipcode.
        /// </summary>
        public string PostalCode
        {
            get { return GetNodeValue("/rsp/profile/address/postalCode"); }
        }

        /// <summary>
        /// The country name component.
        /// </summary>
        public string Country
        {
            get { return GetNodeValue("/rsp/profile/address/country"); }
        }


        /// <summary>
        /// The gender of this person. Canonical values are 'male', and 'female', but may be any value.
        /// SREG(gender), FB(sex)
        /// </summary>
        public string Gender
        {
            get { return GetNodeValue("/rsp/profile/gender"); }
        }

        /// <summary>
        /// Date of birth in YYYY-MM-DD format. Year field may be 0000 if unavailable.
        /// SREG(dob), hCard(bday)
        /// </summary>
        public string Birthday
        {
            get { return GetNodeValue("/rsp/profile/birthday"); }
        }

        /// <summary>
        /// The offset from UTC of this Contact's current time zone, as of the time this response was returned. 
        /// The value MUST conform to the offset portion of xs:dateTime, e.g. -08:00. 
        /// Note that this value MAY change over time due to daylight saving time, 
        /// and is thus meant to signify only the current value of the user's timezone offset.
        /// SREG(timezone), hCard(tz)
        /// </summary>
        public string UtcOffset
        {
            get { return GetNodeValue("/rsp/profile/utcOffset"); }
        }

        /// <summary>
        /// An email address at which the person may be reached.
        /// </summary>
        public string Email
        {
            get { return GetNodeValue("/rsp/profile/email"); }
        }

        /// <summary>
        /// An email address at which the person may be reached.
        /// </summary>
        public string VerifiedEmail
        {
            get { return GetNodeValue("/rsp/profile/verifiedEmail"); } 
        }

        /// <summary>
        /// URL of a webpage relating to this person.
        /// </summary>
        public string Url
        {
            get { return GetNodeValue("/rsp/profile/url"); } 
        }

        /// <summary>
        /// A phone number at which the person may be reached.
        /// </summary>
        public string PhoneNumber
        {
            get { return GetNodeValue("/rsp/profile/phoneNumber"); } 
        }

        /// <summary>
        /// URL to a photo (GIF/JPG/PNG) of the person.
        /// </summary>
        public string PhotoUrl
        {
            get { return GetNodeValue("/rsp/profile/photo"); } 
        }


    }
}
