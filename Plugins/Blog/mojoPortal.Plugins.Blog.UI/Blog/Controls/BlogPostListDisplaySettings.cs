namespace mojoPortal.Web.BlogUI;


public class BlogPostListDisplaySettings : BasePluginDisplaySettings
{
	public override string FeatureName => typeof(BlogDisplaySettings).Name.Replace("DisplaySettings", string.Empty);
	public override string SubFeatureName => "PostList";
	public BlogPostListDisplaySettings() : base() { }
}
