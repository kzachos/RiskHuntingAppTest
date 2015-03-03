<%@ Page Language="C#" Inherits="RiskHuntingAppTest.CreateIdeas_PastRisk" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
  <meta content="yes" name="apple-mobile-web-app-capable" />
 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
 <link href="Theme/css/box.css" rel="stylesheet" media="screen" type="text/css" />
 <script src="Theme/javascript/functions.js" type="text/javascript"></script>
	<title>Risk Hunting App</title>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $(document).on('click','.close',function(){ 
	    $(this).parent().fadeTo(300,0,function(){ 
	          $(this).remove(); 
	    }); 
	});

	$('form').live("submit", function () {
        ShowProgress();
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
</head>
<body>

	<div id="topbar2">
		<div id="leftbutton">
			<a href="javascript:doLoad('CreateIdeas_PastRisks.aspx');" >
				Return to list
			</a> 
		</div>
		<div id="multiselectionbuttons">
			Past Risk
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

			<div id="hint_box" runat="server">
				<div class="alert-box notice">
					<span>hint: </span>
					<div id="creativeGuidance" style="display: inline" runat="server"></div>
					<div class="close">&times;</div>
				</div>
			</div>	

			<ul class="pageitem">
				<div id="content2" runat="server">
			
				</div>  
			</ul>
				

			<ul class="pageitembutton">
				<div id="generatePrompts" runat="server">
					<li class="button3">
						<asp:Button id="GenerateAgain" runat="server" text="GENERATE NEW CREATIVE PROMPTS" onclick="morePromptsClicked"></asp:Button>
					</li>
				</div>
			</ul>

			<!--<ul class="pageitembutton">
				<li class="button">
					<asp:Button id="SaveSelected" runat="server" text="SAVE SELECTED" onclick="submitClicked"></asp:Button>
				</li>
			</ul>-->
				
			
			<!--<ul class="pageitembutton">
				<li class="buttonGrey">
					<asp:Button id="ReturnToList" runat="server" text="RETURN TO LIST..." onclick="backClicked"></asp:Button>
				</li>
			</ul>-->
		</form>
	</div>
	<div id="footer">
	</div>


</body>
</html>
