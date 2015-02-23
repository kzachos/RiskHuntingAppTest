using System;
using System.Web;
using System.Web.UI;

namespace RiskHuntingAppTest
{
	
	public partial class DefaultRedirectErrorPage : System.Web.UI.Page
	{
		protected HttpException ex = null;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Log the exception and notify system operators
			ex = new HttpException("defaultRedirect");
			ExceptionUtility.LogException(ex, "Caught in DefaultRedirectErrorPage");
			ExceptionUtility.NotifySystemOps(ex);
		}
	}
}

