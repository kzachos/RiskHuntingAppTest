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
	<link href="Theme/pics/mirror_startup.png" rel="apple-touch-startup-image" />

<style type="text/css">

</style>
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
	<script type="text/javascript">
    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("modal");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
            loading.css({ top: top, left: left });
        }, 200);
    }
    $('form').live("submit", function () {
        ShowProgress();
    });

    $(document).on('click','.close',function(){ 
	    $(this).parent().fadeTo(300,0,function(){ 
	          $(this).remove(); 
	    }); 
	});
	    

function Confirm() {
    var confirm_value = document.createElement("INPUT");
    confirm_value.type = "hidden";
    confirm_value.name = "confirm_value";
    if (confirm("Are you sure you want to delete the risk permanently?")) {
        confirm_value.value = "Yes";
    } else {
        confirm_value.value = "No";
    }
    document.forms[0].appendChild(confirm_value);
}

function AutoExpand(txtbox) {
    txtbox.style.height = "1px";
    txtbox.style.height = (7 + txtbox.scrollHeight) + "px";
}

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
</head>
<body onload="AutoExpand(RiskDescription)">

	<div id="topbar2">
		<div id="leftbutton">
			<a href="javascript:doLoad('QueryHistory.aspx');" >
				<%--<img alt="home" src="Theme/images/home.png" />--%>
				Our Previous Risks
			</a> 
		</div>
		<!--<div id="rightbutton" runat="server">
			<a href="javascript:doLoad('CreateIdeas_SameRisk.aspx');">
				<img alt="home" style="position:relative; TOP:2px;  height: 65%" src="Theme/images/numbers-2-icon.png" />
				Create Ideas
			</a> 
		</div>-->
		<div id="multiselectionbuttons">
			<img alt="home" style="position:relative; TOP:2px;  height: 45%" src="Theme/images/numbers-1-filled-icon.png" /> 
			Describe Risk
		</div>
	</div>


	<span id="loading"></span>
	<div id="content">
	<form id="form1" runat="server">

		<asp:ScriptManager ID="ScriptManager1" runat="server">
	    </asp:ScriptManager>

	    <asp:Timer ID="Timer1" runat="server"  Interval="3000" ontick="Timer1_Tick"></asp:Timer>  


		<!--<span class="maintitle">SELECT EXISTING RISK</span>
		<ul class="pageitem">
			<li class="blank">
				<asp:Label  runat="server" Width="100%" ></asp:Label>
			</li>
			<li class="select" >
				<asp:DropDownList ID="ExistingRiskDropDown" runat="Server" Width="98%" ></asp:DropDownList>
				<span class="arrow"></span>
			</li>
			<li class="blank">
				<asp:Label  runat="server" Width="100%" ></asp:Label>
			</li>
		</ul>

		<span class="darkredboldtitle">OR</span><br><br>
		-->
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

			


		<div id="errorMsg" runat="server"></div>	
		<span class="maintitle">Enter a suitable title for this risk</span>
		<span class="maintitlesmall">(required)</span>
		<ul class="pageitem">
			<li class="label">
				<cc1:WatermarkedTextBox id="RiskName" Font-Size="Larger" runat="server" TextMode="MultiLine" Width="100%" Height="30" cssclass="txtbox"></cc1:WatermarkedTextBox>
			</li>
		</ul>
		<span class="maintitle">Please describe the risk</span>
		<span class="maintitlesmall">(required)</span>
		<ul class="pageitem">
			<li class="label">
				<cc1:WatermarkedTextBox ID="RiskDescription" Font-Size="Larger" runat="server" TextMode="Multiline" Width="100%" cssclass="txtbox" onkeyup="AutoExpand(this)" Rows="3"></cc1:WatermarkedTextBox>
			</li>
		</ul>
		<span class="maintitle">Enter the name of person reporting the risk</span>
		<span class="maintitlesmall">(required)</span>
		<ul class="pageitem">
			<li class="label">
				<cc1:WatermarkedTextBox id="RiskAuthor" Font-Size="Larger" runat="server" TextMode="MultiLine" Width="100%" Height="30" cssclass="txtbox" ></cc1:WatermarkedTextBox>
			</li>
		</ul>
		<span class="maintitle">Where did it happen?</span>
		<ul class="pageitem">
			<li class="select">
			    <select id="RiskLocation" name="RiskLocation" runat="server">
			    </select>
			    <span class="arrow"></span> 
			</li>
		</ul>
		<span class="maintitle">Which main body part is at risk?</span>
		<ul class="pageitem">
			<li class="select">
			    <select id="RiskBodyParts" name="RiskBodyParts" runat="server">
			    </select>
			    <span class="arrow"></span> 
			</li>
		</ul>



		<ul class="pageitembutton">
			<!--<li class="buttonGrey">
				<asp:Button id="save" runat="server" text="SAVE CHANGES" onclick="saveClicked"></asp:Button>
			</li>-->
			<li class="button">
				<asp:button id="createIdeasButton" runat="server" text="CREATE IDEAS" onclick="ideasClicked"></asp:button>
			</li>
				
		</ul>
			
		<ul class="pageitembutton">
			<li class="buttonGrey">
				<asp:button id="reset" runat="server" text="CLEAR FORM" onclick="resetClicked"></asp:button>
			</li>
		</ul>

		<ul class="pageitembutton">
			<div id="deleteRiskDiv" runat="server">
				<li class="button2">
					<asp:Button id="delete" runat="server" text="DELETE RISK" onclick="deleteClicked" onClientclick="Confirm();"></asp:Button>
				</li>
			</div>	
		</ul>
			
		<div class="loading" align="center">
		    Loading. Please wait.<br />
		    <br />
		    <img src="loader.gif" alt="" />
		</div>

	    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
		        <asp:Label id="AutoSaveLabel" runat="server" Width="100%" ></asp:Label>
			</ContentTemplate>
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="Timer1"  EventName="Tick"/>
			</Triggers>
	    </asp:UpdatePanel>

	</form>

	</div>



	<div id="footer">
	</div>


</body>
</html>

