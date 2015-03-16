using System;
using System.Collections.Generic;
using System.Text;


namespace Toast.Factory
{
    public sealed class Query
    {
        private List<Criterion> _criteria = new List<Criterion>();

        public List<Criterion> Criteria
        {
            get { return _criteria; }
        }

        public void AddCriteria(string propertyName,object value,CriteriaOperator criteriaOperator)
        {
            Criterion c=new Criterion();
            c.PropertyName=propertyName;
            c.Value=value;
            c.Operator=criteriaOperator;
            _criteria.Add(c);
        }

        public void AddCriteria(Criterion criterion)
        {
            _criteria.Add(criterion);
        }

        private List<Member> _members = new List<Member>();

        public List<Member> Members
        {
            get { return _members; }
        }

        public void AddMember(object menber)
        {
            Member m = new Member();
            m = menber as Member;
            _members.Add(m);
        }

        private IList<OrderClause> _orderClauses = new List<OrderClause>();

        public IList<OrderClause> OrderClauses
        {
            get { return _orderClauses; }
        }

        public void AddOrderClause(string property,bool ascending)
        {
            OrderClause o = new OrderClause();
            o.ascending = ascending;
            o.PropertyName = property;
            _orderClauses.Add(o);
        }

        public void AddOrderClause(OrderClause orderClause)
        {
            _orderClauses.Add(orderClause);
        }
    }
}
