//
// System.Web.UI.WebControls.SiteMapPath.cs
//
// Authors:
//	Lluis Sanchez Gual (lluis@novell.com)
//
// (C) 2005 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
//

// borrowed from the Mono project 2009-05-11 and modified to work in mojoportal by 
// Last Modified 2009-05-11


using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	//[Designer ("System.Web.UI.Design.WebControls.SiteMapPathDesigner, " + Consts.AssemblySystem_Design, "System.ComponentModel.Design.IDesigner")]
	public class SiteMapPath: CompositeControl
	{
		SiteMapProvider provider;
		
		Style currentNodeStyle;
		Style nodeStyle;
		Style pathSeparatorStyle;
		Style rootNodeStyle;
		
		ITemplate currentNodeTemplate;
		ITemplate nodeTemplate;
		ITemplate pathSeparatorTemplate;
		ITemplate rootNodeTemplate;

		static readonly object ItemCreatedEvent = new object();
		static readonly object ItemDataBoundEvent = new object();
		
		public event SiteMapNodeItemEventHandler ItemCreated {
			add { Events.AddHandler (ItemCreatedEvent, value); }
			remove { Events.RemoveHandler (ItemCreatedEvent, value); }
		}
		
		public event SiteMapNodeItemEventHandler ItemDataBound {
			add { Events.AddHandler (ItemDataBoundEvent, value); }
			remove { Events.RemoveHandler (ItemDataBoundEvent, value); }
		}
		
		protected virtual void OnItemCreated (SiteMapNodeItemEventArgs e)
		{
			if (Events != null) {
				SiteMapNodeItemEventHandler eh = (SiteMapNodeItemEventHandler) Events [ItemCreatedEvent];
				if (eh != null) eh (this, e);
			}
		}
		
		protected virtual void OnItemDataBound (SiteMapNodeItemEventArgs e)
		{
			if (Events != null) {
				SiteMapNodeItemEventHandler eh = (SiteMapNodeItemEventHandler) Events [ItemDataBoundEvent];
				if (eh != null) eh (this, e);
			}
		}

        private bool renderContentsOnly = true;

        public bool RenderContentsOnly
        {
            get { return renderContentsOnly; }
            set { renderContentsOnly = value; }
        }
		
		[DefaultValueAttribute (null)]
		[DesignerSerializationVisibilityAttribute (DesignerSerializationVisibility.Content)]
		[NotifyParentPropertyAttribute (true)]
		[PersistenceModeAttribute (PersistenceMode.InnerProperty)]
		public Style CurrentNodeStyle {
			get {
				if (currentNodeStyle == null) {
					currentNodeStyle = new Style ();
					if (IsTrackingViewState)
						((IStateManager)currentNodeStyle).TrackViewState ();
				}
				return currentNodeStyle;
			}
		}
	
		[DefaultValue (null)]
		[TemplateContainer (typeof(SiteMapNodeItem), BindingDirection.OneWay)]
		[PersistenceMode (PersistenceMode.InnerProperty)]
		[Browsable (false)]
		public virtual ITemplate CurrentNodeTemplate {
			get { return currentNodeTemplate; }
			set { currentNodeTemplate = value; UpdateControls (); }
		}
		
		[DefaultValueAttribute (null)]
		[DesignerSerializationVisibilityAttribute (DesignerSerializationVisibility.Content)]
		[NotifyParentPropertyAttribute (true)]
		[PersistenceModeAttribute (PersistenceMode.InnerProperty)]
		public Style NodeStyle {
			get {
				if (nodeStyle == null) {
					nodeStyle = new Style ();
					if (IsTrackingViewState)
						((IStateManager)nodeStyle).TrackViewState ();
				}
				return nodeStyle;
			}
		}
	
		[DefaultValue (null)]
		[TemplateContainer (typeof(SiteMapNodeItem), BindingDirection.OneWay)]
		[PersistenceMode (PersistenceMode.InnerProperty)]
		[Browsable (false)]
		public virtual ITemplate NodeTemplate {
			get { return nodeTemplate; }
			set { nodeTemplate = value; UpdateControls (); }
		}
		
		[DefaultValueAttribute (-1)]
		[ThemeableAttribute (false)]
		public virtual int ParentLevelsDisplayed {
			get 
            {
                if (ViewState["ParentLevelsDisplayed"] != null) { return Convert.ToInt32(ViewState["ParentLevelsDisplayed"]); }

                return -1; 
            }
			set {
				if (value < -1) throw new ArgumentOutOfRangeException ("value");
				ViewState ["ParentLevelsDisplayed"] = value;
				UpdateControls ();
			}
		}
		
		[DefaultValueAttribute (PathDirection.RootToCurrent)]
		public virtual PathDirection PathDirection {
			get 
            {
                if (ViewState["PathDirection"] != null) { return (PathDirection)Convert.ToInt32(ViewState["PathDirection"]); }
                return PathDirection.RootToCurrent; 
            }
			set {
				if (value != PathDirection.RootToCurrent && value != PathDirection.CurrentToRoot)
					throw new ArgumentOutOfRangeException ("value");
				ViewState ["PathDirection"] = value;
				UpdateControls ();
			}
		}
		
		[DefaultValueAttribute (" &gt; ")]
		[LocalizableAttribute (true)]
		public virtual string PathSeparator {
			get 
            {
                if (ViewState["PathSeparator"] != null) { return ViewState["PathSeparator"].ToString(); }
                return " > "; 
            }
			set {
				ViewState ["PathSeparator"] = value;
				UpdateControls ();
			}
		}
		
		[DefaultValueAttribute (null)]
		[DesignerSerializationVisibilityAttribute (DesignerSerializationVisibility.Content)]
		[NotifyParentPropertyAttribute (true)]
		[PersistenceModeAttribute (PersistenceMode.InnerProperty)]
		public Style PathSeparatorStyle {
			get {
				if (pathSeparatorStyle == null) {
					pathSeparatorStyle = new Style ();
					if (IsTrackingViewState)
						((IStateManager)pathSeparatorStyle).TrackViewState ();
				}
				return pathSeparatorStyle;
			}
		}
	
		[DefaultValue (null)]
		[TemplateContainer (typeof(SiteMapNodeItem), BindingDirection.OneWay)]
		[PersistenceMode (PersistenceMode.InnerProperty)]
		[Browsable (false)]
		public virtual ITemplate PathSeparatorTemplate {
			get { return pathSeparatorTemplate; }
			set { pathSeparatorTemplate = value; UpdateControls (); }
		}
		
		[BrowsableAttribute (false)]
		[DesignerSerializationVisibilityAttribute (DesignerSerializationVisibility.Hidden)]
		public SiteMapProvider Provider {
			get {
				if (provider == null) {
					if (this.SiteMapProvider.Length == 0) {
						provider = SiteMap.Provider;
						if (provider == null)
							throw new HttpException ("There is no default provider configured for the site.");
					} else {
						provider = SiteMap.Providers [this.SiteMapProvider];
						if (provider == null)
							throw new HttpException ("SiteMap provider '" + this.SiteMapProvider + "' not found.");
					}
				}
				return provider;
			}
			set {
				provider = value;
				UpdateControls ();
			}
		}
		
		[DefaultValueAttribute (false)]
		public virtual bool RenderCurrentNodeAsLink {
			get 
            {
                if (ViewState["RenderCurrentNodeAsLink"] != null) { return Convert.ToBoolean(ViewState["RenderCurrentNodeAsLink"]); }
                return false; 
            }
			set {
				ViewState ["RenderCurrentNodeAsLink"] = value;
				UpdateControls ();
			}
		}
		
		[DefaultValueAttribute (null)]
		[DesignerSerializationVisibilityAttribute (DesignerSerializationVisibility.Content)]
		[NotifyParentPropertyAttribute (true)]
		[PersistenceModeAttribute (PersistenceMode.InnerProperty)]
		public Style RootNodeStyle {
			get {
				if (rootNodeStyle == null) {
					rootNodeStyle = new Style ();
					if (IsTrackingViewState)
						((IStateManager)rootNodeStyle).TrackViewState ();
				}
				return rootNodeStyle;
			}
		}
	
		[DefaultValue (null)]
		[TemplateContainer (typeof(SiteMapNodeItem), BindingDirection.OneWay)]
		[PersistenceMode (PersistenceMode.InnerProperty)]
		[Browsable (false)]
		public virtual ITemplate RootNodeTemplate {
			get { return rootNodeTemplate; }
			set { rootNodeTemplate = value; UpdateControls (); }
		}
		
		[DefaultValueAttribute (true)]
		[ThemeableAttribute (false)]
		public virtual bool ShowToolTips {
			get 
            {
                if (ViewState["ShowToolTips"] != null) { return Convert.ToBoolean(ViewState["ShowToolTips"]); }
                return true; 
            }
			set {
				ViewState ["ShowToolTips"] = value;
				UpdateControls ();
			}
		}
		
		[DefaultValueAttribute ("")]
		[ThemeableAttribute (false)]
		public virtual string SiteMapProvider {
			get 
            {
                if (ViewState["SiteMapProvider"] != null) { return ViewState["SiteMapProvider"].ToString(); }
                return string.Empty;
            }
			set {
				ViewState ["SiteMapProvider"] = value;
				UpdateControls ();
			}
		}

		[Localizable (true)]
		public virtual string SkipLinkText 
		{
			get 
            { 
                if(ViewState["SkipLinkText"] != null){ return ViewState["SkipLinkText"].ToString(); }
                return string.Empty;
            }
			set { ViewState["SkipLinkText"] = value; }
		}
		
		
		void UpdateControls ()
		{
			ChildControlsCreated = false;
		}

		public override void DataBind ()
		{
            if (!this.Visible) { return; }

			base.DataBind ();

			/* the child controls get bound in
			 * base.DataBind */
			foreach (Control c in Controls) {
				if (c is SiteMapNodeItem) {
					SiteMapNodeItem it = (SiteMapNodeItem)c;
					OnItemDataBound (new SiteMapNodeItemEventArgs (it));
				}
			}
		}
		
		protected override void CreateChildControls ()
		{
			Controls.Clear ();
			CreateControlHierarchy ();
			DataBind ();
		}

        //added by  to enable showing breadcrumbs on pages that don't match the current url but still have a page context by pageid param
        private SiteMapNode overrideCurrentNode = null;

        /// <summary>
        /// enable showing breadcrumbs on pages that don't match the current url but still have a page context by pageid param
        /// by assigning the current page sitemapnode
        /// </summary>
        public SiteMapNode OverrideCurrentNode
        {
            set { overrideCurrentNode = value; }
        }

		protected virtual void CreateControlHierarchy ()
		{
            if (!this.Visible) { return; }

			ArrayList nodes = new ArrayList ();
			SiteMapNode node = Provider.CurrentNode;

            if ((node == null) && (overrideCurrentNode != null)) { node = overrideCurrentNode; }

            if (node == null) { return; }
			
			int levels = ParentLevelsDisplayed != -1 ? ParentLevelsDisplayed + 1 : int.MaxValue;
			
			while (node != null && levels > 0) {
				if (nodes.Count > 0) {
                    mojoSiteMapNodeItem sep = new mojoSiteMapNodeItem(nodes.Count, SiteMapNodeItemType.PathSeparator);
					InitializeItem (sep);
					SiteMapNodeItemEventArgs sargs = new SiteMapNodeItemEventArgs (sep);
					OnItemCreated (sargs);
					nodes.Add (sep);
				}

				SiteMapNodeItemType nt;
				if (nodes.Count == 0)
					nt = SiteMapNodeItemType.Current;
				else if (node.ParentNode == null)
					nt = SiteMapNodeItemType.Root;
				else
					nt = SiteMapNodeItemType.Parent;

                mojoSiteMapNodeItem it = new mojoSiteMapNodeItem(nodes.Count, nt);
				it.SiteMapNode = node;
				InitializeItem (it);
				
				SiteMapNodeItemEventArgs args = new SiteMapNodeItemEventArgs (it);
				OnItemCreated (args);
				
				nodes.Add (it);
				node = node.ParentNode;
				levels--;
			}
			
			if (PathDirection == PathDirection.RootToCurrent) {
				for (int n=nodes.Count - 1; n>=0; n--)
					Controls.Add ((Control)nodes[n]);
			} else {
				for (int n=0; n<nodes.Count; n++)
					Controls.Add ((Control)nodes[n]);
			}
		}
		
		protected virtual void InitializeItem (SiteMapNodeItem item)
		{
			switch (item.ItemType) {
				case SiteMapNodeItemType.Root:
					if (RootNodeTemplate != null) {
						item.ApplyStyle (NodeStyle);
						item.ApplyStyle (RootNodeStyle);
						RootNodeTemplate.InstantiateIn (item);
					}
					else if (NodeTemplate != null) {
						item.ApplyStyle (NodeStyle);
						item.ApplyStyle (RootNodeStyle);
						NodeTemplate.InstantiateIn (item);
					}
					else {
						WebControl c = CreateHyperLink (item);
						c.ApplyStyle (NodeStyle);
						c.ApplyStyle (RootNodeStyle);
						item.Controls.Add (c);
					}
					break;

				case SiteMapNodeItemType.Current:
					if (CurrentNodeTemplate != null) {
						item.ApplyStyle (NodeStyle);
						item.ApplyStyle (CurrentNodeStyle);
						CurrentNodeTemplate.InstantiateIn (item);
					}
					else if (NodeTemplate != null) {
						item.ApplyStyle (NodeStyle);
						item.ApplyStyle (CurrentNodeStyle);
						NodeTemplate.InstantiateIn (item);
					} else if (RenderCurrentNodeAsLink) {
						HyperLink c = CreateHyperLink (item);
						c.ApplyStyle (NodeStyle);
						c.ApplyStyle (CurrentNodeStyle);
						item.Controls.Add (c);
					} else {
						Literal c = CreateLiteral (item);
						item.ApplyStyle (NodeStyle);
						item.ApplyStyle (CurrentNodeStyle);
						item.Controls.Add (c);
					}
					break;
					
				case SiteMapNodeItemType.Parent:
					if (NodeTemplate != null) {
						item.ApplyStyle (NodeStyle);
						NodeTemplate.InstantiateIn (item);
					}
					else {
						WebControl c = CreateHyperLink (item);
						c.ApplyStyle (NodeStyle);
						item.Controls.Add (c);
					}
					break;
					
				case SiteMapNodeItemType.PathSeparator:
					if (PathSeparatorTemplate != null) {
						item.ApplyStyle (PathSeparatorStyle);
						PathSeparatorTemplate.InstantiateIn (item);
					}
					else {
						Literal h = new Literal ();
						h.Text = PathSeparator;
						item.ApplyStyle (PathSeparatorStyle);
						item.Controls.Add (h);
					}
					break;
			}
		}
		
		HyperLink CreateHyperLink (SiteMapNodeItem item)
		{
			HyperLink h = new HyperLink ();
			h.Text = item.SiteMapNode.Title;
			h.NavigateUrl = item.SiteMapNode.Url;
			if (ShowToolTips)
				h.ToolTip = item.SiteMapNode.Description;
			return h;
		}

		Literal CreateLiteral (SiteMapNodeItem item)
		{
			Literal h = new Literal ();
			h.Text = item.SiteMapNode.Title;
			return h;
		}
		
		protected override void LoadViewState (object savedState)
		{
			if (savedState == null) {
				base.LoadViewState (null);
				return;
			}
			
			object[] states = (object[]) savedState;
			base.LoadViewState (states [0]);
			
			if (states[1] != null) ((IStateManager)CurrentNodeStyle).LoadViewState (states[1]);
			if (states[2] != null) ((IStateManager)NodeStyle).LoadViewState (states[2]);
			if (states[3] != null) ((IStateManager)PathSeparatorStyle).LoadViewState (states[3]);
			if (states[4] != null) ((IStateManager)RootNodeStyle).LoadViewState (states[4]);
		}

		
		protected override void OnDataBinding (EventArgs e)
		{
			base.OnDataBinding (e);
		}
		
		
		protected override void Render (HtmlTextWriter w)
		{
			//base.Render (w);
            if (renderContentsOnly)
            {
                base.RenderContents(w);
            }
            else
            {
                base.Render(w);
            }
		}

		protected override void RenderContents (HtmlTextWriter w)
		{
			string skip_id = ClientID + "_SkipLink";

            //if (SkipLinkText != "") {
            //    HtmlAnchor anchor = new HtmlAnchor ();
            //    anchor.HRef = "#" + skip_id;

            //    Image img = new Image ();
            //    img.ImageUrl = Page.ClientScript.GetWebResourceUrl (typeof (SiteMapPath), "transparent.gif");
            //    img.Attributes.Add ("height", "0");
            //    img.Attributes.Add ("width", "0");
            //    img.AlternateText = SkipLinkText;

            //    anchor.Controls.Add (img);

                
            //    anchor.Render (w);
            //}

			base.RenderContents (w);

            //if (SkipLinkText != "") {
            //    HtmlAnchor anchor = new HtmlAnchor ();
            //    anchor.ID = skip_id;
            //    anchor.Render (w);
			//}
		}

        protected override object SaveViewState()
        {
            object[] state = new object[5];
            state[0] = base.SaveViewState();

            if (currentNodeStyle != null) state[1] = ((IStateManager)currentNodeStyle).SaveViewState();
            if (nodeStyle != null) state[2] = ((IStateManager)nodeStyle).SaveViewState();
            if (pathSeparatorStyle != null) state[3] = ((IStateManager)pathSeparatorStyle).SaveViewState();
            if (rootNodeStyle != null) state[4] = ((IStateManager)rootNodeStyle).SaveViewState();

            for (int n = 0; n < state.Length; n++)
                if (state[n] != null) return state;
            return null;
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (currentNodeStyle != null) ((IStateManager)currentNodeStyle).TrackViewState();
            if (nodeStyle != null) ((IStateManager)nodeStyle).TrackViewState();
            if (pathSeparatorStyle != null) ((IStateManager)pathSeparatorStyle).TrackViewState();
            if (rootNodeStyle != null) ((IStateManager)rootNodeStyle).TrackViewState();
        }
	}

    public class mojoSiteMapNodeItem : SiteMapNodeItem
    {
        public mojoSiteMapNodeItem(int itemIndex, SiteMapNodeItemType itemType)
            : base(itemIndex, itemType)
        {

        }
        

        private bool renderContentsOnly = true;

        public bool RenderContentsOnly
        {
            get { return renderContentsOnly; }
            set { renderContentsOnly = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (renderContentsOnly)
            {
                base.RenderContents(writer);
            }
            else
            {
                base.Render(writer);
            }
        }
    }
}


