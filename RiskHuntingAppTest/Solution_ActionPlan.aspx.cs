using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace RiskHuntingAppTest
{
	
	public partial class Solution_ActionPlan : System.Web.UI.Page
	{
		const string Tag1a = "<li class=\"store\">";
		const string Tag2a = "<a class=\"noeffect\" href=\"javascript:doLoad('EditAction.aspx?id=";
		const string Tag3a = "');\">";
		const string Tag4a = "<span class=\"name\">";
		const string Tag5a = "</span>";
		const string Tag6a = "<span class=\"comment\">";
		const string Tag7a = "</span>";
		const string Tag8a = "<span class=\"starcomment\">";
		const string Tag9a = "</span>";
		const string Tag10a = "<span class=\"arrow\"></span>";
		const string Tag11a = "</a>";
		const string Tag12a = "</li>";

		protected string sourcesPath = SettingsTool.GetApplicationPath() + "xmlFiles/Sources/";
		protected const string SOURCESPECIFICATION = "SourceSpecification";
		protected const string PROBLEM = "Problem";
		protected const string SOLUTION = "Solution";
		protected const string ADDITIONAL = "Additional";
		protected const string PROCESSFOLDER = "_toProcess";

		protected string processPath = SettingsTool.GetApplicationPath() + "xmlFiles/Sources/_toProcess/";

		protected const string SOURCE_TYPE = Constants.CASEREF;
		protected const string CASE_TYPE = "Risk";

		protected List<ListItem> items;
		protected int maxId;
		protected string sourceId;
		protected Risk currentRisk;


		protected void Page_Load(object sender, EventArgs e)
		{			
			if (Session ["CURRENT_RISK"] != null)
			this.sourceId = Session ["CURRENT_RISK"].ToString();	
			RetrieveCurrentRisk ();

			if (!Page.IsPostBack) {
				Console.WriteLine ("Page_Init - NOT Page.IsPostBack");
				GenerateActionList ();

			}
			else {
				Console.WriteLine ("Page_Init - Page.IsPostBack");
			}
			Topbar_Problem_Search_Solution ();
		}

	

		#region Initializing
		private void Topbar_Problem_Solution ()
		{
			TopbarProblemSolution.Visible = true;
			TopbarProblemSearchSolution.Visible = false;
		}
		private void Topbar_Problem_Search_Solution ()
		{
			TopbarProblemSolution.Visible = false;
			TopbarProblemSearchSolution.Visible = true;
		}
		#endregion

		void GenerateActionList ()
		{

			if (this.currentRisk.Actions.Count > 0) {
				for (int i = 0; i < this.currentRisk.Actions.Count; i++) {
					divActions.InnerHtml += GenerateHtml (this.currentRisk.Actions[i]);
				}
			}
			else
				statusLabel.Text = "No actions available.";

		}

		void RetrieveCurrentRisk ()
		{
			string location = String.Empty;

			location = processPath + "SourceSpecification" + "/" + Constants.CASEREF + this.sourceId + "_" + "SourceSpecification" + ".xml";
			XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(location);

			location = processPath + "Problem" + "/" + Constants.CASEREF + this.sourceId + "_" + "Problem" + ".xml";
			XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Load(location);

			location = processPath + "Solution" + "/" + Constants.CASEREF + this.sourceId + "_" + "Solution" + ".xml";
			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Load(location);

			this.currentRisk = new Risk (ss, problem, solution);

		}
			
		public virtual void addNewActionClicked(object sender, EventArgs args)
		{
			Response.Redirect ("AddAction.aspx");
		}

		#region Html related

		private string GenerateHtml(Action action)
		{
			return Tag1a + Tag2a + action.Id + Tag3a + Tag4a + action.Content + Tag5a + 
				Tag6a + "Owner: " + action.Owner + Tag7a + 
				Tag8a + "Impelement by: " + action.ImplementBy.ToShortDateString() + Tag9a + Tag10a + Tag11a + Tag12a;
		}

		#endregion



	}
}

