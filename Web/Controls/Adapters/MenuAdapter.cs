/// Originally from Microsoft, Sample released under 
/// MS Permissive License 
/// http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
/// (see license-ms-permissive.txt in the root of the solution)
/// 
/// Original source urls:
/// http://www.asp.net/cssadapters/
/// http://www.asp.net/CSSAdapters/WhitePaper.aspx
/// 
/// 
/// 
/// with modifications by 
/// Last Modified:      11/27/2006 
/// added logic to use a different class for selected items
/// 2011-03-04 added configurability of css classes and whether they are rendered

using System;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;
using System.Web.UI.HtmlControls;
using mojoPortal.Web.UI;

namespace mojoPortal.Web
{
    
    public class MenuAdapter : System.Web.UI.WebControls.Adapters.MenuAdapter
    {
        private const string container_Element = "div";
        private const string ul_CssClass = "AspNet-Menu";
        private const string li_CssClassWithChildren = "AspNet-Menu-WithChildren";
        private const string li_CssClassWithoutChildren = "AspNet-Menu-Leaf";
        private const string li_SelectedCssClassWithChildren = "AspNet-Menu-SelectedWithChildren";
        private const string li_SelectedCssClassWithoutChildren = "AspNet-Menu-SelectedLeaf";
        private const string li_ChildSelected_CssClass = "AspNet-Menu-ChildSelected";
        private const string li_ParentSelected_CssClass = "AspNet-Menu-ParentSelected";


        private const string anchor_CssClass = "AspNet-Menu";
        private const string anchor_SelectedCssClassWithChildren = "AspNet-Menu-SelectedWithChildren";
        private const string anchor_SelectedCssClassWithoutChildren = "AspNet-Menu-SelectedLeaf";
        private const string anchor_ChildSelected_CssClass = "AspNet-Menu-ChildSelected";
        private const string anchor_ParentSelected_CssClass = "AspNet-Menu-ParentSelected";


        private string containerElement = container_Element;
        private bool renderContainerCssClass = true;
        private bool renderCssClasses = true;
        private bool renderLiSelectedCss = true;
        private bool renderAnchorSelectedCss = true;
        private string containerCssClass = string.Empty;

        private string ulCssClass = ul_CssClass;
        private string liCssClassWithChildren = li_CssClassWithChildren;
        private string liCssClassWithoutChildren = li_CssClassWithoutChildren;
        private string liSelectedCssClassWithChildren = li_SelectedCssClassWithChildren;
        private string liSelectedCssClassWithoutChildren = li_SelectedCssClassWithChildren;
        private string liChildSelectedCssClass = li_ChildSelected_CssClass;
        private string liParentSelectedCssClass = li_ParentSelected_CssClass;

        private string anchorCssClass = anchor_CssClass;
        private string anchorSelectedCssClassWithChildren = anchor_SelectedCssClassWithChildren;
        private string anchorSelectedCssClassWithoutChildren = anchor_SelectedCssClassWithoutChildren;
        private string anchorChildSelectedCssClass = anchor_ChildSelected_CssClass;
        private string anchorParentSelectedCssClass = anchor_ParentSelected_CssClass;

        private bool renderCustomClassOnLi = true;
        private bool renderCustomClassOnAnchor = false;
        private string innerSpanMode = string.Empty;

        private bool renderMenuText = true;
        private bool renderImages = true;

        private bool useMenuTooltipForCustomCss = false;
        private WebControlAdapterExtender _extender = null;

