
using System;
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


	public partial class SearchResults : System.Web.UI.Page
	{
		protected const float THRESHOLD = 30.0f;

		const string Tag1 = "<li class=\"store\">";
		const string Tag2 = "<a class=\"noeffect\" href=\"javascript:doLoad('DisplayResponse_Description.aspx?id=";
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


		//		<li class="store">
		//			<a class="noeffect" href="javascript:doLoad('DisplayResponse_Description.aspx?id=1000');">
		//			<!--<span class="image" style="background-image: url('http://a1.phobos.apple.com/us/r1000/002/Music/0e/f0/26/mzi.bxbwyvvz.170x170-75.jpg')"></span>-->
		//			<span class="name">Uneven Floor</span>
		//			<span class="comment">During change of shift, the hallway is very crowded and the floor is not so visible. People trip often because they don’t see the holes or uneven surfaces. </span>
		//		<!--<span class="stars5"></span>-->
		//			<span class="starcomment">March 15th 2013, Basilldon (UK)</span>
		//		<span class="arrow"></span>
		//			</a>
		//			</li>

		protected void Page_Load(object sender, EventArgs e)
		{

			//            if (!Page.IsPostBack)
			//            {
			hide.Visible = false;
			string responseUri = DetermineResponseUri();
			if (!responseUri.Equals(String.Empty))
			{
				Session.Remove ("CURRENT_RISK_DESC");
				Session.Remove ("CREATIVITY_PROMPTS");
				int max;
				if (Session["MaxResults"] != null)
					max = Convert.ToInt32(Session["MaxResults"]);
				else
					max = 5;
				GenerateMatchedSources(responseUri, max - 1);
			}
			else
			{
				statusLabel.Text = "Submit a query";
			}
			//            }
			Topbar_Problem_Search_Solution ();
		}

		#region Initializing

		private void Topbar_Problem_Search_Solution ()
		{
			TopbarProblemSearchSolution.Visible = true;
		}

		#endregion

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

		private void GenerateMatchedSources(string responseUri, int max)
		{
			XmlProc.ResponseSerialized.MatchedSources response = XmlProc.ObjectXMLSerializer<XmlProc.ResponseSerialized.MatchedSources>.Load(responseUri);

			List<XmlProc.ResponseSerialized.MatchedSourcesMatchedSource> matchedSources = (List<XmlProc.ResponseSerialized.MatchedSourcesMatchedSource>)response.MatchedSource;
			if (matchedSources.Count > 0)
			{
				if (matchedSources.Count-1 < max)
					max = matchedSources.Count-1;

				for (int i = 0; i <= max; i++)
				{
					XmlProc.ResponseSerialized.MatchedSourcesMatchedSource matchedSource = matchedSources[i];
					if (Convert.ToDouble (matchedSource.OverallMatchValue) >= THRESHOLD)
						responses.InnerHtml += GenerateMatchedSourceHtml(matchedSource);
				}
				Console.WriteLine (responses.InnerHtml);
				if (responses.InnerHtml.Trim().Equals (String.Empty))
					statusLabel.Text = "No cases have been found.";
			}
			else
				statusLabel.Text = "No cases have been found.";
		}

		private string GenerateMatchedSourceHtml(XmlProc.ResponseSerialized.MatchedSourcesMatchedSource matchedSource)
		{
			string riskName = Util.ExtractAttributeContentFromString (matchedSource.Content, "Content").ExtractKeywords ().TruncateAtWord (10);
			return Tag1 + Tag2 + matchedSource.SourceId + Tag3 + Tag4 + riskName + Tag5 + Tag6 +
				Util.ExtractAttributeContentFromString (matchedSource.Content, "Content") + Tag7 + Tag8 + DATEANDLOCATION + Tag9 + Tag10 + Tag11 + Tag12;
		}


	}
}

