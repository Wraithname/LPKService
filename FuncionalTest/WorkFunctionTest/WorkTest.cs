using Dapper;
using Dapper.Oracle;
using LPKService.Domain.Models.CCM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oracle.ManagedDataAccess.Client;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuncionalTest.WorkFunctionTest
{
    [TestClass]
    public class WorkTest
    {
        private const string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA= (SERVICE_NAME=XEPDB1)));User Id=OMK;Password=oracle;";
        private Country rescount;
        private Country count = new Country
        {
            countr="Россия",
            countrcode= "RUS",
            countrondoc= "RUS",
            moduserid=103,
            modDatetime=new System.DateTime(1970, 01, 01, 0, 00, 00)
        };
        [TestMethod]
        public void ConnectionTest()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                string res = conn.ExecuteScalar<string>("SELECT user_id FROM all_users where username='OMK'", null);
                Assert.AreEqual("103", res);
            }
        }
    }
}
