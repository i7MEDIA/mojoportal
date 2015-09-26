using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class mojoListBox : ListBox
    {
        public string DataGroupField
        {
            get;
            set;
        }

        private bool htmlEncodeItems = false;

        public bool HtmlEncodeItems
        {
            get { return htmlEncodeItems; }
            set { htmlEncodeItems = value; }
        }

        // should be sufficient to prevent xss without boogering up the content with html encoding
        private bool htmlAttributeEncodeItems = true;

        public bool HtmlAttributeEncodeItems
        {
            get { return htmlAttributeEncodeItems; }
            set { htmlAttributeEncodeItems = value; }
        }

        private bool autoGroup = false;
        /// <summary>
        /// true forces groups to be sorted together
        /// </summary>
        public bool AutoGroup
        {
            get { return autoGroup; }
            set { autoGroup = value; }
        }

        protected override void PerformDataBinding(IEnumerable dataSource)
        {
            base.PerformDataBinding(dataSource);


            if ((string.IsNullOrEmpty(this.DataGroupField) == false) && (dataSource != null))
            {

                ListItemCollection items = this.Items;
                IEnumerable<Object> data = dataSource.OfType<Object>();
                Int32 count = data.Count();

                for (Int32 i = 0; i < count; ++i)
                {
                    String group = DataBinder.Eval(data.ElementAt(i), this.DataGroupField) as String ?? String.Empty;

                    if (String.IsNullOrEmpty(group) == false)
                    {
                        items[i].Attributes["Group"] = group;
                    }
                }
            }
        }


        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) { return; }

            if (autoGroup)
            {
                RenderContentsAutoGrouped(writer);
            }
            else
            {
                RenderContentsNaturalGrouped(writer);
            }

            

        }

        private void RenderContentsNaturalGrouped(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) { return; }

            string currentGroup = string.Empty;
            string previousGroup = string.Empty;

            int count = Items.Count;
            ListItem item;
            bool flag = false;

            for (int i = 0; i < count;)
            {
                item = Items[i];
                currentGroup = item.Attributes["Group"];
                if (!string.IsNullOrEmpty(currentGroup))
                {

                    if (previousGroup != currentGroup)
                    {

                        if (!string.IsNullOrEmpty(previousGroup))
                        {
                            // close the previous group
                            writer.WriteEndTag("optgroup");
                        }

                        //open current group
                        writer.WriteBeginTag("optgroup");
                        writer.WriteAttribute("label", currentGroup);
                        writer.Write('>');

                    }


                    previousGroup = currentGroup;
                }
                else
                {
                    // no current group
                    if (!string.IsNullOrEmpty(previousGroup))
                    {
                        // close the previous group
                        writer.WriteEndTag("optgroup");
                    }


                    previousGroup = string.Empty;
                }

                // write the item

                if (item.Enabled == true)
                {
                    writer.WriteBeginTag("option");

                    if (item.Selected == true)
                    {
                        if (flag == true)
                        {
                            this.VerifyMultiSelect();
                        }

                        flag = true;

                        writer.WriteAttribute("selected", "selected");
                    }


                    writer.WriteAttribute("value", item.Value, true);

                    if (item.Attributes.Count != 0)
                    {
                        item.Attributes.Render(writer);
                    }

                    if (this.Page != null)
                    {
                        this.Page.ClientScript.RegisterForEventValidation(this.UniqueID, item.Value);
                    }

                    writer.Write(Html32TextWriter.TagRightChar);

                    if (htmlEncodeItems)
                    {
                        HttpUtility.HtmlEncode(item.Text, writer);
                    }
                    else if (htmlAttributeEncodeItems)
                    {
                        HttpUtility.HtmlAttributeEncode(item.Text, writer);
                    }
                    else
                    {
                        writer.Write(item.Text);
                    }

                    writer.WriteEndTag("option");
                    writer.WriteLine();
                }

                if (i == count - 1)
                {
                    // just wrote last item
                    if (!string.IsNullOrEmpty(currentGroup))
                    {
                        // close the final group
                        writer.WriteEndTag("optgroup");
                    }

                }

                i += 1;

            }


            

        }

        private void RenderContentsAutoGrouped(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) { return; }

            ListItemCollection items = this.Items;
            int count = items.Count;

            var groupedItems = items.OfType<ListItem>().GroupBy(x => x.Attributes["Group"] ?? String.Empty).Select(x => new { Group = x.Key, Items = x.ToList() });

            if (count > 0)
            {
                Boolean flag = false;

                foreach (var groupedItem in groupedItems)
                {
                    if (string.IsNullOrEmpty(groupedItem.Group) == false)
                    {
                        writer.WriteBeginTag("optgroup");
                        writer.WriteAttribute("label", groupedItem.Group);
                        writer.Write('>');
                    }



                    for (int i = 0; i < groupedItem.Items.Count; ++i)
                    {
                        ListItem item = groupedItem.Items[i];

                        if (item.Enabled == true)
                        {
                            writer.WriteBeginTag("option");

                            if (item.Selected == true)
                            {
                                if (flag == true)
                                {
                                    this.VerifyMultiSelect();
                                }

                                flag = true;

                                writer.WriteAttribute("selected", "selected");
                            }


                            writer.WriteAttribute("value", item.Value, true);

                            if (item.Attributes.Count != 0)
                            {
                                item.Attributes.Render(writer);
                            }

                            if (this.Page != null)
                            {
                                this.Page.ClientScript.RegisterForEventValidation(this.UniqueID, item.Value);
                            }

                            writer.Write(Html32TextWriter.TagRightChar);

                            if (htmlEncodeItems)
                            {
                                HttpUtility.HtmlEncode(item.Text, writer);
                            }
                            else if (htmlAttributeEncodeItems)
                            {
                                HttpUtility.HtmlAttributeEncode(item.Text, writer);
                            }
                            else
                            {
                                writer.Write(item.Text);
                            }

                            writer.WriteEndTag("option");
                            writer.WriteLine();
                        }

                    }



                    if (string.IsNullOrEmpty(groupedItem.Group) == false)
                    {
                        writer.WriteEndTag("optgroup");
                    }

                }

            }

        }

    }
}