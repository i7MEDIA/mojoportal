using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using log4net;
using mojoPortal.Business;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mojoPortal.Web.ExternalNewsletter
{
    public class SendGridNewsletterProvider : ExternalNewsletterProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SendGridNewsletterProvider));
        private static readonly HttpClient httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

        private string apiKey = string.Empty;
        private bool defaultOptIn = false;
        private List<ExternalNewsletterInfo> configuredLists = new List<ExternalNewsletterInfo>();
        private bool hasConfiguredLists = false;

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (string.IsNullOrEmpty(name))
            {
                name = "SendGridNewsletterProvider";
            }

            base.Initialize(name, config);

            apiKey = config["apiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = ConfigurationManager.AppSettings["SendGridApiKey"] ?? string.Empty;
            }

            if (bool.TryParse(config["defaultOptIn"], out bool optIn))
            {
                defaultOptIn = optIn;
            }

            string rawXml = config["rawConfigXml"];
            if (!string.IsNullOrEmpty(rawXml))
            {
                ParseConfiguredLists(rawXml);
            }
        }

        private void ParseConfiguredLists(string rawXml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<root>" + rawXml + "</root>");

                XmlNodeList listNodes = doc.SelectNodes("//list");
                if (listNodes != null && listNodes.Count > 0)
                {
                    configuredLists.Clear();
                    foreach (XmlNode node in listNodes)
                    {
                        var idAttr = node.Attributes?["id"]?.Value;
                        var titleAttr = node.Attributes?["title"]?.Value;
                        var descAttr = node.Attributes?["description"]?.Value ?? string.Empty;
                        var optInAttr = node.Attributes?["profileOptIn"]?.Value;
                        var sortAttr = node.Attributes?["sortRank"]?.Value;

                        if (!string.IsNullOrEmpty(idAttr) && !string.IsNullOrEmpty(titleAttr))
                        {
                            bool profileOptIn = defaultOptIn;
                            if (!string.IsNullOrEmpty(optInAttr) && bool.TryParse(optInAttr, out bool parsedOptIn))
                            {
                                profileOptIn = parsedOptIn;
                            }

                            int sortRank = 500;
                            if (!string.IsNullOrEmpty(sortAttr) && int.TryParse(sortAttr, out int parsedSort))
                            {
                                sortRank = parsedSort;
                            }

                            ExternalNewsletterInfo info = new ExternalNewsletterInfo
                            {
                                LetterInfoGuid = GenerateDeterministicGuid(Name + ":" + idAttr),
                                ExternalListId = idAttr,
                                ProviderName = Name,
                                Title = titleAttr,
                                Description = descAttr,
                                ProfileOptIn = profileOptIn,
                                SortRank = sortRank
                            };

                            configuredLists.Add(info);
                        }
                    }
                    hasConfiguredLists = configuredLists.Count > 0;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error parsing configured lists in SendGridNewsletterProvider", ex);
            }
        }

        public override List<ExternalNewsletterInfo> GetAvailableNewsletters(Guid siteGuid)
        {
            if (hasConfiguredLists)
            {
                return new List<ExternalNewsletterInfo>(configuredLists);
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                return new List<ExternalNewsletterInfo>();
            }

            // Fallback: Query SendGrid API with in-memory caching
            string cacheKey = "SendGrid_Discovered_Lists_" + Name;
            if (HttpRuntime.Cache?[cacheKey] is List<ExternalNewsletterInfo> cachedLists)
            {
                return cachedLists;
            }

            List<ExternalNewsletterInfo> discovered = FetchListsFromSendGrid();
            if (HttpRuntime.Cache != null && discovered.Count > 0)
            {
                HttpRuntime.Cache.Insert(
                    cacheKey,
                    discovered,
                    null,
                    DateTime.Now.AddMinutes(15),
                    Cache.NoSlidingExpiration);
            }

            return discovered;
        }

        private List<ExternalNewsletterInfo> FetchListsFromSendGrid()
        {
            List<ExternalNewsletterInfo> list = new List<ExternalNewsletterInfo>();

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, "https://api.sendgrid.com/v3/marketing/lists?page_size=100"))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        string json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        JObject parsed = JObject.Parse(json);
                        JArray result = parsed["result"] as JArray;

                        if (result != null)
                        {
                            int rank = 100;
                            foreach (JToken item in result)
                            {
                                string id = item["id"]?.ToString();
                                string name = item["name"]?.ToString();

                                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name))
                                {
                                    list.Add(new ExternalNewsletterInfo
                                    {
                                        LetterInfoGuid = GenerateDeterministicGuid(Name + ":" + id),
                                        ExternalListId = id,
                                        ProviderName = Name,
                                        Title = name,
                                        Description = string.Empty,
                                        ProfileOptIn = defaultOptIn,
                                        SortRank = rank
                                    });
                                    rank += 10;
                                }
                            }
                        }
                    }
                    else
                    {
                        log.Warn($"SendGrid API returned status {response.StatusCode} when fetching lists.");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to fetch lists from SendGrid API", ex);
            }

            return list;
        }

        public override bool Subscribe(ExternalNewsletterInfo newsletter, string email, string firstName = "", string lastName = "")
        {
            if (newsletter == null || string.IsNullOrWhiteSpace(email)) return false;
            if (string.IsNullOrEmpty(apiKey))
            {
                log.Warn("SendGridApiKey is not configured; cannot subscribe user.");
                return false;
            }

            try
            {
                var payload = new
                {
                    list_ids = new[] { newsletter.ExternalListId },
                    contacts = new[]
                    {
                        new
                        {
                            email = email.Trim().ToLowerInvariant(),
                            first_name = firstName ?? string.Empty,
                            last_name = lastName ?? string.Empty
                        }
                    }
                };

                string json = JsonConvert.SerializeObject(payload);

                using (var request = new HttpRequestMessage(HttpMethod.Put, "https://api.sendgrid.com/v3/marketing/contacts"))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        log.Info($"Successfully subscribed {email} to SendGrid list {newsletter.ExternalListId}");
                        return true;
                    }
                    else
                    {
                        string err = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        log.Error($"Failed to subscribe {email} to SendGrid list {newsletter.ExternalListId}. Status: {response.StatusCode}, Body: {err}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception during SendGrid Subscribe for {email}", ex);
                return false;
            }
        }

        public override bool Unsubscribe(ExternalNewsletterInfo newsletter, string email)
        {
            if (newsletter == null || string.IsNullOrWhiteSpace(email)) return false;
            if (string.IsNullOrEmpty(apiKey))
            {
                log.Warn("SendGridApiKey is not configured; cannot unsubscribe user.");
                return false;
            }

            try
            {
                string contactId = FindContactIdByEmail(email.Trim().ToLowerInvariant());
                if (string.IsNullOrEmpty(contactId))
                {
                    // Contact not found on SendGrid; already effectively unsubscribed
                    return true;
                }

                string url = $"https://api.sendgrid.com/v3/marketing/lists/{newsletter.ExternalListId}/contacts?contact_ids={contactId}";
                using (var request = new HttpRequestMessage(HttpMethod.Delete, url))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        log.Info($"Successfully removed {email} from SendGrid list {newsletter.ExternalListId}");
                        return true;
                    }
                    else
                    {
                        string err = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        log.Error($"Failed to remove {email} from SendGrid list {newsletter.ExternalListId}. Status: {response.StatusCode}, Body: {err}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception during SendGrid Unsubscribe for {email}", ex);
                return false;
            }
        }

        private string FindContactIdByEmail(string email)
        {
            try
            {
                var payload = new
                {
                    query = $"email = '{email.ToLowerInvariant()}'"
                };

                string json = JsonConvert.SerializeObject(payload);

                using (var request = new HttpRequestMessage(HttpMethod.Post, "https://api.sendgrid.com/v3/marketing/contacts/search"))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        JObject parsed = JObject.Parse(responseBody);
                        JArray results = parsed["result"] as JArray;
                        if (results != null && results.Count > 0)
                        {
                            return results[0]["id"]?.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error searching for SendGrid contact by email '{email}'", ex);
            }

            return null;
        }

        private static Guid GenerateDeterministicGuid(string input)
        {
            // If the external ID is already a valid Guid, use it directly
            string rawId = input.Contains(":") ? input.Substring(input.IndexOf(':') + 1) : input;
            if (Guid.TryParse(rawId, out Guid parsedGuid))
            {
                return parsedGuid;
            }

            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return new Guid(hash);
            }
        }
    }
}
