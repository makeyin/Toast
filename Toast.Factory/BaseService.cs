using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace Toast.Factory
{
    /// <summary>
    /// NHibernate专用的服务基类
    /// </summary>
    public class BaseService
    {
        protected ITransaction tx;

        protected virtual ISession Session
        {
            get { return SessionBuilder.GetCurrentSession(); }
        }

        protected virtual void BeginTransaction()
        {
            tx = Session.BeginTransaction();
        }

        protected virtual void Rollback()
        {
            tx.Rollback();
        }

        protected virtual void Commit()
        {
            tx.Commit();
        }
    }
}
