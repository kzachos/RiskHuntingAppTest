﻿<%@ Page Language="C#" Inherits="RiskHuntingAppTest.CreateIdeas_BrightSparks" %>
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
	<script type="text/javascript" src="Theme/javascript/jquery-latest.js"></script>
	<script type="text/javascript" src="Theme/javascript/jquery.layout-latest.js"></script>
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

		<div class="ui-layout-north">
			<div id="topbar2">
				<div id="leftbutton">
					<a href="javascript:doLoad('Default.aspx');" >
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
					<a id="pressed" href="">Ideas from Superheroes</a>
					<a href="javascript:doLoad('CreateIdeas_PastRisks.aspx');">Ideas from previous risks</a>
				</div>
			</div>
		</div>
		<div class="ui-layout-center">
			<iframe src="http://brightsparks.city.ac.uk/RiskHunting/" border="0" frameborder="0" height="100%" width="100%"></iframe>
		</div>
			
		<div class="ui-layout-south" id="content">
			<form id="form1" runat="server">
				<div id="AddNewIdeaDiv" runat="server">
					<ul class="pageitembutton">
						<li class="button">
							<asp:Button id="AddNewIdea" runat="server" text="ADD NEW IDEA" onclick="addNewIdeaClicked"></asp:Button>
						</li>
					</ul>
				</div>
			</form>
		</div>



	</body>
</html>