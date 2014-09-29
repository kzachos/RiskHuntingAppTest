using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;

namespace RiskHuntingAppTest
{
    public partial class QueryHistory : System.Web.UI.Page
    {
		protected string xmlFilesPath = SettingsTool.GetApplicationPath() + "xmlFiles/";
		protected string requestPath = SettingsTool.GetApplicationPath() + "xmlFiles/Requests/";
		protected string responsePath = SettingsTool.GetApplicationPath() + "xmlFiles/Responses/";

		protected string processPath = SettingsTool.GetApplicationPath() + "xmlFiles/Sources/_toProcess/";
		protected const string SOURCESPECIFICATION = "SourceSpecification";
		protected const string PROBLEM = "Problem";
		protected const string SOLUTION = "Solution";
		protected const string ADDITIONAL = "Additional";

		protected int maxId;

        const string SpanStartTagMenu = "<span class=menu>";
        const string SpanStartTagName = "<span class=name>";
        const string SpanStartTagArrow = "<span class=arrow>";
        const string SpanEndTag = "</span>";

        const string LiStartTagMenu = "<li class=menu>";
        const string LiEndTag = "</li>";

        const string aStartTag = "<a href=\"javascript:doLoad('";
        const string aMidTag = "');\">";
        const string aEndTag = "</a>";


		const string Tag1 = "<li class=\"store\">";
		const string Tag2 = "<a class=\"noeffect\" href=\"javascript:doLoad('Default.aspx?id=";
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
		const string RISKSTATE = "Problem Described";

        protected void Page_Load(object sender, EventArgs e)
        {
			this.maxId = 1000;
//			GenerateQueryHistory(xmlFilesPath, 1000);
			GenerateQueryHistory();
        }

		void GenerateQueryHistory ()
		{
			var allSpecs = GetFilesFromDirectoryList (processPath + SOURCESPECIFICATION + "/", this.maxId);
			var allProblems = GetFilesFromDirectoryList (processPath + PROBLEM + "/", this.maxId);
			var allSolutions = GetFilesFromDirectoryList (processPath + SOLUTION + "/", this.maxId);

//			var allPaths = GetFilesFromDirectorySortedList3(processPath + SOURCESPECIFICATION + "/", this.maxId);
//			ICollection keyCollection = allPaths.Keys;
//			ICollection valueCollection = allPaths.Values;
//			String[] allPathsKeys = new String[allPaths.Count];
//			String[] allPathsValues = new String[allPaths.Count];
//			keyCollection.CopyTo(allPathsKeys, 0);
//			valueCollection.CopyTo(allPathsValues, 0);

			if (allSpecs.Count > 0) {
				for (int i = 0; i < allSpecs.Count; i++) {
					Console.WriteLine (allSpecs [i]);
					var risk = RetrieveCurrentRisk (allSpecs [i], allProblems[i], allSolutions[i]);
					queries.InnerHtml += GenerateQueryHtml (risk);
					Console.WriteLine ("risk.State (GenerateQueryHistory): " + risk.State.ToString ());

				}
			}
			else
				statusLabel.Text = "No risk queries available.";

		}

		Risk RetrieveCurrentRisk (string locSpec, string locProblem, string locSolution)
		{
			XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(locSpec);

			XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Load(locProblem);

			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Load(locSolution);

			return new Risk (ss, problem, solution);

		}


		private string GenerateQueryHtml(Risk risk)
		{
			string responseXmlUri = responsePath + "Response_" + risk.Id + ".xml";
			string path = "&path=" + responseXmlUri;
			string search = risk.SimilarCasesFound?path:String.Empty;
			return Tag1 + Tag2 + risk.Id + search + Tag3 + Tag4 + risk.Name + Tag5 + 
				Tag6 + risk.Content + Tag7 + 
				Tag8 + "Status: " + Util.GetEnumDescription(risk.State) + 
				"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + " Last Modified: " + Util.FormatDate (risk.LaunchDate) + Tag9 + Tag10 + Tag11 + Tag12;
		}

