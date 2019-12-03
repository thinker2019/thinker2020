<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logo.aspx.cs" Inherits="IPv6Client.Logo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>欢迎IPv6传感网客户端登陆系统</title>
    <style type="text/css">

body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}

     .style1
    {
        font-family: 华文行楷;
        font-size: 50px;
    }

    #ImgvailNum
    {
        width: 75px;
    }

    .style2
    {
        font-family: 宋体, Arial, Helvetica, sans-serif;
    }

        .style3
        {
            color: #66CCFF;
        }

    </style>
    <script type="text/javascript">

        function change() {
            var img = document.getElementById("ImgvailNum");
            img.src = img.src + '?';
        }

</script>
<link href="Styles/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <table style="border-style: none; width:100%; height:147px; background-image: url('images/top02.gif'); background-repeat: repeat-x; "   border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td height="147" style="border-style: none; width:100%; height:147px; background-image: url('images/top02.gif'); background-repeat: repeat-x;"  class="style1">
        <%--<img alt="" src="images/top03.jpg" />--%>
        <span class="style3" style="margin-left: 350px">
        IPv6传感网</span><br class="style3" />
        <span class="style3" style="margin-left: 600px">客户端登陆系统</span>
    </td>
  </tr>
    
</table>
<table width="562" border="0" align="center" cellpadding="0" cellspacing="0" class="right-table03">
  <tr>
    <td width="221">
        <table width="95%" border="0" cellpadding="0" cellspacing="0" class="login-text01">
      
      <tr>
        <td><table width="100%" border="0" cellpadding="0" cellspacing="0" class="login-text01">
          <tr>
            <td align="center"><img alt="" src="images/ico13.gif" 
                    style="height: 151px; width: 148px" /></td>
          </tr>
          <tr>
            <td height="40" align="center">&nbsp;</td>
          </tr>
          
        </table></td>
        <td><img src="images/line01.gif" width="5" height="292" /></td>
      </tr>
    </table>
    </td>
    <td>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td width="31%" height="35" class="login-text02">用 户 名：<br /></td>
        <td width="69%"><input  runat="server" id="txtAdmName" type="text" size="30" style="width: 218px" /></td>
      </tr>
      <tr>
        <td height="35" class="login-text02">密　    码：<br /></td>
        <td><input  runat="server" type="password" id="txtPassword" size="30" style="width: 218px" /></td>
      </tr>
      <tr>
        <td height="35" class="login-text02">验证图片：<br /></td>
        <td><img alt="" id="ImgvailNum" src="ValidateNum.aspx" height="20" /> <a href="javascript:change();" class="login-text03">看不清楚，换张图片</a></td>
      </tr>
      <tr>
        <td height="35" class="login-text02">验 证 码：</td>
       
        <td><input type="text" runat="server" id="txtVailumNum" size="30" style="width: 218px" /></td>
      </tr>
      <tr>
        <td height="35">&nbsp;</td>
        <td >
            <asp:Button ID="btnLogin" runat="server"   Text="登陆" 
                onclick="btnLogin_Click"/>
            <asp:Button ID="btnReset" runat="server"    Text="重置"  
                onclick="btnReset_Click"/>
                <asp:Button ID="btnRegister" runat="server"    Text="注册"  
                onclick="btnRegister_Click"/>
            </td>
      </tr>
    </table>
    </td>
  </tr>
</table>
<table style="border-style: none; width:100%; height:30px; text-align:center;color: #7F9DB9;" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td style="height: 60px"></td>
    </tr>
    <tr>
    <td>
        <span class="style2">重庆邮电大学工业物联网与网络化控制教育部重点实验室</span><br class="style2" />
        <span class="style2">重庆市南岸区崇文路2号，邮编：400065</span>
    </td>
    </tr>

</table>
    </form>
</body>
</html>
