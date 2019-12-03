<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="content.aspx.cs" Inherits="IPv6Client.content" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>IPv6传感网访问控制客户端</title>
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
    <link rel="stylesheet" href="Styles/style.css" type="text/css"/>
    </head>
<body >
    <form id="form2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <div>
       <table style="border-style: none; width:100%; height:100px; background-image: url('images/top02.gif'); background-repeat: repeat-x; "   border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td style="border-style: none; width:100%; height:100px; background-image: url('images/top02.gif'); background-repeat: repeat-x;"  class="style1">
        <%--<img alt="" src="images/top03.jpg" />--%>
        <span class="style3" style="margin-left: 300px">
        IPv6传感网访问控制客户端</span>
    </td>
  </tr>
    
</table>
   </div>
   <div>             
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>
               <asp:Timer ID="Timer1" runat="server" ontick="Timer1_Tick" Interval="1000" Enabled="false">
               </asp:Timer>
       当前用户：<asp:Label runat="server" ID="CurrrentUser"></asp:Label>  日期：<asp:Label runat="server" ID="CuttentDate"></asp:Label> 时间：<asp:Label runat="server" ID="CurrentTime"></asp:Label>
   </ContentTemplate>
       </asp:UpdatePanel> 
   </div>
   <div style="color: #9370db">
       <marquee direction="left" behavior="Scroll">欢迎登录IPv6传感网访问控制客户端</marquee>
   </div>
   <div>
      <table width="100%" style="text-align: center">
          <tr style="background-color: #66cdaa">
              <td>
                  <asp:DropDownList runat="server" ID="SourceID" CssClass="DropDownListSyle" >
                      <asp:ListItem>一氧化碳</asp:ListItem>
                      <asp:ListItem>温度</asp:ListItem>
                      <asp:ListItem>湿度</asp:ListItem>
                      <asp:ListItem>红外</asp:ListItem>
                      <asp:ListItem>震动</asp:ListItem>
                      <asp:ListItem>电表</asp:ListItem>
                      <asp:ListItem>甲烷</asp:ListItem>
                      <asp:ListItem>粉尘</asp:ListItem>
                      <asp:ListItem>烟雾</asp:ListItem>
                  </asp:DropDownList>
              </td>
              <td>
                  <asp:DropDownList runat="server" ID="UserOperation" CssClass="DropDownListSyle">
                      <asp:ListItem>读</asp:ListItem>
                      <asp:ListItem>写</asp:ListItem>
                      <asp:ListItem>读/写</asp:ListItem>
                  </asp:DropDownList>
              </td>
              <td><asp:Button runat="server" ID="Confirm" CssClass="buttonstyle" Text="访问数据" 
                      onclick="Confirm_Click"/></td>
          </tr>
          <tr>
              <td colspan="3">
                  <table width="100%">
                      <tr>
                          <td style="width: 50%;text-align: left" valign="top">
                              <asp:Button ID="RequestAu" runat="server" Text="授权请求" CssClass="buttonstyle" 
                                  onclick="RequestAu_Click"/>
                              <asp:Panel ID="Panel1" runat="server" Height="510px">
                      <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                <asp:GridView ID="GridView1" runat="server" BackColor="White" onrowdatabound="GridView1_RowDataBound"
                                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"  PageSize="15" 
                                    
                                    AutoGenerateColumns="False" Height="200px" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="用户ID" HeaderText="用户ID" Visible="False"
                                            HeaderStyle-Height="40px" >
                                        <HeaderStyle Height="40px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="资源类型" HeaderText="资源类型"
                                            HeaderStyle-Height="40px" >
                                        <HeaderStyle Height="40px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="资源等级" HeaderText="资源等级"
                                        HeaderStyle-Height="40px">
                                        <HeaderStyle Height="40px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="操作权限" HeaderText="操作权限" 
                                            HeaderStyle-Height="40px"  >
                                        <HeaderStyle Height="40px" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#66CCFF" Font-Bold="True" ForeColor="Black" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" Height="25px" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="GridView1" />
                            </Triggers>
                            </asp:UpdatePanel>
                  </asp:Panel>
                          </td>
                          <td>
                              <div style="height: 36px;text-align:right"><asp:Button ID="quit" runat="server" Text="退出" 
                                      CssClass="buttonstyle" onclick="quit_Click" 
                                  /></div>
                              <asp:Panel ID="Panel2" runat="server" Height="510px" ScrollBars="Auto">
                      <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                <asp:GridView ID="GridView2" runat="server" BackColor="White" onrowdatabound="GridView1_RowDataBound"
                                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"  
                                    OnPageIndexChanging="GridView2_PageIndexChanging" AllowPaging="False" 
                                    AutoGenerateColumns="False" Height="120px" Width="100%">
                                    <Columns>
                                        
                                        <asp:BoundField DataField="传感器类型" HeaderText="传感器类型"
                                            HeaderStyle-Height="20px" >
                                        <HeaderStyle Height="20px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="数据信息" HeaderText="数据信息"
                                        HeaderStyle-Height="20px">
                                        <HeaderStyle Height="20px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="采集时间" HeaderText="采集时间" 
                                            HeaderStyle-Height="20px"  >
                                        <HeaderStyle Height="20px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="序号" HeaderText="序号" Visible="false"
                                            HeaderStyle-Height="20px" >
                                        <HeaderStyle Height="20px" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#66CCFF" Font-Bold="True" ForeColor="Black" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" Height="25px" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="GridView1" />
                            </Triggers>
                            </asp:UpdatePanel>
                  </asp:Panel>
                          </td>
                      </tr>
                  </table>
                  
              </td>
          </tr>
      </table>
   </div>
    </form>
</body>
</html>
