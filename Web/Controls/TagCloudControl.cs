// a modified version from this article on Codeplex http://www.codeproject.com/KB/custom-controls/cloud.aspx
// licensed under the Code Project Open License http://www.codeproject.com/info/cpol10.aspx
// last modified by  2009-05-05

// changed to render as an unordered list and use only css classes
// changed from scale 1-7 to scale 1-10 for normalized weights

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;


namespace mojoPortal.Web.UI
{
    public class TagCloudControl : CompositeDataBoundControl, IPostBackEventHandler
    {
        private Collection<TagCloudItem> items = new Collection<TagCloudItem>();

        /// <summary>
        /// Collection of TagCloudItems. 
        /// </summary>
        [Themeable(false), PersistenceMode(PersistenceMode.InnerProperty), MergableProperty(false)]
        public Collection<TagCloudItem> Items
        {
            get
            {
                return items;
            }
        }

        private bool useWeightInTextFormat = false;

        public bool UseWeightInTextFormat
        {
            get { return useWeightInTextFormat; }
            set { useWeightInTextFormat = value; }
        }

        private static int NormalizeWeight(double weight, double mean, double standardDeviation)
        {
            double factor = (weight - mean);

            if (factor != 0 && standardDeviation != 0) factor /= standardDeviation;

            if (factor > 2.5) { return 10; }
            if (factor > 2) { return 9; }
            if (factor > 1.5) { return 8; }
            if (factor > 1) { return 7; }
            if (factor > 0.5) { return 6; }
            if (factor > 0) { return 5; }
            if (factor > -0.5) { return 4; }
            if (factor > -1) { return 3; }
            if (factor > -1.5) { return 2; }

            return 1;

            
        }

