<%@ Page Language="C#" Inherits="RiskHuntingAppTest.CreativeGuidanceForProblem" %>
<!DOCTYPE html>
<html>
<head runat="server">
  <meta content="yes" name="apple-mobile-web-app-capable" />
 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
 <script src="Theme/javascript/functions.js" type="text/javascript"></script>
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
			<a id="pressed" href="">Creative guidance from problem</a>
			<a  href="javascript:doLoad('Solution_ResolutionIdeas.aspx');">Resolution ideas</a>
			<a href="javascript:doLoad('Solution_ActionPlan.aspx');">Action plan</a>
		</div>
	</div>
</div>


<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">
	<div id="SubmitReport" runat="server">
	
	</div>
	<span id="RiskName" runat="server" class="maintitle"></span>
	<ul class="pageitem">
		<div id="generatePrompts" runat="server">
			<ul class="pageitem">
				<li class="button3">
					<asp:Button id="GenerateAgain" runat="server" text="GENERATE AGAIN" onclick="morePromptsClicked"></asp:Button>
				</li>
			</ul>
		</div>
		<div id="content2" runat="server">
	
		</div>

		<!--<li class="checkbox">
			<span class="name">Think about how to use the floor so that it regenerates</span>
		    <asp:CheckBox id="CheckBox1" runat="server"></asp:CheckBox>
		</li>
		<li class="blank">
			<asp:Label  runat="server" Width="100%" ></asp:Label>
		</li>-->
			
		

		<li class="button3">
			<asp:Button id="MorePrompts" runat="server" text="MORE PROMPTS" onclick="morePromptsClicked"></asp:Button>
		</li>     

	</ul>

	<ul class="pageitembutton">
		<li class="button3">
			<asp:Button id="SaveSelected" runat="server" text="SAVE SELECTED AND VIEW ALL IDEAS" onclick="submitClicked"></asp:Button>
		</li>
	</ul>
		
</form>


</div>
<div id="footer">
</div>


</body>
</html>
