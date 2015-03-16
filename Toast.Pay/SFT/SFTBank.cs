using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using Toast.Utility;

namespace Toast.Pay
{
    public class SFTBank : PayPlatForm
    {
        #region 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="retBillValue"></param>
        /// <param name="bankCode">支付完成后，按照原样返回给商户</param>
        /// <param name="operType">支付渠道，PayType类型确定</param>
        /// <param name="operCode">银行编码</param>
        /// <param name="operValue"></param>
        /// <returns></returns>
        public OpResult SetParamtersByInterface(OpResult retBillValue, string bankCode, string operType, string operCode, string operValue)
        {
            OpResult retValue = new OpResult();
            retValue.HasError = true;

            //条件如果是有错误 或者 消费金额返回 0 或者订单状态不等于0
            //if (retBillValue.HasError || retBillValue.GetIntValue("Amount") <= 0 || retBillValue.GetIntValue("Status") != 0)
            //{
            //    retValue.Message = "不存在该" + retBillValue.GetStringValue("BillNo") + "号 或该单已经处理了";
            //    return retValue;
            //}
            string SFTPayConfig = System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.PayConfig"].ToString(); 

            string[] c = SFTPayConfig.Split('|');
            //由盛付通提供,默认为:商户号(由盛付通提供的6位正整数),用于盛付通判别请求方的身份
            string MsgSender = c[0];
            string key = c[1];//密钥

            string Name = "B2CPayment";
            string Version = "V4.1.1.1.1";
            string Charset = "UTF-8";

            string SendTime1 = DateTime.Now.ToString("yyyyMMddHHmmss");
            string SendTime = SendTime1;
            string OrderNo = retBillValue.GetStringValue("OrderNo", "");
            string OrderAmount = retBillValue.GetStringValue("Amount", "");
            string OrderTime1 = retBillValue.GetDateTimeValue("ChargeDate").ToString("yyyyMMddHHmmss");
            string OrderTime = OrderTime1;
            string PayType = System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.payTypeSFT"].ToString();
            string PayChannel = operType;
            string InstCode = operCode;


            //设置回调地址，发货通知地址
            string PageUrl = System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.postBackURL"].ToString();
            string NotifyUrl = System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.notifyURL"].ToString();

            string ProductName = System.Configuration.ConfigurationSettings.AppSettings["Yeel.RechargeOnLine.ProductName"].ToString();



            //string operType, string operCode
            string BuyerIp = "10.135.12.20";
            string Ext1 = bankCode;
            string SignType = "MD5";

            //加密数据串

            // string PayType = "";
            // string InstCode = "";
            string BuyerContact = "";

            string testStr = Name + Version + Charset + MsgSender + SendTime
                + OrderNo + OrderAmount + OrderTime + PayType + PayChannel + InstCode + PageUrl
                + NotifyUrl + ProductName + BuyerContact + BuyerIp +
                  Ext1 + SignType + key;



            MD5 myMD5 = new MD5CryptoServiceProvider();
            byte[] signed = myMD5.ComputeHash(Encoding.GetEncoding("gbk").GetBytes(testStr));
            string signResult = byte2mac(signed);//Convert.ToBase64String(signed);
            string SignMsg = signResult.ToUpper();

            StringBuilder posthtml = new StringBuilder();
            posthtml.Append("<form id=\"BankPost\" method=\"post\" action=\"" + System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.paymentGateWayURL"].ToString() + "\"/>");
            posthtml.Append("<input type=\"hidden\" id=\"Name\" name=\"Name\" value=\"");
            posthtml.Append(Name);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"Version\" name=\"Version\"  value=\"");
            posthtml.Append(Version);

