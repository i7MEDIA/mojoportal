// Author:					i7MEDIA (joe davis)
// Created:				    2014-12-22
// Last Modified:			2017-06-14
//
// You must not remove this notice, or any other, from this software.
//
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using SuperFlexiBusiness;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Web.UI;

namespace SuperFlexiUI
{
    public partial class WidgetRazor : UserControl
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(typeof(Widget));
        private ModuleConfiguration config = new ModuleConfiguration();
        private List<Field> fields = new List<Field>();
        private string moduleTitle = string.Empty;
        //private string markupErrorFormat = "SuperFlexi markup definition error when rendering {0} for {1}. Error was {2}";
        StringBuilder strOutput = new StringBuilder();
        StringBuilder strAboveMarkupScripts = new StringBuilder();
        StringBuilder strBelowMarkupScripts = new StringBuilder();
        List<Item> items = new List<Item>();
        List<ItemFieldValue> fieldValues = new List<ItemFieldValue>();
        SiteSettings siteSettings;
        //PageSettings pageSettings;
        Module module;
        public ModuleConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }
		public string SiteRoot { get; set; } = string.Empty;
		public string ImageSiteRoot { get; set; } = string.Empty;
		public bool IsEditable { get; set; } = false;
		public int ModuleId { get; set; } = -1;
		public int PageId { get; set; } = -1;
		public PageSettings CurrentPage { get; set; }

		#endregion

		protected void Page_Load(object sender, EventArgs e)
        {
            //LoadSettings();
            //SetupScripts();

            module = new Module(ModuleId);
            moduleTitle = module.ModuleTitle;

            siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (CurrentPage == null)
			{
				CurrentPage = CacheHelper.GetCurrentPage();
				if (CurrentPage == null)
				{
					log.Info("Can't use CacheHelper.GetCurrentPage() here.");
					CurrentPage = new PageSettings(siteSettings.SiteId, PageId);
				}
			}
			if (config.MarkupDefinition != null)
            {
                displaySettings = config.MarkupDefinition;
            }

            fields = Field.GetAllForDefinition(config.FieldDefinitionGuid);

            if (config.IsGlobalView)
            {
                items = Item.GetForDefinition(config.FieldDefinitionGuid, siteSettings.SiteGuid, config.DescendingSort);
            }
            else
            {
                items = Item.GetForModule(ModuleId, config.DescendingSort);
            }


            if (SiteUtils.IsMobileDevice() && config.MobileMarkupDefinition != null)
            {
                displaySettings = config.MobileMarkupDefinition;
            }

            if (config.MarkupScripts.Count > 0 || (SiteUtils.IsMobileDevice() && config.MobileMarkupScripts.Count > 0))
            {

                if (SiteUtils.IsMobileDevice() && config.MobileMarkupScripts.Count > 0)
                {
                    SuperFlexiHelpers.SetupScripts(config.MobileMarkupScripts, config, displaySettings, IsEditable, IsPostBack, ClientID, siteSettings, module, CurrentPage, Page, this);
                }
                else
                {
                    SuperFlexiHelpers.SetupScripts(config.MarkupScripts, config, displaySettings, IsEditable, IsPostBack, ClientID, siteSettings, module, CurrentPage, Page, this);
				}

            }

            if (config.MarkupCSS.Count > 0)
            {
                SuperFlexiHelpers.SetupStyle(config.MarkupCSS, config, displaySettings, IsEditable, ClientID, siteSettings, module, CurrentPage, Page, this);
            }

            //if (Page.IsPostBack) { return; }

            PopulateControls();

        }

        private void PopulateControls()
        {
            string featuredImageUrl = string.Empty;

            featuredImageUrl = String.IsNullOrWhiteSpace(config.InstanceFeaturedImage) ? featuredImageUrl : SiteUtils.GetNavigationSiteRoot() + config.InstanceFeaturedImage;

            //dynamic expando = new ExpandoObject();

            var itemModels = new List<dynamic>();

            //var model = expando as IDictionary<string, object>;
            WidgetModel model = new WidgetModel();
            model.ModuleGuid = module.ModuleGuid;
            model.ModuleId = module.ModuleId;
            model.ModuleTitle = module.ModuleTitle;
            model.ModuleFriendlyName = config.ModuleFriendlyName;
            model.SortDescending = config.DescendingSort;
            model.GlobalView = config.IsGlobalView;
            model.GlobalViewSortOrder = config.GlobalViewSortOrder;
            model.Header = config.HeaderContent;
            model.Footer = config.FooterContent;
            model.FeaturedImage = featuredImageUrl;
            model.PageId = CurrentPage.PageId;
            model.PageUrl = CurrentPage.Url;
            model.PageName = CurrentPage.PageName;

            //dynamic dModel = model;

            foreach (Item item in items)
            {
                bool itemIsEditable = IsEditable || WebUser.IsInRoles(item.EditRoles);
                bool itemIsViewable = WebUser.IsInRoles(item.ViewRoles) || itemIsEditable;
                if (!itemIsViewable)
                {
                    continue;
                }
                string itemEditUrl = SiteUtils.GetNavigationSiteRoot() + "/SuperFlexi/Edit.aspx?pageid=" + PageId + "&mid=" + item.ModuleID + "&itemid=" + item.ItemID;

                //var itemModel = new ItemModel();

                //itemModel.Id = item.ItemID;
                //itemModel.Guid = item.ItemGuid;
                //itemModel.SortOrder = item.SortOrder;
                //itemModel.EditUrl = itemEditUrl;

                dynamic itemModel = new ExpandoObject();
                itemModel.Id = item.ItemID;
                itemModel.Guid = item.ItemGuid;
                itemModel.SortOrder = item.SortOrder;
                itemModel.EditUrl = itemEditUrl;

                //var itemModel = localexpando as IDictionary<string, object>;

                //itemModel.Add("Id", item.ItemID);
                //itemModel.Add("Guid", item.ItemGuid);
                //itemModel.Add("SortOrder", item.SortOrder);
                //itemModel.Add("EditUrl", itemEditUrl);
                List<ItemFieldValue> fieldValues = ItemFieldValue.GetItemValues(item.ItemGuid);

                //using item.ModuleID here because if we are using a 'global view' we need to be sure the item edit link uses the correct module id.
                //string itemEditLink = itemIsEditable ? String.Format(displaySettings.ItemEditLinkFormat, itemEditUrl) : string.Empty;

                foreach (Field field in fields)
                {
                    foreach (ItemFieldValue fieldValue in fieldValues)
                    {
                        if (field.FieldGuid == fieldValue.FieldGuid)
                        {
                            ((IDictionary<String, String>)itemModel)[field.Name] = fieldValue.FieldValue;
                            //var thisItem = itemModel;
                            //itemModel.AddProperty(field.Name.ToString(), fieldValue.FieldValue);
                            //itemModel = thisItem;
                            //dItemModel[field.Name.ToString()] = fieldValue.FieldValue;
                            //itemModel.Add(field.Name.ToString(), fieldValue.FieldValue);
                        }
                    }
                }
                itemModels.Add(itemModel);

            }

            //dModel["Items"] = itemModels;
            model.Items = itemModels;
            //model.AddProperty("Items", itemModels);
            //theLit.Text = RazorBridge.RenderPartialToString("_SuperFlexiRazor", model, "SuperFlexi");
            theLit.Text = RazorBridge.RenderPartialToString("_SuperFlexiRazor", model, "SuperFlexi");
        }

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }
        #endregion
    }

    //public static class ObjectExtensions
    //{
    //    public static IDictionary<string, object> AddProperty(this object obj, string name, object value)
    //    {
    //        var dictionary = obj.ToDictionary();
    //        dictionary.Add(name, value);
    //        return dictionary;
    //    }

    //    // helper
    //    public static IDictionary<string, object> ToDictionary(this object obj)
    //    {
    //        IDictionary<string, object> result = new Dictionary<string, object>();
    //        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
    //        foreach (PropertyDescriptor property in properties)
    //        {
    //            result.Add(property.Name, property.GetValue(obj));
    //        }
    //        return result;
    //    }
    //}
    public class WidgetModel
    {
        public Guid ModuleGuid { get; set; }
        public int ModuleId { get; set; }
        public string ModuleTitle { get; set; }
        public string ModuleFriendlyName { get; set; }
        public bool SortDescending { get; set; }
        public bool GlobalView { get; set; }
        public int GlobalViewSortOrder { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string FeaturedImage { get; set; }
        public int PageId { get; set; }
        public string PageUrl { get; set; }
        public string PageName { get; set; }

        public List<dynamic> Items { get; set; }
        //public WidgetModel() : base()
        //{ }

        //public WidgetModel(object instance) : base(instance)
        //{ }

    }
    //public class ItemModel : DynamicObject
    //{
    //    public int Id { get; set; }
    //    public Guid Guid { get; set; }
    //    public int SortOrder { get; set; }
    //    public string EditUrl { get; set; }

    //    //public ItemModel() : base()
    //    //{ }

    //    //public ItemModel(object instance) : base(instance)
    //    //{ }
    //}
}