using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace mojoPortal.Web
{
    public class ChangePasswordAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
    {
        private enum State
        {
            ChangePassword,
            Failed,
            Success,
        }
        State _state = State.ChangePassword;

        private WebControlAdapterExtender _extender = null;
        private WebControlAdapterExtender Extender
        {
            get
            {
                if (((_extender == null) && (Control != null)) ||
                    ((_extender != null) && (Control != _extender.AdaptedControl)))
                {
                    _extender = new WebControlAdapterExtender(Control);
                }

                System.Diagnostics.Debug.Assert(_extender != null, "CSS Friendly adapters internal error", "Null extender instance");
                return _extender;
            }
        }

        public ChangePasswordAdapter()
        {
            _state = State.ChangePassword;
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PROTECTED        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ChangePassword changePwd = Control as ChangePassword;
            if (Extender.AdapterEnabled && (changePwd != null))
            {
                RegisterScripts();
                changePwd.ChangedPassword += OnChangedPassword;
                changePwd.ChangePasswordError += OnChangePasswordError;
                _state = State.ChangePassword;
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            ChangePassword changePwd = Control as ChangePassword;
            if (changePwd != null)
            {
                if (changePwd.ChangePasswordTemplate != null)
                {
                    changePwd.ChangePasswordTemplateContainer.Controls.Clear();
                    changePwd.ChangePasswordTemplate.InstantiateIn(changePwd.ChangePasswordTemplateContainer);
                    changePwd.ChangePasswordTemplateContainer.DataBind();
                }

                if (changePwd.SuccessTemplate != null)
                {
                    changePwd.SuccessTemplateContainer.Controls.Clear();
                    changePwd.SuccessTemplate.InstantiateIn(changePwd.SuccessTemplateContainer);
                    changePwd.SuccessTemplateContainer.DataBind();
                }

                changePwd.Controls.Add(new ChangePasswordCommandBubbler());
            }
        }

        protected void OnChangedPassword(object sender, EventArgs e)
        {
            _state = State.Success;
        }

        protected void OnChangePasswordError(object sender, EventArgs e)
        {
            if (_state != State.Success)
            {
                _state = State.Failed;
            }
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderBeginTag(writer, "AspNet-ChangePassword");
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderEndTag(writer);
            }
            else
            {
                base.RenderEndTag(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                ChangePassword changePwd = Control as ChangePassword;
                if (changePwd != null)
                {
                    if ((_state == State.ChangePassword) || (_state == State.Failed))
                    {
                        if (changePwd.ChangePasswordTemplate != null)
                        {
                            changePwd.ChangePasswordTemplateContainer.RenderControl(writer);
                        }
                        else
                        {
                            WriteChangePasswordTitlePanel(writer, changePwd);
                            WriteInstructionPanel(writer, changePwd);
                            WriteHelpPanel(writer, changePwd);
                            WriteUserPanel(writer, changePwd);
                            WritePasswordPanel(writer, changePwd);
                            WriteNewPasswordPanel(writer, changePwd);
                            WriteConfirmNewPasswordPanel(writer, changePwd);
                            if (_state == State.Failed)
                            {
                                WriteFailurePanel(writer, changePwd);
                            }
                            WriteSubmitPanel(writer, changePwd);
                            WriteCreateUserPanel(writer, changePwd);
                            WritePasswordRecoveryPanel(writer, changePwd);
                        }
                    }
                    else if (_state == State.Success)
                    {
                        if (changePwd.SuccessTemplate != null)
                        {
                            changePwd.SuccessTemplateContainer.RenderControl(writer);
                        }
                        else
                        {
                            WriteSuccessTitlePanel(writer, changePwd);
                            WriteSuccessTextPanel(writer, changePwd);
                            WriteContinuePanel(writer, changePwd);
                        }
                    }
                }
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PRIVATE        

        private void RegisterScripts()
        {
        }

        /////////////////////////////////////////////////////////
        // Step 1: change password
        /////////////////////////////////////////////////////////

        private void WriteChangePasswordTitlePanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (!String.IsNullOrEmpty(changePwd.ChangePasswordTitleText))
            {
                string className = (changePwd.TitleTextStyle != null) && (!String.IsNullOrEmpty(changePwd.TitleTextStyle.CssClass)) ? changePwd.TitleTextStyle.CssClass + " " : "";
                className += "AspNet-ChangePassword-ChangePasswordTitlePanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className, "");
                WebControlAdapterExtender.WriteSpan(writer, "", changePwd.ChangePasswordTitleText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteInstructionPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (!String.IsNullOrEmpty(changePwd.InstructionText))
            {
                string className = (changePwd.InstructionTextStyle != null) && (!String.IsNullOrEmpty(changePwd.InstructionTextStyle.CssClass)) ? changePwd.InstructionTextStyle.CssClass + " " : "";
                className += "AspNet-ChangePassword-InstructionPanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className, "");
                WebControlAdapterExtender.WriteSpan(writer, "", changePwd.InstructionText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteFailurePanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            string className = (changePwd.FailureTextStyle != null) && (!String.IsNullOrEmpty(changePwd.FailureTextStyle.CssClass)) ? changePwd.FailureTextStyle.CssClass + " " : "";
            className += "AspNet-ChangePassword-FailurePanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, "");
            WebControlAdapterExtender.WriteSpan(writer, "", changePwd.ChangePasswordFailureText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        private void WriteHelpPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if ((!String.IsNullOrEmpty(changePwd.HelpPageIconUrl)) || (!String.IsNullOrEmpty(changePwd.HelpPageText)))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-HelpPanel", "");
                WebControlAdapterExtender.WriteImage(writer, changePwd.HelpPageIconUrl, "Help");
                WebControlAdapterExtender.WriteLink(writer, changePwd.HyperLinkStyle.CssClass, changePwd.HelpPageUrl, "Help", changePwd.HelpPageText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteUserPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (changePwd.DisplayUserName)
            {
                TextBox textbox = changePwd.ChangePasswordTemplateContainer.FindControl("UserName") as TextBox;
                if (textbox != null)
                {
                    Page.ClientScript.RegisterForEventValidation(textbox.UniqueID);
                    WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-UserPanel", "");
                    Extender.WriteTextBox(writer, false, changePwd.LabelStyle.CssClass, changePwd.UserNameLabelText, changePwd.TextBoxStyle.CssClass, changePwd.ChangePasswordTemplateContainer.ID + "_UserName", changePwd.UserName);
                    WebControlAdapterExtender.WriteRequiredFieldValidator(writer, changePwd.ChangePasswordTemplateContainer.FindControl("UserNameRequired") as RequiredFieldValidator, changePwd.ValidatorTextStyle.CssClass, "UserName", changePwd.UserNameRequiredErrorMessage);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
        }

        private void WritePasswordPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            TextBox textbox = changePwd.ChangePasswordTemplateContainer.FindControl("CurrentPassword") as TextBox;
            if (textbox != null)
            {
                Page.ClientScript.RegisterForEventValidation(textbox.UniqueID);
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-PasswordPanel", "");
                Extender.WriteTextBox(writer, true, changePwd.LabelStyle.CssClass, changePwd.PasswordLabelText, changePwd.TextBoxStyle.CssClass, changePwd.ChangePasswordTemplateContainer.ID + "_CurrentPassword", "");
                WebControlAdapterExtender.WriteRequiredFieldValidator(writer, changePwd.ChangePasswordTemplateContainer.FindControl("CurrentPasswordRequired") as RequiredFieldValidator, changePwd.ValidatorTextStyle.CssClass, "CurrentPassword", changePwd.PasswordRequiredErrorMessage);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteNewPasswordPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            TextBox textbox = changePwd.ChangePasswordTemplateContainer.FindControl("NewPassword") as TextBox;
            if (textbox != null)
            {
                Page.ClientScript.RegisterForEventValidation(textbox.UniqueID);
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-NewPasswordPanel", "");
                Extender.WriteTextBox(writer, true, changePwd.LabelStyle.CssClass, changePwd.NewPasswordLabelText, changePwd.TextBoxStyle.CssClass, changePwd.ChangePasswordTemplateContainer.ID + "_NewPassword", "");
                WebControlAdapterExtender.WriteRequiredFieldValidator(writer, changePwd.ChangePasswordTemplateContainer.FindControl("NewPasswordRequired") as RequiredFieldValidator, changePwd.ValidatorTextStyle.CssClass, "NewPassword", changePwd.NewPasswordRequiredErrorMessage);
                WebControlAdapterExtender.WriteRegularExpressionValidator(writer, changePwd.ChangePasswordTemplateContainer.FindControl("RegExpValidator") as RegularExpressionValidator, changePwd.ValidatorTextStyle.CssClass, "NewPassword", changePwd.NewPasswordRegularExpressionErrorMessage, changePwd.NewPasswordRegularExpression);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteConfirmNewPasswordPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            TextBox textbox = changePwd.ChangePasswordTemplateContainer.FindControl("ConfirmNewPassword") as TextBox;
            if (textbox != null)
            {
                Page.ClientScript.RegisterForEventValidation(textbox.UniqueID);
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-ConfirmNewPasswordPanel", "");
                Extender.WriteTextBox(writer, true, changePwd.LabelStyle.CssClass, changePwd.ConfirmNewPasswordLabelText, changePwd.TextBoxStyle.CssClass, changePwd.ChangePasswordTemplateContainer.ID + "_ConfirmNewPassword", "");
                WebControlAdapterExtender.WriteRequiredFieldValidator(writer, changePwd.ChangePasswordTemplateContainer.FindControl("ConfirmNewPasswordRequired") as RequiredFieldValidator, changePwd.ValidatorTextStyle.CssClass, "ConfirmNewPassword", changePwd.ConfirmPasswordRequiredErrorMessage);
                WebControlAdapterExtender.WriteCompareValidator(writer, changePwd.ChangePasswordTemplateContainer.FindControl("NewPasswordCompare") as CompareValidator, changePwd.ValidatorTextStyle.CssClass, "ConfirmNewPassword", changePwd.ConfirmPasswordCompareErrorMessage, "NewPassword");
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteSubmitPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-SubmitPanel", "");

            string id = "ChangePassword";
            id += (changePwd.ChangePasswordButtonType == ButtonType.Button) ? "Push" : "";
            string idWithType = WebControlAdapterExtender.MakeIdWithButtonType(id, changePwd.ChangePasswordButtonType);
            Control btn = changePwd.ChangePasswordTemplateContainer.FindControl(idWithType);
            if (btn != null)
            {
                Page.ClientScript.RegisterForEventValidation(btn.UniqueID);

                PostBackOptions options = new PostBackOptions(btn, "", "", false, false, false, true, true, changePwd.UniqueID);
                string javascript = "javascript:" + Page.ClientScript.GetPostBackEventReference(options);
                javascript = Page.Server.HtmlEncode(javascript);

                Extender.WriteSubmit(writer, changePwd.ChangePasswordButtonType, changePwd.ChangePasswordButtonStyle.CssClass, changePwd.ChangePasswordTemplateContainer.ID + "_" + id, changePwd.ChangePasswordButtonImageUrl, javascript, changePwd.ChangePasswordButtonText);
            }

            id = "Cancel";
            id += (changePwd.ChangePasswordButtonType == ButtonType.Button) ? "Push" : "";
            idWithType = WebControlAdapterExtender.MakeIdWithButtonType(id, changePwd.CancelButtonType);
            btn = changePwd.ChangePasswordTemplateContainer.FindControl(idWithType);
            if (btn != null)
            {
                Page.ClientScript.RegisterForEventValidation(btn.UniqueID);
                Extender.WriteSubmit(writer, changePwd.CancelButtonType, changePwd.CancelButtonStyle.CssClass, changePwd.ChangePasswordTemplateContainer.ID + "_" + id, changePwd.CancelButtonImageUrl, "", changePwd.CancelButtonText);
            }

            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        private void WriteCreateUserPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if ((!String.IsNullOrEmpty(changePwd.CreateUserUrl)) || (!String.IsNullOrEmpty(changePwd.CreateUserText)))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-CreateUserPanel", "");
                WebControlAdapterExtender.WriteImage(writer, changePwd.CreateUserIconUrl, "Create user");
                WebControlAdapterExtender.WriteLink(writer, changePwd.HyperLinkStyle.CssClass, changePwd.CreateUserUrl, "Create user", changePwd.CreateUserText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WritePasswordRecoveryPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if ((!String.IsNullOrEmpty(changePwd.PasswordRecoveryUrl)) || (!String.IsNullOrEmpty(changePwd.PasswordRecoveryText)))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-PasswordRecoveryPanel", "");
                WebControlAdapterExtender.WriteImage(writer, changePwd.PasswordRecoveryIconUrl, "Password recovery");
                WebControlAdapterExtender.WriteLink(writer, changePwd.HyperLinkStyle.CssClass, changePwd.PasswordRecoveryUrl, "Password recovery", changePwd.PasswordRecoveryText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        /////////////////////////////////////////////////////////
        // Step 2: success
        /////////////////////////////////////////////////////////

        private void WriteSuccessTitlePanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (!String.IsNullOrEmpty(changePwd.SuccessTitleText))
            {
                string className = (changePwd.TitleTextStyle != null) && (!String.IsNullOrEmpty(changePwd.TitleTextStyle.CssClass)) ? changePwd.TitleTextStyle.CssClass + " " : "";
                className += "AspNet-ChangePassword-SuccessTitlePanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className, "");
                WebControlAdapterExtender.WriteSpan(writer, "", changePwd.SuccessTitleText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteSuccessTextPanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            if (!String.IsNullOrEmpty(changePwd.SuccessText))
            {
                string className = (changePwd.SuccessTextStyle != null) && (!String.IsNullOrEmpty(changePwd.SuccessTextStyle.CssClass)) ? changePwd.SuccessTextStyle.CssClass + " " : "";
                className += "AspNet-ChangePassword-SuccessTextPanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className, "");
                WebControlAdapterExtender.WriteSpan(writer, "", changePwd.SuccessText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteContinuePanel(HtmlTextWriter writer, ChangePassword changePwd)
        {
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-ChangePassword-ContinuePanel", "");

            string id = "Continue";
            id += (changePwd.ChangePasswordButtonType == ButtonType.Button) ? "Push" : "";
            string idWithType = WebControlAdapterExtender.MakeIdWithButtonType(id, changePwd.ContinueButtonType);
            Control btn = changePwd.SuccessTemplateContainer.FindControl(idWithType);
            if (btn != null)
            {
                Page.ClientScript.RegisterForEventValidation(btn.UniqueID);
                Extender.WriteSubmit(writer, changePwd.ContinueButtonType, changePwd.ContinueButtonStyle.CssClass, changePwd.SuccessTemplateContainer.ID + "_" + id, changePwd.ContinueButtonImageUrl, "", changePwd.ContinueButtonText);
            }

            WebControlAdapterExtender.WriteEndDiv(writer);
        }
    }

    public class ChangePasswordCommandBubbler : Control
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (Page.IsPostBack)
            {
                ChangePassword changePassword = NamingContainer as ChangePassword;
                if (changePassword != null)
                {
                    Control container = changePassword.ChangePasswordTemplateContainer;
                    if (container != null)
                    {
                        CommandEventArgs cmdArgs = null;
                        String[] prefixes = { "ChangePassword", "Cancel", "Continue" };
                        String[] postfixes = { "PushButton", "Image", "Link" };
                        foreach (string prefix in prefixes)
                        {
                            foreach (string postfix in postfixes)
                            {
                                string id = prefix + postfix;
                                Control ctrl = container.FindControl(id);
                                if ((ctrl != null) && (!String.IsNullOrEmpty(Page.Request.Params.Get(ctrl.UniqueID))))
                                {
                                    switch (prefix)
                                    {
                                        case "ChangePassword":
                                            cmdArgs = new CommandEventArgs(ChangePassword.ChangePasswordButtonCommandName, this);
                                            break;
                                        case "Cancel":
                                            cmdArgs = new CommandEventArgs(ChangePassword.CancelButtonCommandName, this);
                                            break;
                                        case "Continue":
                                            cmdArgs = new CommandEventArgs(ChangePassword.ContinueButtonCommandName, this);
                                            break;
                                    }
                                    break;
                                }
                            }
                            if (cmdArgs != null)
                            {
                                break;
                            }
                        }

                        if ((cmdArgs != null) && (cmdArgs.CommandName == ChangePassword.ChangePasswordButtonCommandName))
                        {
                            Page.Validate();
                            if (!Page.IsValid)
                            {
                                cmdArgs = null;
                            }
                        }

                        if (cmdArgs != null)
                        {
                            RaiseBubbleEvent(this, cmdArgs);
                        }
                    }
                }
            }
        }
    }
}
