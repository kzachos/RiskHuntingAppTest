using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;

namespace RiskHuntingAppTest
{
	
	public partial class CreateIdeas_PastRisk : System.Web.UI.Page
	{
		const string defaultProcessGuidance = "Create new resolutions for your risk. Select from the guidance below";

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


		int total;

		List<NLResponseToken> NLResponse;

		protected string Expression = "Think about ";

		protected string processPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources", "_toProcess");

		protected IList<string> CreativityPromptsFeed;
		protected string sourceId;
		protected Risk currentRisk;
		protected string currentRiskDescription;

		protected void Page_Init(object sender, EventArgs e)
		{
			generatePrompts.Visible = true;
			alert_message_success.Visible = false;
			alert_message_error.Visible = false;
			if (Session ["CURRENT_RISK"] != null)
				sourceId = Session ["CURRENT_RISK"].ToString();

			GenerateContent ();

			if (Session ["CREATIVITY_PROMPTS_PAST_RISK"] != null) {
//				if (Session ["CURRENT_PAST_RISK_DESC"] != null) 
//					Session.Remove ("CURRENT_PAST_RISK_DESC");
				CreativityPromptsFeed = (IList<string>) Session ["CREATIVITY_PROMPTS_PAST_RISK"];
				CreativityPromptsFeed.Shuffle ();
				PopulateData ();
			} else {
//				if (Session ["CURRENT_PAST_RISK_DESC"] != null)
//					NLResponse = (List<NLResponseToken>)Session ["CURRENT_PAST_RISK_DESC"];
//				else {
//					NLResponse = new List<NLResponseToken> ();
					RetrieveCurrentRisk ();
					RetrieveNLData ();
//				}
				PrepareData ();
				PopulateData ();
			}

			var processGuidanceText = Util.GenerateProcessGuidance ("creativeGuidance");
			creativeGuidance.InnerText = processGuidanceText.Equals(String.Empty)?defaultProcessGuidance:processGuidanceText;

			if (Page.IsPostBack) {
				Console.WriteLine ("Page_Init - Page.IsPostBack");
				content2.Controls.Clear ();
				GenerateContent ();
//				hint_box.Visible = false;
			} else {
				Console.WriteLine ("Page_Init - NOT Page.IsPostBack");
//				GenerateContent ();
			}
		}



		protected void Page_Load(object sender, EventArgs e)
		{

			if (Session ["CURRENT_RISK"] != null)
				sourceId = Session ["CURRENT_RISK"].ToString();
		}

		#region Creativity Prompts

		void RetrieveNLData ()
		{
			RiskHuntingAppTest.antiqueService.AntiqueService antique = new RiskHuntingAppTest.antiqueService.AntiqueService ();
			System.Net.ServicePointManager.Expect100Continue = false;
			var output = antique.NLParser (this.currentRisk.Content);
			this.NLResponse = Util.DeserializeNLResponse (output);
			if (Session ["CREATIVITY_PROMPTS_PAST_RISK"] != null)
				Session.Remove("CREATIVITY_PROMPTS_PAST_RISK");
//			Session ["CURRENT_PAST_RISK_DESC"] = NLResponse;
		}

		List<string> GenerateGenericCreativityPrompts()
		{
			List<NLResponseToken> NLResponseTrimmed = new List<NLResponseToken> () ;
			foreach(var item in NLResponse)
			{
				if (!item.TermValue.Equals (String.Empty))
					NLResponseTrimmed.Add (item);
			}

			int count = 0;
			List<string> ps = new List<string> ();
			NLResponseTrimmed.Shuffle ();

			if (NLResponseTrimmed.Count > 5)
				for (int i = 0; i < 4; i++) {
					//			foreach (var item in NLResponseTrimmed) {
					var item = NLResponseTrimmed [i];
					if (!item.TermValue.Equals (String.Empty)) {
						GenericCreativityPrompts g = new GenericCreativityPrompts (item.TermValue, item.Pos);
						foreach (string s in  g.genericCPs) {
							//							Console.WriteLine (s);
							ps.Add (Expression + s);
							count++;
						}
					}
				}
			else
				foreach (var item in NLResponseTrimmed) {
					if (!item.TermValue.Equals (String.Empty)) {
						GenericCreativityPrompts g = new GenericCreativityPrompts (item.TermValue, item.Pos);
						foreach (string s in  g.genericCPs) {
							//							Console.WriteLine (s);
							ps.Add (Expression + s);
							count++;
						}
					}
				}
			Console.WriteLine ("ps.Count: " + ps.Count);
			return ps;
		}

