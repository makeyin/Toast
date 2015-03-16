using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toast.Utility;

namespace Toast.Pay
{
    public class SFTBankNotify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestPost();
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                SFTNotify notify = new SFTNotify();
                bool verifyResult = notify.Verify(sPara, Request.Form["SignMsg"]);
                verifyResult = true;
                if (verifyResult)
                {
                    //商户订单号
                    string OrderNo = sPara["OrderNo"];
                    //订单金额
                    string OrderAmount = sPara["OrderAmount"];
                    //盛付通的交易号
                    string TraceNo = sPara["TraceNo"];
                    //交易信息
                    string BankReturn = "银行代码：" + sPara["InstCode"]
                                    + "； 盛付通交易号：" + sPara["TransNo"]
                                    + "； 盛付通交易时间：" + sPara["TransTime"]
                                    + "； 盛付通实际交易金额：" + sPara["TransAmount"] + "元"
                                    + "； 支付平台返回的错误代码：" + sPara["ErrorCode"]
                                      + "； 返回的支付状态：" + sPara["TransStatus"]; ;

                    if (sPara["TransStatus"] == "01")
                    {
                        DataTable dt = null; //**获得订单详情   JIUQU.BLL.BInfo_Recharge._.GetDataTable(string.Format(" rOrders='{0}' ", OrderNo), 0);

                        if (dt.Rows.Count > 0)
                        {
                            int gid = Convert.ToInt32(dt.Rows[0]["GameID"].ToString());//游戏ID
                            //MODEL.Game_Main game = JIUQU.BLL.BGame_Main._.GetGamInfo(gid);
                            int sid = Convert.ToInt32(dt.Rows[0]["ServiceNum"].ToString());//服务器ID
                            string uname = dt.Rows[0]["TargetUser"].ToString();//充值目标账号
                            string gcode = "";//game.GameCode;
                            //GamePayFactory factory = new GamePayFactory();
                            //factory.Factory(gid, sid, uname, OrderNo, gcode);


                            //查询订单状态准通知发货
                            string id = dt.Rows[0]["ID"].ToString();
                            string state = dt.Rows[0]["Status"].ToString();

                            if (state == "0")
                            {

                                //   发货  OK 更改发货状态为已发货   OR   NO 发货失败

                                Response.Redirect("BackResult.aspx?no=" + OrderNo);
                            }
                            else
                            {
                                string no = "加密后的订单";//DESHelper.DesEncrypt(OrderNo, ConfigurationManager.AppSettings["verifykey"]);
                                Response.Redirect("BackResult.aspx?no=" + no);
                            }
                        }
                    }
                    else
                    {
                        //更改更改成无效订单  更改银行卡充值订单状态出错
                        bool chargeResult = false;//U.BLL.BInfo_Recharge._.AlertOrderState(-1, OrderNo);
                        if (!chargeResult) LogHelper.SaveErrorLog("更改银行卡充值订单状态出错", "AlertOrderState");
                    }
                }
                else
                {
                    string no = "加密后订单编号";// DESHelper.DesEncrypt(sPara["OrderNo"], 、、ConfigurationManager.AppSettings["verifykey"]);
                    Response.Redirect("BackResult.aspx?no=" + no);
                }
            }
        }



        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }
}