        private WebControlAdapterExtender Extender
        {
            get
            {
                if (((_extender == null) && (Control != null)) ||
                    ((_extender != null) && (Control != _extender.AdaptedControl)))
                {
                    _extender = new WebControlAdapterExtender(Control);
                }

                System.Diagnostics.Debug.Assert(_extender != null, "CSS Friendly adapters internal error", "Null extender instance");
                return _extender;
            }
        }

        
       
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Extender.AdapterEnabled)
            {
                RegisterScripts();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            mojoMenu m = Control as mojoMenu;
            if (m != null) 
            { 
                useMenuTooltipForCustomCss = m.UseMenuTooltipForCustomCss;
                ulCssClass = m.UlCssClass;
                liCssClassWithChildren = m.LiCssClassWithChildren;
                liCssClassWithoutChildren = m.LiCssClassWithoutChildren;
                liSelectedCssClassWithChildren = m.LiSelectedCssClassWithChildren;
                liSelectedCssClassWithoutChildren = m.LiSelectedCssClassWithoutChildren;
                liChildSelectedCssClass = m.LiChildSelectedCssClass;
                liParentSelectedCssClass = m.LiParentSelectedCssClass;

                anchorCssClass = m.AnchorCssClass;
                anchorSelectedCssClassWithChildren = m.AnchorSelectedCssClassWithChildren;
                anchorSelectedCssClassWithoutChildren = m.AnchorSelectedCssClassWithoutChildren;

                renderContainerCssClass = m.RenderContainerCssClass;
                renderCssClasses = m.RenderCssClasses;
                renderLiSelectedCss = m.RenderLiSelectedCss;
                renderAnchorSelectedCss = m.RenderAnchorSelectedCss;
                anchorChildSelectedCssClass = m.AnchorChildSelectedCssClass;
                anchorParentSelectedCssClass = m.AnchorParentSelectedCssClass;
                renderImages = m.RenderImages;
                renderCustomClassOnLi = m.RenderCustomClassOnLi;
                renderCustomClassOnAnchor = m.RenderCustomClassOnAnchor;
                innerSpanMode = m.InnerSpanMode;
                renderMenuText = m.RenderMenuText;
                containerElement = m.ContainerElement;
                containerCssClass = m.ContainerCssClass;
            }
        }
       
