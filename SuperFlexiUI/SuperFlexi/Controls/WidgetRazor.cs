using log4net;
using Microsoft.CSharp;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using Resources;
using SuperFlexiBusiness;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using Reflection = System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace SuperFlexiUI
{
    [ToolboxData("<{0}:WidgetRazor runat=server></{0}:WidgetRazor>")]
    public class WidgetRazor : WebControl
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(typeof(WidgetRazor));
        private ModuleConfiguration config = new ModuleConfiguration();
        private List<Field> fields = new List<Field>();
        private string moduleTitle = string.Empty;
        private SuperFlexiDisplaySettings displaySettings { get; set; }

        StringBuilder strOutput = new StringBuilder();
        StringBuilder strAboveMarkupScripts = new StringBuilder();
        StringBuilder strBelowMarkupScripts = new StringBuilder();
        List<Item> items = new List<Item>();
        List<ItemFieldValue> fieldValues = new List<ItemFieldValue>();
        SiteSettings siteSettings;
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


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadSettings();

            if (module == null)
            {
                Visible = false;
                return;
            }

            if (Page.IsPostBack)
            {
                return;
            }
        }

        protected virtual void LoadSettings()
        {
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
                    SuperFlexiHelpers.SetupScripts(config.MobileMarkupScripts, config, displaySettings, IsEditable, Page.IsPostBack, ClientID, siteSettings, module, CurrentPage, Page, this);
                }
                else
                {
                    SuperFlexiHelpers.SetupScripts(config.MarkupScripts, config, displaySettings, IsEditable, Page.IsPostBack, ClientID, siteSettings, module, CurrentPage, Page, this);
                }

            }

            if (config.MarkupCSS.Count > 0)
            {
                SuperFlexiHelpers.SetupStyle(config.MarkupCSS, config, displaySettings, IsEditable, ClientID, siteSettings, module, CurrentPage, Page, this);
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            string featuredImageUrl = string.Empty;

            featuredImageUrl = String.IsNullOrWhiteSpace(config.InstanceFeaturedImage) ? featuredImageUrl : SiteUtils.GetNavigationSiteRoot() + config.InstanceFeaturedImage;

            //dynamic expando = new ExpandoObject();

            var superFlexiItemClass = CreateClass();

            var itemModels = new List<object> ();

            //var model = expando as IDictionary<string, object>;
            WidgetModel model = new WidgetModel
            {
                Module = new ModuleModel
                {
                    Id = module.ModuleId,
                    Guid = module.ModuleGuid,
                    Title = module.ModuleTitle,
                    TitleElement = module.HeadElement
                },
                Config = Config,
                Page = new PageModel
                {
                    Id = CurrentPage.PageId,
                    Url = CurrentPage.Url,
                    Name = CurrentPage.PageName
                },
                Site = new SiteModel
                {
                    Id = module.SiteId,
                    SkinPath = SiteUtils.DetermineSkinBaseUrl(SiteUtils.GetSkinName(true, this.Page))
                },
                SiteRoot = SiteUtils.GetNavigationSiteRoot()
            };

            //dynamic dModel = model;

            foreach (Item item in items)
            {
                var itemObject = Activator.CreateInstance(superFlexiItemClass);

                bool itemIsEditable = IsEditable || WebUser.IsInRoles(item.EditRoles);
                bool itemIsViewable = WebUser.IsInRoles(item.ViewRoles) || itemIsEditable;
                if (!itemIsViewable)
                {
                    continue;
                }

                string itemEditUrl = SiteUtils.GetNavigationSiteRoot() + "/SuperFlexi/Edit.aspx?pageid=" + PageId + "&mid=" + item.ModuleID + "&itemid=" + item.ItemID;

                SetItemClassProperty(itemObject, "Id", item.ItemID);
                SetItemClassProperty(itemObject, "Guid", item.ItemGuid);
                SetItemClassProperty(itemObject, "SortOrder", item.SortOrder);
                SetItemClassProperty(itemObject, "EditUrl", itemEditUrl);

                //dynamic itemModel = new ExpandoObject();
                //itemModel.Id = item.ItemID;
                //itemModel.Guid = item.ItemGuid;
                //itemModel.SortOrder = item.SortOrder;
                //itemModel.EditUrl = itemEditUrl;

                List<ItemFieldValue> fieldValues = ItemFieldValue.GetItemValues(item.ItemGuid);

                //foreach (Field field in fields)
                //{
                //    foreach (ItemFieldValue fieldValue in fieldValues)
                //    {
                //        if (field.FieldGuid == fieldValue.FieldGuid)
                //        {
                //            if (field.ControlType == "CheckBox")
                //            {
                //                ((IDictionary<string, object>)itemModel)[field.Name] = Convert.ToBoolean(fieldValue.FieldValue);
                //            }
                //            ((IDictionary<string, object>)itemModel)[field.Name] = fieldValue.FieldValue;
                //        }
                //    }
                //}

                //var itemModel = new
                //{
                //    Id = item.ItemID,
                //    Guid = item.ItemGuid,
                //    SortOrder = item.SortOrder,
                //    EditUrl = itemEditUrl,
                //};

                foreach (Field field in fields)
                {
                    foreach (ItemFieldValue fieldValue in fieldValues)
                    {
                        if (field.FieldGuid == fieldValue.FieldGuid)
                        {
                            switch (field.ControlType)
                            {
                                case "CheckBox":
                                    SetItemClassProperty(itemObject, field.Name, Convert.ToBoolean(fieldValue.FieldValue));

                                    break;
                                case "CheckBoxList":
                                case "DynamicCheckBoxList":
                                    SetItemClassProperty(itemObject, field.Name, fieldValue.FieldValue.SplitOnCharAndTrim(';'));
                                    break;
                                case "DateTime":
                                case "Date":
                                    if (!string.IsNullOrWhiteSpace(fieldValue.FieldValue))
                                    {
                                        DateTime.TryParse(fieldValue.FieldValue, out DateTime dt);  
                                        SetItemClassProperty(itemObject, field.Name, dt);
                                    }
                                    break;
                                case "TextBox":
                                default:
                                    SetItemClassProperty(itemObject, field.Name, fieldValue.FieldValue);
                                    break;
                            }
                        }
                    }
                }
                
                itemModels.Add(itemObject);
            }

            model.Items = itemModels;

            var viewPath = config.RelativeSolutionLocation +"/"+ config.ViewName;

            string text;
            try
            {
                text = RazorBridge.RenderPartialToString(viewPath, model, "SuperFlexi");
            }
            catch (Exception ex)
            {
                renderDefaultView(ex.ToString());
            }

            void renderDefaultView(string error = "")
            {
                if (!string.IsNullOrWhiteSpace(error))
                {
                    log.ErrorFormat(
                        "chosen layout ({0}) for _SuperFlexiRazor was not found in skin {1} or SuperFlexi Solution. Perhaps it is in a different skin or Solution. Error was: {2}",
                        config.ViewName,
                        SiteUtils.GetSkinBaseUrl(true, Page),
                        error
                    );
                }
                text = RazorBridge.RenderPartialToString("_SuperFlexiRazor", model, "SuperFlexi");
            }

            output.Write(text);


        }

        private Type CreateClass()
        {
            var className = config.MarkupDefinitionName + config.FieldDefinitionGuid.ToString("N");
            string getFields()
            {

                var sb = new StringBuilder();
                var forConstructor = new StringBuilder();
                forConstructor.AppendLine($"public {className}(){{");

                foreach (Field field in fields)
                {
                    switch (field.ControlType)
                    {
                        case "CheckBox":
                            sb.AppendLine($"public bool {field.Name} {{get;set;}}");

                            break;
                        case "CheckBoxList":
                        case "DynamicCheckBoxList":
                            sb.AppendLine($"public List<string> {field.Name} {{get;set;}}");
                            forConstructor.AppendLine(field.Name + " = new List<string>();");
                            break;
                        case "DateTime":
                        case "Date":
                            sb.AppendLine($"public DateTime {field.Name} {{get;set;}}");
                            break;
                        case "TextBox":
                        default:
                            sb.AppendLine($"public string {field.Name} {{get;set;}}");
                            break;
                    }
                }
                if (forConstructor.Length > 1)
                {
                    forConstructor.AppendLine("}");
                    sb.AppendLine(forConstructor.ToString());
                }
                return sb.ToString();
            }
            var classCode = $@"
                    using System;
                    using System.Collections.Generic;
                    public class {className} {{
                        public int Id {{get;set;}}
                        public Guid Guid {{get;set;}}
                        public int SortOrder {{get;set;}}
                        public string EditUrl {{get;set;}}
                        {getFields()}                        
                    }}";


            log.Info(classCode);
            var options = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,

            };
            //options.ReferencedAssemblies.Add(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/System.dll"));
            options.ReferencedAssemblies.Add(Reflection.Assembly.GetExecutingAssembly().CodeBase.Substring(8));

            var provider = new CSharpCodeProvider();
            var compile = provider.CompileAssemblyFromSource(options, classCode);

            if (compile != null)
            {
                if (compile.Errors.Count > 0)
                {
                    log.Error(compile.Errors);
                }

                return compile.CompiledAssembly.GetType(className);

                //return Activator.CreateInstance(type);
            }
            else
            {
                log.Error("could not compile");
                return null;
            }
        }

        public void SetItemClassProperty(object itemObject, string propName, object propValue)
        {
            itemObject.GetType()
                .GetProperty(propName, Reflection.BindingFlags.Public | Reflection.BindingFlags.Instance)
                .SetValue(itemObject, propValue);
        }
    }

    public class WidgetModel 
    {
        public ModuleModel Module { get; set; }
        public PageModel Page { get; set; }
        public SiteModel Site { get; set; }
        public string SkinPath { get; set; }
        public int SiteId { get; set; }
        public string SiteRoot { get; set; }
        public ModuleConfiguration Config { get; set; }
        public List<object> Items { get; set; } 
    }

    public class ModuleModel
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string TitleElement { get; set; }
    }

    public class PageModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }

    public class SiteModel
    {
        public int Id { get; set; }
        public string MojoLocation { get; set; }
        public string SiteInstancePath { get; set; }
        public string SkinPath { get; set; }
        public Guid CacheGuid { get; set; }
    }
}
