
using System;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

namespace RiskHuntingAppTest
{


	public partial class DisplayResponse_Description : System.Web.UI.Page
	{
		const string SpanStartTagHeader = "<span class=header>";
		const string SpanEndTag = "</span>";
		
		const string LiStartTagTextbox = "<li class=textbox>";
		const string LiEndTag = "</li>";
		
		const string pStartTag = "<p>";
		const string pEndTag = "</p>";
		
		const string DivStartTagTitle = "<div class=title>";
		const string DivStartTagLeftnav = "<div class=leftnav>";
		const string DivStartTagRightnav = "<div class=rightnav>";
		const string DivEndTag = "</div>";

        const string aStartTag = "<a href=\"javascript:doLoad('";
        const string aMidTag = "');\">";
		const string aEndTag = "</a>";
		
		protected void Page_Init(object sender, EventArgs e)
		{
			string id = DetermineID ();
			Console.WriteLine ("id: " + id);
			Console.WriteLine ("responseUri: " + DetermineResponseUri());
			string responseUri = DetermineResponseUri();

			if (!responseUri.Equals(String.Empty))
			{
				XmlProc.ResponseSerialized.MatchedSources response = XmlProc.ObjectXMLSerializer<XmlProc.ResponseSerialized.MatchedSources>.Load(responseUri);

				List<XmlProc.ResponseSerialized.MatchedSourcesMatchedSource> matchedSources = (List<XmlProc.ResponseSerialized.MatchedSourcesMatchedSource>)response.MatchedSource;

				foreach (XmlProc.ResponseSerialized.MatchedSourcesMatchedSource matchedSource in matchedSources)
				{
					if (matchedSource.SourceId == id)
					{
						//                        currentIndex = matchedSourcesIds.IndexOfValue(id);
						//SortedList elements = SeperateStringByChar(matchedSource.Content);

//						title.InnerHtml = String.Empty;

						string temp = "Test";
						RiskDanger.Text = Util.ExtractAttributeContentFromString(matchedSource.Content, "InjuryNature");
						RiskLocation.Text = Util.ExtractAttributeContentFromString(matchedSource.Content, "LocationDetail");
//						RiskRisk.Text = Util.ExtractAttributeContentFromString(matchedSource.Content, "RootCause");
						RiskDescription.Text = Util.ExtractAttributeContentFromString(matchedSource.Content, "Content");
						RiskBodyParts.Text = Util.ExtractAttributeContentFromString(matchedSource.Content, "BodyPart");
						RiskName.Text = RiskDescription.Text.ExtractKeywords ().TruncateAtWord (10);

						RetrieveNLData ();

						break;

					}
				}
				Topbar_Problem_Search_Solution ();
			}

		}
		
        protected void Page_Load(object sender, EventArgs e)
        {

			
        }

		#region Initializing

		private void Topbar_Problem_Search_Solution ()
		{
			TopbarProblemSearchSolution.Visible = true;
		}

		#endregion

		#region Service Call
		private void RetrieveNLData()
		{
			var watch = Stopwatch.StartNew();


			try {
				RiskHuntingAppTest.antique.AntiqueService antique = new RiskHuntingAppTest.antique.AntiqueService ();
				antique.NLParserCompleted += new RiskHuntingAppTest.antique.NLParserCompletedEventHandler (objAntique_NLParserCompleted);
				antique.NLParserAsync (RiskDescription.Text);
			}
			finally {


				watch.Stop ();
				// Get the elapsed time as a TimeSpan value.
				TimeSpan ts = watch.Elapsed;
				// Format and display the TimeSpan value.
				string elapsedTime = String.Format ("{0:00}:{1:00}:{2:00}.{3:00}",
					ts.Hours, ts.Minutes, ts.Seconds,
					ts.Milliseconds / 10);
				Console.WriteLine (elapsedTime, "RunTime");
			}

		}

		void objAntique_NLParserCompleted(object sender, 
			RiskHuntingAppTest.antique.NLParserCompletedEventArgs e)
		{
			Console.WriteLine (e.Result);
			var NLResponse = Util.DeserializeNLResponse (e.Result);


//			foreach(var item in NLResponse)
//			{
//				if (!item.TermValue.Equals (String.Empty))
//					NLResponseTrimmed.Add (item);
//				Console.WriteLine("id: {0}, name: {1}",item.Pos,item.TermValue);
//			}
//			if (NLResponseTrimmed.Count > 5)
//			for(var item in NLResponseTrimmed)
//			{
//				Console.WriteLine("id: {0}, name: {1}",item.Pos,item.TermValue);
//			}
			Session ["CURRENT_RISK_DESC"] = NLResponse;

		}

		#endregion

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

        private SortedList<int, string> CreateMatchedSourcesList(List<XmlProc.ResponseSerialized.MatchedSourcesMatchedSource> matchedSources, int max)
        {
            SortedList<int, string> matchedSourcesIds = new SortedList<int, string>();
            if (matchedSources.Count - 1 < max)
                max = matchedSources.Count - 1;
            for (int i = 0; i <= max; i++)
            {
                XmlProc.ResponseSerialized.MatchedSourcesMatchedSource matchedSource = (XmlProc.ResponseSerialized.MatchedSourcesMatchedSource)matchedSources[i];
                matchedSourcesIds.Add(i, matchedSource.SourceId);
            }
            return matchedSourcesIds;
        }

		public virtual void submitClicked(object sender, EventArgs args)
		{

			Response.Redirect("DisplayResponse_ResolutionsApplied.aspx");

		}

        #region Html related

        #endregion
    }
}

