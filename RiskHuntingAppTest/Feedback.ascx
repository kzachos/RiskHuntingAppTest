<%@ Control Language="C#" AutoEventWireup="true" Inherits="RiskHuntingAppTest.Feedback" %>
<asp:Panel ID="Panel1" runat="server" DefaultButton="btnSubmit">
    <p>
        Please Fill the Following to Send Mail.
    </p>

        Your name:
        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="*"
            ControlToValidate="YourName" ValidationGroup="save" /><br />
        <asp:TextBox ID="YourName" runat="server" Width="250px" /><br />
        Your Feedback/Suggestion/Error Reporting:
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
            ControlToValidate="Comments" ValidationGroup="save" /><br />
        <asp:TextBox ID="Comments" runat="server" TextMode="MultiLine" Rows="10" Width="400px" />
    
    <p>
        <asp:Button ID="btnSubmit" runat="server" Text="Send" OnClick="Button1_Click" ValidationGroup="save" />
    </p>
</asp:Panel>
<p>
    <asp:Label ID="lblMsgSend" runat="server" Visible="false" />
</p>

