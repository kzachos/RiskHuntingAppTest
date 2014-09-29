<%@ Page Language="C#" Inherits="RiskHuntingAppTest.ActionPlan" %>
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
			<a href="javascript:doLoad('Solution_ResolutionIdeas.aspx');">Resolution ideas</a>
			<a id="pressed" href="">Action plan</a>
		</div>
	</div>
</div>


<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">
	<div id="SubmitReport" runat="server">
	
	</div>
<div id="content2" runat="server">
	<span class="maintitle">Suggested actions</span>
	<br><br>
	<ul class="pageitemborder">
		<div id="sourceDiv" runat="server">
		</div>
		<!--<li class="label">
			<asp:Label runat="server" Width="100%" >Resolution Idea A</asp:Label>
		</li>-->			

	</ul>

	<br>

	<%--<span class="maintitle">Select Resolution idea to update or delete</span>
	<br>--%>
	<span class="maintitle">Edit or delete action</span>
	<br><br>
	<ul class="pageitem">
		<li class="select">
		    <%--<select id="IdeaDropDown" name="IdeaDropDown" runat="server"></select>--%>
			<asp:DropDownList ID="ActionDropDown" runat="Server" Width="99%" OnSelectedIndexChanged="updateData" AutoPostback="True"></asp:DropDownList>
		    <span class="arrow"></span> 
		</li>
	</ul>

	<div id="EditSection" runat="server">
		<ul class="pageitem">
			<li class="label">
				<cc1:WatermarkedTextBox id="EditActionDescription" runat="server" TextMode="MultiLine" Width="100%" Height="40" Font-Size="Larger" cssclass="txtbox"></cc1:WatermarkedTextBox>
			</li>
		</ul>
		<span class="maintitle">Owner of action</span>
		<ul class="pageitem">
			<li class="label">
				<cc1:WatermarkedTextBox id="EditActionOwner" runat="server" TextMode="MultiLine" Width="100%" Height="40" Font-Size="Larger" cssclass="txtbox"></cc1:WatermarkedTextBox>
			</li>
		</ul>
		<span class="maintitle">To be implemented by</span>
		<UL class="pageitem">
			<LI class="select">
				<SELECT name="ImplementedDay" id="EditImplementedDay" size="1" runat="server">
				</SELECT>
				<SPAN class="arrow"></SPAN>
			</LI>
			<LI class="select">
				<SELECT name="ImplementedMonth" id="EditImplementedMonth" size="1" runat="server">
				</SELECT>
				<SPAN class="arrow"></SPAN>
			</LI>

			<LI class="select">
				<SELECT name="ImplementedYear" id="EditImplementedYear" size="1" runat="server">
				</SELECT>
				<SPAN class="arrow"></SPAN>
			</LI>
		</UL>

		<ul class="pageitembutton">
			<li class="button">
				<asp:Button id="UpdateAction" runat="server" text="UPDATE ACTION" onclick="updateClicked"></asp:Button>
			</li>
			<li class="button2">
				<asp:Button id="DeleteAction" runat="server" text="DELETE ACTION" onclick="deleteClicked"></asp:Button>
			</li>
		</ul>
	</div>

	<span class="maintitle">Add new action</span>
	<ul class="pageitem">
		<li class="label">
			<cc1:WatermarkedTextBox id="AddActionDescription" runat="server" TextMode="MultiLine" Width="100%" Height="40" Font-Size="Larger" cssclass="txtbox"></cc1:WatermarkedTextBox>
		</li>
	</ul>
	<span class="maintitle">Owner of action</span>
	<ul class="pageitem">
		<li class="label">
			<cc1:WatermarkedTextBox id="AddActionOwner" runat="server" TextMode="MultiLine" Width="100%" Height="40" Font-Size="Larger" cssclass="txtbox"></cc1:WatermarkedTextBox>
		</li>
	</ul>
	<span class="maintitle">To be implemented by</span>
	<UL class="pageitem">
		<LI class="select">
			<SELECT name="ImplementedDay" id="AddImplementedDay" size="1" runat="server">
			</SELECT>
			<SPAN class="arrow"></SPAN>
		</LI>
		<LI class="select">
			<SELECT name="ImplementedMonth" id="AddImplementedMonth" size="1" runat="server">
			</SELECT>
			<SPAN class="arrow"></SPAN>
		</LI>

		<LI class="select">
			<SELECT name="ImplementedYear" id="AddImplementedYear" size="1" runat="server">
			</SELECT>
			<SPAN class="arrow"></SPAN>
		</LI>
	</UL>


	<ul class="pageitembutton">
		<li class="button">
			<asp:Button id="AddAction" runat="server" text="ADD ACTION" onclick="addClicked"></asp:Button>
		</li>
	</ul>
</div>

	<div id="saveRisk" runat="server">
		<ul class="pageitembutton">
			<li class="button3">
				<asp:Button id="saveRiskButton" runat="server" text="SAVE CURRENT RISK TO DATABASE" onclick="saveRiskClicked"></asp:Button>
			</li>
		</ul>
	</div>
		
</form>


</div>
<div id="footer">
</div>


</body>
</html>


