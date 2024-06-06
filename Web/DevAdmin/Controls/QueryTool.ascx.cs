using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Extensions;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.DevAdmin.Controls;

/// <summary>
/// A Quick and dirty tool for querying the database, be very careful with this, you can delete site data with it.
/// </summary>
public partial class QueryTool : UserControl
{
	private SiteSettings siteSettings = null;
	private string overrideConnectionString = string.Empty;
	protected string SiteRoot = string.Empty;
	private SavedQueryRespository repository = new();
	private string dbPlatform = "MSSQL";
	private Guid queryId = Guid.Empty;
	private SiteUser currentUser = null;

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();

		if (
			siteSettings is null
			|| !siteSettings.IsServerAdminSite
			|| !WebUser.IsAdmin
			|| !WebConfigSettings.EnableQueryTool
			|| !WebConfigSettings.EnableDeveloperMenuInAdminMenu
			)
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		PopulateLabels();
		SetupScript();
		if (!IsPostBack)
		{
			PopulateControls();
		}
	}

	private void PopulateControls()
	{
		BindTableList();
		BindQueryList();
		if (queryId != Guid.Empty)
		{
			SavedQuery q = repository.Fetch(queryId);
			if (q != null)
			{
				txtQuery.Text = q.Statement;
			}
		}
	}

	private void BindQueryList()
	{
		var queryList = repository.GetAll();
		ddQueries.DataSource = queryList;
		ddQueries.DataBind();

		if (queryId != Guid.Empty)
		{
			ListItem item = ddQueries.Items.FindByValue(queryId.ToString());
			if (item is not null)
			{
				ddQueries.ClearSelection();
				item.Selected = true;
			}
		}
	}

	private void BindTableList()
	{
		string tableSelectSql = string.Empty;

		tableSelectSql = dbPlatform switch
		{
			"MySQL" => WebConfigSettings.QueryToolMySqlTableSelectSql,
			"pgsql" => WebConfigSettings.QueryToolPgSqlTableSelectSql,
			"SQLite" => WebConfigSettings.QueryToolSqliteTableSelectSql,
			_ => WebConfigSettings.QueryToolMsSqlTableSelectSql,
		};

		if (tableSelectSql.Length == 0)
		{
			pnlTables.Visible = false;
			return;
		}

		using IDataReader reader = DatabaseHelper.GetReader(overrideConnectionString, tableSelectSql);
		ddTables.DataSource = reader;
		ddTables.DataBind();
	}

	void btnExecuteNonQuery_Click(object sender, EventArgs e)
	{
		if (txtQuery.Text.Trim().Length == 0)
		{
			lblError.Text = DevTools.NoQueryWarning;
			return;
		}

		try
		{
			int rowsAffected = DatabaseHelper.ExecteNonQuery(overrideConnectionString, txtQuery.Text);
			lblError.Text = $"{rowsAffected.ToInvariantString()} rows affected";

		}
		catch (Exception ex)
		{
			lblError.Text = ex.ToString();
		}
	}

	void btnExecuteQuery_Click(object sender, EventArgs e)
	{
		lblError.Text = string.Empty;

		if (txtQuery.Text.Trim().Length == 0)
		{
			lblError.Text = DevTools.NoQueryWarning;
			return;
		}

		try
		{
			using IDataReader reader = DatabaseHelper.GetReader(overrideConnectionString, txtQuery.Text);
			grdResults.DataSource = reader;
			grdResults.DataBind();

		}
		catch (Exception ex)
		{
			lblError.Text = ex.ToString();
		}
	}

	void btnExport_Click(object sender, EventArgs e)
	{
		lblError.Text = string.Empty;

		if (string.IsNullOrWhiteSpace(txtQuery.Text))
		{
			lblError.Text = DevTools.NoQueryWarning;
			return;
		}

		try
		{
			var dt = new DataTable();

			using (IDataReader reader = DatabaseHelper.GetReader(overrideConnectionString, txtQuery.Text))
			{
				dt.Load(reader);
			}

			string fileName = $"data-export-{DateTimeHelper.GetDateTimeStringForFileName()}.csv";

			ExportHelper.ExportDataTableToCsv(HttpContext.Current, dt, fileName);
		}
		catch (Exception ex)
		{
			lblError.Text = ex.ToString();
		}
	}

	void btnSave_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(txtQuery.Text))
		{
			lblError.Text = DevTools.NoQueryWarning;
			return;
		}

		if (string.IsNullOrWhiteSpace(txtQueryName.Text))
		{
			lblError.Text = DevTools.QueryNameRequired;
			return;

		}

		SavedQuery q = repository.Fetch(txtQueryName.Text);
		q ??= new SavedQuery();
		q.Statement = txtQuery.Text;
		q.Name = txtQueryName.Text;

		if (currentUser != null)
		{
			q.LastModBy = currentUser.UserGuid;
		}

		q.LastModUtc = DateTime.UtcNow;
		repository.Save(q);
		RedirectToQuery(q.Id.ToString());
	}

	void btnDelete_Click(object sender, EventArgs e)
	{
		if ((ddQueries.SelectedIndex != -1) && (ddQueries.SelectedValue.Length == 36))
		{
			var qid = new Guid(ddQueries.SelectedValue);
			repository.Delete(qid);
		}

		RedirectToQuery(Guid.Empty.ToString());
	}

	void btnLoadQuery_Click(object sender, EventArgs e)
	{
		if ((ddQueries.SelectedIndex != -1) && (ddQueries.SelectedValue.Length == 36))
		{
			RedirectToQuery(ddQueries.SelectedValue);
		}
	}

	private void RedirectToQuery(string queryId)
	{
		WebUtils.SetupRedirect(this, $"{SiteRoot}/DevAdmin/QueryTool.aspx?qid={queryId}");
	}

	private void SetupScript()
	{
		var script = new StringBuilder();
		script.Append("\n<script data-loader=\"QueryTool\">\n");

		script.Append("function SelectTable(){");
		script.Append($"var table = $('#{ddTables.ClientID}').val(); ");

		switch (dbPlatform)
		{
			case "MySQL":
			case "pgsql":
			case "SQLite":
			case "FirebirdSql":
				script.Append("var terminus = ';'; ");
				break;

			case "MSSQL":
			case "SqlAzure":
			default:
				script.Append("var terminus = ''; ");
				break;
		}

		script.Append($"editAreaLoader.setValue('{txtQuery.ClientID}','SELECT * FROM ' + table + terminus); ");
		script.Append("}");
		script.Append("\n</script>\n");

		this.Page.ClientScript.RegisterClientScriptBlock(
			typeof(Page),
			this.UniqueID,
			script.ToString());

		btnSelectTable.Attributes.Add("onclick", "SelectTable(); return false;");
	}

	private void PopulateLabels()
	{
		litHeading.Text = string.Format(CultureInfo.InvariantCulture, DevTools.QueryToolHeadingFormat, dbPlatform);
		litWarning.Text = DevTools.QueryToolWarning;
		lblError.Text = string.Empty;

		btnSelectTable.Text = DevTools.SelectButton;
		btnLoadQuery.Text = DevTools.LoadSavedQuery;
		btnDelete.Text = DevTools.DeleteSavedQuery;
		btnExecuteQuery.Text = DevTools.ExecuteQuery;
		btnExecuteNonQuery.Text = DevTools.ExecuteNonQuery;
		btnExport.Text = DevTools.ExportQueryDataAsCSV;
		btnSave.Text = DevTools.SaveQueryAs;
		lnkClear.Text = DevTools.Clear;
	}

	private void LoadSettings()
	{
		siteSettings = CacheHelper.GetCurrentSiteSettings();
		SiteRoot = SiteUtils.GetNavigationSiteRoot();
		lnkClear.NavigateUrl = $"{SiteRoot}/DevAdmin/QueryTool.aspx";
		txtQuery.Language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
		dbPlatform = DatabaseHelper.DBPlatform();
		queryId = WebUtils.ParseGuidFromQueryString("qid", queryId);
		currentUser = SiteUtils.GetCurrentSiteUser();
		overrideConnectionString = WebConfigSettings.QueryToolOverrideConnectionString;
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		btnExecuteNonQuery.Click += new EventHandler(btnExecuteNonQuery_Click);
		btnExecuteQuery.Click += new EventHandler(btnExecuteQuery_Click);
		btnLoadQuery.Click += new EventHandler(btnLoadQuery_Click);
		btnDelete.Click += new EventHandler(btnDelete_Click);
		btnSave.Click += new EventHandler(btnSave_Click);
		btnExport.Click += new EventHandler(btnExport_Click);
	}
}