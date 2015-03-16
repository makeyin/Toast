using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Toast.Pay
{
    public abstract class PayPlatForm
    {
        public abstract OpResult SetParamters(OpResult retBillValue, string bankCode);

        public abstract OpResult GetPayResult(HttpRequest request, OpResult retRequest);

        public abstract void Callback(OpResult orderResult);
    }
}
