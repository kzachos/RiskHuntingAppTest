using System;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace RiskHuntingAppTest
{
	
	public partial class CreateIdeas_Superheroes : System.Web.UI.Page
	{
		const string Tag1a = "<iframe src=\"http://brightsparks.city.ac.uk/RiskHunting/indexApp.php?persona=Iceman border=\"0\" frameborder=\"0\" height=\"100%\" width=\"100%\"></iframe>";
		const string Tag1b = "<iframe src=\"";
		const string Tag2 = "http://brightsparks.city.ac.uk/RiskHunting/indexApp.php?persona=";
		const string Tag3 = "\" border=\"0\" frameborder=\"0\" height=\"100%\" width=\"100%\"></iframe>";

		protected string sourcesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources");
		protected string processPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources", "_toProcess");
		protected Risk currentRisk;
		protected string requestContent, sourceId;
		protected const string ADDIDEA_WATERMARK = "[Enter your new idea]";

		protected List<Persona> HofResponse;
		protected string currentPersona;

		protected void Page_Init(object sender, EventArgs e)
		{
			alert_message_success.Visible = false;
			alert_message_error.Visible = false;

			if (!Page.IsPostBack) {
				Util.AccessLog(Util.ScreenType.CreateIdea_Superheroes);
//			AddIdeaDescription.WatermarkText = ADDIDEA_WATERMARK;
				if (Sessions.RiskState != String.Empty)
					sourceId = Sessions.RiskState;
				if (Sessions.ProblemDescState != null)
					Session.Remove (Sessions.problemDescState);
				RetrieveCurrentRisk ();
				if (DetermineFrom ().Equals (String.Empty))
					BrightSparksDiv.InnerHtml = GenerateHtml ();
				else
					BrightSparksDiv.InnerHtml = GenerateHtml ();
				if (DetermineFrom ().Equals (String.Empty)) {
					RetrievePersonas ();
					if (this.HofResponse.Count > 0) {
						this.HofResponse.Shuffle ();
						Sessions.PersonasState = this.HofResponse;
						currentPersona = this.HofResponse [0].Name.ToString ();
						Sessions.PersonaState = currentPersona;
						BrightSparksDiv.InnerHtml = GenerateHtml (currentPersona);

					}
				} else {
					if (Sessions.PersonaState != String.Empty) {
						BrightSparksDiv.InnerHtml = GenerateHtml (Sessions.PersonaState);
					} else {
						BrightSparksDiv.InnerHtml = GenerateHtml ();
					}
				}
			}
		}

		private string DetermineFrom()
		{
			string c = String.Empty;
			if (Request.QueryString["pb"] != null)
			{
				c = Request.QueryString["pb"];
				NameValueCollection filtered = new NameValueCollection(Request.QueryString);
				filtered.Remove("pb");			
			}
			return c;
		}

		private string GenerateHtml()
		{
			return Tag1a;
		}

		private string GenerateHtml(string persona)
		{
//			persona = persona.Replace(" ","%20");
			var enc = Uri.EscapeUriString (Tag2 + persona);
			return Tag1b + enc + Tag3;
		}

		#region Service call


		void RetrievePersonas ()
		{
			string errorMsg;
			if(Util.ServiceExists(Constants.HALLOFFAME_URI, false, out errorMsg)) {
				RiskHuntingAppTest.hofService.WebBasedHallofFameService hof = new RiskHuntingAppTest.hofService.WebBasedHallofFameService ();
				try
				{
					System.Net.ServicePointManager.Expect100Continue = false;
					var output = hof.RetrieveAllPersonasFromType (Constants.PERSONA_TYPE);
					this.HofResponse = Util.DeserializeHofResponse (output);
					if (Sessions.PersonaState != String.Empty)
						Session.Remove(Sessions.personaState);
					//			Session ["CURRENT_PAST_RISK_DESC"] = NLResponse;
				}
				catch (Exception ex)
				{
				}
				finally {
				}
			}
			else
			{
				this.HofResponse = new List<Persona> ();
				FormDiv.Visible = false;
				alert_message_success.Visible = false;
				errorMessage.InnerText = "Currently unable to generate superheroes. Please try again later.";
				alert_message_error.Visible = true;
			}
		}

		#endregion

		#region Xml generation

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

		public virtual void moreClicked(object sender, EventArgs args)
		{
			if (Sessions.PersonasState != null) {
				Util.AccessLog(Util.ScreenType.CreateIdea_Superheroes, Util.FeatureType.CreateIdea_Superheroes_GenerateNewSuperheroButton);

				this.HofResponse = Sessions.PersonasState;
				if (this.HofResponse.Count > 0) {
					this.HofResponse.Shuffle ();
					currentPersona = this.HofResponse [0].Name.ToString ();
					Sessions.PersonaState = currentPersona;
					BrightSparksDiv.InnerHtml = GenerateHtml (currentPersona);

				}
			}
		}

		public virtual void addNewIdeaClicked(object sender, EventArgs args)
		{
			Response.Redirect ("AddResolutionIdea.aspx?from=CreateIdeas_Superheroes.aspx");
		}

		public virtual void addClicked(object sender, EventArgs args)
		{
			if (Page.IsValid)
			{

//				if (!AddIdeaDescription.Text.Equals(ADDIDEA_WATERMARK)) {
//
//					this.currentRisk.State = RiskQueryState.IdeasGenerated;
//					this.currentRisk.Recommendations.Add (AddIdeaDescription.Text);
//
//					GenerateXml("SourceSpecification");
//					GenerateXml("Problem");
//					GenerateXml("Solution");
//
//				}

			}
		}
	}
}

