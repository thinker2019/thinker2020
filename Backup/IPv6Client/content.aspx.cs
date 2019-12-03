using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IPv6Client
{
    public partial class content : System.Web.UI.Page
    {
        private Thread thread;
        MyClass myClass = new MyClass();
        DB _db = new DB();
        private static Thread threadreceive;
        private static Thread thread_Process;
        public static string SourceType;
        public static string Operation;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrrentUser.Text = Session["AdmName"].ToString();
                CuttentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                CurrentTime.Text = DateTime.Now.ToString("HH:mm");
                myClass.Current = Session["AdmID"].ToString();
                string Sqlstr = "delete from tb_用户授权证书 where 用户ID='" + Session["AdmID"] + "'";
                myClass.Databaseoperation(Sqlstr);
                string sms_sqlstr删除1 = "delete from tb_用户访问资源数据";
                myClass.Databaseoperation(sms_sqlstr删除1);

                string sqlstr1 = "DBCC CHECKIDENT (tb_用户访问资源数据,  RESEED, 0)";
                myClass.Databaseoperation(sqlstr1);
                //DataTable dt = _db.GetDataSetStr("select * from tb_用户访问资源权限 where 用户ID='" + Session["AdmID"] + "'",
                //                                 "tb_用户访问资源权限");
                //if (dt.Rows.Count != 0)
                //{
                //    RequestAu.Enabled = false;
                //}
                //else
                //{
                //    RequestAu.Enabled = true;
                //}
                Thread threadsend = new Thread(myClass.DataSend);//发送命令给管理端，报告用户上线
                threadsend.Start();
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            CurrentTime.Text = DateTime.Now.ToString("HH:mm");
            GridView2.DataSource = _db.GetDataSetStr("select * from tb_用户访问资源数据 order by 序号 desc", "tb_用户访问资源数据");
            GridView2.DataBind();
            GridView1.DataSource = _db.GetDataSetStr("select * from tb_用户访问资源权限 where 用户ID='" + Session["AdmID"] + "'", "tb_用户访问资源权限");
            GridView1.DataBind();
        }
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            GridView2.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //高亮显示指示行
                e.Row.Attributes.Add("onMouseOver", "Color=this.style.backgroundColor;this.style.backgroundColor='#FF80FF'");
                e.Row.Attributes.Add("onMouseOut", "this.style.backgroundColor=Color;");
                //设置审核状态，并且设置相应的颜色。 
            }
            e.Row.Style.Add("cursor", "pointer");//鼠标手形
            PostBackOptions myPostBackOptions = new PostBackOptions(this);
            //{

            myPostBackOptions.AutoPostBack = false;
            myPostBackOptions.RequiresJavaScriptProtocol = true;
            myPostBackOptions.PerformValidation = false;
            //};
            var evt = Page.ClientScript.GetPostBackClientHyperlink(sender as GridView, "Select$" + e.Row.RowIndex.ToString(CultureInfo.InvariantCulture));
            e.Row.Attributes.Add("onclick", evt);
        }
        protected void Confirm_Click(object sender, EventArgs e)
        {
            string Selected_SourceType = SourceID.SelectedItem.Text;
            string Selected_Opetation = UserOperation.SelectedItem.Text;
            switch (Selected_SourceType)
            {
                case "一氧化碳":
                    SourceType = "00";
                    break;
                case "温度":
                    SourceType = "01";
                    break;
                case "湿度":
                    SourceType = "02";
                    break;
                case "红外":
                    SourceType = "03";
                    break;
                case "震动":
                    SourceType = "04";
                    break;
                case "电表":
                    SourceType = "05";
                    break;
                case "甲烷":
                    SourceType = "06";
                    break;
                case "粉尘":
                    SourceType = "07";
                    break;
                case "烟雾":
                    SourceType = "08";
                    break;
            }
            switch (Selected_Opetation)
            {
                case "读":
                    Operation = "00";
                    break;
                case "写":
                    Operation = "01";
                    break;
                case "读/写":
                    Operation = "02";
                    break;
            }
            DataTable ds = _db.GetDataSetStr("select * from tb_用户授权证书 where 用户ID='" + Session["AdmID"].ToString() + "'", "tb_用户授权证书");
            if (ds.Rows.Count != 0)
            {
                string Certification = ds.Rows[0]["授权证书"].ToString();
                string SendData = "56 AC 39 51 " + Session["AdmID"].ToString() + " " + SourceType + " " + Operation + " 00 " + Session["AdmID"].ToString() + " " + Certification;
                myClass.RequestData = SendData;
                Thread threadRequest = new Thread(myClass.RequestSend);
                threadRequest.Start();
            }
            else
            {
                string SendData = "56 AC 39 51 " + Session["AdmID"].ToString() + " " + SourceType + " " + Operation + " 00" ;
                myClass.RequestData = SendData;
                Thread threadRequest = new Thread(myClass.RequestSend);
                threadRequest.Start();
            }
            
        }
        protected void RequestAu_Click(object sender, EventArgs e)
        {
            
            myClass.Current = Session["AdmID"].ToString();
            DataTable dt = _db.GetDataSetStr("select * from tb_用户授权证书 where 用户ID='" + Session["AdmID"].ToString() + "'", "tb_用户授权证书");
            if (dt.Rows.Count == 0)
            {
                myClass.RandomNum1(9);
            }
            Timer1.Enabled = true;
            //GridView1.DataSource = _db.GetDataSetStr("select * from tb_用户访问资源权限 where 用户ID='" + Session["AdmID"] + "'", "tb_用户访问资源权限");
            //GridView1.DataBind();
            DataTable ds = _db.GetDataSetStr("select * from tb_ComState where CurState='当前状态'", "tb_ComState");
            if (ds.Rows[0]["ComState"].ToString() == "IPv4")
            {
                threadreceive = new Thread(myClass.DataReceivev4);
            }
            else
            {
                threadreceive = new Thread(myClass.DataReceivev6);
            }
            thread_Process = new Thread(myClass.Data_Process);
            threadreceive.Start();
            thread_Process.Start();
            thread_Process.IsBackground = true;
            threadreceive.IsBackground = true;
            RequestAu.Enabled = false;
            //thread.IsBackground = true;
        }

        protected void quit_Click(object sender, EventArgs e)
        {
            myClass.Current = Session["AdmID"].ToString();
            Thread threadsenddown = new Thread(myClass.DataSenddown);//发送命令给管理端，报告用户下线
            threadsenddown.Start();
            Response.Redirect("Logo.aspx");
            Session["AdmName"] = "";
            Session["AdmID"] = "";
        }
    }
}