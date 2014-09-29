﻿
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using World.Code.WebControls;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

//using ActivityTrackingLog.Utils;

namespace RiskHuntingAppTest
{


	public partial class Default : Page
	{
		protected const string SOURCESPECIFICATION = "SourceSpecification";
		protected const string PROBLEM = "Problem";
		protected const string SOLUTION = "Solution";
		protected const string ADDITIONAL = "Additional";
		protected const string PROCESSFOLDER = "_toProcess";

		protected const string SOURCE_TYPE = Constants.CASEREF;
		protected const string CASE_TYPE = "Risk";

//		protected const string AUTHOR = "Bassildon Trial";

		protected string xmlFilesPath = SettingsTool.GetApplicationPath() + "xmlFiles/";
		protected string requestPath = SettingsTool.GetApplicationPath() + "xmlFiles/Requests/";
		protected string responsePath = SettingsTool.GetApplicationPath() + "xmlFiles/Responses/";
		protected string sourcesPath = SettingsTool.GetApplicationPath() + "xmlFiles/Sources/";
		protected string processPath = SettingsTool.GetApplicationPath() + "xmlFiles/Sources/_toProcess/";

		protected string requestId, responseUri;

		protected int maxId;
		protected string sourceId;
		protected Risk currentRisk;

		const string SpanStartTagMenu = "<span class=menu>";
		const string SpanStartTagName = "<span class=name>";
		const string SpanStartTagArrow = "<span class=arrow>";
		const string SpanEndTag = "</span>";

		const string LiStartTagMenu = "<li class=menu>";
		const string LiEndTag = "</li>";

		protected const string DESC_WATERMARK = "[Enter a description of the risk - Max 120 words]";
		protected const string NAME_WATERMARK = "[Enter a name for the risk]";
		protected const string AUTHOR_WATERMARK = "[Enter the name of person reporting the risk]";
		protected const string LOCATION_WATERMARK = "[Enter the risk location]";
		protected const string BODYPARTS_WATERMARK = "[Enter the body parts that are at risk, e.g. feet]";

		//		const string StartTagNav = "<div id=duobutton>";
		//		const string Mid1TagNav = "<div class=links>";
		//		const string Mid2TagNav = "<a id=\"pressed\" href=\"#\">Risk description</a><a href=\"javascript:doLoad('ResolutionIdeas.aspx');\">Risk resolutions</a>";
		//		const string EndTagNav = "</div>";

