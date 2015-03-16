using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace Toast.Factory
{
    public class Translator
    {
        public Order ExecByOrderClause(OrderClause orderClauses)
        {
            Order order = null;
            if (orderClauses.ascending)
            {
                order = Order.Asc(orderClauses.PropertyName);
            }
            else
            {
                order = Order.Desc(orderClauses.PropertyName);
            }
            return order;
        }

        public AbstractCriterion ExecByCriteria(Criterion criteria)
        {
            AbstractCriterion ac = null;
            switch (criteria.Operator.ToString())
            {
                case "Eq":
                    ac=Expression.Eq(criteria.PropertyName, criteria.Value) as AbstractCriterion;break;
                case "NotEq":
                    ac=Expression.Not(Expression.Eq(criteria.PropertyName, criteria.Value)) as AbstractCriterion; break;
                case "Like":
                    ac=Expression.Like(criteria.PropertyName, criteria.Value) as AbstractCriterion; break;
                case "Gt":
                    ac=Expression.Gt(criteria.PropertyName, criteria.Value) as AbstractCriterion; break;
                case "Ge":
                    ac=Expression.Ge(criteria.PropertyName, criteria.Value) as AbstractCriterion; break;
                case "Lt":
                    ac=Expression.Lt(criteria.PropertyName, criteria.Value) as AbstractCriterion; break;
                case "Le":
                    ac=Expression.Le(criteria.PropertyName, criteria.Value) as AbstractCriterion; break;
                case "In":
                    ac=Expression.In(criteria.PropertyName, criteria.Value as object[]) as AbstractCriterion; break;
                case "NotIn":
                    ac=Expression.Not(Expression.In(criteria.PropertyName, criteria.Value as object[])) as AbstractCriterion; break;
                case "IsNull":
                    ac=Expression.IsNull(criteria.PropertyName) as AbstractCriterion; break;
                case "IsNotNull":
                    ac=Expression.IsNotNull(criteria.PropertyName) as AbstractCriterion; break;
                //case "IsEmpty":
                //    ac=Expression.IsEmpty(criteria.PropertyName) as AbstractCriterion; break;
                //case "IsNotEmpty":
                //    ac=Expression.IsNotEmpty(criteria.PropertyName) as AbstractCriterion; break;
                default: 
                    throw new Exception("翻译查询时条件操作符输入错误！");
            }
            return ac;
        }

        public ICriteria GetCriteria(ICriteria criteria,Query query)
        {
            foreach (Criterion criterion in query.Criteria)
            {
                criteria.Add(ExecByCriteria(criterion));
            }
            foreach (OrderClause orderClause in query.OrderClauses)
            {
                criteria.AddOrder(ExecByOrderClause(orderClause));
            }
            return criteria;
        }
    }
}
