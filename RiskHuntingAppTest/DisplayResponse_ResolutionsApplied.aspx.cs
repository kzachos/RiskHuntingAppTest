using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

namespace RiskHuntingAppTest
{

	public partial class DisplayResponse_ResolutionsApplied : System.Web.UI.Page
	{
		const string defaultProcessGuidance = "Select any of the following that apply to your risk. Be open-minded";

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

		const string Tag4a = "<input id=\"CheckBox";
		const string Tab4b = "\" AutoPostBack=\"True\" type=\"checkbox\" name=\"CheckBox";
		const string Tag4c = "\" />";

		protected string processPath = SettingsTool.GetApplicationPath() + "xmlFiles/Sources/_toProcess/";

		protected string sourceId;
		SortedList categoryIDList;
		protected Risk currentRisk;


		protected void Page_Init(object sender, EventArgs e)
		{
			if (Session ["CURRENT_RISK"] != null)
				sourceId = Session ["CURRENT_RISK"].ToString();

			if (!Page.IsPostBack) {
//				RePopulateValues ();
				ResolutionIdeas.Visible = false;


//				categoryIDList = new SortedList();
//				if (Session["CHECKED_ITEMS"] != null)
//					categoryIDList = (SortedList)Session["CHECKED_ITEMS"];

				Console.WriteLine ("Page_Init - NOT Page.IsPostBack");
				GenerateContent ();

			}
			else {
				Console.WriteLine ("Page_Init - Page.IsPostBack");
				content2.Controls.Clear ();

//				categoryIDList = new SortedList();
//				if (Session["CHECKED_ITEMS"] != null)
//					categoryIDList = (SortedList)Session["CHECKED_ITEMS"];
			
				GenerateContent ();
			}
			Topbar_Problem_Search_Solution ();
	
			var processGuidanceText = Util.GenerateProcessGuidance ("riskResolutionsApplied");
			creativeGuidance.InnerText = processGuidanceText.Equals(String.Empty)?defaultProcessGuidance:processGuidanceText;
		}

//		protected void On_Init (object sender, EventArgs e)
//		{
//
//		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session ["CURRENT_RISK"] != null)
				sourceId = Session ["CURRENT_RISK"].ToString();

			if (!Page.IsPostBack) {


				Console.WriteLine ("Page_Load - NOT Page.IsPostBack");
//				CreateSessionValues ();
			} else {
//				Console.WriteLine ("Page_Load - Page.IsPostBack");
//				GenerateContent ();
			}

		}

		#region Initializing

		private void Topbar_Problem_Search_Solution ()
		{
			TopbarProblemSearchSolution.Visible = true;
		}

		#endregion

		private CheckBox CreateCheckboxControl(string id, string content)
		{
			var k = new CheckBox();
			k.ID = id;
			k.AutoPostBack = false;
			k.ToolTip = content;
//			k.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);

			if (categoryIDList != null) 
				categoryIDList.Add (k.ID, k);

			return k;
		}

		private void CheckState()
		{
			foreach (var ctrl in content2.Controls)
			{
				Console.WriteLine ("type: " + ctrl.GetType ().ToString());
				if (ctrl is CheckBox)
					if (((CheckBox) ctrl).Checked)
						Console.WriteLine ("Checked");
					else
						Console.WriteLine ("UnChecked");
			}

//			Table t = (Table) Session["MyTable"];
//
//			if (((CheckBox) t.Rows[0].Cells[0].Controls[0]).Checked)
//				Console.WriteLine ("Checked");
//			else
//			{
//				Console.WriteLine ("UnChecked");
//			}
		}

