using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
using Toast.Cfg;
using Toast.DomainModel;
using Toast.Map;


namespace Toast.Factory
{
    public class SessionBuilder
    {
        private static ISessionFactory sessionFactory = null;
        private static object _lock = new object();

        #region 初始化 生成SessionFactory，并配置上下文策略
        public static void Instance(string currentSessionContextClass)
        {
            ToastConfig config = new ToastConfig("Cfg");
            lock (_lock)
            {
                if (config.ConnectionType == "sql2008")
                {
                    sessionFactory = Fluently.Configure().CurrentSessionContext(currentSessionContextClass).Database(MsSqlConfiguration.MsSql2008.ConnectionString(config.ConnectionString)).Mappings(
       m => m.FluentMappings.AddFromAssembly(Assembly.Load("Toast.Map"))).BuildSessionFactory();
                }
                else if (config.ConnectionType == "mysql")
                {
                    MySQLConnectionStringBuilder mysql= new MySQLConnectionStringBuilder();

                    sessionFactory = Fluently.Configure().CurrentSessionContext("call").Database(MySQLConfiguration.Standard.ConnectionString(config.ConnectionString)).Mappings(
       m => m.FluentMappings.AddFromAssembly(Assembly.Load("Toast.Map"))).BuildSessionFactory();
                }
                else if (config.ConnectionString == "sqlite")
                {
                    sessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.ConnectionString(config.ConnectionString)).Mappings(
           m => m.FluentMappings.AddFromAssembly(Assembly.Load("Toast.Map"))).BuildSessionFactory();
                }
                else if (config.ConnectionType == "db2")
                {
                    sessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.ConnectionString(config.ConnectionString)).Mappings(
                             m => m.FluentMappings.AddFromAssembly(Assembly.Load("Toast.Map"))).BuildSessionFactory();
                }
            

                //new Configuration().Configure()
                //.SetProperty("current_session_context_class", currentSessionContextClass)
                //.BuildSessionFactory();
            }
        }
        #endregion


        public static void Instance(string currentSessionContextClass,string ConnStringName)
        {
            ToastConfig config = new ToastConfig(ConnStringName);
            lock (_lock)
            {
                if (config.ConnectionType == "sql2008")
                {
                    sessionFactory = Fluently.Configure().CurrentSessionContext(currentSessionContextClass).Database(MsSqlConfiguration.MsSql2008.ConnectionString(config.ConnectionString)).Mappings(
       m => m.FluentMappings.AddFromAssembly(Assembly.Load("Toast.Map"))).BuildSessionFactory();
                }
                else if (config.ConnectionType == "mysql")
                {
                    sessionFactory = Fluently.Configure().Database(MySQLConfiguration.Standard.ConnectionString(config.ConnectionString)).Mappings(
       m => m.FluentMappings.AddFromAssembly(Assembly.Load("Toast.Map"))).BuildSessionFactory();
                }
                else if (config.ConnectionType == "sqlite")
                {
                    sessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.ConnectionString(config.ConnectionString)).Mappings(
           m => m.FluentMappings.AddFromAssembly(Assembly.Load("Toast.Map"))).BuildSessionFactory();
                }
                else if (config.ConnectionType == "db2")
                {
                    sessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.ConnectionString(config.ConnectionString)).Mappings(
                             m => m.FluentMappings.AddFromAssembly(Assembly.Load("Toast.Map"))).BuildSessionFactory();
                }


            }
        }

        #region Session在当前上下文的操作
        private static void BindContext()
        {
            lock (_lock)
            {
                if (!CurrentSessionContext.HasBind(sessionFactory))
                {
                    CurrentSessionContext.Bind(sessionFactory.OpenSession());
                }
            }
     
        }

        private static void UnBindContext()
        {
            lock (_lock)
            {
                if (CurrentSessionContext.HasBind(sessionFactory))
                {
                    CurrentSessionContext.Unbind(sessionFactory);
                }
            }
        }

        public static void CloseCurrentSession()
        {
            UnBindContext();
        }

        public static ISession GetCurrentSession()
        {
            BindContext();
            return sessionFactory.GetCurrentSession();
        }
        #endregion

        #region 关闭SessionFactory（一般在应用程序结束时操作）
        public static void CloseSessionFactory()
        {
            if (!sessionFactory.IsClosed)
            {
                sessionFactory.Close();
            }
        }
        #endregion

        #region 打开一个新的Session
        public static ISession OpenSession()
        {
            lock (_lock)
            {
                return sessionFactory.OpenSession();
            }
        }
        #endregion

    }

}