		private OrderedDictionary GetFilesFromDirectorySortedList4(string dirPath, int max)
		{
			OrderedDictionary all = new OrderedDictionary ();
			DirectoryInfo dir = new DirectoryInfo(dirPath);
			//			FileInfo[] FileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
			FileInfo[] FileList = dir.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();
			//Array.Reverse(FileList);
			NLP.StringProc str = new NLP.StringProc();
			char[] deliminator = new char[] { '_' };
			SortedList fileNameParts = new SortedList();
			if (FileList.Length - 1 < max)
				max = FileList.Length - 1;
			//foreach (FileInfo FI in FileList)
			//{
			//    FileInfo FI = FileList[i];
			//    fileNameParts = str.SeperateStringByChar(FI.FullName, deliminator);
			//    all.Add(fileNameParts[1].ToString(), FI.FullName);
			//}
			for (int i = 0; i <= max; i++)
			{
				FileInfo FI = FileList[i];
				//string file = files[i];
				fileNameParts = str.SeperateStringByChar(FI.FullName, deliminator);
				all.Add(fileNameParts[1].ToString(), FI.FullName);
			}

			return all;
		}

        private void GenerateQueryHistory(string dirPath, int max)
        {
			List<string> allResponsePaths = GetFilesFromDirectoryList(dirPath + "Responses/", max);
//			List<string> allResponsePaths = GetFilesFromDirectoryList2(dirPath, max);
			var allRequestPaths = GetFilesFromDirectorySortedList3(dirPath + "Requests/", max);
			ICollection keyCollection = allRequestPaths.Keys;
			ICollection valueCollection = allRequestPaths.Values;
			String[] allRequestPathsKeys = new String[allRequestPaths.Count];
			String[] allRequestPathsValues = new String[allRequestPaths.Count];
			keyCollection.CopyTo(allRequestPathsKeys, 0);
			valueCollection.CopyTo(allRequestPathsValues, 0);
//			SortedList allRequestPaths = GetFilesFromDirectorySortedList2(dirPath, max);

//			NLP.StringProc str = new NLP.StringProc();
//			string[] deliminator3 = new string[] { "xmlFiles\\" };
//			char[] deliminator = new char[] { '_' };
//			char[] deliminator2 = new char[] { '.' };
//
//			SortedList fileNameParts = new SortedList();
//			SortedList fileNameParts2 = new SortedList();
//			string requestId, requestPath;

			if (allResponsePaths.Count > 0)
				for (int i = 0; i < allResponsePaths.Count; i++)
				{
	//				var fileNameParts3 = responsePath.Split(deliminator3, StringSplitOptions.None);
	//				fileNameParts = str.SeperateStringByChar(fileNameParts3[1], deliminator);
	//				fileNameParts2 = str.SeperateStringByChar(fileNameParts[2].ToString(), deliminator2);
	//				requestId = fileNameParts[1].ToString() + fileNameParts2[0].ToString();

					requestPath = allRequestPathsValues[i];

					//Response.Write(requestPath + "<BR>");
					XmlProc.RequestSerialized.Request request = XmlProc.ObjectXMLSerializer<XmlProc.RequestSerialized.Request>.Load(requestPath);
					XmlProc.RequestSerialized.RequestTarget target = (XmlProc.RequestSerialized.RequestTarget)request.Target[0];

					queries.InnerHtml += GenerateQueryHtml(allResponsePaths[i], allRequestPathsKeys[i], target.TargetDescription);
				}
			else
				statusLabel.Text = "No risk queries available.";
        }

		private List<string> GetFilesFromDirectoryList(string dirPath, int max)
		{
			List<string> all = new List<string>();
			DirectoryInfo dir = new DirectoryInfo(dirPath);
//			FileInfo[] FileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
			FileInfo[] FileList = dir.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();
			if (FileList.Length-1 < max)
				max = FileList.Length-1;
			//foreach (FileInfo FI in FileList)
			//{
			//    allQueries.Add(FI.FullName);
			//}
			for (int i = 0; i <= max; i++)
			{
				FileInfo FI = FileList[i];
				all.Add(FI.FullName);
			}
			return all;
		}

