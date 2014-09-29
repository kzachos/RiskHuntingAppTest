<%@ Page Language="C#" Inherits="RiskHuntingAppTest.About" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
  <meta content="yes" name="apple-mobile-web-app-capable" />
 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
 <script src="Theme/javascript/functions.js" type="text/javascript"></script>
	<title>Carer App</title>
</head>
<body>

<div id="topbar">
  <div id="title">Carer App</div>
  <div id="leftbutton">
    <a href="javascript:doLoad('Settings.aspx');" ><%--<img alt="home" src="Theme/images/home.png" />--%>Settings</a> 
  </div>
  <div id="rightbutton">
    <a href="javascript:doLoad('QueryHistory.aspx');"><%--<img alt="home" src="Theme/images/home.png" />--%>History</a> 
  </div>
</div>
<div id="tributton">
	<div class="links">
		<a href="javascript:doLoad('Default.aspx');">Query</a><a href="javascript:doLoad('SearchResults.aspx');">Results</a><a id="pressed" href="#">About</a></div>
</div>
<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">
	<ul class="pageitem">
	<!--	<Li class="textbox">
		    <span class="header">About this app</span>
		    <p>CarerApp uses EDDiE 
		    (Expansion and Disambiguation Discovery Engine) as its discovery engine. EDDiE discovers text 
		    artefacts (e.g. web service descriptions, documents) from queries expressed using structured 
		    natural language by extending information retrieval techniques to overcome incompleteness and 
		    ambiguity. EDDiE extends query expansion techniques to generate more complete service queries 
		    while at the same time applies term disambiguation techniques to generate unambiguous service 
		    queries.</p>		
		    <span class="header">Development</span>
		    <p>CarerApp has been developed by Konstantinos Zachos. Konstantinos Zachos is a
researcher at the Centre for Human Computer Interaction Design at
City University and has been involved in research in the areas of
requirements engineering, service discovery mechanism and techniques,
and creative problem-solving and learning.</p>
		    <span class="header">MIRROR project</span>
		    <p>CarerApp has been developed as part of the EU-funded MIRROR project. MIRROR ...</p>
		</li>-->
	</ul>
</form>


</div>

<div id="footer">
</div>


</body>
</html>
