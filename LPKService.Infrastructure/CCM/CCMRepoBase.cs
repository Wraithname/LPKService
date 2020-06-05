using Dapper.Oracle;
using LPKService.Repository;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using LPKService.Domain.Models.CCM;

namespace LPKService.Infrastructure.CCM
{
    public class CCMRepoBase:BaseRepo
    {
        private Logger logger = LogManager.GetLogger(nameof(CCM));
        public override OracleConnection GetConnection()
        {
            return GetDBConnection(new List<Type>
            {
                typeof(AddresCat),
                typeof(Country),
                typeof(CustomerCat),
                typeof(CustomerCatCredit),
                typeof(L4L3Customer),
                typeof(ZipCatalogue)
            });
        }
        protected void LogSqlWithParams(string stm, OracleDynamicParameters odp = null)
        {
            Regex regex = new Regex(" {0,}\r{0,1}\n{0,1} {2,}");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(regex.Replace(stm, " "));
            if (odp != null)
            {
                // т.к. odp.GetParameter NullPointerException приходится использовать рефлексию
                Dictionary<string, OracleDynamicParameters.OracleParameterInfo> parameters =
                    (Dictionary<string, OracleDynamicParameters.OracleParameterInfo>)odp.GetType()
                    .GetProperty("Parameters", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(odp);
                foreach (KeyValuePair<string, OracleDynamicParameters.OracleParameterInfo> param in parameters)
                {
                    sb.AppendLine($"{param.Key}: {param.Value.Value}; ");
                }
            }
            logger.Trace(sb.ToString());
        }
    }
}
