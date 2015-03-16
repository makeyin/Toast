using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Toast.Utility;

namespace Toast.Cfg
{
    public class ToastConfig
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// 


                public string _connectionString;
        

                    public string ConnectionString
                    {
                      get  {
                          return _connectionString;
                                }
                      set { _connectionString = value; }
                    }


                    private string _connectionType;
                    public string ConnectionType
                    {
                      get { return _connectionType; }
                      set { _connectionType = value; }
                    }

        public  ToastConfig (string DbConfigNodeName)
        {
            this._connectionString=xmlCfg.GetXmlNodeValue("Root/" + DbConfigNodeName + "/ConnctionString");
            this._connectionType = xmlCfg.GetXmlNodeValue("Root/" + DbConfigNodeName + "/Type"); 
        }


        public static XMLHelper xmlCfg
        {
            get {
                return new XMLHelper(ConfigurationSettings.AppSettings["SysCfgPath"].ToString(), XMLHelper.enumXmlPathType.VirtualPath);
            }
        }



        public static string DomainModelCfg()
        {
            Assembly ass = null;
            try
            {
                string strPath = System.Configuration.ConfigurationSettings.AppSettings["DomainModelCfg"].ToString().Trim();
                //以下这行代码：把这个文件加载到内存当中来
                ass = Assembly.LoadFile(strPath);
            }
            catch (Exception ex)
            {
                return "";

            }
            return ass.FullName.ToString(); ;
        }

    }
    
}
