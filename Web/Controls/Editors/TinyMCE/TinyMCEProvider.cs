using System.Collections.Specialized;

namespace mojoPortal.Web.Editor
{
    public class TinyMCEProvider : EditorProvider
    {
        public override IWebEditor GetEditor()
        {
            return new TinyMceEditorAdapter();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
            // don't read anything from config
            // here as this would raise an error under Medium Trust
        }
    }
}
