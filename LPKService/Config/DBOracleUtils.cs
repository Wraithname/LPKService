using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace LPKService.Config
{
    class DBOracleUtils
    {
        public static OracleConnection GetDBConnection()
        {
            Console.WriteLine("Getting Connection ...");
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString);
            return conn;
        }
    }
}
