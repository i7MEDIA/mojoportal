using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Configuration;
using GCheckout;
using GCheckout.Checkout;
using GCheckout.Util;

namespace mojoPortal.Web.UI
{

    /// <summary>
    /// Class for All Google Checkout Buttons.
    /// </summary>
    public class mojoGCheckoutButton : ImageButton
    {

        private BackgroundType _Background = BackgroundType.White;
        private string _Currency = "USD";
        private int _CartExpirationMinutes = 0;
        private bool _UseHttps = false;
        private CommerceConfiguration commercConfig = null;
        private ButtonSize _Size = ButtonSize.Medium;

        protected CommerceConfiguration CommercConfig
        {
            get
            {
                if (commercConfig == null) commercConfig = SiteUtils.GetCommerceConfig();

                return commercConfig;
            }
        }

        /// <summary>
        /// The <b>Size</b> property value determines the size of the 
        /// Google Checkout button that will display on your web page.
        /// Valid values for this property are "Small", "Medium" and 
        /// "Large". A small button is 160 pixels wide and 43 pixels high.
        /// A medium button is 168 pixels wide and 44 pixels high. A large
        /// button is 180 pixels wide and 46 pixels high.
        /// </summary>
        [Category("Google")]
        [Description("Small: 160 by 43 pixels\nMedium: 168 by 44 pixels\n" +
           "Large: 180 by 46 pixels")]
        public ButtonSize Size
        {
            get
            {
                return _Size;
            }
            set
            {
                _Size = value;
                SetImageUrl();
            }
        }

        /// <summary>
        /// The name of the gif file for the image
        /// </summary>
        protected string GifFileName
        {
            get { return "checkout"; }
        }

        /// <summary>
        /// True if this is a donation to a non-profit, false if it is a regular 
        /// purchase. The Checkout pages have slightly different wording for 
        /// donations.
        /// </summary>
        protected virtual bool IsDonation
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// The <b>GoogleMerchantID</b> property value identifies your Google 
        /// Checkout Merchant ID. This value should be set in your web.config file.
        /// </summary>
        [Category("Google")]
        [Description("Your numeric Merchant ID. To see it, log in to Google, " +
           "select the Settings tab, click the Integration link.")]
        protected string MerchantID
        {
            get
            {
                  
                if(CommercConfig != null)
                {
                    return CommercConfig.GoogleMerchantID;
                }
              
                return string.Empty;
                
            }
        }

        /// <summary>
        /// The <b>GoogleMerchantKey</b> property value identifies your Google 
        /// Checkout Merchant key. This value should be set in your web.config file.
        /// </summary>
        [Category("Google")]
        [Description("Your alpha-numeric Merchant Key. To see it, log in to " +
           "Google, select the Settings tab, click the Integration link.")]
        protected string MerchantKey
        {
            get
            {
                if(CommercConfig != null)
                {
                    return CommercConfig.GoogleMerchantKey;
                }
              
                return string.Empty;

                
            }
        }

        /// <summary>
        /// The <b>GoogleEnvironment</b> property value identifies the environment 
        /// in which your application is running. In your test environment, the 
        /// value of the <b>GoogleEnvironment</b> property should be 
        /// <b>Sandbox</b>. In your production environment, the value of the 
        /// property should be <b>Production</b>.
        /// </summary>
        [Category("Google")]
        [Description("Sandbox is the test environment where no funds are ever " +
           "taken from or paid to anyone. In Production all transactions are " +
           "real.")]
        protected EnvironmentType Environment
        {
            get
            {
                if(CommercConfig != null)
                {
                    return CommercConfig.GoogleEnvironment;
                }
              
                return EnvironmentType.Unknown;

                
            }
        }

        /// <summary>
        /// The <b>Background</b> property value indicates whether the Google
        /// Checkout button should be displayed on a white background or a 
        /// transparent background. Valid values for this property are "White"
        /// and "Transparent".
        /// </summary>
        [Category("Google")]
        [Description("Use White if you're placing the button on a white " +
           "background, or Transparent if you're placing the button on a " +
           "colored background.")]
        [DefaultValue(BackgroundType.White)]
        public BackgroundType Background
        {
            get
            {
                return _Background;
            }
            set
            {
                _Background = value;
                SetImageUrl();
            }
        }

        /// <summary>
        /// The <b>Currency</b> property value identifies the currency that should
        /// be associated with prices in your Checkout API requests. The value of 
        /// this property should be a three-letter ISO 4217 currency code.
        /// </summary>
        [Category("Google")]
        [Description("USD for US dollars, GBP for British pounds, SEK for " +
           "Swedish krona, EUR for Euro etc.")]
        public string Currency
        {
            get
            {
                return _Currency;
            }
            set
            {
                _Currency = value;
            }
        }

