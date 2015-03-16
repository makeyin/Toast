using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toast.Pay
{
    public class AlipayNotify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            SortedDictionary<string, string> sPara = GetRequestGet();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);
                if (verifyResult)//验证成功
                {
                    //商户订单号
                    string out_trade_no = Request.QueryString["out_trade_no"];
                    //金额 
                    string strPrice = Request.QueryString["total_fee"];
                    //支付宝交易号
                    string trade_no = Request.QueryString["trade_no"];
                    //交易状态
                    string trade_status = Request.QueryString["trade_status"];

                    if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序


                        //查询订单状态 out_trade_no  Yes or No 更订单状态



                        //调用发货接口   Result  yes 更新收货状态 or No  发货失败
                       

                   
                    }
                    else
                    {
                        
                        Response.Write("支付失败"); //支付失败，提示信息 
                    }
                }
                else
                {
                    Response.Write("验证失败");
                }
            }
            else
            {
                Response.Write("无返回参数");
            }

        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }
    }

}
