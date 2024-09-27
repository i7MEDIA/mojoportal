#nullable enable
using log4net;
using mojoPortal.Web.Components;
using mojoPortal.Web.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace mojoPortal.Web.Controllers;

public class ExternalLoginController : Controller
{
	private static readonly ILog _Log = LogManager.GetLogger(typeof(ExternalLoginController));

	public async Task<ActionResult> Callback(
		[FromUri]
		string iss,
		string code,
		string? error = null,
		string? error_description = null,
		string? returnurl = null
	)
	{
		if (error is not null)
		{
			// handle error
		}

		var (success, errorTitle, errorMessage) = await Global.OidcService.GetTokensByCodeAsync(iss, code);

		if (success)
		{
			var link = PageUrlService.GetLoginRedirectLink();

			if (returnurl is not null)
			{
				link = returnurl;
			}

			return Redirect(link);
		}

		var incidentNumber = Guid.NewGuid();

		_Log.Error($"(Incident {incidentNumber}) There was an issue with the identity provider: {errorMessage}");

		return RedirectToAction(
			"Error",
			"ExternalLogin",
			new
			{
				IncidentNumber = incidentNumber,
				Title = errorTitle,
				Message = errorMessage,
			}
		);
	}


	public async Task<ActionResult> Logout()
	{
		var logoutLink = await Global.OidcService.Logout();

		return Redirect(logoutLink);
	}


	public ActionResult Error(Guid incidentNumber, string title, string message)
	{
		return View(new ExternalLoginErrorModel
		{
			IncidentNumber = incidentNumber,
			Title = title,
			Message = message,
		});
	}
}
