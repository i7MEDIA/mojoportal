// http://www.aspnetresources.com/blog/fighting_view_state_spam.aspx
// Copyright (c) 2004-2006, Milan Negovan 
// http://www.AspNetResources.com
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
//
//    * Redistributions of source code must retain the above copyright 
//      notice, this list of conditions and the following disclaimer.
//      
//    * Redistributions in binary form must reproduce the above copyright 
//      notice, this list of conditions and the following disclaimer in 
//      the documentation and/or other materials provided with the 
//      distribution.
//      
//    * The name of the author may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED 
// TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, 
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR 
// OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF 
// ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
// 2007-09-23  added logging with log4net
// 2007-09-24  changed so that banned ips are stored in the db
// instead of text file
// 2008-01-05 added try catch to prevent error before upgrade creates the table
// 2008-08-15 added ViewStateIsHacked funtion and additional true criteria

using log4net;
using mojoPortal.Business;
using mojoPortal.Core.EF;
using mojoPortal.Data.EF;
using mojoPortal.Web.Framework;
using System;
using System.Data.Common;
using System.Web;

namespace mojoPortal.Web
{
	public class BannedIPBlockingHttpModule : IHttpModule
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(BannedIPBlockingHttpModule));
		private readonly IUnitOfWork unitOfWork;


		public BannedIPBlockingHttpModule()
		{
			// TODO: Replace with Dependancy Injection
			unitOfWork = new UnitOfWork(new mojoPortalDbContext());
		}


		public void Init(HttpApplication application)
		{
			application.BeginRequest += new EventHandler(BeginRequest);
			application.EndRequest += new EventHandler(this.EndRequest);
		}


		private void BeginRequest(object sender, EventArgs e)
		{
			if (WebConfigSettings.DisableBannedIpBlockingModule)
			{
				return;
			}

			HttpApplication app = (HttpApplication)sender;

			if (WebUtils.IsRequestForStaticFile(app.Request.Path)) 
			{
				return;
			}

			HttpContext context = app.Context;

			string ip = SiteUtils.GetIP4Address();

			try
			{
				if (!IsBanned(ip))
				{
					return;
				}

				AbortRequestFromBannedIP(context);
			}
			catch (DbException ex)
			{
				log.Error("handled exception: ", ex);
			}
			catch (InvalidOperationException ex)
			{
				log.Error("handled exception: ", ex);
			}
			catch (Exception ex)
			{
				// hate to trap System.Exception but SqlCeException doe snot inherit from DbException as it should
				if (DatabaseHelper.DBPlatform() != "SqlCe")
				{
					throw;
				}

				log.Error(ex);
			}
		}


		private void EndRequest(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			HttpContext context = app.Context;
			HttpResponse response = context.Response;

			if (!context.Items.Contains("BanCurrentRequest"))
			{
				return;
			}

			response.ClearContent();
			response.SuppressContent = true;
			response.StatusCode = 403;
			response.StatusDescription = "Access denied: your IP has been banned due to spamming or hacking attempts.";
		}


		private static void AbortRequestFromBannedIP(HttpContext context)
		{
			context.Items["BanCurrentRequest"] = true;
			context.ApplicationInstance.CompleteRequest();

			if (WebConfigSettings.LogBlockedRequests)
			{
				log.Info("BannedIPBlockingHttpModule blocked request from banned ip address " + SiteUtils.GetIP4Address());
			}
		}


		private bool IsBanned(string ipAddress)
		{
			// 2008-08-13 this list got too large over time
			// better to make a small hit to the db on each request than to cache this huge List

			//List<String> bannedIPs = CacheHelper.GetBannedIPList();
			//if(bannedIPs.Contains(ip))return true;
			//return false;

			// Replace old business logic with EF Repository
			//return BannedIPAddress.IsBanned(ip);

			var isBanned = unitOfWork.BannedIPAddresses.IsBanned(ipAddress);

			unitOfWork.Complete();

			return isBanned;
		}


		public void Dispose() { }
	}
}
