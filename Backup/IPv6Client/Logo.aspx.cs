using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IPv6Client
{
    public partial class Logo : System.Web.UI.Page
    {
        CommonClass ccObj = new CommonClass();
        DB db = new DB();
        MyClass myClass = new MyClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string txtAdmName = this.txtAdmName.Value;
            string txtPassword = this.txtPassword.Value;
            string Num = this.txtVailumNum.Value.Trim();
            if (Session["ValidateNum"].ToString() == Num.ToUpper())
            {
                DataTable dt = db.GetDataSetStr("select *  from tb_用户信息  where 用户名='" + txtAdmName + "'", "tb_用户信息");
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][2].ToString() == txtPassword)
                    {
                        Session["AdmName"] = dt.Rows[0]["用户名"].ToString();
                        Session["AdmID"] = dt.Rows[0][0].ToString();
                        Response.Write(ccObj.MessageBox("登陆成功！", "content.aspx"));
                        Response.Redirect("content.aspx");
                    }
                    else
                    {
                        Response.Write(ccObj.MessageBox("请输入正确密码！", "Logo.aspx"));
                    }
                }
                else
                {
                    Response.Write(ccObj.MessageBox("请输入正确用户名！", "Logo.aspx"));
                }
            }
            else
            {
                Response.Write(ccObj.MessageBox("请输入正确验证码", "Logo.aspx"));
            }

        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            this.txtAdmName.Value = "";
            this.txtPassword.Value = "";
            this.txtVailumNum.Value = "";
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
}