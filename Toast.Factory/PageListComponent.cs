using System;
using System.Collections.Generic;
using System.Text;

namespace Toast.Factory
{
    public class PageListComponent
    {
        public static IList<T> GetPageListFor<T>(Query query, int pageindex, int pagesize, out int total) where T : class,new()
        {
            Repository<T> rep = new Repository<T>();
            IList<T> datalist = rep.GetByCriteria(query, pageindex, pagesize);//获取列表        条件，起始记录，每页记录数
            total = rep.GetCount(query);//获取总记录数
            return datalist;
        }

        public static IList<T> GetPageListFor<T>(string hql, int pageindex, int pagesize, out int total) where T : class,new()
        {
            Repository<T> rep = new Repository<T>();
            IList<T> datalist = rep.GetByCriteria(hql, pageindex, pagesize);//获取列表        HQL，起始记录，每页记录数
            total = rep.GetCount(hql);//获取总记录数
            return datalist;
        }

        public static IList<T> GetPageListFor<T>(string hql, IDictionary<string, object> parameters, int pageindex, int pagesize, out int total) where T : class,new()
        {
            Repository<T> rep = new Repository<T>();
            IList<T> datalist = rep.GetByCriteria(hql, parameters, pageindex, pagesize);//获取列表        HQL，参数，起始记录，每页记录数
            total = rep.GetCount(hql, parameters);//获取总记录数
            return datalist;
        }
    }
}
