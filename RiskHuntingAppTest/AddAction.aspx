<%@ Page Language="C#" Inherits="RiskHuntingAppTest.AddAction" %>
<%@ Register Assembly="WaterMarkTextBox" Namespace="World.Code.WebControls" TagPrefix="cc1" %>

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

<div id="TopbarProblemIdeas" runat="server">
	<div id="topbar2">
		<div id="leftbutton">
			<a href="javascript:doLoad('Solution_ActionPlan.aspx');" >
				cancel
			</a> 
		</div>
		<div id="multiselectionbuttons">
				Add New Action
		</div>
	</div>
</div>

<span id="loading"></span>
<div id="content">
	<form id="form1" runat="server">
		<span class="maintitle">Action description</span>
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
				<asp:Button id="AddNewAction" runat="server" text="ADD ACTION" onclick="addClicked"></asp:Button>
			</li>
		</ul>
	</form>


</div>
<div id="footer">
</div>


</body>
</html>