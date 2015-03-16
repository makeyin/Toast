using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toast.Utility;


namespace Toast.Pay
{
    public class SFTNotify
    {

        /// <summary>
        ///  验证消息是否是盛付通发出的合法消息
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="notify_id">通知验证ID</param>
        /// <param name="sign">支付宝生成的签名结果</param>
        /// <returns>验证结果</returns>
        public bool Verify(SortedDictionary<string, string> inputPara, string sign)
        {
            try
            {
                Dictionary<string, string> sPara = new Dictionary<string, string>();
                //过滤空值、sign与sign_type参数
                sPara = SFTCore.FilterPara(inputPara);
                LogHelper.RechargeErrorLog("sftbankreturn", "盛付通网银", "sPara");
                //获取待签名字符串
                string preSignStr = SFTCore.CreateLinkString(sPara);
                LogHelper.RechargeErrorLog("sftbankreturn", "盛付通网银", preSignStr);
                //获得签名验证结果
                bool isSgin = false;
                isSgin = PayMD5.Verify(preSignStr, sign, SFTConfig.Key, SFTConfig.Input_charset);
                LogHelper.RechargeErrorLog("sftbankreturn", "盛付通网银", isSgin.ToString());
                return isSgin;
            }
            catch (Exception ex)
            {
                LogHelper.RechargeErrorLog("sftbankreturn", "盛付通网银", ex.Message);
                return false;
            }
        }



    }
}
