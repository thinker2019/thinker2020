<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="IPv6Client.Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
         .style1
    {
        font-family: 华文行楷;
        font-size: 50px;
    }
    .style3
        {
            color: #66CCFF;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table style="border-style: none; width:100%; height:147px; background-image: url('../images/top02.gif'); background-repeat: repeat-x; "   border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td height="147" style="border-style: none; width:100%; height:147px; background-image: url('../images/top02.gif'); background-repeat: repeat-x;"  class="style1">
        <%--<img alt="" src="images/top03.jpg" />--%>
        <span class="style3" style="margin-left: 300px">
        欢迎注册IPv6传感网系统</span><br class="style3" />
    </td>
  </tr>
    
</table>
    <div>
        <div style="margin-left: 300px">
           <span class="failureNotification" style="margin-top: 100px">
                <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server" CssClass="failureNotification" 
                    ValidationGroup="RegisterUserValidationGroup"/> 
        </div>
            
            <div class="accountInfo">
         <fieldset class="register" style="width: 50%;margin-left: 300px">
        <legend style="font-size: medium; font-weight: bold; color: #0066FF">帐户信息</legend>
        <p>
            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">用 户 名:</asp:Label>
            <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                    CssClass="failureNotification" ErrorMessage="必须填写“用户名”。" ToolTip="必须填写“用户名”。" 
                    ValidationGroup="RegisterUserValidationGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
        </p>
        <p>
            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">密      码:</asp:Label>
            <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                    CssClass="failureNotification" ErrorMessage="必须填写“密码”。" ToolTip="必须填写“密码”。" 
                    ValidationGroup="RegisterUserValidationGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
        </p>
        <p>
            <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">确认密码:</asp:Label>
            <asp:TextBox ID="ConfirmPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="ConfirmPassword" CssClass="failureNotification" Display="Dynamic" 
                    ErrorMessage="必须填写“确认密码”。" ID="ConfirmPasswordRequired" runat="server" 
                    ToolTip="必须填写“确认密码”。" ValidationGroup="RegisterUserValidationGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
            <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                    CssClass="failureNotification" Display="Dynamic" ErrorMessage="“密码”和“确认密码”必须匹配。"
                    ValidationGroup="RegisterUserValidationGroup" ForeColor="Red">*</asp:CompareValidator>
        </p>
    </fieldset>
    <p class="submitButton" style="margin-left: 300px">
        <asp:Button ID="CreateUserButton" runat="server" CommandName="MoveNext" Text="创建用户" 
                ValidationGroup="RegisterUserValidationGroup" 
            onclick="CreateUserButton_Click"/>
    </p>
            </div>
    </div>
    </form>
</body>
</html>
