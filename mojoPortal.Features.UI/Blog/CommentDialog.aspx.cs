using mojoPortal.Business;
using mojoPortal.Web.Framework;
using System;
using System.Collections;


namespace mojoPortal.Web.BlogUI;

public partial class CommentDialog : mojoDialogBasePage
{
    protected int pageId = -1;
    protected int moduleId = -1;
    protected int itemId = -1;
    protected Guid commentGuid = Guid.Empty;
    private Blog blog = null;
    private Module module = null;
    private Hashtable moduleSettings = null;
    private BlogConfiguration config = null;
    private Comment comment = null;
    private CommentRepository commentRepository = null;
    private SiteUser currentUser = null;
    private bool userCanEdit = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadSettings();
        if (SiteUtils.SslIsAvailable() && (siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl))
        {
            SiteUtils.ForceSsl();
        }
        else
        {
            SiteUtils.ClearSsl();
        }
        if (
            (!userCanEdit)
            ||(commentGuid == Guid.Empty)||(blog == null))
        {
            SiteUtils.RedirectToAccessDeniedPage(this);
            return;
        }
    }

    private bool UserCanEditComment()
    {
        if (comment == null) { return false; }

        if (UserCanEditModule(moduleId, Blog.FeatureGuid)) { return true; }

        if (config.RequireApprovalForComments && (comment.ModerationStatus == Comment.ModerationApproved)) { return false; } // no edits by user after moderation

        if ((currentUser != null) && (comment.UserGuid == currentUser.UserGuid))
        {
            if ((!config.RequireApprovalForComments) || (comment.ModerationStatus == Comment.ModerationPending))
            {
                TimeSpan t = DateTime.UtcNow - comment.CreatedUtc;
                if (t.Minutes < config.AllowedEditMinutesForUnModeratedPosts)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void LoadSettings()
    {
        pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
        moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
        itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);
        commentGuid = WebUtils.ParseGuidFromQueryString("c", commentGuid);
        if (commentGuid == Guid.Empty) { return; }

        blog = new Blog(itemId);
        module = GetModule(moduleId, Blog.FeatureGuid);
        commentRepository = new CommentRepository();

        if (blog.ModuleId != module.ModuleId)
        {
            blog = null;
            module = null;
            return;
        }

        comment = commentRepository.Fetch(commentGuid);
        if ((comment.ContentGuid != blog.BlogGuid)||(comment.ModuleGuid != module.ModuleGuid))
        {
            blog = null;
            module = null;
            return;
        }

        moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

        config = new BlogConfiguration(moduleSettings);

        currentUser = SiteUtils.GetCurrentSiteUser();

        userCanEdit = UserCanEditComment();

        commentEditor.SiteGuid = SiteInfo.SiteGuid;
        commentEditor.SiteId = SiteInfo.SiteId;
        commentEditor.SiteRoot = SiteRoot;
        commentEditor.CommentsClosed = false;
        commentEditor.ContentGuid = blog.BlogGuid;
        commentEditor.FeatureGuid = Blog.FeatureGuid;
        commentEditor.ModuleGuid = module.ModuleGuid;
        commentEditor.RequireCaptcha = false;
        commentEditor.UserCanModerate = userCanEdit;
        commentEditor.CurrentUser = currentUser;
        commentEditor.UserComment = comment;
        commentEditor.ShowRememberMe = false;
    }

    override protected void OnInit(EventArgs e)
    {
        Load += new EventHandler(this.Page_Load);
        base.OnInit(e);
    }
}