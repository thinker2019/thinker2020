
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IPv6Client
{
    class CommonClass
    {
        DB db = new DB();
        public CommonClass()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public string MessageBox(string TxtMessage, string Url)
        {
            string str;
            str = "<script language=javascript>alert('" + TxtMessage + "');location='" + Url + "';</script>";
            return str;
        }


        public string MessageBox(string TxtMessage)
        {
            string str;
            str = "<script language=javascript>alert('" + TxtMessage + "')</script>";
            return str;
        }

        public string MessageBoxPage(string TxtMessage)
        {
            string str;
            str = "<script language=javascript>alert('" + TxtMessage + "')</script>";
            return str;
        }
        /// <summary>
        /// 实现随机验证码
        /// </summary>
        /// <param name="n">显示验证码的个数</param>
        /// <returns>返回生成的随机数</returns>
        public string RandomNum(int n) //
        {
            //定义一个包括数字、大写英文字母和小写英文字母的字符串
            string strchar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            //将strchar字符串转化为数组
            //String.Split 方法返回包含此实例中的子字符串（由指定Char数组的元素分隔）的 String 数组。
            string[] VcArray = strchar.Split(',');
            string VNum = "";
            //记录上次随机数值，尽量避免产生几个一样的随机数           
            int temp = -1;
            //采用一个简单的算法以保证生成随机数的不同
            Random rand = new Random();
            for (int i = 1; i < n + 1; i++)
            {
                if (temp != -1)
                {
                    //unchecked 关键字用于取消整型算术运算和转换的溢出检查。
                    //DateTime.Ticks 属性获取表示此实例的日期和时间的刻度数。
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                //Random.Next 方法返回一个小于所指定最大值的非负随机数。
                int t = rand.Next(61);
                if (temp != -1 && temp == t)
                {
                    return RandomNum(n);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;//返回生成的随机数
        }
        /// <summary>
        /// 用来截取小数点后nleng位
        /// </summary>
        /// <param name="sString">sString原字符串。</param>
        /// <param name="nLeng">nLeng长度。</param>
        /// <returns>处理后的字符串。</returns>
        public string VarStr(string sString, int nLeng)
        {
            int index = sString.IndexOf(".");
            if (index == -1 || index + nLeng >= sString.Length)
                return sString;
            else
                return sString.Substring(0, (index + nLeng + 1));
        }
        ///<summary>用来获取一段文字中的数值
        ///</summary>
        ///<param name="str">需要转换的字符串</param>
        ///<returns>处理后的返回值</returns>
        public float GetNum(string str)
        {
            float result = 0;
            if (str != null && str != string.Empty)
            {
                //正则表达式剔除非数字部分
                str = Regex.Replace(str, @"[^\d.\d]", "");
                //如果是数字转化为float型数值
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = float.Parse(str);
                }
            }
            return result;
        }
        ///<summary>
        ///说明：用来去掉数组中重复的元素
        ///参数：str为参数，为需要操作的数组
        ///str1为去掉重复元素的数组
        ///</summary>
        public string[] GetArray(string[] str)
        {

            List<string> listString = new List<string>();
            int n;
            foreach (string eachString in str)
            {
                if (!listString.Contains(eachString))
                    listString.Add(eachString);
            }
            n = listString.Count;
            string[] str1 = new string[n];
            for (int i = 0; i < n; i++)
            {
                str1[i] = listString[i];
            }
            return str1;
        }

        /// <summary>
        /// 说明：ExecSQL用来执行SQL语句。
        ///	返回值：操作是否成功(True\False)。
        ///	参数：sqlStr SQL字符串
        /// </summary>
        public Boolean ExecSQL(string sqlStr)
        {
            SqlConnection myConn = db.GetConnection();
            myConn.Open();
            SqlCommand myCmd = new SqlCommand(sqlStr, myConn);
            try
            {
                myCmd.ExecuteNonQuery();
                myConn.Close();
            }
            catch
            {
                myConn.Close();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 说明：GetDataSet数据集，返回数据源的数据集
        ///	返回值：数据集DataSet
        ///	参数：sqlStr SQL字符串，TableName 数据表名称
        /// </summary>
        public System.Data.DataSet GetDataSet(string sqlStr, string TableName)
        {
            SqlConnection myConn = db.GetConnection();
            myConn.Open();
            SqlDataAdapter adapt = new SqlDataAdapter(sqlStr, myConn);
            DataSet ds = new DataSet();
            adapt.Fill(ds, TableName);
            myConn.Close();
            return ds;
        }
    }
}
