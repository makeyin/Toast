using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace Toast.Factory
{
    public class Repository<T> : BaseRepository where T : class,new()
    {
        //仓储实现
        public virtual IList<T> GetAll()
        {
            return Session.CreateCriteria<T>()
                .List<T>();
        }
        public virtual IList<T> GetAll(int pageIndex, int pageSize)
        {
            return Session.CreateCriteria<T>()
                .SetFirstResult(pageIndex)
                .SetMaxResults(pageSize)
                .List<T>();
        }
        public virtual IList<T> GetByCriteria(Query query)
        {
            ICriteria criteria = Session.CreateCriteria<T>();
            Translator translator = new Translator();
            translator.GetCriteria(criteria, query);
            return criteria.List<T>();
        }
        public virtual IList<T> GetByCriteria(string hql)
        {
            return Session.CreateQuery(hql).List<T>();
        }
        public virtual IList<T> GetByCriteria(string hql,IDictionary<string,object> parameters)
        {
            IQuery query = Session.CreateQuery(hql);
            foreach (string key in parameters.Keys)
            {
                query.SetParameter(key, parameters[key]);
            }
            return query.List<T>();
        }
        public virtual IList<T> GetByCriteria(Query query, int pageIndex, int pageSize)
        {
            ICriteria criteria = Session.CreateCriteria<T>();
            Translator translator = new Translator();
            translator.GetCriteria(criteria, query);
            return criteria
                .SetFirstResult(pageIndex)
                .SetMaxResults(pageSize)
                .List<T>();
        }
        public virtual IList<T> GetByCriteria(string hql, int pageIndex, int pageSize)
        {
            return Session.CreateQuery(hql)
                .SetFirstResult(pageIndex)
                .SetMaxResults(pageSize)
                .List<T>();
        }
        public virtual IList<T> GetByCriteria(string hql, IDictionary<string, object> parameters, int pageIndex, int pageSize)
        {
            IQuery query = Session.CreateQuery(hql);
            foreach (string key in parameters.Keys)
            {
                query.SetParameter(key, parameters[key]);
            }
            return query
                .SetFirstResult(pageIndex)
                .SetMaxResults(pageSize)
                .List<T>();
        }
        public virtual T GetByID(object id)
        {
            return Session.Get<T>(id);
        }
        public virtual IList<T> GetByPropertyValue(Criterion criterion)
        {
            ICriteria criteria = Session.CreateCriteria<T>();
            Translator translator=new Translator();
            IList<T> list = criteria.Add(translator.ExecByCriteria(criterion)).List<T>();
            return list;
        }
        public virtual int GetCount()
        {
            ICriteria countCriteria = Session.CreateCriteria<T>();
            var count = (int)countCriteria.SetProjection(Projections.ProjectionList().Add(Projections.RowCount()))
                .UniqueResult();
            return count;
        }
        public virtual int GetCount(Query query)
        {
            ICriteria countCriteria = Session.CreateCriteria<T>();
            Translator translator = new Translator();
            foreach (Criterion criterion in query.Criteria)
            {
                countCriteria.Add(translator.ExecByCriteria(criterion));
            }
            var count = (int)countCriteria.SetProjection(Projections.ProjectionList().Add(Projections.RowCount()))
                .UniqueResult();
            return count;
        }
        public virtual int GetCount(string hql)
        {
            int fromindex = hql.ToLower().IndexOf("from");
            if (fromindex < 0)
            {
                throw new Exception("HQL语句有误，没有找到from");
            }
            string newhql = string.Concat("select count(*) ", hql.Substring(hql.IndexOf("from")));
            return Convert.ToInt32(Session.CreateQuery(newhql).UniqueResult());
        }
        public virtual int GetCount(string hql,IDictionary<string, object> parameters)
        {
            int fromindex = hql.ToLower().IndexOf("from");
            if (fromindex < 0)
            {
                throw new Exception("HQL语句有误，没有找到from");
            }
            string newhql = string.Concat("select count(*) ", hql.Substring(hql.IndexOf("from")));
            IQuery query = Session.CreateQuery(newhql);
            foreach (string key in parameters.Keys)
            {
                query.SetParameter(key, parameters[key]);
            }
            return Convert.ToInt32(query.UniqueResult());
        }
        public string TranslateQuery(Query query)
        {
            return "";
        }
        public virtual void Create(T item)
        {
            try
            {
                Session.Save(item);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public virtual void Update(T item)
        {
            try
            {
                Session.SaveOrUpdate(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual void Delete(T item)
        {
            try
            {
                Session.Delete(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
