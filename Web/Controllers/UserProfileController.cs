using Microsoft.Ajax.Utilities;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace mojoPortal.Web.Controllers
{
	[Authorize]
	[RoutePrefix("api/UserProfile")]
	public class UserProfileController : ApiController
	{
		#region Fields

		private readonly mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
		private readonly SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
		private readonly double timeOffset = SiteUtils.GetUserTimeOffset();
		private readonly TimeZoneInfo timeZone = SiteUtils.GetUserTimeZone();
		private SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

		#endregion


		#region Endpoints

		[HttpGet]
		public IHttpActionResult Get()
		{
			return Ok(getProperties());
		}


		// TODO: Need to add guards to allow only those with permissions to edit another user
		//[HttpGet]
		//[Route("{userGuid:guid}")]
		//public IHttpActionResult GetByUserGuid(Guid userGuid)
		//{
		//	siteUser = SiteUser.GetByGuid(siteSettings, userGuid);

		//	return Ok(getProperties());
		//}


		[HttpGet]
		[Route("Property/{key}")]
		public IHttpActionResult Get(string key)
		{
			return Ok(getProperty(key));
		}


		[HttpPost]
		public IHttpActionResult Post(Dictionary<string, object> profile)
		{
			setProperties(profile);

			return Ok();
		}


		[HttpPost]
		[Route("Property/{key}")]
		public IHttpActionResult Post([FromUri] string key, [FromBody] object val)
		{
			setProperty(key, val);

			return Ok();
		}

		#endregion


		#region Private Methods

		private Dictionary<string, object> getProperties()
		{
			var properties = new Dictionary<string, object>();

			if (profileConfig == null)
			{
				return properties;
			}

			foreach (var propertyDefinition in profileConfig.PropertyDefinitions)
			{
				// we allow this to be configured as a profile property so it can be required for registration
				// but we don't need to load it here because we have a dedicated control for the property already
				if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeZoneIdKey)
				{
					continue;
				}

				properties.Add(propertyDefinition.Name, getProperty(propertyDefinition));
			}

			return properties;
		}


		private object getProperty(string key)
		{
			if (profileConfig == null)
			{
				return null;
			}

			var propertyDefinition = profileConfig.PropertyDefinitions.SingleOrDefault(x => x.Name.ToLower() == key.ToLower());

			if (propertyDefinition != null)
			{
				return getProperty(propertyDefinition);
			}

			return null;
		}


		private object getProperty(mojoProfilePropertyDefinition propertyDefinition)
		{
			object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);

			if (propValue != null)
			{
				object castProp;

				switch (propertyDefinition.Type)
				{
					case "System.Boolean":
						castProp = Convert.ToBoolean(propValue);

						break;

					case "System.DateTime":
						if (DateTime.TryParse(propValue.ToString(), out var val))
						{
							castProp = val;
						}
						else
						{
							castProp = null;
						}

						break;

					default:
					case "System.String":
						var strVal = string.IsNullOrWhiteSpace((string)propValue) ? null : (string)propValue;

						if (propertyDefinition.Name == "Gender")
						{
							strVal = "Male";

							if (strVal == "F")
							{
								strVal = "Female";
							}
						}

						castProp = string.IsNullOrWhiteSpace(strVal) ? null : strVal;
						break;
				}

				return castProp;
			}

			return null;
		}


		private void setProperties(Dictionary<string, object> properties)
		{
			if (profileConfig == null)
			{
				return;
			}

			foreach (var propertyDefinition in profileConfig.PropertyDefinitions)
			{
				// we allow this to be configured as a profile property so it can be required for registration
				// but we don't need to load it here because we have a dedicated control for the property already
				if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeZoneIdKey)
				{
					continue;
				}

				var comparer = new UserProfileKeyComparer();

				if (!properties.Keys.Contains(propertyDefinition.Name, comparer))
				{
					continue;
				}

				var property = properties
					.Where(x => x.Key.ToLower() == propertyDefinition.Name.ToLower())
					.Select(x => (KeyValuePair<string, object>?)x)
					.FirstOrDefault();

				if (property == null)
				{
					return;
				}

				// .Value.Value here is due to using a nullable KeyValuePair type
				setProperty(propertyDefinition, property.Value.Value);
			}

			siteUser.UpdateLastActivityTime();
		}


		private void setProperty(string key, object val)
		{
			if (profileConfig == null)
			{
				return;
			}

			var propertyDefinition = profileConfig.PropertyDefinitions.FirstOrDefault(x => x.Name.ToLower() == key.ToLower());

			if (propertyDefinition != null)
			{
				setProperty(propertyDefinition, val);
			}

			siteUser.UpdateLastActivityTime();
		}


		private void setProperty(mojoProfilePropertyDefinition propertyDefinition, object newValue)
		{
			switch (propertyDefinition.Type)
			{
				case "System.Boolean":
					siteUser.SetProperty(
						propertyDefinition.Name,
						newValue,
						propertyDefinition.SerializeAs,
						propertyDefinition.LazyLoad
					);

					break;

				case "System.DateTime":
					if (newValue.ToString().Length > 0)
					{
						if (DateTime.TryParse(
								newValue.ToString(),
								CultureInfo.CurrentCulture,
								DateTimeStyles.AdjustToUniversal, out DateTime dt
							)
						)
						{
							if (propertyDefinition.IncludeTimeForDate)
							{
								if (timeZone != null)
								{
									dt = dt.ToUtc(timeZone);
								}
								else
								{
									dt = dt.AddHours(-timeOffset);
								}

								if (propertyDefinition.Name == "DateOfBirth")
								{
									siteUser.DateOfBirth = dt.Date;

									siteUser.Save();
								}
								else
								{
									siteUser.SetProperty(
										propertyDefinition.Name,
										dt.ToString(),
										propertyDefinition.SerializeAs,
										propertyDefinition.LazyLoad
									);
								}
							}
							else
							{
								if (propertyDefinition.Name == "DateOfBirth")
								{
									siteUser.DateOfBirth = dt.Date;

									siteUser.Save();
								}
								else
								{
									siteUser.SetProperty(
									propertyDefinition.Name,
									dt.Date.ToShortDateString(),
									propertyDefinition.SerializeAs,
									propertyDefinition.LazyLoad);
								}
							}
						}
						else
						{
							siteUser.SetProperty(
								propertyDefinition.Name,
								newValue,
								propertyDefinition.SerializeAs,
								propertyDefinition.LazyLoad
							);
						}
					}
					else // blank
					{
						if (propertyDefinition.Name == "DateOfBirth")
						{
							siteUser.DateOfBirth = DateTime.MinValue;

							siteUser.Save();
						}
						else
						{
							siteUser.SetProperty(
								propertyDefinition.Name,
								string.Empty,
								propertyDefinition.SerializeAs,
								propertyDefinition.LazyLoad
							);
						}
					}

					break;

				case "System.String":
				default:
					if (propertyDefinition.Type == "CheckboxList")
					{
						siteUser.SetProperty(
							propertyDefinition.Name,
							string.Join(",", (Array)newValue),
							propertyDefinition.SerializeAs,
							propertyDefinition.LazyLoad
						);
					}
					else
					{
						if (propertyDefinition.Name.ToLower() == "gender")
						{
							if (newValue.ToString() == "Male")
							{
								newValue = "M";
							}
							else if (newValue.ToString() == "Female")
							{
								newValue = "F";
							}
						}

						siteUser.SetProperty(
							propertyDefinition.Name,
							newValue,
							propertyDefinition.SerializeAs,
							propertyDefinition.LazyLoad
						);
					}

					break;
			}
		}

		#endregion
	}


	class UserProfileKeyComparer : EqualityComparer<string>
	{
		public override bool Equals(string x, string y)
		{
			return x.ToLower() == y.ToLower();
		}

		public override int GetHashCode(string obj)
		{
			return obj.GetHashCode();
		}
	}
}
