namespace mojoPortal.Web.BlogUI;

/// <summary>
/// Display Settings for Blog Post List feature.
/// Configuration is per skin via the config/plugins/Blog/PostList-display.json file.
/// </summary>
public class BlogPostListDisplaySettings : BasePluginDisplaySettings
{
	public override string FeatureName => nameof(BlogDisplaySettings).Replace("DisplaySettings", string.Empty);
	public override string SubFeatureName => "PostList";
	public BlogPostListDisplaySettings() : base() { }
}
