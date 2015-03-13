using System;
using System.IO;
using System.Web;
using System.Web.UI;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;


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
		const string aStartTag = "<a href=\"javascript:doLoad('EditResolutionIdea.aspx?from=Summary&content=";
		const string aMidTag = "');\">";
		const string aEndTag = "</a>";


		const string defaultProcessGuidance = "Define all elements of the danger in the web form. Ask a colleague to check these elements.";

		protected string xmlFilesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles");
		protected string requestPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Requests");
		protected string responsePath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Responses");
		protected string sourcesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources");
		protected string processPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles", "Sources", "_toProcess");
		static string resourcesPath = Path.Combine (SettingsTool.GetApplicationPath(), "Resources");

		protected string requestId, responseUri;

		protected int maxId;
		protected string sourceId;
		protected Risk currentRisk;


		protected void Page_Init(object sender, EventArgs e)
		{
			alert_message_success.Visible = false;
			alert_message_error.Visible = false;
			Util.AccessLog(Util.ScreenType.Summary);
			if (Sessions.RiskState != String.Empty)
				this.sourceId = Sessions.RiskState;
			else
				this.sourceId = String.Empty;

			RetrieveCurrentRisk ();
			PopulateElements ();
			PopulateIdeaItems ();
//			PopulateActionItems ();
		

		}

		protected void Page_Load(object sender, EventArgs e)
		{


		}

		#region Initializing
	

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
				GenerateContentHtml ("Author", this.currentRisk.Author) +
				LiEndTag;

			sourceDiv.InnerHtml += LiStartTagLabel +
				GenerateContentHtml ("Risk Location", this.currentRisk.LocationDetail) +
				LiEndTag;

			sourceDiv.InnerHtml += LiStartTagLabel +
				GenerateContentHtml ("Body Part at Risk", this.currentRisk.BodyPart) +
				LiEndTag;

			sourceDiv.InnerHtml += LiStartTagLabel +
				GenerateContentHtml ("Injury Category", this.currentRisk.InjuryNature) +
				LiEndTag;

			sourceDiv.InnerHtml += LiStartTagLabel +
				GenerateContentHtml ("Date of Incident", this.currentRisk.DateIncidentOccurred.ToShortDateString()) +
				LiEndTag;

//			RiskName.Text = this.currentRisk.Name;
//			RiskDescription.Text = this.currentRisk.Content;
//			RiskAuthor.Text = this.currentRisk.Author;
//			RiskLocation.Text = this.currentRisk.LocationDetail;
//			RiskBodyParts.Text = this.currentRisk.BodyPart;
		}

		#endregion

		#region Actions
