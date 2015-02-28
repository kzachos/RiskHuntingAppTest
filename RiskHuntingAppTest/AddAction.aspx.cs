using System;
using System.IO;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace RiskHuntingAppTest
{
	
	public partial class AddAction : System.Web.UI.Page
	{
		
		protected string sourcesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources");
		protected const string SOURCESPECIFICATION = "SourceSpecification";
		protected const string PROBLEM = "Problem";
		protected const string SOLUTION = "Solution";
		protected const string ADDITIONAL = "Additional";
		protected const string PROCESSFOLDER = "_toProcess";

		protected string processPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources", "_toProcess");

		protected const string SOURCE_TYPE = Constants.CASEREF;
		protected const string CASE_TYPE = "Risk";

		protected string sourceId;

		protected Risk currentRisk;

		protected const string ADDACTION_WATERMARK = "[Enter your new action]";

		protected void Page_Init(object sender, EventArgs e)
		{
			if (Session ["CURRENT_RISK"] != null)
				this.sourceId = Session ["CURRENT_RISK"].ToString();	
			RetrieveCurrentRisk ();
			PopulateAddDateDropDown ();

		}
			

		void RetrieveCurrentRisk ()
		{
			string location = String.Empty;

			location = Path.Combine (processPath, "SourceSpecification", Constants.CASEREF + this.sourceId + "_" + "SourceSpecification" + ".xml");
			XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(location);

			location = Path.Combine (processPath, "Problem", Constants.CASEREF + this.sourceId + "_" + "Problem" + ".xml");
			XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Load(location);

			location = Path.Combine (processPath, "Solution", Constants.CASEREF + this.sourceId + "_" + "Solution" + ".xml");
			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Load(location);

			this.currentRisk = new Risk (ss, problem, solution);

		}
			
		private void PopulateAddDateDropDown()
		{
			AddImplementedDay.Items.Clear();
			AddImplementedMonth.Items.Clear();
			AddImplementedYear.Items.Clear();

			var dayNow = DateTime.Now.Day;
			var monthNow = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
			var yearNow = DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);

			int count = 0;
			for (int i=1; i<=31; i++)
			{
				AddImplementedDay.Items.Add (new ListItem (i.ToString()));
				if (dayNow.Equals (i))
					AddImplementedDay.SelectedIndex = count;
				count++;
			}

			string[] monthNames = 
				System.Globalization.CultureInfo.CurrentCulture
					.DateTimeFormat.MonthGenitiveNames;
			for (int i=0; i<monthNames.Length; i++)
			{
				AddImplementedMonth.Items.Add (new ListItem (monthNames[i]));
				if (monthNow.Equals (monthNames[i]))
					AddImplementedMonth.SelectedIndex = i;
			}

			DateTime startDate = DateTime.Now;
			DateTime endDate = new DateTime(2018, 01, 01);
			while (startDate <= endDate)
			{
				AddImplementedYear.Items.Add (new ListItem (startDate.ToString("yyyy", CultureInfo.InvariantCulture)));
				startDate = startDate.AddYears(1);
			}
		}
			

		public virtual void addClicked(object sender, EventArgs args)
		{
			if (Page.IsValid)
			{
				if (!AddActionDescription.Text.Equals(ADDACTION_WATERMARK)) {

//					this.currentRisk.State = RiskQueryState.ActionsFormulated;
					Action action = new Action ();
					action.Id = Guid.NewGuid().ToString("N");
					action.Content = AddActionDescription.Text;
					action.Owner = AddActionOwner.Text;
					var month = DateTime.ParseExact (AddImplementedMonth.Value, "MMMM", CultureInfo.CurrentCulture).Month;
					string dateString = AddImplementedDay.Value + "/" + month.ToString() + "/" + AddImplementedYear.Value;
					action.ImplementBy = DateTime.Parse (dateString).Date;
					this.currentRisk.Actions.Add (action);

					GenerateXml("SourceSpecification");
					GenerateXml("Problem");
					GenerateXml("Solution");

					Response.Redirect("Solution_ActionPlan.aspx");

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

