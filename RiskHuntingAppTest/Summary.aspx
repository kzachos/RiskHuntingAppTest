<%@ Page Language="C#" Inherits="RiskHuntingAppTest.Summary" %>
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
</script>
</head>
<body>

<div id="TopbarProblemSolution" runat="server">
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
			<a href="javascript:doLoad('Solution_ResolutionIdeas.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-2-filled-icon.png" /> Solution
			</a>
			<a id="pressed" href="">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-3-icon.png" /> Summary
			</a>
		</div>
	</div>
</div>
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
			<a href="javascript:doLoad('SearchResults.aspx');">
			<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-2-icon.png" /> Search
			</a>
			<a href="javascript:doLoad('Solution_ResolutionIdeas.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-3-icon.png" /> Solution
			</a>
			<a  id="pressed" href="javascript:doLoad('Summary.aspx');">
			<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-4-filled-icon.png" /> Summary
			</a>
		</div>
	</div>
</div>


<span id="loading"></span>
<div id="content">
<form id="form1" runat="server">

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
		<!--<li class="label">
			<asp:Label runat="server" Width="100%" >Resolution Idea A</asp:Label>
		</li>-->			

	</ul>

	<br><br>

	<span class="maintitle">Resolution Ideas</span>
	<br><br>
	<ul class="pageitemborder">
		<div id="divIdeas" runat="server">

		</div>
	</ul>

	<br><br>

	<!--<span class="maintitle">Action Plan</span>
	<br><br>
	<ul class="pageitemborder">
		<div id="divActions" runat="server">
		</div>
	</ul>-->

	<!--<span class="maintitle">Risk Name</span>
	<ul class="pageitem">
		<li class="labelcontent">
			<asp:Label id="RiskName" runat="server" TextMode="MultiLine" Width="100%" ></asp:Label>
		</li>
	</ul>
	<span class="maintitle">Description</span>
	<ul class="pageitem">
		<li class="labelcontent">
			<asp:Label id="RiskDescription" runat="server" TextMode="MultiLine" Width="100%" >During change of shift, the hallway is very crowded and the floor is not visible. People trip often because they do not see the holes or uneven surfaces</asp:Label>
		</li>
	</ul>
	<span class="maintitle">Name of person reporting the risk</span>
	<ul class="pageitem">
		<li class="labelcontent">
			<asp:Label id="RiskAuthor" runat="server" TextMode="MultiLine" Width="100%" >Uneven floor</asp:Label>
		</li>
	</ul>
	<span class="maintitle">Location</span>
	<ul class="pageitem">
		<li class="labelcontent">
			<asp:Label id="RiskLocation" runat="server" TextMode="MultiLine" Width="100%" >Path from work to locker room</asp:Label>
		</li>
	</ul>
	<span class="maintitle">Body Parts at Risk</span>
	<ul class="pageitem">
		<li class="labelcontent">
			<asp:Label id="RiskBodyParts" runat="server" TextMode="MultiLine" Width="100%" >Ankles, wrists, head, shoulders</asp:Label>
		</li>
	</ul>-->


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
			<asp:Button id="Export" runat="server" text="EXPORT" onclick="exportClicked"></asp:Button>
		</li>
	</ul>
</form>


</div>
<div id="footer">
</div>


</body>
</html>


