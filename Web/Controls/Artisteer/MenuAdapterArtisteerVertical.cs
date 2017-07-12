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
/// Last Modified:      2010-08-24
/// added logic to use a different class for selected items

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

    public class MenuAdapterArtisteerVertical : System.Web.UI.WebControls.Adapters.MenuAdapter
    {
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

            mojoMenuArtisteerVertical mm = Control as mojoMenuArtisteerVertical;
            if (mm != null) { useMenuTooltipForCustomCss = mm.UseMenuTooltipForCustomCss; }
        }


        private void RegisterScripts()
        {
            Extender.RegisterScripts();
            
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderBeginTag(writer, "art-vmenublock");

                writer.Write("<div class=\"art-vmenublock-tl\"></div>");
                writer.Write("<div class=\"art-vmenublock-tr\"></div>");
                writer.Write("<div class=\"art-vmenublock-bl\"></div>");
                writer.Write("<div class=\"art-vmenublock-br\"></div>");
                writer.Write("<div class=\"art-vmenublock-tc\"></div>");
                writer.Write("<div class=\"art-vmenublock-bc\"></div>");
                writer.Write("<div class=\"art-vmenublock-cl\"></div>");
                writer.Write("<div class=\"art-vmenublock-cr\"></div>");
                writer.Write("<div class=\"art-vmenublock-cc\"></div>");

                writer.Write("<div class=\"art-vmenublock-body\">");
                writer.Write("<div class=\"art-vmenublockcontent\">");
                writer.Write("<div class=\"art-vmenublockcontent-body\">");
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
                writer.Write("</div>");
                writer.Write("</div>");
                writer.Write("</div>");

                Extender.RenderEndTag(writer);


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
                BuildItems(null, Control.Items, true, writer);
                writer.Indent--;
                writer.WriteLine();
            }
            else
            {
                base.RenderContents(writer);
            }
        }


        private void BuildItems(MenuItem currentItem, MenuItemCollection items, bool isRoot, HtmlTextWriter writer)
        {
            
            if (items.Count > 0)
            {


                writer.WriteLine();

                writer.WriteBeginTag("ul");
                if (isRoot)
                {
                    writer.WriteAttribute("class", "art-vmenu");
                }
                else if(currentItem != null)
                {
                    string classname = GetSelectStatusClass(currentItem);
                    if (classname.Length > 0) { writer.WriteAttribute("class", classname); }
                    
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

                string theClass = GetSelectStatusClass(item);
                if (theClass.Length > 0)
                {
                    writer.WriteAttribute("class", theClass);
                }

                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;
                writer.WriteLine();

                if (((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null)) ||
                    ((item.Depth >= menu.StaticDisplayLevels) && (menu.DynamicItemTemplate != null)))
                {
                    writer.WriteBeginTag("div");
                    writer.WriteAttribute("class", GetItemClass(menu, item));
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
                            writer.WriteAttribute("href", menu.ResolveUrl(item.NavigateUrl));
                        }
                        else
                        {
                            writer.WriteAttribute("href", Page.ClientScript.GetPostBackClientHyperlink(menu, "b" + item.ValuePath.Replace(menu.PathSeparator.ToString(), "\\"), true));
                        }

                        string linkClass = GetSelectStatusClass(item);
                        if (linkClass.Length > 0)
                        {
                            writer.WriteAttribute("class", linkClass);
                        }
                        //writer.WriteAttribute("class", GetItemClass(menu, item));
                        
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
                        writer.WriteAttribute("class", GetItemClass(menu, item));
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;
                        writer.WriteLine();
                    }

                    bool parentSelected = IsParentItemSelected(item);

                    if (item.Depth < 1)
                    {
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
                    }

                    if (!String.IsNullOrEmpty(item.ImageUrl))
                    {
                        writer.WriteBeginTag("img");
                        writer.WriteAttribute("src", menu.ResolveClientUrl(item.ImageUrl));
                        writer.WriteAttribute("alt", !String.IsNullOrEmpty(item.ToolTip) ? item.ToolTip : (!String.IsNullOrEmpty(menu.ToolTip) ? menu.ToolTip : item.Text));
                        writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                        //writer.Write(HtmlTextWriter.SpaceChar);
                    }

                    writer.Write(item.Text);

                    if (item.Depth < 1)
                    {
                        writer.WriteEndTag("span");
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
                    BuildItems(item, item.ChildItems, false, writer);
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
                    //value = "AspNet-Menu-Link";
                    value = "AspNet-Menu";
                }
                string selectedStatusClass = GetSelectStatusClass(item);
                if (!String.IsNullOrEmpty(selectedStatusClass))
                {
                    value += " " + selectedStatusClass;
                }
                if (!item.Selectable) { value += " unclickable"; }

                if ((useMenuTooltipForCustomCss) && (item.ToolTip.Length > 0))
                {
                    value += " " + item.ToolTip; //we are using tooltip to store a custom css class
                }
            }
            
            return value;
        }

        private string GetSelectStatusClass(MenuItem item)
        {
            string value = string.Empty;
            if (item.Selected)
            {
                value = "active";
            }
            else if (IsChildItemSelected(item))
            {
                value = "active";
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
