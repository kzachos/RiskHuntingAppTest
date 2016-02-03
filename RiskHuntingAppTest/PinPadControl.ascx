<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PinPadControl.ascx.cs" Inherits="RiskHuntingAppTest.PinPadControl" %>
<table align="center" valign="middle" style="border: 1px solid #9999FF; border-collapse: collapse; height: 400px; width: 400px; text-align: center; background-color: white;">
    <tr>
        <td class="button3">
        	<asp:Button ID="Button1" runat="server" Text="Button" Height="75px" Width="75px" OnClick="Button_Click" /></td>
        <td class="button3">
            <asp:Button ID="Button2" runat="server" Text="Button" Height="75px"  Width="75px" OnClick="Button_Click" /></td>
        <td class="button3">
            <asp:Button ID="Button3" runat="server" Text="Button" Height="75px"  Width="75px" OnClick="Button_Click" /></td>
    </tr>
    <tr>
        <td class="button3">
            <asp:Button ID="Button4" runat="server" Text="Button" Height="75px" Width="75px" OnClick="Button_Click" /></td>
        <td class="button3">
            <asp:Button ID="Button5" runat="server" Text="Button" Height="75px" Width="75px" OnClick="Button_Click" /></td>
        <td class="button3">
            <asp:Button ID="Button6" runat="server" Text="Button" Height="75px" Width="75px" OnClick="Button_Click" /></td>
    </tr>
    <tr>
        <td class="button3">
            <asp:Button ID="Button7" runat="server" Text="Button" Height="75px" Width="75px" OnClick="Button_Click" /></td>
        <td class="button3">
            <asp:Button ID="Button8" runat="server" Text="Button" Height="75px" Width="75px" OnClick="Button_Click" /></td>
        <td class="button3">
            <asp:Button ID="Button9" runat="server" Text="Button" Height="75px" Width="75px" OnClick="Button_Click" /></td>
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
