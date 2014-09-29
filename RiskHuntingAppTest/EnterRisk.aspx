<%@ Page Language="C#" Inherits="RiskHuntingAppTest.EnterRisk" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="index,follow" name="robots" />
	<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
	<link href="Theme/pics/mirror_icon.gif" rel="apple-touch-icon" />
	<meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
	<link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
	<script src="Theme/javascript/functions.js" type="text/javascript"></script>
	<title>Enter a new Risk</title>
	<link href="Theme/pics/mirror_startup.png" rel="apple-touch-startup-image" />
</head>
<body>

<!---<div id="tributton">
	<div class="links">
		<a id="pressed" href="#">Query</a><a href="javascript:doLoad('SearchResults.aspx');">Results</a><a href="javascript:doLoad('About.aspx');">About</a></div>
</div>-->
<div id="duobutton">
	<div class="links">
		<a id="pressed" href="#">Enter new risk</a><a href="javascript:doLoad('SearchResults.aspx');">Select existing risk</a></div>
</div>
<span id="loading"></span>
<div id="content">
	<form id="form1" runat="server">
	</form>
</div>
<div id="footer">
</div>


</body>
</html>