		private void GenerateContent ()
		{
			string id = DetermineID ();
			string responseUri = DetermineResponseUri ();

			if (!responseUri.Equals (String.Empty)) {
				XmlProc.ResponseSerialized.MatchedSources response = XmlProc.ObjectXMLSerializer<XmlProc.ResponseSerialized.MatchedSources>.Load (responseUri);

				List<XmlProc.ResponseSerialized.MatchedSourcesMatchedSource> matchedSources = (List<XmlProc.ResponseSerialized.MatchedSourcesMatchedSource>)response.MatchedSource;

				int counter = 0;
				foreach (XmlProc.ResponseSerialized.MatchedSourcesMatchedSource matchedSource in matchedSources) {
					if (matchedSource.SourceId == id) {
						//                        currentIndex = matchedSourcesIds.IndexOfValue(id);
						//SortedList elements = SeperateStringByChar(matchedSource.Content);

//						title.InnerHtml = String.Empty;

						var recommendation = Util.ExtractAttributeContentFromString2 (matchedSource.Content, "Recommendation").Trim();
						if (!recommendation.Equals (String.Empty))
							GenerateHtml3 (recommendation, String.Empty, counter++);
						//								content2.InnerHtml += GenerateHtml3 (recommendation, String.Empty, 0);

						var correctiveActions = Util.ExtractAttributeContentFromString2 (matchedSource.Content, "Corrective Actions");
						if (!correctiveActions.Equals (String.Empty))
							GenerateHtml3 (correctiveActions, String.Empty, counter++);
						//								content2.InnerHtml += GenerateHtml3 (correctiveActions, String.Empty, 0);

						string ignoreWord = "None";
						var countermeasures = Util.ExtractAttributeContentFromString2 (matchedSource.Content, "Countermeasures");
						if (!countermeasures.Equals (String.Empty) 
							&& !countermeasures.ToLower().Contains (ignoreWord.ToLower())
						)
							GenerateHtml3 (countermeasures, String.Empty, counter++);
						//								content2.InnerHtml += GenerateHtml2 (countermeasures, String.Empty, 0);

						//							var c = new CheckBox ();
						//							c.ID = "CheckBox0";
						//							c.CheckedChanged += new EventHandler (CheckBox1_CheckedChanged);
						break;

					}
				}

			}
		}

		private string DetermineID()
		{
			string id = String.Empty;
			if (Request.QueryString["id"] != null)
			{
				id = Request.QueryString["id"];
				Session["CurrentOriginalID"] = id;
			}
			else
			{
				if (Session["CurrentOriginalID"] != null)
					id = Session["CurrentOriginalID"].ToString();
			}
			return id;
		}

		private string DetermineResponseUri()
		{
			string responseUri = String.Empty;
			if (Session["CurrentResponseUri"] != null)
				responseUri = Session["CurrentResponseUri"].ToString();
			else
			{
				if (Request.QueryString["path"] != null)
					responseUri = Request.QueryString["path"];
			}
			return responseUri;
		}


		private string GenerateHtml (string main, string second, int id)
		{
			return Tag1 + Tag2 + main + Tag3 + Tag4 + id + Tag5 + Tag6 +
				Tag7 + Tag8 + second + Tag9 + Tag10;
		}

		private string GenerateHtml2 (string main, string second, int id)
		{
			return Tag1 + Tag2 + main + Tag3 + Tag4a + id + Tab4b + id + Tag4c + Tag6 +
				Tag7 + Tag8 + second + Tag9 + Tag10;
		}

		private void GenerateHtml3 (string main, string second, int id)
		{
			content2.Controls.Add (new LiteralControl (Tag1)); 
			content2.Controls.Add (new LiteralControl (Tag2));
			content2.Controls.Add (new LiteralControl (main));
			content2.Controls.Add (new LiteralControl (Tag3));

			if (categoryIDList != null && categoryIDList.Count > 0) {
				if (categoryIDList.ContainsKey (id.ToString ())) {
					CheckBox chk = (CheckBox)categoryIDList.GetByIndex (id);
					content2.Controls.Add (chk);
				} else
					content2.Controls.Add (CreateCheckboxControl (id.ToString (), main));
			}
			else
				content2.Controls.Add (CreateCheckboxControl (id.ToString (), main));

			content2.Controls.Add (new LiteralControl (Tag6));
			content2.Controls.Add (new LiteralControl (Tag7));
			content2.Controls.Add (new LiteralControl (Tag8));
			content2.Controls.Add (new LiteralControl (second));
			content2.Controls.Add (new LiteralControl (Tag9));
			content2.Controls.Add (new LiteralControl (Tag10));
		}

