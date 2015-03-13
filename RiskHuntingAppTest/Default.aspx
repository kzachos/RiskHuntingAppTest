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
 <link href="Theme/css/mozillaStyle.css" rel="stylesheet" media="screen" type="text/css" />
 <link href="Theme/css/ieStyle.css" rel="stylesheet" media="screen" type="text/css" />
 <link href="Theme/css/box.css" rel="stylesheet" media="screen" type="text/css" />
 <%--<link href="Theme/css/style2.css" rel="stylesheet" media="screen" type="text/css" />--%>
  <script src="Theme/javascript/functions.js" type="text/javascript"></script>
	<title>Risk Hunting App</title>
	<script type="text/javascript" src="Theme/javascript/jquery-latest.js"></script>
	<script type="text/javascript" src="Theme/javascript/jquery.layout-latest.js"></script>

	<style type="text/css">
	body {
	  font-family: sans-serif;
	}

	/* sizes */
	#main-wrap > div { min-height: 500px; }

	#header {
	  margin-top: 20px;
	  text-align: right;
	  text-align: center;
	}

	#sub-header {
	  font-family: sans-serif;
	  margin-top: 20px;
	  font-size: 20px;
	  min-height: 70px;
	  text-align: center;
	}

	#sub-header2 {
		position: relative;
		font-weight: bold;
		font-size: 20px;
		right: 20px;
		left: 9px;
		color: #941830;
		text-shadow: #FFF 0 1px 0;
		padding: 1px 0 3px 8px;
		text-align: center;
	}

	/* layout */
	#main-wrap {
	  /* overflow to handle inner floating block */
	  overflow: hidden;
	}

	#col-left {
	  float: left;
	  text-align: center;
	  width: 49%;
	 }

	.right {
	  width: 404px;
	  float: right;
	}

	#col-right {
	  float: right;
	  text-align: center;
	  width: 49%;
	}   

	.left {
	  width: 404px;
	  float: left;
	}

	a img{
	    border:0;
	}

	input#btnOpenBS, #btnOpenRH {
	  margin-top: 10px;
	  padding: 7px 15px 7px 15px;
	  font-family: Lucida Sans, Tahoma, sans-serif;
	  font-size:18px;
	  font-weight:600;
	  outline: none;
	  cursor: pointer;
	  text-align: center;
	  text-decoration: none;
	  color: #ffffff;
	  border: solid 1px #3598DC;
	  background-color: #3598DC;
	  border-radius: 4px 4px 4px 4px;
	}

	</style>
	<script type="text/javascript">

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

	<span id="loading"></span>
	 <div id="header"><a href="http://projectcollage.eu/" target="_blank"><img src="Theme/images/collagelogo.png" height="60"></a></div>
	 <p><br><br><div id="sub-header2">Welcome to our Risk Hunting space.</div><p><br><br>
	 <div id="main-wrap">
	    <div id="col-left">
	     <div class="right"> 
	       <h2>Superheroes for Risks</h2>
	       <div>
		       <a href="javascript:doLoad('http://brightsparks.city.ac.uk/RiskHunting');">
		       		<img style="position:center; width: 100%" src="Theme/images/brightsparks.png">
		       </a>
	       </div>
	       <input id="btnOpenBS" onclick="javascript:window.location.href='http://brightsparks.city.ac.uk/RiskHunting'" value="Open" type="button" />
	     </div>
	    </div>
	    <div id="col-right">
	     <div class="left">
	       <h2>Risk Hunting and Resolving</h2>
	       <div>
		       <a href="javascript:doLoad('DescribeRisk.aspx');">
		       		<img style="position:center; width: 100%" src="Theme/images/riskhunting.png">
		       </a>
	       </div>
	       <input id="btnOpenRH" onclick="javascript:window.location.href='DescribeRisk.aspx'" value="Open" type="button" />
	     </div>
	 </div>


	</body>
</html>

