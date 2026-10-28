<%@ Page Language="C#" %>

<script runat="server">
	protected void Page_Load(object sender, EventArgs e)
	{
		SiteUtils.RedirectToAdminMenu(this);
		return;
	}
</script>
