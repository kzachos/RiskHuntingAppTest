<%@ Page Language="C#" Inherits="RiskHuntingAppTest.SearchResults" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
  <meta content="yes" name="apple-mobile-web-app-capable" />
 <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
 <meta content="minimum-scale=1.0, width=device-width, maximum-scale=0.6667, user-scalable=no" name="viewport" />
 <link href="Theme/css/style.css" rel="stylesheet" media="screen" type="text/css" />
 <script src="Theme/javascript/functions.js" type="text/javascript"></script>
	<title>Risk Hunting App</title>
</head>
<body>


<div id="TopbarProblemSearchSolution" runat="server">
	<div id="topbar2">
		<div id="leftbutton">
			<a href="javascript:doLoad('Settings.aspx');" >
				<%--<img alt="home" src="Theme/images/home.png" />--%>
				Feedback
			</a> 
		</div>
		<div id="rightbutton">
			<a href="javascript:doLoad('QueryHistory.aspx');">
				<%--<img alt="home" src="Theme/images/home.png" />--%>
				Risk History
			</a> 
		</div>
		<div id="multiselectionbuttons">
			<a href="javascript:doLoad('Default.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-1-icon.png" /> Problem
			</a>
			<a id="pressed" href="" >
			<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-2-filled-icon.png" /> Search
			</a>
			<a href="javascript:doLoad('Solution_ResolutionIdeas.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-3-icon.png" /> Solution
			</a>
		</div>
	</div>
</div>


<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">

	<ul class="pageitem">
		<div id="responses" runat="server">
	
		</div>
		<div id="hide" runat="server">
			<li class="store">
				<a class="noeffect" href="javascript:doLoad('DisplayResponse_Description.aspx?id=1000');">
					<!--<span class="image" style="background-image: url('http://a1.phobos.apple.com/us/r1000/002/Music/0e/f0/26/mzi.bxbwyvvz.170x170-75.jpg')"></span>-->
					<span class="name">Uneven Floor</span>
					<span class="comment">During change of shift, the hallway is very crowded and the floor is not so visible. People trip often because they don’t see the holes or uneven surfaces. </span>
					<!--<span class="stars5"></span>-->
					<span class="starcomment">March 15th 2013, Basilldon (UK)</span>
					<span class="arrow"></span>
				</a>
			</li>
		</div>
	</ul>
	<ul class="pageitem">
		<li class="textbox">
			<asp:Label id="statusLabel" runat="server"></asp:Label>
		</li>
	</ul>
</form>


</div>
<div id="footer">
</div>


</body>
</html>

