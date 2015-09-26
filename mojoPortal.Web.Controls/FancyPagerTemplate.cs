// FancyPagerTemplate.cs
//
// Copyright (c) 2007 Gonzalo Paniagua
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
// Author: Gonzalo Paniagua (gonzalo@venafi.com)
//
// added to mojoPortal.Web.controls namespace 2007-08-10
//
using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls
{
    public class FancyPagerTemplate : ITemplate
    {
        const string PreviousString = "\u25c4";
        const string NextString = "\u25ba";

        GridView m_gridView;

        public FancyPagerTemplate()
        {
        }

        public FancyPagerTemplate(GridView gv)
        {
            m_gridView = gv;
        }

        public GridView GridView
        {
            get { return m_gridView; }
            set { m_gridView = value; }
        }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            Table tbl = new Table();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Controls.Add(BuildLinkButton("prevArrow", PreviousString, OnPreviousClicked, OnPreRenderPrevious));
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.Controls.Add(new LiteralControl(" Page "));
            row.Cells.Add(cell);
            int pageCount = m_gridView.PageCount;
            cell = new TableCell();
            cell.Controls.Add(BuildPageInputTextBox(m_gridView.PageIndex, pageCount, OnPageTextChanged));
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.Controls.Add(new LiteralControl(String.Format(" / {0} ", pageCount)));
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.Controls.Add(BuildLinkButton("nextArrow", NextString, OnNextClicked, OnPreRenderNext));
            row.Cells.Add(cell);
            container.Controls.Add(tbl);
            tbl.Rows.Add(row);
        }

        #endregion

        static Button BuildLinkButton(string id, string text, EventHandler onClick, EventHandler onPreRender)
        {
            Button link = new Button();
            link.CssClass = "buttonlink";
            link.ID = id;
            link.Text = text;
            link.Click += onClick;
            link.PreRender += onPreRender;
            return link;
        }

        static TextBox BuildPageInputTextBox(int currentPage, int totalPages, EventHandler onTextChanged)
        {
            TextBox tb = new TextBox();
            tb.ID = "tbPage";
            tb.TextChanged += onTextChanged;
            tb.Style.Add("width", String.Format("{0}em", ((double)GetNumberOfDigits(totalPages) / 2)));
            tb.Text = (currentPage + 1).ToString();
            tb.AutoPostBack = true;
            return tb;
        }

        static int GetNumberOfDigits(int number)
        {
            int ndigits = 1;
            while (number > 0)
            {
                number /= 10;
                ndigits++;
            }
            return ndigits;
        }

        #region Event handlers
        public void OnPreviousClicked(object sender, EventArgs e)
        {
            if (m_gridView.PageIndex > 0)
            {
                m_gridView.PageIndex--;
            }
        }

        public void OnNextClicked(object sender, EventArgs e)
        {
            if (m_gridView.PageIndex < m_gridView.PageCount - 1)
            {
                m_gridView.PageIndex++;
            }
        }

        void OnPreRenderPrevious(object sender, EventArgs e)
        {
            if (m_gridView.PageIndex == 0)
            {
                Button btn = (Button)sender;
                btn.ForeColor = Color.FromArgb(0xbb, 0xbb, 0xbb);
                btn.Enabled = false;
            }
        }

        void OnPreRenderNext(object sender, EventArgs e)
        {
            if (m_gridView.PageIndex == m_gridView.PageCount - 1)
            {
                Button btn = (Button)sender;
                btn.ForeColor = Color.FromArgb(0xbb, 0xbb, 0xbb);
                btn.Enabled = false;
            }
        }

        void OnPageTextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int new_page;
            if (!Int32.TryParse(tb.Text, out new_page))
            {
                tb.Text = (m_gridView.PageIndex + 1).ToString();
            }
            else if (new_page <= 0 || new_page > m_gridView.PageCount)
            {
                tb.Text = (m_gridView.PageIndex + 1).ToString();
            }
            else
            {
                m_gridView.PageIndex = new_page - 1;
            }
            /* Use this if the pager is inside a UpdatePanel
             * Replace 'ScriptManager1' if needed.
            ScriptManager manager = (ScriptManager)tb.Page.Master.FindControl("ScriptManager1");
            if(manager != null) {
                manager.SetFocus(tb.ClientID);
            }
            */
        }
        #endregion
    }
}
