using System;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace RiskHuntingAppTest
{


	public partial class ActionPlan : System.Web.UI.Page
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

		protected List<ListItem> items;
		protected int maxId;
		protected string sourceId;
		protected Risk currentRisk;
		protected Action selectedAction;

		const string initDropDownText = "[Select action to be edited or deleted]";
		const string noDropDownText = "<no actions>";
		const string LiStartTagLabel = "<li class=\"label\">";
		const string LiEndTag = "</li>";

		const string StartTag = "<asp:Label";
		const string MidTag = " Width=\"100%\" runat=\"server\">";
		const string EndTag = "</asp:Label>";

		const string aStartTag = "<a href=\"javascript:doLoad('";
		const string aMidTag = "');\">";
		const string aEndTag = "</a>";

		protected const string ADDACTION_WATERMARK = "[Enter your new action]";
		protected const string EDITACTION_WATERMARK = "[Select action from list above to edit]";


		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session ["CURRENT_RISK"] != null)
				this.sourceId = Session ["CURRENT_RISK"].ToString();	
			RetrieveCurrentRisk ();

			if (!Page.IsPostBack) {
				EditSection.Visible = false;
				items = GenerateActions ();
				InitDropDownLists (items);
				PopulateItems (items);
				PopulateElements ();
				PopulateAddDateDropDown ();
				PopulateEditDateDropDown ();
				Topbar_Problem_Search_Solution ();
			} else {
				EditSection.Visible = true;
			}
		}

		public List<ListItem> GenerateActions()
		{
			List<ListItem> items = new List<ListItem> ();

			ActionDropDown.Items.Clear ();
			int counter = 2;
			for (int i = 0; i < this.currentRisk.Actions.Count; i++) {
				items.Add (new ListItem (this.currentRisk.Actions[i].Content, counter.ToString()));
				counter++;
			}

			return items;
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

		private void PopulateElements()
		{
			AddActionDescription.WatermarkText = ADDACTION_WATERMARK;
			EditActionDescription.WatermarkText = EDITACTION_WATERMARK;
		}


		private void InitDropDownLists(List<ListItem> ideas)
		{
			if (ideas.Count > 0) {
				ActionDropDown.Items.Add (new ListItem (initDropDownText, "1"));
				foreach (var item in ideas) {
					ActionDropDown.Items.Add (item);
				}
			}
			else
				ActionDropDown.Items.Add (new ListItem (noDropDownText, "1"));
			ActionDropDown.SelectedValue = "1";

			//			ActionDropDown.Items.Add(ideas[0]);
			//			ActionDropDown.Items.Add(ideas[1]);
			//			ActionDropDown.Items.Add(ideas[2]);
			//			ActionDropDown.Items.Add(ideas[3]);
			//			ActionDropDown.Items.Add(ideas[4]);
			//			ActionDropDown.Items.Add(ideas[5]);
			//			ActionDropDown.Items.Add(ideas[6]);

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

		#endregion

		#region Service Call
		private void RetrieveNLData()
		{
			var watch = Stopwatch.StartNew();

			//			string riskDescription = String.Empty;
			//			if (Session ["CURRENT_PROBLEM_DESC"] != null)
			//				riskDescription = Session ["CURRENT_PROBLEM_DESC"].ToString ();

			try {
				RiskHuntingAppTest.antiqueService.AntiqueService antique = new RiskHuntingAppTest.antiqueService.AntiqueService ();
				antique.NLParserCompleted += new RiskHuntingAppTest.antiqueService.NLParserCompletedEventHandler (objAntique_NLParserCompleted);
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
			RiskHuntingAppTest.antiqueService.NLParserCompletedEventArgs e)
		{
			Console.WriteLine (e.Result);
			var NLResponse = Util.DeserializeNLResponse (e.Result);

			Session ["CURRENT_PROBLEM_DESC"] = NLResponse;
		}

		#endregion

		public void PopulateItems (List<ListItem> actions)
		{
			sourceDiv.InnerHtml = String.Empty;
			if (actions.Count > 0)
				for (int i=0; i < actions.Count; i++) {
					//				foreach (var item in ideas) {
					var item = actions [i];
					if (!item.Text.Equals(initDropDownText) || !item.Text.Equals(noDropDownText))
						sourceDiv.InnerHtml += LiStartTagLabel +
							GenerateContentHtml (item, i) +
							LiEndTag;
				}
			else
				sourceDiv.InnerHtml += LiStartTagLabel +
					GenerateContentHtml ("--no actions--") +
					LiEndTag;
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


		public virtual void updateClicked(object sender, EventArgs args)
		{
			if (Page.IsValid)
			{
				if (!EditActionDescription.Text.Equals(EDITACTION_WATERMARK))
				{
					//					string ok3 = String.Empty;
					//					int predicateID = Convert.ToInt32(ActionDropDown.SelectedItem.Value);
					//					if (ok3.Equals(String.Empty))
					//					{
					ActionDropDown.Items.FindByValue (ActionDropDown.SelectedItem.Value).Text = EditActionDescription.Text;
					EditActionDescription.WatermarkText = EDITACTION_WATERMARK;

					UpdateRisk (false);

					SubmitReport.InnerHtml = "<span class=\"greentitle\">The idea was updated successfully.</span>";

					//					}
					//					else
					//						SubmitReport.InnerHtml = "<span class=\"redtitle\">A problem occured. " + ok3 + "</span>";
				}
			}
		}

		public virtual void deleteClicked(object sender, EventArgs args)
		{
			if (Page.IsValid)
			{
				//				bool ok3 = true;
				//				if (ok3)
				//				{
				ListItem removeItem = ActionDropDown.Items.FindByValue (ActionDropDown.SelectedItem.Value);
				ActionDropDown.Items.Remove (removeItem);
				EditActionDescription.WatermarkText = EDITACTION_WATERMARK;

				UpdateRisk (false);

				if (this.currentRisk.Actions.Count == 0)
					this.currentRisk.State = RiskQueryState.IdeasGenerated;

				SubmitReport.InnerHtml = "<span class=\"greentitle\">The idea was deleted successfully.</span>";

				//				}
				//				else
				//					SubmitReport.InnerHtml = "<span class=\"redtitle\">A problem occured. " + ok3 + "</span>";
			}
		}

		public virtual void updateData(object sender, EventArgs args)
		{
			Console.WriteLine ("updateData");
			int actionID = Convert.ToInt32(ActionDropDown.SelectedItem.Value);
			this.selectedAction = this.currentRisk.Actions [actionID - 2];
			EditActionDescription.Text = this.selectedAction.Content;
			EditActionOwner.Text = this.selectedAction.Owner;
			EditImplementedDay.Value = this.selectedAction.ImplementBy.Day.ToString();
			EditImplementedMonth.Value = this.selectedAction.ImplementBy.ToString("MMMM", CultureInfo.InvariantCulture);
			EditImplementedYear.Value = this.selectedAction.ImplementBy.Year.ToString();
			if (ActionDropDown.Items[0].Text.Equals(initDropDownText))
				ActionDropDown.Items.RemoveAt (0);

		}

		public virtual void addClicked(object sender, EventArgs args)
		{
			if (Page.IsValid)
			{
				if (!this.sourceId.Equals (String.Empty) && !AddActionDescription.Text.Equals(ADDACTION_WATERMARK)) {
					int currentIndex = ActionDropDown.Items.Count + 1;
					ActionDropDown.Items.Add (new ListItem (AddActionDescription.Text, currentIndex.ToString()));
					AddActionDescription.WatermarkText = ADDACTION_WATERMARK;
					UpdateRisk (true);

//					Console.WriteLine ("day: " + ImplementedDay.Value);
//					Console.WriteLine ("month: " + ImplementedMonth.Value);
//					Console.WriteLine ("year: " + ImplementedYear.Value);
				}

			}
		}

		public virtual void saveRiskClicked(object sender, EventArgs args)
		{
			if (Page.IsValid)
			{
//				saveRisk.Visible = false;
				content2.Visible = false;

			}
		}

		public virtual void risksClicked(object sender, EventArgs args)
		{
			Response.Redirect("SearchResults.aspx");
		}

		void UpdateRisk (bool fromAdd)
		{
			//			RetrieveCurrentRisk ();


			//			foreach (var res in this.currentRisk.Resolutions)
			//				Console.WriteLine ("res after: " + res);

			if (fromAdd) {
//				this.currentRisk.State = RiskQueryState.SimilarRisksFound;
				Action action = new Action ();
				action.Id = Guid.NewGuid().ToString("N");
				action.Content = AddActionDescription.Text;
				action.Owner = AddActionOwner.Text;
				var month = DateTime.ParseExact (AddImplementedMonth.Value, "MMMM", CultureInfo.CurrentCulture).Month;
				string dateString = AddImplementedDay.Value + "/" + month.ToString() + "/" + AddImplementedYear.Value;
				action.ImplementBy = DateTime.Parse (dateString).Date;
				this.currentRisk.Actions.Add (action);
			} else {
				Action updatedAction = new Action ();
				updatedAction.Id = this.selectedAction.Id;
				updatedAction.Content = EditActionDescription.Text;
				updatedAction.Owner = EditActionOwner.Text;
				var month = DateTime.ParseExact (EditImplementedMonth.Value, "MMMM", CultureInfo.CurrentCulture).Month;
				string dateString = EditImplementedDay.Value + "/" + month.ToString() + "/" + EditImplementedYear.Value;
				updatedAction.ImplementBy = DateTime.Parse (dateString).Date;
				int index = this.currentRisk.Actions.IndexOf (this.selectedAction);
				if (index != -1)
					this.currentRisk.Actions [index] = updatedAction;


			}

			GenerateXml("SourceSpecification");
			GenerateXml("Problem");
			GenerateXml("Solution");

			RetrieveCurrentRisk ();
			items = GenerateActions ();
			InitDropDownLists(items);
			PopulateItems(items);
			Console.WriteLine ("this.currentRisk.State (UpdateRisk): " + this.currentRisk.State.ToString ());

		}




		#region Html related

		private string GenerateContentHtml(ListItem item, int counter)
		{
			return StartTag + MidTag + "ACTION " + ++counter + ": " + item.Text + EndTag;
		}

		private string GenerateContentHtml(string text)
		{
			return StartTag + MidTag + text + EndTag;
		}

		//		private string GenerateEditableContentHtml(string header, string content, int i)
		//		{
		//
		//			return "<textarea name=\"predicate" + i.ToString() + "\" rows=\"1\" cols=\"20\" id=\"predicate" + i.ToString() + "\" style=\"height:30px;width:100%;\">" + content + "</textarea>";
		//
		//			//<textarea name="queryText" rows="2" cols="20" id="queryText" style="height:50px;width:100%;"></textarea>
		//			//<asp:textbox id="queryText" runat="server" TextMode="Multiline" Width="100%" Height="50" ></asp:textbox>
		//		}
		//
		//		private string GenerateLeft1NavHtml()
		//		{
		//			return aStartTag + "ViewAnalogies.aspx" + aMidTag + "back" + aEndTag;
		//		}



		#endregion


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


		private void GenerateXml()
		{
			string Ref;
			string xmlUri;

			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = Util.CreateSolutionXml (this.currentRisk);
			Ref = Constants.CASEREF + this.sourceId + "_" + "Solution" + ".xml";
			xmlUri = Path.Combine (processPath, "Solution", Ref);
			XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri);
			foreach (var res in this.currentRisk.Actions)
				Console.WriteLine ("res: " + res);
		}

		private XmlProc.SourceSpecificationSerialized.SourceSpecification CreateSourceSpecificationXml()
		{
			XmlProc.SourceSpecificationSerialized.SourceSpecification ss = new XmlProc.SourceSpecificationSerialized.SourceSpecification();

			DateTime now = DateTime.Now;
			ss.SourceId = maxId;
			this.sourceId = maxId.ToString();
			ss.SourceName = this.currentRisk.Name;
			ss.SourceType = SOURCE_TYPE;
			ss.Domain = CASE_TYPE;
			ss.Filename = ss.SourceType + maxId + ".docx";
			ss.LaunchDate = now.ToString();
			ss.SourceSpecificationLastEdited = now.ToString();

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification fspecProblem = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification();
			fspecProblem.FacetSpecificationLanguage = "Text";
			fspecProblem.FacetSpecificationLink = ss.SourceType + maxId + "_Problem.xml";

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification fspecSolution = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification();
			fspecSolution.FacetSpecificationLanguage = "Text";
			fspecSolution.FacetSpecificationLink = ss.SourceType + maxId + "_Solution.xml";

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet problem = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet();
			problem.FacetType = "Problem";
			problem.Author = this.currentRisk.Author;
			problem.FacetSpecification = fspecProblem;

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet solution = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet();
			solution.FacetType = "Solution";
			solution.Author = this.currentRisk.Author;
			solution.FacetSpecification = fspecSolution;

			ss.Facet = new System.Collections.Generic.List<XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet>();
			ss.Facet.Add(problem);
			ss.Facet.Add(solution);

			return ss;
		}

		private XmlProc.ProblemSerialized.LanguageSpecificSpecification CreateProblemXml()
		{
			XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = new XmlProc.ProblemSerialized.LanguageSpecificSpecification();

			DateTime now = DateTime.Now;
			problem.FacetType = "Problem";
			problem.FacetSpecificationLanguage = "Text";
			problem.Author = this.currentRisk.Author;
			problem.LaunchDate = now.ToString();
			problem.SourceSpecificationLastEdited = now.ToString();

			XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData fsd = new XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData();
			fsd.Content = currentRisk.Content;
			//fsd.Observations = new System.Collections.Generic.List<XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation>();
			//XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation obs = new XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation();
			//obs.id = 1;
			//obs.launchDate = now.ToString();
			//obs.Value = "n/A";
			//fsd.Observations.Add(obs);
			fsd.Observations = String.Empty;
			fsd.ObservedBehaviour = String.Empty;
			fsd.TreatmentType = String.Empty;
			fsd.DateOfIncident = String.Empty;
			fsd.AilmentType = String.Empty;
			fsd.TriggeringEvent = String.Empty;
			fsd.Miscellaneous = String.Empty;
			fsd.MatchingDetails = String.Empty;

			problem.FacetSpecificationData = fsd;


			return problem;
		}

		private XmlProc.SolutionSerialized.LanguageSpecificSpecification CreateSolutionXml()
		{
			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = new XmlProc.SolutionSerialized.LanguageSpecificSpecification();

			DateTime now = DateTime.Now;
			solution.FacetType = "Solution";
			solution.FacetSpecificationLanguage = "Text";
			solution.Author = this.currentRisk.Author;
			solution.LaunchDate = now.ToString();
			solution.SourceSpecificationLastEdited = now.ToString();

			XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData fsd = new XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData();

			string completeSolutionString = currentRisk.Recommendation + ";" + currentRisk.CorrectiveActions + ";" + currentRisk.Countermeasures;
			fsd.Content = completeSolutionString;
			fsd.Miscellaneous = String.Empty;

			solution.FacetSpecificationData = fsd;


			return solution;
		}

		private int GetNewSourceId()
		{
			int maxId = 0;
			string path = FindLatestSourcePath();
			if (!path.Equals(String.Empty))
			{
				if (File.Exists(path))
				{
					XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(path);
					maxId = Convert.ToInt32(ss.SourceId);
					maxId = maxId + 1;
				}
			}
			else
				maxId = 1;
			return maxId;
		}

		private string FindLatestSourcePath()
		{
			string path = String.Empty;
			// get your files (names)
			string[] fileNames = Directory.GetFiles(Path.Combine (sourcesPath, CASE_TYPE, SOURCESPECIFICATION), "*.*");
			if (fileNames.Length > 0)
			{
				// Now read the creation time for each file
				DateTime[] creationTimes = new DateTime[fileNames.Length];
				for (int i = 0; i < fileNames.Length; i++)
					creationTimes[i] = new FileInfo(fileNames[i]).CreationTime;

				// sort it
				Array.Sort(creationTimes, fileNames);
				path = fileNames[fileNames.Length - 1];
			}

			return path;
		}
		#endregion

	}
}