        static string[] cssClasses = new string[] { "weight1", "weight2", "weight3", "weight4", "weight5", "weight6", "weight7", "weight8", "weight9", "weight10" };

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.items.Count == 0) { return; }
            base.Render(writer);
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        /// <summary>
        /// Gets or sets the name of the data field that is bound to the Text property of an item.
        /// </summary>
        [Category("Data")]
        [TypeConverter(typeof(DataFieldConverter))]
        public string DataTextField
        {
            get
            {
                string val = ViewState["DataTextField"] as string;

                if (val != null)
                {
                    return val;
                }

                return String.Empty;
            }
            set
            {
                ViewState["DataTextField"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }


        /// <summary>
        /// Gets or sets the format string for the Text property.
        /// </summary>
        [Category("Data")]
        public string DataTextFormatString
        {
            get
            {
                string val = ViewState["DataTextFormatString"] as string;

                if (val != null)
                {
                    return val;
                }

                return String.Empty;
            }
            set
            {
                ViewState["DataTextFormatString"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }


        /// <summary>
        /// Gets or sets the data field which is bound to the Href property of an item.
        /// </summary>
        [Category("Data")]
        [TypeConverter(typeof(DataFieldConverter))]
        public string DataHrefField
        {
            get
            {
                string val = ViewState["DataHrefField"] as string;

                if (val != null)
                {
                    return val;
                }

                return String.Empty;
            }
            set
            {
                ViewState["DataHrefField"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the format string to format the Href property value.
        /// </summary>
        [Category("Data")]
        public string DataHrefFormatString
        {
            get
            {
                string val = ViewState["DataHrefFormatString"] as string;

                if (val != null)
                {
                    return val;
                }

                return String.Empty;
            }
            set
            {
                ViewState["DataHrefFormatString"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the data field which is bound to the Title property of an item.
        /// </summary>
        [Category("Data")]
        [TypeConverter(typeof(DataFieldConverter))]
        public string DataTitleField
        {
            get
            {
                string val = ViewState["DataTitleField"] as string;

                if (val != null)
                {
                    return val;
                }

                return String.Empty;
            }
            set
            {
                ViewState["DataTitleField"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        /// The format string for the title(tooltip) of an item. {0} in this string is replaced with the
        /// value of the field specified as the DataTitleField.
        /// </summary>
        [Category("Data")]
        public string DataTitleFormatString
        {
            get
            {
                string val = ViewState["DataTitleFormatString"] as string;

                if (val != null)
                {
                    return val;
                }

                return String.Empty;
            }
            set
            {
                ViewState["DataTitleFormatString"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        /// The field from the Data Source where the weight of an item is to be obtained.
        /// </summary>
        [Category("Data")]
        [TypeConverter(typeof(DataFieldConverter))]
        public string DataWeightField
        {
            get
            {
                string val = ViewState["DataWeightField"] as string;

                if (val != null)
                {
                    return val;
                }

                return String.Empty;
            }
            set
            {
                ViewState["DataWeightField"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        private IEnumerable<double> ItemWeights
        {
            get
            {
                foreach (TagCloudItem item in this.Items)
                {
                    yield return item.Weight;
                }
            }
        }

        private IEnumerable<double> Log10ItemWeights
        {
            get
            {
                foreach (TagCloudItem item in this.Items)
                {
                    yield return Math.Log10(item.Weight);
                }
            }
        }

        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            if (dataBinding && !this.DesignMode) { CreateItemsFromData(dataSource); }

            double mean;
            double standardDeviation = StatisticsHelper.StandardDeviation(Log10ItemWeights, out mean);
            int index = 0;

            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "tag-cloud");

            foreach (TagCloudItem item in Items)
            {
                int itemWeight = NormalizeWeight(Math.Log10(item.Weight), mean, standardDeviation);

                HtmlGenericControl li = new HtmlGenericControl("li");
                li.Attributes.Add("class", cssClasses[itemWeight -1]);

                HtmlAnchor a = new HtmlAnchor();

                if (string.IsNullOrEmpty(item.Href))
                {
                    a.HRef = this.Page.ClientScript.GetPostBackClientHyperlink(this, index.ToString());
                }
                else
                {
                    a.HRef = item.Href;
                }

                a.InnerHtml = item.Text;
                a.Title = item.Title;
                

                li.Controls.Add(a);
                li.Controls.Add(new LiteralControl(" "));
                ul.Controls.Add(li);
               
                index++;
            }

            if ((!DesignMode) && (index > 0))
            {
                this.Controls.Add(ul);
            }

            if (this.DesignMode && Items.Count == 0)
            {

                HtmlAnchor a = new HtmlAnchor();
                a.HRef = "javascript:void(0)";
                a.InnerText = "TagCloud";
                this.Controls.Add(a);
            }

            return Items.Count;
        }

        private void CreateItemsFromData(System.Collections.IEnumerable dataSource)
        {
            foreach (object data in dataSource)
            {
                TagCloudItem item = new TagCloudItem();

                if (String.IsNullOrEmpty(this.DataHrefField))
                {
                    if (String.IsNullOrEmpty(this.DataHrefFormatString))
                        item.Href = String.Empty;
                    else
                        String.Format(CultureInfo.CurrentCulture, this.DataHrefFormatString, new object[] { data });
                }
                else
                {
                    item.Href = DataBinder.Eval(data, this.DataHrefField, this.DataHrefFormatString);
                }

                if (!String.IsNullOrEmpty(this.DataTextField))
                {
                    if (useWeightInTextFormat)
                    {
                        if (CultureInfo.CurrentCulture.TextInfo.IsRightToLeft)
                        {
                            this.DataTextFormatString = "<span class='tagcount'>({1})</span>{0}";
                        }
                        else
                        {
                            this.DataTextFormatString = "{0}<span class='tagcount'>({1})</span>";
                        }


                        item.Text = string.Format(CultureInfo.InvariantCulture, 
                            this.DataTextFormatString, 
                            DataBinder.Eval(data, this.DataTextField), 
                            DataBinder.Eval(data, this.DataWeightField)
                            );
                    }
                    else
                    {
                        item.Text = DataBinder.Eval(data, this.DataTextField, this.DataTextFormatString);
                    }
                }

                if (!String.IsNullOrEmpty(this.DataTitleField))
                {
                    item.Title = DataBinder.Eval(data, this.DataTitleField, this.DataTitleFormatString);
                }

                if (!String.IsNullOrEmpty(this.DataWeightField))
                {
                    item.Weight = Convert.ToDouble(DataBinder.GetPropertyValue(data, this.DataWeightField));
                }

                this.Items.Add(item);
            }
        }

        public event EventHandler<TagCloudItemClickEventArgs> ItemClick;

        protected void OnItemClick(TagCloudItemClickEventArgs e)
        {
            if (ItemClick != null) { ItemClick(this, e); }
        }

        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            int selectedIndex = 0;
            if (Int32.TryParse(eventArgument, out selectedIndex))
            {
                this.RequiresDataBinding = true;
                this.EnsureDataBound();

                if (selectedIndex >= 0 && selectedIndex < this.Items.Count)
                {
                    OnItemClick(new TagCloudItemClickEventArgs(this.Items[selectedIndex]));
                }
            }
        }

        #endregion

    }

    public class TagCloudItem
    {
        #region Constructors

        public TagCloudItem()
        {
        }

        public TagCloudItem(string text, double weight)
        {
            this.text = text;
            this.weight = weight;
        }

        public TagCloudItem(string text, double weight, string href)
            : this(text, weight)
        {
            this.href = href;
        }

        public TagCloudItem(string text, double weight, string href, string title)
            : this(text, weight, href)
        {
            this.title = title;
        }

        #endregion

        #region Private Properties

        private string text = string.Empty;
        private string title = string.Empty;
        private string href = string.Empty;
        private double weight = 0;


        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the text
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Get or sets the href url
        /// </summary>
        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        /// <summary>
        /// Gets or sets the tooltip
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }


        /// <summary>
        /// Gets or sets the weight
        /// </summary>
        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        #endregion




    }

    public class TagCloudItemClickEventArgs : EventArgs
    {
        internal TagCloudItemClickEventArgs(TagCloudItem item)
        {
            this.item = item;
        }

        private TagCloudItem item;


        public TagCloudItem Item
        {
            get { return item; }
        }
    }
}
