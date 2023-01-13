using System.Configuration.Provider;
using System.Web.Configuration;

namespace mojoPortal.Web.Controls.DatePicker
{
    public sealed class DatePickerManager
    {
        static DatePickerManager()
        {
            Initialize();
        }

        private static void Initialize()
        {
            DatePickerConfiguration config = DatePickerConfiguration.GetConfig();

            if (
                config.DefaultProvider == null ||
				config.Providers == null ||
                config.Providers.Count < 1
            )
            {
                throw new ProviderException("You must specify a valid default provider.");
            }

            Providers = new DatePickerProviderCollection();

            ProvidersHelper.InstantiateProviders(
                config.Providers, 
                Providers,
                typeof(DatePickerProvider));
                
            Providers.SetReadOnly();
            Provider = Providers[config.DefaultProvider];
        }

        public static DatePickerProvider Provider { get; private set; }

        public static DatePickerProviderCollection Providers { get; private set; }

    }
}