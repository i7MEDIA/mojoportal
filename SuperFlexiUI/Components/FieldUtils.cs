/// Author:					i7MEDIA
/// You must not remove this notice, or any other, from this software.
/// 
using log4net;
using mojoPortal.FileSystem;
using mojoPortal.Web;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using Core = mojoPortal.Core;
using SuperFlexiBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Xml;

namespace SuperFlexiUI
{
	public class FieldUtils
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FieldUtils));

        /// <summary>
        /// Saves fields from Definition File to database
        /// </summary>
        /// <param name="fieldDefinitionSrc"></param>
        /// <param name="siteGuid"></param>
        /// <param name="featureGuid"></param>
        /// <param name="deleteOrphans"></param>
        public static void SaveFieldsToDB(ModuleConfiguration config, Guid siteGuid, Guid featureGuid, bool deleteOrphans = false)
        {
            List<Field> fields = ParseFieldDefinitionXml(config, siteGuid);
            //List<MarkupScript> editPageScripts = ParseEditPageScript
            SaveFieldsToDB(fields, siteGuid, featureGuid, deleteOrphans);

        }
        /// <summary>
        /// Saves fields from list to database
        /// </summary>
        /// <param name="definedFields"></param>
        /// <param name="siteGuid"></param>
        /// <param name="featureGuid"></param>
        public static void SaveFieldsToDB(List<Field> definedFields, Guid siteGuid, Guid featureGuid, bool deleteOrphans = false)
        {

			Guid definitionGuid = Guid.Empty;
			if (definedFields.Count > 0)
			{
				definitionGuid = definedFields[0].DefinitionGuid;
			}
			else
			{
				log.Error("definedFields is empty.");
				return;
			}

            List<Field> savedFields = Field.GetAllForDefinition(definitionGuid, true);
            
            FieldComparer fieldComp = new FieldComparer();
            SimpleFieldComparer simpleFieldComp = new SimpleFieldComparer();
            List<Field> matchedFields = savedFields.Where(i => definedFields.Contains(i, simpleFieldComp)).ToList<Field>();

            foreach (Field match in matchedFields)
            {
                Field updatedField = definedFields.Where(i => i.Name == match.Name).Single();
                if (updatedField != null && !savedFields.Contains(updatedField, fieldComp))
                {
                    updatedField.IsDeleted = false; //in case field was deleted, we're going to undelete it 
                    updatedField.FieldGuid = match.FieldGuid;
                    updatedField.SiteGuid = match.SiteGuid;
                    updatedField.FeatureGuid = match.FeatureGuid;
                    updatedField.Save();
                }
            }



            List<Field> newFields = definedFields.Except(matchedFields, simpleFieldComp).ToList();

            foreach(Field newField in newFields)
            {
                newField.SiteGuid = siteGuid;
                newField.FeatureGuid = featureGuid;
                newField.Save();
                matchedFields.Add(newField);
            }

            savedFields = Field.GetAllForDefinition(definitionGuid, true);

            /// orphans are those fields which exist in the db but no longer exist in the definition
            /// if we don't delete the fields, they will continue to show up on the edit page. 
            /// If we delete the fields we have to delete any values associated with them.
            List<Field> orphans = savedFields.Except(matchedFields, simpleFieldComp).ToList();

            if (deleteOrphans)
            {
                foreach (Field orphan in orphans)
                {
                    ItemFieldValue.DeleteByField(orphan.FieldGuid);
                    Field.Delete(orphan.FieldGuid);
                }
            }
            else
            {
                foreach (Field orphan in orphans)
                {
                    Field.MarkAsDeleted(orphan.FieldGuid);
                }
            }
        }

        public static bool DefinitionExists(string path, out XmlDocument doc)
        {
			doc = new XmlDocument();

			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
			if (p == null)
			{
				log.Error("File System Provider Could Not Be Loaded.");
				return false;
			}
			IFileSystem fileSystem = p.GetFileSystem();
			if (fileSystem == null)
			{
				log.Error("File System Could Not Be Loaded.");
				return false;
			}

			//if (path.IndexOf("~", 0) < 0) path = "~" + path;
			//string fullPath = string.Empty;

			//try
			//{
			//	fullPath = HttpContext.Current.Server.MapPath(path);
			//}
			//catch(System.Web.HttpException ex)
			//{
			//	fullPath = path;
			//}

			

			//if (!File.Exists(fullPath))
			//{
			//	path = "~" + path;
			//	fullPath = HttpContext.Current.Server.MapPath(path);
			//}
			if (fileSystem.FileExists(path))
            //if (File.Exists(fullPath))
            {
				WebFile webFile = fileSystem.RetrieveFile(path);
                //FileInfo fileInfo = new FileInfo(fullPath);

                try
                {
					//doc.Load(fileInfo.FullName);
					doc = Core.Helpers.XmlHelper.GetXmlDocument(webFile.Path);
                    return true;
                }
                catch (XmlException ex)
                {
                    log.Error(ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //public static List<MarkupScript> ParseScriptsFromXml(ModuleConfiguration config)
        //{
        //    List<MarkupScript> scripts = new List<MarkupScript>();
        //    string fullPath = string.Empty;
        //    XmlDocument doc = new XmlDocument();
        //    if (DefinitionExists(config.FieldDefinitionSrc, out doc))
        //    {
        //        XmlNode node = doc.DocumentElement.SelectSingleNode("/Fields/Scripts");

        //        if (node == null) return scripts;

        //        try
        //        {
        //            scripts = SuperFlexiHelpers.ParseScriptsFromXmlNode(node);
        //        }
        //        catch (System.Xml.XmlException ex)
        //        {
        //            log.Error(ex);
        //        }
        //    }
        //    return scripts;
        //}



        /// <summary>
        /// Creates a list of Field from field definition xml file.
        /// </summary>
        /// <param name="fieldDefinitionSrc"></param>
        /// <returns>IList</returns>
        public static List<Field> ParseFieldDefinitionXml(ModuleConfiguration config, Guid siteGuid)
        {
			List<Field> fields = new List<Field>();
            //string fullPath = string.Empty;
            XmlDocument doc = new XmlDocument();

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
			if (p == null)
			{
				log.Error("File System Provider Could Not Be Loaded.");
				return fields;
			}
			IFileSystem fileSystem = p.GetFileSystem();
			if (fileSystem == null)
			{
				log.Error("File System Could Not Be Loaded.");
				return fields;
			}



			//implemented "solutions" on 9/13/2017 (mojoPortal 2.6.0.0) which allows for markup definitions and field definitions to be wrapped up in a single folder
			//b/c of this, we added the ability to pull the field definition file from the location of the markup definition (.sfmarkup) file w/o needing to use the full path in the fieldDefinitionSrc property

			string solutionFieldDefSrc = string.Empty;

			if (config.FieldDefinitionSrc.StartsWith("~/"))
			{
				solutionFieldDefSrc = config.FieldDefinitionSrc;
			}
			else if (config.FieldDefinitionSrc.StartsWith("/"))
			{
				solutionFieldDefSrc = "~" + config.FieldDefinitionSrc;
			}
			//else if (File.Exists(System.Web.Hosting.HostingEnvironment.MapPath(config.MarkupDefinitionFile)))
			else
			{
				var sfMarkupFile = fileSystem.RetrieveFile(config.MarkupDefinitionFile);
				string sfFieldPath = fileSystem.CombinePath(sfMarkupFile.FolderVirtualPath, config.FieldDefinitionSrc);
				var sfFieldFile = fileSystem.RetrieveFile(sfFieldPath);

				//FileInfo fileInfo = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath(config.MarkupDefinitionFile));

				//solutionFieldDefSrc = fileInfo.DirectoryName + "/" + config.FieldDefinitionSrc;

				solutionFieldDefSrc = sfFieldFile.VirtualPath;
			}



            if (DefinitionExists(solutionFieldDefSrc, out doc))
            {
                XmlNode node = doc.DocumentElement.SelectSingleNode("/Fields");
                if (node != null)
                {
                    XmlAttributeCollection attribs = node.Attributes;

                    string definitionName = string.Empty;
                    Guid definitionGuid = Guid.NewGuid();
                    if (attribs["definitionName"] != null) definitionName = attribs["definitionName"].Value;
                    if (attribs["definitionGuid"] != null) definitionGuid = Guid.Parse(attribs["definitionGuid"].Value);

					if (definitionGuid != config.FieldDefinitionGuid)
					{
						log.Error($@"
							SuperFlexi Solution [{config.MarkupDefinitionName}] located at [{config.MarkupDefinitionFile}] uses 
							fieldDefinitionGuid = [{config.FieldDefinitionGuid.ToString()}] but the field definition at [{solutionFieldDefSrc}] 
							uses definitionGuid = [{definitionGuid}]. Items will not display properly and may end up corrupted.");

						return null;
					}

                    foreach (XmlNode childNode in node)
                    {
                        if (childNode.Name == "Field")
                        {
                            try
                            {
                                Field field = new Field();
                                XmlAttributeCollection itemDefAttribs = childNode.Attributes;

                                field.DefinitionName = definitionName;
                                field.DefinitionGuid = definitionGuid;
                                if (itemDefAttribs["name"] != null) field.Name = itemDefAttribs["name"].Value;
                                if (itemDefAttribs["label"] != null) field.Label = itemDefAttribs["label"].Value;
                                if (itemDefAttribs["defaultValue"] != null) field.DefaultValue = itemDefAttribs["defaultValue"].Value;
                                if (itemDefAttribs["controlType"] != null) field.ControlType = itemDefAttribs["controlType"].Value;
                                if (itemDefAttribs["controlSrc"] != null) field.ControlSrc = itemDefAttribs["controlSrc"].Value;
                                if (itemDefAttribs["dataType"] != null) field.DataType = itemDefAttribs["dataType"].Value;
                                field.IsList = XmlUtils.ParseBoolFromAttribute(itemDefAttribs, "isList", field.IsList);
								field.SortOrder = XmlUtils.ParseInt32FromAttribute(itemDefAttribs, "sortOrder", field.SortOrder);
                                if (itemDefAttribs["helpKey"] != null) field.HelpKey = itemDefAttribs["helpKey"].Value;
                                field.Required = XmlUtils.ParseBoolFromAttribute(itemDefAttribs, "required", field.Required);
                                if (itemDefAttribs["requiredMessageFormat"] != null) field.RequiredMessageFormat = itemDefAttribs["requiredMessageFormat"].Value;
                                if (itemDefAttribs["regex"] != null) field.Regex = itemDefAttribs["regex"].Value;
                                if (itemDefAttribs["regexMessageFormat"] != null) field.RegexMessageFormat = itemDefAttribs["regexMessageFormat"].Value;
                                if (itemDefAttribs["token"] != null && !String.IsNullOrWhiteSpace(itemDefAttribs["token"].Value)) field.Token = itemDefAttribs["token"].Value;

                                field.Searchable = XmlUtils.ParseBoolFromAttribute(itemDefAttribs, "isSearchable", field.Searchable);
                                if (itemDefAttribs["editPageControlWrapperCssClass"] != null) field.EditPageControlWrapperCssClass = itemDefAttribs["editPageControlWrapperCssClass"].Value;
                                if (itemDefAttribs["editPageLabelCssClass"] != null) field.EditPageLabelCssClass = itemDefAttribs["editPageLabelCssClass"].Value;
                                if (itemDefAttribs["editPageControlCssClass"] != null) field.EditPageControlCssClass = itemDefAttribs["editPageControlCssClass"].Value;
                                field.DatePickerIncludeTimeForDate = XmlUtils.ParseBoolFromAttribute(itemDefAttribs, "datePickerIncludeTimeForDate", field.DatePickerIncludeTimeForDate);
                                field.DatePickerShowMonthList = XmlUtils.ParseBoolFromAttribute(itemDefAttribs, "datePickerShowMonthList", field.DatePickerShowMonthList);
                                field.DatePickerShowYearList = XmlUtils.ParseBoolFromAttribute(itemDefAttribs, "datePickerShowYearList", field.DatePickerShowYearList);
                                if (itemDefAttribs["datePickerYearRange"] != null) field.DatePickerYearRange = itemDefAttribs["datePickerYearRange"].Value;
                                if (itemDefAttribs["imageBrowserEmptyUrl"] != null) field.ImageBrowserEmptyUrl = itemDefAttribs["imageBrowserEmptyUrl"].Value;
                                field.CheckBoxReturnBool = XmlUtils.ParseBoolFromAttribute(itemDefAttribs, "checkBoxReturnBool", field.CheckBoxReturnBool);
                                if (itemDefAttribs["checkBoxReturnValueWhenTrue"] != null) field.CheckBoxReturnValueWhenTrue = itemDefAttribs["checkBoxReturnValueWhenTrue"].Value;
                                if (itemDefAttribs["checkBoxReturnValueWhenFalse"] != null) field.CheckBoxReturnValueWhenFalse = itemDefAttribs["checkBoxReturnValueWhenFalse"].Value;
                                if (itemDefAttribs["dateFormat"] != null) field.DateFormat = itemDefAttribs["dateFormat"].Value;
                                if (itemDefAttribs["textBoxMode"] != null) field.TextBoxMode = itemDefAttribs["textBoxMode"].Value;
                                field.IsDeleted = XmlUtils.ParseBoolFromAttribute(itemDefAttribs, "isDeleted", field.IsDeleted);
                                field.IsGlobal = XmlUtils.ParseBoolFromAttribute(itemDefAttribs, "isGlobal", field.IsGlobal);
								if (itemDefAttribs["viewRoles"] != null)
								{
									string viewRoles = itemDefAttribs["viewRoles"].Value;
									if (String.IsNullOrWhiteSpace(viewRoles))
									{
										viewRoles = "All Users;";
									}
									field.ViewRoles = viewRoles;
								}
								if (itemDefAttribs["editRoles"] != null) field.EditRoles = itemDefAttribs["editRoles"].Value;

								StringBuilder options = new StringBuilder();
                                StringBuilder attributes = new StringBuilder();
                                foreach (XmlNode subNode in childNode)
                                {
                                    switch (subNode.Name)
                                    {
                                        case "Options":
											options = Core.Helpers.XmlHelper.GetKeyValuePairsAsStringBuilder(subNode.ChildNodes);
											//GetKeyValuePairs(subNode.ChildNodes, out options);
                                            break;
                                        case "Attributes":
											attributes = Core.Helpers.XmlHelper.GetKeyValuePairsAsStringBuilder(subNode.ChildNodes);
                                            //GetKeyValuePairs(subNode.ChildNodes, out attributes);
                                            break;
                                        case "PreTokenString":
                                            field.PreTokenString = subNode.InnerText.Trim();
                                            break;
                                        case "PostTokenString":
                                            field.PostTokenString = subNode.InnerText.Trim();
                                            break;
										case "PreTokenStringWhenTrue":
											field.PreTokenStringWhenTrue = subNode.InnerText.Trim();
											break;
										case "PostTokenStringWhenTrue":
											field.PostTokenStringWhenTrue = subNode.InnerText.Trim();
											break;
										case "PreTokenStringWhenFalse":
											field.PreTokenStringWhenFalse = subNode.InnerText.Trim();
											break;
										case "PostTokenStringWhenFalse":
											field.PostTokenStringWhenFalse = subNode.InnerText.Trim();
											break;
									}
                                }

                                if (options.Length > 0)
                                {
                                    field.Options = options.ToString();
                                }

                                if (attributes.Length > 0)
                                {
                                    field.Attributes = attributes.ToString();
                                }

                                fields.Add(field);
                            }
                            catch (System.Xml.XmlException ex)
                            {
                                log.Error(ex);
                            }
                        }
                        else if (childNode.Name == "Scripts")
                        {
                            try
                            {
                                config.EditPageScripts = SuperFlexiHelpers.ParseScriptsFromXmlNode(childNode);
                            }
                            catch (System.Xml.XmlException ex)
                            {
                                log.Error(ex);
                            }
                        }
                        else if (childNode.Name == "Styles")
                        {
                            try
                            {
                                config.EditPageCSS = SuperFlexiHelpers.ParseCssFromXmlNode(childNode);
                            }
                            catch (System.Xml.XmlException ex)
                            {
                                log.Error(ex);
                            }
                        }
                        else if (childNode.Name == "SearchDefinition")
                        {
                            try
                            {
                                SuperFlexiHelpers.ParseSearchDefinition(childNode, definitionGuid, siteGuid);
                            }
                            catch (XmlException ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                }
            }
			else
			{
				log.Error($"Definition \"{solutionFieldDefSrc}\" does not exist.");
			}
            fields.Sort();
            return fields;
        }

        //private static void GetKeyValuePairs(XmlNodeList nodes, out StringBuilder sb)
        //{
            
        //    sb = new StringBuilder();
        //    foreach (XmlNode node in nodes)
        //    {
        //        XmlAttributeCollection attribs = node.Attributes;
        //        if (attribs["name"] != null)
        //        {
        //            if (!String.IsNullOrWhiteSpace(attribs["name"].Value))
        //            {
        //                string opValue = " ";
        //                if (attribs["value"] != null && !String.IsNullOrWhiteSpace(attribs["value"].Value))
        //                {
        //                    opValue = attribs["value"].Value;
        //                }
        //                string option = attribs["name"].Value + "|" + opValue;

        //                sb.Append(option + ";");
        //            }
        //        }
        //    }
            
        //}

        public static void GetFieldAttributes(string p, out AttributeCollection attribCollection)
        {
            List<string> attributes = p.SplitOnChar(';');
            StateBag bag = new StateBag();
            attribCollection = new AttributeCollection(bag);
            foreach (string attribute in attributes)
            {
                List<string> attr = attribute.SplitOnCharAndTrim('|');
                attribCollection.Add(attr[0], attr[1]);
            }
        }

        public static bool EnsureFields(Guid siteGuid, ModuleConfiguration config, out List<Field> savedFields, bool deleteOrphanedFieldValues = false)
        {
            savedFields = null;
            List<Field> definedFields = ParseFieldDefinitionXml(config, siteGuid);
            FieldComparer fieldComp = new FieldComparer();
            if (config.FieldDefinitionGuid != Guid.Empty)
            {
                savedFields = Field.GetAllForDefinition(config.FieldDefinitionGuid);
            }
            else
            {
                return false;
            }

            bool fieldsChanged = false;



            if (savedFields != null)
            {

                if (savedFields.Count != definedFields.Count)
                {
                    fieldsChanged = true;
                }
                else
                {
                    foreach (Field definedField in definedFields)
                    {
                        if (!savedFields.Contains(definedField, fieldComp))

                        {
                            fieldsChanged = true;
                            break;
                        }
                    }

                }
            }

            if (savedFields == null || fieldsChanged)
            {
                FieldUtils.SaveFieldsToDB(definedFields, siteGuid, config.FeatureGuid, deleteOrphanedFieldValues);
                savedFields = Field.GetAllForDefinition(config.FieldDefinitionGuid);
            }

            if (savedFields == null) return false;

            return true;
        }
	}
}