		private List<string> GetFilesFromDirectoryList2(string dirPath, int max)
		{
			List<string> all = new List<string>();
			var sortedFiles = new DirectoryInfo(dirPath).GetFiles()
				.OrderBy(f => f.LastWriteTime)
				.ToList();
			DirectoryInfo dir = new DirectoryInfo(dirPath);
			FileInfo[] FileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
			if (sortedFiles.Count - 1 < max)
				max = sortedFiles.Count - 1;
			//foreach (FileInfo FI in FileList)
			//{
			//    allQueries.Add(FI.FullName);
			//}

			NLP.StringProc str = new NLP.StringProc();
			string[] deliminator3 = new string[] { "xmlFiles\\" };
			char[] deliminator = new char[] { '_' };
			SortedList fileNameParts = new SortedList();

			for (int i = 0; i <= max; i++)
			{
				//FileInfo FI = FileList[i];
				FileInfo FI = sortedFiles[i];
				var fileNameParts3 = FI.FullName.Split(deliminator3, StringSplitOptions.None);
				fileNameParts = str.SeperateStringByChar(fileNameParts3[1], deliminator);
				if (fileNameParts[0].ToString().Equals("Response"))
				{
					all.Add(FI.FullName);
				}

			}
			return all;
		}

		private SortedList GetFilesFromDirectorySortedList(string dirPath, int max)
		{
			SortedList all = new SortedList();
			DirectoryInfo dir = new DirectoryInfo(dirPath);
//			FileInfo[] FileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
			FileInfo[] FileList = dir.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();
			//Array.Reverse(FileList);
			NLP.StringProc str = new NLP.StringProc();
			char[] deliminator = new char[] { '_' };
			char[] deliminator2 = new char[] { '.' };
			SortedList fileNameParts = new SortedList();
			SortedList fileNameParts2 = new SortedList();
			if (FileList.Length - 1 < max)
				max = FileList.Length - 1;
			//foreach (FileInfo FI in FileList)
			//{
			//    FileInfo FI = FileList[i];
			//    fileNameParts = str.SeperateStringByChar(FI.FullName, deliminator);
			//    all.Add(fileNameParts[1].ToString(), FI.FullName);
			//}
			for (int i = 0; i <= max; i++)
			{
				FileInfo FI = FileList[i];
				//string file = files[i];
				fileNameParts = str.SeperateStringByChar(FI.FullName, deliminator);
				fileNameParts2 = str.SeperateStringByChar(fileNameParts[1].ToString(), deliminator2);
				all.Add(fileNameParts2[0].ToString(), FI.FullName);
			}

//			foreach (var kvp in all.Keys)
//			{
//				Console.WriteLine(kvp);
//				//					Console.WriteLine(kvp);
//			}
//			foreach (var kvp in all.Values)
//			{
//				Console.WriteLine(kvp);
//				//					Console.WriteLine(kvp);
//			}
			return all;
		}

		private OrderedDictionary GetFilesFromDirectorySortedList3(string dirPath, int max)
		{
			OrderedDictionary all = new OrderedDictionary ();
			DirectoryInfo dir = new DirectoryInfo(dirPath);
			//			FileInfo[] FileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
			FileInfo[] FileList = dir.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();
			//Array.Reverse(FileList);
			NLP.StringProc str = new NLP.StringProc();
			char[] deliminator = new char[] { '_' };
			char[] deliminator2 = new char[] { '.' };
			SortedList fileNameParts = new SortedList();
			SortedList fileNameParts2 = new SortedList();
			if (FileList.Length - 1 < max)
				max = FileList.Length - 1;
			//foreach (FileInfo FI in FileList)
			//{
			//    FileInfo FI = FileList[i];
			//    fileNameParts = str.SeperateStringByChar(FI.FullName, deliminator);
			//    all.Add(fileNameParts[1].ToString(), FI.FullName);
			//}
			for (int i = 0; i <= max; i++)
			{
				FileInfo FI = FileList[i];
				//string file = files[i];
				fileNameParts = str.SeperateStringByChar(FI.FullName, deliminator);
				fileNameParts2 = str.SeperateStringByChar(fileNameParts[1].ToString(), deliminator2);
				all.Add(fileNameParts2[0].ToString(), FI.FullName);
			}
				
			return all;
		}

		private IEnumerable<string> SortDir(string dirPath)
		{
			var files = Directory.GetFiles(dirPath, "*").OrderByDescending(d => new FileInfo(d).LastWriteTime);
			foreach (var directory in files)
			{
				Console.WriteLine(directory);
			}

			return files;
		}

