<%@ Page Language="C#" Inherits="RiskHuntingAppTest.PinEntry" %>

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
		<title>Risk Hunting App</title>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">

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
		<div id="leftbutton">
			<a href="javascript:doLoad('DescribeRisk.aspx');" >
				Cancel
			</a> 
		</div>
		<div id="multiselectionbuttons">
			
		</div>
	</div>
	<span id="loading"></span>
	<form runat="server">

		<div id="alert_message_error" runat="server">
			<div class="alert-box error">
				<div id="errorMessage" style="display: inline" runat="server"></div>
			</div>
		</div>
		<br>
		<div align="center">
			<span class="maintitle">Enter PIN to delete risk</span>
		</div>
		<br>
		<table align="center" valign="middle" style="border: 1px solid #9999FF; border-collapse: collapse; height: 400px; width: 400px; text-align: center; background-color: white;">
		    <tr>
		        <td class="button3">
		        	<asp:Button ID="Button1" runat="server" Text="1" Height="75px" Width="75px" OnClick="Button_Click" /></td>
		        <td class="button3">
		            <asp:Button ID="Button2" runat="server" Text="2" Height="75px"  Width="75px" OnClick="Button_Click" /></td>
		        <td class="button3">
		            <asp:Button ID="Button3" runat="server" Text="3" Height="75px"  Width="75px" OnClick="Button_Click" /></td>
		    </tr>
		    <tr>
		        <td class="button3">
		            <asp:Button ID="Button4" runat="server" Text="4" Height="75px" Width="75px" OnClick="Button_Click" /></td>
		        <td class="button3">
		            <asp:Button ID="Button5" runat="server" Text="5" Height="75px" Width="75px" OnClick="Button_Click" /></td>
		        <td class="button3">
		            <asp:Button ID="Button6" runat="server" Text="6" Height="75px" Width="75px" OnClick="Button_Click" /></td>
		    </tr>
		    <tr>
		        <td class="button3">
		            <asp:Button ID="Button7" runat="server" Text="7" Height="75px" Width="75px" OnClick="Button_Click" /></td>
		        <td class="button3">
		            <asp:Button ID="Button8" runat="server" Text="8" Height="75px" Width="75px" OnClick="Button_Click" /></td>
		        <td class="button3">
		            <asp:Button ID="Button9" runat="server" Text="9" Height="75px" Width="75px" OnClick="Button_Click" /></td>
		    </tr>
		    <tr>
		        <td colspan="3" >
		            <asp:TextBox ID="pinTextBox" Height="30px" Width="90%" runat="server" ReadOnly="True"></asp:TextBox></td>
		    </tr>
		    <tr>
		        <td colspan="2" style="text-align: left; padding-left: 10px;">
		            <asp:LinkButton ID="clearButton" runat="server" OnClick="clearButton_Click">Clear</asp:LinkButton></td>
		        <td colspan="1" style="text-align: right; padding-right: 10px;">
		            <asp:LinkButton ID="submitButton" runat="server" OnClick="submitButton_Click">Submit</asp:LinkButton></td>
		    </tr>
		</table>
	</form>
</body>

</html>

