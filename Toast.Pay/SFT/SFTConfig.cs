using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Toast.Pay
{
    public class SFTConfig
    {
        private static string busid = "";
        private static string key = "";
        private static string input_charset = "";


        static SFTConfig()
        {
            string configStr = ConfigurationManager.AppSettings["App.SFTBank.PayConfig"];
            string[] a = configStr.Split('|');
            //商户号
            busid = a[0];
            //秘钥
            key = a[1];
            //字符编码格式
            input_charset = "utf-8";
        }
        /// <summary>
        /// 商户号
        /// </summary>
        public static string BusID
        {
            get { return busid; }
            set { busid = value; }
        }
        /// <summary>
        /// 秘钥
        /// </summary>
        public static string Key
        {
            get { return key; }
            set { key = value; }
        }
        /// <summary>
        /// 字符编码格式
        /// </summary>
        public static string Input_charset
        {
            get { return input_charset; }
            set { input_charset = value; }
        }
    }
}
