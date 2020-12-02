using System;
using System.Collections.Generic;
using System.Linq;

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Configuration;

using SuperFlexiBusiness;


namespace SuperFlexiUI
{
	public class SuperFlexiDeleteHandler : ContentDeleteHandlerProvider
    {
        public SuperFlexiDeleteHandler()
        { }

        public override void DeleteContent(int moduleId, Guid moduleGuid)
        {
            ItemFieldValue.DeleteByModule(moduleGuid);
            Item.DeleteByModule(moduleGuid);

            Module module = new Module(moduleGuid);
            ModuleConfiguration config = new ModuleConfiguration(module);

            //having field definitions in the database when the fields aren't used by any module is just clutter.
            //we'll delete the field definitions from the database when they aren't used but of course we don't touch the actual field definition XML files.
            bool deleteOrphanFields = ConfigHelper.GetBoolProperty("SuperFlexi:DeleteFieldDefinitionsWhenNotUsed", true);

            //we didn't implement the delete handler for a year or so after building superflexi so we have a lot of instances in the wild that probably have orphaned items
            //if we upgrade those to this version, create a module instance, and then delete it, these orphaned items will be removed
            bool deleteOrphanItems = ConfigHelper.GetBoolProperty("SuperFlexi:DeleteOrphanedItemsWhenDeletingModules", true);

            //clean up search definitions 
            bool deleteOrphanSearchDefinitions = ConfigHelper.GetBoolProperty("SuperFlexi:DeleteOrphanedSearchDefinitions", true);

            if ( deleteOrphanFields || deleteOrphanItems || deleteOrphanSearchDefinitions)
            {
                List<Item> sflexiItems = Item.GetForModule(module.ModuleId);
                List<Guid> definitionGuids = new List<Guid>();


                foreach (Item item in sflexiItems)
                {
                    //add definitionGuids here b/c we'll need them for all three checks
                    definitionGuids.Add(item.DefinitionGuid);

                    if (deleteOrphanItems)
                    {
                        Module checkModule = new Module(item.ModuleGuid);
                        if (checkModule == null)
                        {
                            ItemFieldValue.DeleteByModule(item.ModuleGuid);
                            Item.DeleteByModule(item.ModuleGuid);
                        }
                    }
                }

                if (deleteOrphanFields)
                {
                    definitionGuids = definitionGuids.Distinct().ToList();

                    foreach (Guid guid in definitionGuids)
                    {
                        List<Item> definitionItems = Item.GetForDefinition(guid, module.SiteGuid);
                        if (definitionItems.Count == 0)
                        {
                            //delete field definitions when there are no more modules using them
                            Field.DeleteByDefinition(guid);

                            if (deleteOrphanSearchDefinitions)
                            {
                                //delete search definitions when there are no more modules using their definition
                                SearchDef.DeleteByFieldDefinition(guid);
                            }
                        }
                    }
                }
            }
        }
    }
}
