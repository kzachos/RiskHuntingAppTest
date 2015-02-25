
using System;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RiskHuntingAppTest
{
	public partial class DisplayResponse_CreativeGuidance : System.Web.UI.Page
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

		const int maxPromptsAtATime = 10;
		int total;

		List<NLResponseToken> NLResponse;

		protected string Expression = "Think about ";

		protected string processPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources", "_toProcess");

		protected IList<string> CreativityPromptsFeed;
		protected string sourceId;
		protected Risk currentRisk;

		protected void Page_Init(object sender, EventArgs e)
		{
			MorePrompts.Visible = false;
			generatePrompts.Visible = false;
			if (Session ["CURRENT_RISK"] != null)
				sourceId = Session ["CURRENT_RISK"].ToString();

			if (Session ["CURRENT_RISK_DESC"] != null) 
				NLResponse = (List<NLResponseToken>) Session ["CURRENT_RISK_DESC"];
			else
				NLResponse = new List<NLResponseToken> ();

			if (!Page.IsPostBack) {
				Console.WriteLine ("Page_Init - NOT Page.IsPostBack");
				ResolutionIdeas.Visible = false;

				PrepareData ();
				PopulateData ();

			}
			else {
				Console.WriteLine ("Page_Init - Page.IsPostBack");


				content2.Controls.Clear ();
				//				NLResponse = new List<NLResponseToken> ();
				//				RetrieveNLData ();


				PrepareData ();
				PopulateData ();

				//				if (Session ["CREATIVITY_PROMPTS"] != null)
				//					CreativityPromptsFeed = (List<string>)Session ["CREATIVITY_PROMPTS"];
				//				else
				//					CreativityPromptsFeed = new List<string> ();

				//				if (CreativityPromptsFeed.Count == 0) {
				//					GenerateAgain.Text = "GENERATE AGAIN";
				//					Session.Remove ("CREATIVITY_PROMPTS");
				//					PrepareData ();
				//					}

				//				if (Session ["CREATIVITY_PROMPTS_TOTAL"] != null)
				//					this.total = (int)Session ["CREATIVITY_PROMPTS_TOTAL"];
				//				for ( int i = 0; i < this.total; i++ ) {
				//					CreativityPromptsFeed.RemoveAt (0);
				//				}
				//				PopulateData ();

			}
			Topbar_Problem_Search_Solution ();

			var processGuidanceText = Util.GenerateProcessGuidance ("creativeGuidance");
			creativeGuidance.InnerText = processGuidanceText.Equals(String.Empty)?defaultProcessGuidance:processGuidanceText;
		}

		protected void Page_Load(object sender, EventArgs e)
		{

			if (Session ["CURRENT_RISK"] != null)
				sourceId = Session ["CURRENT_RISK"].ToString();

			if (!Page.IsPostBack) {

				Console.WriteLine ("Page_Load - NOT Page.IsPostBack");

			} 
			//			else
			//				PopulateData ();
			//				if (Session ["CREATIVITY_PROMPTS"] != null)
			//					CreativityPromptsFeed = (IList<string>)Session ["CREATIVITY_PROMPTS"];
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

			string[] valuesNP = new string[4] {"water", "floor", "operators", "snow"};
			string[] valuesVP = new string[1] {"fit the tyres"};

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

				if (Session ["CREATIVITY_PROMPTS"] != null)
					CreativityPromptsFeed = (IList<string>)Session ["CREATIVITY_PROMPTS"];
				else {
					Console.WriteLine ("no CREATIVITY_PROMPTS");
				}
			}

		}

		void PopulateData ()
		{
			if (!Page.IsPostBack) {
				Console.WriteLine ("****** PopulateData *******");
				this.total = CreativityPromptsFeed.Count < maxPromptsAtATime ? CreativityPromptsFeed.Count : maxPromptsAtATime;
				if (this.total > 0) {
					int counter = 0;
					for (int i = 0; i < this.total; i++) {
						GenerateHtml3 (CreativityPromptsFeed [i], String.Empty, counter++);
						Console.WriteLine (CreativityPromptsFeed [i]);
					}
					Session ["CREATIVITY_PROMPTS"] = CreativityPromptsFeed;
				}
				else
					GenerateHtml3 ("No prompts avaiable", String.Empty);
			} else {
				Console.WriteLine ("PopulateData");
				if (Session ["CREATIVITY_PROMPTS"] != null) {
					this.total = CreativityPromptsFeed.Count < maxPromptsAtATime ? CreativityPromptsFeed.Count : maxPromptsAtATime;
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
			//			Session ["CREATIVITY_PROMPTS"] = CreativityPromptsFeed;
			//			GenerateAgain.Text = "MORE PROMPTS";
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

		public virtual void morePromptsClicked(object sender, EventArgs args)
		{
			//			RetrieveNLData ();
		}

		public virtual void submitClicked(object sender, EventArgs args)
		{
			Console.WriteLine ("submitClicked");
			if (!this.sourceId.Equals (String.Empty)) {
				UpdateRisk ();
				Response.Redirect("Solution_ResolutionIdeas.aspx");
			}
			ResolutionIdeas.Visible = true;


		}

		//		private void RetrieveNLData()
		//		{
		//			var watch = Stopwatch.StartNew();
		//
		//			string currentRiskDesc;
		//			if (Session ["CURRENT_RISK_DESC"] != null) {
		//				currentRiskDesc = Session ["CURRENT_RISK_DESC"].ToString ();
		//
		//				try {
		//					RiskHuntingApp.antique.AntiqueService antique = new RiskHuntingApp.antique.AntiqueService ();
		//					antique.NLParserCompleted += new RiskHuntingApp.antique.NLParserCompletedEventHandler (objAntique_NLParserCompleted);
		//					antique.NLParserAsync (currentRiskDesc);
		//				}
		//				finally {
		//
		//
		//					watch.Stop ();
		//					// Get the elapsed time as a TimeSpan value.
		//					TimeSpan ts = watch.Elapsed;
		//					// Format and display the TimeSpan value.
		//					string elapsedTime = String.Format ("{0:00}:{1:00}:{2:00}.{3:00}",
		//						                    ts.Hours, ts.Minutes, ts.Seconds,
		//						                    ts.Milliseconds / 10);
		//					Console.WriteLine (elapsedTime, "RunTime");
		//				}
		//			}
		//			else
		//				Console.WriteLine ("No description!");
		//		}

		//		void objAntique_NLParserCompleted(object sender, 
		//			RiskHuntingApp.antique.NLParserCompletedEventArgs e)
		//		{
		//			Console.WriteLine (e.Result);
		//			NLResponse = Util.DeserializeNLResponse (e.Result);
		//
		//			foreach(var item in NLResponse)
		//			{
		//				Console.WriteLine("id: {0}, name: {1}",item.Pos,item.TermValue);
		//			}
		//
		//
		//		}

		void DoSomeLongJob()
		{
			System.Threading.Thread.Sleep(5000);
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
						Console.WriteLine ("Selected: " + ((CheckBox)ctrl).ToolTip.Trim ());
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

		public virtual void resolutionIdeasClicked(object sender, EventArgs args)
		{
			Response.Redirect("Solution_ResolutionIdeas.aspx");
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
	}
}

