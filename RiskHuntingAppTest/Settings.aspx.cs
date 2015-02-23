using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace RiskHuntingAppTest
{
    public partial class Settings : System.Web.UI.Page
    {
		protected string xmlFilesPath = Path.Combine (SettingsTool.GetApplicationPath(), "SearchApp", "xmlFiles");
		protected string requestPath = Path.Combine (SettingsTool.GetApplicationPath(), "SearchApp", "xmlFiles", "Requests");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        protected void submitClicked(object sender, EventArgs e)
        {

			throw new DivideByZeroException();

        }
    }
}
