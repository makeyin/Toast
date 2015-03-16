using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toast.Pay
{
    /// <summary>
    /// 订单结算类型 
    /// 0=直接扣款          1=邮政汇款或银行电汇支付    2=招商银行网上支付 
    /// 3=工商银行网上支付  4=建设银行网上支付          5=首信易 
    /// 8=易宝支付          10= 农业银行                12=支付互联星空支付         
    /// 13=货到付款         14=网汇通                   15=贝宝 
    /// 16=腾讯财富通       18=支付宝
    /// 170=快钱人民币支付  171=快钱充值卡支付          172=快钱游戏卡支付
    /// </summary>
    public enum OrderSettleType : int
    {
        //DeductAccount = 0,
        //MailOrBankPost = 1,
        //ZhaoHangOnline = 2,
        //GongHangOnline = 3,
        //JianHangOnline = 4,
        //CapitalBankOnline = 5,
        /// <summary>
        /// 易宝人民币支付
        /// </summary>
        YeePayRMBPay = 80,
        /// <summary>
        /// 易宝支付卡支付
        /// 810 = 代表神州行充值卡（SZX）
        /// 811 = 代表联通充值卡（UNICOM）
        /// 813 = 代表电信充值卡（TELECOM）
        /// </summary>
        YeePayRechargeCardPay = 81,

        /// <summary>
        /// 易付通语音支付
        /// 101 = 固话购买(1)
        /// 102 = 短信购买(2)
        /// 103 = 手机语音(3)
        /// 104 = 宽带购买(4)
        /// </summary>
        EFTVoicePay = 10,
        //VnetPayment = 12,
        //CWArrivePay = 13,
        //udpay = 14,
        //PayPal = 15,
        //TenPay = 16,
        /// <summary>
        /// 快钱人民币支付
        /// </summary>
        Bill99RMBPay = 170, //快钱人民币支付
        /// <summary>
        /// 快钱充值卡支付
        /// 1710 = 代表神州行充值卡(0)
        /// 1711 = 代表联通充值卡(1)
        /// 1713 = 代表电信充值卡(3)
        /// 1714 = 代表骏网一卡通(4)
        /// 1719 = 代表已开通任一卡类型(9)
        /// </summary>
        Bill99RechargeCardPay = 171, //快钱充值卡支付
        /// <summary>
        /// 快钱游戏卡支付
        /// </summary>
        Bill99GameCardPay = 172, //快钱游戏卡支付
        //AliPay = 18
        //业联一卡通充值
        YLCardPay = 190,

        //盛付通-盛大一卡通
        YLToSFT = 400,

        //银联无卡充值
        YLToYinLianNoCard = 420,

        //支付宝
        YLToZFB = 440,

        //新付支付
        XFZF = 450,

        //汇付宝一键支付（手机端）
        SJToHFBToYJZF = 1800,

        //汇付宝电话卡支付（手机端）
        SJToHFBToDHK = 1810,

        //网银充值盛大
        SFTBank = 1900,

        //电话卡充值--盛大
        SFTTelCard = 1901,

        //汇付宝网银(部分)
        YLToHFBBankOther = 1820

    }
}
