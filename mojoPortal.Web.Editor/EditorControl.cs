using log4net;
using mojoPortal.Core.Extensions;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor;

public class EditorControl : Panel
{
	#region Fields

	private static readonly ILog _log = LogManager.GetLogger(typeof(EditorControl));

	private const string _failsafeProviderName = "CKEditorProvider";

	#endregion


	#region Properties

	public IWebEditor WebEditor { get; private set; }
	public string ScriptBaseUrl { get; set; } = "~/ClientScript";

	public string Text
	{
		get => ProcessText(WebEditor.Text);
		set => WebEditor.Text = ProcessText(value);
	}

	/// <summary>
	/// This should be set in Page PreInit event
	/// </summary>
	public string ProviderName
	{
		get { return field; }
		set
		{
			if (HttpContext.Current is null)
			{
				return;
			}

			if (Site is not { DesignMode: true })
			{
				if (value != field || Provider is null)
				{
					field = value;
					SetupProvider();
				}
			}
		}
	} = "CKEditorProvider";

	public EditorProvider Provider { get; private set; } = null;

	#endregion


	#region Override Methods

	protected override void OnInit(EventArgs e)
	{
		if (HttpContext.Current is null)
		{
			return;
		}

		base.OnInit(e);

		DoInit();
	}


	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		if (ProviderName != Provider.Name)
		{
			SetupProvider();
		}
	}


	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current is null)
		{
			writer.Write($"[{ID}]");
			return;
		}

		base.Render(writer);
	}

	#endregion


	#region Private Methods

	private void DoInit()
	{
		Page.RegisterRequiresControlState(this);

		// an exception always happens here in design mode
		// this try is just to fix the display in design view in VS
		if (Provider is null)
		{
			SetupProvider();
		}
	}


	private void SetupProvider()
	{
		try
		{
			if (EditorManager.Providers[ProviderName] is not null)
			{
				Provider = EditorManager.Providers[ProviderName];
			}
			else
			{
				Provider = EditorManager.Providers[_failsafeProviderName];
			}

			WebEditor = Provider.GetEditor();
			WebEditor.ControlID = $"{ID}innerEditor";

			WebEditor.SiteRoot = Page.ResolveUrl("~/");
			WebEditor.ScriptBaseUrl = Page.ResolveUrl(ScriptBaseUrl);

			Controls.Clear();
			Controls.Add(WebEditor.GetEditorControl());
		}
		catch (TypeInitializationException ex)
		{
			_log.Error(ex);
		}
	}


	private string ProcessText(string text) =>
		// Prevent non-admins from including script in the content.
		(Context?.User?.Identity?.IsAuthenticated ?? false) &&
		(Context?.User?.IsInRole("Admins") ?? false) ?
			text :
			text?.SanitizeMarkup();

	#endregion
}
