<%@ Page Language="C#" Inherits="RiskHuntingAppTest.Summary" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
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
	var ignore = true;
	
    $(document).on('click','.close',function(){ 
	    $(this).parent().fadeTo(300,0,function(){ 
	          $(this).remove(); 
	    }); 
	});

	$('form').live("submit", function () {
        if (needToConfirm)
        	ShowProgress();
    });

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

	<div id="topbar2">
		<div id="leftnav">
    		<a href="javascript:doLoad('Default.aspx');" ><img alt="home" src="Theme/images/home.png" />
    		</a>
			<a href="javascript:doLoad('ResolveRisk.aspx');" >
				&nbsp;&nbsp;&nbsp; <img alt="home" style="position:relative; TOP:2px;  height: 65%" src="Theme/images/numbers-3-icon.png" />
				Resolve Risk
			</a> 
		</div>
		<div id="multiselectionbuttons">
			<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-4-filled-icon.png" /> 
			Summary
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
	<br>

	<span class="maintitle">Risk Problem Description</span>
	<br><br>
	<ul class="pageitemborder">
		<div id="sourceDiv" runat="server">
		</div>

	</ul>

	<br><br>

	<span class="maintitle">Resolution Ideas</span>
	<br><br>
	<ul class="pageitemborder">
		<div id="divIdeas" runat="server">

		</div>
	</ul>

	<br><br>


	<ul class="pageitem">
		<li class="textbox">
			<asp:Label id="statusLabel" runat="server"></asp:Label>
		</li>
	</ul>
	<ul class="pageitembutton">
		<li class="button">
			<asp:button id="submit" runat="server" text="SUBMIT CASE" onclick="submitClicked" onClientclick="needToConfirm = false;"></asp:button>
		</li>
	</ul>

	<ul class="pageitembutton">
		<li class="button3">
			<asp:Button id="Export" runat="server" text="GENERATE REPORT TO PRINT" onclick="exportClicked" onClientclick="ignore = false;"></asp:Button>
		</li>
	</ul>

	<ul class="pageitembutton">
		<li class="buttonGrey">
			<asp:Button id="NewRisk" runat="server" text="CREATE NEW RISK" onclick="createNewClicked"></asp:Button>
		</li>
	</ul>
</form>


</div>
<div id="footer">
</div>


</body>
</html>


