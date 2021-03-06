﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.Net;
using System.Web;

namespace RiskHuntingAppTest
{
	public static class Util
	{
		static string xmlFilesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles");
		static string resourcesPath = Path.Combine (SettingsTool.GetApplicationPath(), "Resources");


		#region Extract content 

		/// <summary>
		/// Separates a string into different compononents divided by a delimitor
		/// </summary>
		/// <example><c>toSeparate</c>: <c>house is on fire</c>; <c>delim</c> is <c>' '</c> and string will be separated into <c>house</c> <c>is</c> <c>on</c> <c>fire</c></example>
		/// <param name="toSeparate">string to be separated</param>
		/// <param name="delim">array of characters to be used as delimitors</param>
		/// <returns>a SortedList containing each string component</returns>
		public static SortedList SeperateStringByChar(string toSeparate, char[] delim)
		{
			char[] deliminator = delim;
			ArrayList firstResults = new ArrayList();
			firstResults.AddRange(toSeparate.Split(deliminator));

			// ...then trim each record in list
			string trimmedString;
			SortedList finalResults = new SortedList();
			for (int j = 0; j < firstResults.Count; j++)
			{
				trimmedString = firstResults[j].ToString().Trim(); //eliminate all newLines, tabs, spaces, etc.
				finalResults.Add(j, trimmedString);
			}

			// remove first any empty records in the list
			for (int i = 0; i < finalResults.Count; i++)
			{
				if (finalResults[i].ToString().Equals(String.Empty)) finalResults.RemoveAt(i);
			}
			return finalResults;
		}

		public static string ExtractAttributeContentFromString (string text, string attribute)
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

		public static string ExtractAttributeContentFromString2 (string text, string attribute)
		{
			//Regex regex = new Regex(@"\[ProblemDescription\]\:\s*(((?!\[SolutionDescription\]:|\[).)+)\s*\[");
			Regex regex = new Regex(@"\[" + attribute + @"\:([\s\S]*?)\]");
			string extractedText = null;
			Match match = regex.Match(text);
			if(match.Success)
			{
				extractedText = match.Groups[1].Value;
			}	
			return extractedText;
		}

		public static string ExtractLastAttributeFromString(string text)
		{
			char[] deliminator = new char[] { ']' };
			NLP.StringProc str = new NLP.StringProc();
			SortedList stringParts = str.SeperateStringByChar(text, deliminator);
			string extractedText = stringParts[stringParts.Count - 1].ToString();
			extractedText = extractedText.Replace(": ", String.Empty);

			return extractedText;
		}


		#endregion


		#region Access Log

		public enum ScreenType
		{
			History,
			DescribeRisk,
			CreateIdea_SameRisk,
			CreateIdea_Superheroes,
			CreateIdea_PastRisks,
			CreateIdea_PastRisk,
			ResolveRisk,
			Summary,
			AddIdea,
			EditIdea,
			MarkRiskAsResolved
		}

		public enum FeatureType
		{
			History_Sort,
			DescribeRisk_CreateIdeasButton,
			DescribeRisk_ClearFormButton,
			DescribeRisk_DeleteRiskButton,
			CreateIdea_SameRisk_GenerateNewPromptsButton,
			CreateIdea_Superheroes_GenerateNewSuperheroButton,
			CreateIdea_PastRisks_FindRisksButton,
			CreateIdea_PastRisk_GenerateNewPromptsButton,
			AddIdea_AddIdeaButton,
			EditIdea_UpdateIdeaButton,
			EditIdea_DeleteIdeaButton,
			Summary_SubmitCaseButton,
			Summary_GenerateReportButton,
			Summary_CreateNewRiskButton,
			Summary_MarkAsResolvedButton,
			MarkAsResolved_SubmitButton
		}

