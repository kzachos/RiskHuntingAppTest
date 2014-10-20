using System;
using System.IO;
using System.Web;
using System.Web.UI;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace RiskHuntingAppTest
{
	
	public partial class Summary : System.Web.UI.Page
	{
		const string Tag1a = "<li class=\"store\">";
		const string Tag2a = "<a class=\"noeffect\" href=\"javascript:doLoad('EditAction.aspx?id=";
		const string Tag3a = "');\">";
		const string Tag4a = "<span class=\"name\">";
		const string Tag5a = "</span>";
		const string Tag6a = "<span class=\"comment\">";
		const string Tag7a = "</span>";
		const string Tag8a = "<span class=\"starcomment\">";
		const string Tag9a = "</span>";
		const string Tag10a = "<span class=\"arrow\"></span>";
		const string Tag11a = "</a>";
		const string Tag12a = "</li>";

		const string LiStartTagLabel = "<li class=\"label\">";
		const string LiEndTag = "</li>";
		const string StartTag = "<asp:Label";
		const string MidTag = " Width=\"100%\" runat=\"server\">";
		const string EndTag = "</asp:Label>";

		protected const string SOURCESPECIFICATION = "SourceSpecification";
		protected const string PROBLEM = "Problem";
		protected const string SOLUTION = "Solution";
		protected const string ADDITIONAL = "Additional";
		protected const string PROCESSFOLDER = "_toProcess";

		const string defaultProcessGuidance = "Define all elements of the danger in the web form. Ask a colleague to check these elements.";

		protected const string SOURCE_TYPE = Constants.CASEREF;
		protected const string CASE_TYPE = "Risk";

		protected string xmlFilesPath = SettingsTool.GetApplicationPath() + "xmlFiles/";
		protected string requestPath = SettingsTool.GetApplicationPath() + "xmlFiles/Requests/";
		protected string responsePath = SettingsTool.GetApplicationPath() + "xmlFiles/Responses/";
		protected string sourcesPath = SettingsTool.GetApplicationPath() + "xmlFiles/Sources/";
		protected string processPath = SettingsTool.GetApplicationPath() + "xmlFiles/Sources/_toProcess/";

		protected string requestId, responseUri;

		protected int maxId;
		protected string sourceId;
		protected Risk currentRisk;


		protected void Page_Init(object sender, EventArgs e)
		{
			if (Session ["CURRENT_RISK"] != null)
				this.sourceId = Session ["CURRENT_RISK"].ToString ();

			RetrieveCurrentRisk ();

			Topbar_Problem_Search_Solution ();

			PopulateElements ();

			PopulateActionItems ();

		}

		protected void Page_Load(object sender, EventArgs e)
		{


		}

		#region Initializing

		private void Topbar_Problem_Search_Solution ()
		{
			TopbarProblemSearchSolution.Visible = true;
		}

		private void PopulateElements()
		{

			var processGuidanceText = Util.GenerateProcessGuidance ("summary");
			creativeGuidance.InnerText = processGuidanceText.Equals(String.Empty)?defaultProcessGuidance:processGuidanceText;

			sourceDiv.InnerHtml = String.Empty;
			sourceDiv.InnerHtml += LiStartTagLabel +
				GenerateContentHtml ("Risk Name", this.currentRisk.Name) +
				LiEndTag;

			sourceDiv.InnerHtml += LiStartTagLabel +
				GenerateContentHtml ("Risk Description", this.currentRisk.Content) +
				LiEndTag;

			sourceDiv.InnerHtml += LiStartTagLabel +
				GenerateContentHtml ("Risk Author", this.currentRisk.Author) +
				LiEndTag;

			sourceDiv.InnerHtml += LiStartTagLabel +
				GenerateContentHtml ("Risk Location", this.currentRisk.LocationDetail) +
				LiEndTag;

			sourceDiv.InnerHtml += LiStartTagLabel +
				GenerateContentHtml ("Risk Body Parts", this.currentRisk.BodyPart) +
				LiEndTag;

//			RiskName.Text = this.currentRisk.Name;
//			RiskDescription.Text = this.currentRisk.Content;
//			RiskAuthor.Text = this.currentRisk.Author;
//			RiskLocation.Text = this.currentRisk.LocationDetail;
//			RiskBodyParts.Text = this.currentRisk.BodyPart;
		}

		#endregion

		public void PopulateActionItems ()
		{
			divActions.InnerHtml = String.Empty;
			if (this.currentRisk.Actions.Count > 0) {
				for (int i = 0; i < this.currentRisk.Actions.Count; i++) {
					var item = this.currentRisk.Actions [i];
//					if (i == 0)
//						divActions.InnerHtml += GenerateHtml (item);
//					else
						divActions.InnerHtml += GenerateHtml (item);
				}
			}
			else
				divActions.InnerHtml += LiStartTagLabel +
				GenerateContentHtml ("--No actions available--") +
				LiEndTag;
		}

		void GenerateActionList ()
		{
			if (this.currentRisk.Actions.Count > 0) {
				for (int i = 0; i < this.currentRisk.Actions.Count; i++) {
					divActions.InnerHtml += GenerateHtml (this.currentRisk.Actions[i]);
				}
			}
			else
				statusLabel.Text = "No actions available.";

		}

		#region Xml generation

		void RetrieveCurrentRisk ()
		{
			string location = String.Empty;

			location = processPath + "SourceSpecification" + "/" + Constants.CASEREF + this.sourceId + "_" + "SourceSpecification" + ".xml";
			XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(location);

			location = processPath + "Problem" + "/" + Constants.CASEREF + this.sourceId + "_" + "Problem" + ".xml";
			XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Load(location);

			location = processPath + "Solution" + "/" + Constants.CASEREF + this.sourceId + "_" + "Solution" + ".xml";
			XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Load(location);

			this.currentRisk = new Risk (ss, problem, solution);
		}
			

		#endregion

		#region Html related

		private string GenerateContentHtml(string itemType, string itemContent)
		{
			return StartTag + MidTag + itemType + ": " + itemContent + EndTag;
		}

		private string GenerateContentHtml(Action item, int counter)
		{
			return StartTag + MidTag + "ACTION " + ++counter + ": " + item.Content + EndTag;
		}

		private string GenerateContentHtml(string text)
		{
			return StartTag + MidTag + text + EndTag;
		}

		private string GenerateHtml(Action action)
		{
			return Tag1a + Tag2a + action.Id + Tag3a + Tag4a + action.Content + Tag5a + 
				Tag6a + "Owner: " + action.Owner + Tag7a + 
				Tag8a + "Impelement by: " + action.ImplementBy.ToShortDateString() + Tag9a + 
				Tag10a + Tag11a + 
				Tag12a;
		}

		#endregion

		#region Pdf 


		// add a table to the PDF document
		private PdfPTable _stateTableProblem() {
			string[] col = { "Type", "Description" };
			PdfPTable table = new PdfPTable(2);
			/*
			* default table width => 80%
			*/  
			table.WidthPercentage = 100;
			// then set the column's __relative__ widths
			table.SetWidths(new Single[] {1, 5});
			/*
			* by default tables 'collapse' on surrounding elements,
			* so you need to explicitly add spacing
			*/
			table.SpacingBefore = 10;

			for (int i = 0; i < col.Length; ++i) {
				PdfPCell cell = new PdfPCell(new Phrase(col[i]));
				cell.BackgroundColor = new BaseColor(204, 204, 204);
				table.AddCell(cell);
			}

			table.AddCell("Risk Name");
			table.AddCell(this.currentRisk.Name);
			table.AddCell("Risk Description");
			table.AddCell(this.currentRisk.Content);
			table.AddCell("Risk Author");
			table.AddCell(this.currentRisk.Author);
			table.AddCell("Risk Location");
			table.AddCell(this.currentRisk.LocationDetail);
			table.AddCell("Risk Body Parts");
			table.AddCell(this.currentRisk.BodyPart);;

			return table;
		}  

		// add a table to the PDF document
		private PdfPTable _stateTableActions() {
			string[] col = { "Owner", "Description", "Implement By" };
			PdfPTable table = new PdfPTable(3);
			/*
			* default table width => 80%
			*/  
			table.WidthPercentage = 100;
			// then set the column's __relative__ widths
			table.SetWidths(new Single[] {1, 5, 4});
			/*
			* by default tables 'collapse' on surrounding elements,
			* so you need to explicitly add spacing
			*/
			table.SpacingBefore = 10;

			for (int i = 0; i < col.Length; ++i) {
				PdfPCell cell = new PdfPCell(new Phrase(col[i]));
				cell.BackgroundColor = new BaseColor(204, 204, 204);
				table.AddCell(cell);
			}


			if (this.currentRisk.Actions.Count > 0) {
				for (int i = 0; i < this.currentRisk.Actions.Count; i++) {
					table.AddCell(this.currentRisk.Actions[i].Owner);
					table.AddCell(this.currentRisk.Actions[i].Content);
					table.AddCell(this.currentRisk.Actions[i].ImplementBy.ToShortDateString());
				}
			}
			return table;
		}  

		public void BuildPdf() 
		{
			using (MemoryStream myMemoryStream = new MemoryStream ()) {
				Document doc = new Document ();

				var output = new System.IO.FileStream (xmlFilesPath + "test.pdf", 
					            System.IO.FileMode.Create);
				PdfWriter writer = PdfWriter.GetInstance (doc, myMemoryStream);

				doc.Open ();


				Rectangle page = doc.PageSize;
				PdfPTable head = new PdfPTable (1);
				head.TotalWidth = page.Width;
				Phrase phrase = new Phrase (
					               DateTime.UtcNow.ToString ("yyyy-MM-dd HH:mm:ss") + " GMT",
					               new Font (Font.FontFamily.COURIER, 8)
				               );    
				PdfPCell c = new PdfPCell (phrase);
				c.Border = Rectangle.NO_BORDER;
				c.VerticalAlignment = Element.ALIGN_TOP;
				c.HorizontalAlignment = Element.ALIGN_CENTER;
				head.AddCell (c);
				head.WriteSelectedRows (
				// first/last row; -1 writes all rows
					0, -1,
				// left offset
					0,
				// ** bottom** yPos of the table
					page.Height - doc.TopMargin + head.TotalHeight + 20,
					writer.DirectContent 
				);

				// table heading
				Paragraph p = new Paragraph ("Risk Problem");
				p.Alignment = 1;
				doc.Add (p);

				// table data, see code snippet following this one
				doc.Add (_stateTableProblem ());

				Paragraph p2 = new Paragraph ("Risk Actions");
				p2.Alignment = 1;
				doc.Add (p2);
				// table data, see code snippet following this one

				doc.Add (_stateTableActions ());

				doc.Close ();

				byte[] content = myMemoryStream.ToArray();
				HttpContext.Current.Response.Buffer = false;  
				HttpContext.Current.Response.Clear();         
				HttpContext.Current.Response.ClearContent(); 
				HttpContext.Current.Response.ClearHeaders();  
				HttpContext.Current.Response.AppendHeader("content-disposition","attachment;filename=" + "RiskResolution.pdf");                
				HttpContext.Current.Response.ContentType = "Application/pdf";        

				//Write the file content directly to the HTTP content output stream.    
				HttpContext.Current.Response.BinaryWrite(content);         
				HttpContext.Current.Response.Flush();                
				HttpContext.Current.Response.End(); 
			}
		}

		#endregion

		public virtual void submitClicked(object sender, EventArgs args)
		{

			BuildPdf ();


		}
	}
}

