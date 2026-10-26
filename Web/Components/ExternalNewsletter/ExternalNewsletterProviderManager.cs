using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Web.Configuration;
using log4net;
using mojoPortal.Business;

namespace mojoPortal.Web.ExternalNewsletter
{
    public static class ExternalNewsletterProviderManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExternalNewsletterProviderManager));
        private static ExternalNewsletterProviderCollection providerCollection;
        private static readonly object syncLock = new object();

        static ExternalNewsletterProviderManager()
        {
            Initialize();
        }

        private static void Initialize()
        {
            lock (syncLock)
            {
                providerCollection = new ExternalNewsletterProviderCollection();

                try
                {
                    ExternalNewsletterProviderConfig config = ExternalNewsletterProviderConfig.GetConfig();
                    if (config?.Providers != null && config.Providers.Count > 0)
                    {
                        ProvidersHelper.InstantiateProviders(
                            config.Providers,
                            providerCollection,
                            typeof(ExternalNewsletterProvider));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Failed to initialize ExternalNewsletterProviderManager", ex);
                }

                providerCollection.SetReadOnly();
            }
        }

        public static ExternalNewsletterProviderCollection Providers
        {
            get
            {
                if (providerCollection == null)
                {
                    Initialize();
                }
                return providerCollection;
            }
        }

        public static List<ExternalNewsletterInfo> GetAvailableNewsletters(Guid siteGuid)
        {
            List<ExternalNewsletterInfo> newsletters = new List<ExternalNewsletterInfo>();

            try
            {
                foreach (ExternalNewsletterProvider provider in Providers)
                {
                    if (provider == null) continue;

                    try
                    {
                        var providerNewsletters = provider.GetAvailableNewsletters(siteGuid);
                        if (providerNewsletters != null)
                        {
                            newsletters.AddRange(providerNewsletters);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error retrieving available newsletters from provider '{provider.Name}'", ex);
                    }
                }

                newsletters.Sort((a, b) => a.SortRank.CompareTo(b.SortRank));
            }
            catch (Exception ex)
            {
                log.Error("Error querying external newsletter providers", ex);
            }

            return newsletters;
        }

        public static int GetNewsletterCount(Guid siteGuid)
        {
            return GetAvailableNewsletters(siteGuid).Count;
        }

        public static bool IsExternal(Guid letterInfoGuid)
        {
            if (letterInfoGuid == Guid.Empty) return false;

            return GetProviderForNewsletter(letterInfoGuid, out _) != null;
        }

        public static bool IsExternal(LetterInfo letter)
        {
            if (letter == null) return false;
            if (letter is ExternalNewsletterInfo) return true;

            return IsExternal(letter.LetterInfoGuid);
        }

        public static ExternalNewsletterProvider GetProviderForNewsletter(Guid letterInfoGuid, out ExternalNewsletterInfo foundNewsletter)
        {
            foundNewsletter = null;
            if (letterInfoGuid == Guid.Empty) return null;

            try
            {
                foreach (ExternalNewsletterProvider provider in Providers)
                {
                    if (provider == null) continue;

                    try
                    {
                        var list = provider.GetAvailableNewsletters(Guid.Empty);
                        if (list != null)
                        {
                            foreach (var item in list)
                            {
                                if (item.LetterInfoGuid == letterInfoGuid)
                                {
                                    foundNewsletter = item;
                                    return provider;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error querying provider '{provider.Name}' for newsletter GUID {letterInfoGuid}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in GetProviderForNewsletter", ex);
            }

            return null;
        }

        public static bool Subscribe(LetterInfo letter, string email, string firstName = "", string lastName = "")
        {
            if (letter == null || string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                ExternalNewsletterInfo extNewsletter = letter as ExternalNewsletterInfo;
                ExternalNewsletterProvider provider = null;

                if (extNewsletter != null && !string.IsNullOrEmpty(extNewsletter.ProviderName))
                {
                    provider = Providers[extNewsletter.ProviderName];
                }

                if (provider == null)
                {
                    provider = GetProviderForNewsletter(letter.LetterInfoGuid, out extNewsletter);
                }

                if (provider == null || extNewsletter == null)
                {
                    log.Warn($"Cannot subscribe email '{email}' to external newsletter '{letter.Title}' ({letter.LetterInfoGuid}): Provider is missing or removed.");
                    return false;
                }

                return provider.Subscribe(extNewsletter, email, firstName, lastName);
            }
            catch (Exception ex)
            {
                log.Error($"Failed to dispatch Subscribe to external newsletter '{letter.Title}' for email '{email}'", ex);
                return false;
            }
        }

        public static bool Unsubscribe(LetterInfo letter, string email)
        {
            if (letter == null || string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                ExternalNewsletterInfo extNewsletter = letter as ExternalNewsletterInfo;
                ExternalNewsletterProvider provider = null;

                if (extNewsletter != null && !string.IsNullOrEmpty(extNewsletter.ProviderName))
                {
                    provider = Providers[extNewsletter.ProviderName];
                }

                if (provider == null)
                {
                    provider = GetProviderForNewsletter(letter.LetterInfoGuid, out extNewsletter);
                }

                if (provider == null || extNewsletter == null)
                {
                    log.Warn($"Cannot unsubscribe email '{email}' from external newsletter '{letter.Title}' ({letter.LetterInfoGuid}): Provider is missing or removed.");
                    return false;
                }

                return provider.Unsubscribe(extNewsletter, email);
            }
            catch (Exception ex)
            {
                log.Error($"Failed to dispatch Unsubscribe to external newsletter '{letter.Title}' for email '{email}'", ex);
                return false;
            }
        }

        public static bool Unsubscribe(Guid letterInfoGuid, string email)
        {
            if (letterInfoGuid == Guid.Empty || string.IsNullOrWhiteSpace(email)) return false;

            var provider = GetProviderForNewsletter(letterInfoGuid, out var extNewsletter);
            if (provider == null || extNewsletter == null)
            {
                log.Warn($"Cannot unsubscribe email '{email}' from external newsletter GUID {letterInfoGuid}: Provider is missing or removed.");
                return false;
            }

            return provider.Unsubscribe(extNewsletter, email);
        }
    }
}