		public static void AccessLog(ScreenType screen)
		{
			var sourceId = String.Empty;
			if (Sessions.RiskState != String.Empty)
				sourceId = Sessions.RiskState;
			string msg = string.Format("{0}, {1}, {2}, {3}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), sourceId.Equals(String.Empty)?"no_ID":sourceId, screen.ToString(), GetVisitor());

			//			Console.WriteLine ("AccessLogFilename: " + AccessLogFilename);
			lock (loggingGate)
			{
				using (StreamWriter sw = File.AppendText(AccessLogFilename))
				{
					sw.WriteLine(msg);
					sw.Flush();
					sw.Close();
				}

			}

		}

		public static void AccessLog(ScreenType screen, FeatureType feature)
		{
			var sourceId = String.Empty;
			if (Sessions.RiskState != String.Empty)
				sourceId = Sessions.RiskState;
			string msg = string.Format("{0}, {1}, {2}, {3}, {4}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), sourceId.Equals(String.Empty)?"no_ID":sourceId, screen.ToString(), feature.ToString(),  GetVisitor());

			//			Console.WriteLine ("AccessLogFilename: " + AccessLogFilename);
			lock (loggingGate)
			{
				using (StreamWriter sw = File.AppendText(AccessLogFilename))
				{
					sw.WriteLine(msg);
					sw.Flush();
					sw.Close();
				}

			}

		}

		public static void DeleteAccessLogFile()
		{
			if (File.Exists(AccessLogFilename))
			{
				File.Delete(AccessLogFilename);
			}
		}

		public static string AccessLogFilename
		{
			get
			{
				return Path.Combine(resourcesPath, "Access.log");
			}
		}

		static string GetVisitor()
		{

			string strIPAddress = string.Empty;
			string strVisitorCountry = string.Empty;

			strIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

			if (strIPAddress == "" || strIPAddress == null)
				strIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
			else
			{
				string[] ipRange = strIPAddress.Split(',');
				int le = ipRange.Length - 1;
				strIPAddress = ipRange[le];
			}

			DataTable _objDataTable = Util.GetLocation(strIPAddress);

			if (_objDataTable != null)
			{

				if (_objDataTable.Rows.Count > 0)
				{
					bool success = false;
					strVisitorCountry =
						"{IP: "
						+ strIPAddress
						+ ", COUNTRY: "
						+ Convert.ToString(_objDataTable.Rows[0]["Country"]).ToUpper()
						//+ ", COUNTRY CODE: "
						//+ Convert.ToString(_objDataTable.Rows[0]["CountryCode"]).ToUpper()
						//+", LATITUDE: "
						//+ Convert.ToString(_objDataTable.Rows[0]["Latitude"]).ToUpper()
						//+", LONGITUDE: "
						//+ Convert.ToString(_objDataTable.Rows[0]["Longitude"]).ToUpper()
//						+", LOCATION: "
//						+ GoogleReverseGeocoder.ReverseGeocode (Convert.ToDouble(_objDataTable.Rows[0]["Latitude"]),Convert.ToDouble(_objDataTable.Rows[0]["Longitude"]), out success)
						+ "}";

				}

				else
				{
					strVisitorCountry = "{IP: " + strIPAddress + "}";

				}

			}
			else
				strVisitorCountry = "{IP: " + strIPAddress + "}";
			return strVisitorCountry;
		}

		/// <summary>
		/// Get the location from IP Address using the free 
		/// services of (http://freegeoip.appspot.com/xml/) 
		/// </summary>
		/// <param name="strIPAddress"></param>
		/// <returns> The Response into the Datatable Index 0 </returns>
		static DataTable GetLocation(string strIPAddress)
		{
			//Create a WebRequest with the current Ip
			WebRequest _objWebRequest =
				WebRequest.Create("http://ws.cdyne.com/ip2geo/ip2geo.asmx/ResolveIP?ipAddress="
					+ strIPAddress + "&licenseKey=0");
			//Create a Web Proxy
			WebProxy _objWebProxy =
				new WebProxy("http://ws.cdyne.com/ip2geo/ip2geo.asmx/ResolveIP?ipAddress="
					+ strIPAddress + "&licenseKey=0", true);

			//Assign the proxy to the WebRequest
			_objWebRequest.Proxy = _objWebProxy;

			//Set the timeout in Seconds for the WebRequest
			_objWebRequest.Timeout = 2000;

			try
			{
				//Get the WebResponse 
				WebResponse _objWebResponse = _objWebRequest.GetResponse();
				//Read the Response in a XMLTextReader
				XmlTextReader _objXmlTextReader
				= new XmlTextReader(_objWebResponse.GetResponseStream());

				//Create a new DataSet
				DataSet _objDataSet = new DataSet();
				//Read the Response into the DataSet
				_objDataSet.ReadXml(_objXmlTextReader);

				return _objDataSet.Tables[0];
			}

			catch
			{

				return null;

			}

		} 



		public static object loggingGate = new object();

		#endregion

		public static bool ServiceExists(string url, bool throwExceptions, out string errorMessage)
		{
			try
			{
				errorMessage = string.Empty;

				// try accessing the web service directly via it's URL
				HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
				request.Timeout = 100000;

				using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
				{
					if (response.StatusCode != HttpStatusCode.OK)
						throw new Exception("Error locating web service");
				}

				// try getting the WSDL?
				// asmx lets you put "?wsdl" to make sure the URL is a web service
				// could parse and validate WSDL here

			}
			catch (WebException ex)
			{   
				// decompose 400- codes here if you like
				//		        errorMessage = string.Format("Error testing connection to web service at \"{0}\":\r\n{1}", url, ex);
				errorMessage = "Currently unable to connect to the remote discovery service.";
				if (throwExceptions)
					throw new Exception(errorMessage, ex);
				return false;
			}   
			catch (Exception ex)
			{
				errorMessage = "Currently unable to connect to the remote discovery service.";
				if (throwExceptions)
					throw new Exception(errorMessage, ex);
				return false;
			}

			return true;
		}



		public static string TruncateAtWord(this string value, int length) 
		{
			if (value == null || value.Length < length || value.IndexOf(" ", length) == -1)
				return value;

			return value.Substring(0, value.IndexOf(" ", length));
		}

		public static string ExtractKeywords (this string value) 
		{
			string strWordsToExclude="a,able,about,across,after,all,almost,also," +
				"am,among,an,and,any,are,as,at,be,because,been,but,by,can,cannot,could," +
				"dear,did,do,does,either,else,ever,every,for,from,get,got,had,has,have,he," +
				"her,hers,him,his,how,however,i,if,in,into,is,it,its,just,least,let,like,likely," +
				"may,me,might,most,must,my,neither,no,nor,not,of,off,often,on,only,or,other,our," +
				"own,rather,said,say,says,she,should,since,so,some,than,that,the,their,them,then," +
				"there,these,they,this,tis,to,too,twas,us,wants,was,we,were,what,when,where,which," +
				"while,who,whom,why,will,with,would,yet,you,your";
			var ignoredWords = strWordsToExclude.Split(',');
			var newWords = GetSearchWords(value, ignoredWords);
			return ConvertStringArrayToString(newWords);
		}

		static string[] GetSearchWords(string text, IEnumerable<string> toExclude)
		{
			var words = text.Split();

			return words.Where(word => !toExclude.Contains(word)).ToArray();
		}

		static string ConvertStringArrayToString(string[] array)
		{
			//
			// Concatenate all the elements into a StringBuilder.
			//
			StringBuilder builder = new StringBuilder();
			foreach (string value in array)
			{
				builder.Append(value);
				builder.Append(' ');
			}
			return builder.ToString();
		}

		static string ConvertStringArrayToStringJoin(string[] array)
		{
			//
			// Use string Join to concatenate the string elements.
			//
			string result = string.Join(".", array);
			return result;
		}


		public static DateTime ConvertDateTime(string date)
		{
//			var gb = new System.Globalization.CultureInfo("en-GB");
//			string format = "dd/MM/yyyy HH:mm:ss";
//			IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
//			DateTime dt = DateTime.ParseExact (date, format, gb);

			// Specify exactly how to interpret the string.
			IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
			DateTime dt = DateTime.Parse(date, culture, System.Globalization.DateTimeStyles.AssumeLocal);
			return dt;
		}

		#region Xml 

		public static XmlProc.SourceSpecificationSerialized.SourceSpecification CreateSourceSpecificationXml(Risk risk)
		{
			XmlProc.SourceSpecificationSerialized.SourceSpecification ss = new XmlProc.SourceSpecificationSerialized.SourceSpecification();

			DateTime now = DateTime.Now;
			ss.SourceId = risk.Id;
			ss.SourceName = risk.Name;
//			ss.SourceType = "" + Constants.CASEREF;
			ss.SourceType = risk.State.ToString();
			ss.Domain = "Risk";
//			ss.Filename = ss.SourceType + risk.Id + ".docx";
			ss.Filename = risk.SimilarCasesFound?"Found":String.Empty;
			ss.LaunchDate = now.ToString();
			ss.SourceSpecificationLastEdited = risk.DateIncidentOccurred.ToShortDateString();

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification fspecProblem = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification();
			fspecProblem.FacetSpecificationLanguage = "Text";
			fspecProblem.FacetSpecificationLink = Constants.CASEREF + risk.Id + "_Problem.xml";

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification fspecSolution = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification();
			fspecSolution.FacetSpecificationLanguage = "Text";
			fspecSolution.FacetSpecificationLink = Constants.CASEREF + risk.Id + "_Solution.xml";

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet problem = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet();
			problem.FacetType = "Problem";
			problem.Author = risk.Author + "|" + risk.AuthorFIN;
			problem.FacetSpecification = fspecProblem;

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet solution = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet();
			solution.FacetType = "Solution";
			solution.Author = risk.Author;
			solution.FacetSpecification = fspecSolution;

			ss.Facet = new System.Collections.Generic.List<XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet>();
			ss.Facet.Add(problem);
			ss.Facet.Add(solution);

			return ss;
		}

		public static XmlProc.ProblemSerialized.LanguageSpecificSpecification CreateProblemXml(Risk risk)
		{
			XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = new XmlProc.ProblemSerialized.LanguageSpecificSpecification();

			DateTime now = DateTime.Now;
			problem.FacetType = "Problem";
			problem.FacetSpecificationLanguage = "Text";
			problem.Author = risk.Author + "|" + risk.AuthorFIN;
			problem.LaunchDate = now.ToString();
			problem.SourceSpecificationLastEdited = risk.DateIncidentOccurred.ToShortDateString();

			XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData fsd = new XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData();
			fsd.Content = risk.Content;
			//fsd.Observations = new System.Collections.Generic.List<XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation>();
			//XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation obs = new XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation();
			//obs.id = 1;
			//obs.launchDate = now.ToString();
			//obs.Value = "n/A";
			//fsd.Observations.Add(obs);
			fsd.InjuryNature = risk.InjuryNature;
			fsd.LocationDetail = risk.LocationDetail;
			fsd.BodyPart = risk.BodyPart;
			fsd.InjuryCause = String.Empty;
			fsd.IncidentPriority = String.Empty;
			fsd.IncidentStatus = risk.IncidentStatus;
			fsd.RootCause = String.Empty;
			fsd.RoutineWork = String.Empty;
			fsd.ShiftType = String.Empty;
			fsd.Title = String.Empty;
			fsd.ClosedByOperator = String.Empty;
			fsd.ContractorName = risk.ContractorName;
			fsd.DateClosed = String.Empty;
			fsd.DateIncidentOccurred = risk.DateIncidentOccurred.ToString();
			fsd.Miscellaneous = risk.ImageUri;
			fsd.MatchingDetails = String.Empty;

			problem.FacetSpecificationData = fsd;


			return problem;
		}

		public static XmlProc.SolutionSerialized.LanguageSpecificSpecification CreateSolutionXml(Risk risk)
		{
			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = new XmlProc.SolutionSerialized.LanguageSpecificSpecification();

			DateTime now = DateTime.Now;
			solution.FacetType = "Solution";
			solution.FacetSpecificationLanguage = "Text";
			solution.Author = risk.Author + "|" + risk.AuthorFIN;
			solution.LaunchDate = now.ToString();
			solution.SourceSpecificationLastEdited = risk.DateIncidentOccurred.ToShortDateString();

			XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData fsd = new XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData();

			fsd.Content = ResolutionsArrayToString (risk.Recommendations, risk.Actions);
			fsd.Miscellaneous = risk.Miscellaneous;

			solution.FacetSpecificationData = fsd;


			return solution;
		}

		public static string ResolutionsArrayToString (ArrayList recommendations, List<Action> actions)
		{
			string content = String.Empty;
			if (recommendations.Count > 0) {
				content += "[Recommendation:";
				for (int i = 0; i < recommendations.Count; i++)
					content += recommendations [i] + ";";
				if (recommendations.Count > 0)
					content = content.Remove (content.Length - 1, 1);
				content += "];";
				Console.WriteLine (content);

			} 
			else
				content += "[Recommendation:];";

			if (actions.Count > 0) {
				content += "[Corrective Actions:";
				for (int i = 0; i < actions.Count; i++) {
					Action action = actions [i];
					content += action.Id + "|";
					content += action.Content + "|";
					content += action.Owner + "|";
					content += action.ImplementBy.ToShortDateString();
					content += ";";
				}
				if (actions.Count > 0)
					content = content.Remove (content.Length - 1, 1);
				content += "];";
			}
			else
				content += "[Corrective Actions:];";
			content += "[Countermeasures:]";
			return content;
		}

		public static ArrayList ResolutionsStringToArray (string resolutions, string filter)
		{
			ArrayList resolutionsArray = new ArrayList ();

			var content = Util.ExtractAttributeContentFromString2 (resolutions, filter).Trim ();
			char[] delim = new char[] {';'};
			resolutionsArray.AddRange(content.Split(delim));
			if (resolutionsArray[resolutionsArray.Count-1].ToString ().Trim ().Equals (String.Empty))
				resolutionsArray.Remove (resolutionsArray[resolutionsArray.Count-1]);
			return resolutionsArray;
		}

		public static Action ActionStringToObject (string actionString)
		{
			NLP.StringProc str = new NLP.StringProc();
			char[] deliminator = new char[] { '|' };
			var bits = str.SeperateStringByChar (actionString, deliminator);
		
			Action action = new Action ();
			if (bits.Count == 4) {
				action.Id = bits [0].ToString ();
				action.Content = bits [1].ToString ();
				action.Owner = bits [2].ToString ();
				action.ImplementBy = Convert.ToDateTime (bits [3].ToString ());
			} 
			return action;
		}

		#endregion 

		public static string GenerateAdaptedIdeaFromCreativityPrompt(string text)
		{
			string formattedText = String.Empty;
			if (text.StartsWith("how to"))
				formattedText = text.Replace ("how to ", String.Empty);
				
			if (text.StartsWith("if you can"))
				formattedText = text.Replace ("if you can ", String.Empty);
				
			if (text.StartsWith("if it is possible to"))
				formattedText = text.Replace ("if it is possible to ", String.Empty);
				
			if (text.StartsWith("how you"))
				formattedText = text.Replace ("how you ", String.Empty);
				
			if (text.StartsWith("whether you can"))
				formattedText = text.Replace ("whether you can ", String.Empty);
				
			if (text.StartsWith("how you might"))
				formattedText = text.Replace ("how you might ", String.Empty);
				
			if (text.StartsWith("if you could"))
				formattedText = text.Replace ("if you could ", String.Empty);
				
			if (text.StartsWith("whether it is possible to"))
				formattedText = text.Replace ("whether it is possible to ", String.Empty);
				
			if (text.StartsWith("trying to"))
				formattedText = text.Replace ("trying to ", String.Empty);
				
			if (text.StartsWith("either trying to"))
				formattedText = text.Replace ("either trying to ", String.Empty);
				
			if (text.StartsWith("doing"))
				formattedText = text.Replace ("doing", "do");
				
			if (text.StartsWith("making"))
				formattedText = text.Replace ("making", "make");
				
			if (text.StartsWith("dividing"))
				formattedText = text.Replace ("dividing", "divide");
				
			if (text.StartsWith("putting"))
				formattedText = text.Replace ("putting", "put");
				
			if (text.StartsWith("deactivating"))
				formattedText = text.Replace ("deactivating", "deactivate");
				
			if (text.StartsWith("using"))
				formattedText = text.Replace ("using", "use");
				
			if (text.StartsWith("having"))
				formattedText = text.Replace ("having", "have");
				
			if (text.StartsWith("evening out"))
				formattedText = text.Replace ("evening out", "even out");

			return formattedText;
		}

		public static List<NLResponseToken> DeserializeNLResponse (string jsonString)
		{
			List<NLResponseToken> response = new List<NLResponseToken> ();
			try {
				response = JsonConvert.DeserializeObject<List<NLResponseToken>>(jsonString);

			} catch (Exception ex) {
				Debug.WriteLine ("ERROR deserializing downloaded NLParser JSON: " + ex);
			}
			return response;
		}

		public static List<Persona> DeserializeHofResponse (string jsonString)
		{
			List<Persona> response = new List<Persona> ();
			try {
				response = JsonConvert.DeserializeObject<List<Persona>>(jsonString);

			} catch (Exception ex) {
				Debug.WriteLine ("ERROR deserializing downloaded NLParser JSON: " + ex);
			}
			return response;
		}

		public static string GetEnumDescription(Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			DescriptionAttribute[] attributes = 
				(DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes != null && attributes.Length > 0)
				return attributes[0].Description;
			else
				return value.ToString();
		}

		public static string FormatDate (DateTime date)
		{

			if (DateTime.Today == date.Date) {
				return "today " + date.ToString ("HH:mm");
			} else if ((DateTime.Today - date.Date).TotalDays < 7) {
				return date.ToString ("ddd HH:mm");
			} else
			{
				return date.ToShortDateString ();			
			}
		}

		public static string GenerateProcessGuidance(string elementName)
		{
			List<string> problemDescriptions = new List<string> ();
				var doc = XDocument.Load(Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "ProcessGuidance.xml"), LoadOptions.None); 
			if (doc.Descendants(elementName).Count() > 0)
				foreach (XElement xe in doc.Descendants(elementName))
					problemDescriptions.Add(xe.Element("n").Value);    

			if (problemDescriptions.Count > 0) {
				problemDescriptions.Shuffle ();
				Console.WriteLine ("problemDescriptions [0]: " + problemDescriptions [0]);
				return problemDescriptions [0];
			} else
				return String.Empty;
		}

		public static int GetHtmlSelectIdForDepartment (string searchValue)
		{
			int id = 0;
			int found = -1;
			var doc = XDocument.Load(Path.Combine (resourcesPath, "Parameters.xml"), LoadOptions.None); 
			if (doc.Descendants ("rl").Count () > 0) {
				foreach (XElement xe in doc.Descendants("rl")) {
					if (xe.Element ("n").Value.Equals (searchValue)) {
						found = id;
						
					} else
						id++;
				}
			}
			return found;

		}

		public static int GetHtmlSelectIdForLocation (string searchValue, ArrayList toSearch)
		{
			int id = 0;
			int found = -1;
			if (toSearch.Count > 0) {
				foreach (var xe in toSearch) {
					if (xe.ToString().Equals (searchValue)) {
						found = id;

					} else
						id++;
				}
			}
			return found;

		}

		public static int GetHtmlSelectIdForBodyPart (string searchValue)
		{
			int id = 0;
			int found = -1;
			var doc = XDocument.Load(Path.Combine (resourcesPath, "Parameters.xml"), LoadOptions.None); 
			if (doc.Descendants ("bp").Count () > 0) {
				foreach (XElement xe in doc.Descendants("bp")) {
					if (xe.Element ("n").Value.Equals (searchValue)) {
						found = id;
						
					} else
						id++;
				}
			}
			return found;

		}

		public static int GetHtmlSelectIdForTypeOfInjury (string searchValue)
		{
			int id = 0;
			int found = -1;
			var doc = XDocument.Load(Path.Combine (resourcesPath, "Parameters.xml"), LoadOptions.None); 
			if (doc.Descendants ("TypeOfIncident").Count () > 0) {
				foreach (XElement xe in doc.Descendants("TypeOfIncident")) {
					if (xe.Element ("n").Value.Equals (searchValue)) {
						found = id;

					} else
						id++;
				}
			}
			return found;

		}

		public static int GetHtmlSelectIdForIncidentCategory (string searchValue)
		{
			int id = 0;
			int found = -1;
			var doc = XDocument.Load(Path.Combine (resourcesPath, "Parameters.xml"), LoadOptions.None); 
			if (doc.Descendants ("IncidentCategory").Count () > 0) {
				foreach (XElement xe in doc.Descendants("IncidentCategory")) {
					if (xe.Element ("n").Value.Equals (searchValue)) {
						found = id;

					} else
						id++;
				}
			}
			return found;

		} 

		public enum ImageFormat
		{
			bmp,
			jpeg,
			gif,
			tiff,
			png,
			unknown
		}

		public static ImageFormat GetImageFormat(string pathToFile)
		{
			var bytes = File.ReadAllBytes(pathToFile);
			// see http://www.mikekunz.com/image_file_header.html  
			var bmp    = Encoding.ASCII.GetBytes("BM");     // BMP
			var gif    = Encoding.ASCII.GetBytes("GIF");    // GIF
			var png    = new byte[] { 137, 80, 78, 71 };    // PNG
			var tiff   = new byte[] { 73, 73, 42 };         // TIFF
			var tiff2  = new byte[] { 77, 77, 42 };         // TIFF
			var jpeg   = new byte[] { 255, 216, 255, 224 }; // jpeg
			var jpeg2  = new byte[] { 255, 216, 255, 225 }; // jpeg canon

			if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
				return ImageFormat.bmp;

			if (gif.SequenceEqual(bytes.Take(gif.Length)))
				return ImageFormat.gif;

			if (png.SequenceEqual(bytes.Take(png.Length)))
				return ImageFormat.png;

			if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
				return ImageFormat.tiff;

			if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
				return ImageFormat.tiff;

			if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
				return ImageFormat.jpeg;

			if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
				return ImageFormat.jpeg;

			return ImageFormat.unknown;
		}

		public static bool IsDirectoryEmpty(string path)
		{
			return !Directory.EnumerateFileSystemEntries(path).Any();
		}
	}
}