//		public void PopulateActionItems ()
//		{
//			divActions.InnerHtml = String.Empty;
//			if (this.currentRisk.Actions.Count > 0) {
//				for (int i = 0; i < this.currentRisk.Actions.Count; i++) {
//					var item = this.currentRisk.Actions [i];
////					if (i == 0)
////						divActions.InnerHtml += GenerateHtml (item);
////					else
//						divActions.InnerHtml += GenerateHtml (item);
//				}
//			}
//			else
//				divActions.InnerHtml += LiStartTagLabel +
//				GenerateContentHtml ("--No actions available as yet--") +
//				LiEndTag;
//		}
//
//		void GenerateActionList ()
//		{
//			if (this.currentRisk.Actions.Count > 0) {
//				for (int i = 0; i < this.currentRisk.Actions.Count; i++) {
//					divActions.InnerHtml += GenerateHtml (this.currentRisk.Actions[i]);
//				}
//			}
//			else
//				divActions.InnerHtml += LiStartTagLabel +
//					GenerateContentHtml ("--No actions available as yet--") +
//					LiEndTag;
//		}

		#endregion

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
					GenerateContentHtml ("No ideas available") +
					LiEndTag;
		}

		#region Xml generation

		void RetrieveCurrentRisk ()
		{
			if (!this.sourceId.Equals(String.Empty))
			{
				string location = String.Empty;

				location = Path.Combine (processPath, "SourceSpecification", Constants.CASEREF + this.sourceId + "_" + "SourceSpecification" + ".xml");
				XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(location);

				location = Path.Combine (processPath, "Problem", Constants.CASEREF + this.sourceId + "_" + "Problem" + ".xml");
				XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Load(location);

				location = Path.Combine (processPath, "Solution", Constants.CASEREF + this.sourceId + "_" + "Solution" + ".xml");
				XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Load(location);

				this.currentRisk = new Risk (ss, problem, solution);
			} else {
				Response.Redirect ("DescribeRisk.aspx?pb=" + Constants.SESSION_EXPIRED_LABEL);
			}
		}
			
		private bool GenerateXml(string componentType)
		{
			bool success = false;
			string Ref;
			string xmlUri, xmlUri2;
			if (componentType.Equals("SourceSpecification"))
			{
				XmlProc.SourceSpecificationSerialized.SourceSpecification ss = Util.CreateSourceSpecificationXml(this.currentRisk);
				//				Console.WriteLine ("this.sourceId (GenerateXml): " + this.s[Sessions.SortState]String ());
				Ref = Constants.SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
				xmlUri = Path.Combine (sourcesPath, Constants.CASE_TYPE, Constants.SOURCESPECIFICATION, Ref);
				xmlUri2 = Path.Combine (sourcesPath, Constants.PROCESSFOLDER, Constants.SOURCESPECIFICATION, Ref);
				XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Save(ss, xmlUri);
//				if (File.Exists(xmlUri2))
//					File.Delete (xmlUri2);
				XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Save(ss, xmlUri2);

				var output = UpdateCase (xmlUri, Constants.SOURCESPECIFICATION, "<SourceSpecification> ");
				if (output.StartsWith ("Stored document", StringComparison.Ordinal))
					success = true;
			}
			else if (componentType.Equals("Problem"))
			{
				XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = Util.CreateProblemXml(this.currentRisk);
				Ref = Constants.SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
				xmlUri = Path.Combine (sourcesPath, Constants.CASE_TYPE, Constants.PROBLEM, Ref);
				xmlUri2 = Path.Combine (sourcesPath, Constants.PROCESSFOLDER, Constants.PROBLEM, Ref);
				XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Save(problem, xmlUri);
//				if (File.Exists(xmlUri2))
//					File.Delete (xmlUri2);
				XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Save(problem, xmlUri2);

				var output = UpdateCase (xmlUri, Constants.PROBLEM, "<LanguageSpecificSpecification> ");
				if (output.StartsWith ("Stored document", StringComparison.Ordinal))
					success = true;
			}
			else if (componentType.Equals("Solution"))
			{
				XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = Util.CreateSolutionXml(this.currentRisk);
				Ref = Constants.SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
				xmlUri = Path.Combine (sourcesPath, Constants.CASE_TYPE, Constants.SOLUTION, Ref);
				xmlUri2 = Path.Combine (sourcesPath, Constants.PROCESSFOLDER, Constants.SOLUTION, Ref);
				XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri);
//				if (File.Exists(xmlUri2))
//					File.Delete (xmlUri2);
				XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri2);

				var output = UpdateCase (xmlUri, Constants.SOLUTION, "<LanguageSpecificSpecification> ");
				if (output.StartsWith ("Stored document", StringComparison.Ordinal))
					success = true;
			}
			return success;
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



		private string UpdateCase(string xmlUri, string componentType, string firstLine)
		{
			string errorMsg;
			if (Util.ServiceExists (Constants.ANTIQUE_URI, false, out errorMsg)) {
				FileStream cgStream = new FileStream (xmlUri, FileMode.Open, FileAccess.Read);
				var xmlContent = String.Empty;
				using (StreamReader cgStreamReader = new StreamReader (cgStream)) {
					string headerLine = cgStreamReader.ReadLine ();
					headerLine = cgStreamReader.ReadLine ();
					string line;
					while ((line = cgStreamReader.ReadLine ()) != null) {
						xmlContent += line;

					}
				}
				cgStream.Close ();
				xmlContent = firstLine + xmlContent;
				RiskHuntingAppTest.eddieService.EDDiEWebService eddie = new RiskHuntingAppTest.eddieService.EDDiEWebService ();
				var output = eddie.UpdateCase (xmlContent, componentType, this.sourceId);
				Console.WriteLine (output);
				return output;
			}
			else
			{
				return String.Empty;
			}
		}

		public virtual void submitClicked(object sender, EventArgs args)
		{
			if (Page.IsValid) {
				Util.AccessLog(Util.ScreenType.Summary, Util.FeatureType.Summary_SubmitCaseButton);


				var success1 = true;
				var success2 = true;
				var success3 = true;

				success1 = GenerateXml(Constants.SOURCESPECIFICATION);
				success2 = GenerateXml(Constants.PROBLEM);
				success3 = GenerateXml(Constants.SOLUTION);


//				string pb = String.Empty;
				if (success1 && success2 && success3) {
//					pb = "success";
					alert_message_success.Visible = true;
					successMessage.InnerText = "Thank you for your safty imput. Your risk has been successfully submitted and uploaded to the database";
					alert_message_error.Visible = false;
					GeneratePDF (false);
					SendEmail (true);
				} else {
//					pb = "nosuccess";
					alert_message_success.Visible = false;
					errorMessage.InnerText = "Your risk could not be uploaded to the database. Please try again later.";
					alert_message_error.Visible = true;
				}
//				Response.Redirect ("DescribeRisk.aspx?pb=" + pb);
			}

		}

		public virtual void exportClicked (object sender, EventArgs args)
		{
			Util.AccessLog(Util.ScreenType.Summary, Util.FeatureType.Summary_GenerateReportButton);

//			BuildPdf ();

			GeneratePDF (false);
			SendEmail (false);
			GeneratePDF (true);

//			Response.Redirect ("Summary.aspx");

		}

		public virtual void createNewClicked (object sender, EventArgs args)
		{
			Util.AccessLog(Util.ScreenType.Summary, Util.FeatureType.Summary_CreateNewRiskButton);

			Session.RemoveAll ();
			Response.Redirect ("DescribeRisk.aspx");
		}
			

		private void SendEmail(bool submission)
		{
			string from = "creativecareconsultancy@gmail.com"; //Replace this with your own correct Gmail Address

			string to = "kzachos@gmail.com"; //Replace this with the Email Address to whom you want to send the mail
//			string to = "derek.read@cnhind.com"; //Replace this with the Email Address to whom you want to send the mail

			MailMessage mail = new MailMessage();
			mail.To.Add(to);
			mail.From = new MailAddress(from, "RiskHunting", System.Text.Encoding.UTF8);
			mail.Subject = submission?"new risk submitted":"risk report generated";
			mail.SubjectEncoding = System.Text.Encoding.UTF8;
			var initialText = submission ? "A new risk has been submitted: <p>" : "A risk report has been generated: <p>";
			mail.Body = "Risk Name: " + this.currentRisk.Name + "<p>"
				+ "Risk Description: " + this.currentRisk.Content + "<p>"
				+ "Author: " + this.currentRisk.Author + "<p>"
				+ "Risk Location: " + this.currentRisk.LocationDetail + "<p>"
				+ "Body Part at Risk: " + this.currentRisk.BodyPart + "<p>"
				+ "Injury Category: " + this.currentRisk.InjuryNature + "<p>"
				+ "Date of Incident: " + this.currentRisk.DateIncidentOccurred.ToShortDateString() + "<p>"
				+ "Recommendation(s): ";
			if (this.currentRisk.Recommendations.Count > 0) {
				for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
					mail.Body += this.currentRisk.Recommendations [i].ToString () + "; ";
				}
			} else
				mail.Body += "n/A";
			mail.BodyEncoding = System.Text.Encoding.UTF8;
			mail.IsBodyHtml = true;
			mail.Priority = MailPriority.Normal;
			System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
			mail.Attachments.Add (new Attachment (Path.Combine (resourcesPath, Constants.PDF_OUTPUT), ct));

			SmtpClient client = new SmtpClient();
			//Add the Creddentials- use your own email id and password

			client.Credentials = new System.Net.NetworkCredential(from, "gCr34t1v3C4r3");

			client.Port = 587; // Gmail works on this port
			client.Host = "smtp.gmail.com";
			client.EnableSsl = true; //Gmail works on Server Secured Layer
			ServicePointManager.ServerCertificateValidationCallback = 
				delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
			{ return true; };
			try
			{
				client.Send(mail);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				string error = string.Empty;
				while (ex2 != null)
				{
					error += ex2.ToString();
					ex2 = ex2.InnerException;
				}
				Console.WriteLine(error);
			} // end try 
			finally 
			{
				mail.Attachments.Dispose ();
			}

		}

		#region PDF

		void GeneratePDF ()
		{
			PdfReader pdfReader = null;
			PdfStamper pdfStamper = null;

			using (MemoryStream myMemoryStream = new MemoryStream ()) {
				try
				{
					string pdfTemplate = Path.Combine (resourcesPath, "Risk.pdf"); 
					FileStream pdfOutputFile = new FileStream (pdfTemplate, FileMode.OpenOrCreate, 
						FileAccess.ReadWrite, 
						FileShare.Read);
					pdfReader = new PdfReader (Path.Combine (resourcesPath, Constants.PDF_TEMPLATE));
					pdfStamper = new PdfStamper (pdfReader, myMemoryStream);
					AcroFields testForm = pdfStamper.AcroFields;

					testForm.SetField ("Name", this.currentRisk.Author);
					testForm.SetField ("TimeDate", this.currentRisk.DateIncidentOccurred.ToShortDateString ());
					testForm.SetField ("Location", this.currentRisk.LocationDetail);
					testForm.SetField ("Description", this.currentRisk.Content);
					string rec = String.Empty;
					if (this.currentRisk.Recommendations.Count > 0) {
						for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
							rec += this.currentRisk.Recommendations [i].ToString () + "; ";
						}
					} else
						rec = "n/A";
					testForm.SetField ("Recommendation", rec);
//					testForm.SetField ("UnsafeAct", "Yes");

					if (!this.currentRisk.ImageUri.Equals (String.Empty)) {
						Image instanceImg = Image.GetInstance (this.currentRisk.ImageUri);
						PdfContentByte overContent = pdfStamper.GetOverContent (1);
						var iArea = testForm.GetFieldPositions ("Image");
						var imageRect = iArea [0].position;
						instanceImg.ScaleToFit (imageRect);
						instanceImg.SetAbsolutePosition (imageRect.Left, imageRect.Bottom);
						//			instanceImg.SetAbsolutePosition(imageRect.Top - instanceImg.ScaledWidth + (imageRect.Width - instanceImg.ScaledWidth) / 2, imageRect.Bottom + (imageRect.Height - instanceImg.ScaledHeight) / 2);
						overContent.AddImage (instanceImg);
					}

				}
				catch (iTextSharp.text.DocumentException dex)
				{
					Console.Write(dex.Message);
				}
				finally
				{


					string pdfOutputUri = Path.Combine (resourcesPath, "Risk.pdf"); 
					Response.TransmitFile(pdfOutputUri);

					SendEmail (false);

					pdfStamper.FormFlattening = true;
					pdfStamper.Close ();
					pdfReader.Close ();



					byte[] content = myMemoryStream.ToArray();
					HttpContext.Current.Response.Buffer = false;
					HttpContext.Current.Response.Clear();
					HttpContext.Current.Response.ClearContent();
					HttpContext.Current.Response.ClearHeaders();
					//HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + "Case_" + new Random().Next(100).ToString() + ".pdf");
					HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + "Risk.pdf");
					HttpContext.Current.Response.ContentType = "Application/pdf";

					//Write the file content directly to the HTTP content output stream.    
					HttpContext.Current.Response.BinaryWrite(content);
					HttpContext.Current.Response.Flush();
					HttpContext.Current.Response.End();


				}

			}

			//			// Get the form fields for this PDF and fill them in!
			//			var formFieldMap = PDFHelper.GetFormFieldNames(pdfTemplate);
			//			formFieldMap["Name"] = this.currentRisk.Author;
			//			formFieldMap["TimeDate"] = this.currentRisk.DateIncidentOccurred.ToShortDateString();
			//			formFieldMap["Location"] = this.currentRisk.LocationDetail;
			//			formFieldMap["Description2"] = this.currentRisk.Content;
			//			string rec = String.Empty;
			//			if (this.currentRisk.Recommendations.Count > 0) {
			//				for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
			//					rec += this.currentRisk.Recommendations[i].ToString() + "; ";
			//				}
			//			}
			//			else
			//				rec = "n/A";
			//			formFieldMap["Recommendation"] = rec;
			//			formFieldMap["UnsafeAct"] = "Yes";
			//
			//
			//
			//			var pdfContents = PDFHelper.GeneratePDF(Path.Combine (resourcesPath, Constants.PDFTEMPLATE), formFieldMap);
			//			PDFHelper.ReturnPDF(pdfContents, "Completed-W9.pdf");
		}

		PdfStamper CreateFormElements (PdfReader pdfReader, Stream stream)
		{
			var pdfStamper = new PdfStamper (pdfReader, stream);
			AcroFields form = pdfStamper.AcroFields;

			form.SetField ("Name", this.currentRisk.Author);
			form.SetField ("TimeDate", this.currentRisk.DateIncidentOccurred.ToShortDateString ());
			form.SetField ("Location", this.currentRisk.LocationDetail);
			form.SetField ("Description", this.currentRisk.Content);
			string rec = String.Empty;
			if (this.currentRisk.Recommendations.Count > 0) {
				for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
					rec += this.currentRisk.Recommendations [i].ToString () + "; ";
				}
			} else
				rec = "n/A";
			form.SetField ("Recommendation", rec);
