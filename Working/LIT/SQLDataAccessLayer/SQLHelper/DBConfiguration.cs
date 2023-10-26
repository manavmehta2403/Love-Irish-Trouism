using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.SQLHelper
{
    public sealed class DBConfiguration
    {
        private string _strConnectionString { get; set; }
        public string ConnectionString { get { return _strConnectionString; } }

        private static DBConfiguration _instance = null;
        private static object _lock = new object();
        private DBConfiguration()
        {
            _strConnectionString = (ConfigurationManager.AppSettings["LITDBConnection"] != null) ? ConfigurationManager.AppSettings["LITDBConnection"].ToString() : string.Empty;
        }

        public static DBConfiguration instance
        {
            get
            {
                lock (_lock)
                {
                    _instance = _instance ?? new DBConfiguration();
                    return _instance;
                }
            }
        }

    }
}
