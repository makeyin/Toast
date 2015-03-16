using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toast.Utility;

namespace Toast.Pay
{
    public class SFTCore
    {
        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && temp.Key.ToLower() != "sign_type" && temp.Value != "" && temp.Value != null)
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }
        /// <summary>
        /// 获取数组中需要签名的value值拼接成字符串
        /// </summary>
        /// <param name="dicArray"></param>
        /// <returns></returns>
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            prestr.Append(dicArray["Name"]);
            LogHelper.TestLog("name");
            prestr.Append(dicArray["Version"]);
            LogHelper.TestLog("Version");
            prestr.Append(dicArray["Charset"]);
            LogHelper.TestLog("Charset");
            prestr.Append(dicArray["TraceNo"]);
            LogHelper.TestLog("TraceNo");
            prestr.Append(dicArray["MsgSender"]);
            LogHelper.TestLog("MsgSender");
            prestr.Append(dicArray["SendTime"]);
            LogHelper.TestLog("SendTime");
            prestr.Append(dicArray["InstCode"]);
            LogHelper.TestLog("InstCode");
            prestr.Append(dicArray["OrderNo"]);
            LogHelper.TestLog("OrderNo");
            prestr.Append(dicArray["OrderAmount"]);
            LogHelper.TestLog("OrderAmount");
            prestr.Append(dicArray["TransNo"]);
            LogHelper.TestLog("TransNo");
            prestr.Append(dicArray["TransAmount"]);
            LogHelper.TestLog("TransAmount");
            prestr.Append(dicArray["TransStatus"]);
            LogHelper.TestLog("TransStatus");
            prestr.Append(dicArray["TransType"]);
            LogHelper.TestLog("TransType");
            prestr.Append(dicArray["TransTime"]);
            LogHelper.TestLog("TransTime");
            prestr.Append(dicArray["MerchantNo"]);
            LogHelper.TestLog("MerchantNo");
            prestr.Append(dicArray["ErrorCode"]);
            LogHelper.TestLog("ErrorCode");
            prestr.Append(dicArray["ErrorMsg"]);
            LogHelper.TestLog("ErrorMsg");
            prestr.Append(dicArray["Ext1"]);
            LogHelper.TestLog("Ext1");
            prestr.Append(dicArray["SignType"]);
            LogHelper.TestLog("SignType");
            return prestr.ToString();
        }
    }
}