		public virtual void submitClicked(object sender, EventArgs args)
		{
//			Console.WriteLine (this.sourceId);
			if (!this.sourceId.Equals (String.Empty)) {
				UpdateRisk ();
				Response.Redirect("DisplayResponse_CreativeGuidance.aspx");
			}
			else
				SubmitReport.InnerHtml = "<span class=\"redtitle\">No resolutions were added to the current risk. A problem occured.</span>";

			ResolutionIdeas.Visible = true;

//			RememberOldValues ();
//			foreach (var ctrl in content2.Controls)
//			{
//				Console.WriteLine ("type: " + ctrl.GetType ().ToString());
//				if (ctrl is CheckBox) {
//					Console.WriteLine ("test: " + ((CheckBox)ctrl).Text);
//					if (((CheckBox)ctrl).Checked)
//						Console.WriteLine ("Checked");
//					else
//						Console.WriteLine ("UnChecked");
//				}
//			}
		}


		void UpdateRisk ()
		{
			RetrieveCurrentRisk ();
			bool entryAdded = false;
			for (int i = 0; i < content2.Controls.Count; i++) {
				var ctrl = content2.Controls [i];
				if (ctrl is CheckBox) {
					bool result = ((CheckBox)ctrl).Checked;
					if (result)
					if ((!this.currentRisk.Recommendations.Contains (RetrieveCheckboxText (((CheckBox)ctrl).ClientID)))
						&& (!((CheckBox)ctrl).ToolTip.Trim ().Equals(String.Empty))) {
						this.currentRisk.Recommendations.Add (((CheckBox)ctrl).ToolTip.Trim ());
						entryAdded = true;
					}
				}
			}
			this.currentRisk.Recommendations.Remove (String.Empty);

			GenerateXml ();
			if (entryAdded)
				SubmitReport.InnerHtml = "<span class=\"greentitle\">The resolution(s) were added to the current risk successfully.</span>";
			else
				SubmitReport.InnerHtml = "<span class=\"redtitle\">No resolutions were added to the current risk. The resolution(s) already exist.</span>";
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

		private void GenerateXml()
		{
			string Ref;
			string xmlUri;

			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = Util.CreateSolutionXml (this.currentRisk);
			Ref = Constants.CASEREF + this.sourceId + "_" + "Solution" + ".xml";
			xmlUri = processPath + "/" + "Solution" + "/" + Ref;
			XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri);

		}
			
		public virtual void resolutionIdeasClicked(object sender, EventArgs args)
		{
			Response.Redirect("Solution_ResolutionIdeas.aspx");
		}

		void CheckBox1_CheckedChanged(object sender, EventArgs e)  
		{  
			Console.WriteLine ("test: " + ((CheckBox)sender).Text);
		}  

		private string RetrieveCheckboxText (string id)
		{
			string text = String.Empty;
			foreach (var ctrl in content2.Controls) {
				if (ctrl is CheckBox) {
					if (((CheckBox)ctrl).ClientID.Equals (id))
						text = ((CheckBox)ctrl).ToolTip.Trim();
				}
			}
			return text;
		}

