// Author:		        
// Created:            2007-08-16
// Last Modified:      2014-05-22
// 
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Globalization;
using mojoPortal.Web.Controls.Captcha;

namespace mojoPortal.Web.Controls
{
    
    public class SimpleMathCaptchaControl : BaseValidator 
    {
        #region Constructors

        public SimpleMathCaptchaControl()
		{
			EnsureControls();

            this.txtAnswerInput.ID = this.txtAnswerInput.UniqueID;
            this.txtAnswerInput.MaxLength = 10;
            this.txtAnswerInput.CssClass = this.CssClass;
            this.lblInstructions.AssociatedControlID = this.txtAnswerInput.ID;
		
            
            
		}

		#endregion

        public string ResourceFile
        {
            get { return (ViewState["ResourceFile"] != null ? (string)ViewState["ResourceFile"] : "Resource"); }
            set { ViewState["ResourceFile"] = value; }
        }

        public string ResourceKey
        {
            get { return (ViewState["ResourceKey"] != null ? (string)ViewState["ResourceKey"] : "SimpleMatchCaptchaControlInstructions"); }
            set { ViewState["ResourceKey"] = value; }
        }

        public bool IsValid
        {
            get
            {
                if (SpamPreventionQuestion != null)
                {

                    return SpamPreventionQuestion.IsCorrectAnswer(txtAnswerInput.Text);

                }

                return false;

            }

        }
		public override short TabIndex { get; set; } = 0;
		protected override bool EvaluateIsValid()
        {
            if (SpamPreventionQuestion != null)
            {

                return SpamPreventionQuestion.IsCorrectAnswer(txtAnswerInput.Text);

            }

            return false;
        }
        

		#region Control Declarations

		protected TextBox txtAnswerInput = null;
        protected Literal litQuestionExpression = null;
        protected Label lblInstructions = null;
        protected Literal space;
        

        public SimpleMathQuestion SpamPreventionQuestion
        {
            get { return (ViewState["SpamPreventionQuestion"] != null ? (SimpleMathQuestion)ViewState["SpamPreventionQuestion"] : null); }
            set { ViewState["SpamPreventionQuestion"] = value; }
        }
		

		#endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            EnsureControls();

            
        }

        private void EnsureControls()
        {
            if (txtAnswerInput != null) { return; }

            litQuestionExpression = new Literal();
            txtAnswerInput = new TextBox();
            lblInstructions = new Label();
			txtAnswerInput.TabIndex = this.TabIndex;

            this.Controls.Add(this.litQuestionExpression);
            this.Controls.Add(this.txtAnswerInput);
            space = new Literal();
            space.Text = "&nbsp;";
            this.Controls.Add(space);
            this.Controls.Add(this.lblInstructions);

            
        }

        protected override void OnPreRender(EventArgs e)
        {
            ControlToValidate = txtAnswerInput.ID;
            base.OnPreRender(e);

            Localize();
            if (SpamPreventionQuestion == null) SpamPreventionQuestion = new SimpleMathQuestion();

            litQuestionExpression.Text = SpamPreventionQuestion.QuestionExpression;

        }

       
        private void Localize()
        {
         
            try
            {
                object resource = HttpContext.GetGlobalResourceObject(
                    this.ResourceFile, this.ResourceKey);

                if (resource != null)
                {
                    this.lblInstructions.Text = "<strong>" 
                        + resource.ToString() + "</strong>";
                }
                else
                {
                    this.lblInstructions.Text = "<strong>Solve This To Prove You are a Real Person, not a SPAM script.</strong>";
                }
            }
            catch { }

            

        }

        
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // render to the designer
                this.txtAnswerInput.RenderControl(writer);
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                // render to the response stream
                base.RenderContents(writer);
                //litQuestionExpression.RenderControl(writer);
                //txtAnswerInput.RenderControl(writer);
                //space.RenderControl(writer);
                //lblInstructions.RenderControl(writer);
            }
        }

        protected override void CreateChildControls()
        {
            EnsureControls();

        }

    }
}
