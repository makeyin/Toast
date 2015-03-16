using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Toast.Utility
{
 public class LogHelper
    {
        /// <summary>
        /// 错误日志记录 by:冯岩
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="method">方法名称</param>
        public static void SaveErrorLog(string message, string method)
        {

            StringBuilder str = new StringBuilder();
            if (null != HttpContext.Current)
            {
                str.AppendFormat("Ulr:{0}", HttpContext.Current.Request.Url.ToString());
                str.AppendFormat("\r\nIP:{0}", HttpContext.Current.Request.UserHostAddress.ToString());
                str.AppendFormat("\r\nMethod:{0}", method);
                str.AppendFormat("\r\nError:{0}", message);
                str.AppendFormat("\r\nTime:{0}", DateTime.Now.ToString());
                str.Append("\r\n\r\n=================================================================================\r\n\r\n");
                string savePath = HttpContext.Current.Server.MapPath("~/Error");
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                File.AppendAllText(savePath + "/" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".log", str.ToString());
            }
            else
            {
                string savePath = "C:/App_Data/WebLog";
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                str.AppendFormat("Method:{0}\r\nError:{1}\r\nTime:{2}", method, message, DateTime.Now.ToString());
                str.Append("\r\n\r\n=================================================================================\r\n\r\n");
                File.AppendAllText(savePath + "/" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".log", str.ToString());
            }

        }
        /// <summary>
        /// 操作日志 by：冯岩
        /// </summary>
        /// <param name="operInfo">操作信息</param>
        public static void SaveOperLog(string operInfo)
        {
            if (null != HttpContext.Current)
            {
                string savePath = HttpContext.Current.Server.MapPath("~/Log");
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                File.AppendAllText(savePath + "/" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".log", operInfo);
            }
            else
            {
                string savePath = "C:/App_Data/OperationLog";
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                File.AppendAllText(savePath + "/" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".log", operInfo);
            }
        }

        public static void WriteLogToFile(string pageName, string methodName, string error)
        {
            string filePath = MapPath(System.Configuration.ConfigurationSettings.AppSettings["Yeel.LogFile.Path"].ToString());

            if (HttpContext.Current != null)
            {
               // Logger.WriteLogToFile(filePath, pageName, methodName, error);
            }
            else
            {
                SaveExceptionToD(pageName + methodName, error, "52jiuqu");
            }


        }
        public static void WriteLogToFile(string pageName, string methodName, string error, string logname)
        {
            string filePath = MapPath(System.Configuration.ConfigurationSettings.AppSettings["Yeel.LogFile.Folder"].ToString()+ logname);

            if (HttpContext.Current != null)
            {
               // Logger.WriteLogToFile(filePath, pageName, methodName, error);
            }
            else
            {
                SaveExceptionToD(pageName + methodName, error, "52jiuqu");
            }
        }

        public static string MapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    //strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                    strPath = strPath.TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 直接写入d盘App_Data/Err
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method"></param>
        public static void SaveExceptionToD(string ex, string method, string wjjName)
        {
            string message = ex.ToString() + method + "\r\n" + DateTime.Now.ToString() + "\r\n\r\n=======================================================================================\r\n\r\n";
            string logname = "D:/App_Data/" + wjjName + "/" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".log";
            System.IO.File.AppendAllText(logname, message);
        }

        /// <summary>
        /// 游戏币充值日志记录
        /// </summary>
        /// <param name="method">方法名</param>
        /// <param name="operation">操作记录</param>
        /// <param name="ex">错误信息</param>
        public static void GameCurrencyRechargeOperationLog(string method, string operation)
        {
            string filePath = MapPath( System.Configuration.ConfigurationSettings.AppSettings["GamePayLogFile.Path"].ToString());
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            StringBuilder str = new StringBuilder();
            str.Append("\r\n=================================================================================\r\n");
            str.AppendFormat("Method:{0}\r\nOperation:{1}\r\nTime:{2}", method, operation, DateTime.Now.ToString());
            str.Append("\r\n\r\n=================================================================================\r\n\r\n");
            File.AppendAllText(filePath + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".log", str.ToString());
        }
        /// <summary>
        /// 充值操作日志
        /// </summary>
        /// <param name="method"></param>
        /// <param name="operation"></param>
        public static void RechargeOperationLog(string method, string operation)
        {
            string filePath = MapPath(System.Configuration.ConfigurationSettings.AppSettings["PayLogFile.Path"].ToString()); ;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            StringBuilder str = new StringBuilder();
            str.Append("\r\n=================================================================================\r\n");
            str.AppendFormat("Method:{0}\r\nOperation:{1}\r\nTime:{2}", method, operation, DateTime.Now.ToString());
            str.Append("\r\n\r\n=================================================================================\r\n\r\n");
            File.AppendAllText(filePath + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".log", str.ToString());
        }

        /// <summary>
        /// 充值错误日志
        /// </summary>
        /// <param name="method"></param>
        /// <param name="operation"></param>
        /// <param name="ex"></param>
        public static void RechargeErrorLog(string method, string operation, string ex)
        {
            string filePath = MapPath(System.Configuration.ConfigurationSettings.AppSettings["PayErrorFile.Path"].ToString());
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            StringBuilder str = new StringBuilder();
            str.Append("\r\n=================================================================================\r\n");
            str.AppendFormat("Method：{0}\r\nOperation：{1}\r\nError：{3}\r\nTime：{2}", method, operation, DateTime.Now.ToString(), ex);
            str.Append("\r\n\r\n=================================================================================\r\n\r\n");
            File.AppendAllText(filePath + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".log", str.ToString());
        }

        public static void TestLog(string msg)
        {
            string filePath = MapPath("/Test/Pay/");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            StringBuilder str = new StringBuilder();
            str.Append("\r\n=================================================================================\r\n");
            str.AppendFormat("Method：{0}\r\nOperation：{1}\r\nTime：{2}", "", msg, DateTime.Now.ToString());
            str.Append("\r\n\r\n=================================================================================\r\n\r\n");
            File.AppendAllText(filePath + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".log", str.ToString());
        }

    }
}
