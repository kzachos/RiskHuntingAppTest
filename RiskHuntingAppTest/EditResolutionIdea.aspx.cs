using System;
using System.IO;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;


namespace RiskHuntingAppTest
{
	
	public partial class EditResolutionIdea : System.Web.UI.Page
	{
		const string Tag1a = "<div id=\"topbar2\">";
		const string Tag2a = "<div id=\"leftbutton\">";
		const string Tag3a = "<a href=\"javascript:doLoad('";
		const string Tag3b = ".aspx');\" >";
		const string Tag4a = "cancel";
		const string Tag5a = "</a>";
		const string Tag6a = "</div>";
		const string Tag7a = "<div id=\"multiselectionbuttons\">";
		const string Tag8a = "Edit Idea";
		const string Tag9a = "</div>";
		const string Tag10a = "</div>";

		protected string sourcesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources");
		protected const string SOURCESPECIFICATION = "SourceSpecification";
		protected const string PROBLEM = "Problem";
		protected const string SOLUTION = "Solution";
		protected const string ADDITIONAL = "Additional";
		protected const string PROCESSFOLDER = "_toProcess";

		protected string processPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources", "_toProcess");

		protected const string SOURCE_TYPE = Constants.CASEREF;
		protected const string CASE_TYPE = "Risk";

		protected string requestContent, sourceId;
		protected int index;

		protected Risk currentRisk;
		protected Action selectedAction;

		protected const string EDITIDEA_WATERMARK = "[Enter the idea description]";

		protected void Page_Init(object sender, EventArgs e)
		{
			if (Session ["CURRENT_RISK"] != null)
				this.sourceId = Session ["CURRENT_RISK"].ToString();	
			RetrieveCurrentRisk ();

			TopbarProblemIdeas.InnerHtml = GenerateHtml (DetermineFrom ());

			//			if (!Page.IsPostBack) {
			this.requestContent = DetermineContent ();
			if (!this.requestContent.Equals (String.Empty)) {
				if (this.currentRisk.Recommendations.Count > 0) {
					this.index = FindIdea ();
					if (this.index > -1) {
						PopulateIdea ();
					}
				}
				//				} else {
				//				}
			} 
		}

		private string DetermineContent()
		{
			string id = String.Empty;
			if (Request.QueryString["content"] != null)
			{
				id = Request.QueryString["content"];
			}
			return id;
		}

		private string DetermineFrom()
		{
			string f = String.Empty;
			if (Request.QueryString["from"] != null)
			{
				f = Request.QueryString["from"];
			}
			return f;
		}

		void RetrieveCurrentRisk ()
		{
			string location;

			location = Path.Combine (processPath, "SourceSpecification", Constants.CASEREF + this.sourceId + "_" + "SourceSpecification" + ".xml");
			XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(location);

			location = Path.Combine (processPath, "Problem", Constants.CASEREF + this.sourceId + "_" + "Problem" + ".xml");
			XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Load(location);

			location = Path.Combine (processPath, "Solution", Constants.CASEREF + this.sourceId + "_" + "Solution" + ".xml");
			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Load(location);

			this.currentRisk = new Risk (ss, problem, solution);

		}

		int FindIdea ()
		{
			int index = -1;
			foreach (var idea in this.currentRisk.Recommendations) {
				if (this.requestContent.Equals (idea.ToString())) {
					index = this.currentRisk.Recommendations.IndexOf (this.requestContent);
				}
			}
			return index;
		}

		void PopulateIdea ()
		{
			EditIdeaDescription.Text = this.requestContent;
		}


		public virtual void updateClicked(object sender, EventArgs args)
		{
			if (Page.IsValid)
			{
				if (!EditIdeaDescription.Text.Equals(EDITIDEA_WATERMARK))
				{

					this.currentRisk.Recommendations [this.index] = EditIdeaDescription.Text;

					GenerateXml("SourceSpecification");
					GenerateXml("Problem");
					GenerateXml("Solution");

					Response.Redirect(DetermineFrom () + ".aspx");
				}
			}
		}

		public virtual void deleteClicked(object sender, EventArgs args)
		{
			if (Page.IsValid)
			{
				string confirmValue = Request.Form["confirm_value"];
				if (confirmValue == "Yes") {
					int index = this.currentRisk.Recommendations.IndexOf (this.requestContent);
					if (index != -1)
						this.currentRisk.Recommendations.RemoveAt (index);

					if (this.currentRisk.Recommendations.Count == 0)
						this.currentRisk.State = RiskQueryState.ProblemDescribed;

					GenerateXml ("SourceSpecification");
					GenerateXml ("Problem");
					GenerateXml ("Solution");

					Response.Redirect (DetermineFrom () + ".aspx");
				}
			}
		}

		private string GenerateHtml(string from)
		{
			return Tag1a + Tag2a + Tag3a + from + Tag3b + Tag4a + Tag5a + 
				Tag6a + Tag7a + Tag8a + Tag9a + Tag10a;
		}

		#region Xml generation
		private void GenerateXml(string componentType)
		{
			string Ref;
			string xmlUri, xmlUri2;
			if (componentType.Equals("SourceSpecification"))
			{
				XmlProc.SourceSpecificationSerialized.SourceSpecification ss = Util.CreateSourceSpecificationXml(this.currentRisk);
				//				Console.WriteLine ("this.sourceId (GenerateXml): " + this.sourceId.ToString ());
				Ref = SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
				xmlUri = Path.Combine (sourcesPath, CASE_TYPE, SOURCESPECIFICATION, Ref);
				xmlUri2 = Path.Combine (sourcesPath, PROCESSFOLDER, SOURCESPECIFICATION, Ref);
				XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Save(ss, xmlUri);
				XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Save(ss, xmlUri2);
			}
			else if (componentType.Equals("Problem"))
			{
				XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = Util.CreateProblemXml(this.currentRisk);
				Ref = SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
				xmlUri = Path.Combine (sourcesPath, CASE_TYPE, PROBLEM, Ref);
				xmlUri2 = Path.Combine (sourcesPath, PROCESSFOLDER, PROBLEM, Ref);
				XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Save(problem, xmlUri);
				XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Save(problem, xmlUri2);
			}
			else if (componentType.Equals("Solution"))
			{
				XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = Util.CreateSolutionXml(this.currentRisk);
				Ref = SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
				xmlUri = Path.Combine (sourcesPath, CASE_TYPE, SOLUTION, Ref);
				xmlUri2 = Path.Combine (sourcesPath, PROCESSFOLDER, SOLUTION, Ref);
				XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri);
				XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri2);
			}

		}

		#endregion
	}
}
