﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcessSource.aspx.cs" Inherits="RiskHuntingAppTest.ProcessSource" %>
<%@ Register Assembly="WaterMarkTextBox" Namespace="World.Code.WebControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head id="Head1" runat="server">
  <meta content="yes" name="apple-mobile-web-app-capable" />
 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
 <script src="Theme/javascript/functions.js" type="text/javascript"></script>
	<title>Create Case Study</title>
</head>
<body>

<div id="topbar">
	<div id="title">Create a Case</div>
</div>
<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">
	<ul class="pageitem">
		<li class="button">
			<asp:Button id="submit" runat="server" text="Process cases" onclick="submitClicked"></asp:Button>
		</li>
	</ul>

</form>


</div>
<div id="footer">
</div>


</body>
</html>

