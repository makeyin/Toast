using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Toast.Pay
{
    public class SftpaySubmit
    {
        private static string GATEWAY_NEW = "https://mas.sdo.com/web-acquire-channel/cashier.htm";


        /// <summary>
        /// 建立请求，以表单HTML形式构造（默认）
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue)
        {
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id=\"form1\" action=\"" + GATEWAY_NEW + "\" method=\"" + strMethod.ToLower().Trim() + "\">");

            foreach (KeyValuePair<string, string> temp in sParaTemp)
            {
                sbHtml.Append("<input type=\"hidden\" id=\"" + temp.Key + "\" name=\"" + temp.Key + "\" value=\"" + temp.Value + "\"/>");
            }

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type=\"submit\" value=\"" + strButtonValue + "\" style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['form1'].submit();</script>");

            return sbHtml.ToString();
        }
    }
}