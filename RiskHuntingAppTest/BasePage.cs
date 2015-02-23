using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using ActivityTrackingLog.Utils;

namespace RiskHuntingApp
{
	public class BasePage : Page
	{
		protected void LogActivity(ServerSideActivitiesEnum activity)
		{
			if (!IsPostBack)
			{
				// log server side feature accessed activity
				var activityItem = Log.LogActivity(ActivityCategories.WebSitePages, ServerSideActivitiesEnum.HomePageAccessed);
				if (null != activityItem)
					(this.Master as SiteMaster).LoggedServerMessage = string.Format("This page triggerred an <span class=\"bold italic\">SERVER</span> side activity <span class=\"bold italic\">[{0}]</span> at {1}", activity.ToString(), activityItem.TimeUtc.ToString("MM/dd/yyyy HH:MM:ss.fff"));
			}

			/* ----- alternative method --------
                // log server side feature accessed activity
                Log.LogActivity(ActivityCategories.CustomerPages, ServerSideActivitiesEnum.HomePageAccessed, IsPostBack);
            }*/
		}
	}
}