		List<string> GenerateGenericCreativityPromptsStatic()
		{
			List<NLResponseToken> NLResponseTrimmed = new List<NLResponseToken> () ;
			foreach(var item in NLResponse)
			{
				if (!item.TermValue.Equals (String.Empty))
					NLResponseTrimmed.Add (item);
			}

			int count = 0;
			List<string> ps = new List<string> ();
			NLResponseTrimmed.Shuffle ();

			string[] valuesNP = new string[2] {"floor", "slippery"};
			string[] valuesVP = new string[1] {"is wet"};

			foreach (var item in valuesNP) {
				if (!item.Equals (String.Empty)) {
					GenericCreativityPrompts g = new GenericCreativityPrompts (item, "NP");
					foreach (string s in  g.genericCPs) {
						//							Console.WriteLine (s);
						ps.Add (Expression + s);
						count++;
					}
				}
			}

			foreach (var item in valuesVP) {
				if (!item.Equals (String.Empty)) {
					GenericCreativityPrompts g = new GenericCreativityPrompts (item, "VP");
					foreach (string s in  g.genericCPs) {
						//							Console.WriteLine (s);
						ps.Add (Expression + s);
						count++;
					}
				}
			}
			Console.WriteLine ("ps.Count: " + ps.Count);
			return ps;
		}

		void PrepareData ()
		{
			if (!Page.IsPostBack) {
				CreativityPromptsFeed = GenerateGenericCreativityPrompts ();
				//				CreativityPromptsFeed = GenerateGenericCreativityPromptsStatic ();
				CreativityPromptsFeed.Shuffle ();

				Console.WriteLine ("****** PrepareData *******");
				foreach (var item in CreativityPromptsFeed) {
					Console.WriteLine (item);
				}
				Console.WriteLine ("CreativityPromptsFeed.Count: " + CreativityPromptsFeed.Count);
			} else {

				if (Session ["CREATIVITY_PROMPTS_PAST_RISK"] != null)
					CreativityPromptsFeed = (IList<string>)Session ["CREATIVITY_PROMPTS_PAST_RISK"];
				else {
					Console.WriteLine ("no CREATIVITY_PROMPTS");
				}
			}

		}

		void PopulateData ()
		{
			if (!Page.IsPostBack) {
				Console.WriteLine ("****** PopulateData *******");
				this.total = CreativityPromptsFeed.Count < Constants.MaxPromptsAtATime ? CreativityPromptsFeed.Count : Constants.MaxPromptsAtATime;
				int counter = 0;
				if (this.total > 0)
				{
					for (int i = 0; i < this.total; i++) {
						GenerateHtml3 (CreativityPromptsFeed [i], String.Empty, counter++);
						Console.WriteLine (CreativityPromptsFeed [i]);
					}
					Session ["CREATIVITY_PROMPTS_PAST_RISK"] = CreativityPromptsFeed;
				}
				else
					GenerateHtml3 ("No prompts avaiable", String.Empty);
			} else {
				Console.WriteLine ("PopulateData");
				if (Session ["CREATIVITY_PROMPTS_PAST_RISK"] != null) {
					this.total = CreativityPromptsFeed.Count < Constants.MaxPromptsAtATime ? CreativityPromptsFeed.Count : Constants.MaxPromptsAtATime;
					int counter = 0;
					for (int i = 0; i < this.total; i++) {
						GenerateHtml3 (CreativityPromptsFeed [i], String.Empty, counter++);
						Console.WriteLine (CreativityPromptsFeed [i]);
					}
				}
				else {
					Console.WriteLine ("no CREATIVITY_PROMPTS");
				}
			}
			//			Session ["CREATIVITY_PROMPTS_TOTAL"] = this.total;
			//			Session ["CREATIVITY_PROMPTS_PAST_RISK"] = CreativityPromptsFeed;
			//			GenerateAgain.Text = "MORE PROMPTS";
		}