//			form.SetField ("UnsafeAct", "Yes");

			if (!this.currentRisk.ImageUri.Equals (String.Empty)) {
				Image instanceImg = Image.GetInstance (this.currentRisk.ImageUri);
				PdfContentByte overContent = pdfStamper.GetOverContent (1);
				var iArea = form.GetFieldPositions ("Image");
				var imageRect = iArea [0].position;
				instanceImg.ScaleToFit (imageRect);
				instanceImg.SetAbsolutePosition (imageRect.Left, imageRect.Bottom);
				//			instanceImg.SetAbsolutePosition(imageRect.Top - instanceImg.ScaledWidth + (imageRect.Width - instanceImg.ScaledWidth) / 2, imageRect.Bottom + (imageRect.Height - instanceImg.ScaledHeight) / 2);
				overContent.AddImage (instanceImg);
			}
			return pdfStamper;
		}

		void GeneratePDF (bool toStream)
		{
			PdfReader pdfReader = null;
			PdfStamper pdfStamper = null;

			if (toStream) {
				MemoryStream myMemoryStream = new MemoryStream ();
				pdfReader = new PdfReader (Path.Combine (resourcesPath, Constants.PDF_TEMPLATE));

				pdfStamper = CreateFormElements (pdfReader, myMemoryStream);

				pdfStamper.FormFlattening = true;
				pdfStamper.Close ();
				pdfReader.Close ();

				byte[] content = myMemoryStream.ToArray();
				HttpContext.Current.Response.Buffer = false;
				HttpContext.Current.Response.Clear();
				HttpContext.Current.Response.ClearContent();
				HttpContext.Current.Response.ClearHeaders();
				//HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + "Case_" + new Random().Next(100).ToString() + ".pdf");
				HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + "Risk.pdf");
				HttpContext.Current.Response.ContentType = "Application/pdf";

				//Write the file content directly to the HTTP content output stream.    
				HttpContext.Current.Response.BinaryWrite(content);
				HttpContext.Current.Response.Flush();
				HttpContext.Current.Response.End();
			} else {
				string pdf = Path.Combine (resourcesPath, "Risk.pdf"); 
				FileStream pdfOutputFile = new FileStream (pdf, FileMode.Create);
				pdfReader = new PdfReader (Path.Combine (resourcesPath, Constants.PDF_TEMPLATE));

				pdfStamper = CreateFormElements (pdfReader, pdfOutputFile);

				pdfStamper.FormFlattening = true;
				pdfStamper.Close ();
				pdfReader.Close ();
			}



		}

		#endregion

		#region Pdf (OLD)


