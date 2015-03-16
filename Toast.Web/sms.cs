using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Toast.Web
{
    public class sms
    {
        public string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < 4; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('0' + (char)(number % 10));
                //code = (char)('A' + (char)(number % 26));

                checkCode += code.ToString();
            }

            return checkCode;
        }
        public string SendSMS2(int sendId, string mobile, string content)
        {
            string msg = HttpUtility.UrlEncode(content, Encoding.GetEncoding("gb2312"));
            string Url ="" ;//Confg.ReadSetting("Yeel.SMS.SendSM.Url", "") + "&mobile=" + mobile + "&msg=" + msg;//发送短信地址（接口）
            string postData = string.Empty;
            string pageCode = "gb2312";
            string result = GetPageHtml(Url, "", pageCode);
            if (string.IsNullOrEmpty(result)) result = string.Empty;
            else result = result.Trim().Trim(new char[] { '&' }).Replace("result=", "");
            if (result != "1")
            {

              //短信发送失败错误日志  LogManager.WriteLogToFile("ShortMessage", "SendSMS2", "发送短信(id=" + sendId.ToString() + ",mobile:" + mobile + ",content:" + content + ")失败" + GetSendSMSResultDes(result));
            }
            return result;
        }

        /// <summary>
        /// Get
        /// </summary>
        private string GetPageHtml(
               string strURL,
               string strReferer,
               string strPageCode)
        {
            string strResult = "";
            try
            {

                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(strURL);
                Request.ContentType = "text/html";
                Request.Method = "GET";
                Request.Referer = strReferer;
                Request.Timeout = 60000;
                //
                HttpWebResponse Response = null;
                System.IO.StreamReader sr = null;
                Response = (HttpWebResponse)Request.GetResponse();
                Encoding defaultEncoding;
                if (strPageCode.Trim() == "")
                    defaultEncoding = Encoding.Default;
                else
                    defaultEncoding = Encoding.GetEncoding(strPageCode);
                sr = new System.IO.StreamReader(Response.GetResponseStream(), defaultEncoding);
                strResult = sr.ReadToEnd();

                Request.Abort();
                Response.Close();
            }
            catch (Exception ex)
            {
                strResult = "发送短信出现异常，异常描述：" + ex.Message;
            }
            return strResult;
        }

        /// <summary>
        /// Post
        /// </summary>
        private static string PostPageHtml(string strURL,
               string strArgs,
               string strReferer,
               string strPageCode)
        {
            string strResult = "";
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(strURL);
            Request.AllowAutoRedirect = true;
            Request.KeepAlive = true;
            Request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/msword, application/x-shockwave-flash, */*";
            Request.Referer = strReferer;

            Request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 2.0.50727)";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Method = "POST";

            Stream MyRequestStrearm = Request.GetRequestStream();
            StreamWriter MyStreamWriter = new StreamWriter(MyRequestStrearm, Encoding.GetEncoding(strPageCode));//这里需要改一下
            MyStreamWriter.Write(strArgs);
            MyStreamWriter.Close();
            MyRequestStrearm.Close();
            HttpWebResponse Response = null;
            System.IO.StreamReader sr = null;
            Response = (HttpWebResponse)Request.GetResponse();
            sr = new System.IO.StreamReader(Response.GetResponseStream(), Encoding.GetEncoding(strPageCode));
            strResult = sr.ReadToEnd();
            Request.Abort();
            Response.Close();
            return strResult;
        }
        /*调用时用
                 string SN = Confg.ReadSetting("Plate_Msg_SN", "");
                string pwd = Confg.ReadSetting("Plate_Msg_Pwd", "");
                string content = Confg.ReadSetting("Plate_MsgContent_Unbind", "");
                string vcode = GenerateCheckCode();
                content = content.Replace("#Vcode#", vcode);//
                int id = manager.SaveMsgInfo1(UserCode, phone, content);
                ShortMessage sm = new ShortMessage();
                string result = sm.SendSMS2(id, phone, content);
                string ErrorMsg = sm.GetSendSMSResultDes(result);
              
                if (result != "1")
                {
                    MessageBox.Show(this, "系统出错，请稍后在试！");
                    this.WriteLogToFile("btnsumbit_Click", "发送短信失败" + ErrorMsg);
                    new ShortMessage().PhoneMsgSendDeal(id, ErrorMsg, result.ToString(),-1);
                    return;
                }
         */

        /*
        <add key="Yeel.SMS.SendSM.Url" value="http://si.800617.com:4400/SendSms.aspx?un=zjylkj-1&amp;pwd=36cb3e"/>
        <add key="Plate_MsgContent_Bind" value="您正在使用业联平台 yeelnet.com 手机绑定服务，验证码：#Vcode#，为保证您账号安全，此短信有效时间为10分钟。"/>
      <add key="Plate_MsgContent_Unbind" value="您正在使用业联平台 yeelnet.com 手机解绑服务，验证码：#Vcode#，为保证您账号安全，此短信有效时间为10分钟。"/>
      <add key="Plate_DeductJinbi_Count" value="90000"/>
     
      <add key="Plate_Msg_SN" value="zjylkj-1"/>
      <add key="Plate_Msg_Pwd" value="36cb3e"/>
         */
    }
}