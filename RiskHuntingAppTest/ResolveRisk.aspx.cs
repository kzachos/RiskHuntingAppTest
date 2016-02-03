using System;
using System.Web;
using System.Web.UI;
using System.IO;

namespace RiskHuntingAppTest
{

	public partial class ResolveRisk : System.Web.UI.Page
	{
		const string SpanStartTagMenu = "<span class=menu>";
		const string SpanStartTagName = "<span class=name>";
		const string SpanStartTagArrow = "<span class=arrow>";
		const string SpanEndTag = "</span>";
		const string LiStartTagMenu = "<li class=menu>";
		const string LiEndTag = "</li>";
		const string aStartTag = "<a href=\"javascript:doLoad('EditResolutionIdea.aspx?from=ResolveRisk&content=";
		const string aMidTag = "');\">";
		const string aEndTag = "</a>";

		protected string requestPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Requests");
		protected string sourcesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources");
		protected string processPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources", "_toProcess");

		protected string sourceId;
		protected Risk currentRisk;

		protected void Page_Load(object sender, EventArgs e)
		{			
			if (Sessions.PersonaState != String.Empty) 
				Session.Remove (Sessions.personaState);
			if (Sessions.PersonasState != null) 
				Session.Remove (Sessions.personasState);

			if (Sessions.RiskState != String.Empty)
				sourceId = Sessions.RiskState;

			if (!Page.IsPostBack) {
				Console.WriteLine ("Page_Init - NOT Page.IsPostBack");
				Util.AccessLog(Util.ScreenType.ResolveRisk);

				if (!this.sourceId.Equals (String.Empty)) {
					RetrieveCurrentRisk ();
					GenerateIdeaList ();
				} else {
					AddNewIdeaDiv.Visible = false;
					statusLabel.Text = "First, enter a problem description and either press 'SAVE CHANGES' or 'FIND SIMILAR RISKS'";
				}

			}
			else {
				Console.WriteLine ("Page_Init - Page.IsPostBack");
			}

		}

		void GenerateIdeaList ()
		{

			if (this.currentRisk.Recommendations.Count > 0) {
				this.currentRisk.State = RiskQueryState.IdeasGenerated;
				for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
					divIdeas.InnerHtml += GenerateHtml (this.currentRisk.Recommendations [i].ToString ());
				}
			} else {
				statusLabel.Text = "No ideas available.";
			}

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

		public virtual void addNewIdeaClicked(object sender, EventArgs args)
		{
			Response.Redirect ("AddResolutionIdea.aspx?from=ResolveRisk.aspx");
		}

		public virtual void goBackClicked(object sender, EventArgs args)
		{
			Response.Redirect ("DescribeRisk.aspx");
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

