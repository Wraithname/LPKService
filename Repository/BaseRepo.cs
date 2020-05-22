using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Linq;
using NLog;

namespace Repository
{
    public abstract class BaseRepo
    {
        private Logger logger = LogManager.GetLogger(nameof(Repository));
        protected void SetTypeMap(Type model)
        {
            SqlMapper.SetTypeMap(
                model,
                new CustomPropertyTypeMap(
                    model,
                    (type, columnName) => type.GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.ColumnName == columnName))
                )
            );
        }
        public static OracleConnection GetDBConnection()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString);
            return conn;
        }
        public static char BoolToChar(bool flag)
        {
            char answ='N';
            if (flag)
                answ = 'Y';
            return answ;
        }
        public static bool CharToBool(char flag)
        {
            bool flager = true;
            if (flag == 'N')
                flager = false;
            return flager;
        }
    }
}