		private Control RetrieveCheckboxControl (string id)
		{
			foreach (var ctrl in content2.Controls) {
				if (ctrl is CheckBox) {
					if (((CheckBox)ctrl).ClientID.Equals (id))
						return (CheckBox)ctrl;
				}
			}
			return null;
		}

//		private void CreateSessionValues()
//		{
//			if (Session ["CHECKED_ITEMS"] == null) {
//				SortedList categoryIDList = new SortedList ();
//				for (int i = 0; i < content2.Controls.Count; i++) 
//				{
//					var ctrl = content2.Controls [i];
//					Console.WriteLine(ctrl.GetType ().ToString());
//					if (ctrl is CheckBox) {
//						categoryIDList.Add (i, ((CheckBox)ctrl).ClientID);
//					}
//					else
//						categoryIDList.Add (i, "none");
//				}
//				if (categoryIDList != null && categoryIDList.Count > 0)
//					Session ["CHECKED_ITEMS"] = categoryIDList;
//
//				Console.WriteLine ("CreateSessionValues");
//				foreach (var kvp in categoryIDList.Keys)
//				{
//					Console.WriteLine(kvp);
//					//					Console.WriteLine(kvp);
//				}
//				foreach (var kvp in categoryIDList.Values)
//				{
//					Console.WriteLine(kvp);
////					Console.WriteLine(kvp);
//				}
//			}
//		}
//
//		private void RememberOldValues()
//		{
//
//			if (Session ["CHECKED_ITEMS"] != null)
//				categoryIDList.Clear ();
//			else
//				categoryIDList = new SortedList ();
//
//			Console.WriteLine ("RememberOldValues");
//			foreach (var kvp in categoryIDList.Keys)
//			{
//				Console.WriteLine(kvp);
//				//					Console.WriteLine(kvp);
//			}
//			foreach (var kvp in categoryIDList.Values)
//			{
//				Console.WriteLine(kvp);
//				//					Console.WriteLine(kvp);
//			}
//
////			for (int i = 0; i < content2.Controls.Count; i++) 
////			{
////				var k = categoryIDList [i];
//////				Console.WriteLine(k);
////				if (!k.Equals ("none")) {
////					var ctrl = content2.Controls [i-1];
//////					Console.WriteLine(ctrl.GetType ().ToString());
////
////					if (ctrl is CheckBox) {
//////						Console.WriteLine (((CheckBox)ctrl).ClientID);
////						bool result = ((CheckBox)ctrl).Checked;
////						if (!result)
////							categoryIDList[i] = "unchecked";
////					}
////				}
////			
////
////			}
//
//			for (int i = 0; i < content2.Controls.Count; i++) {
//				var ctrl = content2.Controls [i];
//				if (ctrl is CheckBox) {
//					categoryIDList.Add (((CheckBox)ctrl).ClientID, (CheckBox)ctrl);
//				}
//
//			}
//
////				if (ctrl is CheckBox) {
////
////					bool result = ((CheckBox)ctrl).Checked;
////					// Check in the Session
////
////					if (!result)
//////					{
//////						if (!categoryIDList.Contains(i))
//////							categoryIDList.Add(i, ((CheckBox)ctrl).ClientID);
//////					}
//////					else
////						categoryIDList.Remove(i);
////				}
////				else
////					categoryIDList.Remove(i);
//
//
//			if (categoryIDList != null && categoryIDList.Count > 0)
//				Session["CHECKED_ITEMS"] = categoryIDList;
//
//
//		}
//
//		private void RePopulateValues()
//		{
//			SortedList categoryIDList = (SortedList)Session["CHECKED_ITEMS"];
//			if (categoryIDList != null && categoryIDList.Count > 0)
//			{
//				for (int i = 0; i < content2.Controls.Count; i++) 
//				{
//					var k = categoryIDList [i];
//					//				Console.WriteLine(k);
//					if (!k.Equals ("none")) {
//						if (!k.Equals ("unchecked")) {
//							var ctrl = content2.Controls [i];
//												Console.WriteLine(ctrl.GetType ().ToString());
//
//							if (ctrl is CheckBox) {
//								//						Console.WriteLine (((CheckBox)ctrl).ClientID);
//								bool result = ((CheckBox)ctrl).Checked;
//								if (result) {
//									content2.Controls.RemoveAt (i);
//									Console.WriteLine ("In: " + i);
//								}
//							}
//						}
//					}
//
////					var ctrl = content2.Controls [i];
//////					if (ctrl is CheckBox) {
////
////						if (categoryIDList.Contains(i))
////						{
////						Console.WriteLine ("In: ");
////							content2.Controls.RemoveAt (i);
//////							content2.Controls.Add (CreateCheckboxControl(((CheckBox)ctrl).ClientID, ((CheckBox)ctrl).ToolTip));
////						}
////					}
//
//				}
//
//			}
//		}

	}
}

