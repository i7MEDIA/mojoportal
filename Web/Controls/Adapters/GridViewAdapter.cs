using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using mojoPortal.Web.Controls;

namespace mojoPortal.Web
{
    public class GridViewAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
    {
        private bool renderCellSpacing = true;
        private bool renderTableId = true;
        private string tableCssClass = string.Empty;

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

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PROTECTED        

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

            mojoGridView mojoGrid = Control as mojoGridView;
            if (mojoGrid != null)
            {
                renderCellSpacing = mojoGrid.RenderCellSpacing;
                renderTableId = mojoGrid.RenderTableId;
                tableCssClass = mojoGrid.TableCssClass;
            }
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderBeginTag(writer, "AspNet-GridView");
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
                GridView gridView = Control as GridView;
                if (gridView != null)
                {
                    writer.Indent++;
                    WritePagerSection(writer, PagerPosition.Top);

                    writer.WriteLine();
                    writer.WriteBeginTag("table");

                    if (renderTableId)
                    {
                        writer.WriteAttribute("id", "tbl" + gridView.ClientID);
                    }

                    if (tableCssClass.Length > 0)
                    {
                        writer.WriteAttribute("class", tableCssClass);
                    }

                    if (renderCellSpacing)
                    {
                        writer.WriteAttribute("cellspacing", "0");
                    }
                    
                    writer.WriteAttribute("summary", Control.ToolTip);

                    if (!String.IsNullOrEmpty(gridView.CssClass))
                    {
                        writer.WriteAttribute("class", gridView.CssClass);
                    }

                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    ArrayList rows = new ArrayList();
                    GridViewRowCollection gvrc = null;

                    ///////////////////// HEAD /////////////////////////////

                    rows.Clear();
                    if (gridView.ShowHeader && (gridView.HeaderRow != null))
                    {
                        rows.Add(gridView.HeaderRow);
                    }
                    gvrc = new GridViewRowCollection(rows);
                    WriteRows(writer, gridView, gvrc, "thead");

                    ///////////////////// FOOT /////////////////////////////

                    rows.Clear();
                    if (gridView.ShowFooter && (gridView.FooterRow != null))
                    {
                        rows.Add(gridView.FooterRow);
                    }
                    gvrc = new GridViewRowCollection(rows);
                    WriteRows(writer, gridView, gvrc, "tfoot");

                    /////////////// EmptyDataTemplate ///////////////////////
                    // http://stackoverflow.com/questions/3856890/gridview-using-css-friendly-control-adapters-removes-emptydatatemplate-and-empt
                    if (gridView.Rows.Count == 0)
                    {
                        //Control[0].Control[0] s/b the EmptyDataTemplate.
                        if (gridView.HasControls())
                        {
                            if (gridView.Controls[0].HasControls())
                            {
                                if (gridView.Controls[0].Controls[0] is GridViewRow)
                                {
                                    rows.Clear();
                                    rows.Add(gridView.Controls[0].Controls[0]);
                                    gvrc = new GridViewRowCollection(rows);
                                    WriteRows(writer, gridView, gvrc, "tfoot");
                                }
                            }
                        }
                    }   


                    ///////////////////// BODY /////////////////////////////

                    WriteRows(writer, gridView, gridView.Rows, "tbody");

                    ////////////////////////////////////////////////////////

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("table");

                    WritePagerSection(writer, PagerPosition.Bottom);

                    writer.Indent--;
                    writer.WriteLine();
                }
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PRIVATE        

        private void RegisterScripts()
        {
        }

