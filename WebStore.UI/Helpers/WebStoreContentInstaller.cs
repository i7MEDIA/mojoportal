// Author:					Joe Audette
// Created:				    2011-03-24
// Last Modified:			2011-03-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Text;
using System.Web.Hosting;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using WebStore.Business;
using Resources;

namespace WebStore.UI
{
    public class WebStoreContentInstaller : IContentInstaller
    {
        public void InstallContent(Module module, string configInfo)
        {
            if (string.IsNullOrEmpty(configInfo)) { return; }

            Store store = new Store(module.SiteGuid, module.ModuleId);
            if (store.Guid == Guid.Empty) // No store created yet
            {
                store = new Store();
                store.SiteGuid = module.SiteGuid;
                store.ModuleId = module.ModuleId;
                store.Save();
            }

            

            SiteSettings siteSettings = new SiteSettings(module.SiteId);

            Guid taxClassGuid = Guid.Empty;

            List<TaxClass> taxClasses = TaxClass.GetList(siteSettings.SiteGuid);
            if (taxClasses.Count == 0)
            {
                TaxClass taxClass = new TaxClass();
                taxClass.SiteGuid = siteSettings.SiteGuid;
                taxClass.Title = "Taxable";
                taxClass.Description = "Taxable";
                taxClass.Save();

                taxClass = new TaxClass();
                taxClass.SiteGuid = siteSettings.SiteGuid;
                taxClass.Title = "Not Taxable";
                taxClass.Description = "Not Taxable";
                taxClass.Save();

                taxClassGuid = taxClass.Guid;

            }
            else
            {
                foreach (TaxClass t in taxClasses)
                {
                    if (t.Title == "Not Taxable")
                    {
                        taxClassGuid = t.Guid;
                    }
                }
            }

            SiteUser admin = SiteUser.GetNewestUser(siteSettings);
            

            XmlDocument xml = new XmlDocument();

            using (StreamReader stream = File.OpenText(HostingEnvironment.MapPath(configInfo)))
            {
                xml.LoadXml(stream.ReadToEnd());
            }

            CultureInfo currencyCulture = new CultureInfo("en-US");

            if (xml.DocumentElement.Name == "store")
            {
                // update the store
                XmlAttributeCollection storeAttrributes = xml.DocumentElement.Attributes;

                if (storeAttrributes["storeName"] != null)
                {
                    store.Name = storeAttrributes["storeName"].Value;
                }

                if (storeAttrributes["emailFrom"] != null)
                {
                    store.EmailFrom = storeAttrributes["emailFrom"].Value;
                }

                if (storeAttrributes["currencyCulture"] != null)
                {
                    currencyCulture = new CultureInfo(storeAttrributes["currencyCulture"].Value);
                }

                if (storeAttrributes["ownerName"] != null)
                {
                    store.OwnerName = storeAttrributes["ownerName"].Value;
                }

                if (storeAttrributes["ownerEmail"] != null)
                {
                    store.OwnerEmail = storeAttrributes["ownerEmail"].Value;
                }

                if (storeAttrributes["city"] != null)
                {
                    store.City = storeAttrributes["city"].Value;
                }

                if (storeAttrributes["postalCode"] != null)
                {
                    store.PostalCode = storeAttrributes["postalCode"].Value;
                }

                if (storeAttrributes["phone"] != null)
                {
                    store.Phone = storeAttrributes["phone"].Value;
                }

                if (storeAttrributes["address"] != null)
                {
                    store.Address = storeAttrributes["address"].Value;
                }

                if (storeAttrributes["countryGuid"] != null)
                {
                    string g = storeAttrributes["countryGuid"].Value;
                    if(g.Length == 36)
                    {
                        Guid countryGuid = new Guid(g);
                        store.CountryGuid = countryGuid;
                    }
                }

                if (storeAttrributes["stateGuid"] != null)
                {
                    string s = storeAttrributes["stateGuid"].Value;
                    if(s.Length == 36)
                    {
                        Guid stateGuid = new Guid(s);
                        store.ZoneGuid = stateGuid;
                    }
                }

                foreach (XmlNode descriptionNode in xml.DocumentElement.ChildNodes)
                {
                    if (descriptionNode.Name == "storeDescription")
                    {
                        store.Description = descriptionNode.InnerText;
                        break;
                    }
                }

                store.Save();

                //module settings
                foreach (XmlNode node in xml.DocumentElement.ChildNodes)
                {
                    if (node.Name == "moduleSetting")
                    {
                        XmlAttributeCollection settingAttributes = node.Attributes;

                        if ((settingAttributes["settingKey"] != null) && (settingAttributes["settingKey"].Value.Length > 0))
                        {
                            string key = settingAttributes["settingKey"].Value;
                            string val = string.Empty;
                            if (settingAttributes["settingValue"] != null)
                            {
                                val = settingAttributes["settingValue"].Value;
                            }

                            ModuleSettings.UpdateModuleSetting(module.ModuleGuid, module.ModuleId, key, val);
                        }
                    }
                }

                if (storeAttrributes["productFilesPath"] != null)
                {
                    if (storeAttrributes["productFilesPath"].Value.Length > 0)
                    {
                        string destPath = "~/Data/Sites/" + module.SiteId.ToInvariantString() + "/webstoreproductfiles/";
                        if (!Directory.Exists(HostingEnvironment.MapPath(destPath)))
                        {
                            Directory.CreateDirectory(HostingEnvironment.MapPath(destPath));
                        }

                        IOHelper.CopyFolderContents(HostingEnvironment.MapPath(storeAttrributes["productFilesPath"].Value), HostingEnvironment.MapPath(destPath));
                    }

                }

                if (storeAttrributes["teaserFilesPath"] != null)
                {
                    if (storeAttrributes["teaserFilesPath"].Value.Length > 0)
                    {
                        string destPath = "~/Data/Sites/" + module.SiteId.ToInvariantString() + "/webstoreproductpreviewfiles/";
                        if (!Directory.Exists(HostingEnvironment.MapPath(destPath)))
                        {
                            Directory.CreateDirectory(HostingEnvironment.MapPath(destPath));
                        }

                        IOHelper.CopyFolderContents(HostingEnvironment.MapPath(storeAttrributes["teaserFilesPath"].Value), HostingEnvironment.MapPath(destPath));
                    }

                }

                FullfillDownloadTerms term = new FullfillDownloadTerms();
                term.Name = WebStoreResources.DownloadUnlimited;
                term.Description = WebStoreResources.DownloadUnlimited;
                if (admin != null)
                {
                    term.CreatedBy = admin.UserGuid;
                }
                term.StoreGuid = store.Guid;
                term.Save();

                XmlNode offersNode = null;
                foreach (XmlNode n in xml.DocumentElement.ChildNodes)
                {
                    if (n.Name == "offers")
                    {
                        offersNode = n;
                        break;
                    }
                }

                if (offersNode != null)
                {
                    foreach (XmlNode node in offersNode.ChildNodes)
                    {
                        if (node.Name == "offer")
                        {
                            XmlAttributeCollection offerAttrributes = node.Attributes;

                            Offer offer = new Offer();
                            offer.StoreGuid = store.Guid;
                            offer.Created = DateTime.UtcNow;
                            offer.LastModified = DateTime.UtcNow;

                            if (admin != null)
                            {
                                offer.CreatedBy = admin.UserGuid;
                                offer.LastModifiedBy = admin.UserGuid;
                            }

                            if (offerAttrributes["offerName"] != null)
                            {
                                offer.Name = offerAttrributes["offerName"].Value;
                            }

                            if (offerAttrributes["nameOnProductList"] != null)
                            {
                                offer.ProductListName = offerAttrributes["nameOnProductList"].Value;
                            }

                            if (
                                (offerAttrributes["isVisible"] != null)
                                && (offerAttrributes["isVisible"].Value.ToLower() == "true")
                                )
                            {
                                offer.IsVisible = true;
                            }

                            if (
                                (offerAttrributes["isSpecial"] != null)
                                && (offerAttrributes["isSpecial"].Value.ToLower() == "true")
                                )
                            {
                                offer.IsSpecial = true;
                            }

                            //offer.IsDonation = chkIsDonation.Checked;

                            if (
                                (offerAttrributes["showOfferDetailLink"] != null)
                                && (offerAttrributes["showOfferDetailLink"].Value.ToLower() == "true")
                                )
                            {
                                offer.ShowDetailLink = true;
                            }

                            if (offerAttrributes["primarySort"] != null)
                            {
                                int sort = 5000;
                                if (int.TryParse(offerAttrributes["primarySort"].Value,
                                    out sort))
                                {
                                    offer.SortRank1 = sort;
                                }
                            }

                            if (offerAttrributes["secondarySort"] != null)
                            {
                                int sort = 5000;
                                if (int.TryParse(offerAttrributes["secondarySort"].Value,
                                    out sort))
                                {
                                    offer.SortRank2 = sort;
                                }
                            }

                            if (offerAttrributes["price"] != null)
                            {
                                offer.Price = decimal.Parse(offerAttrributes["price"].Value, NumberStyles.Currency, currencyCulture);
                            }
                           
                           
                            offer.TaxClassGuid = taxClassGuid;
                            

                            offer.Url = "/"
                                    + SiteUtils.SuggestFriendlyUrl(
                                    offer.Name + WebStoreResources.OfferUrlSuffix,
                                    siteSettings);

                            foreach (XmlNode descriptionNode in node.ChildNodes)
                            {
                                if (descriptionNode.Name == "abstract")
                                {
                                    offer.Teaser = descriptionNode.InnerText;
                                    break;
                                }
                            }

                            foreach (XmlNode descriptionNode in node.ChildNodes)
                            {
                                if (descriptionNode.Name == "description")
                                {
                                    offer.Description = descriptionNode.InnerText;
                                    break;
                                }
                            }

                            if (offer.Save())
                            {

                                FriendlyUrl newUrl = new FriendlyUrl();
                                newUrl.SiteId = siteSettings.SiteId;
                                newUrl.SiteGuid = siteSettings.SiteGuid;
                                newUrl.PageGuid = offer.Guid;
                                newUrl.Url = offer.Url.Replace("/", string.Empty);
                                newUrl.RealUrl = "~/WebStore/OfferDetail.aspx?pageid="
                                    + module.PageId.ToInvariantString()
                                    + "&mid=" + module.ModuleId.ToInvariantString()
                                    + "&offer=" + offer.Guid.ToString();

                                newUrl.Save();

                            }

                            foreach (XmlNode productNode in node.ChildNodes)
                            {
                                if (productNode.Name == "product")
                                {
                                    XmlAttributeCollection productAttrributes = productNode.Attributes;

                                    Product product = new Product();
                                    product.StoreGuid = store.Guid;
                                    if (admin != null)
                                    {
                                        product.CreatedBy = admin.UserGuid;

                                    }

                                    product.Created = DateTime.UtcNow;
                                    product.TaxClassGuid = taxClassGuid;

                                    if (productAttrributes["name"] != null)
                                    {
                                        product.Name = productAttrributes["name"].Value;
                                    }

                                    if (productAttrributes["modelNumber"] != null)
                                    {
                                        product.ModelNumber = productAttrributes["modelNumber"].Value;
                                    }

                                    if (productAttrributes["teaser"] != null)
                                    {
                                        product.TeaserFile = productAttrributes["teaser"].Value;
                                    }

                                    

                                    if (productAttrributes["fulfillmentType"] != null)
                                    {
                                        product.FulfillmentType
                                        = Product.FulfillmentTypeFromString(productAttrributes["fulfillmentType"].Value);
                                    }

                                    product.Status = ProductStatus.Available;
                                    product.Weight = 0;
                                    product.QuantityOnHand = 0;

                                   

                                    if (
                                        (productAttrributes["showInProductList"] != null)
                                        && (productAttrributes["showInProductList"].Value.ToLower() == "true")
                                        )
                                    {
                                        product.ShowInProductList = true;
                                    }

                                    if (
                                        (productAttrributes["enableRating"] != null)
                                        && (productAttrributes["enableRating"].Value.ToLower() == "true")
                                        )
                                    {
                                        product.EnableRating = true;
                                    }

                                    if (productAttrributes["primarySort"] != null)
                                    {
                                        int sort = 5000;
                                        if (int.TryParse(productAttrributes["primarySort"].Value,
                                            out sort))
                                        {
                                            product.SortRank1 = sort;
                                        }
                                    }

                                    if (productAttrributes["secondarySort"] != null)
                                    {
                                        int sort = 5000;
                                        if (int.TryParse(productAttrributes["secondarySort"].Value,
                                            out sort))
                                        {
                                            product.SortRank2 = sort;
                                        }
                                    }
                                    
                                    product.Url = "/"
                                            + SiteUtils.SuggestFriendlyUrl(
                                            product.Name + WebStoreResources.ProductUrlSuffix,
                                            siteSettings);

                                    foreach (XmlNode descriptionNode in productNode.ChildNodes)
                                    {
                                        if (descriptionNode.Name == "abstract")
                                        {
                                            product.Teaser = descriptionNode.InnerText;
                                            break;
                                        }
                                    }

                                    foreach (XmlNode descriptionNode in productNode.ChildNodes)
                                    {
                                        if (descriptionNode.Name == "description")
                                        {
                                            product.Description = descriptionNode.InnerText;
                                            break;
                                        }
                                    }

                                    if (product.Save())
                                    {
                                        if (productAttrributes["file"] != null)
                                        {
                                       
                                            ProductFile productFile = new ProductFile(product.Guid);
                                            productFile.ServerFileName = productAttrributes["file"].Value;

                                            if (productAttrributes["fileName"] != null)
                                            {
                                                productFile.FileName = productAttrributes["fileName"].Value;
                                            }

                                            if (admin != null)
                                            {
                                                productFile.CreatedBy = admin.UserGuid;
                                            }

                                            productFile.Save();
                                        }

                                        FriendlyUrl newUrl = new FriendlyUrl();
                                        newUrl.SiteId = siteSettings.SiteId;
                                        newUrl.SiteGuid = siteSettings.SiteGuid;
                                        newUrl.PageGuid = product.Guid;
                                        newUrl.Url = product.Url.Replace("/", string.Empty);
                                        newUrl.RealUrl = "~/WebStore/ProductDetail.aspx?pageid="
                                            + module.PageId.ToInvariantString()
                                            + "&mid=" + module.ModuleId.ToInvariantString()
                                            + "&product=" + product.Guid.ToString();

                                        newUrl.Save();



                                    }

                                    // create offer product
                                    OfferProduct offerProduct = new OfferProduct();
                                    if (admin != null)
                                    {
                                        offerProduct.CreatedBy = admin.UserGuid;
                                    }

                                    offerProduct.ProductGuid = product.Guid;

                                    if (productAttrributes["fulfillmentType"] != null)
                                    {
                                        offerProduct.FullfillType = Convert.ToByte(productAttrributes["fulfillmentType"].Value);
                                    }

                                    if (productAttrributes["sortOrder"] != null)
                                    {
                                        int sort = 100;
                                        if (int.TryParse(productAttrributes["sortOrder"].Value, out sort))
                                        {
                                           offerProduct.SortOrder = sort;
                                        }
                                    }

                                    if (productAttrributes["quantity"] != null)
                                    {
                                        int qty = 1;
                                        if (int.TryParse(productAttrributes["quantity"].Value, out qty))
                                        {
                                            offerProduct.Quantity = qty;
                                        }
                                    }

                                    offerProduct.FullfillType = (byte)product.FulfillmentType;
                                    offerProduct.FullFillTermsGuid = term.Guid;
                                    
                                    offerProduct.OfferGuid = offer.Guid;
                                    offerProduct.Save();

                                }
                            }

                            

                        }

                    }


                }


            }

            

            //create product

            
            



        }

    }
}