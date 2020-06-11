using System;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oracle.ManagedDataAccess.Client;

namespace UnitTestProject2
{
    [TestClass]
    public class WorkTest
    {
        private const string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA= (SERVICE_NAME=XEPDB1)));User Id=OMK;Password=oracle;";
        [TestMethod]
        public void TestConnection()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                string res = conn.ExecuteScalar<string>("SELECT user_id FROM all_users where username='OMK'", null);
                Assert.AreEqual("103", res);
            }
        }
    }
}
