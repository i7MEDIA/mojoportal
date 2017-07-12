/********************************************************
 * Copyright (C) 2007 Andrew Arnott
 * Released under the New BSD License
 * License available here: http://www.opensource.org/licenses/bsd-license.php
 * For news or support on this file: http://jmpinline.nerdbank.net/
 ********************************************************/

// based on work as noted above with modifications 
// to meet the need of mojoPortal by 
//  Last Modified:  2007-08-23
// 2008-08-13  added support for ID Selector https://www.idselector.com/
// 2008-05-14 upgrade to dotnetopenauth

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using DotNetOpenAuth.OpenId.RelyingParty;
using mojoPortal.Web.Controls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    [DefaultProperty("OpenIdUrl")]
    [ToolboxData("<{0}:OpenIdLogin runat=\"server\"></{0}:OpenIdLogin>")]
    public class mojoOpenIdRegister : OpenIdTextBox
    {
        // Regex comes from http://mjtsai.com/blog/2003/08/18/url_regex_generator/
        // It's (allegedly) fully RFC-compliant.  
        // The short version that the RegularExpressionValidator control 
        // prescribes doesn't allow "localhost" or IP addresses as hosts.
        internal const string UriRegex = @"(?:http://(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?)(?:/(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;:@&=])*)(?:/(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;:@&=])*))*)(?:\?(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;:@&=])*))?)?)|(?:ftp://(?:(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;?&=])*)(?::(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;?&=])*))?@)?(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?))(?:/(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[?:@&=])*)(?:/(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[?:@&=])*))*)(?:;type=[AIDaid])?)?)|(?:news:(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;/?:&=])+@(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3})))|(?:[a-zA-Z](?:[a-zA-Z\d]|[_.+-])*)|\*))|(?:nntp://(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?)/(?:[a-zA-Z](?:[a-zA-Z\d]|[_.+-])*)(?:/(?:\d+))?)|(?:telnet://(?:(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;?&=])*)(?::(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;?&=])*))?@)?(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?))/?)|(?:gopher://(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?)(?:/(?:[a-zA-Z\d$\-_.+!*'(),;/?:@&=]|(?:%[a-fA-F\d]{2}))(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),;/?:@&=]|(?:%[a-fA-F\d]{2}))*)(?:%09(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;:@&=])*)(?:%09(?:(?:[a-zA-Z\d$\-_.+!*'(),;/?:@&=]|(?:%[a-fA-F\d]{2}))*))?)?)?)?)|(?:wais://(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?)/(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))*)(?:(?:/(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))*)/(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))*))|\?(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;:@&=])*))?)|(?:mailto:(?:(?:[a-zA-Z\d$\-_.+!*'(),;/?:@&=]|(?:%[a-fA-F\d]{2}))+))|(?:file://(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))|localhost)?/(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[?:@&=])*)(?:/(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[?:@&=])*))*))|(?:prospero://(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?)/(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[?:@&=])*)(?:/(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[?:@&=])*))*)(?:(?:;(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[?:@&])*)=(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[?:@&])*)))*)|(?:ldap://(?:(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?))?/(?:(?:(?:(?:(?:(?:(?:[a-zA-Z\d]|%(?:3\d|[46][a-fA-F\d]|[57][Aa\d]))|(?:%20))+|(?:OID|oid)\.(?:(?:\d+)(?:\.(?:\d+))*))(?:(?:%0[Aa])?(?:%20)*)=(?:(?:%0[Aa])?(?:%20)*))?(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))*))(?:(?:(?:%0[Aa])?(?:%20)*)\+(?:(?:%0[Aa])?(?:%20)*)(?:(?:(?:(?:(?:[a-zA-Z\d]|%(?:3\d|[46][a-fA-F\d]|[57][Aa\d]))|(?:%20))+|(?:OID|oid)\.(?:(?:\d+)(?:\.(?:\d+))*))(?:(?:%0[Aa])?(?:%20)*)=(?:(?:%0[Aa])?(?:%20)*))?(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))*)))*)(?:(?:(?:(?:%0[Aa])?(?:%20)*)(?:[;,])(?:(?:%0[Aa])?(?:%20)*))(?:(?:(?:(?:(?:(?:[a-zA-Z\d]|%(?:3\d|[46][a-fA-F\d]|[57][Aa\d]))|(?:%20))+|(?:OID|oid)\.(?:(?:\d+)(?:\.(?:\d+))*))(?:(?:%0[Aa])?(?:%20)*)=(?:(?:%0[Aa])?(?:%20)*))?(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))*))(?:(?:(?:%0[Aa])?(?:%20)*)\+(?:(?:%0[Aa])?(?:%20)*)(?:(?:(?:(?:(?:[a-zA-Z\d]|%(?:3\d|[46][a-fA-F\d]|[57][Aa\d]))|(?:%20))+|(?:OID|oid)\.(?:(?:\d+)(?:\.(?:\d+))*))(?:(?:%0[Aa])?(?:%20)*)=(?:(?:%0[Aa])?(?:%20)*))?(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))*)))*))*(?:(?:(?:%0[Aa])?(?:%20)*)(?:[;,])(?:(?:%0[Aa])?(?:%20)*))?)(?:\?(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))+)(?:,(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))+))*)?)(?:\?(?:base|one|sub)(?:\?(?:((?:[a-zA-Z\d$\-_.+!*'(),;/?:@&=]|(?:%[a-fA-F\d]{2}))+)))?)?)?)|(?:(?:z39\.50[rs])://(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?)(?:/(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))+)(?:\+(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))+))*(?:\?(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))+))?)?(?:;esn=(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))+))?(?:;rs=(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))+)(?:\+(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))+))*)?))|(?:cid:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;?:@&=])*))|(?:mid:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;?:@&=])*)(?:/(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[;?:@&=])*))?)|(?:vemmi://(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?)(?:/(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[/?:@&=])*)(?:(?:;(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[/?:@&])*)=(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[/?:@&])*))*))?)|(?:imap://(?:(?:(?:(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[&=~])+)(?:(?:;[Aa][Uu][Tt][Hh]=(?:\*|(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[&=~])+))))?)|(?:(?:;[Aa][Uu][Tt][Hh]=(?:\*|(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[&=~])+)))(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[&=~])+))?))@)?(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?))/(?:(?:(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[&=~:@/])+)?;[Tt][Yy][Pp][Ee]=(?:[Ll](?:[Ii][Ss][Tt]|[Ss][Uu][Bb])))|(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[&=~:@/])+)(?:\?(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[&=~:@/])+))?(?:(?:;[Uu][Ii][Dd][Vv][Aa][Ll][Ii][Dd][Ii][Tt][Yy]=(?:[1-9]\d*)))?)|(?:(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[&=~:@/])+)(?:(?:;[Uu][Ii][Dd][Vv][Aa][Ll][Ii][Dd][Ii][Tt][Yy]=(?:[1-9]\d*)))?(?:/;[Uu][Ii][Dd]=(?:[1-9]\d*))(?:(?:/;[Ss][Ee][Cc][Tt][Ii][Oo][Nn]=(?:(?:(?:[a-zA-Z\d$\-_.+!*'(),]|(?:%[a-fA-F\d]{2}))|[&=~:@/])+)))?)))?)|(?:nfs:(?:(?://(?:(?:(?:(?:(?:[a-zA-Z\d](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?)\.)*(?:[a-zA-Z](?:(?:[a-zA-Z\d]|-)*[a-zA-Z\d])?))|(?:(?:\d+)(?:\.(?:\d+)){3}))(?::(?:\d+))?)(?:(?:/(?:(?:(?:(?:(?:[a-zA-Z\d\$\-_.!~*'(),])|(?:%[a-fA-F\d]{2})|[:@&=+])*)(?:/(?:(?:(?:[a-zA-Z\d\$\-_.!~*'(),])|(?:%[a-fA-F\d]{2})|[:@&=+])*))*)?)))?)|(?:/(?:(?:(?:(?:(?:[a-zA-Z\d\$\-_.!~*'(),])|(?:%[a-fA-F\d]{2})|[:@&=+])*)(?:/(?:(?:(?:[a-zA-Z\d\$\-_.!~*'(),])|(?:%[a-fA-F\d]{2})|[:@&=+])*))*)?))|(?:(?:(?:(?:(?:[a-zA-Z\d\$\-_.!~*'(),])|(?:%[a-fA-F\d]{2})|[:@&=+])*)(?:/(?:(?:(?:[a-zA-Z\d\$\-_.!~*'(),])|(?:%[a-fA-F\d]{2})|[:@&=+])*))*)?)))";
        Panel panel;
        Button loginButton;
        //HtmlGenericControl label;
        RequiredFieldValidator requiredValidator;
        RegularExpressionValidator uriFormatValidator;
        Label examplePrefixLabel;
        Label exampleUrlLabel;
        TableCell buttonCell;
        //HyperLink registerLink;

        protected override void CreateChildControls()
        {
            // Don't call base.CreateChildControls().  This would add the WrappedTextBox
            // to the Controls collection, which would implicitly remove it from the table
            // we have already added it to.

            // Just add the panel we've assembled earlier.
            Controls.Add(panel);

            if (ShouldBeFocused)
                WrappedTextBox.Focus();
        }

        protected override void InitializeControls()
        {
            base.InitializeControls();

            panel = new Panel();

            HtmlGenericControl divSettingRow = new HtmlGenericControl("div");
            divSettingRow.Attributes.Add("class", "settingrow");

            HtmlGenericControl divButtonRow = new HtmlGenericControl("div");
            divButtonRow.Attributes.Add("class", "settingrow");

            Table table = new Table();
            TableRow exampleRow, inputRow;
            TableCell cell;
            exampleRow = new TableRow();
            table.Rows.Add(exampleRow);
            table.Rows.Add(inputRow = new TableRow());

            // top row
            cell = new TableCell();
            cell.Style[HtmlTextWriterStyle.Color] = "gray";
            cell.Style[HtmlTextWriterStyle.FontSize] = "smaller";
            requiredValidator = new RequiredFieldValidator();
            requiredValidator.ErrorMessage = RequiredTextDefault + RequiredTextSuffix;
            requiredValidator.Text = RequiredTextDefault + RequiredTextSuffix;
            requiredValidator.Display = ValidatorDisplay.Dynamic;
            requiredValidator.ControlToValidate = WrappedTextBox.ID;
            requiredValidator.ValidationGroup = ValidationGroupDefault;
            cell.Controls.Add(requiredValidator);
            uriFormatValidator = new RegularExpressionValidator();
            uriFormatValidator.ErrorMessage = UriFormatTextDefault + RequiredTextSuffix;
            uriFormatValidator.Text = UriFormatTextDefault + RequiredTextSuffix;
            uriFormatValidator.ValidationExpression = UriRegex;
            uriFormatValidator.Enabled = UriValidatorEnabledDefault;
            uriFormatValidator.Display = ValidatorDisplay.Dynamic;
            uriFormatValidator.ControlToValidate = WrappedTextBox.ID;
            uriFormatValidator.ValidationGroup = ValidationGroupDefault;
            cell.Controls.Add(uriFormatValidator);
            examplePrefixLabel = new Label();
            examplePrefixLabel.Text = ExamplePrefixDefault;
            cell.Controls.Add(examplePrefixLabel);
            cell.Controls.Add(new LiteralControl(" "));
            exampleUrlLabel = new Label();
            exampleUrlLabel.Font.Bold = true;
            exampleUrlLabel.Text = ExampleUrlDefault;
            cell.Controls.Add(exampleUrlLabel);
            exampleRow.Cells.Add(cell);

            //  row2
            buttonCell = new TableCell();
            buttonCell.Controls.Add(WrappedTextBox);
            inputRow.Cells.Add(buttonCell);

      
            loginButton = new Button();
            loginButton.ID = "loginButton";
            loginButton.Text = ButtonTextDefault;
            loginButton.ToolTip = ButtonToolTipDefault;
            loginButton.Click += new EventHandler(loginButton_Click);
            if (ValidationGroupDefault.Length > 0)
            {
                loginButton.ValidationGroup = ValidationGroupDefault;
            }
#if !Mono
            panel.DefaultButton = loginButton.ID;
#endif

            
            
            SiteLabel lbl = new SiteLabel();
            lbl.ForControl = WrappedTextBox.ID;
            lbl.ConfigKey = "OpenIDLabel";
            lbl.CssClass = "settinglabel";
            divSettingRow.Controls.Add(lbl);
            divSettingRow.Controls.Add(table);

            lbl = new SiteLabel();
            lbl.ConfigKey = "spacer";
            lbl.CssClass = "settinglabel";
            divButtonRow.Controls.Add(lbl);
            divButtonRow.Controls.Add(loginButton);

            panel.Controls.Add(divSettingRow);
            panel.Controls.Add(divButtonRow);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupSelectorScript();
        }

        private void SetupSelectorScript()
        {

            if (buttonCell == null) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }
            if (siteSettings.OpenIdSelectorId.Length == 0) { return; }

            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n");
            script.Append("idselector_input_id = \"" + WrappedTextBox.ClientID + "\";");
            script.Append("\n");
            script.Append("idselector_target_id = \"" + buttonCell.ClientID + "\";");
            script.Append("\n");

            script.Append(" </script>");

            script.Append("\n");

            script.Append("<script type=\"text/javascript\" id=\"__openidselector\" src=\"https://www.idselector.com/selector/"
                + siteSettings.OpenIdSelectorId + "\" charset=\"utf-8\"></script>");

            script.Append("\n");


            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "openidselector",
                script.ToString());


        }

        protected override void RenderChildren(HtmlTextWriter writer)
        {
            
            base.RenderChildren(writer);
        }

        #region Properties
        

        [Bindable(true)]
        [Category("Appearance")]
        [Localizable(true)]
        new public string Text
        {
            get { return WrappedTextBox.Text; }
            set { WrappedTextBox.Text = value; }
        }

        private bool showExample = true;
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool ShowExample
        {
            get { return showExample; }
            set { showExample = value; }
        }

        const string ExamplePrefixDefault = "Example:";
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(ExamplePrefixDefault)]
        [Localizable(true)]
        public string ExamplePrefix
        {
            get { return examplePrefixLabel.Text; }
            set { examplePrefixLabel.Text = value; }
        }

        const string ExampleUrlDefault = "http://your.name.myopenid.com";
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(ExampleUrlDefault)]
        [Localizable(true)]
        public string ExampleUrl
        {
            get { return exampleUrlLabel.Text; }
            set { exampleUrlLabel.Text = value; }
        }

        const string RequiredTextSuffix = "<br/>";
        const string RequiredTextDefault = "Provide an OpenID first.";
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(RequiredTextDefault)]
        [Localizable(true)]
        public string RequiredText
        {
            get { return requiredValidator.Text.Substring(0, requiredValidator.Text.Length - RequiredTextSuffix.Length); }
            set { requiredValidator.ErrorMessage = requiredValidator.Text = value + RequiredTextSuffix; }
        }

        const string UriFormatTextDefault = "Invalid OpenID URL.";
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(UriFormatTextDefault)]
        [Localizable(true)]
        public string UriFormatText
        {
            get { return uriFormatValidator.Text.Substring(0, uriFormatValidator.Text.Length - RequiredTextSuffix.Length); }
            set { uriFormatValidator.ErrorMessage = uriFormatValidator.Text = value + RequiredTextSuffix; }
        }

        const bool UriValidatorEnabledDefault = true;
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(UriValidatorEnabledDefault)]
        public bool UriValidatorEnabled
        {
            get { return uriFormatValidator.Enabled; }
            set { uriFormatValidator.Enabled = value; }
        }

       
        const string ButtonTextDefault = "Login";
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(ButtonTextDefault)]
        [Localizable(true)]
        public string ButtonText
        {
            get { return loginButton.Text; }
            set { loginButton.Text = value; }
        }

        const string ButtonToolTipDefault = "Account login";
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(ButtonToolTipDefault)]
        [Localizable(true)]
        public string ButtonToolTip
        {
            get { return loginButton.ToolTip; }
            set { loginButton.ToolTip = value; }
        }

        public string ButtonID
        {
            get { return loginButton.ID; }
        }

        [Bindable(false)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Localizable(false)]
        public bool ButtonFontBold
        {
            get { return loginButton.Font.Bold; }
            set { loginButton.Font.Bold = value; }
        }

        const string ValidationGroupDefault = "";
        [Category("Behavior")]
        [DefaultValue(ValidationGroupDefault)]
        public string ValidationGroup
        {
            get { return requiredValidator.ValidationGroup; }
            set
            {
                requiredValidator.ValidationGroup = value;
                loginButton.ValidationGroup = value;
            }
        }
        #endregion

        #region Event handlers
        void loginButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            
            base.LogOn();
           
        }

        #endregion

       
    }
}
