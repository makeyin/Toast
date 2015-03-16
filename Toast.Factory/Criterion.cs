using System;
using System.Collections.Generic;
using System.Text;

namespace Toast.Factory
{
    public class Criterion
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public CriteriaOperator Operator { get; set; }
    }
    public enum CriteriaOperator
    {
        Eq,
        NotEq,
        Like,
        Gt,
        Ge,
        Lt,
        Le,
        In,
        NotIn,
        IsNull,
        IsNotNull,
        IsEmpty,
        IsNotEmpty
    }
}
