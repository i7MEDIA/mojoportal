// Author:					i7MEDIA (joe davis)
// Created:				    2014-12-22
// Last Modified:			2017-12-19
//
// You must not remove this notice, or any other, from this software.
//
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuperFlexiUI
{
    /// <summary>
    /// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class SuperFlexiDisplaySettings : WebControl
    {
        private bool hideOuterWrapperPanel = false;
        public bool HideOuterWrapperPanel
        {
            get { return hideOuterWrapperPanel; }
            set { hideOuterWrapperPanel = value; }
        }

        private bool hideInnerWrapperPanel = false;
        public bool HideInnerWrapperPanel
        {
            get { return hideInnerWrapperPanel; }

            set { hideInnerWrapperPanel = value; }
        }

        private bool hideOuterBodyPanel = false;
        public bool HideOuterBodyPanel
        {
            get { return hideOuterBodyPanel; }

            set { hideOuterBodyPanel = value; }
        }

        private bool hideInnerBodyPanel = false;
        public bool HideInnerBodyPanel
        {
            get { return hideInnerBodyPanel; }

            set { hideInnerBodyPanel = value; }
        }


        private string moduleTitleMarkup = "$_ModuleTitle_$$_ModuleLinks_$";
        public string ModuleTitleMarkup
        {
            get { return moduleTitleMarkup; }
            set { moduleTitleMarkup = value; }
        }

        private string moduleTitleFormat = "<h2 class='moduletitle'>{0}</h2>";
        public string ModuleTitleFormat
        {
            get { return moduleTitleFormat; }
            set { moduleTitleFormat = value; }
        }

        private string moduleInstanceMarkupTop = string.Empty;
        public string ModuleInstanceMarkupTop
        {
            get { return moduleInstanceMarkupTop; }
            set { moduleInstanceMarkupTop = value; }
        }

        private string moduleInstanceMarkupBottom = string.Empty;
        public string ModuleInstanceMarkupBottom
        {
            get { return moduleInstanceMarkupBottom; }
            set { moduleInstanceMarkupBottom = value; }
        }
        private string instanceFeaturedImageFormat = string.Empty;
        public string InstanceFeaturedImageFormat
        {
            get { return instanceFeaturedImageFormat; }
            set { instanceFeaturedImageFormat = value; }
        }

        private string headerContentFormat = "<div class='flexi-header'>{0}</div>";
        public string HeaderContentFormat
        {
            get { return headerContentFormat; }
            set { headerContentFormat = value; }
        }

        private string footerContentFormat = "<div class='flexi-footer'>{0}</div>";
        public string FooterContentFormat
        {
            get { return footerContentFormat; }
            set { footerContentFormat = value; }
        }

        private string itemMarkup = string.Empty;
        public string ItemMarkup
        {
            get { return itemMarkup; }
            set { itemMarkup = value; }
        }

        private string itemsWrapperFormat = "<div class='flexi-items'>{0}</div>";
        public string ItemsWrapperFormat
        {
            get { return itemsWrapperFormat; }
            set { itemsWrapperFormat = value; }
        }

        private string itemsRepeaterMarkup = "$_Items_$";
        public string ItemsRepeaterMarkup
        {
            get { return itemsRepeaterMarkup; }
            set { itemsRepeaterMarkup = value; }
        }

        private int itemsPerGroup = -1;
        public int ItemsPerGroup
        {
            get { return itemsPerGroup; }
            set { itemsPerGroup = value; }
        }
        private string moduleLinksFormat = "<span class=\"modulelinks flexi-module-links\">{0}{1}{2}{3}{4}{5}</span>";
        /// <summary>
        /// {0} = ModuleSettingsLink,
        /// {1} = AddItemLink,
        /// {2} = EditHeaderLink,
        /// {3} = EditFooterLink,
        /// {4} = ImportLink,
        /// {5} = ExportLink
        /// </summary>
        public string ModuleLinksFormat
        {
            get { return moduleLinksFormat; }
            set { moduleLinksFormat = value; }
        }

        private string moduleSettingsLinkFormat = "&nbsp;<a class='ModuleEditLink' href='{0}'>{1}</a>";
        public string ModuleSettingsLinkFormat
        {
            get { return moduleSettingsLinkFormat; }
            set { moduleSettingsLinkFormat = value; }
        }

        private string addItemLinkFormat = "&nbsp;<a class='ModuleEditLink flexi-item-add' href='{0}'><span class='fa fa-plus'></span>&nbsp;{1}</a>";
        /// <summary>
        /// {0} - url,
        /// {1} - title
        /// </summary>
        public string AddItemLinkFormat
        {
            get { return addItemLinkFormat; }
            set { addItemLinkFormat = value; }
        }

        private string editHeaderLinkFormat = "&nbsp;<a class='ModuleEditLink flexi-header-edit' href='{0}'><span class='fa fa-pencil'></span>&nbsp;{1}</a>";
        public string EditHeaderLinkFormat
        {
            get { return editHeaderLinkFormat; }
            set { editHeaderLinkFormat = value; }
        }

        private string editFooterLinkFormat = "&nbsp;<a class='ModuleEditLink flexi-footer-edit' href='{0}'><span class='fa fa-pencil'></span>&nbsp;{1}</a>";
        public string EditFooterLinkFormat
        {
            get { return editFooterLinkFormat; }
            set { editFooterLinkFormat = value; }
        }

        private string importLinkFormat = "&nbsp;<a class='ModuleEditLink flexi-import-link' href='{0}'><span class='fa fa-upload'></span>&nbsp;{1}</a>";
        public string ImportLinkFormat
        {
            get { return importLinkFormat; }
            set { importLinkFormat = value; }
        }

        private string exportLinkFormat = "&nbsp;<a class='ModuleEditLink flexi-export-link' href='{0}'><span class='fa fa-download'></span>&nbsp;{1}</a>";
        public string ExportLinkFormat
        {
            get { return exportLinkFormat; }
            set { exportLinkFormat = value; }
        }

        private string itemEditLinkFormat = "<a class='flexi-item-edit' href='{0}'><span class='fa fa-pencil'></span>&nbsp;Edit</a>";
        public string ItemEditLinkFormat
        {
            get { return itemEditLinkFormat; }
            set { itemEditLinkFormat = value; }
        }

        private string globalViewMarkup = string.Empty;
        public string GlobalViewMarkup
        {
            get { return globalViewMarkup; }

            set { globalViewMarkup = value; }
        }

        private string globalViewItemMarkup = string.Empty;
        public string GlobalViewItemMarkup
        {
            get { return globalViewItemMarkup; }

            set { globalViewItemMarkup = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            // nothing to render
        }
    }
}