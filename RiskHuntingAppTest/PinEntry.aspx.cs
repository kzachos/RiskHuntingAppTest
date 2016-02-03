using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace RiskHuntingAppTest
{
	
	public partial class PinEntry : System.Web.UI.Page
	{
		protected const string INCORRECTPIN_MESSAGE = "Incorrect PIN. Try again";
		protected string sourceId;
		protected string xmlFilesPath = Path.Combine (SettingsTool.GetApplicationPath(), "xmlFiles");

		public string Pin { get { return pinTextBox.Text; } }

		protected override void OnInit(EventArgs e)
		{
			//			int[] array = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			//			Random rnd = new Random();
			//			for (int i = array.Length - 1; i > 0; i--)
			//			{
			//				//				int j = rint j = rnd.Next(i);nd.Next(i);
			//				int j = i;
			//				int k = array[j];
			//				array[j] = array[i - 1];
			//				array[i - 1] = k;
			//			}

			var buttons = this.Controls.OfType<Button>().ToArray();

			for (int i = 0; i < buttons.Length; i++)
			{
				int j = i + 1;
				buttons[i].Text = j.ToString();
			}
			base.OnInit(e);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			alert_message_error.Visible = false;
			pinTextBox.Attributes["type"] = "password";
		}

		protected void Button_Click(object sender, EventArgs e)
		{
			if (pinTextBox.Text.Equals (INCORRECTPIN_MESSAGE))
				pinTextBox.Text = string.Empty;
			pinTextBox.Text += (sender as Button).Text;
		}

		protected void clearButton_Click(object sender, EventArgs e)
		{
			pinTextBox.Text = string.Empty;
		}

		protected void submitButton_Click(object sender, EventArgs e)
		{
			if (pinTextBox.Text.Equals (Constants.PIN)) {
				//				string confirmValue = Request.Form["confirm_value"];
				//				if (confirmValue == "Yes") {
				Util.AccessLog (Util.ScreenType.DescribeRisk, Util.FeatureType.DescribeRisk_DeleteRiskButton);
				if (Sessions.RiskState != String.Empty) {
					this.sourceId = Sessions.RiskState;
					string filesToDelete = @"*" + this.sourceId + "*.xml";   // Only delete xml files containing *sourceID* in their filenames
					string[] fileList = System.IO.Directory.GetFiles (xmlFilesPath, filesToDelete, System.IO.SearchOption.AllDirectories);
					//				Debug.WriteLine (filesToDelete);
					foreach (string file in fileList) {
						Debug.WriteLine (file + " will be deleted");
						File.Delete (file);
					}
					string pb = String.Empty;
					var output = RemoveCase (this.sourceId);
					if (output) 
						pb = "deleted";
					else
						pb = "notdeleted";
					pb = "deleted";
					Response.Redirect ("DescribeRisk.aspx?pb=" + pb);
				} else {
					Response.Redirect ("DescribeRisk.aspx?pb=" + "notdeleted");
				}
				//				}

			} else {
				alert_message_error.Visible = true;
				errorMessage.InnerText = INCORRECTPIN_MESSAGE;
				pinTextBox.Text = string.Empty;
//				pinTextBox.Text = INCORRECTPIN_MESSAGE;
			}
		}


		bool RemoveCase(string caseId)
		{
			try
			{
				RiskHuntingAppTest.eddieService.EDDiEWebService eddie = new RiskHuntingAppTest.eddieService.EDDiEWebService();
				var output1 = eddie.RemoveCase ("SourceSpecification", caseId);
				var output2 = eddie.RemoveCase ("Problem", caseId);
				var output3 = eddie.RemoveCase ("Solution", caseId);
				Console.WriteLine("SourceSpecification output1: " + output1);
				Console.WriteLine("Problem output2: " + output2);
				Console.WriteLine("Solution output3: " + output3);

				if (output1.StartsWith ("Stored document", StringComparison.Ordinal) &&
					output2.StartsWith ("Stored document", StringComparison.Ordinal) &&
					output3.StartsWith ("Stored document", StringComparison.Ordinal))
					return true;
				else 
					return false;
			}
			catch {
				return false;
			}
		}
	}
}


