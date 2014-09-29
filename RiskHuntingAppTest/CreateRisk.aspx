<%@ Page Language="C#" Inherits="RiskHuntingAppTest.CreateRisk" %>
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
	<title>Risk Hunting App</title>
	<link href="Theme/pics/mirror_startup.png" rel="apple-touch-startup-image" />
</head>
<body>

<div id="topbar">
	<div id="title" runat="server"><%--<img width="10%" alt="home" src="Theme/images/mirror-logo.jpg" />--%>Current risk</div>
	<div id="leftbutton">
		<a href="javascript:doLoad('Settings.aspx');" ><%--<img alt="home" src="Theme/images/home.png" />--%>Settings</a> 
	</div>
	<div id="rightbutton">
		<a href="javascript:doLoad('QueryHistory.aspx');"><%--<img alt="home" src="Theme/images/home.png" />--%>Risk History</a> 
	</div>
</div>

<!--<div id="tributton">
	<div class="links">
		<a id="pressed" href="#">Query</a><a href="javascript:doLoad('SearchResults.aspx');">Results</a><a href="javascript:doLoad('About.aspx');">About</a>
	</div>
</div>-->
<div id="navigationbar" runat="server">
	<div id="duobutton">
		<div class="links">
			<a id="pressed" href="#">See current risk DESCRIPTION</a><a href="javascript:doLoad('ResolutionIdeas.aspx');">See current risk RESOLUTIONS</a>
		</div>
	</div>
</div>
<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">

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

	<span class="maintitle">ENTER NEW RISK</span>
	<div id="errorMsg" runat="server">
	
	</div>	
	<ul class="pageitem">
		<li class="label">
			<asp:Label text="Name" runat="server" Width="100%" ></asp:Label>
			<asp:TextBox id="RiskName" runat="server" TextMode="MultiLine" Width="98%" Height="37" >test</asp:TextBox>
		</li>
		<li class="label">
			<asp:Label text="Danger" runat="server" Width="100%" ></asp:Label>
			<asp:TextBox id="RiskDanger" runat="server" TextMode="MultiLine" Width="98%" Height="37" ></asp:TextBox>
		</li>
		<li class="label">
			<asp:Label text="Location" runat="server" Width="100%" ></asp:Label>
			<asp:TextBox id="RiskLocation" runat="server" TextMode="MultiLine" Width="98%" Height="37" ></asp:TextBox>
		</li>
		<li class="label">
			<asp:Label text="Risk" runat="server" Width="100%" ></asp:Label>
			<asp:TextBox id="RiskRisk" runat="server" TextMode="MultiLine" Width="98%" Height="37" ></asp:TextBox>
		</li>
		<li class="label">
			<asp:Label text="Description" runat="server" Width="100%" ></asp:Label>
			<asp:TextBox id="RiskDescription" runat="server" TextMode="MultiLine" Width="98%" Height="80" >test</asp:TextBox>
		</li>
		<li class="label">
			<asp:Label text="Body Parts at Risk" runat="server" Width="100%" ></asp:Label>
			<asp:TextBox id="RiskBodyParts" runat="server" TextMode="MultiLine" Width="98%" Height="37" ></asp:TextBox>
		</li>
	</ul>



	<ul class="pageitembutton">
		<li class="button2">
			<asp:Button id="submit" runat="server" text="SEARCH" onclick="submitClicked"></asp:Button>
		</li>
	</ul>

<div id="previousrisks" runat="server">
	<ul class="pageitembutton">
		<li class="button3">
			<asp:Button id="previousrisksButton" runat="server" text="VIEW RETRIEVED PREVIOUS RISKS" onclick="risksClicked"></asp:Button>
		</li>
	</ul>
</div>

</form>

</div>



<div id="footer">
</div>


</body>
</html>

