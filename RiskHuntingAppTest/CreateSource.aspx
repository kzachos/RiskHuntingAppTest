<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateSource.aspx.cs" Inherits="RiskHuntingAppTest.CreateSource" %>
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
    <div id="leftbutton">
        <a href="javascript:doLoad('DisplaySources.aspx');" ><%--<img alt="home" src="Theme/images/home.png" />--%>All Cases</a> 
    </div>
</div>
<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">
	<div id="SubmitReport" runat="server">
	
	</div>
	<br />
	<span class="maintitle">Select a domain</span>
	<div id="DomainErrorMsg" runat="server">
	
	</div>
	<ul class="pageitem">
		<li class="select" >
			<asp:DropDownList ID="DomainDropDown" runat="Server">
			</asp:DropDownList>
			<span class="arrow"></span>
		</li>
	</ul>
	<span class="maintitle">Where is the case from?</span>
	<ul class="pageitem">
		<li class="textbox">
		    <cc1:WatermarkedTextBox ID="SourceTextBox" runat="server" TextMode="Multiline" Width="100%" Height="20"></cc1:WatermarkedTextBox>
		</li>
	</ul>
	<span class="maintitle">Enter a case name</span>
	<div id="NameErrorMsg" runat="server">
	
	</div>	
    
	<ul class="pageitem">
		<li class="textbox">
		    <cc1:WatermarkedTextBox ID="NameTextBox" runat="server" TextMode="Multiline" Width="100%" Height="20"></cc1:WatermarkedTextBox>
		</li>
	</ul>
	<span class="maintitle">Enter the situation encountered</span>
	<div id="ProblemDescrErrorMsg" runat="server">
	
	</div>	
	<ul class="pageitem">
		<li class="textbox">
		    <cc1:WatermarkedTextBox ID="ProblemDescrTextBox" runat="server" TextMode="Multiline" Width="100%" Height="100" ></cc1:WatermarkedTextBox>
		</li>
	</ul>
	<span class="maintitle">Enter the resolution that was applied</span>
	<div id="SolutionDescrErrorMsg" runat="server">
	
	</div>	
	<ul class="pageitem">
		<li class="textbox">
			<cc1:WatermarkedTextBox ID="SolutionDescrTextBox" runat="server" TextMode="Multiline" Width="100%" Height="100" ></cc1:WatermarkedTextBox>
		</li>
<%--		<li class="textbox">
		    <cc1:WatermarkedTextBox ID="Miscellaneous2TextBox" runat="server" TextMode="Multiline" Width="100%" Height="50" ></cc1:WatermarkedTextBox>
		</li>--%>
	</ul>
	<span class="maintitle">Enter keywords seperated by ';' (for example: Verbal abuse; risk of harm)</span>
	<div id="MiscellaneousErrorMsg" runat="server">
	
	</div>	
	<ul class="pageitem">
		<li class="textbox">
			<cc1:WatermarkedTextBox ID="MiscellaneousTextBox" runat="server" TextMode="Multiline" Width="100%" Height="50" ></cc1:WatermarkedTextBox>
		</li>
	</ul>
	<span class="maintitle">If the case is in the healthcare domain, what best describes the behaviour of the patient?</span>
	<ul class="pageitem">
		<li class="select" >
			<asp:DropDownList ID="ObservedBehaviourDropDown" runat="Server">
			</asp:DropDownList>
			<span class="arrow"></span>
		</li>
<%--		<li class="select" >
			<asp:DropDownList ID="TreatmentTypeDropDown" runat="Server">
			</asp:DropDownList>
			<span class="arrow"></span>
		</li>
		<li class="select" >
			<asp:DropDownList ID="AilmentTypeDropDown" runat="Server">
			</asp:DropDownList>
			<span class="arrow"></span>
		</li>
		<li class="textbox">
		    <cc1:WatermarkedTextBox ID="TriggeringEventTextBox" runat="server" TextMode="Multiline" Width="100%" Height="50" ></cc1:WatermarkedTextBox>
		</li>
		<li class="textbox">
		    <cc1:WatermarkedTextBox ID="MiscellaneousTextBox" runat="server" TextMode="Multiline" Width="100%" Height="50" ></cc1:WatermarkedTextBox>
		</li>--%>
	</ul>
	<ul class="pageitem">
		<li class="button">
			<asp:Button BackColor="LightGray" id="submit" runat="server" text="Submit case" onclick="submitClicked"></asp:Button>
		</li>
		<li class="button">
			<asp:Button BackColor="LightGray" id="reset" runat="server" text="Reset values" onclick="reset_Click"></asp:Button>
		</li>
	</ul>

</form>


</div>
<div id="footer">
</div>


</body>
</html>

