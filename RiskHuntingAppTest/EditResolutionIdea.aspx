<%@ Page Language="C#" Inherits="RiskHuntingAppTest.EditResolutionIdea" %>
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
 <link href="Theme/css/box.css" rel="stylesheet" media="screen" type="text/css" />
 <script src="Theme/javascript/functions.js" type="text/javascript"></script>
	<title>Risk Hunting App</title>
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
	<script type="text/javascript">
	    
	function Confirm() {
	    var confirm_value = document.createElement("INPUT");
	    confirm_value.type = "hidden";
	    confirm_value.name = "confirm_value";
	    if (confirm("Are you sure you want to delete the idea?")) {
	        confirm_value.value = "Yes";
	    } else {
	        confirm_value.value = "No";
	    }
	    document.forms[0].appendChild(confirm_value);
	}

	window.setTimeout(function () {
	    $("#alert_message_success").fadeTo(500, 0).slideUp(500, function () {
	        $(this).remove();
	    });
	}, 3000);
	window.setTimeout(function () {
	    $("#alert_message_error").fadeTo(500, 0).slideUp(500, function () {
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

</div>

<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">

	<div id="alert_message_error" runat="server">
		<div class="alert-box error">
			<div id="errorMessage" style="display: inline" runat="server"></div>
		</div>
	</div>
		
	<ul class="pageitem">
		<li class="label">
			<cc1:WatermarkedTextBox id="EditIdeaDescription" runat="server" TextMode="MultiLine" Width="100%" Height="80" Font-Size="Larger" cssclass="txtbox"></cc1:WatermarkedTextBox>
		</li>
	</ul>

	<ul class="pageitembutton">
		<li class="button">
			<asp:Button id="UpdateIdea" runat="server" text="UPDATE IDEA" onclick="updateClicked"></asp:Button>
		</li>
	</ul>

	<ul class="pageitembutton">
		<li class="button2">
			<asp:Button id="DeleteIdea" runat="server" text="DELETE IDEA" onclick="deleteClicked"  onClientclick="Confirm();"></asp:Button>
		</li>
	</ul>
</form>


</div>
<div id="footer">
</div>


</body>
</html>
