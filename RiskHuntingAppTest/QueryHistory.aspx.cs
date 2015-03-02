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
		protected string xmlFilesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles");
		protected string requestPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Requests");
		protected string responsePath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Responses");

		protected string processPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources", "_toProcess");
		protected const string SOURCESPECIFICATION = "SourceSpecification";
		protected const string PROBLEM = "Problem";
		protected const string SOLUTION = "Solution";
		protected const string ADDITIONAL = "Additional";

		protected int maxId;
		protected string sortBy;

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
		const string Tag2 = "<a class=\"noeffect\" href=\"javascript:doLoad('DescribeRisk.aspx?id=";
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

        protected void Page_Init(object sender, EventArgs e)
        {
			Session.RemoveAll ();
			this.maxId = 1000;
			if (!Page.IsPostBack) {
				Console.WriteLine ("Page_Init - NOT Page.IsPostBack");
				PopulateSortDropDown ();
			} else {
				Console.WriteLine ("Page_Init - Page.IsPostBack");

			}

        }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack) {
				Console.WriteLine ("Page_Load - NOT Page.IsPostBack");
				GenerateQueryHistory();
			} else {
				Console.WriteLine ("Page_Load - Page.IsPostBack");
			}

		}

		protected void Page_PreRender (object sender, EventArgs e)
		{
			if (Page.IsPostBack) {
				this.sortBy = DetermineSortAttribute ();
//				Console.WriteLine ("SortBy (Page_PreRender): " + this.sortBy);
				GenerateQueryHistory ();
			}
		}

		string DetermineSortAttribute()
		{
			var sortBy = String.Empty;
			if (Session ["SortBy"] != null)
				sortBy = Session ["SortBy"].ToString();
			return sortBy;
		}

		private void PopulateSortDropDown()
		{
//			SortDropDown.Items.Add (new ListItem("Date"));
//			SortDropDown.Items.Add (new ListItem("Status"));
//			SortDropDown.Items.Add (new ListItem("Name"));
//			SortDropDown.Items.Add (new ListItem("Author"));
			SortDropDown.SelectedValue = "0";
		}


		void GenerateQueryHistory ()
		{
//			var allSpecs = GetFilesFromDirectoryList (processPath + SOURCESPECIFICATION + "/", this.maxId);
//			var allProblems = GetFilesFromDirectoryList (processPath + PROBLEM + "/", this.maxId);
//			var allSolutions = GetFilesFromDirectoryList (processPath + SOLUTION + "/", this.maxId);
			var allSpecs = GetFilesFromDirectoryList (Path.Combine (processPath, SOURCESPECIFICATION), this.maxId);
			var allProblems = GetFilesFromDirectoryList (Path.Combine (processPath, PROBLEM), this.maxId);
			var allSolutions = GetFilesFromDirectoryList (Path.Combine (processPath, SOLUTION), this.maxId);

//			var allPaths = GetFilesFromDirectorySortedList3(processPath + SOURCESPECIFICATION + "/", this.maxId);
//			ICollection keyCollection = allPaths.Keys;
//			ICollection valueCollection = allPaths.Values;
//			String[] allPathsKeys = new String[allPaths.Count];
//			String[] allPathsValues = new String[allPaths.Count];
//			keyCollection.CopyTo(allPathsKeys, 0);
//			valueCollection.CopyTo(allPathsValues, 0);

			if (allSpecs.Count > 0) {
				List<Risk> risks = new List<Risk> ();
				for (int i = 0; i < allSpecs.Count; i++) {
//					Console.WriteLine (allSpecs [i]);
					var risk = RetrieveCurrentRisk (allSpecs [i], allProblems [i], allSolutions [i]);
					risks.Add (risk);
				}

				List<Risk> sortedRisks = new List<Risk> ();
				switch (this.sortBy) {
				case "Status":
					sortedRisks = risks.OrderBy (q => q.State).ToList ();
					break;
				case "Name":
					sortedRisks = risks.OrderBy (q => q.Name).ToList ();
					break;
				case "Date":
					sortedRisks = risks;
					break;
				case "Author":
					sortedRisks = risks.OrderBy (q => q.Author).ToList ();
					break;
				default:
					sortedRisks = risks;
					break;
				}

				queries.InnerHtml = String.Empty;
				foreach (var r in sortedRisks) {
					queries.InnerHtml += GenerateQueryHtml (r);
//					Console.WriteLine ("risk.State (GenerateQueryHistory): " + r.State.ToString ());
				}
			} else {
				SortDiv.Visible = false;
				statusLabel.Text = "No previous risk cases available.";
			}

			if (Session["CurrentResponseUri"] != null) 
				Session.Remove("CurrentResponseUri");
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
//			string path = "&path=" + responseXmlUri;
			string path = String.Empty;
//			Console.WriteLine ("risk.SimilarCasesFound: " + risk.SimilarCasesFound);
			string search = risk.SimilarCasesFound?path:String.Empty;
			return Tag1 + Tag2 + risk.Id + search + Tag3 + Tag4 + risk.Name + Tag5 + 
				Tag6 + risk.Content + Tag7 + 
				Tag8 + "<b>Status</b>: " + Util.GetEnumDescription(risk.State) + 
				"&nbsp;&nbsp;&nbsp;" + "<b>Author</b>: " + risk.Author + 
				"&nbsp;&nbsp;&nbsp;" + " <b>Last Modified</b>: " + Util.FormatDate (risk.LastEdited) + 
				Tag9 + Tag10 + Tag11 + Tag12;
		}

		protected void itemSelected(object sender, EventArgs e)
		{
			DropDownList dropdown = (DropDownList) sender;  
			Session ["SortBy"] = dropdown.SelectedItem.Text;
//			Console.WriteLine ("SortBy (itemSelected): " + Session ["SortBy"].ToString ());

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
			List<string> allResponsePaths = GetFilesFromDirectoryList(Path.Combine (dirPath, "Responses"), max);
//			List<string> allResponsePaths = GetFilesFromDirectoryList2(dirPath, max);
			var allRequestPaths = GetFilesFromDirectorySortedList3(Path.Combine (dirPath + "Requests"), max);
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
			return GenerateInnerHtmlAdvanced (responseXmlUri, requestId, requestDescr);
        }

		private string GenerateInnerHtmlSimple(string responseXmlUri, string requestId, string requestDescr)
		{
			return LiStartTagMenu +
				aStartTag + "DescribeRisk.aspx?id=" + requestId + "&path=" + responseXmlUri + aMidTag +
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
