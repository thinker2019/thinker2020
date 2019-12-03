using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IPv6Client
{
    class MyClass
    {
        readonly DB _dbObj = new DB();
        //CommonClass _cObj = new CommonClass();
        private const int PortReceive = 8090;//本地监听接收端口
        public int Sendwaittime = 8000;//每次发送等待时间
        private const int PortRemote = 8080;//网关的接收端口
        private const int PortRemotes = 8080;//管理端的接收端口
        private const string IP = "172.22.146.22";//网关的IP地址
        private const string IPs = "2001::22";//管理端的IPv6地址
        private const string IPv4 = "172.22.146.135";//管理端的IP地址
        private const string IPv6 = "2001::20";//网关的IPv6地址
        private static int sum1 = 0;
        private static int sum2 = 0;
        private static int sum3 = 0;
        private static int sum4 = 0;
        string[] strings = new string[5];
        public string Current;
        public string RequestData;
        private static Queue<byte[]> myqueue_actual = new Queue<byte[]>(1000); //此队列保存抓包器数据

        static bool Bool_Actual_Packets_DataBase_Function = false;//为true时会开始数据存储，为false.......

        static bool Bool_IsCirculate = false;//为true时会开始接受，为false不接受，通过赋值来执行是否接受
        public void Databaseoperation(string smsSqlstr)//操作数据库，传递参数为SQL语句
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture);
            var mySqlConnection = new SqlConnection(connectionString);
            try
            {
                mySqlConnection.Open();//打开数据库的连接               
                var myCommand = new SqlCommand(smsSqlstr, mySqlConnection);//声明SqlCommand对象实例
                myCommand.ExecuteReader();//执行SQL语句生成一个sqlDataReader对象
            }
            catch (SqlException)
            {
                //读取数据库出错输出错误信息
                //System.Web.HttpContext.Current.Response.Write(ex.Message.ToString());
            }
            finally
            {
                mySqlConnection.Close();//关闭与数据库的连接
                //System.Web.HttpContext.Current.Response.Write("关闭数据库成功！");
            }
        }
        public byte[] ConvertData(int recv, byte[] data) //因为接收到的data是1024位，而需要返回一个接收到的recv位
        {
            var newbyte = new byte[recv];
            for (var i = 0; i < recv; i++)
            {
                newbyte[i] = data[i];
            }
            return newbyte;
        }
        private static byte[] StrToToHexByte(string hexString)//字符转换为数组
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            var returnBytes = new byte[hexString.Length / 2];
            for (var i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        public static string ByteToHexStr(byte[] bytes)//数组转换为字符
        {
            string returnStr = "";
            if (bytes.Length != 0)
            {
                //for (int i = 8; i < bytes.Length-2; i++)
                for (var i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2") + " ";
                }
            }
            return returnStr;
        }
        public static string ByteToHexStr5(byte[] bytes) //提取报文头
        {
            string returnStr = "";

            if (bytes.Length != 0)
            {
                //for (int i = 8; i < bytes.Length-2; i++)
                for (var i = 0; i < 2; i++)
                {
                    returnStr += bytes[i].ToString("X2") + " ";
                }
            }
            return returnStr;
        }
        public string RandomNum(int n)//产生固定位的16进制随机数
        {
            var random = new Random();
            var ss = "";
            for (var i = 0; i < n; i++)
            {
                var randomnum = random.Next(0, 255);
                if (randomnum <= 15)
                {
                    ss = ss + "0" + Convert.ToString(randomnum, 16).ToUpper();
                }
                else
                {
                    ss = ss + Convert.ToString(randomnum, 16).ToUpper();
                }
                if (i < n - 1)
                {
                    ss = ss + " ";
                }
            }
            return ss;
        }
        public void RandomNum1(int n)//产生九位的16进制随机数并生成10位的数组
        {

            //var random = new Random();
            string[] strings = new string[9];//用户操作权限
            DataTable dstables = _dbObj.GetDataSetStr("select * from tb_资源对应等级表", "tb_资源对应等级表");//资源类型和资源等级
            DataTable dstable = _dbObj.GetDataSetStr("select * from tb_用户访问资源权限 where 用户ID='" + Current + "'", "tb_用户访问资源权限");//检查是否已有操作权限
            if (dstable.Rows.Count == 0)
            {
                for (var i = 0; i < n; i++)
                {
                    //string ss = dstables.Rows[i]["资源等级"].ToString();
                    if (dstables.Rows[i]["资源等级"].ToString() == "04" || dstables.Rows[i]["资源等级"].ToString() == "03")
                    {
                        strings[i] = "01";
                    }
                    else
                    {
                        strings[i] = "02";
                    }
                }
                for (int j = 0; j < n; j++)
                {
                    string Sqlstr = "insert into tb_用户访问资源权限(用户ID,资源类型,资源等级,操作权限) values ('" + Current + "','" + dstables.Rows[j]["资源类型"] +
                                    "','" + dstables.Rows[j]["资源等级"] + "','" + strings[j] + "')";
                    Databaseoperation(Sqlstr);
                }
                string AuthorityCode = "00 01 " + strings[0] + " 64 01 01 " + strings[1] + " 64 02 01 " + strings[2] +
                                       " 64 03 01 " + strings[3] + " 64 04 01 " +
                                       strings[4] + " 64 05 01 " + strings[5] + " 64 06 01 " + strings[6] + " 64 07 01 " +
                                       strings[7] + " 64 08 01 " +
                                       strings[8] + " 64";
                string Sqlstrs = "insert into tb_用户授权证书(用户ID,授权证书) values ('" + Current + "','" + AuthorityCode + "')";
                Databaseoperation(Sqlstrs);
            }
            else
            {
                string AuthorityCode = "00 " + dstable.Rows[0]["资源等级"] + dstable.Rows[0]["操作权限"] + " 64 01 " + dstable.Rows[1]["资源等级"] + dstable.Rows[1]["操作权限"] + " 64 02 " + dstable.Rows[2]["资源等级"] + dstable.Rows[2]["操作权限"] +
                                       " 64 03 " + dstable.Rows[3]["资源等级"] + dstable.Rows[3]["操作权限"] + " 64 04 " +
                                       dstable.Rows[4]["资源等级"] + dstable.Rows[4]["操作权限"] + " 64 05 " + dstable.Rows[5]["资源等级"] + dstable.Rows[5]["操作权限"] + " 64 06 " + dstable.Rows[6]["资源等级"] + dstable.Rows[6]["操作权限"] + " 64 07 " +
                                       dstable.Rows[7]["资源等级"] + dstable.Rows[7]["操作权限"] + " 64 08 " +
                                       dstable.Rows[8]["资源等级"] + dstable.Rows[8]["操作权限"] + " 64";
                string Sqlstrs = "insert into tb_用户授权证书(用户ID,授权证书) values ('" + Current + "','" + AuthorityCode + "')";
                Databaseoperation(Sqlstrs);
            }
        }

        public void DataReceivev4()
        {
            Bool_IsCirculate = true;
            while (Bool_IsCirculate)
            {
                ReceiveDatav4(PortReceive, Sendwaittime);
            }
        }
        public void DataReceivev6()
        {
            Bool_IsCirculate = true;
            while (Bool_IsCirculate)
            {
                ReceiveDatav6(PortReceive, Sendwaittime);
            }
        }
        public void DataSend()
        {
            byte[] data = new byte[] { };
            string data_head = "66 AC 0C 11 00 ";
            string Senddata = data_head + Current;
            data = StrToToHexByte(Senddata);
            DataTable dt = _dbObj.GetDataSetStr("select * from tb_ComState where CurState='当前状态'", "tb_ComState");
            if (dt.Rows[0]["ComState"].ToString() == "IPv4")
            {
                SendData(data, IPv4, PortRemotes);
            }
            else
            {
                SendDatas(data, IPs, PortRemotes);
            }
        }
        public void DataSenddown()
        {
            byte[] data = new byte[] { };
            string data_head = "66 AC 0C 12 00 ";
            string Senddata = data_head + Current;
            data = StrToToHexByte(Senddata);
            DataTable dt = _dbObj.GetDataSetStr("select * from tb_ComState where CurState='当前状态'", "tb_ComState");
            if (dt.Rows[0]["ComState"].ToString() == "IPv4")
            {
                SendData(data, IPv4, PortRemotes);
            }
            else
            {
                SendDatas(data, IPs, PortRemotes);
            }
        }


        public void RequestSend()
        {
            byte[] data = new byte[] { };
            data = StrToToHexByte(RequestData);
            DataTable dt = _dbObj.GetDataSetStr("select * from tb_ComState where CurState='当前状态'", "tb_ComState");
            if (dt.Rows[0]["ComState"].ToString() == "IPv4")
            {
                SendData(data, IP, PortRemote);
            }
            else
            {
                SendDatas(data, IPv6, PortRemote);
            }
        }

        public void SendData(byte[] strsend, string stringIp, int portLongdis)//IPv4发送数据函数 
        {
            var myUdpClient = new UdpClient();
            var remoteIpAdress = IPAddress.Parse(stringIp);
            var iep = new IPEndPoint(remoteIpAdress, portLongdis);
            try
            {
                myUdpClient.Send(strsend, strsend.Length, iep);
            }
            catch (Exception)
            {
            }
            finally
            {
                myUdpClient.Close();
            }
        }

        public void SendDatas(byte[] strsend, string stringIp, int portLongdis) //IPv6发送数据函数 
        {
            var myUdpClient = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            var remoteIpAdress = IPAddress.Parse(stringIp);
            var iep = new IPEndPoint(remoteIpAdress, portLongdis);
            myUdpClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1);
            try
            {
                myUdpClient.SendTo(strsend, iep);
            }
            catch (Exception)
            {
            }
            finally
            {
                myUdpClient.Close();
            }
        }
        public void ReceiveDatav4(int portRe, int intDelay) //接收数据
        {
            var data = new byte[1024];
            byte[] data1;
            ////////////////////////////////////////////////////////////////////
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var ipep = new IPEndPoint(IPAddress.Any, 8090);
            if (!client.IsBound)
                client.Bind(ipep);
            var messagesender = new IPEndPoint(IPAddress.Any, 0);
            var remote = (EndPoint)(messagesender);

            client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, intDelay);
            var recv = 0;
            try
            {
                recv = client.ReceiveFrom(data, ref remote);
                data1 = new byte[recv];
                for (int i = 0; i < recv; i++)
                {
                    data1[i] = data[i];
                }
                if (recv != 0)
                {
                    data_store_queue(data1, recv);
                }
            }
            catch (SocketException)
            {

            }
            finally
            {
                client.Close();
            }


        }

        public void ReceiveDatav6(int portRe, int intDelay) //接收数据
        {
            var data = new byte[1024];
            byte[] data1;
            ////////////////////////////////////////////////////////////////////
            var client = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            var ipep = new IPEndPoint(IPAddress.IPv6Any, 8090);
            if (!client.IsBound)
                client.Bind(ipep);
            var messagesender = new IPEndPoint(IPAddress.IPv6Any, 0);
            var remote = (EndPoint)(messagesender);

            client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, intDelay);
            var recv = 0;
            try
            {
                recv = client.ReceiveFrom(data, ref remote);
                data1 = new byte[recv];
                for (int i = 0; i < recv; i++)
                {
                    data1[i] = data[i];
                }
                if (recv != 0)
                {
                    data_store_queue(data1, recv);
                }
            }
            catch (SocketException)
            {

            }
            finally
            {
                client.Close();
            }


        }
        public void data_store_queue(byte[] data, int recv)
        {
            if (data.Length != 0)
            {
                string Data_Head = ByteToHexStr5(data);
                if (Data_Head == "A1 A2 ")
                {
                    int i = data.Length;
                    byte[] bytes_queue = new byte[i];
                    for (int j = 0; j < i; j++)
                    {
                        bytes_queue[j] = data[j];
                    }
                    lock (this)
                    {
                        myqueue_actual.Enqueue(bytes_queue);
                    }
                }
            }

        }
        /********************************************************************************/
        //数据处理线程入口
        public void Data_Process()
        {
            //Thread.Sleep(5000);
            byte[] bytes;
            int i_count = myqueue_actual.Count;
            Bool_Actual_Packets_DataBase_Function = true;
            while (Bool_Actual_Packets_DataBase_Function)
            {
                if (myqueue_actual.Count != 0)
                {
                    lock (this)
                    {
                        bytes = myqueue_actual.Dequeue();
                    }
                    DataProcesing(bytes);
                }
            }
        }

        public void DataProcesing(byte[] bytess)
        {
            if (bytess != null)
            {
                int data_length = bytess.Length;
                byte[] data = new byte[data_length];
                data = bytess;
                string receivedata = ByteToHexStr(data);
                string stringtime = Gettime();
                string Data_head = ByteToHexStr5(data); //报文头
                if (Data_head == "A1 A2 " && data.Length == 38) //数据的头部和长度均满足要求才开始处理数据
                {
                    string datalength = Convert.ToString(data.Length);
                    string Temperature = Convert.ToString(data[37]);
                    string NowTime = Gettime();
                    string Sqlstr = "insert into tb_用户访问资源数据(传感器类型,数据信息,采集时间) values ('温度','" + Temperature + "','" + NowTime + "')";
                    Databaseoperation(Sqlstr);
                }
            }
        }

        public string Gettime()//获得当前时间
        {
            //var strtime = DateTime.Now.ToLongTimeString();
            var strtime = DateTime.Now.ToString("HH:mm:ss");
            return strtime;
        }
    }
}