		private void GenerateHtml3 (string main, string second, int id)
		{
			content2.Controls.Add (new LiteralControl (Tag1)); 
			content2.Controls.Add (new LiteralControl (Tag2));
			content2.Controls.Add (new LiteralControl (main));
			content2.Controls.Add (new LiteralControl (Tag3));

			content2.Controls.Add (CreateCheckboxControl (id.ToString (), main));

			content2.Controls.Add (new LiteralControl (Tag6));
			content2.Controls.Add (new LiteralControl (Tag7));
			content2.Controls.Add (new LiteralControl (Tag8));
			content2.Controls.Add (new LiteralControl (second));
			content2.Controls.Add (new LiteralControl (Tag9));
			content2.Controls.Add (new LiteralControl (Tag10));
		}
			
		public virtual void morePromptsClicked(object sender, EventArgs args)
		{
			if (Session ["CREATIVITY_PROMPTS_PAST_RISK"] != null) {
				CreativityPromptsFeed = (IList<string>) Session ["CREATIVITY_PROMPTS_PAST_RISK"];
				CreativityPromptsFeed.Shuffle ();
				PopulateData ();
			}
		}

		#endregion

		#region Risk Resolutions

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

//						RiskDescription.Text = Util.ExtractAttributeContentFromString(matchedSource.Content, "Content");

						var recommendation = Util.ExtractAttributeContentFromString2 (matchedSource.Content, "Recommendation").Trim();
						if (!recommendation.Trim().Equals (String.Empty))
							GenerateHtml2 (recommendation, String.Empty, counter++);
						//								content2.InnerHtml += GenerateHtml3 (recommendation, String.Empty, 0);

						var correctiveActions = Util.ExtractAttributeContentFromString2 (matchedSource.Content, "Corrective Actions");
						if (!correctiveActions.Trim().Equals (String.Empty))
							GenerateHtml2 (correctiveActions, String.Empty, counter++);
						//								content2.InnerHtml += GenerateHtml3 (correctiveActions, String.Empty, 0);

						string ignoreWord = "None";
						var countermeasures = Util.ExtractAttributeContentFromString2 (matchedSource.Content, "Countermeasures");
						if (!countermeasures.Equals (String.Empty) 
							&& !countermeasures.Trim().ToLower().Contains (ignoreWord.ToLower())
						)
							GenerateHtml2 (countermeasures, String.Empty, counter++);
						//								content2.InnerHtml += GenerateHtml2 (countermeasures, String.Empty, 0);

						//							var c = new CheckBox ();
						//							c.ID = "CheckBox0";
						//							c.CheckedChanged += new EventHandler (CheckBox1_CheckedChanged);
						break;

					}
				}

			}
