using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace mojoPortal.Web.Editor;

public class EditorControl : Panel
{
	private static readonly ILog log = LogManager.GetLogger(typeof(EditorControl));
	private EditorProvider editorProvider = null;
	private string providerName = "CKEditorProvider";
	private string failsafeProviderName = "CKEditorProvider";

	public IWebEditor WebEditor { get; private set; }

	public string ScriptBaseUrl { get; set; } = "~/ClientScript";

	public string Text
	{
		get { return WebEditor.Text; }
		set { WebEditor.Text = value; }
	}

	/// <summary>
	/// This should be set in Page PreInit event
	/// </summary>
	public string ProviderName
	{
		get { return providerName; }
		set
		{
			if (HttpContext.Current == null) { return; }
			if (Site != null && Site.DesignMode)
			{
				//this seem dumb
			}
			else
			{
				if (value != providerName || editorProvider == null)
				{
					providerName = value;
					SetupProvider();
				}
			}
		}
	}

	public EditorProvider Provider
	{
		get { return editorProvider; }
	}

	protected override void OnInit(EventArgs e)
	{
		if (HttpContext.Current == null) { return; }
		base.OnInit(e);
		DoInit();
	}

	private void DoInit()
	{
		Page.RegisterRequiresControlState(this);
		// an exception always happens here in design mode
		// this try is just to fix the display in design view in VS
		if (editorProvider == null)
		{
			SetupProvider();
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (providerName != editorProvider.Name)
		{
			SetupProvider();
		}
	}

	private void SetupProvider()
	{
		try
		{
			if (EditorManager.Providers[providerName] is not null)
			{
				editorProvider = EditorManager.Providers[providerName];
			}
			else
			{
				editorProvider = EditorManager.Providers[failsafeProviderName];
			}
			WebEditor = editorProvider.GetEditor();
			WebEditor.ControlID = $"{ID}innerEditor";

			WebEditor.SiteRoot = Page.ResolveUrl("~/");
			WebEditor.ScriptBaseUrl = Page.ResolveUrl(ScriptBaseUrl);

			Controls.Clear();
			Controls.Add(WebEditor.GetEditorControl());
		}
		catch (TypeInitializationException ex)
		{
			log.Error(ex);
		}
	}

	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current is null)
		{
			writer.Write("[" + ID + "]");
			return;
		}

		base.Render(writer);
	}
}