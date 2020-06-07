using Dapper.Oracle;
using Repository;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using LPKService.Domain.Models.Material;

namespace LPKService.Infrastructure.Material
{
    public class MatRepoBase:BaseRepo
    {
        private Logger logger = LogManager.GetLogger(nameof(BaseRepo));
        /// <summary>
        /// Подключение к БД
        /// </summary>
        /// <returns>Подключение к БД для получение данных через модель</returns>
        public override OracleConnection GetConnection()
        {
            return GetDBConnection(new List<Type>
            {
                typeof(L4L3RmAndMatCat)
            });
        }
        /// <summary>
        /// Логирование sql запросов с параметрами
        /// </summary>
        /// <param name="stm">Строка запроса</param>
        /// <param name="odp">Параметры</param>
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
