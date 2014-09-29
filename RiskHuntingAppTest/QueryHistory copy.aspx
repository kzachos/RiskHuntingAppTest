<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryHistory.aspx.cs" Inherits="RiskHuntingAppTest.QueryHistory" %>

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

<div id="topbar">
  <div id="title">Carer App</div>
  <div id="leftbutton">
    <a href="javascript:doLoad('Default.aspx');" ><%--<img alt="home" src="Theme/images/home.png" />--%>Home</a> 
  </div>
</div>
<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">
	<span class="graytitle">Previous queries</span>
	<ul class="pageitem">
		<div id="queries" runat="server">

		</div>
	</ul>
	<ul class="pageitem">
		<li class="textbox">
			<asp:Label id="statusLabel" runat="server"></asp:Label>
		</li>
	</ul>
</form>


</div>
<div id="footer">
</div>


</body>
</html>

