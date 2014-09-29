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
using System.Collections.Generic;

namespace RiskHuntingAppTest
{
    public partial class DeleteQueries : System.Web.UI.Page
    {
        protected string xmlFilesPath = SettingsTool.GetApplicationPath() + "SearchApp/xmlFiles/";
        protected string requestPath = SettingsTool.GetApplicationPath() + "SearchApp/xmlFiles/Requests/";
        protected string responsePath = SettingsTool.GetApplicationPath() + "SearchApp/xmlFiles/Responses/";

        const string SpanStartTagName = "<span class=name>";
        const string SpanEndTag = "</span>";
        const string InputStartTag = "<input type=checkbox runat=server name=";
        const string InputMidTag = " id=";
        const string InputEndTag = " />";

        const string LiStartTag = "<li class=checkbox>";
        const string LiEndTag = "</li>";

        const string aStartTag = "<a href=\"javascript:doLoad('";
        const string aMidTag = "');\">";
        const string aEndTag = "</a>";

        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateQueryHistory(xmlFilesPath, 10);
        }


        private void GenerateQueryHistory(string dirPath, int max)
        {
            List<string> allResponsePaths = GetFilesFromDirectoryList(dirPath + "Responses/", max);
            SortedList allRequestPaths = GetFilesFromDirectorySortedList(dirPath + "Requests/", max);

            NLP.StringProc str = new NLP.StringProc();
            char[] deliminator = new char[] { '_' };
            char[] deliminator2 = new char[] { '.' };
            SortedList fileNameParts = new SortedList();
            SortedList fileNameParts2 = new SortedList();
            string requestId, requestPath;
            foreach (string responsePath in allResponsePaths)
            {
                fileNameParts = str.SeperateStringByChar(responsePath, deliminator);
                fileNameParts2 = str.SeperateStringByChar(fileNameParts[1].ToString(), deliminator2);
                requestId = fileNameParts2[0].ToString();
                requestPath = (string)allRequestPaths[requestId.ToString()];

                XmlProc.RequestSerialized.Request request = XmlProc.ObjectXMLSerializer<XmlProc.RequestSerialized.Request>.Load(requestPath);
                XmlProc.RequestSerialized.RequestTarget target = (XmlProc.RequestSerialized.RequestTarget)request.Target[0];

                queries.InnerHtml += GenerateQueryHtml(responsePath, requestId, target.TargetDescription);
            }
        }

        private List<string> GetFilesFromDirectoryList(string dirPath, int max)
        {
            List<string> all = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            FileInfo[] FileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
            if (FileList.Length - 1 < max)
                max = FileList.Length - 1;
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

        private SortedList GetFilesFromDirectorySortedList(string dirPath, int max)
        {
            SortedList all = new SortedList();
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            FileInfo[] FileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
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
                fileNameParts = str.SeperateStringByChar(FI.FullName, deliminator);
                fileNameParts2 = str.SeperateStringByChar(fileNameParts[1].ToString(), deliminator2);
                all.Add(fileNameParts2[0].ToString(), FI.FullName);
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
            for (int i = 0; i < fileParts.Count - 1; i++)
                responseXmlUri += fileParts[i].ToString() + "\\\\";
            responseXmlUri += fileParts[fileParts.Count - 1].ToString();
            if (Session["CurrentResponseUri"] != null) Session.Remove("CurrentResponseUri");
            return LiStartTag +
                SpanStartTagName + requestId + ": " + requestDescr + SpanEndTag +
                InputStartTag + requestId + InputMidTag + requestId + InputEndTag + LiEndTag;
        }

        protected void submitClicked(object sender, EventArgs e)
        {
            //List<string> allResponsePaths = GetFilesFromDirectoryList(dirPath + "Responses/", max);
            //SortedList allRequestPaths = GetFilesFromDirectorySortedList(dirPath + "Requests/", max);

            //NLP.StringProc str = new NLP.StringProc();
            //char[] deliminator = new char[] { '_' };
            //char[] deliminator2 = new char[] { '.' };
            //SortedList fileNameParts = new SortedList();
            //SortedList fileNameParts2 = new SortedList();
            //string requestId, requestPath;
            //foreach (string responsePath in allResponsePaths)
            //{
            //    fileNameParts = str.SeperateStringByChar(responsePath, deliminator);
            //    fileNameParts2 = str.SeperateStringByChar(fileNameParts[1].ToString(), deliminator2);
            //    requestId = fileNameParts2[0].ToString();
            //    requestPath = (string)allRequestPaths[requestId.ToString()];

                //if (synonym.Checked)

                
            }
        }
    }

