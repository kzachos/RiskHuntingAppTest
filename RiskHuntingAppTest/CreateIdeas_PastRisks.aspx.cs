using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RiskHuntingAppTest
{
	
	public partial class CreateIdeas_PastRisks : System.Web.UI.Page
	{
		const string Tag1 = "<li class=\"store\">";
		const string Tag2 = "<a class=\"noeffect\" href=\"javascript:doLoad('CreateIdeas_PastRisk.aspx?id=";
		const string Tag3 = "');\">";
		const string Tag4 = "<span class=\"name\">";
		const string Tag5 = "</span>";
		const string Tag6 = "<span class=\"comment\">";
		const string Tag7 = "</span>";
		const string Tag8 = "<span class=\"starcomment\">";
		const string Tag9 = "</span>";
		const string Tag10 = "<span class=\"arrow\"></span>";
		const string Tag11 = "</a>";
		const string Tag12 = "</li>";
		const string DATEANDLOCATION = "March 15th 2013, Basilldon (UK)";

		const string defaultProcessGuidance = "Select a risk resolution similar to your risk. Browse each short description below";

		protected string xmlFilesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles");
		protected string requestPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Requests");
		protected string sourcesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources");
		protected string responsePath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Responses");
		protected string processPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources", "_toProcess");

		protected string requestId, sourceId, responseUri;

		protected Risk currentRisk;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session ["CURRENT_PAST_RISK_DESC"] != null) 
				Session.Remove ("CURRENT_PAST_RISK_DESC");
			if (!Page.IsPostBack) {
				Console.WriteLine ("Page_Load - NOT Page.IsPostBack");
				creativeGuidance.Visible = false;
				creativeGuidance2.Visible = false;
				alert_message_success.Visible = false;
				alert_message_error.Visible = false;

				this.sourceId = DetermineID ();
				this.responseUri = DetermineResponseUri ();
				if (!responseUri.Equals (String.Empty)) {
					//				Session.Remove ("CURRENT_RISK_DESC");
					//				Session.Remove ("CREATIVITY_PROMPTS");
					creativeGuidance.Visible = true;
					GenerateMatchedSources (responseUri, 0, Constants.MaxPastRisksAtATime - 1);
				} 


			} else {
				Console.WriteLine ("Page_Load - Page.IsPostBack");

				creativeGuidance2.Visible = true;
				this.responseUri = DetermineResponseUri ();
				if (!responseUri.Equals (String.Empty)) {
					//				Session.Remove ("CURRENT_RISK_DESC");
					//				Session.Remove ("CREATIVITY_PROMPTS");
					creativeGuidance.Visible = true;
					GenerateMatchedSources (responseUri, 0, Constants.MaxPastRisksAtATime - 1);
				} 
//				else {
//					errorMessage.InnerHtml = "the service could not retrieve any previous risks. Please try again later.";
//					alert_message_success.Visible = false;
//					alert_message_error.Visible = true;
//				}

			}


			var processGuidanceText = Util.GenerateProcessGuidance ("riskResolutions");
			creativeGuidance.InnerText = processGuidanceText.Equals(String.Empty)?defaultProcessGuidance:processGuidanceText;

		}

		private string DetermineID()
		{
			string id = String.Empty;
			if (Session ["CURRENT_RISK"] != null)
			{
				id = Session ["CURRENT_RISK"].ToString ();
			}
			return id;
		}

		private void FindRisks ()
		{
			string responseXml = PerformEddie();

//			this.requestId = DetermineID ();
			string responseRef = "Response_" + this.requestId + ".xml";
			//string responseRef = "Response_201131_153042.xml";
			string responseXmlUri = Path.Combine (responsePath, responseRef);
			if (File.Exists (responseXmlUri))
				File.Delete (responseXmlUri);
			FileStream responseStream = new FileStream(responseXmlUri, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
			StreamWriter responseStreamWriter = new StreamWriter(responseStream, Encoding.ASCII);
			responseStreamWriter.Write(responseXml);
			responseStreamWriter.Flush();
			responseStreamWriter.Close();
			responseStream.Close();

			this.responseUri = responseXmlUri;
			Session ["CurrentResponseUri"] = responseXmlUri;
//			Response.Redirect("CreateIdeas_PastRisks.aspx", false);
		}

		#region Service Call
		string PerformEddie()
		{
			XmlProc.RequestSerialized.Request request = CreateEddieRequestWithNewRequestId(Path.Combine (xmlFilesPath, "EddieRequest_template.xml"));

//			this.requestId = DetermineID ();
			string requestRef = "Request_" + this.requestId + ".xml";
			string requestXmlUri = Path.Combine (requestPath, requestRef);

			XmlProc.ObjectXMLSerializer<XmlProc.RequestSerialized.Request>.Save(request, requestXmlUri);

			FileStream cgStream1 = new FileStream(requestXmlUri, FileMode.Open, FileAccess.Read);
			StreamReader cgStreamReader1 = new StreamReader(cgStream1);
			string requestXml = cgStreamReader1.ReadToEnd();
			cgStreamReader1.Close();

			//errorLabel.Text = requestXml;

			var watch = Stopwatch.StartNew();

			RiskHuntingAppTest.eddieService.EDDiEWebService eddie = new RiskHuntingAppTest.eddieService.EDDiEWebService();
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


		#endregion

		#region Xml generation
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

		private string GenerateTargetDescription ()
		{
			string content = String.Empty;
			content += "[InjuryNature]: " + String.Empty;
			content += " [LocationDetail]: " + this.currentRisk.LocationDetail;
			content += " [Content]: " + this.currentRisk.Content;
			content += " [BodyPart]: " + this.currentRisk.BodyPart;
			return content;

		}

		private int GetNewSourceId()
		{
			int maxId = 0;
			//			if (Session ["CURRENT_RISK"] == null) {
			//				string path = FindLatestSourcePath ();
			string path = GetFilesFromDirectorySortedList4 (Path.Combine (sourcesPath, Constants.CASE_TYPE, Constants.SOURCESPECIFICATION));
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
		private string DetermineResponseUri()
		{
			string responseUri = String.Empty;
			if (Session ["CurrentResponseUri"] != null)
				responseUri = Session ["CurrentResponseUri"].ToString ();
			else if (Request.QueryString ["path"] != null) {
				responseUri = Request.QueryString ["path"];
				Session ["CurrentResponseUri"] = responseUri;
			} else if (File.Exists (Path.Combine (responsePath, "Response_" + this.sourceId + ".xml"))) {
				responseUri = Path.Combine (responsePath, "Response_" + this.sourceId + ".xml");
				Session ["CurrentResponseUri"] = responseUri;
			}
			return responseUri; 
		}

		private void GenerateMatchedSources(string responseUri, int startIndex, int max)
		{
			responses.InnerHtml = String.Empty;
			XmlProc.ResponseSerialized.MatchedSources response = XmlProc.ObjectXMLSerializer<XmlProc.ResponseSerialized.MatchedSources>.Load(responseUri);

			List<XmlProc.ResponseSerialized.MatchedSourcesMatchedSource> matchedSources = (List<XmlProc.ResponseSerialized.MatchedSourcesMatchedSource>)response.MatchedSource;
			if (matchedSources.Count > 0) {
				submit.Text = "FIND MORE RISKS";
				if (matchedSources.Count - 1 < max)
					max = matchedSources.Count - 1;

				for (int i = startIndex; i <= max; i++) {
					XmlProc.ResponseSerialized.MatchedSourcesMatchedSource matchedSource = matchedSources [i];
					if (Convert.ToDouble (matchedSource.OverallMatchValue) >= Constants.THRESHOLD)
						responses.InnerHtml += GenerateMatchedSourceHtml (matchedSource);
				}
				Console.WriteLine (responses.InnerHtml);
				if (responses.InnerHtml.Trim ().Equals (String.Empty)) {
					errorMessage.InnerHtml = "No cases have been found.";
					alert_message_success.Visible = false;
					alert_message_error.Visible = true;
//					creativeGuidance2.Visible = true;
				}
			} else {
				errorMessage.InnerHtml = "No cases have been found.";
				alert_message_success.Visible = false;
				alert_message_error.Visible = true;
//				creativeGuidance2.Visible = true;
			}
		}

		private string GenerateMatchedSourceHtml(XmlProc.ResponseSerialized.MatchedSourcesMatchedSource matchedSource)
		{
			string riskName = Util.ExtractAttributeContentFromString (matchedSource.Content, "Content").ExtractKeywords ().TruncateAtWord (10);
			var bodyPart = Util.ExtractAttributeContentFromString (matchedSource.Content, "BodyPart").Equals (String.Empty) ? "Not specified" : Util.ExtractAttributeContentFromString (matchedSource.Content, "BodyPart");
			return Tag1 + Tag2 + matchedSource.SourceId + Tag3 + Tag4 + riskName + Tag5 + Tag6 +
				Util.ExtractAttributeContentFromString (matchedSource.Content, "Content") + Tag7 + 
				Tag8 + "<b>Location</b>: " + Util.ExtractAttributeContentFromString (matchedSource.Content, "LocationDetail") + 
				"&nbsp;&nbsp;&nbsp;" + "<b>Body Part</b>: " + bodyPart + 
				"&nbsp;&nbsp;&nbsp;" + 
//				" <b>Created</b>: " + Util.FormatDate () + 
				Tag9 + Tag10 + Tag11 + Tag12;
		}

		public virtual void submitClicked(object sender, EventArgs args)
		{
			this.requestId = DetermineID ();
			if (!this.requestId.Equals (String.Empty)) {
				this.sourceId = this.requestId;
				RetrieveCurrentRisk ();
				FindRisks ();
				GenerateMatchedSources(responseUri, 0, Constants.MaxPastRisksAtATime - 1);
			}

		}

		public virtual void creativeGuidanceClicked(object sender, EventArgs args)
		{
			Response.Redirect ("CreateIdeas_SameRisk.aspx");
		}
	}
}

