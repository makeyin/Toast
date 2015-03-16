using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

using Toast.Pay;
namespace JIUQU.Pay
{
    public class PayFactory
    {
        public PayFactory()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">订单信息</param>
        /// <param name="orderid">订单ID</param>
        /// <param name="money">充值金额</param>
        /// <param name="paytype">充值方式</param>
        /// <param name="username">充值账户</param>
        /// <param name="operCode">银行编码</param>
        /// <param name="operValue">充值渠道等</param>
        /// <returns></returns>
        public OpResult SetParamters(object model, string orderid, double money, string paytype, string username, string operCode, string operValue)
        {
         
            DataTable dt = null;  //根据订单编号获取订单   JIUQU.BLL.BInfo_Recharge._.GetDataTable(" rOrders='" + orderid + "' ", 0);
            if (dt != null && dt.Rows.Count > 0)
            {
                OrderSettleType ost = (OrderSettleType) int.Parse(paytype);
                switch (ost)
                {
                    case OrderSettleType.YeePayRMBPay:
                        break;
                    case OrderSettleType.YeePayRechargeCardPay:
                        break;
                    case OrderSettleType.EFTVoicePay:
                        break;
                    case OrderSettleType.Bill99RMBPay:
                        break;
                    case OrderSettleType.Bill99RechargeCardPay:
                        break;
                    case OrderSettleType.Bill99GameCardPay:
                        break;
                    case OrderSettleType.YLCardPay:
                        break;
                    case OrderSettleType.YLToSFT:
                        break;
                    case OrderSettleType.YLToYinLianNoCard:
                        break;
                    case OrderSettleType.YLToZFB:
                        break;
                    case OrderSettleType.XFZF:
                        break;
                    case OrderSettleType.SJToHFBToYJZF:
                        break;
                    case OrderSettleType.SJToHFBToDHK:
                        break;
                    case OrderSettleType.SFTBank:
                        return new SFTBank().SetParamtersByInterface(GetBillInfo(0), "", operValue, operCode, "");
                    case OrderSettleType.SFTTelCard:
                        break;
                    case OrderSettleType.YLToHFBBankOther:
                        break;
                    default:
                        break;
                }
                switch (paytype)
                {
                    case "alipay":
                        Alipay(money.ToString(), dt.Rows[0]["rOrders"].ToString());
                        break;
                    case "sftbank":
                        
                    default:
                        break;
                }
            }

            /*订单实体*/
            /*
             var order = new object();
            order.rOrders = OrderNo();
            order.Amount = money;
            order.Status = 0;//0：处理中，1充值成功
            order.OperationIP = Utility.GetUserIP();
            order.OperationTime = DateTime.Now;
            order.TargetUser = username;
            order.Target = Convert.ToInt32(operValue);
            order.GameID = model.GameID;
            order.GameServer = model.GameServer;
            order.Remark = "游戏充值";
            order.Methods = "银行卡充值";
             **/
            int order_id=0;
            
           //创建订单，返回订单号    JIUQU.BLL.BInfo_Recharge._.PayOrder(order, out order_id);
            if (order_id > 0)
            {
                switch (paytype)
                {
                    case "alipay":
                        return null;
                    case "sftbank":
                        return new SFTBank().SetParamtersByInterface(GetBillInfo(order_id), "", operValue, operCode, "");
                }
            }
            else
            {
                
            }
            return null;
        }


        public void Alipay(string money, string tradeNo)
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["Yeel.PayLogFile.Path"].ToString());//Utility.GetXmlMapPath(""));
            string pageName = HttpContext.Current.Request.Path;


            //****订单日志LogHelper.SaveOperLog("开始接入支付宝接口-----" + "订单号：" + tradeNo + ",金额:" + money);

            //支付类型
            string payment_type = "1";
            //服务器通知返回接口  
            string return_url = System.Configuration.ConfigurationSettings.AppSettings["App.ZFB.return_url"].ToString();
            //服务器通知返回接口  
            string notify_url = System.Configuration.ConfigurationSettings.AppSettings["App.ZFB.notify_url"].ToString(); 
            //卖家支付宝帐户
            string seller_email = System.Configuration.ConfigurationSettings.AppSettings["App.ZFB.Account"].ToString();
            //商户订单号
            string out_trade_no = tradeNo;
            //订单名称
            string subject = "九趣页游";
            //商品描述  
            string body = "无";
            //付款金额
            string total_fee = money;
            //需以http://开头的完整路径     //防钓鱼时间戳
            string anti_phishing_key = Submit.Query_timestamp();
            //若要使用请调用类文件submit中的query_timestamp函数        //客户端的IP地址
            string exter_invoke_ip = HttpContext.Current.Request.UserHostAddress;

            ////把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("service", "create_direct_pay_by_user");
            sParaTemp.Add("payment_type", payment_type);
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("return_url", return_url);
            sParaTemp.Add("seller_email", seller_email);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("body", body);
            sParaTemp.Add("anti_phishing_key", anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            
        }


        private OpResult GetBillInfo(int billid)
        {
            OpResult retValue = new OpResult();
            retValue.HasError = true;
            string where = " ID=" + billid + " ";
            DataTable dt = null; //** 获得订单 JIUQU.BLL.BInfo_Recharge._.GetDataTable(where, 0);
            if (dt.Rows.Count > 0)
            {
                retValue.HasError = false;
                retValue.PutValue("OrderID", dt.Rows[0]["ID"]);
                retValue.PutValue("OrderNo", dt.Rows[0]["rOrders"]);
                retValue.PutValue("UserID", dt.Rows[0]["TargetUser"]);
                retValue.PutValue("Amount", dt.Rows[0]["Amount"]);
                retValue.PutValue("PayType", "银行卡");
                retValue.PutValue("ChargeIP", dt.Rows[0]["OperationIP"]);
                retValue.PutValue("UserCode", dt.Rows[0]["TargetUser"]);
                retValue.PutValue("Contact", dt.Rows[0]["Mobile"]); //充值时填写的手机号
                retValue.PutValue("ChargeDate", dt.Rows[0]["OperationTime"]);
                retValue.PutValue("Status", dt.Rows[0]["Status"]);
                retValue.PutValue("UserPwd", "");
                retValue.PutValue("ContactEmail", "");
            }
            return retValue;
        }

        //订单号生成规则
        public string OrderNo()
        {
            string code = "ZJ";
            code += DateTime.Now.Year.ToString();
            code += DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            code += DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();
            code += DateTime.Now.Hour.ToString().Length == 1 ? "0" + DateTime.Now.Hour.ToString() : DateTime.Now.Hour.ToString();
            code += DateTime.Now.Minute.ToString().Length == 1 ? "0" + DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString();
            code += DateTime.Now.Second.ToString().Length == 1 ? "0" + DateTime.Now.Second.ToString() : DateTime.Now.Second.ToString();
            if (DateTime.Now.Millisecond.ToString().Length == 1)
            {
                code += "00" + DateTime.Now.Millisecond.ToString();
            }
            else if (DateTime.Now.Millisecond.ToString().Length == 2)
            {
                code += "0" + DateTime.Now.Millisecond.ToString();
            }
            else
            {
                code += DateTime.Now.Millisecond.ToString();
            }
            return code;
        }

    }
}