        private void RegisterScripts()
        {
            Extender.RegisterScripts();
            //string folderPath = WebConfigurationManager.AppSettings.Get("CSSFriendly-JavaScript-Path");
            //if (String.IsNullOrEmpty(folderPath))
            //{
            //    folderPath = "~/ClientScript";
            //}
            ////string filePath = folderPath.EndsWith("/") ? folderPath + "MenuAdapter.js" : folderPath + "/MenuAdapter.js";
            ////Page.ClientScript.RegisterClientScriptInclude(GetType(), GetType().ToString(), Page.ResolveUrl(filePath));
            //string filePath = folderPath.EndsWith("/") ? folderPath + "mojocombined.js" : folderPath + "/mojocombined.js";
            //Page.ClientScript.RegisterClientScriptInclude(typeof(Page), "mojocombined", Page.ResolveUrl(filePath));
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                if (renderContainerCssClass)
                {
                    string containerClass = "AspNet-Menu-" + Control.Orientation.ToString();
                    if (containerCssClass.Length > 0) { containerClass = containerCssClass; }

                    Extender.RenderBeginTag(writer, containerElement, containerClass);
                }
                else
                {
                    Extender.RenderBeginTag(writer, containerElement, string.Empty);
                }
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderEndTag(writer, containerElement);
            }
            else
            {
                base.RenderEndTag(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                writer.Indent++;
                BuildItems(Control.Items, true, writer);
                writer.Indent--;
                writer.WriteLine();
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        

        private void BuildItems(MenuItemCollection items, bool isRoot, HtmlTextWriter writer)
        {
            if (items.Count > 0)
            {
               
                writer.WriteLine();

                writer.WriteBeginTag("ul");
                if ((isRoot)&&(renderCssClasses))
                {
                    if (ulCssClass.Length > 0)
                    {
                        writer.WriteAttribute("class", ulCssClass);
                    }
                }

                if ((Control != null) && (Control is mojoMenu))
                {
                    mojoMenu m = Control as mojoMenu;
                    if (m.UseDataRole)
                    {
                        writer.WriteAttribute("data-role", "listview");
                    }
                }

                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;

                foreach (MenuItem item in items)
                {
                    BuildItem(item, writer);
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("ul");
            }
        }

        

        private void BuildItem(MenuItem item, HtmlTextWriter writer)
        {
            Menu menu = Control as Menu;
            if ((menu != null) && (item != null) && (writer != null))
            {
                writer.WriteLine();
                writer.WriteBeginTag("li");

                
                if (renderCssClasses)
                {
                    string theClass = GetItemLiClass(menu, item);
                    
                    if (renderLiSelectedCss)
                    {
                        string selectedStatusClass = GetLiSelectStatusClass(item);
                        if (!String.IsNullOrEmpty(selectedStatusClass))
                        {
                            if (theClass.Length > 0)
                            {
                                theClass += " " + selectedStatusClass;
                            }
                            else
                            {
                                theClass = selectedStatusClass;
                            }
                        }
                    }

                    if (theClass.Length > 0)
                    {
                        writer.WriteAttribute("class", theClass);
                    }
                }

                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;
                writer.WriteLine();

                if (((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null)) ||
                    ((item.Depth >= menu.StaticDisplayLevels) && (menu.DynamicItemTemplate != null)))
                {
                    writer.WriteBeginTag("div");
                    if (renderCssClasses)
                    {
                        writer.WriteAttribute("class", GetItemClass(menu, item));
                    }
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;
                    writer.WriteLine();

                    MenuItemTemplateContainer container = new MenuItemTemplateContainer(menu.Items.IndexOf(item), item);
                    if ((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null))
                    {
                        menu.StaticItemTemplate.InstantiateIn(container);
                    }
                    else
                    {
                        menu.DynamicItemTemplate.InstantiateIn(container);
                    }
                    container.DataBind();
                    container.RenderControl(writer);

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("div");
                }
                else
                {
                    if (IsLink(item))
                    {
                        writer.WriteBeginTag("a");
                        if (!String.IsNullOrEmpty(item.NavigateUrl))
                        {
                            writer.WriteAttribute("href", Page.Server.HtmlEncode(menu.ResolveUrl(item.NavigateUrl)));
                        }
                        else
                        {
                            writer.WriteAttribute("href", Page.ClientScript.GetPostBackClientHyperlink(menu, "b" + item.ValuePath.Replace(menu.PathSeparator.ToString(), "\\"), true));
                        }

                        if (renderCssClasses)
                        {
                            string anchorCss = GetItemClass(menu, item);
                            if (anchorCss.Length > 0)
                            {
                                writer.WriteAttribute("class", anchorCss);
                            }
                        }

                        WebControlAdapterExtender.WriteTargetAttribute(writer, item.Target);

                        if (!useMenuTooltipForCustomCss)
                        {
                            if (!String.IsNullOrEmpty(item.ToolTip))
                            {
                                writer.WriteAttribute("title", item.ToolTip);
                            }
                            else if (!String.IsNullOrEmpty(menu.ToolTip))
                            {
                                writer.WriteAttribute("title", menu.ToolTip);
                            }
                        }
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;
                        writer.WriteLine();
                    }
                    else
                    {
                        writer.WriteBeginTag("span");
                        if (renderCssClasses)
                        {
                            writer.WriteAttribute("class", GetItemClass(menu, item));
                        }
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;
                        writer.WriteLine();
                    }

                    

                    switch (innerSpanMode)
                    {
                        case "Artisteer":

                            writer.WriteBeginTag("span");
                            writer.WriteAttribute("class", "l");
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.WriteEndTag("span");

                            writer.WriteBeginTag("span");
                            writer.WriteAttribute("class", "r");
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.WriteEndTag("span");

                            writer.WriteBeginTag("span");
                            writer.WriteAttribute("class", "t");
                            writer.Write(HtmlTextWriter.TagRightChar);

                            break;

                        case "SingleSpan":
                            writer.WriteFullBeginTag("span");
                            break;

                        case "ThreeSpans":
                            writer.WriteBeginTag("span");
                            writer.WriteAttribute("class", "menutext");
                            writer.Write(HtmlTextWriter.TagRightChar);

                            break;

                        case "":
                        case "None":
                        default:
                            // do nothing
                            break;
                    }

                    if ((renderImages)&&(!string.IsNullOrEmpty(item.ImageUrl)))
                    {
                        writer.WriteBeginTag("img");
                        writer.WriteAttribute("src", menu.ResolveClientUrl(item.ImageUrl));
                        if (useMenuTooltipForCustomCss)
                        {
                            writer.WriteAttribute("alt", item.Text);
                        }
                        else
                        {
                            writer.WriteAttribute("alt", !String.IsNullOrEmpty(item.ToolTip) ? item.ToolTip : (!String.IsNullOrEmpty(menu.ToolTip) ? menu.ToolTip : item.Text));
                        }
                        writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                        writer.Write(HtmlTextWriter.SpaceChar);
                    }

                    // put a switch here to satisfy this request
                    // http://www.mojoportal.com/Forums/Thread.aspx?thread=2824&mid=34&pageid=5&ItemID=4&pagenumber=1#post12578
                    if (renderMenuText)
                    {
                        writer.Write(item.Text);
                    }

                    switch (innerSpanMode)
                    {
                        case "Artisteer":
                            writer.WriteEndTag("span");
                            break;

                        case "SingleSpan":
                            writer.WriteEndTag("span");
                            break;

                        case "ThreeSpans":
                            writer.WriteEndTag("span");

                            writer.WriteBeginTag("span");
                            writer.WriteAttribute("class", "tab-l");
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Write("&nbsp;");
                            writer.WriteEndTag("span");

                            writer.WriteBeginTag("span");
                            writer.WriteAttribute("class", "tab-r");
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Write("&nbsp;");
                            writer.WriteEndTag("span");

                            break;

                        case "":
                        case "None":
                        default:
                            // do nothing
                            break;
                    }

                    if (IsLink(item))
                    {
                        writer.Indent--;
                        writer.WriteEndTag("a");
                    }
                    else
                    {
                        writer.Indent--;
                        writer.WriteEndTag("span");
                    }

                }

                if ((item.ChildItems != null) && (item.ChildItems.Count > 0))
                {
                    BuildItems(item.ChildItems, false, writer);
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("li");
            }
        }

        private bool IsLink(MenuItem item)
        {
            return (item != null) && item.Enabled && ((!String.IsNullOrEmpty(item.NavigateUrl)) || item.Selectable);
        }

        private string GetItemLiClass(Menu menu, MenuItem item)
        {
            string value;
            if (item.ChildItems.Count > 0)
            {
                value = liCssClassWithChildren;
            }
            else
            {
                value = liCssClassWithoutChildren;
            }

            if (renderCustomClassOnLi && (useMenuTooltipForCustomCss) && (item.ToolTip.Length > 0))
            {
                if (value.Length > 0)
                {
                    value += " " + item.ToolTip; //we are using tooltip to store a custom css class
                }
                else
                {
                    value = item.ToolTip;
                }

            }

            return value;
        }

        private string GetItemClass(Menu menu, MenuItem item)
        {
            string value = "AspNet-Menu-NonLink";
            if (item != null)
            {
                if (((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null)) ||
                    ((item.Depth >= menu.StaticDisplayLevels) && (menu.DynamicItemTemplate != null)))
                {
                    value = "AspNet-Menu-Template";
                }
                else if (IsLink(item))
                {
                    value = anchorCssClass;
                }

                if (renderAnchorSelectedCss)
                {
                    string selectedStatusClass = GetSelectStatusClass(item);
                    if (!string.IsNullOrEmpty(selectedStatusClass))
                    {
                        if (value.Length > 0)
                        {
                            value += " " + selectedStatusClass;
                        }
                        else
                        {
                            value = selectedStatusClass;
                        }
                    }
                }

                if (!item.Selectable) 
                {
                    if (value.Length > 0)
                    {
                        value += " unclickable";
                    }
                    else
                    {
                        value = "unclickable";
                    }
                }

                if (renderCustomClassOnAnchor && (useMenuTooltipForCustomCss) && (item.ToolTip.Length > 0))
                {
                    if (value.Length > 0)
                    {
                        value += " " + item.ToolTip; //we are using tooltip to store a custom css class
                    }
                    else
                    {
                        value = item.ToolTip; 
                    }
                }
            }

            return value;
        }

        private string GetSelectStatusClass(MenuItem item)
        {
            string value = string.Empty;
            if (item.Selected)
            {
                //value += " AspNet-Menu-Selected";
                if (item.ChildItems.Count > 0)
                {
                    value = anchorSelectedCssClassWithChildren;
                }
                else
                {
                    value = anchorSelectedCssClassWithoutChildren;
                }
            }
            else if (IsChildItemSelected(item))
            {
                // top if logic added by 
                // so the topmost item is highlighted if
                // a child or grandchild is the current page
                if (item.Parent == null)
                {
                    if (anchorSelectedCssClassWithChildren.Length > 0)
                    {
                        value = " " + anchorSelectedCssClassWithChildren;
                    }
                }
                else
                {
                    if (anchorChildSelectedCssClass.Length > 0)
                    {
                        value = " " + anchorChildSelectedCssClass;
                    }
                }
            }
            else if (IsParentItemSelected(item))
            {
                if (anchorParentSelectedCssClass.Length > 0)
                {
                    value = " " + anchorParentSelectedCssClass;
                }
            }

            return value;
        }

        private string GetLiSelectStatusClass(MenuItem item)
        {
            string value = string.Empty;
            if (item.Selected)
            {
                //value += " AspNet-Menu-Selected";
                if (item.ChildItems.Count > 0)
                {
                    value = liSelectedCssClassWithChildren;
                }
                else
                {
                    value = liSelectedCssClassWithoutChildren;
                }
            }
            else if (IsChildItemSelected(item))
            {
                // top if logic added by 
                // so the topmost item is highlighted if
                // a child or grandchild is the current page
                if (item.Parent == null)
                {
                    if (liSelectedCssClassWithChildren.Length > 0)
                    {
                        value = " " + liSelectedCssClassWithChildren;
                    }
                }
                else
                {
                    if (liChildSelectedCssClass.Length > 0)
                    {
                        value = " " + liChildSelectedCssClass;
                    }
                }
            }
            else if (IsParentItemSelected(item))
            {
                if (liParentSelectedCssClass.Length > 0)
                {
                    value = " " + liParentSelectedCssClass;
                }
            }

            return value;
        }

        private bool IsChildItemSelected(MenuItem item)
        {
            bool bRet = false;

            if ((item != null) && (item.ChildItems != null))
            {
                bRet = IsChildItemSelected(item.ChildItems);
            }

            return bRet;
        }


        private bool IsChildItemSelected(MenuItemCollection items)
        {
            bool bRet = false;

            if (items != null)
            {
                foreach (MenuItem item in items)
                {
                    if (item.Selected || IsChildItemSelected(item.ChildItems))
                    {
                        bRet = true;
                        break;
                    }
                }
            }

            return bRet;
        }

        private bool IsParentItemSelected(MenuItem item)
        {
            bool bRet = false;

            if ((item != null) && (item.Parent != null))
            {
                if (item.Parent.Selected)
                {
                    bRet = true;
                }
                else
                {
                    bRet = IsParentItemSelected(item.Parent);
                }
            }

            return bRet;
        }

    }

}
