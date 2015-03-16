using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Toast.Factory
{
    /// <summary>
    /// 仓储基类
    /// 所有仓库都需要继承此类来获得一个特定上下文的Session
    /// </summary>
    public class BaseRepository
    {
        protected virtual ISession Session
        {
            get
            {
                return SessionBuilder.GetCurrentSession();
            }
        }
    }
}