            posthtml.Append("\"/><input type=\"hidden\"  id=\"Charset\" name=\"Charset\" value=\"");
            posthtml.Append(Charset);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"MsgSender\" name=\"MsgSender\" value=\"");
            posthtml.Append(MsgSender);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"SendTime\" name=\"SendTime\" value=\"");
            posthtml.Append(SendTime);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"OrderNo\" name=\"OrderNo\" value=\"");
            posthtml.Append(OrderNo);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"OrderAmount\" name=\"OrderAmount\" value=\"");
            posthtml.Append(OrderAmount);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"OrderTime\" name=\"OrderTime\" value=\"");
            posthtml.Append(OrderTime);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"PayType\" name=\"PayType\" value=\"");
            posthtml.Append(PayType);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"PayChannel\" name=\"PayChannel\" value=\"");
            posthtml.Append(PayChannel);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"InstCode\" name=\"InstCode\" value=\"");
            posthtml.Append(InstCode);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"PageUrl\" name=\"PageUrl\" value=\"");
            posthtml.Append(PageUrl);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"NotifyUrl\" name=\"NotifyUrl\" value=\"");
            posthtml.Append(NotifyUrl);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"ProductName\" name=\"ProductName\" value=\"");
            posthtml.Append(ProductName);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"BuyerContact\" name=\"BuyerContact\" value=\"");
            posthtml.Append(BuyerContact);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"BuyerIp\" name=\"BuyerIp\" value=\"");
            posthtml.Append(BuyerIp);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"Ext1\" name=\"Ext1\" value=\"");
            posthtml.Append(Ext1);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"SignType\" name=\"SignType\" value=\"");
            posthtml.Append(SignType);
            posthtml.Append("\"/><input type=\"hidden\"  id=\"SignMsg\" name=\"SignMsg\" value=\"");
            posthtml.Append(SignMsg);
            posthtml.Append("\"/><input type=\"submit\"  value=\"提 交\">");
            posthtml.Append("</form>");

            retValue.PutValue("posthtml", posthtml.ToString());
            //**日志 LogHelper.SaveOperLog(posthtml.ToString());
            retValue.HasError = false;
            return retValue;
        }
        #endregion

        public override OpResult GetPayResult(HttpRequest request, OpResult retRequest)
        {
            string logname = "PayCenterLog.txt";
            OpResult retValue = new OpResult();   //定义一个用户信息的对象
            retValue.HasError = true;

            retValue.Message = "#0 初始化";
            retValue.PutValue("BankStatus", "N");


            float amount;
            if (float.TryParse(retRequest.GetStringValue("OrderAmount"), out amount))
            {
                amount = retRequest.GetFloatValue("OrderAmount");
            }
            else
            {
                amount = 0;
                retValue.Message = "#1 金额参数错误";
                retValue.PutValue("BankStatus", "N");
                return retValue;
            }

            try
            {
                string payResult = retRequest.GetStringValue("TransStatus");

                retValue.PutValue("OrderNo", retRequest.GetStringValue("OrderNo")); //订单流水号传给Bill_NO
                retValue.PutValue("Amount", amount); //订单金额
                retValue.PutValue("BankNo", retRequest.GetStringValue("TransNo")); //在盛付通的交易号

                retValue.PutValue("BankName", "盛付通支付"); //银行支付方式

                retValue.PutValue("SettleType", (int)OrderSettleType.SFTBank);//06.07.12 add 添加支付类型参数


                string bankReturn = "银行代码：" + retRequest.GetStringValue("InstCode")
                                    + "； 盛付通交易号：" + retRequest.GetStringValue("TransNo")
                                    + "； 盛付通交易时间：" + retRequest.GetStringValue("TransTime")
                                    + "； 盛付通实际交易金额：" + retRequest.GetStringValue("TransAmount") + "元"
                                    + "； 支付平台返回的错误代码：" + retRequest.GetStringValue("ErrorCode")
                                      + "； 返回的支付状态：" + payResult;

                retValue.PutValue("BankReturn", bankReturn);////各个银行的返回值不同，有可能是几个返回值组成的结果


                string SFTPayConfig = System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.PayConfig"].ToString();// Utility.GetAppSettingValue("");
                string[] c = SFTPayConfig.Split('|');

                string merchantAcctId = c[0];
                string key = c[1];//密钥


                //获取加密签名串
                String signMsg = retRequest.GetStringValue("SignMsg").ToString().Trim();

                string signMessage = retRequest.GetStringValue("Name") + retRequest.GetStringValue("Version") + retRequest.GetStringValue("Charset") + retRequest.GetStringValue("TraceNo")
                     + retRequest.GetStringValue("MsgSender") + retRequest.GetStringValue("SendTime") + retRequest.GetStringValue("InstCode") + retRequest.GetStringValue("OrderNo")
                    + retRequest.GetStringValue("OrderAmount") + retRequest.GetStringValue("TransNo") + retRequest.GetStringValue("TransAmount")
                    + payResult + retRequest.GetStringValue("TransType") + retRequest.GetStringValue("TransTime") + retRequest.GetStringValue("MerchantNo")
                    + retRequest.GetStringValue("ErrorCode") + retRequest.GetStringValue("ErrorMsg") + retRequest.GetStringValue("Ext1")
                    + retRequest.GetStringValue("SignType") + key;

                LogHelper.SaveOperLog("PayResultCenter, 加密前, 接受的参数为:" + signMessage + "," + logname);

                MD5 myMD5 = new MD5CryptoServiceProvider();
                byte[] signed = myMD5.ComputeHash(Encoding.GetEncoding("UTF-8").GetBytes(signMessage));
                string signResult = byte2mac(signed);//Convert.ToBase64String(signed);
                string SignMsg2 = signResult.ToUpper();

                ///首先进行签名字符串验证
                if (SignMsg2 == signMsg)
                {
                    switch (payResult)
                    {
                        case "01":
                            /*  
                             ' 商户网站逻辑处理，比方更新订单支付状态为成功
                            ' 特别注意：只有signMsg.ToUpper() == merchantSignMsg.ToUpper()，且payResult=10，才表示支付成功！
                            */

                            //报告给快钱处理结果，并提供将要重定向的地址。
                            retValue.PutValue("BankStatus", "Y");
                            break;

                        default:
                            retValue.Message = "#1 支付失败，或不是从银行返回的信息";
                            retValue.PutValue("BankStatus", "N");
                            break;
                    }
                }
                else
                {
                    retValue.Message = "#2 返回的信息格式不正确，或不是从银行返回的信息";
                    retValue.PutValue("BankStatus", "E");
                }

                if (retValue.GetStringValue("BankStatus") == "Y")
                {
                    retValue.PutValue("BankReturnMsg", "");//银行返回结果
                    retValue.PutValue("UserHostAddress", request.UserHostAddress);   //浏览者ip

                    retValue.HasError = false;
                    retValue.Message = "盛付通支付_返回信息正确";
                }
                return retValue;
            }
            catch (Exception ex)
            {
                retValue.Message = "#3 返回的信息不正确，或不是从银行返回的信息，原因：" + ex.Message;
                return retValue;
            }


        }



        /// <summary>
        /// Md5加密方法
        /// </summary>
        /// <param name="signed"></param>
        /// <returns></returns>
        public static string byte2mac(byte[] signed)
        {
            StringBuilder EnText = new StringBuilder();
            foreach (byte Byte in signed)
            {
                EnText.AppendFormat("{0:x2}", Byte);
            }

            return EnText.ToString();
        }

        public override OpResult SetParamters(OpResult retBillValue, string bankCode)
        {
            return null;
        }


        /// <summary>
        /// 通知银行及重定位链接
        /// </summary>
        public override void Callback(OpResult orderResult)
        {
            HttpContext.Current.Response.Write("OK");
        }


        public string GetSFTTimeStamp()
        {
            string SFTPayConfig = System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.PayConfig"].ToString();
            string timeStampUlr = System.Configuration.ConfigurationSettings.AppSettings["App.SFT.TimeStampURL"].ToString();
            string[] c = SFTPayConfig.Split('|');
            //由盛付通提供,默认为:商户号(由盛付通提供的6位正整数),用于盛付通判别请求方的身份
            string MsgSender = c[0];
            string strPostData = string.Format("merchantNo={0}", MsgSender);
            string respHtml = string.Empty;
            //发送post请求
            HttpWebRequest request = WebRequest.Create(timeStampUlr) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            Encoding encoding = Encoding.Default;
            byte[] buffer = encoding.GetBytes(strPostData);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), encoding);
            string cookie = "";
            respHtml = reader.ReadToEnd();
            return "";
        }
    }
}
