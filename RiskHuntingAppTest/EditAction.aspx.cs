using System;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace RiskHuntingAppTest
{
	
	public partial class EditAction : System.Web.UI.Page
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

		protected string requestId, sourceId;
		protected int index;

		protected Risk currentRisk;
		protected Action selectedAction;

		protected const string EDITACTION_WATERMARK = "[Enter an action description]";

		protected void Page_Init(object sender, EventArgs e)
		{
			if (Session ["CURRENT_RISK"] != null)
				this.sourceId = Session ["CURRENT_RISK"].ToString();	
			RetrieveCurrentRisk ();

//			if (!Page.IsPostBack) {
				this.requestId = DetermineID ();
				if (!this.requestId.Equals (String.Empty)) {
					if (this.currentRisk.Actions.Count > 0) {
						this.index = FindAction ();
						if (this.index > -1) {
							PopulateEditDateDropDown ();
							PopulateAction ();
						}
					}
//				} else {
//				}
			} 
		}

		private string DetermineID()
		{
			string id = String.Empty;
			if (Request.QueryString["id"] != null)
			{
				id = Request.QueryString["id"];
			}
			return id;
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

		int FindAction ()
		{
			int index = -1;
			this.selectedAction = new Action ();
			foreach (var ac in this.currentRisk.Actions) {
				if (this.requestId.Equals (ac.Id)) {
					this.selectedAction = ac;
					index = this.currentRisk.Actions.IndexOf (this.selectedAction);
				}
			}
			return index;
		}

		private void PopulateEditDateDropDown()
		{
			EditImplementedDay.Items.Clear();
			EditImplementedMonth.Items.Clear();
			EditImplementedYear.Items.Clear();

			var dayNow = DateTime.Now.Day;
			var monthNow = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
			var yearNow = DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);

			int count = 0;
			for (int i=1; i<=31; i++)
			{
				EditImplementedDay.Items.Add (new ListItem (i.ToString()));
				if (dayNow.Equals (i))
					EditImplementedDay.SelectedIndex = count;
				count++;
			}

			string[] monthNames = 
				System.Globalization.CultureInfo.CurrentCulture
					.DateTimeFormat.MonthGenitiveNames;
			for (int i=0; i<monthNames.Length; i++)
			{
				EditImplementedMonth.Items.Add (new ListItem (monthNames[i]));
				if (monthNow.Equals (monthNames[i]))
					EditImplementedMonth.SelectedIndex = i;
			}

			DateTime startDate = DateTime.Now;
			DateTime endDate = new DateTime(2018, 01, 01);
			while (startDate <= endDate)
			{
				EditImplementedYear.Items.Add (new ListItem (startDate.ToString("yyyy", CultureInfo.InvariantCulture)));
				startDate = startDate.AddYears(1);
			}
		}

		void PopulateAction ()
		{
			EditActionDescription.Text = this.selectedAction.Content;
			EditActionOwner.Text = this.selectedAction.Owner;
			EditImplementedDay.Value = this.selectedAction.ImplementBy.Day.ToString();
			EditImplementedMonth.Value = this.selectedAction.ImplementBy.ToString("MMMM", CultureInfo.InvariantCulture);
			EditImplementedYear.Value = this.selectedAction.ImplementBy.Year.ToString();
		}


		public virtual void updateClicked(object sender, EventArgs args)
		{
			if (Page.IsValid)
			{
				if (!EditActionDescription.Text.Equals(EDITACTION_WATERMARK))
				{
					Action updatedAction = new Action ();
					updatedAction.Id = this.selectedAction.Id;
					updatedAction.Content = EditActionDescription.Text;
					updatedAction.Owner = EditActionOwner.Text;
					var month = DateTime.ParseExact (EditImplementedMonth.Value, "MMMM", CultureInfo.CurrentCulture).Month;
					string dateString = EditImplementedDay.Value + "/" + month.ToString() + "/" + EditImplementedYear.Value;
					updatedAction.ImplementBy = DateTime.Parse (dateString).Date;


					this.currentRisk.Actions [this.index] = updatedAction;

					GenerateXml("SourceSpecification");
					GenerateXml("Problem");
					GenerateXml("Solution");

					Response.Redirect("Solution_ActionPlan.aspx");
				}
			}
		}

		public virtual void deleteClicked(object sender, EventArgs args)
		{
			if (Page.IsValid)
			{
				int index = this.currentRisk.Actions.IndexOf (this.selectedAction);
				if (index != -1)
					this.currentRisk.Actions.RemoveAt (index);

				GenerateXml("SourceSpecification");
				GenerateXml("Problem");
				GenerateXml("Solution");

				Response.Redirect("Solution_ActionPlan.aspx");
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

