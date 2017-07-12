/// Author:					
/// Created:				2008-03-01
/// Last Modified:		    2008-07-19
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using GCheckout.Util;
using GCheckout.AutoGen;
using GCheckout.AutoGen.Extended;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.Commerce;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.Services
{
    
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GCheckoutNotificationHandler : IHttpHandler
    {
        private HttpContext requestContext;
        private static readonly ILog log = LogManager.GetLogger(typeof(GCheckoutNotificationHandler));

        private const string MerchantNode = "MERCHANT_DATA_HIDDEN";
        private const string GCheckoutMerchantDataNamespace = "xmlns=\"http://checkout.google.com/schema/2\"";

        protected string NotificationSerialNumber = string.Empty;
        private string RequestXml = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            //log.Info("Recieved Request");
            //log.Info(context.Request.ToString());

            requestContext = context;

            HandleNotification();
            SendResponse();


        }

        private void SendResponse()
        {
            if (requestContext == null) return;

            requestContext.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            requestContext.Response.AppendHeader("Accept", "application/xml");

            XmlTextWriter xmlTextWriter = new XmlTextWriter(requestContext.Response.OutputStream, encoding);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("notification-acknowledgment");
            xmlTextWriter.WriteAttributeString("xmlns", "http://checkout.google.com/schema/2");
            xmlTextWriter.WriteAttributeString("serial-number", NotificationSerialNumber);
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();

            xmlTextWriter.Close();

            requestContext.Response.StatusCode = 200;

        }

        private void HandleNotification()
        {
            if (requestContext == null) return;

            Stream RequestStream = requestContext.Request.InputStream;
            StreamReader RequestStreamReader = new StreamReader(RequestStream);
            RequestXml = RequestStreamReader.ReadToEnd();
            RequestStream.Close();



            if (RequestXml.Length == 0) return;

            

            try
            {
                object requestObject = EncodeHelper.Deserialize(RequestXml);

                if (requestObject is NewOrderNotificationExtended)
                {
                    NewOrderNotificationExtended n1
                      = requestObject as NewOrderNotificationExtended;

                    HandleNewOrderNotificationExtended(n1);
                    return;

                }


                if (requestObject is GCheckout.AutoGen.RiskInformationNotification)
                {
                    RiskInformationNotification n2
                      = requestObject as RiskInformationNotification;

                    HandleRiskInformationNotification(n2);
                    return;

                }

                if (requestObject is GCheckout.AutoGen.ChargeAmountNotification)
                {
                    ChargeAmountNotification n3
                      = requestObject as ChargeAmountNotification;

                    HandleChargeAmountNotification(n3);
                    return;

                }

                if (requestObject is GCheckout.AutoGen.ChargebackAmountNotification)
                {
                    ChargebackAmountNotification n4
                      = requestObject as ChargebackAmountNotification;

                    HandleChargebackAmountNotification(n4);
                    return;

                }

                if (requestObject is GCheckout.AutoGen.AuthorizationAmountNotification)
                {
                    AuthorizationAmountNotification n5
                      = requestObject as AuthorizationAmountNotification;

                    HandleAuthorizationAmountNotification(n5);
                    return;

                }

                if (requestObject is GCheckout.AutoGen.RefundAmountNotification)
                {
                    RefundAmountNotification n6
                      = requestObject as RefundAmountNotification;

                    HandleRefundAmountNotification(n6);
                    return;

                }

                if (requestObject is OrderStateChangeNotification)
                {
                    OrderStateChangeNotification n7
                      = requestObject as OrderStateChangeNotification;

                    HandleOrderStateChangeNotification(n7);
                    return;

                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error(RequestXml);

                // after testing there me be some invalid posts from google
                // for deleted orders or carts. They'll keep sending them for 30 days 
                // or until they get a 200 ok response
                // so this option is to allow clearing those without wating 30 days
                // other than during development this should be false
                bool alwaysSendNormalResponse = ConfigHelper.GetBoolProperty("GCheckoutForceNormalResponse", false);

                if (!alwaysSendNormalResponse)
                    throw (ex);
            }


            // probably some hacking or random traffic if we reach here but log it


            //log.Error(RequestXml);
            log.Info("google notification: " + RequestXml);




        }

        

        private string GetMerchantData(XmlNode[] merchantNodes)
        {
            foreach (XmlNode node in merchantNodes)
            {
                if (node.Name == MerchantNode)
                    return node.InnerXml;
            }

            return string.Empty;


        }

        private MerchantData DeserializeMerchantData(string merchantDataString)
        {
            if (merchantDataString == null) return null;
            if (merchantDataString.Length == 0) return null;
            merchantDataString = merchantDataString.Replace(GCheckoutMerchantDataNamespace, string.Empty);


            object obj = SerializationHelper.DeserializeFromString(typeof(MerchantData), SerializationHelper.RestoreXmlDeclaration(merchantDataString));
            if (obj != null)
                return obj as MerchantData;


            return null;

        }

        private void HandleNewOrderNotificationExtended(NewOrderNotificationExtended newOrder)
        {
            NotificationSerialNumber = newOrder.serialnumber;

            string merchantDataString = GetMerchantData(newOrder.shoppingcart.merchantprivatedata.Any);
            MerchantData merchantData = DeserializeMerchantData(merchantDataString);

            if (merchantData != null)
            {
                GCheckoutNotificationHandlerProvider provider
                    = GCheckoutNotificationManager.Providers[merchantData.ProviderName];

                if (provider != null)
                {
                    provider.HandleNewOrderNotificationExtended(RequestXml, newOrder, merchantData);

                    return;
                }


            }


            // if no providers found just log it
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.SiteGuid = siteSettings.SiteGuid;
            gLog.RawResponse = RequestXml;
            gLog.NotificationType = "NewOrderNotification";
            gLog.SerialNumber = newOrder.serialnumber;
            gLog.OrderNumber = newOrder.googleordernumber;
            gLog.OrderTotal = newOrder.ordertotal.Value;
            gLog.BuyerId = newOrder.buyerid.ToString(CultureInfo.InvariantCulture);
            gLog.FullfillState = newOrder.fulfillmentorderstate.ToString();
            gLog.FinanceState = newOrder.financialorderstate.ToString();
            gLog.ShippingTotal = newOrder.ShippingCost;
            gLog.TaxTotal = newOrder.orderadjustment.totaltax.Value;
            //gLog.DiscountTotal = ext.orderadjustment.adjustmenttotal.Value;
            gLog.EmailListOptIn = newOrder.buyermarketingpreferences.emailallowed;
            gLog.GTimestamp = newOrder.timestamp;
            gLog.CartXml = merchantDataString;
            gLog.Save();


        }

        private void HandleOrderStateChangeNotification(OrderStateChangeNotification notification)
        {
            NotificationSerialNumber = notification.serialnumber;

            string providerName = GoogleCheckoutLog.GetProviderNameFromOrderNumber(notification.googleordernumber);

            if (providerName.Length > 0)
            {
                GCheckoutNotificationHandlerProvider provider
                    = GCheckoutNotificationManager.Providers[providerName];

                if (provider != null)
                {
                    provider.HandleOrderStateChangeNotification(RequestXml, notification);

                    return;
                }

            }
            

            // if no provider found just log it
            log.Info("No GCheckoutNotification Provider found for google order " + notification.googleordernumber + " so just logging it");

            Guid orderGuid = GoogleCheckoutLog.GetCartGuidFromOrderNumber(notification.googleordernumber);

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.SiteGuid = siteSettings.SiteGuid;
            gLog.RawResponse = RequestXml;
            gLog.NotificationType = "OrderStateChangeNotification";
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.FinanceState = notification.newfinancialorderstate.ToString();
            gLog.FullfillState = notification.newfulfillmentorderstate.ToString();
            gLog.GTimestamp = notification.timestamp;
            gLog.AvsResponse = notification.reason;
            gLog.CartGuid = orderGuid;
            
            gLog.Save();

            
        }




        private void HandleRiskInformationNotification(RiskInformationNotification notification)
        {
            NotificationSerialNumber = notification.serialnumber;

            string providerName = GoogleCheckoutLog.GetProviderNameFromOrderNumber(notification.googleordernumber);

            if (providerName.Length > 0)
            {
                GCheckoutNotificationHandlerProvider provider
                    = GCheckoutNotificationManager.Providers[providerName];

                if (provider != null)
                {
                    provider.HandleRiskInformationNotification(RequestXml, notification);

                    return;
                }

            }

            // if no provider found just log it
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.SiteGuid = siteSettings.SiteGuid;
            gLog.RawResponse = RequestXml;
            gLog.NotificationType = "RiskInformationNotification";
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.GTimestamp = notification.timestamp;
            gLog.AvsResponse = notification.riskinformation.avsresponse;
            gLog.CvnResponse = notification.riskinformation.cvnresponse;
            gLog.BuyerId = notification.riskinformation.ipaddress;
            gLog.Save();

            
        }

        private void HandleChargeAmountNotification(ChargeAmountNotification notification)
        {
            NotificationSerialNumber = notification.serialnumber;

            string providerName = GoogleCheckoutLog.GetProviderNameFromOrderNumber(notification.googleordernumber);

            if (providerName.Length > 0)
            {
                GCheckoutNotificationHandlerProvider provider
                    = GCheckoutNotificationManager.Providers[providerName];

                if (provider != null)
                {
                    provider.HandleChargeAmountNotification(RequestXml, notification);

                    return;
                }

            }

            // if no provider found just log it
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.SiteGuid = siteSettings.SiteGuid;
            gLog.RawResponse = RequestXml;
            gLog.NotificationType = "ChargeAmountNotification";
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.GTimestamp = notification.timestamp;
            gLog.TotalChgAmt = notification.totalchargeamount.Value;
            gLog.LatestChgAmt = notification.latestchargeamount.Value;

            gLog.Save();

            

        }

        private void HandleChargebackAmountNotification(ChargebackAmountNotification notification)
        {
            NotificationSerialNumber = notification.serialnumber;

            string providerName = GoogleCheckoutLog.GetProviderNameFromOrderNumber(notification.googleordernumber);

            if (providerName.Length > 0)
            {
                GCheckoutNotificationHandlerProvider provider
                    = GCheckoutNotificationManager.Providers[providerName];

                if (provider != null)
                {
                    provider.HandleChargebackAmountNotification(RequestXml, notification);

                    return;
                }

            }

            // if no provider found just log it
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.SiteGuid = siteSettings.SiteGuid;
            gLog.NotificationType = "ChargebackAmountNotification";
            gLog.RawResponse = RequestXml;
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.GTimestamp = notification.timestamp;
            gLog.LatestChargeback = notification.latestchargebackamount.Value;
            gLog.TotalChargeback = notification.totalchargebackamount.Value;

            gLog.Save();

            


        }

        private void HandleAuthorizationAmountNotification(AuthorizationAmountNotification notification)
        {
            NotificationSerialNumber = notification.serialnumber;

            string providerName = GoogleCheckoutLog.GetProviderNameFromOrderNumber(notification.googleordernumber);

            if (providerName.Length > 0)
            {
                GCheckoutNotificationHandlerProvider provider
                    = GCheckoutNotificationManager.Providers[providerName];

                if (provider != null)
                {
                    provider.HandleAuthorizationAmountNotification(RequestXml, notification);

                    return;
                }

            }

            // if no provider found just log it
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.SiteGuid = siteSettings.SiteGuid;
            gLog.NotificationType = "AuthorizationAmountNotification";
            gLog.RawResponse = RequestXml;
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.GTimestamp = notification.timestamp;
            gLog.AuthAmt = notification.authorizationamount.Value;
            gLog.AuthExpDate = notification.authorizationexpirationdate;
            gLog.CvnResponse = notification.cvnresponse;
            gLog.AvsResponse = notification.avsresponse;

            gLog.Save();

            
        }

        private void HandleRefundAmountNotification(RefundAmountNotification notification)
        {
            NotificationSerialNumber = notification.serialnumber;

            string providerName = GoogleCheckoutLog.GetProviderNameFromOrderNumber(notification.googleordernumber);

            if (providerName.Length > 0)
            {
                GCheckoutNotificationHandlerProvider provider
                    = GCheckoutNotificationManager.Providers[providerName];

                if (provider != null)
                {
                    provider.HandleRefundAmountNotification(RequestXml, notification);

                    return;
                }

            }

            // if no provider found just log it
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.SiteGuid = siteSettings.SiteGuid;
            gLog.NotificationType = "RefundAmountNotification";
            gLog.RawResponse = RequestXml;
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.GTimestamp = notification.timestamp;

            gLog.LatestRefundAmt = notification.latestrefundamount.Value;
            gLog.TotalRefundAmt = notification.totalrefundamount.Value;


            gLog.Save();


        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
