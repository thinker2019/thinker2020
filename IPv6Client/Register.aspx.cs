using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IPv6Client
{
    public partial class Register : System.Web.UI.Page
    {
        MyClass myClass = new MyClass();
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        protected void CreateUserButton_Click(object sender, EventArgs e)
        {
            if (Password.Text.Length >= 5)
            {
                string UserID = myClass.RandomNum(7);

                string Sqlstr = "insert into tb_用户信息(用户ID,用户名,在线状态,密码,用户级别) values ('" + UserID + "','" + UserName.Text + "','离线','" + Password.Text + "','02')";
                myClass.Databaseoperation(Sqlstr);
                Response.Redirect("~/Logo.aspx");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "<script>alert('注册成功！');</script>");

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "<script>alert('请输入至少5位的密码!');</script>");
            }
        }
    }
}