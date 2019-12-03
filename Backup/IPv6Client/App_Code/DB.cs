using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IPv6Client
{
    class DB
    {
        public DB()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns>返回SqlConnection对象</returns>
        public SqlConnection GetCon()
        {
            return new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        ///<param name="cmdstr">SQL语句</param>
        /// <returns>返回值为int型：成功返1，失败返回0</returns>
        public int sqlEx(string cmdstr)
        {
            SqlConnection con = GetCon();//连接数据库
            con.Open();//打开连接
            SqlCommand cmd = new SqlCommand(cmdstr, con);
            try
            {
                cmd.ExecuteNonQuery();//执行SQL 语句并返回受影响的行数
                return 1;//成功返回１
            }
            catch (Exception e)
            {
                return 0;//失败返回０
            }
            finally
            {
                con.Dispose();//释放连接对象资源
            }
        }

        /// <summary>
        /// 执行SQL查询语句
        /// </summary>
        /// <param name="cmdstr">查询语句</param>
        /// <returns>返回DataTable数据表</returns>
        public DataTable reDt(string cmdstr)
        {
            SqlConnection con = GetCon();
            SqlDataAdapter da = new SqlDataAdapter(cmdstr, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return (ds.Tables[0]);
        }
        /// <summary>
        /// 执行SQL查询语句
        /// </summary>
        /// <param name="str">查询语句</param>
        /// <returns>返回SqlDataReader对象dr</returns>
        public SqlDataReader reDr(string str)
        {
            SqlConnection conn = GetCon();//连接数据库
            conn.Open();//并打开了连接
            SqlCommand com = new SqlCommand(str, conn);
            SqlDataReader dr = com.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;//返回SqlDataReader对象dr
        }
        /// <summary>
        /// 执行SQL查询语句
        /// </summary>
        /// <param name="cmdstr">查询语句</param>
        /// <returns>返回DataTable数据表</returns>
        public DataSet reDs(string cmdstr)
        {
            SqlConnection con = GetCon();
            SqlDataAdapter da = new SqlDataAdapter(cmdstr, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strPwd">被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public string GetMD5(string strPwd)
        {
            string pwd = "";
            //实例化一个md5对象
            MD5 md5 = MD5.Create();
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(strPwd));
            //翻转生成的MD5码        
            s.Reverse();
            //通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            //只取MD5码的一部分，这样恶意访问者无法知道取的是哪几位
            for (int i = 3; i < s.Length - 1; i++)
            {
                //将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                //进一步对生成的MD5码做一些改造
                pwd = pwd + (s[i] < 198 ? s[i] + 28 : s[i]).ToString("X");
            }
            return pwd;
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns>返回SqlConnection对象</returns>
        public SqlConnection GetConnection()
        {
            string myStr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection myConn = new SqlConnection(myStr);
            return myConn;
        }
        /// <summary>
        /// 执行SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="myCmd">执行SQL语句命令的SqlCommand对象</param>
        public void ExecNonQuery(SqlCommand myCmd)
        {
            try
            {
                if (myCmd.Connection.State != ConnectionState.Open)
                {
                    myCmd.Connection.Open(); //打开与数据库的连接
                }
                //使用SqlCommand对象的ExecuteNonQuery方法执行SQL语句，并返回受影响的行数
                myCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (myCmd.Connection.State == ConnectionState.Open)
                {
                    myCmd.Connection.Close(); //关闭与数据库的连接
                }
            }
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。 
        /// </summary>
        /// <param name="myCmd"></param>
        /// <returns>执行SQL语句命令的SqlCommand对象</returns>
        public string ExecScalar(SqlCommand myCmd)
        {
            string strSql;
            try
            {
                if (myCmd.Connection.State != ConnectionState.Open)
                {
                    myCmd.Connection.Open(); //打开与数据库的连接
                }
                //使用SqlCommand对象的ExecuteScalar方法执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。 

                strSql = Convert.ToString(myCmd.ExecuteScalar());
                return strSql;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (myCmd.Connection.State == ConnectionState.Open)
                {
                    myCmd.Connection.Close();//关闭与数据库的连接
                }
            }
        }
        /// <summary>
        /// 说  明：  返回数据集的表的集合
        ///	返回值：  数据源的数据表
        ///	参  数：  myCmd 执行SQL语句命令的SqlCommand对象，TableName 数据表名称
        /// </summary>



        ///返回具体字符
        public string getdetail(string user, string cell, string table)
        {
            string cmdstr = "Select " + cell + " from " + table + " where 姓名='" + user + "'";
            // "Select "+cell+" from " + table + "where UserN='"+user+"'";
            SqlCommand myCmd = new SqlCommand(cmdstr, GetCon());
            string strdata = ExecScalar(myCmd);
            return strdata;
        }
        public string getdetail1(string user, string cell, string table)
        {
            string cmdstr = "Select " + cell + " from " + table + " where StafName='" + user + "'";
            // "Select "+cell+" from " + table + "where UserN='"+user+"'";
            SqlCommand myCmd = new SqlCommand(cmdstr, GetCon());
            string strdata = ExecScalar(myCmd);
            return strdata;
        }
        public DataTable GetDataSet(SqlCommand myCmd, string TableName)
        {
            SqlDataAdapter adapt;
            DataSet ds = new DataSet();
            try
            {
                if (myCmd.Connection.State != ConnectionState.Open)
                {
                    myCmd.Connection.Open();
                }
                adapt = new SqlDataAdapter(myCmd);
                adapt.Fill(ds, TableName);
                return ds.Tables[TableName];

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
            finally
            {
                if (myCmd.Connection.State == ConnectionState.Open)
                {
                    myCmd.Connection.Close();

                }
            }

        }
        /// <summary>
        /// 执行存储过程语句，返回sqlCommand类对象
        /// </summary>
        /// <param name="strProcName">存储过程名</param>
        /// <returns>返回sqlCommand类对象</returns>
        public SqlCommand GetCommandProc(string strProcName)
        {
            SqlConnection myConn = GetConnection();
            SqlCommand myCmd = new SqlCommand();
            myCmd.Connection = myConn;
            myCmd.CommandText = strProcName;
            myCmd.CommandType = CommandType.StoredProcedure;
            return myCmd;
        }
        /// <summary>
        /// 执行查询语句，返回sqlCommand类对象
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <returns>返回sqlCommand类对象</returns>
        public SqlCommand GetCommandStr(string strSql)
        {
            SqlConnection myConn = GetConnection();
            SqlCommand myCmd = new SqlCommand();
            myCmd.Connection = myConn;
            myCmd.CommandText = strSql;
            myCmd.CommandType = CommandType.Text;
            return myCmd;
        }
        /// <summary>
        /// 说  明：  执行SQL语句，返回数据源的数据表
        ///	返回值：  数据源的数据表DataTable
        ///	参  数：  sqlStr执行的SQL语句，TableName 数据表名称
        /// </summary>
        public DataTable GetDataSetStr(string sqlStr, string TableName)
        {
            SqlConnection myConn = GetConnection();
            myConn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapt = new SqlDataAdapter(sqlStr, myConn);
            adapt.Fill(ds, TableName);
            myConn.Close();
            return ds.Tables[TableName];
        }
    }
}
