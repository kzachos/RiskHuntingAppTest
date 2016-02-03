<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryHistory.aspx.cs" Inherits="RiskHuntingAppTest.QueryHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head id="Head1" runat="server">
  <meta content="yes" name="apple-mobile-web-app-capable" />
 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
 <link href="Theme/css/mozillaStyle.css" rel="stylesheet" media="screen" type="text/css" />
 <link href="Theme/css/ieStyle.css" rel="stylesheet" media="screen" type="text/css" />
 <link href="Theme/css/box.css" rel="stylesheet" media="screen" type="text/css" />
<script src="Theme/javascript/functions.js" type="text/javascript"></script>
	<title>Risk Hunting App</title>
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
	<script type="text/javascript">


window.setTimeout(function() {
  $("#alert_message_success").fadeTo(500, 0).slideUp(500, function(){
    $(this).remove(); 
  });
}, 3000);
window.setTimeout(function() {
  $("#alert_message_error").fadeTo(500, 0).slideUp(500, function(){
    $(this).remove(); 
  });
}, 3000);

</script>
<!--[if gte IE 9]>
  <style type="text/css">
    .gradient {
       filter: none;
    }
  </style>
<![endif]-->
</head>

<body>



<div id="TopbarProblemIdeas" runat="server">
	<div id="topbar2">
		<div id="leftbutton">
			<a href="javascript:doLoad('DescribeRisk.aspx');" >
				back
			</a> 
		</div>
		<div id="multiselectionbuttons">
				Our Previous Risks
		</div>

	</div>
</div>

	<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">

	<div id="alert_message_notice" runat="server">
		<div class="alert-box notice">
			<div id="noticeMessage" style="display: inline" runat="server"></div>
		</div>
	</div>

	<div id="SortDiv" runat="server">
		<span class="maintitle">Sort by: </span>
		<ul class="pageitem">
			<li class="select" >
				<asp:DropDownList ID="SortDropDown" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="itemSelected" runat="Server">
	                <asp:ListItem Text="Date" Value="0" />
	                <asp:ListItem Text="Status" Value="1" />
	                <asp:ListItem Text="Category" Value="2" />
	                <asp:ListItem Text="Author" Value="3" />
	   			</asp:DropDownList>
				<span class="arrow"></span>
			</li>
		</ul> <br>
	</div>
	<ul class="pageitem">
		<div id="queries" runat="server">

		</div>
	</ul>
</form>


</div>
<div id="footer">
</div>


</body>
</html>