		protected void Page_Init(object sender, EventArgs e)
		{
			Session.Remove ("CURRENT_PROBLEM_DESC");
			Session.Remove ("CREATIVITY_PROMPTS");

			if (!Page.IsPostBack) {
				Console.WriteLine ("Page_Init - NOT Page.IsPostBack");

				if (Session ["CURRENT_RISK"] != null)
					sourceId = Session ["CURRENT_RISK"].ToString ();
					
				this.responseUri = DetermineResponseUri ();

				this.requestId = DetermineID ();

				if (!this.requestId.Equals (String.Empty)) {
					Topbar_Problem_Search_Solution ();
//					previousrisks.Visible = true;
					//					navigationbar.Visible = true;

					//					PopulateRiskDetails(this.requestId);
					this.sourceId = this.requestId;
					RetrieveRiskXml ("SourceSpecification");
					RetrieveRiskXml ("Problem");
					RetrieveRiskXml ("Solution");
					Console.WriteLine (this.requestId);
					Session ["CURRENT_RISK"] = this.sourceId;
				} else {
					Topbar_Problem_Solution ();
//					previousrisks.Visible = false;
					//					navigationbar.Visible = false;
				}

				//queryText.Text = "temp";
				//				InitDropDownLists();
				EnableAutoComplete ();

				string script = "$(document).ready(function () { $('[id*=submitClicked]').click(); });";
				ClientScript.RegisterStartupScript (this.GetType (), "load", script, true);
			} else {
				Console.WriteLine ("Page_Init - Page.IsPostBack");

				if (Session ["CURRENT_RISK"] != null) {
					sourceId = Session ["CURRENT_RISK"].ToString ();
				}
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			//			string username = CASP.Authenticate("https://login.case.edu/cas/", this.Page);
			//do whatever with username
			//			Console.WriteLine (username);

			if (!Page.IsPostBack) {
				Console.WriteLine ("Page_Load - NOT Page.IsPostBack");

			} else {
				Console.WriteLine ("Page_Load - Page.IsPostBack");

			}
			PopulateElements();
			PopulateParametersDropDown ();
		}


		private string DetermineResponseUri()
		{
			string responseUri = String.Empty;
			if (Session["CurrentResponseUri"] != null)
				responseUri = Session["CurrentResponseUri"].ToString();
			else
			{
				if (Request.QueryString["path"] != null)
				{
					responseUri = Request.QueryString["path"];
					Session["CurrentResponseUri"] = responseUri;
				}
			}
			return responseUri;
		}

		private string DetermineID()
		{
			//			string id = String.Empty;
			//			if (Session["CurrentID"] != null)
			//				id = Session["CurrentID"].ToString();
			//			else
			//			{
			//				if (Request.QueryString["id"] != null)
			//				{
			//					id = Request.QueryString["id"];
			//					Session["CurrentID"] = id;
			//				}
			//			}
			//			return id;

			string id = String.Empty;
			if (Request.QueryString["id"] != null)
			{
				id = Request.QueryString["id"];
			}
			else if (Session ["CURRENT_RISK"] != null)
			{
				id = Session ["CURRENT_RISK"].ToString ();
			}
			return id;
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

			RiskDescription.WatermarkText = DESC_WATERMARK;
			RiskName.WatermarkText = NAME_WATERMARK;
			RiskAuthor.WatermarkText = AUTHOR_WATERMARK;
//			RiskLocation.WatermarkText = LOCATION_WATERMARK;
//			RiskBodyParts.WatermarkText = BODYPARTS_WATERMARK;
		}

		private void PopulateParametersDropDown()
		{
			var doc = XDocument.Load(xmlFilesPath + "Parameters.xml", LoadOptions.None); 
			RiskLocation.Items.Clear();
			if (doc.Descendants("rl").Count() > 0)
				foreach (XElement xe in doc.Descendants("rl"))
					RiskLocation.Items.Add(new ListItem(xe.Element("n").Value));    
//			else
//				RiskLocation.Items.Add(new ListItem("None")); 
			if (doc.Descendants("bp").Count() > 0)
				foreach (XElement xe in doc.Descendants("bp"))
					RiskBodyParts.Items.Add(new ListItem(xe.Element("n").Value));    
		}

		#endregion

		private bool CheckTextBox()
		{
			if (RiskDescription.Text.Equals(String.Empty) || RiskDescription.Text.Equals(DESC_WATERMARK))
			{
				errorMsg.InnerHtml = "<span class=\"redtitle\">please enter a descriptoin of the risk encountered</span>";
				return false;
			}
			else
			{
				errorMsg.InnerHtml = String.Empty;
				return true;
			}
		}

		//		private string CheckTextBoxContent(WatermarkedTextBox tb, string watermarkText)
		//		{
		//			Console.WriteLine (watermarkText + ": " + tb.Text);
		//			if (tb.Text.Equals(watermarkText))
		//				if (!watermarkText.Equals (NAME_WATERMARK))
		//					return String.Empty;
		//				else
		//					return RiskDescription.Text.ExtractKeywords ().TruncateAtWord (10);
		//			else
		//				return tb.Text;
		//		}

		private string CheckTextBoxContent(WatermarkedTextBox tb, string watermarkText)
		{
			if (tb.Text.Equals (String.Empty)) {
				if (!watermarkText.Equals (NAME_WATERMARK))
					return String.Empty;
				else
					return RiskDescription.Text.ExtractKeywords ().TruncateAtWord (10);
			}
			else
				return tb.Text;
		}
			

		private int RandomNumber(int min, int max)
		{
			Random random = new Random();
			return random.Next(min, max);
		}

		private int GetNewRequestId()
		{
			int maxId = 0;
			string requestPath = FindLatestRequestPath();
			if (!requestPath.Equals(String.Empty))
			{
				if (File.Exists(requestPath))
				{
					XmlProc.RequestSerialized.Request request = XmlProc.ObjectXMLSerializer<XmlProc.RequestSerialized.Request>.Load(requestPath);
					maxId = Convert.ToInt32(request.requestId);
					maxId = maxId + 1;
				}
			}
			else
				maxId = 1;
			return maxId;
		}

		private string FindLatestRequestPath()
		{
			string path = String.Empty;
			// get your files (names)
			string[] fileNames = Directory.GetFiles(requestPath, "*.*");

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
			

		private void EnableAutoComplete()
		{
			//DBSelect dbs = new DBSelect();
			//int currentSysID = dbs.GetSystemID(Session["SystemName"].ToString());
			//int currentUCID = GetUseCaseID();
			//int currentDomainID = GetDomainID();
			//ArrayList autoComplTermsActors = GetActorsForAutoCompl(currentUCID);
			//ArrayList autoComplTermsGlossary = GetGlossaryTermsForAutoCompl(currentDomainID, currentSysID, currentUCID);
			string test = "koccos";
			string Script = "";
			Script += "<script language=javascript src=Theme/javascript/actb.js></script>\n";
			Script += "<script language=javascript src=Theme/javascript/common.js></script>\n";
			Script += "<script language=JavaScript id='AutoComplete'>\n";
			//Script += "var customarray=new Array('automobile ','auto ','car ','vehicle ','" + test + "','kingbolt', 'kingcraft','kingcup','kingdom','kingfisher','kingpin');";
			Script += "var customarray=new Array('Abdominal aneurysm ','Abductor spasmodic dysphonia ','ABO blood group ','Arabidopsis thaliana genome ','" + test + "','Bacterial artificial chromosome ', 'Ball-and-socket joint ','Deafness and keratopachydermia ','Deafness-ichthyosis-keratitis syndrome ','Facultative heterochromatin ','Factitious disorder by proxy ');";
			//Script += Environment.NewLine; 
			//Script += "var customarray=new Array(";
			//for (int i = 0; i < autoComplTermsGlossary.Count; i++)
			//{
			//    Script += "'" + autoComplTermsGlossary[i].ToString() + " ',";
			//}
			//Script += "'');";
			Script += Environment.NewLine;
			Script += "var obj1 = actb(document.getElementById('queryText'),customarray);";
			//Script += Environment.NewLine;
			//Script += "var obj2 = actb(document.getElementById('Source'),customarray);";
			//Script += Environment.NewLine;
			//Script += "var obj3 = actb(document.getElementById('AddedValue'),customarray);";
			//Script += Environment.NewLine;
			//Script += "var obj4 = actb(document.getElementById('Justification'),customarray);";
			Script += "\n</script>";

			//Check wether they are already  registered
			if (!this.IsStartupScriptRegistered("AutoComplete"))
			{
				//Register the script
				this.RegisterStartupScript("AutoComplete", Script);
			}
		}

		private void CreateRisk (bool search)
		{
			if (search) {
				this.maxId = GetNewSourceId ();
				this.sourceId = GetNewSourceId ().ToString ();
				Session ["CURRENT_RISK"] = this.sourceId;
				this.currentRisk = NewRisk (search);
			} else {
				if (Session ["CURRENT_RISK"] == null) {
					this.maxId = GetNewSourceId ();
					this.sourceId = GetNewSourceId ().ToString ();
					Session ["CURRENT_RISK"] = this.sourceId;
					this.currentRisk = NewRisk (search);
				} else {
					this.sourceId = Session ["CURRENT_RISK"].ToString();
					Console.WriteLine ("this.sourceId (CreateRisk): " + this.sourceId.ToString ());
					RetrieveCurrentRisk ();
					UpdateRisk (search);

				}
			}


			Console.WriteLine ("this.currentRisk.State (CreateRisk): " + this.currentRisk.State.ToString ());
			Console.WriteLine ("this.sourceId (CreateRisk): " + this.sourceId.ToString ());

//			string responseXml = PerformEddie();
//
//			string responseRef = "Response_" + this.requestId + ".xml";
//			//string responseRef = "Response_201131_153042.xml";
//			string responseXmlUri = responsePath + responseRef;
//			FileStream responseStream = new FileStream(responseXmlUri, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
//			StreamWriter responseStreamWriter = new StreamWriter(responseStream, Encoding.ASCII);
//			responseStreamWriter.Write(responseXml);
//			responseStreamWriter.Flush();
//			responseStreamWriter.Close();
//			responseStream.Close();


			GenerateXml("SourceSpecification");
			GenerateXml("Problem");
			GenerateXml("Solution");

//			this.responseUri = responseXmlUri;
//			Session["CurrentResponseUri"] = responseXmlUri;
//			Response.Redirect("SearchResults.aspx");
		}

		private void FindRisks ()
		{
			string responseXml = PerformEddie();

			string responseRef = "Response_" + this.requestId + ".xml";
			//string responseRef = "Response_201131_153042.xml";
			string responseXmlUri = responsePath + responseRef;
			FileStream responseStream = new FileStream(responseXmlUri, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
			StreamWriter responseStreamWriter = new StreamWriter(responseStream, Encoding.ASCII);
			responseStreamWriter.Write(responseXml);
			responseStreamWriter.Flush();
			responseStreamWriter.Close();
			responseStream.Close();

			this.responseUri = responseXmlUri;
			Session["CurrentResponseUri"] = responseXmlUri;
			Response.Redirect("SearchResults.aspx");
		}

		private Risk NewRisk (bool search)
		{
			Risk risk = new Risk ();
			risk.Id = Convert.ToInt32 (this.sourceId);

			risk.Content = RiskDescription.Text;
			risk.Author = CheckTextBoxContent (RiskAuthor, AUTHOR_WATERMARK);
			risk.Name = CheckTextBoxContent(RiskName, NAME_WATERMARK);
			//			risk.InjuryNature = CheckTextBoxContent(RiskDanger, DANGER_WATERMARK);
			risk.InjuryNature = String.Empty;
			risk.LocationDetail = RiskLocation.Value; 
			risk.BodyPart = RiskBodyParts.Value;
		
			risk.Recommendations = new ArrayList ();
			risk.Countermeasures = String.Empty;
			risk.CorrectiveActions = String.Empty;
			risk.Recommendation = String.Empty;
			risk.Actions = new List<Action> ();

			risk.Miscellaneous = String.Empty;
			risk.State = RiskQueryState.ProblemDescribed;
			if (search)
				risk.SimilarCasesFound = true;
			else
				risk.SimilarCasesFound = false;

			return risk;
		}

		private void UpdateRisk (bool search)
		{
			Console.WriteLine ("this.currentRisk.Id (UpdateRisk): " + this.currentRisk.Id.ToString ());
			Console.WriteLine ("this.sourceId (UpdateRisk): " + this.sourceId.ToString ());
			this.currentRisk.Content = RiskDescription.Text;
			this.currentRisk.Name = CheckTextBoxContent(RiskName, NAME_WATERMARK);
			//			risk.InjuryNature = CheckTextBoxContent(RiskDanger, DANGER_WATERMARK);
			this.currentRisk.InjuryNature = String.Empty;
			this.currentRisk.LocationDetail = RiskLocation.Value;
			this.currentRisk.BodyPart = RiskBodyParts.Value;
			if (search)
				this.currentRisk.SimilarCasesFound = true;
			else
				this.currentRisk.SimilarCasesFound = false;
		}

		public virtual void submitClicked(object sender, EventArgs args)
		{
			if (CheckTextBox() == true)
			{
				try 
				{

//					if (Session ["CURRENT_RISK"] == null)
//					{
						CreateRisk (true);
//					}
//					else
//						UpdateRisk ();
					FindRisks ();
				}
				catch(Exception ex)
				{
					Console.WriteLine (ex.ToString ());
				}
			}
			//			else
			//			{
			//				errorMsg.InnerHtml = "<span class=\"redtitle\">please enter a descriptoin of the risk encountered</span>";
			//			}

		}


		private string PerformEddie()
		{
			XmlProc.RequestSerialized.Request request = CreateEddieRequestWithNewRequestId(xmlFilesPath + "EddieRequest_template.xml");

			string requestRef = "Request_" + this.requestId + ".xml";
			string requestXmlUri = requestPath + requestRef;

			XmlProc.ObjectXMLSerializer<XmlProc.RequestSerialized.Request>.Save(request, requestXmlUri);

			FileStream cgStream1 = new FileStream(requestXmlUri, FileMode.Open, FileAccess.Read);
			StreamReader cgStreamReader1 = new StreamReader(cgStream1);
			string requestXml = cgStreamReader1.ReadToEnd();
			cgStreamReader1.Close();

			//errorLabel.Text = requestXml;

			var watch = Stopwatch.StartNew();

			RiskHuntingAppTest.eddie.EDDiEWebService eddie = new RiskHuntingAppTest.eddie.EDDiEWebService();
			eddie.Timeout = 3000000;
			string eddieResponseXml = eddie.PerformEddieDomain(requestXml, "Risk");

			watch.Stop();
			// Get the elapsed time as a TimeSpan value.
			TimeSpan ts = watch.Elapsed;
			// Format and display the TimeSpan value.
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
				ts.Hours, ts.Minutes, ts.Seconds,
				ts.Milliseconds / 10);
			Console.WriteLine(elapsedTime, "RunTime");

			return eddieResponseXml;


		}

		private XmlProc.RequestSerialized.Request CreateEddieRequestWithNewRequestId(string requestUri)
		{
			XmlProc.RequestSerialized.Request requestNew = new XmlProc.RequestSerialized.Request();
			//			int maxId = GetNewRequestId();
			int maxId = GetNewSourceId (); 

			XmlProc.RequestSerialized.Request request = XmlProc.ObjectXMLSerializer<XmlProc.RequestSerialized.Request>.Load(requestUri);
			List<XmlProc.RequestSerialized.RegistryComponent> regList = request.Registries;
			XmlProc.RequestSerialized.RequestProperties propList = request.Properties;
			//				XmlProc.RequestSerialized.PartOfSpeechComponent pos = request.Properties.PartOfSpeech;
			//				XmlProc.RequestSerialized.TermExpansionComponent exp = request.Properties.TermExpansion;
			XmlProc.RequestSerialized.SimilarityType similarityType = request.SimilarityType;
			XmlProc.RequestSerialized.RequestDataSource dst = request.DataSource[0];
			XmlProc.RequestSerialized.RequestTarget target = (XmlProc.RequestSerialized.RequestTarget)request.Target[0];
			target.TargetDescription = GenerateTargetDescription();

			DateTime now = DateTime.Now;
			requestNew.DateTime = now.ToString();
			requestNew.Properties = propList;
			requestNew.Registries = regList;
			requestNew.requestId = maxId.ToString();
			this.requestId = this.sourceId;
			requestNew.SimilarityType = similarityType;
			requestNew.DataSource.Add(dst);
			requestNew.Target.Add(target);

			return requestNew;
		}


		private XmlProc.RequestSerialized.Request CreateAntiqueRequestWithNewRequestId(string requestUri)
		{
			XmlProc.RequestSerialized.Request requestNew = new XmlProc.RequestSerialized.Request();
			int maxId = GetNewRequestId();

			XmlProc.RequestSerialized.Request request = XmlProc.ObjectXMLSerializer<XmlProc.RequestSerialized.Request>.Load(requestUri);
			XmlProc.RequestSerialized.SimilarityType similarityType = request.SimilarityType;
			XmlProc.RequestSerialized.RequestDataSource dst = request.DataSource[0];
			XmlProc.RequestSerialized.RequestTarget target = (XmlProc.RequestSerialized.RequestTarget)request.Target[0];
			target.TargetDescription = RiskDescription.Text;
			//target.TargetDescription = "Patient bullies other patients and the patient fights other patients. The other patients avoid the patient. They don't like talking to him for fear that it will lead to a fight. Some patients are terrified of the patient.";

			DateTime now = DateTime.Now;
			requestNew.DateTime = now.ToString();
			requestNew.requestId = maxId.ToString();
			this.requestId = maxId.ToString();
			requestNew.SimilarityType = similarityType;
			requestNew.DataSource.Add(dst);
			requestNew.Target.Add(target);

			return requestNew;
		}



		public virtual void risksClicked(object sender, EventArgs args)
		{
			Response.Redirect("SearchResults.aspx");
		}

		protected void Timer1_Tick(object sender, EventArgs e)
		{
			if (!CheckTextBoxContent(RiskName, NAME_WATERMARK).Equals(String.Empty) && !CheckTextBoxContent(RiskDescription, DESC_WATERMARK).Equals(String.Empty))
				Save();
		}

		private void Save()
		{
			//Add the save function here ex store the text to DB
			//Here we only move the between two textboxes to show that it works
			if (Session ["CURRENT_RISK"] == null) {
				Console.WriteLine ("new");
				AutoSaveLabel.Text = "Last Auto-Saved and created on " + DateTime.Now;
			} else {
				Console.WriteLine ("update");
				AutoSaveLabel.Text = "Last Auto-Saved and updated on " + DateTime.Now;
			}
			CreateRisk (false);
		}
			


		#region Generate and Extract content 

		private string GenerateTargetDescription ()
		{
			string content = String.Empty;
			content += "[InjuryNature]: " + String.Empty;
			content += " [LocationDetail]: " + RiskLocation.Value;
			content += " [Content]: " + RiskDescription.Text;
			content += " [BodyPart]: " + RiskBodyParts.Value;
			return content;

		}

		private string ExtractAttributeContentFromString (string text, string attribute)
		{
			//Regex regex = new Regex(@"\[ProblemDescription\]\:\s*(((?!\[SolutionDescription\]:|\[).)+)\s*\[");
			Regex regex = new Regex(@"\[" + attribute + @"\]\:([\s\S]*?)\[");
			string extractedText = null;
			Match match = regex.Match(text);
			if(match.Success)
			{
				extractedText = match.Groups[1].Value;
			}	
			return extractedText;
		}

		private string ExtractLastAttributeFromString(string text)
		{
			char[] deliminator = new char[] { ']' };
			NLP.StringProc str = new NLP.StringProc();
			SortedList stringParts = str.SeperateStringByChar(text, deliminator);
			string extractedText = stringParts[stringParts.Count - 1].ToString();
			extractedText = extractedText.Replace(":", String.Empty);

			return extractedText;
		}

		#endregion

		#region Html related
		//		string GenerateNavigationBarHtml()
		//		{
		//			return StartTagNav + Mid1TagNav + Mid2TagNav + EndTagNav + EndTagNav;
		//		}


		#endregion


		#region Xml generation

		//		private void PopulateRiskDetails (string id)
		//		{
		//			string temp = String.Empty;
		//			XmlProc.RequestSerialized.Request request = XmlProc.ObjectXMLSerializer<XmlProc.RequestSerialized.Request>.Load(requestPath + "Request_" + id + ".xml");
		//			XmlProc.RequestSerialized.RequestTarget target = (XmlProc.RequestSerialized.RequestTarget)request.Target[0];
		//			RiskName.Text = temp;
		//			RiskDanger.Text = ExtractAttributeContentFromString(target.TargetDescription, "InjuryNature").Trim();
		//			RiskLocation.Text = ExtractAttributeContentFromString(target.TargetDescription, "LocationDetail").Trim();
		//			//			RiskRisk.Text = temp;
		//			RiskDescription.Text = ExtractAttributeContentFromString(target.TargetDescription, "Content").Trim();
		//			RiskBodyParts.Text = ExtractLastAttributeFromString(target.TargetDescription).Trim();
		//			title.InnerHtml = String.Empty;
		//
		//		}

		void RetrieveCurrentRisk ()
		{
			string location = String.Empty;

			location = processPath + "SourceSpecification" + "/" + Constants.CASEREF + this.sourceId + "_" + "SourceSpecification" + ".xml";
			XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(location);

			location = processPath + "Problem" + "/" + Constants.CASEREF + this.sourceId + "_" + "Problem" + ".xml";
			XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Load(location);

			location = processPath + "Solution" + "/" + Constants.CASEREF + this.sourceId + "_" + "Solution" + ".xml";
			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Load(location);

//			this.currentRisk = new Risk ();
//			this.currentRisk.Id = ss.SourceId;
//			this.currentRisk.Name = ss.SourceName;
//			this.currentRisk.Author = ss.Facet [0].Author;
//			this.currentRisk.Content = problem.FacetSpecificationData.Content;

			this.currentRisk = new Risk (ss, problem, solution);
			Console.WriteLine ("this.currentRisk: " + this.currentRisk.Id.ToString ());
		}

		void RetrieveRiskXml ()
		{
//			title.InnerHtml = String.Empty;
			string location = String.Empty;

			location = processPath + "SourceSpecification" + "/" + Constants.CASEREF + this.sourceId + "_" + "SourceSpecification" + ".xml";
			XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(location);

			location = processPath + "Problem" + "/" + Constants.CASEREF + this.sourceId + "_" + "Problem" + ".xml";
			XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Load(location);

			location = processPath + "Solution" + "/" + Constants.CASEREF + this.sourceId + "_" + "Solution" + ".xml";
			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Load(location);

			this.currentRisk = new Risk (ss, problem, solution);

			RiskName.Text = this.currentRisk.Name;
			RiskDescription.Text = this.currentRisk.Content;
			RiskAuthor.Text = this.currentRisk.Author;
//			RiskLocation.Text = String.Empty;
//			RiskBodyParts.Text = String.Empty;

		}

		void RetrieveRiskXml (string componentType)
		{
//			title.InnerHtml = String.Empty;
			string location = String.Empty;
			if (componentType.Equals("SourceSpecification"))
			{
				location = processPath + componentType + "/" + Constants.CASEREF + this.sourceId + "_" + componentType + ".xml";
				XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(location);
				RiskName.Text = ss.SourceName;
			}
			else if (componentType.Equals("Problem"))
			{
				location = processPath + componentType + "/" + Constants.CASEREF + this.sourceId + "_" + componentType + ".xml";
				XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Load(location);
				XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData problemFacet = problem.FacetSpecificationData;
				RiskDescription.Text = problemFacet.Content;
				RiskAuthor.Text = problem.Author;
//				RiskLocation.Text = String.Empty;
//				RiskBodyParts.Text = String.Empty;
			}
			else if (componentType.Equals("Solution"))
			{
				location = processPath + componentType + "/" + Constants.CASEREF + this.sourceId + "_" + componentType + ".xml";
				XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Load(location);
				XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData solutionFacet = solution.FacetSpecificationData;
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

		private XmlProc.SourceSpecificationSerialized.SourceSpecification CreateSourceSpecificationXml()
		{
			XmlProc.SourceSpecificationSerialized.SourceSpecification ss = new XmlProc.SourceSpecificationSerialized.SourceSpecification();

			DateTime now = DateTime.Now;
			ss.SourceId = Convert.ToInt32(this.sourceId);
//			this.sourceId = maxId.ToString();
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
			problem.Author = CheckTextBoxContent(RiskAuthor, AUTHOR_WATERMARK);
			problem.FacetSpecification = fspecProblem;

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet solution = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet();
			solution.FacetType = "Solution";
			solution.Author = CheckTextBoxContent(RiskAuthor, AUTHOR_WATERMARK);
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
			problem.Author = CheckTextBoxContent(RiskAuthor, AUTHOR_WATERMARK);
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
			solution.Author = CheckTextBoxContent(RiskAuthor, AUTHOR_WATERMARK);
			solution.LaunchDate = now.ToString();
			solution.SourceSpecificationLastEdited = now.ToString();

			XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData fsd = new XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData();

			string completeSolutionString;
			if (currentRisk.Recommendation.Equals (String.Empty) && currentRisk.CorrectiveActions.Equals (String.Empty) && currentRisk.Countermeasures.Equals (String.Empty))
				completeSolutionString = String.Empty;
			else
				completeSolutionString = currentRisk.Recommendation + ";" + currentRisk.CorrectiveActions + ";" + currentRisk.Countermeasures;

			fsd.Content = completeSolutionString;
			fsd.Miscellaneous = String.Empty;

			solution.FacetSpecificationData = fsd;


			return solution;
		}


//		private XmlProc.SourceSpecificationSerialized.SourceSpecification CreateSourceSpecificationXml()
//		{
//			XmlProc.SourceSpecificationSerialized.SourceSpecification ss = new XmlProc.SourceSpecificationSerialized.SourceSpecification();
//
//			DateTime now = DateTime.Now;
//			ss.SourceId = maxId;
//			this.sourceId = maxId.ToString();
//			ss.SourceName = CheckTextBoxContent(RiskName, NAME_WATERMARK);
//			ss.SourceType = SOURCE_TYPE;
//			ss.Domain = CASE_TYPE;
//			ss.Filename = ss.SourceType + maxId + ".docx";
//			ss.LaunchDate = now.ToString();
//			ss.SourceSpecificationLastEdited = now.ToString();
//
//			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification fspecProblem = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification();
//			fspecProblem.FacetSpecificationLanguage = "Text";
//			fspecProblem.FacetSpecificationLink = ss.SourceType + maxId + "_Problem.xml";
//
//			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification fspecSolution = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification();
//			fspecSolution.FacetSpecificationLanguage = "Text";
//			fspecSolution.FacetSpecificationLink = ss.SourceType + maxId + "_Solution.xml";
//
//			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet problem = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet();
//			problem.FacetType = "Problem";
//			problem.Author = CheckTextBoxContent(RiskAuthor, AUTHOR_WATERMARK);
//			problem.FacetSpecification = fspecProblem;
//
//			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet solution = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet();
//			solution.FacetType = "Solution";
//			solution.Author = CheckTextBoxContent(RiskAuthor, AUTHOR_WATERMARK);
//			solution.FacetSpecification = fspecSolution;
//
//			ss.Facet = new System.Collections.Generic.List<XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet>();
//			ss.Facet.Add(problem);
//			ss.Facet.Add(solution);
//
//			return ss;
//		}
//
//		private XmlProc.ProblemSerialized.LanguageSpecificSpecification CreateProblemXml()
//		{
//			XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = new XmlProc.ProblemSerialized.LanguageSpecificSpecification();
//
//			DateTime now = DateTime.Now;
//			problem.FacetType = "Problem";
//			problem.FacetSpecificationLanguage = "Text";
//			problem.Author = CheckTextBoxContent(RiskAuthor, AUTHOR_WATERMARK);
//			problem.LaunchDate = now.ToString();
//			problem.SourceSpecificationLastEdited = now.ToString();
//
//			XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData fsd = new XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData();
//			fsd.Content = currentRisk.Content;
//			//fsd.Observations = new System.Collections.Generic.List<XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation>();
//			//XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation obs = new XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation();
//			//obs.id = 1;
//			//obs.launchDate = now.ToString();
//			//obs.Value = "n/A";
//			//fsd.Observations.Add(obs);
//			fsd.Observations = String.Empty;
//			fsd.ObservedBehaviour = String.Empty;
//			fsd.TreatmentType = String.Empty;
//			fsd.DateOfIncident = String.Empty;
//			fsd.AilmentType = String.Empty;
//			fsd.TriggeringEvent = String.Empty;
//			fsd.Miscellaneous = String.Empty;
//			fsd.MatchingDetails = String.Empty;
//
//			problem.FacetSpecificationData = fsd;
//
//
//			return problem;
//		}
//
//		private XmlProc.SolutionSerialized.LanguageSpecificSpecification CreateSolutionXml()
//		{
//			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = new XmlProc.SolutionSerialized.LanguageSpecificSpecification();
//
//			DateTime now = DateTime.Now;
//			solution.FacetType = "Solution";
//			solution.FacetSpecificationLanguage = "Text";
//			solution.Author = CheckTextBoxContent(RiskAuthor, AUTHOR_WATERMARK);
//			solution.LaunchDate = now.ToString();
//			solution.SourceSpecificationLastEdited = now.ToString();
//
//			XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData fsd = new XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData();
//
//			//			string completeSolutionString = currentRisk.Recommendation + ";" + currentRisk.CorrectiveActions + ";" + currentRisk.Countermeasures;
//			fsd.Content = String.Empty;
//			fsd.Miscellaneous = String.Empty;
//
//			solution.FacetSpecificationData = fsd;
//
//
//			return solution;
//		}

		private int GetNewSourceId()
		{
			int maxId = 0;
//			if (Session ["CURRENT_RISK"] == null) {
//				string path = FindLatestSourcePath ();
			string path = GetFilesFromDirectorySortedList4 (sourcesPath + CASE_TYPE + "/" + SOURCESPECIFICATION + "/");
			Console.WriteLine ("path: " + path);
				if (!path.Equals (String.Empty)) {
					if (File.Exists (path)) {
						XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load (path);
						maxId = Convert.ToInt32 (ss.SourceId);
						maxId = maxId + 1;
					}
				} else
					maxId = 1;
//			} else
//				maxId = Convert.ToInt32(this.sourceId);
			return maxId;
		}

		private string FindLatestSourcePath()
		{
			string path = String.Empty;
			// get your files (names)
			string[] fileNames = Directory.GetFiles(sourcesPath + CASE_TYPE + "/" + SOURCESPECIFICATION + "/", "*.*");
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

		private string GetFilesFromDirectorySortedList4(string dirPath)
		{
			SortedList all = new SortedList ();
			DirectoryInfo dir = new DirectoryInfo(dirPath);
			//			FileInfo[] FileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
			FileInfo[] FileList = dir.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();
			//Array.Reverse(FileList);
			NLP.StringProc str = new NLP.StringProc();
			char[] deliminator = new char[] { '_' };
			SortedList fileNameParts = new SortedList();

			for (int i = 0; i <= FileList.Length-1; i++)
			{
				FileInfo FI = FileList[i];
				//string file = files[i];
				fileNameParts = str.SeperateStringByChar(FI.FullName, deliminator);
				all.Add(fileNameParts[1].ToString(), FI.FullName);
//				Console.WriteLine ("FI.FullName: " + FI.FullName);
			}

			var keyCollection = all.Keys;
			String[] allKeys = new String[all.Count];
			keyCollection.CopyTo(allKeys, 0);
			ICollection valueCollection = all.Values;
			String[] allValues = new String[all.Count];
			valueCollection.CopyTo(allValues, 0);
//			Console.WriteLine ("allKeys: " + allKeys[all.Count-1]);
//			Console.WriteLine ("allValues: " + allValues[0]);

			return allValues[all.Count-1].ToString();
		}
		#endregion

	}
}

