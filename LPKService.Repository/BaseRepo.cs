﻿using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Linq;

namespace LPKService.Repository
{
    public abstract class BaseRepo
    {
        /// <summary>
        /// Карта для функций Dapper.Query
        /// </summary>
        /// <param name="model"></param>
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
        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <returns>Подключение к Oracle</returns>
        public static OracleConnection GetDBConnection()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString);
            return conn;
        }
        /// <summary>
        /// Перобразование из bool в Char
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static char BoolToChar(bool flag)
        {
            char answ='N';
            if (flag)
                answ = 'Y';
            return answ;
        }
        /// <summary>
        /// Преобразование из Char в bool
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool CharToBool(char flag)
        {
            bool flager = true;
            if (flag == 'N')
                flager = false;
            return flager;
        }
    }
}
