using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Toast.Utility;

namespace Toast.Pay
{
     public class PayPage:System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string orderid = Request.QueryString["oid"];
            string type = Request.QueryString["Type"];
            string name = Request.QueryString["name"];//充值账号
            string money = Request.QueryString["m"];//充值金额
            string gameid = Request.QueryString["g"];//游戏ID
            string serviceid = Request.QueryString["s"];//游戏服务器ID

            DataTable dt = null;//***获取订单详情  IUQU.BLL.BInfo_Recharge._.GetDataTable(string.Format(" a.ID={0} ", orderid), 1);
            if (null != dt && dt.Rows.Count > 0)
            {
                switch (type)
                {
                    case "alipay":
                        AlipayDeal(money, dt.Rows[0]["rOrders"].ToString());
                        break;
                    case "sftbank":
                        SFTBankCard(money, dt.Rows[0]["rOrders"].ToString());
                        break;
                    default:
                        break;
                }
            }
        }

        private void AlipayDeal(string money, string orderNo)
        {
            string pageName = HttpContext.Current.Request.Path;
            LogHelper.WriteLogToFile(pageName, "JIUQU.WebGame.jiuqu.re.PayPage.AlipayDeal", "开始接入支付宝接口-----" + "订单号：" + orderNo + ",金额:" + money);
            //支付类型
            string payment_type = "1";
            //服务器通知返回接口
            string return_url = ConfigurationManager.AppSettings["App.ZFB.return_url"];
            //支付结果返回接口
            string notify_url = ConfigurationManager.AppSettings["App.ZFB.notify_url"];
            //卖家支付宝账户
            string seller_email = ConfigurationManager.AppSettings["App.ZFB.Account"];
            //订单号
            string out_trade_no = orderNo;
            //订单名称
            string subject = "九趣游戏充值";
            //商品描述
            string description = "无";
            //充值金额
            string total_fee = money;
            //需以http://开头的完整路径     //防钓鱼时间戳
            string anti_phishing_key = Submit.Query_timestamp();
            //若要使用请调用类文件submit中的query_timestamp函数        //客户端的IP地址
            string exter_invoke_ip = HttpContext.Current.Request.UserHostAddress;

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner",Toast.Pay.Config.Partner);
            sParaTemp.Add("_input_charset", "UTF-8");//编码格式可以config配置
            sParaTemp.Add("service", "create_direct_pay_by_user");
            sParaTemp.Add("payment_type", payment_type);
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("return_url", return_url);
            sParaTemp.Add("seller_email", seller_email);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("body", description);
            sParaTemp.Add("anti_phishing_key", anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            Response.Write(sHtmlText);
        }

        /// <summary>
        /// 盛付通银行卡充值
        /// </summary>
        /// <param name="money">充值金额</param>
        /// <param name="orderNo">充值订单号</param>
        private void SFTBankCard(string money, string orderNo)
        {
            string pageName = HttpContext.Current.Request.Path;

           //获得订单实体
            //MODEL.Info_Recharge order = BLL.BInfo_Recharge._.GetModelByOrderNo(orderNo);


            LogHelper.RechargeOperationLog("JIUQU.WebGame.jiuqu.re.PayPage.SFTBankCard", "开始接入盛付通网银接口-----" + "订单号：" + orderNo + ",金额:" + money);
            string SFTPayConfig = System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.PayConfig"].ToString();
            string[] c = SFTPayConfig.Split('|');
            //由盛付通提供,默认为:商户号(由盛付通提供的6位正整数),用于盛付通判别请求方的身份
            string MsgSender = c[0];
            string key = c[1];//密钥
            //版本名称
            string Name = "B2CPayment";
            //版本号
            string Version = "V4.1.1.1.1";
            //字符集
            string Charset = "UTF-8";
            //发送支付请求时间
            string SendTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            //商户订单号
            string OrderNo = orderNo;
            //支付金额
            string OrderAmount = money;
            //商户订单交易时间
            string OrderTime = DateTime.Now.ToString("yyyyMMddHHmmss");//order.OperationTime.ToDateTime().ToString("yyyyMMddHHmmss");
            //支付类型编码
            string PayType = System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.payTypeSFT"];
            //支付渠道
            string PayChannel = "";
            string bankCode = "operC"; //WebUtil.Get("operC");//获得银行编码
            //银行编码
            string InstCode = bankCode;
            //支付成功回调地址
            string PageUrl = System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.PageUrl"];
            //服务器通知发货地址
            string NotifyUrl = System.Configuration.ConfigurationSettings.AppSettings["App.SFTBank.NotifyUrl"];
            //商品名称
            string ProductName = "九趣游戏网银充值";
            //支付人联系方式
            string BuyerContact = "";
            //卖家IP地址
            string BuyerIp = "115.238.236.133";
            //拓展，支付成功后按照鸳鸯返回客户
            string Ext1 = PayChannel + InstCode;
            // 签名
            string SignType = "MD5";
            //签名字符串
            string tempStr = Name + Version + Charset + MsgSender + SendTime
                + OrderNo + OrderAmount + OrderTime + PayType + PayChannel + InstCode + PageUrl
                + NotifyUrl + ProductName + BuyerContact + BuyerIp +
                  Ext1 + SignType + key;
            MD5 myMD5 = new MD5CryptoServiceProvider();
            byte[] signed = myMD5.ComputeHash(Encoding.GetEncoding("gbk").GetBytes(tempStr));
            string signResult = byte2mac(signed);//Convert.ToBase64String(signed);
            string SignMsg = signResult.ToUpper();
            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("Name", Name);
            sParaTemp.Add("Version", Version);
            sParaTemp.Add("Charset", Charset);
            sParaTemp.Add("MsgSender", MsgSender);
            sParaTemp.Add("SendTime", SendTime);
            sParaTemp.Add("OrderNo", OrderNo);
            sParaTemp.Add("OrderAmount", OrderAmount);
            sParaTemp.Add("OrderTime", OrderTime);
            sParaTemp.Add("PayType", PayType);
            sParaTemp.Add("PayChannel", PayChannel);
            sParaTemp.Add("InstCode", InstCode);
            sParaTemp.Add("PageUrl", PageUrl);
            sParaTemp.Add("NotifyUrl", NotifyUrl);
            sParaTemp.Add("ProductName", ProductName);
            sParaTemp.Add("BuyerContact", BuyerContact);
            sParaTemp.Add("BuyerIp", BuyerIp);
            sParaTemp.Add("Ext1", Ext1);
            sParaTemp.Add("SignType", SignType);
            sParaTemp.Add("SignMsg", SignMsg);

            //建立请求
            string sHtmlText = SftpaySubmit.BuildRequest(sParaTemp, "post", "提交");
            Response.Write(sHtmlText);
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

    }
 }