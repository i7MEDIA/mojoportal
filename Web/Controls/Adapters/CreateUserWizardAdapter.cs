/// Originally from Microsoft, Sample released under 
/// MS Permissive License 
/// http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
/// (see license-ms-permissive.txt in the root of the solution)
/// 
/// Original source urls:
/// http://www.asp.net/cssadapters/
/// http://www.asp.net/CSSAdapters/WhitePaper.aspx
/// 
/// 
/// 
/// Added:      2007/05/07 
/// Modified:   

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
    public class CreateUserWizardAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
    {
        private enum State
        {
            CreateUser,
            Failed,
            Success
        }
        State _state = State.CreateUser;
        string _currentErrorText = "";

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

        private MembershipProvider WizardMembershipProvider
        {
            get
            {
                MembershipProvider provider = Membership.Provider;
                CreateUserWizard wizard = Control as CreateUserWizard;
                if ((wizard != null) && (!String.IsNullOrEmpty(wizard.MembershipProvider)) && (Membership.Providers[wizard.MembershipProvider] != null))
                {
                    provider = Membership.Providers[wizard.MembershipProvider];
                }
                return provider;
            }
        }

        public CreateUserWizardAdapter()
        {
            _state = State.CreateUser;
            _currentErrorText = "";
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PROTECTED        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CreateUserWizard wizard = Control as CreateUserWizard;
            if (Extender.AdapterEnabled && (wizard != null))
            {
                RegisterScripts();
                wizard.CreatedUser += OnCreatedUser;
                wizard.CreateUserError += OnCreateUserError;
                _state = State.CreateUser;
                _currentErrorText = "";
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            CreateUserWizard wizard = Control as CreateUserWizard;
            if (wizard != null)
            {
                TemplatedWizardStep activeStep = wizard.ActiveStep as TemplatedWizardStep;
                if (activeStep != null)
                {
                    if ((activeStep.ContentTemplate != null) && (activeStep.Controls.Count == 1))
                    {
                        Control container = activeStep.ContentTemplateContainer;
                        if (container != null)
                        {
                            container.Controls.Clear();
                            activeStep.ContentTemplate.InstantiateIn(container);
                            container.DataBind();
                        }
                    }
                }
            }
        }

        protected void OnCreatedUser(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            _state = State.Success;
            _currentErrorText = "";
        }

        protected void OnCreateUserError(object sender, CreateUserErrorEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            _state = State.Failed;
            _currentErrorText = "An error has occurred. Please try again.";

            CreateUserWizard wizard = Control as CreateUserWizard;
            if (wizard != null)
            {
                _currentErrorText = wizard.UnknownErrorMessage;
                switch (e.CreateUserError)
                {
                    case MembershipCreateStatus.DuplicateEmail:
                        _currentErrorText = wizard.DuplicateEmailErrorMessage;
                        break;
                    case MembershipCreateStatus.DuplicateUserName:
                        _currentErrorText = wizard.DuplicateUserNameErrorMessage;
                        break;
                    case MembershipCreateStatus.InvalidAnswer:
                        _currentErrorText = wizard.InvalidAnswerErrorMessage;
                        break;
                    case MembershipCreateStatus.InvalidEmail:
                        _currentErrorText = wizard.InvalidEmailErrorMessage;
                        break;
                    case MembershipCreateStatus.InvalidPassword:
                        _currentErrorText = wizard.InvalidPasswordErrorMessage;
                        break;
                    case MembershipCreateStatus.InvalidQuestion:
                        _currentErrorText = wizard.InvalidQuestionErrorMessage;
                        break;
                }
            }
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderBeginTag(writer, "AspNet-CreateUserWizard");
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
                CreateUserWizard wizard = Control as CreateUserWizard;
                if (wizard != null)
                {
                    TemplatedWizardStep activeStep = wizard.ActiveStep as TemplatedWizardStep;
                    if (activeStep != null)
                    {
                        if (activeStep.ContentTemplate != null)
                        {
                            activeStep.RenderControl(writer);

                            if (wizard.CreateUserStep.Equals(activeStep))
                            {
                                WriteCreateUserButtonPanel(writer, wizard);
                            }
                            // Might need to add logic here to render nav buttons for other kinds of
                            // steps (besides the CreateUser step, which we handle above).
                        }
                        else if (wizard.CreateUserStep.Equals(activeStep))
                        {
                            WriteHeaderTextPanel(writer, wizard);
                            WriteStepTitlePanel(writer, wizard);
                            WriteInstructionPanel(writer, wizard);
                            WriteHelpPanel(writer, wizard);
                            WriteUserPanel(writer, wizard);
                            WritePasswordPanel(writer, wizard);
                            WritePasswordHintPanel(writer, wizard); //hbb
                            WriteConfirmPasswordPanel(writer, wizard);
                            WriteEmailPanel(writer, wizard);
                            WriteQuestionPanel(writer, wizard);
                            WriteAnswerPanel(writer, wizard);
                            WriteFinalValidators(writer, wizard);
                            if (_state == State.Failed)
                            {
                                WriteFailurePanel(writer, wizard);
                            }
                            WriteCreateUserButtonPanel(writer, wizard);
                        }
                        else if (wizard.CompleteStep.Equals(activeStep))
                        {
                            WriteStepTitlePanel(writer, wizard);
                            WriteSuccessTextPanel(writer, wizard);
                            WriteContinuePanel(writer, wizard);
                            WriteEditProfilePanel(writer, wizard);
                        }
                        else
                        {
                            System.Diagnostics.Debug.Fail("The adapter isn't equipped to handle a CreateUserWizard with a step that is neither templated nor either the CreateUser step or the Complete step.");
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
        // Step 1: Create user step
        /////////////////////////////////////////////////////////

        private void WriteHeaderTextPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (!String.IsNullOrEmpty(wizard.HeaderText))
            {
                string className = (wizard.HeaderStyle != null) && (!String.IsNullOrEmpty(wizard.HeaderStyle.CssClass)) ? wizard.HeaderStyle.CssClass + " " : "";
                className += "AspNet-CreateUserWizard-HeaderTextPanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className, "");
                WebControlAdapterExtender.WriteSpan(writer, "", wizard.HeaderText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteStepTitlePanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (!String.IsNullOrEmpty(wizard.ActiveStep.Title))
            {
                string className = (wizard.TitleTextStyle != null) && (!String.IsNullOrEmpty(wizard.TitleTextStyle.CssClass)) ? wizard.TitleTextStyle.CssClass + " " : "";
                className += "AspNet-CreateUserWizard-StepTitlePanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className, "");
                WebControlAdapterExtender.WriteSpan(writer, "", wizard.ActiveStep.Title);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteInstructionPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (!String.IsNullOrEmpty(wizard.InstructionText))
            {
                string className = (wizard.InstructionTextStyle != null) && (!String.IsNullOrEmpty(wizard.InstructionTextStyle.CssClass)) ? wizard.InstructionTextStyle.CssClass + " " : "";
                className += "AspNet-CreateUserWizard-InstructionPanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className, "");
                WebControlAdapterExtender.WriteSpan(writer, "", wizard.InstructionText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteFailurePanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            string className = (wizard.ErrorMessageStyle != null) && (!String.IsNullOrEmpty(wizard.ErrorMessageStyle.CssClass)) ? wizard.ErrorMessageStyle.CssClass + " " : "";
            className += "AspNet-CreateUserWizard-FailurePanel";
            WebControlAdapterExtender.WriteBeginDiv(writer, className, "");
            WebControlAdapterExtender.WriteSpan(writer, "", _currentErrorText);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        private void WriteHelpPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if ((!String.IsNullOrEmpty(wizard.HelpPageIconUrl)) || (!String.IsNullOrEmpty(wizard.HelpPageText)))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-HelpPanel", "");
                WebControlAdapterExtender.WriteImage(writer, wizard.HelpPageIconUrl, "Help");
                WebControlAdapterExtender.WriteLink(writer, wizard.HyperLinkStyle.CssClass, wizard.HelpPageUrl, "Help", wizard.HelpPageText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteUserPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            TextBox textBox = wizard.FindControl("CreateUserStepContainer").FindControl("UserName") as TextBox;
            if (textBox != null)
            {
                Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-UserPanel", "");
                Extender.WriteTextBox(writer, false, wizard.LabelStyle.CssClass, wizard.UserNameLabelText, wizard.TextBoxStyle.CssClass, "CreateUserStepContainer_UserName", wizard.UserName);
                WebControlAdapterExtender.WriteRequiredFieldValidator(writer, wizard.FindControl("CreateUserStepContainer").FindControl("UserNameRequired") as RequiredFieldValidator, wizard.ValidatorTextStyle.CssClass, "UserName", wizard.UserNameRequiredErrorMessage);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WritePasswordPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            TextBox textBox = wizard.FindControl("CreateUserStepContainer").FindControl("Password") as TextBox;
            if (textBox != null)
            {
                Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-PasswordPanel", "");
                Extender.WriteTextBox(writer, true, wizard.LabelStyle.CssClass, wizard.PasswordLabelText, wizard.TextBoxStyle.CssClass, "CreateUserStepContainer_Password", "");
                WebControlAdapterExtender.WriteRequiredFieldValidator(writer, wizard.FindControl("CreateUserStepContainer").FindControl("PasswordRequired") as RequiredFieldValidator, wizard.ValidatorTextStyle.CssClass, "Password", wizard.PasswordRequiredErrorMessage);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }


        private void WritePasswordHintPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (!String.IsNullOrEmpty(wizard.PasswordHintText))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-PasswordHintPanel", "");
                WebControlAdapterExtender.WriteSpan(writer, "", wizard.PasswordHintText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteConfirmPasswordPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            TextBox textBox = wizard.FindControl("CreateUserStepContainer").FindControl("ConfirmPassword") as TextBox;
            if (textBox != null)
            {
                Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-ConfirmPasswordPanel", "");
                Extender.WriteTextBox(writer, true, wizard.LabelStyle.CssClass, wizard.ConfirmPasswordLabelText, wizard.TextBoxStyle.CssClass, "CreateUserStepContainer_ConfirmPassword", "");
                WebControlAdapterExtender.WriteRequiredFieldValidator(writer, wizard.FindControl("CreateUserStepContainer").FindControl("ConfirmPasswordRequired") as RequiredFieldValidator, wizard.ValidatorTextStyle.CssClass, "ConfirmPassword", wizard.ConfirmPasswordRequiredErrorMessage);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteEmailPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            TextBox textBox = wizard.FindControl("CreateUserStepContainer").FindControl("Email") as TextBox;
            if (textBox != null)
            {
                Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-EmailPanel", "");
                Extender.WriteTextBox(writer, false, wizard.LabelStyle.CssClass, wizard.EmailLabelText, wizard.TextBoxStyle.CssClass, "CreateUserStepContainer_Email", "");
                WebControlAdapterExtender.WriteRequiredFieldValidator(writer, wizard.FindControl("CreateUserStepContainer").FindControl("EmailRequired") as RequiredFieldValidator, wizard.ValidatorTextStyle.CssClass, "Email", wizard.EmailRequiredErrorMessage);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteQuestionPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if ((WizardMembershipProvider != null) && WizardMembershipProvider.RequiresQuestionAndAnswer)
            {
                TextBox textBox = wizard.FindControl("CreateUserStepContainer").FindControl("Question") as TextBox;
                if (textBox != null)
                {
                    Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

                    WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-QuestionPanel", "");
                    Extender.WriteTextBox(writer, false, wizard.LabelStyle.CssClass, wizard.QuestionLabelText, wizard.TextBoxStyle.CssClass, "CreateUserStepContainer_Question", "");
                    WebControlAdapterExtender.WriteRequiredFieldValidator(writer, wizard.FindControl("CreateUserStepContainer").FindControl("QuestionRequired") as RequiredFieldValidator, wizard.ValidatorTextStyle.CssClass, "Question", wizard.QuestionRequiredErrorMessage);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
        }

        private void WriteAnswerPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if ((WizardMembershipProvider != null) && WizardMembershipProvider.RequiresQuestionAndAnswer)
            {
                TextBox textBox = wizard.FindControl("CreateUserStepContainer").FindControl("Answer") as TextBox;
                if (textBox != null)
                {
                    Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

                    WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-AnswerPanel", "");
                    Extender.WriteTextBox(writer, false, wizard.LabelStyle.CssClass, wizard.AnswerLabelText, wizard.TextBoxStyle.CssClass, "CreateUserStepContainer_Answer", "");
                    WebControlAdapterExtender.WriteRequiredFieldValidator(writer, wizard.FindControl("CreateUserStepContainer").FindControl("AnswerRequired") as RequiredFieldValidator, wizard.ValidatorTextStyle.CssClass, "Answer", wizard.AnswerRequiredErrorMessage);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
        }

        private void WriteFinalValidators(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-FinalValidatorsPanel", "");
            WebControlAdapterExtender.WriteCompareValidator(writer, wizard.FindControl("CreateUserStepContainer").FindControl("PasswordCompare") as CompareValidator, wizard.ValidatorTextStyle.CssClass, "ConfirmPassword", wizard.ConfirmPasswordCompareErrorMessage, "Password");
            WebControlAdapterExtender.WriteRegularExpressionValidator(writer, wizard.FindControl("CreateUserStepContainer").FindControl("PasswordRegExpValidator") as RegularExpressionValidator, wizard.ValidatorTextStyle.CssClass, "Password", wizard.PasswordRegularExpressionErrorMessage, wizard.PasswordRegularExpression);
            WebControlAdapterExtender.WriteRegularExpressionValidator(writer, wizard.FindControl("CreateUserStepContainer").FindControl("EmailRegExpValidator") as RegularExpressionValidator, wizard.ValidatorTextStyle.CssClass, "Email", wizard.EmailRegularExpressionErrorMessage, wizard.EmailRegularExpression);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        private void WriteCreateUserButtonPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            Control btnParentCtrl = wizard.FindControl("__CustomNav0");
            if (btnParentCtrl != null)
            {
                string id = "_CustomNav0_StepNextButton";
                string idWithType = WebControlAdapterExtender.MakeIdWithButtonType("StepNextButton", wizard.CreateUserButtonType);
                Control btn = btnParentCtrl.FindControl(idWithType);
                if (btn != null)
                {
                    Page.ClientScript.RegisterForEventValidation(btn.UniqueID);

                    PostBackOptions options = new PostBackOptions(btn, "", "", false, false, false, true, true, wizard.ID);
                    string javascript = "javascript:" + Page.ClientScript.GetPostBackEventReference(options);
                    javascript = Page.Server.HtmlEncode(javascript);

                    WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-CreateUserButtonPanel", "");

                    Extender.WriteSubmit(writer, wizard.CreateUserButtonType, wizard.CreateUserButtonStyle.CssClass, id, wizard.CreateUserButtonImageUrl, javascript, wizard.CreateUserButtonText);

                    if (wizard.DisplayCancelButton)
                    {
                        Extender.WriteSubmit(writer, wizard.CancelButtonType, wizard.CancelButtonStyle.CssClass, "_CustomNav0_CancelButton", wizard.CancelButtonImageUrl, "", wizard.CancelButtonText);
                    }

                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
        }

        /////////////////////////////////////////////////////////
        // Complete step
        /////////////////////////////////////////////////////////

        private void WriteSuccessTextPanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if (!String.IsNullOrEmpty(wizard.CompleteSuccessText))
            {
                string className = (wizard.CompleteSuccessTextStyle != null) && (!String.IsNullOrEmpty(wizard.CompleteSuccessTextStyle.CssClass)) ? wizard.CompleteSuccessTextStyle.CssClass + " " : "";
                className += "AspNet-CreateUserWizard-SuccessTextPanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className, "");
                WebControlAdapterExtender.WriteSpan(writer, "", wizard.CompleteSuccessText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteContinuePanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            string id = "ContinueButton";
            string idWithType = WebControlAdapterExtender.MakeIdWithButtonType(id, wizard.ContinueButtonType);
            Control btn = wizard.FindControl("CompleteStepContainer").FindControl(idWithType);
            if (btn != null)
            {
                Page.ClientScript.RegisterForEventValidation(btn.UniqueID);
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-ContinuePanel", "");
                Extender.WriteSubmit(writer, wizard.ContinueButtonType, wizard.ContinueButtonStyle.CssClass, "CompleteStepContainer_ContinueButton", wizard.ContinueButtonImageUrl, "", wizard.ContinueButtonText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteEditProfilePanel(HtmlTextWriter writer, CreateUserWizard wizard)
        {
            if ((!String.IsNullOrEmpty(wizard.EditProfileUrl)) || (!String.IsNullOrEmpty(wizard.EditProfileText)))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-CreateUserWizard-EditProfilePanel", "");
                WebControlAdapterExtender.WriteImage(writer, wizard.EditProfileIconUrl, "Edit profile");
                WebControlAdapterExtender.WriteLink(writer, wizard.HyperLinkStyle.CssClass, wizard.EditProfileUrl, "EditProfile", wizard.EditProfileText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }
    }
}
