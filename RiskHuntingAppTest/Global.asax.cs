
namespace RiskHuntingAppTest
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Web;
	using System.Web.SessionState;

	public class Global : System.Web.HttpApplication
	{
//		protected void Page_Init(object sender, EventArgs e)
//		{
//			if (Context.Session != null)
//			{
//				if (Session.IsNewSession)
//				{
//					HttpCookie newSessionIdCookie = Request.Cookies["ASP.NET_SessionId"];
//					if (newSessionIdCookie != null)
//					{
//						string newSessionIdCookieValue = newSessionIdCookie.Value;
//						if (newSessionIdCookieValue != string.Empty)
//						{
//							// This means Session was timed Out and New Session was started
//							Response.Redirect("DescribeRisk.aspx");
//						}
//					}
//				}
//			}
//		}

		protected void Application_Start (Object sender, EventArgs e)
		{
		}

		protected void Session_Start (Object sender, EventArgs e)
		{
			if (Context.Session != null)
			{
				if (Session.IsNewSession)
				{
					HttpCookie newSessionIdCookie = Request.Cookies["ASP.NET_SessionId"];
					if (newSessionIdCookie != null)
					{
						string newSessionIdCookieValue = newSessionIdCookie.Value;
						if (newSessionIdCookieValue != string.Empty)
						{
							// This means Session was timed Out and New Session was started
							Response.Redirect("DescribeRisk.aspx");
						}
					}
				}
			}
			HttpContext.Current.Session.Add(Constants.SESSION_ACTIVITY_LOG_ID, Guid.NewGuid().ToString());
		}

		protected void Application_BeginRequest (Object sender, EventArgs e)
		{
		}

		protected void Application_EndRequest (Object sender, EventArgs e)
		{
		}

		protected void Application_AuthenticateRequest (Object sender, EventArgs e)
		{
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			// Code that runs when an unhandled error occurs

			// Get the exception object.
			Exception exc = Server.GetLastError();

			// Handle HTTP errors
			if (exc.GetType() == typeof(HttpException))
			{
				// The Complete Error Handling Example generates
				// some errors using URLs with "NoCatch" in them;
				// ignore these here to simulate what would happen
				// if a global.asax handler were not implemented.
				if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
					return;

				//Redirect HTTP errors to HttpError page
				Server.Transfer("HttpErrorPage.aspx");
			}

			// For other kinds of errors give the user some information
			// but stay on the default page
			Response.Write("<h1 color=\"Red\">Oops, something went wrong</h1>\n");
			Response.Write(
				"<p>We are currently having problems on the site.  An email has been sent to the site owner to report the problem.</p>\n");
//			Response.Write(
//				"<p>" + exc.Message + "</p>\n");
			Response.Write("Return to the <a href='DescribeRisk.aspx'>" +
				"Home Page</a>\n");

			// Log the exception and notify system operators
			ExceptionUtility.LogException(exc, "DefaultPage");
			ExceptionUtility.NotifySystemOps(exc);

			// Clear the error from the server
			Server.ClearError();
		}


		protected void Session_End (Object sender, EventArgs e)
		{
			Response.Redirect("DescribeRisk.aspx");
		}

		protected void Application_End (Object sender, EventArgs e)
		{
		}
	}
}

