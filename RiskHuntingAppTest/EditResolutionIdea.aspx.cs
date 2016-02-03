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
		protected string processPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources", "_toProcess");
	

		protected string requestContent, sourceId;
		protected int index;

		protected Risk currentRisk;
		protected Action selectedAction;

		protected const string EDITIDEA_WATERMARK = "[Enter the idea description]";

		protected void Page_Init(object sender, EventArgs e)
		{
			Util.AccessLog(Util.ScreenType.EditIdea);
			alert_message_error.Visible = false;
			if (Sessions.RiskState != String.Empty)
				this.sourceId = Sessions.RiskState;
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
			if (!this.sourceId.Equals(String.Empty))
			{
				string location = String.Empty;

				location = Path.Combine (processPath, "SourceSpecification", Constants.CASEREF + this.sourceId + "_" + "SourceSpecification" + ".xml");
				XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(location);

				location = Path.Combine (processPath, "Problem", Constants.CASEREF + this.sourceId + "_" + "Problem" + ".xml");
				XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Load(location);

				location = Path.Combine (processPath, "Solution", Constants.CASEREF + this.sourceId + "_" + "Solution" + ".xml");
				XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Load(location);

				this.currentRisk = new Risk (ss, problem, solution);
			} else {
				Response.Redirect ("DescribeRisk.aspx?pb=" + Constants.SESSION_EXPIRED_LABEL);
			}

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
				if (EditIdeaDescription.Text.Equals(String.Empty) ||
					EditIdeaDescription.Text.Equals(EDITIDEA_WATERMARK))
				{
					errorMessage.InnerHtml = "Please add your idea below.";
					alert_message_error.Visible = true;

				}
				else {
					Util.AccessLog(Util.ScreenType.EditIdea, Util.FeatureType.EditIdea_UpdateIdeaButton);

					this.currentRisk.Recommendations [this.index] = EditIdeaDescription.Text;

					GenerateXml(Constants.SOURCESPECIFICATION);
					GenerateXml(Constants.PROBLEM);
					GenerateXml(Constants.SOLUTION);

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
					Util.AccessLog(Util.ScreenType.EditIdea, Util.FeatureType.EditIdea_DeleteIdeaButton);

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
				Ref = Constants.SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
				xmlUri = Path.Combine (sourcesPath, Constants.CASE_TYPE, Constants.SOURCESPECIFICATION, Ref);
				xmlUri2 = Path.Combine (sourcesPath, Constants.PROCESSFOLDER, Constants.SOURCESPECIFICATION, Ref);
				XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Save(ss, xmlUri);
				XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Save(ss, xmlUri2);
			}
			else if (componentType.Equals("Problem"))
			{
				XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = Util.CreateProblemXml(this.currentRisk);
				Ref = Constants.SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
				xmlUri = Path.Combine (sourcesPath, Constants.CASE_TYPE, Constants.PROBLEM, Ref);
				xmlUri2 = Path.Combine (sourcesPath, Constants.PROCESSFOLDER, Constants.PROBLEM, Ref);
				XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Save(problem, xmlUri);
				XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Save(problem, xmlUri2);
			}
			else if (componentType.Equals("Solution"))
			{
				XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = Util.CreateSolutionXml(this.currentRisk);
				Ref = Constants.SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
				xmlUri = Path.Combine (sourcesPath, Constants.CASE_TYPE, Constants.SOLUTION, Ref);
				xmlUri2 = Path.Combine (sourcesPath, Constants.PROCESSFOLDER, Constants.SOLUTION, Ref);
				XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri);
				XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri2);
			}

		}

		#endregion
	}
}