		private SortedList GetFilesFromDirectorySortedList2(string dirPath, int max)
		{
			SortedList all = new SortedList();
			var sortedFiles = new DirectoryInfo(dirPath).GetFiles()
				.OrderBy(f => f.LastWriteTime)
				.ToList();

			DirectoryInfo dir = new DirectoryInfo(dirPath);
			FileInfo[] FileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
			//Array.Reverse(FileList);
			NLP.StringProc str = new NLP.StringProc();
			string[] deliminator3 = new string[] { "xmlFiles\\" };
			char[] deliminator = new char[] { '_' };
			char[] deliminator2 = new char[] { '.' };
			SortedList fileNameParts = new SortedList();
			SortedList fileNameParts2 = new SortedList();
			if (sortedFiles.Count - 1 < max)
				max = sortedFiles.Count - 1;
			//foreach (FileInfo FI in FileList)
			//{
			//    FileInfo FI = FileList[i];
			//    fileNameParts = str.SeperateStringByChar(FI.FullName, deliminator);
			//    all.Add(fileNameParts[1].ToString(), FI.FullName);
			//}
			for (int i = 0; i <= max; i++)
			{
				//FileInfo FI = FileList[i];
				FileInfo FI = sortedFiles[i];
				var fileNameParts3 = FI.FullName.Split(deliminator3, StringSplitOptions.None);
				fileNameParts = str.SeperateStringByChar(fileNameParts3[1], deliminator);
				//Response.Write(fileNameParts[0].ToString() + "<br>");
				if (fileNameParts[0].ToString().Equals("Request"))
				{
					fileNameParts2 = str.SeperateStringByChar(fileNameParts[2].ToString(), deliminator2);
					string key = fileNameParts[1].ToString() + fileNameParts2[0].ToString();
					key = FI.CreationTimeUtc.ToString();
					all.Add(key, FI.FullName);
					//Response.Write(fileNameParts[1].ToString() + fileNameParts2[0].ToString() + "<br>");
					//Response.Write(all[key].ToString() + "<br>");
				}
			}
			return all;
		}


        private string GenerateQueryHtml(string responseXmlUri, string requestId, string requestDescr)
        {
            NLP.StringProc str = new NLP.StringProc();
            char[] deliminator = new char[] { '\\' };
            SortedList fileParts = new SortedList();
            fileParts = str.SeperateStringByChar(responseXmlUri, deliminator);
            responseXmlUri = String.Empty;
            for (int i = 0; i < fileParts.Count-1; i++)
                responseXmlUri += fileParts[i].ToString() + "\\\\";
            responseXmlUri += fileParts[fileParts.Count-1].ToString();
            if (Session["CurrentResponseUri"] != null) Session.Remove("CurrentResponseUri");
			return GenerateInnerHtmlAdvanced (responseXmlUri, requestId, requestDescr);
        }

		private string GenerateInnerHtmlSimple(string responseXmlUri, string requestId, string requestDescr)
		{
			return LiStartTagMenu +
				aStartTag + "Default.aspx?id=" + requestId + "&path=" + responseXmlUri + aMidTag +
				SpanStartTagName + requestId + ": " + ExtractAttributeContentFromString(requestDescr, "Content") + SpanEndTag +
				SpanStartTagArrow + SpanEndTag + aEndTag + LiEndTag;
		}

		private string GenerateInnerHtmlAdvanced(string responseXmlUri, string requestId, string requestDescr)
		{
			return Tag1 + Tag2 + requestId + "&path=" + responseXmlUri + Tag3 + Tag4 + ExtractAttributeContentFromString(requestDescr, "Content") + Tag5 + 
//				Tag6 + ExtractAttributeContentFromString(requestDescr, "Content") + Tag7 + 
				Tag8 + Util.GetEnumDescription(RiskQueryState.IdeasGenerated) + Tag9 + Tag10 + Tag11 + Tag12;
		}

		#region Extract content 

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
			extractedText = extractedText.Replace(": ", String.Empty);

			return extractedText;
		}

		#endregion
    }

    public partial class InvertedComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return y.CompareTo(x);
        }
    }


}
