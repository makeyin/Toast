using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toast.Pay
{
    public class BackResult:System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            RechargeResult();
        }

        private void RechargeResult()
        {
            string order = Request.Params["no"];
            if (order == null || order == "")
            {
               //参数解析错误 AK.T.Web.Script.AlertAndRedirect("参数错误！", "http://wwwh52jiuqu.com");
            }
            else
            {

            }
            try
            {
                string key = System.Configuration.ConfigurationManager.AppSettings["verifykey"];
                string str1 = "订单编号解密";// DESHelper.DesDecrypt(order, key);
                //获得订单实体
                //MODEL.Info_Recharge model = JIUQU.BLL.BInfo_Recharge._.GetModelByOrderNo(str1);
                
              
            }
            catch (Exception ex)
            {
               // AK.T.Web.Script.AlertAndRedirect("参数错误！", "http://wwwh52jiuqu.com");
            }
        }
    }
}