//		public void BuildPdf2() 
//		{
//			using (MemoryStream myMemoryStream = new MemoryStream ()) {
//				Document doc = new Document ();
//
//				//				var output = new System.IO.FileStream (Path.Combine(xmlFilesPath + "test.pdf"), 
//				//					            System.IO.FileMode.Create);
//				PdfWriter writer = PdfWriter.GetInstance (doc, myMemoryStream);
//
//				doc.Open ();
//
//
//				Rectangle page = doc.PageSize;
//				PdfPTable head = new PdfPTable (1);
//				head.TotalWidth = page.Width;
//				Phrase phrase = new Phrase (
//					DateTime.UtcNow.ToString ("yyyy-MM-dd HH:mm:ss") + " GMT",
//					new Font (Font.FontFamily.COURIER, 8)
//				);    
//				PdfPCell c = new PdfPCell (phrase);
//				c.Border = Rectangle.NO_BORDER;
//				c.VerticalAlignment = Element.ALIGN_TOP;
//				c.HorizontalAlignment = Element.ALIGN_CENTER;
//				head.AddCell (c);
//				head.WriteSelectedRows (
//					// first/last row; -1 writes all rows
//					0, -1,
//					// left offset
//					0,
//					// ** bottom** yPos of the table
//					page.Height - doc.TopMargin + head.TotalHeight + 20,
//					writer.DirectContent 
//				);
//
//				// table heading
//				Paragraph p = new Paragraph ("Risk Problem");
//				p.Alignment = 1;
//				doc.Add (p);
//
//				// table data, see code snippet following this one
//				doc.Add (_stateTableProblem ());
//
//				Paragraph p2 = new Paragraph ("Resolution Ideas");
//				p2.Alignment = 1;
//				doc.Add (p2);
//				// table data, see code snippet following this one
//
//				doc.Add (_stateTableIdeas ());
//
//				//				Paragraph p3 = new Paragraph ("Risk Actions");
//				//				p3.Alignment = 1;
//				//				doc.Add (p3);
//				//				// table data, see code snippet following this one
//				//
//				//				doc.Add (_stateTableActions ());
//
//				doc.Close ();
//
//				byte[] content = myMemoryStream.ToArray();
//				HttpContext.Current.Response.Buffer = false;  
//				HttpContext.Current.Response.Clear();         
//				HttpContext.Current.Response.ClearContent(); 
//				HttpContext.Current.Response.ClearHeaders();  
//				HttpContext.Current.Response.AppendHeader("content-disposition","attachment;filename=" + "Risk.pdf");                
//				HttpContext.Current.Response.ContentType = "Application/pdf";        
//
//				//Write the file content directly to the HTTP content output stream.    
//				HttpContext.Current.Response.BinaryWrite(content);         
//				HttpContext.Current.Response.Flush();                
//				HttpContext.Current.Response.End(); 
//			}
//		}
//
//		public void BuildPdf ()
//		{
//			iTextSharp.text.Document doc = null;
//			using (MemoryStream myMemoryStream = new MemoryStream ()) {
//				try
//				{
//					// Initialize the PDF document
//					doc = new Document();
//					//var output = new System.IO.FileStream(Path.Combine(pdfPath + "test.pdf"),
//					//                System.IO.FileMode.Create);
//					iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, myMemoryStream);
//
//					// Set the margins and page size
//					this.SetStandardPageSize(doc);
//
//					// Add metadata to the document.  This information is visible when viewing the 
//					// document properities within Adobe Reader.
//					doc.AddTitle("Risk Hunting App");
//					doc.AddHeader("title", this.currentRisk.Name);
//					//doc.AddHeader("author", "M. Lichtenberg");
//					//doc.AddCreator("M. Lichtenberg");
//					doc.AddKeywords("risk resolution");
//
//					// Add Xmp metadata to the document.
//					//this.CreateXmpMetadata(writer);
//
//					// Open the document for writing content
//					doc.Open();
//
//					// Add pages to the document
//					this.AddPageWithBasicFormatting(doc);
//
//				}
//				catch (iTextSharp.text.DocumentException dex)
//				{
//					Console.Write(dex.Message);
//				}
//				finally
//				{
//					SendEmail (false, myMemoryStream);
//
//					// Clean up
//					doc.Close();
//					doc = null; 
//
//					byte[] content = myMemoryStream.ToArray();
//					HttpContext.Current.Response.Buffer = false;
//					HttpContext.Current.Response.Clear();
//					HttpContext.Current.Response.ClearContent();
//					HttpContext.Current.Response.ClearHeaders();
//					//HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + "Case_" + new Random().Next(100).ToString() + ".pdf");
//					HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + "Risk.pdf");
//					HttpContext.Current.Response.ContentType = "Application/pdf";
//
//					//Write the file content directly to the HTTP content output stream.    
//					HttpContext.Current.Response.BinaryWrite(content);
//					HttpContext.Current.Response.Flush();
//					HttpContext.Current.Response.End();
//
//
//				}
//			}
//		}
//
//		/// <summary>
//		/// Add the header page to the document.  This shows an example of a page containing
//		/// both text and images.  The contents of the page are centered and the text is of
//		/// various sizes.
//		/// </summary>
//		/// <param name="doc"></param>
//		private void AddPageWithBasicFormatting(iTextSharp.text.Document doc)
//		{
//
//
//			//			// Add a logo
//			//			String appPath = System.IO.Directory.GetCurrentDirectory();
//			//			iTextSharp.text.Image logoImage = iTextSharp.text.Image.GetInstance(Path.Combine(logoPath + "CareNshare_small.jpg"));
//			//			logoImage.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
//			//			doc.Add(logoImage);
//			//			logoImage = null;
//
//			// Write page content.  Note the use of fonts and alignment attributes.
//			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new iTextSharp.text.Chunk("\n\n"));
//			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _largeFont, new Chunk("Risk problem description \n\n"));
//			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _smallFont, _stateTableProblem());
//			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new Chunk("\n"));
//
//			// Write page content.  Note the use of fonts and alignment attributes.
//			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new iTextSharp.text.Chunk("\n\n"));
//			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _largeFont, new Chunk("Resolution ideas \n\n"));
//			// Create an unordered bullet list.  The 10f argument separates the bullet from the text by 10 points
//			iTextSharp.text.List listIdeas = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED, 10f);
//			listIdeas.SetListSymbol("\u2022");   // Set the bullet symbol (without this a hypen starts each list item)
//			listIdeas.IndentationLeft = 20f;     // Indent the list 20 points
//			if (this.currentRisk.Recommendations.Count > 0) {
//				for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
//					listIdeas.Add(new iTextSharp.text.ListItem(this.currentRisk.Recommendations[i].ToString(), _standardFont));
//				}
//			}
//			doc.Add(listIdeas);  // Add the list to the page
//			//			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new Chunk("\n"));
//			//
//			//			// Write page content.  Note the use of fonts and alignment attributes.
//			//			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new iTextSharp.text.Chunk("\n\n"));
//			//			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _largeFont, new Chunk("Risk actions \n\n"));
//			//			// Create an unordered bullet list.  The 10f argument separates the bullet from the text by 10 points
//			//			iTextSharp.text.List listActions = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED, 10f);
//			//			listActions.SetListSymbol("\u2022");   // Set the bullet symbol (without this a hypen starts each list item)
//			//			listActions.IndentationLeft = 20f;     // Indent the list 20 points
//			//			if (this.currentRisk.Actions.Count > 0) {
//			//				for (int i = 0; i < this.currentRisk.Actions.Count; i++) {
//			//					listActions.Add(new iTextSharp.text.ListItem(this.currentRisk.Actions[i].Content, _standardFont));
//			//					var list = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED, 10f);
//			//					list.IndentationLeft = 10f;
//			//					list.Add("Owner: " + new iTextSharp.text.ListItem(this.currentRisk.Actions[i].Owner, _smallFont));
//			//					list.Add("Implement by: " + new iTextSharp.text.ListItem(this.currentRisk.Actions[i].ImplementBy.ToShortDateString(), _smallFont));
//			//					listActions.Add (list);
//			//				}
//			//			}
//			//			doc.Add(listActions);  // Add the list to the page
//
//			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _largeFont, new Chunk("\n\n\n\n"));
//			this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_CENTER, _smallFont, new Chunk("Generated " +
//				DateTime.Now.Day.ToString() + " " +
//				System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month) + " " +
//				DateTime.Now.Year.ToString() + " " +
//				DateTime.Now.ToShortTimeString()));
//		}
//
//
//
//		/// <summary>
//		/// Set margins and page size for the document
//		/// </summary>
//		/// <param name="doc"></param>
//		private void SetStandardPageSize(iTextSharp.text.Document doc)
//		{
//			// Set margins and page size for the document
//			doc.SetMargins(50, 50, 50, 50);
//			// There are a huge number of possible page sizes, including such sizes as
//			// EXECUTIVE, POSTCARD, LEDGER, LEGAL, LETTER_LANDSCAPE, and NOTE
//			doc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.LETTER.Width,
//				iTextSharp.text.PageSize.LETTER.Height));
//		}
//
//		/// <summary>
//		/// Add a paragraph object containing the specified element to the PDF document.
//		/// </summary>
//		/// <param name="doc">Document to which to add the paragraph.</param>
//		/// <param name="alignment">Alignment of the paragraph.</param>
//		/// <param name="font">Font to assign to the paragraph.</param>
//		/// <param name="content">Object that is the content of the paragraph.</param>
//		private void AddParagraph(Document doc, int alignment, iTextSharp.text.Font font, iTextSharp.text.IElement content)
//		{
//			Paragraph paragraph = new Paragraph();
//			paragraph.SetLeading(0f, 1.2f);
//			paragraph.Alignment = alignment;
//			paragraph.Font = font;
//			paragraph.Add(content);
//			doc.Add(paragraph);
//		}
//
//		// add a table to the PDF document
//		private PdfPTable _stateTableProblem() {
//			string[] col = { "Type", "Description" };
//			PdfPTable table = new PdfPTable(2);
//			/*
//			* default table width => 80%
//			*/  
//			table.WidthPercentage = 100;
//			// then set the column's __relative__ widths
//			table.SetWidths(new Single[] {1, 5});
//			/*
//			* by default tables 'collapse' on surrounding elements,
//			* so you need to explicitly add spacing
//			*/
//			table.SpacingBefore = 10;
//
//			for (int i = 0; i < col.Length; ++i) {
//				PdfPCell cell = new PdfPCell(new Phrase(col[i]));
//				cell.BackgroundColor = new BaseColor(204, 204, 204);
//				table.AddCell(cell);
//			}
//
//			table.AddCell("Risk Name");
//			table.AddCell(this.currentRisk.Name);
//			table.AddCell("Risk Description");
//			table.AddCell(this.currentRisk.Content);
//			table.AddCell("Author");
//			table.AddCell(this.currentRisk.Author);
//			table.AddCell("Risk Location");
//			table.AddCell(this.currentRisk.LocationDetail);
//			table.AddCell("Body Part at Risk");
//			table.AddCell(this.currentRisk.BodyPart);
//			table.AddCell("Injury Category");
//			table.AddCell(this.currentRisk.InjuryNature);
//			table.AddCell("Date of Incident");
//			table.AddCell(this.currentRisk.DateIncidentOccurred.ToShortDateString());
//
//			return table;
//		}  
//
//		// add a table to the PDF document
//		private PdfPTable _stateTableIdeas() {
//			string[] col = { "Description"};
//			PdfPTable table = new PdfPTable(1);
//			/*
//			* default table width => 80%
//			*/  
//			table.WidthPercentage = 100;
//			// then set the column's __relative__ widths
//			table.SetWidths(new Single[] {10});
//			/*
//			* by default tables 'collapse' on surrounding elements,
//			* so you need to explicitly add spacing
//			*/
//			table.SpacingBefore = 10;
//
//			for (int i = 0; i < col.Length; ++i) {
//				PdfPCell cell = new PdfPCell(new Phrase(col[i]));
//				cell.BackgroundColor = new BaseColor(204, 204, 204);
//				table.AddCell(cell);
//			}
//
//
//			if (this.currentRisk.Recommendations.Count > 0) {
//				for (int i = 0; i < this.currentRisk.Recommendations.Count; i++) {
//					table.AddCell(this.currentRisk.Recommendations[i].ToString());
//				}
//			}
//			return table;
//		}  
//
//		// add a table to the PDF document
//		private PdfPTable _stateTableActions() {
//			string[] col = { "Owner", "Description", "Implement By" };
//			PdfPTable table = new PdfPTable(3);
//			/*
//			* default table width => 80%
//			*/  
//			table.WidthPercentage = 100;
//			// then set the column's __relative__ widths
//			table.SetWidths(new Single[] {1, 5, 4});
//			/*
//			* by default tables 'collapse' on surrounding elements,
//			* so you need to explicitly add spacing
//			*/
//			table.SpacingBefore = 10;
//
//			for (int i = 0; i < col.Length; ++i) {
//				PdfPCell cell = new PdfPCell(new Phrase(col[i]));
//				cell.BackgroundColor = new BaseColor(204, 204, 204);
//				table.AddCell(cell);
//			}
//
//
//			if (this.currentRisk.Actions.Count > 0) {
//				for (int i = 0; i < this.currentRisk.Actions.Count; i++) {
//					table.AddCell(this.currentRisk.Actions[i].Owner);
//					table.AddCell(this.currentRisk.Actions[i].Content);
//					table.AddCell(this.currentRisk.Actions[i].ImplementBy.ToShortDateString());
//				}
//			}
//			return table;
//		}  


		#endregion


	}
}

