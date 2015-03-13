<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUploadControl.aspx.cs"
    Inherits="RiskHuntingAppTest.FileUploadControl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	  <meta content="yes" name="apple-mobile-web-app-capable" />
	 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
	 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
	 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
	 <link href="Theme/css/mozillaStyle.css" rel="stylesheet" media="screen" type="text/css" />
     <link href="Theme/css/ieStyle.css" rel="stylesheet" media="screen" type="text/css" />
	 <link href="Theme/css/box.css" rel="stylesheet" media="screen" type="text/css" />
	<script src="Theme/javascript/functions.js" type="text/javascript"></script>
		<title>Risk Hunting App</title>
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
	<script type="text/javascript">

		window.setTimeout(function () {
		    $("#alert_message_success").fadeTo(500, 0).slideUp(500, function () {
		        $(this).remove();
		    });
		}, 3000);
		window.setTimeout(function () {
		    $("#alert_message_error").fadeTo(500, 0).slideUp(500, function () {
		        $(this).remove();
		    });
		}, 3000);
		    
	</script>
	<style type="text/css">
	p.MsoNormal
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:10.0pt;
	margin-left:0in;
	line-height:115%;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	}

	</style>
<!--[if gte IE 9]>
  <style type="text/css">
    .gradient {
       filter: none;
    }
  </style>
<![endif]-->
</head>
<body>
	<div id="topbar2">
		<div id="leftbutton">
			<a href="javascript:doLoad('DescribeRisk.aspx');" >
				Return to Risk
			</a> 
		</div>
		<div id="multiselectionbuttons">
			Upload Image
		</div>
	</div>

	<span id="loading"></span>
	<div id="content">
	    <form id="form1" runat="server">
	
			<div id="alert_message_success" runat="server">
				<div class="alert-box success">
					<div id="successMessage" style="display: inline" runat="server"></div>
				</div>
			</div>
			<div id="alert_message_error" runat="server">
				<div class="alert-box error">
					<div id="errorMessage" style="display: inline" runat="server"></div>
				</div>
			</div>
		    
			<div id="previewDiv" runat="server">
			    <span class="maintitle">Image Preview</span><br>
			    <p>
			        <asp:Image ID="Image1" runat="server" Height="500px" Width="500px" />
			    </p>
		    </div>
			<ul class="pageitembutton">
				<li class="buttongrey">
					<asp:FileUpload ID="FileUpload1" runat="server" ></asp:FileUpload>
				</li>
			</ul>
			<ul class="pageitembutton">
				<li class="button">
					<asp:Button id="UploadImage" runat="server" text="UPLOAD IMAGE" onclick="uploadClicked"></asp:Button>
				</li>
			</ul>
			<div id="ButtonsDiv" runat="server">
				<ul class="pageitembutton">
					<li class="button2">
						<asp:Button id="Cancel" runat="server" text="DELETE IMAGE" onclick="cancelClicked"></asp:Button>
					</li>
				</ul>
		    </div>
	    </form>
    </div> 
</body>
</html>
