
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using log4net;
namespace mojoPortal.Web.ModelBinders
{
	/// <summary>
	/// NONE OF THIS IS USED BUT WE'RE KEEPING IT FOR FUTURE REFERENCE, MAYBE, IF WE REMEMBER WE KEPT IT, WHICH WE PROBABLY WON'T, MOST LIKELY, PROBABLY
	/// </summary>

	//public class DateTimeSiteTimeZoneBinder : IModelBinder
	//{
	//	TimeZoneInfo timeZone = null;

	//	public object BindModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
	//	{
	//		timeZone = SiteUtils.GetUserTimeZone();

	//		var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
	//		bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);

	//		var dt = value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
	//		_ = DateTime.TryParse(dt.ToString(), out var dtUtc);

	//		return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dtUtc, DateTimeKind.Utc), timeZone);
	//	}
	//}

	//public class NullableDateTimeBinder : IModelBinder
	//{
	//	public object BindModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
	//	{
	//		var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
	//		bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);

	//		return value?.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
	//	}
	//}

	public class DateTimeBinder : DefaultModelBinder
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(DateTimeBinder));

		protected override void BindProperty(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
		{

			var kindAttr = propertyDescriptor.Attributes.OfType<DateTimeKindAttribute>().FirstOrDefault();
			var model = bindingContext.Model;
			PropertyInfo property = model.GetType().GetProperty(propertyDescriptor.Name);

			var value = bindingContext.ValueProvider.GetValue(propertyDescriptor.Name);
			
			log.Info($"DateTime set in DB is {value.AttemptedValue}");

			if (value != null && propertyDescriptor.PropertyType == typeof(DateTime) && kindAttr != null)
			{
				try
				{
					//System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("it-CH");

					var theKind = kindAttr.Kind;
					log.Info($"DateTimeKind set to {theKind}");

					DateTime theDate;
					switch (theKind)
					{
						case DateTimeKind.Utc:
						case DateTimeKind.Unspecified:
						default:
							theDate = DateTime.Parse(value.AttemptedValue);
							break;
						case DateTimeKind.Local:
							theDate = setToSiteTimeZone();
							break;
					}
					property.SetValue(model, theDate, null);
				}
				catch
				{
					//If something wrong, validation should take care
				}
			}
			else if (value != null && propertyDescriptor.PropertyType == typeof(DateTime))
			{
				log.Info($"DateTimeKind was null so defaulting to site time zone.");

				property.SetValue(model, setToSiteTimeZone(), null);
			}
			else
			{
				log.Info($"Could do a damn thing just binding with the default crap");

				base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
			}

			DateTime setToSiteTimeZone()
			{
				TimeZoneInfo timeZone = SiteUtils.GetUserTimeZone();

				var dt = value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
				_ = DateTime.TryParse(dt.ToString(), out var dtUtc);

				return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dtUtc, DateTimeKind.Utc), timeZone);
			}
		}

		//public object BindModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
		//{
		//	var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
		//	bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);

		//	return value;
		//}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class DateTimeKindAttribute : Attribute
	{
		public DateTimeKind Kind { get; private set; } = DateTimeKind.Unspecified;

		public DateTimeKindAttribute(DateTimeKind kind)
		{
			Kind = kind;
		}


		//public abstract bool BindProperty(ControllerContext controllerContext,
		//System.Web.Mvc.ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor);
	}
}