        private void WriteRows(HtmlTextWriter writer, GridView gridView, GridViewRowCollection rows, string tableSection)
        {
            if (rows.Count > 0)
            {
                writer.WriteLine();
                writer.WriteBeginTag(tableSection);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;

                foreach (GridViewRow row in rows)
                {
                    if (!row.Visible) { continue; }
                    writer.WriteLine();
                    writer.WriteBeginTag("tr");

                    string className = GetRowClass(gridView, row);
                    if (!String.IsNullOrEmpty(className))
                    {
                        writer.WriteAttribute("class", className);
                    }

                    //  Uncomment the following block of code if you want to add arbitrary attributes.
                    /*
                    foreach (string key in row.Attributes.Keys)
                    {
                        writer.WriteAttribute(key, row.Attributes[key]);
                    }
                    */

                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    foreach (TableCell cell in row.Cells)
                    {
                        DataControlFieldCell fieldCell = cell as DataControlFieldCell;
                        if ((fieldCell != null) && (fieldCell.ContainingField != null))
                        {
                            DataControlField field = fieldCell.ContainingField;
                            if (!field.Visible)
                            {
                                cell.Visible = false;
                            }

                            if ((field.ItemStyle != null) && (!String.IsNullOrEmpty(field.ItemStyle.CssClass)))
                            {
                                if (!String.IsNullOrEmpty(cell.CssClass))
                                {
                                    cell.CssClass += " ";
                                }
                                cell.CssClass += field.ItemStyle.CssClass;
                            }
                        }

                        writer.WriteLine();
                        cell.RenderControl(writer);
                    }

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("tr");
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag(tableSection);
            }
        }

        private string GetRowClass(GridView gridView, GridViewRow row)
        {
            string className = "";

            if ((row.RowState & DataControlRowState.Alternate) == DataControlRowState.Alternate)
            {
                className += " AspNet-GridView-Alternate ";
                if (gridView.AlternatingRowStyle != null)
                {
                    className += gridView.AlternatingRowStyle.CssClass;
                }
            }
            else if (row.Equals(gridView.HeaderRow) && (gridView.HeaderStyle != null) && (!String.IsNullOrEmpty(gridView.HeaderStyle.CssClass)))
            {
                className += " " + gridView.HeaderStyle.CssClass;
            }
            else if (row.Equals(gridView.FooterRow) && (gridView.FooterStyle != null) && (!String.IsNullOrEmpty(gridView.FooterStyle.CssClass)))
            {
                className += " " + gridView.FooterStyle.CssClass;
            }
            else if ((gridView.RowStyle != null) && (!String.IsNullOrEmpty(gridView.RowStyle.CssClass)))
            {
                className += " " + gridView.RowStyle.CssClass;
            }

            if ((row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                className += " AspNet-GridView-Edit ";
                if (gridView.EditRowStyle != null)
                {
                    className += gridView.EditRowStyle.CssClass;
                }
            }

            if ((row.RowState & DataControlRowState.Insert) == DataControlRowState.Insert)
            {
                className += " AspNet-GridView-Insert ";
            }

            if ((row.RowState & DataControlRowState.Selected) == DataControlRowState.Selected)
            {
                className += " AspNet-GridView-Selected ";
                if (gridView.SelectedRowStyle != null)
                {
                    className += gridView.SelectedRowStyle.CssClass;
                }
            }

            //// EmptyDataTemplate 
            if ((row.RowType & DataControlRowType.EmptyDataRow) == DataControlRowType.EmptyDataRow)
            {
                className += " AspNet-GridView-Empty ";
            }

            return className.Trim();
        }

        private void WritePagerSection(HtmlTextWriter writer, PagerPosition pos)
        {
            GridView gridView = Control as GridView;
            if ((gridView != null) &&
                gridView.AllowPaging &&
                ((gridView.PagerSettings.Position == pos) || (gridView.PagerSettings.Position == PagerPosition.TopAndBottom)))
            {
                Table innerTable = null;
                if ((pos == PagerPosition.Top) &&
                    (gridView.TopPagerRow != null) &&
                    (gridView.TopPagerRow.Cells.Count == 1) &&
                    (gridView.TopPagerRow.Cells[0].Controls.Count == 1) &&
                    typeof(Table).IsAssignableFrom(gridView.TopPagerRow.Cells[0].Controls[0].GetType()))
                {
                    innerTable = gridView.TopPagerRow.Cells[0].Controls[0] as Table;
                }
                else if ((pos == PagerPosition.Bottom) &&
                    (gridView.BottomPagerRow != null) &&
                    (gridView.BottomPagerRow.Cells.Count == 1) &&
                    (gridView.BottomPagerRow.Cells[0].Controls.Count == 1) &&
                    typeof(Table).IsAssignableFrom(gridView.BottomPagerRow.Cells[0].Controls[0].GetType()))
                {
                    innerTable = gridView.BottomPagerRow.Cells[0].Controls[0] as Table;
                }

                if ((innerTable != null) && (innerTable.Rows.Count == 1))
                {
                    string className = "AspNet-GridView-Pagination AspNet-GridView-";
                    className += (pos == PagerPosition.Top) ? "Top " : "Bottom ";
                    if (gridView.PagerStyle != null)
                    {
                        className += gridView.PagerStyle.CssClass;
                    }
                    className = className.Trim();

                    writer.WriteLine();
                    writer.WriteBeginTag("div");
                    writer.WriteAttribute("class", className);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    TableRow row = innerTable.Rows[0];
                    foreach (TableCell cell in row.Cells)
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            writer.WriteLine();
                            ctrl.RenderControl(writer);
                        }
                    }

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("div");
                }
            }
        }
    }
}
