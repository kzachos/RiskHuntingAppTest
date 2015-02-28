<%@ Page Language="C#" Inherits="RiskHuntingAppTest.Default" Async="true" %>
<%@ Register Assembly="WaterMarkTextBox" Namespace="World.Code.WebControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
  <meta content="yes" name="apple-mobile-web-app-capable" />
  <meta content="index,follow" name="robots" />
 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
 <link href="Theme/pics/mirror_icon.gif" rel="apple-touch-icon" />
 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
 <link href="Theme/css/box.css" rel="stylesheet" media="screen" type="text/css" />
 <%--<link href="Theme/css/style2.css" rel="stylesheet" media="screen" type="text/css" />--%>
  <script src="Theme/javascript/functions.js" type="text/javascript"></script>
	<title>Risk Hunting App</title>
	<script type="text/javascript" src="Theme/javascript/jquery-latest.js"></script>
	<script type="text/javascript" src="Theme/javascript/jquery.layout-latest.js"></script>

<style type="text/css">
	.container {
	width: 1170px;
	}
	.container {
	padding-right: 15px;
	padding-left: 15px;
	margin-right: auto;
	margin-left: auto;
	}
.col-1 {
width: 50%;
}
.col-2 {
width: 50%;
}
.col-1, {
min-height: 1px;
padding-right: 15px;
padding-left: 15px;
float: left;
}
.col-2 {
min-height: 1px;
padding-right: 15px;
padding-left: 15px;
float: right;
}

</style>
	<script type="text/javascript">

	</script>
	</head>
	<body>

	<span id="loading"></span>

	<div class="container">
		<div class="ui-layout-north" id="multiselectionbuttons">
				TEST
		</div>

		<div class="col-1">
			<div id="multiselectionbuttons">
					Superheroes for Risks
			</div><br><br>
			<a href="javascript:doLoad('http://brightsparks.city.ac.uk/RiskHunting');" style="text-align:center">
				<img alt="home" style="position:center; width: 80%" src="Theme/images/brightsparks.png" />
			</a> 


		</div>


		<div class="col-2">
			<div id="multiselectionbuttons">
					Risk Hunting and Resolving
			</div><br><br>
			<a href="javascript:doLoad('http://brightsparks.city.ac.uk/RiskHunting');" >
				<img alt="home" style="position:center; width: 80%" src="Theme/images/riskhunting.png" />
			</a> 

		</div>
	</div>


	</body>
</html>

