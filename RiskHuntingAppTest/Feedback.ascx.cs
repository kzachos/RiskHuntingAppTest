using System;
using System.Web.Mail;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace RiskHuntingAppTest
{


	
	public partial class Feedback : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void SendMail()
		{
			try
			{
				var fromAddress = "koszachos@gmail.com";// Gmail Address from where you send the mail
	//			var toAddress = YourEmail.Text.ToString(); 
				var toAddress = "kzachos@gmail.com"; 
				const string fromPassword = "gSectime13!";//Password of your gmail address
	//			string subject = YourSubject.Text.ToString();
				string body = "From: " + YourName.Text + "\n";
	//			body += "Email: " + YourEmail.Text + "\n";
	//			body += "Subject: " + YourSubject.Text + "\n";
				body += "Feedback: \n" + Comments.Text + "\n";
				var smtp = new System.Net.Mail.SmtpClient();
				{
					smtp.Host = "smtp.gmail.com";
					smtp.Port = 587;
					smtp.EnableSsl = true;
					smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
					smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
					smtp.Timeout = 20000;
				}
				ServicePointManager.ServerCertificateValidationCallback = 
					delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
				{ return true; };
				smtp.Send(fromAddress, toAddress, "Feedback", body);

			}
			catch (Exception e) { 
				Console.WriteLine (e.ToString());
			}

		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			try
			{
				SendMail();
				lblMsgSend.Text = "Thank you for your input!";
				lblMsgSend.Visible = true;
//				YourSubject.Text = "";
//				YourEmail.Text = "";
				YourName.Text = "";
				Comments.Text = "";
			}
			catch (Exception ex) {Console.WriteLine (ex.ToString()); }
		}
	}
}

