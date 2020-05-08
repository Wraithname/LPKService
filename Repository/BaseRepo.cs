﻿using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Repository
{
    public abstract class BaseRepo
    {
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
    }
}
