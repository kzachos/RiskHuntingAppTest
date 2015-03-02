<%@ Page Language="C#" Inherits="RiskHuntingAppTest.CreateIdeas_PastRisks" Async="true" %>
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
			<a href="javascript:doLoad('DescribeRisk.aspx');" >
				<img alt="home" style="position:relative; TOP:2px;  height: 65%" src="Theme/images/numbers-1-icon.png" />
				Describe Risk
			</a> 
		</div>
		<div id="rightbutton">
			<a href="javascript:doLoad('ResolveRisk.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 65%" src="Theme/images/numbers-3-icon.png" />
				Resolve Risk
			</a> 
		</div>
		<div id="multiselectionbuttons">
			<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-2-filled-icon.png" /> 
			Create Ideas
		</div>
	</div>

	<div id="topbar2Sub">
		<div id="multiselectionbuttonsSub">
			<a href="javascript:doLoad('CreateIdeas_SameRisk.aspx');">Ideas from risk itself</a>
			<a href="javascript:doLoad('CreateIdeas_Superheroes.aspx');">Ideas from Superheroes</a>
			<a id="pressed" href="">Ideas from previous risks</a>
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

			<div class="alert-box notice" id="alert_message_guidance" runat="server">
				<span>hint: </span>
				<div id="creativeGuidance" style="display: inline" runat="server"></div>
				<div class="close">&times;</div>
			</div>
				
			<ul class="pageitem">

				<div id="responses" runat="server">
			
				</div>

			</ul>

			<div id="submitDiv" runat="server">
				<ul class="pageitembutton">
					<li class="button3">
						<asp:Button id="submit" runat="server" text="FIND SIMILAR RISKS" onclick="submitClicked"></asp:Button>
					</li>
				</ul>
			</div>
			<!--<div id="creativeGuidance2" runat="server">
				<ul class="pageitembutton">
					<li class="button3">
						<asp:Button id="creativeGuidanceButton" runat="server" text="VIEW CREATIVE GUIDANCE FOR PROBLEM" onclick="creativeGuidanceClicked"></asp:Button>
					</li>
				</ul>
			</div>-->
				
				
		</form>
	</div>
	<div id="footer">
	</div>


</body>
</html>