//			Console.WriteLine ("content.Controls: " + content.Controls.Count);
		}

		private string DetermineID()
		{
			string id = String.Empty;
			if (Request.QueryString["id"] != null)
			{
				id = Request.QueryString["id"];
				Session ["CurrentOriginalID"] = id;
			}
			else
			{
				if (Session ["CurrentOriginalID"] != null)
					id = Session ["CurrentOriginalID"].ToString();
			}
			return id;
		}

		private string DetermineResponseUri()
		{
			string responseUri = String.Empty;
			if (Session ["CurrentResponseUri"] != null)
				responseUri = Session ["CurrentResponseUri"].ToString();
			else
			{
				if (Request.QueryString["path"] != null)
					responseUri = Request.QueryString["path"];
			}
			return responseUri;
		}

		private void GenerateHtml2 (string main, string second, int id)
		{
			content2.Controls.Add (new LiteralControl (Tag1)); 
			content2.Controls.Add (new LiteralControl (Tag2));
			content2.Controls.Add (new LiteralControl (main));
			content2.Controls.Add (new LiteralControl (Tag3));

//			if (categoryIDList != null && categoryIDList.Count > 0) {
//				if (categoryIDList.ContainsKey (id.ToString ())) {
//					CheckBox chk = (CheckBox)categoryIDList.GetByIndex (id);
//					content2.Controls.Add (chk);
//				} else
//					content2.Controls.Add (CreateCheckboxControl (id.ToString (), main));
//			}
//			else
			content2.Controls.Add (CreateCheckboxControl (id.ToString (), main));

			content2.Controls.Add (new LiteralControl (Tag6));
			content2.Controls.Add (new LiteralControl (Tag7));
			content2.Controls.Add (new LiteralControl (Tag8));
			content2.Controls.Add (new LiteralControl (second));
			content2.Controls.Add (new LiteralControl (Tag9));
			content2.Controls.Add (new LiteralControl (Tag10));
		}

		#endregion

		private CheckBox CreateCheckboxControl(string id, string content)
		{
			var k = new CheckBox();
			k.ID = id;
			k.AutoPostBack = false;
			k.ToolTip = content;
			//			k.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);

			return k;
		}
			


		private void GenerateHtml3 (string main, string second)
		{
			content2.Controls.Add (new LiteralControl (Tag1)); 
			content2.Controls.Add (new LiteralControl (Tag2));
			content2.Controls.Add (new LiteralControl (main));
			content2.Controls.Add (new LiteralControl (Tag3));

			content2.Controls.Add (new LiteralControl (Tag6));
			content2.Controls.Add (new LiteralControl (Tag7));
			content2.Controls.Add (new LiteralControl (Tag8));
			content2.Controls.Add (new LiteralControl (second));
			content2.Controls.Add (new LiteralControl (Tag9));
			content2.Controls.Add (new LiteralControl (Tag10));
		}

		void UpdateRisk ()
		{
			RetrieveCurrentRisk ();
			bool entryAdded = false;
//			for (int i = 0; i < content.Controls.Count; i++) {
//				var ctrl = content.Controls [i];
//				if (ctrl is CheckBox) {
//					bool result = ((CheckBox)ctrl).Checked;
//					if (result)
//					if ((!this.currentRisk.Recommendations.Contains (RetrieveCheckboxText (((CheckBox)ctrl).ClientID)))
//						&& (!((CheckBox)ctrl).ToolTip.Trim ().Equals(String.Empty))) {
//						this.currentRisk.Recommendations.Add (((CheckBox)ctrl).ToolTip.Trim ());
//						entryAdded = true;
//						Console.WriteLine ("Selected: " + ((CheckBox)ctrl).ToolTip.Trim ());
//					}
//				}
//			}
			for (int i = 0; i < content2.Controls.Count; i++) {
				var ctrl = content2.Controls [i];
				if (ctrl is CheckBox) {
					bool result = ((CheckBox)ctrl).Checked;
					if (result)
					if ((!this.currentRisk.Recommendations.Contains (RetrieveCheckboxText (((CheckBox)ctrl).ClientID)))
						&& (!((CheckBox)ctrl).ToolTip.Trim ().Equals(String.Empty))) {
						this.currentRisk.Recommendations.Add (((CheckBox)ctrl).ToolTip.Trim ());
						entryAdded = true;
						Console.WriteLine ("Selected: " + ((CheckBox)ctrl).ToolTip.Trim ());
					}
				}
			}
			this.currentRisk.Recommendations.Remove (String.Empty);

			GenerateXml ();
			if (entryAdded) {
				successMessage.InnerHtml = "Selected ideas were saved successfully.";
				alert_message_success.Visible = true;
				alert_message_error.Visible = false;

			} else {
				errorMessage.InnerHtml = "Selected ideas already exist.";
				alert_message_success.Visible = false;
				alert_message_error.Visible = true;

			}
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

		private void GenerateXml()
		{
			string Ref;
			string xmlUri;

			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = Util.CreateSolutionXml (this.currentRisk);
			Ref = Constants.CASEREF + this.sourceId + "_" + "Solution" + ".xml";
			xmlUri = Path.Combine (processPath, "Solution", Ref);
			XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri);

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
			
		public virtual void submitClicked(object sender, EventArgs args)
		{
			Console.WriteLine ("submitButtonClicked");
			if (!this.sourceId.Equals (String.Empty)) {

				UpdateRisk ();
				//				Response.Redirect("Solution_ResolutionIdeas.aspx");
			}
			if (Session ["CREATIVITY_PROMPTS_PAST_RISK"] != null) {
				CreativityPromptsFeed = (IList<string>) Session ["CREATIVITY_PROMPTS_PAST_RISK"];
//				CreativityPromptsFeed.Shuffle ();
				PopulateData ();
			}

		}

		public virtual void backClicked(object sender, EventArgs args)
		{
			Console.WriteLine ("backButtonClicked");
			if (Session ["CREATIVITY_PROMPTS_PAST_RISK"] != null)
				Session.Remove ("CREATIVITY_PROMPTS_PAST_RISK");
			Response.Redirect("CreateIdeas_PastRisks.aspx");


		}

	}
}

