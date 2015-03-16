using System;
using System.Collections.Generic;
using System.Text;

namespace Toast.Factory
{
    public class OrderClause
    {
        public bool ascending { get; set; }
        public string PropertyName { get; set; }
        public OrderClause Asc(string property)
        {
            this.ascending = true;
            this.PropertyName = property;
            return this;
        }

        public OrderClause Desc(string property)
        {
            this.ascending = false;
            this.PropertyName = property;
            return this;
        } 
    }
}
