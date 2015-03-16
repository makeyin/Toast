using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using Toast.Utility;

namespace Toast.Pay
{
    public class PayResultCenter
    {
        private string logname = "PayCenterLog.txt";



        #region 处理支付结果

        public void DealPayResult()
        {
            //得到支付平台返回信息
            string payStatus;
            string orderNo;
            float amount;
            string bankNo;
            string bankReturn;
            //根据httpRequest初始化OpResult实体
            OpResult payResult = GetPayResult(out payStatus, out orderNo, out amount, out bankNo, out bankReturn);

            //根据支付返回结果，处理订单
            OpResult rst = null;
            StringBuilder sb = new StringBuilder("支付返回结果为：payStatus=" + payStatus + ", orderNo=" + orderNo + ", amount=" + amount.ToString("f2") + ", bankNo=" + bankNo + ", bankReturn=" + bankReturn);
            if (payResult != null)
            {
                sb.Append(payResult.ErrorCode);
            }
            try
            {
                
            }
            catch (Exception ex)
            {
                sb.Append("\r\n出现异常：" + ex.Message);
                //LogHelper.WriteLogToFile("PayResultCenter", "DealPayResult", ex.Message, logname);
            }

            if (rst != null && (!rst.HasError))
            {
                sb.Append("\r\n处理成功调用成功后其他程序，如调用杭州接口充值等");
                //充值成功后，其他处理
                //RechargeSuccessfulResult.SuccessfulRecharge(rst, amount, orderNo);

                new SFTBank().Callback(payResult);
            }
            else
            {
                sb.Append("\r\n处理失败");
                if (rst != null)
                {
                    sb.Append("ErrorCode=" + rst.GetStringValue("ErrorCode", "无") + ",ErrorMsg=" + rst.GetStringValue("ErrorMsg", "无"));

                    if (rst.GetIntValue("ErrorCode", -9999) == -4)
                    {
                        //已成功
                        new SFTBank().Callback(payResult);
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("Error01");
                    sb.Append("rst为null");
                }
            }

           //**日志 LogHelper.WriteLogToFile("PayResultCenter", "DealPayResult", sb.ToString(), logname);
        }
        #endregion

        #region 获取支付平台返回信息

        private OpResult GetPayResult(out string bankStatus, out string orderNo, out float amount, out string bankNo, out string bankReturn)
        {
            LogHelper.WriteLogToFile("bankresult.aspx", "Page_Load", HttpContext.Current.Request.Url.ToString());
            HttpRequest request = HttpContext.Current.Request;
            string strKey, strValue;
            OpResult retResult = new OpResult();//方法返回
            retResult.HasError = true;
            OpResult retRequest = new OpResult();//接收到网页参数 

            string valu = "";
            for (int i = 0; i < request.Params.Count; i++)
            {
                strKey = request.Params.Keys[i];
                strValue = request.Params.GetValues(strKey)[0];
                retRequest.PutValue(strKey, strValue);             //把相应的信息放到strKey里去
                valu += strKey + strValue;
            }
            OpResult retParameters = GetPayResult(request, retRequest);
            bankStatus = retParameters.GetStringValue("BankStatus");
            orderNo = retParameters.GetStringValue("OrderNo");
            float.TryParse(retParameters.GetStringValue("Amount"), out amount);
            bankNo = retParameters.GetStringValue("BankNo");
            bankReturn = retParameters.GetStringValue("BankReturn");

            LogHelper.WriteLogToFile("PayResultCenter", "DealPayResult", retParameters.Message, logname);

            return retParameters;
        }
        #endregion


        #region GetPayResult

        /// <summary>
        /// Bill_ID,Bill_NO，Bill_Time，Bank_Status，Bank_ReturnMsg，Bank_Name，Total_Amt,Real_Amt,Bill_Type,UserIP
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Bill_ID,Bill_NO，Bill_Time，Bank_Status，Bank_ReturnMsg，Bank_Name，Total_Amt,Real_Amt,Bill_Type,UserIP</returns>
        public OpResult GetPayResult(HttpRequest request, OpResult retRequest)
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


                string SFTPayConfig =System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.PayConfig"].ToString();
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

                //LogHelper.WriteLogToFile("PayResultCenter", "加密前", "接受的参数为:" + signMessage, logname);

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
        #endregion

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
    }
}
