<%@ Page Language="C#" Inherits="RiskHuntingAppTest.MarkAsResolvedRisk" %>
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

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">

$(document).on('click','.close',function(){ 
    $(this).parent().fadeTo(300,0,function(){ 
          $(this).remove(); 
    }); 
});

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
		<div id="leftbutton">
			<a href="javascript:doLoad('Summary.aspx');" >
				Return to Summary
			</a> 
		</div>
		<div id="multiselectionbuttons">
			Mark Case As Resolved
		</div>
	</div>

	<span id="loading"></span>
	<div id="content">
		<form id="form1" runat="server">
	
			<div id="alert_message_success" runat="server">
				<div class="alert-box success">
					<div id="successMessage" style="display: inline" runat="server"></div>
				</div>
			</div>
			<div id="alert_message_error" runat="server">
				<div class="alert-box error">
					<div id="errorMessage" style="display: inline" runat="server"></div>
				</div>
			</div>

			<div class="alert-box notice">
				<span>hint: </span>
				<div id="creativeGuidance" style="display: inline" runat="server"></div>
				<div class="close">&times;</div>
			</div>

			<ul class="pageitem">
				<div id="divIdeas" runat="server">

				</div>
			</ul>
			<ul class="pageitem">
				<li class="textbox">
					<asp:Label id="statusLabel" runat="server"></asp:Label>
				</li>
			</ul>
			<ul class="pageitembutton">
				<li class="button">
					<asp:Button id="Resolved" runat="server" text="SUBMIT SELECTION" onclick="resolvedClicked"></asp:Button>
				</li>
			</ul>
			<ul class="pageitembutton">
				<li class="button2">
					<asp:Button id="UnResolved" runat="server" text="MARK AS NOT RESOLVED" onclick="unresolvedClicked"></asp:Button>
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



