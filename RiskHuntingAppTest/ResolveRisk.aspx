<%@ Page Language="C#" Inherits="RiskHuntingAppTest.ResolveRisk" %>
<%@ Register Assembly="WaterMarkTextBox" Namespace="World.Code.WebControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head id="Head1" runat="server">
  <meta content="yes" name="apple-mobile-web-app-capable" />
 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
 <link href="Theme/css/mozillaStyle.css" rel="stylesheet" media="screen" type="text/css" />
 <link href="Theme/css/ieStyle.css" rel="stylesheet" media="screen" type="text/css" />
 <script src="Theme/javascript/functions.js" type="text/javascript"></script>

<script type="text/javascript">
var todaysDate=new Date()
</script>
	<title>Risk Hunting App</title>
<!--[if gte IE 9]>
  <style type="text/css">
    .gradient {
       filter: none;
    }
  </style>
<![endif]-->
</head>
<body>

	<div id="topbar2">
		<div id="leftnav">
    		<a href="javascript:doLoad('Default.aspx');" ><img alt="home" src="Theme/images/home.png" />
    		</a>
			<a href="javascript:doLoad('CreateIdeas_SameRisk.aspx');" >
				&nbsp;&nbsp;&nbsp; <img alt="home" style="position:relative; TOP:2px;  height: 65%" src="Theme/images/numbers-2-icon.png" />
				Create Risk
			</a> 
		</div>
		<div id="rightbutton">
			<a href="javascript:doLoad('Summary.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 65%" src="Theme/images/numbers-4-icon.png" />
				Summary
			</a> 
		</div>
		<div id="multiselectionbuttons">
			<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-3-filled-icon.png" /> 
			Resolve Risk
		</div>
	</div>

	<span id="loading"></span>
	<div id="content">
		<form id="form1" runat="server">
	
			<ul class="pageitem">
				<div id="divIdeas" runat="server">

				</div>
			</ul>
			<ul class="pageitem">
				<li class="textbox">
					<asp:Label id="statusLabel" runat="server"></asp:Label>
				</li>
			</ul>
			<div id="AddNewIdeaDiv" runat="server">
				<ul class="pageitembutton">
					<li class="button">
						<asp:Button id="AddNewIdea" runat="server" text="ADD NEW IDEA" onclick="addNewIdeaClicked"></asp:Button>
					</li>
				</ul>
			</div>


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



