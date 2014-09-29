﻿<%@ Page Language="C#" Inherits="RiskHuntingAppTest.Default" %>
<%@ Register Assembly="WaterMarkTextBox" Namespace="World.Code.WebControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
  <meta content="yes" name="apple-mobile-web-app-capable" />
  <meta content="index,follow" name="robots" />
 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
 <link href="Theme/pics/mirror_icon.gif" rel="apple-touch-icon" />
 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
 <%--<link href="Theme/css/style2.css" rel="stylesheet" media="screen" type="text/css" />--%>
  <script src="Theme/javascript/functions.js" type="text/javascript"></script>
	<title>Risk Hunting App</title>
	<link href="Theme/pics/mirror_startup.png" rel="apple-touch-startup-image" />

<style type="text/css">

</style>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("modal");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
            loading.css({ top: top, left: left });
        }, 200);
    }
    $('form').live("submit", function () {
        ShowProgress();
    });
</script>
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
			<a id="pressed" href="">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-1-filled-icon.png" /> Problem
			</a>
			<a href="javascript:doLoad('Solution_ResolutionIdeas.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-2-icon.png" /> Solution
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
			<a id="pressed" href="">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-1-filled-icon.png" /> Problem
			</a>
			<a href="javascript:doLoad('SearchResults.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-2-icon.png" /> Search
			</a>
			<a href="javascript:doLoad('Solution_ResolutionIdeas.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-3-icon.png" /> Solution
			</a>
		</div>
	</div>
</div>

<div id="navigationbar" runat="server">
	<!--<div id="duobutton">
		<div class="links">
			<a id="pressed" href="#">Problem</a>
			<a href="javascript:doLoad('ResolutionIdeas.aspx');">Ideas</a>
		</div>
	</div>-->

</div>
<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:Timer ID="Timer1" runat="server"  Interval="10000" ontick="Timer1_Tick"></asp:Timer>  

	<!--<span class="maintitle">SELECT EXISTING RISK</span>
	<ul class="pageitem">
		<li class="blank">
			<asp:Label  runat="server" Width="100%" ></asp:Label>
		</li>
		<li class="select" >
			<asp:DropDownList ID="ExistingRiskDropDown" runat="Server" Width="98%" ></asp:DropDownList>
			<span class="arrow"></span>
		</li>
		<li class="blank">
			<asp:Label  runat="server" Width="100%" ></asp:Label>
		</li>
	</ul>

	<span class="darkredboldtitle">OR</span><br><br>
	-->

	<!--<span class="maintitle">ENTER NEW RISK</span>-->
	<div id="errorMsg" runat="server">
	
	</div>	
	<span class="maintitle">Risk Name</span>
	<ul class="pageitem">
		<li class="label">
			<cc1:WatermarkedTextBox id="RiskName" Font-Size="Larger" runat="server" TextMode="MultiLine" Width="100%" Height="40" cssclass="txtbox"></cc1:WatermarkedTextBox>
		</li>
	</ul>
	<span class="maintitle">Description</span>
	<span class="maintitlesmall">(this field is required)</span>
	<ul class="pageitem">
		<li class="label">
			<cc1:WatermarkedTextBox ID="RiskDescription" Font-Size="Larger" runat="server" TextMode="Multiline" Width="100%" Height="73" cssclass="txtbox"></cc1:WatermarkedTextBox>
		</li>
	</ul>
	<span class="maintitle">Name of person reporting the risk</span>
	<ul class="pageitem">
		<li class="label">
			<cc1:WatermarkedTextBox id="RiskAuthor" Font-Size="Larger" runat="server" TextMode="MultiLine" Width="100%" Height="40" cssclass="txtbox"></cc1:WatermarkedTextBox>
		</li>
	</ul>
	<span class="maintitle">Location</span>
	<ul class="pageitem">
		<li class="select">
		    <select id="RiskLocation" name="RiskLocation" runat="server">
		    </select>
		    <span class="arrow"></span> 
		</li>
	</ul>
	<span class="maintitle">Body Parts at Risk</span>
	<ul class="pageitem">
		<li class="select">
		    <select id="RiskBodyParts" name="RiskBodyParts" runat="server">
		    </select>
		    <span class="arrow"></span> 
		</li>
	</ul>



	<ul class="pageitembutton">
		<li class="button3">
			<asp:Button id="submit" runat="server" text="FIND SIMILAR RISKS" onclick="submitClicked"></asp:Button>
		</li>
	</ul>


<!--<div id="previousrisks" runat="server">
	<ul class="pageitembutton">
		<li class="button3">
			<asp:Button id="previousrisksButton" runat="server" text="VIEW RETRIEVED PREVIOUS RISKS" onclick="risksClicked"></asp:Button>
		</li>
	</ul>
</div>-->
<div class="loading" align="center">
    Loading. Please wait.<br />
    <br />
    <img src="loader.gif" alt="" />
</div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
	        <asp:Label id="AutoSaveLabel" runat="server" Width="100%" ></asp:Label>
		</ContentTemplate>
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="Timer1"  EventName="Tick"/>
		</Triggers>
    </asp:UpdatePanel>

</form>

</div>



<div id="footer">
</div>


</body>
</html>

