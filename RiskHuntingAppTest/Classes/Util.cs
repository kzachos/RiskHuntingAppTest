using System;
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

namespace RiskHuntingAppTest
{
	public static class Util
	{
		static string xmlFilesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles");

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
			ss.SourceSpecificationLastEdited = now.ToString();

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification fspecProblem = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification();
			fspecProblem.FacetSpecificationLanguage = "Text";
			fspecProblem.FacetSpecificationLink = "CaseStudy_" + risk.Id + "_Problem.xml";

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification fspecSolution = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification();
			fspecSolution.FacetSpecificationLanguage = "Text";
			fspecSolution.FacetSpecificationLink = "CaseStudy_" + risk.Id + "_Solution.xml";

			XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet problem = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet();
			problem.FacetType = "Problem";
			problem.Author = risk.Author;
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
			problem.Author = risk.Author;
			problem.LaunchDate = now.ToString();
			problem.SourceSpecificationLastEdited = now.ToString();

			XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData fsd = new XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData();
			fsd.Content = risk.Content;
			//fsd.Observations = new System.Collections.Generic.List<XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation>();
			//XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation obs = new XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation();
			//obs.id = 1;
			//obs.launchDate = now.ToString();
			//obs.Value = "n/A";
			//fsd.Observations.Add(obs);
			fsd.Observations = String.Empty;
			fsd.ObservedBehaviour = risk.LocationDetail;
			fsd.TreatmentType = risk.BodyPart;
			fsd.DateOfIncident = String.Empty;
			fsd.AilmentType = String.Empty;
			fsd.TriggeringEvent = String.Empty;
			fsd.Miscellaneous = String.Empty;
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
			solution.Author = risk.Author;
			solution.LaunchDate = now.ToString();
			solution.SourceSpecificationLastEdited = now.ToString();

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

		public static int GetHtmlSelectIdForLocation (string searchValue)
		{
			int id = 0;
			bool found = false;
			var doc = XDocument.Load(Path.Combine (xmlFilesPath, "Parameters.xml"), LoadOptions.None); 
			if (doc.Descendants ("rl").Count () > 0)
				foreach (XElement xe in doc.Descendants("rl")) {
					if (xe.Element ("n").Value.Equals (searchValue)) {
						found = true;
						break;
					}
					else
						id++;
				}
			if (found)
				return id;
			else
				return -1;

		}

		public static int GetHtmlSelectIdForBodyPart (string searchValue)
		{
			int id = 0;
			bool found = false;
			var doc = XDocument.Load(Path.Combine (xmlFilesPath, "Parameters.xml"), LoadOptions.None); 
			if (doc.Descendants ("bp").Count () > 0)
				foreach (XElement xe in doc.Descendants("bp")) {
					if (xe.Element ("n").Value.Equals (searchValue)) {
						found = true;
						break;
					}
					else
						id++;
				}
			if (found)
				return id;
			else
				return -1;

		}
	}
}

