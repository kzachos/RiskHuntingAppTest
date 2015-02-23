<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="RiskHuntingAppTest.Settings" %>
<%@ Register Assembly="WaterMarkTextBox" Namespace="World.Code.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/Feedback.ascx" TagName="SendMail" TagPrefix="sm1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head id="Head1" runat="server">
  <meta content="yes" name="apple-mobile-web-app-capable" />
 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
 <script src="Theme/javascript/functions.js" type="text/javascript"></script>
	<title>Risk Hunting App</title>
</head>
<body>

<div id="Topbar" runat="server">
	<div id="topbar2">
		<div id="leftbutton">
			<a href="javascript:doLoad('Default.aspx');" >
				back
			</a> 
		</div>
		<div id="multiselectionbuttons">
				Feedback area
		</div>

	</div>
</div>
<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">
	<%--<span class="maintitle">Part of Speech</span>
	<ul class="pageitem">
		<li class="label">
			<asp:Label text="Description" runat="server" Width="100%" ></asp:Label>
			<cc1:WatermarkedTextBox ID="FeedbackDescription" runat="server" TextMode="Multiline" Width="98%" Height="80"></cc1:WatermarkedTextBox>
		</li>
	</ul>
	<ul class="pageitem">
		<li class="button">
			<asp:Button id="submit" runat="server" text="Save" onclick="submitClicked"></asp:Button>
		</li>
	</ul>--%>
    <div>
    	<sm1:SendMail ID="SendMail1" runat="server" />
    </div>
</form>


</div>
<div id="footer">
</div>


</body>
</html>

