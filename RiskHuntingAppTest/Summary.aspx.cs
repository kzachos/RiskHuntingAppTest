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
		// Set up the fonts to be used on the pages
		private iTextSharp.text.Font _largeFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
		private iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
		private iTextSharp.text.Font _smallFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
		protected string logoPath = Path.Combine (SettingsTool.GetApplicationPath(), "Theme", "images");

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

		const string SpanStartTagMenu = "<span class=menu>";
		const string SpanStartTagName = "<span class=name>";
		const string SpanStartTagArrow = "<span class=arrow>";
		const string SpanEndTag = "</span>";
		const string LiStartTagMenu = "<li class=menu>";
		const string aStartTag = "<a href=\"javascript:doLoad('EditResolutionIdea.aspx?content=";
		const string aMidTag = "');\">";
		const string aEndTag = "</a>";

		protected const string SOURCESPECIFICATION = "SourceSpecification";
		protected const string PROBLEM = "Problem";
		protected const string SOLUTION = "Solution";
		protected const string ADDITIONAL = "Additional";
		protected const string PROCESSFOLDER = "_toProcess";

		const string defaultProcessGuidance = "Define all elements of the danger in the web form. Ask a colleague to check these elements.";

		protected const string SOURCE_TYPE = Constants.CASEREF;
		protected const string CASE_TYPE = "Risk";

		protected string xmlFilesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles");
		protected string requestPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Requests");
		protected string responsePath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Responses");
		protected string sourcesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources");
		protected string processPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources", "_toProcess");

		protected string requestId, responseUri;

		protected int maxId;
		protected string sourceId;
		protected Risk currentRisk;


		protected void Page_Init(object sender, EventArgs e)
		{
			if (Session ["CURRENT_RISK"] != null)
				this.sourceId = Session ["CURRENT_RISK"].ToString ();
			else
				this.sourceId = String.Empty;

			if (File.Exists (Path.Combine (requestPath, "Request_" + this.sourceId + ".xml")))
				Topbar_Problem_Search_Solution ();
			else
				Topbar_Problem_Solution ();

			RetrieveCurrentRisk ();
			PopulateElements ();
			PopulateIdeaItems ();
			PopulateActionItems ();
		

		}

		protected void Page_Load(object sender, EventArgs e)
		{


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
				GenerateContentHtml ("--No actions available as yet--") +
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
				divActions.InnerHtml += LiStartTagLabel +
					GenerateContentHtml ("--No actions available as yet--") +
					LiEndTag;
		}

		void PopulateIdeaItems ()
		{
			divIdeas.InnerHtml = String.Empty;
			if (this.currentRisk.Recommendations.Count > 0) {
				for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
					divIdeas.InnerHtml += GenerateHtml2 (this.currentRisk.Recommendations[i].ToString());
				}
			}
			else
				divIdeas.InnerHtml += LiStartTagLabel +
					GenerateContentHtml ("--No ideas generated as yet--") +
					LiEndTag;
		}

		#region Xml generation

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

		private string GenerateHtml2(string idea)
		{
			return LiStartTagMenu +
				aStartTag + idea + aMidTag +
				SpanStartTagName + idea + SpanEndTag +
				SpanStartTagArrow + SpanEndTag + aEndTag + LiEndTag;
		}
		#endregion

		#region Pdf 


		public void BuildPdf2() 
		{
			using (MemoryStream myMemoryStream = new MemoryStream ()) {
				Document doc = new Document ();

//				var output = new System.IO.FileStream (Path.Combine(xmlFilesPath + "test.pdf"), 
//					            System.IO.FileMode.Create);
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

				Paragraph p2 = new Paragraph ("Resolution Ideas");
				p2.Alignment = 1;
				doc.Add (p2);
				// table data, see code snippet following this one

				doc.Add (_stateTableIdeas ());

				Paragraph p3 = new Paragraph ("Risk Actions");
				p3.Alignment = 1;
				doc.Add (p3);
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

		public void BuildPdf ()
		{
			iTextSharp.text.Document doc = null;
			using (MemoryStream myMemoryStream = new MemoryStream ()) {
				try
				{
					// Initialize the PDF document
					doc = new Document();
					//var output = new System.IO.FileStream(Path.Combine(pdfPath + "test.pdf"),
					//                System.IO.FileMode.Create);
					iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, myMemoryStream);

					// Set the margins and page size
					this.SetStandardPageSize(doc);

					// Add metadata to the document.  This information is visible when viewing the 
					// document properities within Adobe Reader.
					doc.AddTitle("Risk Hunting App");
					doc.AddHeader("title", this.currentRisk.Name);
					//doc.AddHeader("author", "M. Lichtenberg");
					//doc.AddCreator("M. Lichtenberg");
					doc.AddKeywords("risk resolution");

					// Add Xmp metadata to the document.
					//this.CreateXmpMetadata(writer);

					// Open the document for writing content
					doc.Open();

					// Add pages to the document
					this.AddPageWithBasicFormatting(doc);

				}
				catch (iTextSharp.text.DocumentException dex)
				{
					Console.Write(dex.Message);
				}
				finally
				{

					// Clean up
					doc.Close();
					doc = null; 

					byte[] content = myMemoryStream.ToArray();
					HttpContext.Current.Response.Buffer = false;
					HttpContext.Current.Response.Clear();
					HttpContext.Current.Response.ClearContent();
					HttpContext.Current.Response.ClearHeaders();
					//HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + "Case_" + new Random().Next(100).ToString() + ".pdf");
					HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + "RiskResolution.pdf");
					HttpContext.Current.Response.ContentType = "Application/pdf";

					//Write the file content directly to the HTTP content output stream.    
					HttpContext.Current.Response.BinaryWrite(content);
					HttpContext.Current.Response.Flush();
					HttpContext.Current.Response.End();


				}
			}
		}

		/// <summary>
		/// Add the header page to the document.  This shows an example of a page containing
		/// both text and images.  The contents of the page are centered and the text is of
		/// various sizes.
		/// </summary>
		/// <param name="doc"></param>
		private void AddPageWithBasicFormatting(iTextSharp.text.Document doc)
		{


//			// Add a logo
//			String appPath = System.IO.Directory.GetCurrentDirectory();
//			iTextSharp.text.Image logoImage = iTextSharp.text.Image.GetInstance(Path.Combine(logoPath + "CareNshare_small.jpg"));
//			logoImage.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
//			doc.Add(logoImage);
//			logoImage = null;

			// Write page content.  Note the use of fonts and alignment attributes.
			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new iTextSharp.text.Chunk("\n\n"));
			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _largeFont, new Chunk("Risk problem description \n\n"));
			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _smallFont, _stateTableProblem());
			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new Chunk("\n"));

			// Write page content.  Note the use of fonts and alignment attributes.
			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new iTextSharp.text.Chunk("\n\n"));
			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _largeFont, new Chunk("Resolution ideas \n\n"));
			// Create an unordered bullet list.  The 10f argument separates the bullet from the text by 10 points
			iTextSharp.text.List listIdeas = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED, 10f);
			listIdeas.SetListSymbol("\u2022");   // Set the bullet symbol (without this a hypen starts each list item)
			listIdeas.IndentationLeft = 20f;     // Indent the list 20 points
			if (this.currentRisk.Recommendations.Count > 0) {
				for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
					listIdeas.Add(new iTextSharp.text.ListItem(this.currentRisk.Recommendations[i].ToString(), _standardFont));
				}
			}
			doc.Add(listIdeas);  // Add the list to the page
			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new Chunk("\n"));

			// Write page content.  Note the use of fonts and alignment attributes.
			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new iTextSharp.text.Chunk("\n\n"));
			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _largeFont, new Chunk("Risk actions \n\n"));
			// Create an unordered bullet list.  The 10f argument separates the bullet from the text by 10 points
			iTextSharp.text.List listActions = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED, 10f);
			listActions.SetListSymbol("\u2022");   // Set the bullet symbol (without this a hypen starts each list item)
			listActions.IndentationLeft = 20f;     // Indent the list 20 points
			if (this.currentRisk.Actions.Count > 0) {
				for (int i = 0; i < this.currentRisk.Actions.Count; i++) {
					listActions.Add(new iTextSharp.text.ListItem(this.currentRisk.Actions[i].Content, _standardFont));
					var list = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED, 10f);
					list.IndentationLeft = 10f;
					list.Add("Owner: " + new iTextSharp.text.ListItem(this.currentRisk.Actions[i].Owner, _smallFont));
					list.Add("Implement by: " + new iTextSharp.text.ListItem(this.currentRisk.Actions[i].ImplementBy.ToShortDateString(), _smallFont));
					listActions.Add (list);
				}
			}
			doc.Add(listActions);  // Add the list to the page

			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new Chunk("\n\n\n\n"));
			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _smallFont, new Chunk("Generated " +
				DateTime.Now.Day.ToString() + " " +
				System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month) + " " +
				DateTime.Now.Year.ToString() + " " +
				DateTime.Now.ToShortTimeString()));
		}



		/// <summary>
		/// Set margins and page size for the document
		/// </summary>
		/// <param name="doc"></param>
		private void SetStandardPageSize(iTextSharp.text.Document doc)
		{
			// Set margins and page size for the document
			doc.SetMargins(50, 50, 50, 50);
			// There are a huge number of possible page sizes, including such sizes as
			// EXECUTIVE, POSTCARD, LEDGER, LEGAL, LETTER_LANDSCAPE, and NOTE
			doc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.LETTER.Width,
				iTextSharp.text.PageSize.LETTER.Height));
		}

		/// <summary>
		/// Add a paragraph object containing the specified element to the PDF document.
		/// </summary>
		/// <param name="doc">Document to which to add the paragraph.</param>
		/// <param name="alignment">Alignment of the paragraph.</param>
		/// <param name="font">Font to assign to the paragraph.</param>
		/// <param name="content">Object that is the content of the paragraph.</param>
		private void AddParagraph(Document doc, int alignment, iTextSharp.text.Font font, iTextSharp.text.IElement content)
		{
			Paragraph paragraph = new Paragraph();
			paragraph.SetLeading(0f, 1.2f);
			paragraph.Alignment = alignment;
			paragraph.Font = font;
			paragraph.Add(content);
			doc.Add(paragraph);
		}

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
			table.AddCell(this.currentRisk.BodyPart);

			return table;
		}  

		// add a table to the PDF document
		private PdfPTable _stateTableIdeas() {
			string[] col = { "Description"};
			PdfPTable table = new PdfPTable(1);
			/*
			* default table width => 80%
			*/  
			table.WidthPercentage = 100;
			// then set the column's __relative__ widths
			table.SetWidths(new Single[] {10});
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


			if (this.currentRisk.Recommendations.Count > 0) {
				for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
					table.AddCell(this.currentRisk.Recommendations[i].ToString());
				}
			}
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

		#endregion

		public virtual void submitClicked(object sender, EventArgs args)
		{

			BuildPdf ();


		}
	}
}

