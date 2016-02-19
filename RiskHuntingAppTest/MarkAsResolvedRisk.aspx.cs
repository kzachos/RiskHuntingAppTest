using System;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace RiskHuntingAppTest
{

	public partial class MarkAsResolvedRisk : System.Web.UI.Page
	{
		const string Tag1 = "<li class=\"checkbox\">";
		const string Tag2 = "<span class=\"name\">";
		const string Tag3 = "</span>";
		const string Tag4 = "<asp:CheckBox id=\"CheckBox";
		const string Tag5 = "\" runat=\"server\"></asp:CheckBox>";
		const string Tag6 = "</li>";
		const string Tag7 = "<li class=\"labelcontent2\">";
		const string Tag8 = "<asp:Label runat=\"server\" TextMode=\"MultiLine\" Width=\"100%\" >";
		const string Tag9 = "</asp:Label>";
		const string Tag10 = "</li>";


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
		protected const string SOURCESPECIFICATION = "SourceSpecification";
		protected const string PROBLEM = "Problem";
		protected const string SOLUTION = "Solution";
		protected const string ADDITIONAL = "Additional";
		protected const string PROCESSFOLDER = "_toProcess";
		protected const string GUIDANCE = "Select the resolution ideas that contributed towards resolving the risk case.";

		protected const string SOURCE_TYPE = Constants.CASEREF;
		protected const string CASE_TYPE = "Risk";

		protected string sourceId;
		protected Risk currentRisk;

		protected void Page_Init(object sender, EventArgs e)
		{			
			alert_message_success.Visible = false;
			alert_message_error.Visible = false;

			if (Sessions.RiskState != String.Empty)
				sourceId = Sessions.RiskState;

			if (!Page.IsPostBack) {
				Console.WriteLine ("Page_Init - NOT Page.IsPostBack");
				Util.AccessLog(Util.ScreenType.MarkRiskAsResolved);

				if (!this.sourceId.Equals (String.Empty)) {
					InitCreativeGuidance ();
					RetrieveCurrentRisk ();
					GenerateIdeaList ();
				} 

			}
			else {
				Console.WriteLine ("Page_Init - Page.IsPostBack");
			}

		}
			
		private void InitCreativeGuidance()
		{
			creativeGuidance.InnerText = GUIDANCE;
		}

		void GenerateIdeaList ()
		{

			if (this.currentRisk.Recommendations.Count > 0) {
				string[] ideasRes = new string[0]; 
				if (!this.currentRisk.IncidentStatus.Equals (String.Empty)) {
					ideasRes = this.currentRisk.IncidentStatus.Split(';');

				}
				for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
					int pos = Array.IndexOf(ideasRes, this.currentRisk.Recommendations [i].ToString ());
//					divIdeas.InnerHtml += GenerateHtml (this.currentRisk.Recommendations [i].ToString ());
					GenerateHtml3 (this.currentRisk.Recommendations [i].ToString (), String.Empty, pos);

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


		public virtual void resolvedClicked(object sender, EventArgs args)
		{
			if (Page.IsValid) {
				if (!this.sourceId.Equals (String.Empty)) {

					RetrieveCurrentRisk ();

					Util.AccessLog (Util.ScreenType.MarkRiskAsResolved, Util.FeatureType.MarkAsResolved_SubmitButton);

					this.currentRisk.State = RiskQueryState.RiskResolved;

					var selectedIdeasString = String.Empty;
					var selectedIdeasArray = Request.Form.GetValues("riskIdeas");
					if (selectedIdeasArray != null) {
						for (int i = 0; i < selectedIdeasArray.Length; i++) {
							selectedIdeasString += selectedIdeasArray [i] + ";";
						}
					}
					if (selectedIdeasString.Length > 0)
						selectedIdeasString = selectedIdeasString.Remove (selectedIdeasString.Length - 1);
					this.currentRisk.IncidentStatus = selectedIdeasString;

					GenerateXml (Constants.SOURCESPECIFICATION);
					GenerateXml (Constants.PROBLEM);
					GenerateXml (Constants.SOLUTION);

					Response.Redirect ("Summary.aspx?pb=resolved");
				}
			}
		}

		public virtual void unresolvedClicked(object sender, EventArgs args)
		{
			if (Page.IsValid) {
				if (!this.sourceId.Equals (String.Empty)) {

					RetrieveCurrentRisk ();
					this.currentRisk.IncidentStatus = String.Empty;

					GenerateXml (Constants.SOURCESPECIFICATION);
					GenerateXml (Constants.PROBLEM);
					GenerateXml (Constants.SOLUTION);

					Response.Redirect ("Summary.aspx?pb=unresolved");
				}
			}
		}


		#region Html related


		private string GenerateHtml(string idea)
		{
			return LiStartTagMenu +
				aStartTag + idea + aMidTag +
				SpanStartTagName + idea + SpanEndTag +
				SpanStartTagArrow + SpanEndTag + aEndTag + LiEndTag;
		}

		private void GenerateHtml3 (string main, string second, int id)
		{
			divIdeas.Controls.Add (new LiteralControl (Tag1)); 
			divIdeas.Controls.Add (new LiteralControl (Tag2));
			divIdeas.Controls.Add (new LiteralControl (main));
			divIdeas.Controls.Add (new LiteralControl (Tag3));

			if (id > -1) 
				divIdeas.Controls.Add (new LiteralControl ("<input type='checkbox' value='" + main + "' name='riskIdeas' checked='checked' />"));
			else
				divIdeas.Controls.Add (new LiteralControl ("<input type='checkbox' value='" + main + "' name='riskIdeas' />"));
//			divIdeas.Controls.Add (CreateCheckboxControl (id.ToString (), main));

			divIdeas.Controls.Add (new LiteralControl (Tag6));
			divIdeas.Controls.Add (new LiteralControl (Tag7));
			divIdeas.Controls.Add (new LiteralControl (Tag8));
			divIdeas.Controls.Add (new LiteralControl (second));
			divIdeas.Controls.Add (new LiteralControl (Tag9));
			divIdeas.Controls.Add (new LiteralControl (Tag10));
		}

		private CheckBox CreateCheckboxControl(string id, string content)
		{
			var k = new CheckBox();
			k.ID = id;
			k.AutoPostBack = false;
			k.ToolTip = content;
//			k.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);

			return k;
		}

//		protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
//		{
//			//write the client id of the control that triggered the event
//			Console.WriteLine(((CheckBox)sender).ToolTip);
//		}

		#endregion

	}
}