        /// <summary>
        /// The <b>CartExpirationMinutes</b> property value identifies the length
        /// of time, in minutes, after which an unsubmitted shopping cart will 
        /// become invalid. A value of <b>0</b> indicates that the shopping cart
        /// does not expire.
        /// </summary>
        [Category("Google")]
        [Description("How many minutes (after the user clicks the Google " +
           "Checkout button on this page) until the cart expires. 0 means the " +
           "cart doesn't expire.")]
        public int CartExpirationMinutes
        {
            get
            {
                return _CartExpirationMinutes;
            }
            set
            {
                if (value >= 0)
                {
                    _CartExpirationMinutes = value;
                }
            }
        }

        /// <summary>
        /// The <b>UseHttps</b> property sets whether the button graphic should
        /// be requested from Google with an HTTPS call. The default (false) is to
        /// use HTTP. If the page is fetched through HTTPS, some users (depending 
        /// on browser settings) will get security warnings if the button graphic
        /// is fetched with HTTP.
        /// </summary>
        [Category("Google")]
        [Description("If true, the button graphic will be fetched with a HTTPS " +
           "request.")]
        [DefaultValue(false)]
        public bool UseHttps
        {
            get
            {
                return _UseHttps;
            }
            set
            {
                _UseHttps = value;
                SetImageUrl();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is enabled.
        /// </summary>
        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
                SetImageUrl();
            }
        }

        /// <summary>
        /// On initialization, this class calls the SetImageUrl method.
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            SetImageUrl();
        }

        /// <summary>
        /// Set the image url
        /// </summary>
        protected virtual void SetImageUrl()
        {
            int width = 0;
            int height = 0;
            switch (Size)
            {
                case ButtonSize.Small:
                    width = 160;
                    height = 43;
                    break;
                case ButtonSize.Medium:
                    width = 168;
                    height = 44;
                    break;
                case ButtonSize.Large:
                    width = 180;
                    height = 46;
                    break;
            }
            this.Width = width;
            this.Height = height;

            string StyleInUrl = "white";
            if (Background == BackgroundType.Transparent)
                StyleInUrl = "trans";
            string VariantInUrl = "text";
            if (!Enabled)
                VariantInUrl = "disabled";
            string Protocol = "http";
            if (UseHttps)
                Protocol = "https";
            string Location = "en_US";
            if (_Currency == "GBP")
                Location = "en_GB";

            if (Environment == EnvironmentType.Sandbox)
            {
                ImageUrl = string.Format(
                  "{0}://sandbox.google.com/checkout/buttons/{6}.gif?" +
                  "merchant_id={1}&w={2}&h={3}&style={4}&variant={5}&loc={7}",
                  Protocol, MerchantID, Width.Value, Height.Value, StyleInUrl,
                  VariantInUrl, GifFileName, Location);
            }
            else
            {
                ImageUrl = string.Format(
                  "{0}://checkout.google.com/buttons/{6}.gif?" +
                  "merchant_id={1}&w={2}&h={3}&style={4}&variant={5}&loc={7}",
                  Protocol, MerchantID, Width.Value, Height.Value, StyleInUrl,
                  VariantInUrl, GifFileName, Location);
            }

        }

        /// <summary>
        /// This method calls the <see cref="CheckoutShoppingCartRequest"/> class
        /// to initialize a new instance of that class. Before doing so, this method
        /// verifies that the MerchantID, MerchantKey and Environment properties
        /// have all been set.
        /// </summary>
        public CheckoutShoppingCartRequest CreateRequest()
        {
            if (MerchantID == string.Empty)
            {
                throw new ApplicationException("Set GoogleMerchantID in the " +
                  "web.config file.");
            }
            if (MerchantKey == string.Empty)
            {
                throw new ApplicationException("Set GoogleMerchantKey in the " +
                  "web.config file.");
            }
            if (Environment == EnvironmentType.Unknown)
            {
                throw new ApplicationException("Set GoogleEnvironment in the " +
                  "web.config file.");
            }
            return new CheckoutShoppingCartRequest(MerchantID, MerchantKey,
              Environment, Currency, CartExpirationMinutes, IsDonation);
        }
    }


    /// <summary>
    /// This enumeration defines valid sizes for the Google Checkout button.
    /// Valid values for the "Size" property are "Small", "Medium" and "Large".
    /// </summary>
    public enum ButtonSize
    {
        /// <summary>160 x 43 pixels</summary>
        Small = 0,
        /// <summary>168 by 44 pixels</summary>
        Medium = 1,
        /// <summary>180 x 46 pixels</summary>
        Large = 2
    }

    /// <summary>
    /// This enumeration defines valid background colors for the Google Checkout
    /// button. Valid values for the "Background" property are "White" and 
    /// "Transparent".
    /// </summary>
    public enum BackgroundType
    {
        /// <summary>You are placing the button on a white background</summary>
        White = 0,
        /// <summary>You are placing the button on a colored background</summary>
        Transparent = 1
    }
}
