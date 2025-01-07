using mojoPortal.Web.Controls.Captcha;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls;

public class SimpleMathCaptchaControl : BaseValidator
{
	#region Constructors

	public SimpleMathCaptchaControl()
	{
		EnsureControls();
	}

	#endregion


	public string ResourceFile
	{
		get => ViewState["ResourceFile"] is not null ? (string)ViewState["ResourceFile"] : "Resource";
		set => ViewState["ResourceFile"] = value;
	}

	public string ResourceKey
	{
		get => ViewState["ResourceKey"] is not null ? (string)ViewState["ResourceKey"] : "SimpleMatchCaptchaControlInstructions";
		set => ViewState["ResourceKey"] = value;
	}

	public new bool IsValid => SpamPreventionQuestion is not null && SpamPreventionQuestion.IsCorrectAnswer(txtAnswerInput.Text);

	public override short TabIndex { get; set; } = 0;


	protected override bool EvaluateIsValid() => SpamPreventionQuestion is not null && SpamPreventionQuestion.IsCorrectAnswer(txtAnswerInput.Text);


	#region Control Declarations

	protected Label lblInstructions = null;
	protected Literal litBefore = null;
	protected TextBox txtAnswerInput = null;
	protected Literal litQuestionExpression = null;
	protected Literal litAfter = null;


	public SimpleMathQuestion SpamPreventionQuestion
	{
		get => ViewState["SpamPreventionQuestion"] is not null ? (SimpleMathQuestion)ViewState["SpamPreventionQuestion"] : null;
		set => ViewState["SpamPreventionQuestion"] = value;
	}

	#endregion


	protected override void OnInit(EventArgs e)
	{
		EnsureControls();

		base.OnInit(e);
	}


	private void EnsureControls()
	{
		if (txtAnswerInput is not null)
		{
			return;
		}

		litQuestionExpression = new Literal();
		txtAnswerInput = new TextBox
		{
			TabIndex = TabIndex,
			MaxLength = 10,
			CssClass = CssClass,
		};
		txtAnswerInput.ID = txtAnswerInput.UniqueID;
		lblInstructions = new Label
		{
			AssociatedControlID = txtAnswerInput.ID
		};

		litBefore = new Literal { Text = "<div>" };
		litAfter = new Literal { Text = "</div>" };

		Controls.Add(lblInstructions);
		Controls.Add(litBefore);
		Controls.Add(litQuestionExpression);
		Controls.Add(txtAnswerInput);
		Controls.Add(litAfter);
	}


	protected override void OnPreRender(EventArgs e)
	{
		ControlToValidate = txtAnswerInput.ID;
		base.OnPreRender(e);

		Localize();
		lblInstructions.AssociatedControlID = txtAnswerInput.ID;

		SpamPreventionQuestion ??= new SimpleMathQuestion();
		litQuestionExpression.Text = $"""<span class="smq__question-expression">{SpamPreventionQuestion.QuestionExpression}</span> """;
	}


	private void Localize()
	{
		try
		{
			var resource = HttpContext.GetGlobalResourceObject(ResourceFile, ResourceKey);

			if (resource is not null)
			{
				lblInstructions.Text = resource.ToString();
			}
			else
			{
				lblInstructions.Text = "Solve this to prove that you are not a robot:";
			}
		}
		catch { }
	}


	protected override void Render(HtmlTextWriter writer)
	{
		if (Site is not null && Site.DesignMode)
		{
			// render to the designer
			txtAnswerInput.RenderControl(writer);
			writer.Write($"[{ID}]");
		}
		else
		{
			// render to the response stream
			base.RenderContents(writer);
		}
	}


	protected override void CreateChildControls()
	{
		EnsureControls();
	}
}
