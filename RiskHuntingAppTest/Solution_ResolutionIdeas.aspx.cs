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
	
	public partial class Solution_ResolutionIdeas : System.Web.UI.Page
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

		const string SpanStartTagMenu = "<span class=menu>";
		const string SpanStartTagName = "<span class=name>";
		const string SpanStartTagArrow = "<span class=arrow>";
		const string SpanEndTag = "</span>";
		const string LiStartTagMenu = "<li class=menu>";
		const string LiEndTag = "</li>";
		const string aStartTag = "<a href=\"javascript:doLoad('EditResolutionIdea.aspx?content=";
		const string aMidTag = "');\">";
		const string aEndTag = "</a>";


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

				RetrieveNLData ();
				GenerateIdeaList ();

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

		#region Service Call
		private void RetrieveNLData()
		{
			var watch = Stopwatch.StartNew();

			//			string riskDescription = String.Empty;
			//			if (Session ["CURRENT_PROBLEM_DESC"] != null)
			//				riskDescription = Session ["CURRENT_PROBLEM_DESC"].ToString ();

			try {
				RiskHuntingAppTest.antique.AntiqueService antique = new RiskHuntingAppTest.antique.AntiqueService ();
				antique.NLParserCompleted += new RiskHuntingAppTest.antique.NLParserCompletedEventHandler (objAntique_NLParserCompleted);
				antique.NLParserAsync (this.currentRisk.Content);
			}
			finally {

				watch.Stop ();
				// Get the elapsed time as a TimeSpan value.
				TimeSpan ts = watch.Elapsed;
				// Format and display the TimeSpan value.
				string elapsedTime = String.Format ("{0:00}:{1:00}:{2:00}.{3:00}",
					ts.Hours, ts.Minutes, ts.Seconds,
					ts.Milliseconds / 10);
				Console.WriteLine (elapsedTime, "RunTime");
			}

		}

		void objAntique_NLParserCompleted(object sender, 
			RiskHuntingAppTest.antique.NLParserCompletedEventArgs e)
		{
			Console.WriteLine (e.Result);
			var NLResponse = Util.DeserializeNLResponse (e.Result);

			Session ["CURRENT_PROBLEM_DESC"] = NLResponse;
		}

		#endregion

		void GenerateIdeaList ()
		{

			if (this.currentRisk.Recommendations.Count > 0) {
				for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
					divIdeas.InnerHtml += GenerateHtml (this.currentRisk.Recommendations[i].ToString());
				}
			}
			else
				statusLabel.Text = "No resolution ideas available.";

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

		public virtual void addNewIdeaClicked(object sender, EventArgs args)
		{
			Response.Redirect ("AddResolutionIdea.aspx");
		}

		#region Html related

		private string GenerateHtml(string idea)
		{
			return LiStartTagMenu +
				aStartTag + idea + aMidTag +
				SpanStartTagName + idea + SpanEndTag +
				SpanStartTagArrow + SpanEndTag + aEndTag + LiEndTag;
		}

		#endregion



	}
}

