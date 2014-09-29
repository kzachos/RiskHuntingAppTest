<%@ Page Language="C#" Inherits="RiskHuntingAppTest.Solution_ResolutionIdeas" %>
<%@ Register Assembly="WaterMarkTextBox" Namespace="World.Code.WebControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head id="Head1" runat="server">
  <meta content="yes" name="apple-mobile-web-app-capable" />
 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
 <script src="Theme/javascript/functions.js" type="text/javascript"></script>

<script type="text/javascript">
var todaysDate=new Date()
</script>
	<title>Risk Hunting App</title>
</head>
<body>

<div id="TopbarProblemSolution" runat="server">
	<div id="topbar2">
		<div id="leftbutton">
			<a href="javascript:doLoad('Settings.aspx');" >
				<%--<img alt="home" src="Theme/images/home.png" />--%>
				Feedback
			</a> 
		</div>
		<div id="rightbutton">
			<a href="javascript:doLoad('QueryHistory.aspx');">
				<%--<img alt="home" src="Theme/images/home.png" />--%>
				Risk History
			</a> 
		</div>
		<div id="multiselectionbuttons">
			<a href="javascript:doLoad('Default.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-1-icon.png" /> Problem
			</a>
			<a id="pressed" href="">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-2-filled-icon.png" /> Solution
			</a>
		</div>
	</div>
</div>
<div id="TopbarProblemSearchSolution" runat="server">
	<div id="topbar2">
		<div id="leftbutton">
			<a href="javascript:doLoad('Settings.aspx');" >
				<%--<img alt="home" src="Theme/images/home.png" />--%>
				Feedback
			</a> 
		</div>
		<div id="rightbutton">
			<a href="javascript:doLoad('QueryHistory.aspx');">
				<%--<img alt="home" src="Theme/images/home.png" />--%>
				Risk History
			</a> 
		</div>
		<div id="multiselectionbuttons">
			<a href="javascript:doLoad('Default.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-1-icon.png" /> Problem
			</a>
			<a href="javascript:doLoad('SearchResults.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-2-icon.png" /> Search
			</a>
			<a id="pressed" href="" >
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-3-filled-icon.png" /> Solution
			</a>
		</div>
	</div>
</div>
<div id="topbar_v2" runat="server">
	<div id="topbar2Sub">
		<div id="multiselectionbuttonsSub">
			<a href="javascript:doLoad('CreativeGuidanceForProblem.aspx');">Creative guidance from problem</a>
			<a id="pressed" href="">Resolution ideas</a>
			<a href="javascript:doLoad('Solution_ActionPlan.aspx');">Action plan</a>
		</div>
	</div>
</div>


<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">
	<div id="SubmitReport" runat="server">
	
	</div>

	<!--<span class="maintitle">Suggested Resolution Ideas</span>
	<span class="maintitlesmall">(select an idea to edit or delete)</span>
	<br><br>-->
	<ul class="pageitem">
		<div id="divIdeas" runat="server">

		</div>
	</ul>
	<ul class="pageitembutton">
		<li class="button">
			<asp:Button id="AddNewIdea" runat="server" text="ADD NEW IDEA" onclick="addNewIdeaClicked"></asp:Button>
		</li>
	</ul>
	<ul class="pageitem">
		<li class="textbox">
			<asp:Label id="statusLabel" runat="server"></asp:Label>
		</li>
	</ul>

	<ul class="pageitem">
		<div id="content2" runat="server">
	
		</div>   
	</ul>
		

		


</form>


</div>
<div id="footer">
</div>


</body>
</html>


