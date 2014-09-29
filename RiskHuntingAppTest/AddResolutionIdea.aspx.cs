using System;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace RiskHuntingAppTest
{
	
	public partial class AddResolutionIdea : System.Web.UI.Page
	{

		protected string sourcesPath = SettingsTool.GetApplicationPath() + "xmlFiles/Sources/";
		protected const string SOURCESPECIFICATION = "SourceSpecification";
		protected const string PROBLEM = "Problem";
		protected const string SOLUTION = "Solution";
		protected const string ADDITIONAL = "Additional";
		protected const string PROCESSFOLDER = "_toProcess";

		protected string processPath = SettingsTool.GetApplicationPath() + "xmlFiles/Sources/_toProcess/";

		protected const string SOURCE_TYPE = Constants.CASEREF;
		protected const string CASE_TYPE = "Risk";

		protected string requestContent, sourceId;

		protected Risk currentRisk;

		protected const string ADDIDEA_WATERMARK = "[Enter your new resolution idea]";

		protected void Page_Init(object sender, EventArgs e)
		{
			if (Session ["CURRENT_RISK"] != null)
				this.sourceId = Session ["CURRENT_RISK"].ToString();	
			RetrieveCurrentRisk ();

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
			

		public virtual void addClicked(object sender, EventArgs args)
		{
			if (Page.IsValid)
			{
				if (!AddIdeaDescription.Text.Equals(ADDIDEA_WATERMARK)) {

					this.currentRisk.State = RiskQueryState.IdeasGenerated;
					this.currentRisk.Recommendations.Add (AddIdeaDescription.Text);

					GenerateXml("SourceSpecification");
					GenerateXml("Problem");
					GenerateXml("Solution");

					Response.Redirect("Solution_ResolutionIdeas.aspx");

				}

			}
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
				xmlUri = sourcesPath + CASE_TYPE + "/" + SOURCESPECIFICATION + "/" + Ref;
				xmlUri2 = sourcesPath + PROCESSFOLDER + "/" + SOURCESPECIFICATION + "/" + Ref;
				XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Save(ss, xmlUri);
				XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Save(ss, xmlUri2);
			}
			else if (componentType.Equals("Problem"))
			{
				XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = Util.CreateProblemXml(this.currentRisk);
				Ref = SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
				xmlUri = sourcesPath + CASE_TYPE + "/" + PROBLEM + "/" + Ref;
				xmlUri2 = sourcesPath + PROCESSFOLDER + "/" + PROBLEM + "/" + Ref;
				XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Save(problem, xmlUri);
				XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Save(problem, xmlUri2);
			}
			else if (componentType.Equals("Solution"))
			{
				XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = Util.CreateSolutionXml(this.currentRisk);
				Ref = SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
				xmlUri = sourcesPath + CASE_TYPE + "/" + SOLUTION + "/" + Ref;
				xmlUri2 = sourcesPath + PROCESSFOLDER + "/" + SOLUTION + "/" + Ref;
				XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri);
				XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri2);
			}

		}

		#endregion
	}
}

