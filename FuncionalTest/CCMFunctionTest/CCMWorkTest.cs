using System;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oracle.ManagedDataAccess.Client;

namespace FuncionalTest.CCMFunctionTest
{
    [TestClass]
    public class CCMWorkTest
    {
        private const string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA= (SERVICE_NAME=XEPDB1)));User Id=OMK;Password=oracle;";
        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    // ORM_TASKS
                    connection.Execute("INSERT INTO ORM_TASKS(OID,PROD_SCHED_ID,FPS_EQUIPMENT_NAME,PRODUCTIONUNIT_NAME)VALUES(8888888,8888888,'СГП','ЛПК')", transaction);
                    connection.Execute("INSERT INTO ORM_TASKS(OID,PROD_SCHED_ID,FPS_EQUIPMENT_NAME,PRODUCTIONUNIT_NAME)VALUES(8888887,8888887,'АПР1','ЛПК')", transaction);
                    // ORM_LINES
                    connection.Execute("INSERT INTO ORM_LINES(OID,PROD_SCHED_LINE,EXCH_FPS_PROD_SCHED_ID,STATE_ID,PI_STATUS,LATEST_ROW_STATUS)VALUES(9988888,1,8888888,6,'0',1)", transaction);
                    connection.Execute("INSERT INTO ORM_LINES(OID,PROD_SCHED_LINE,EXCH_FPS_PROD_SCHED_ID,STATE_ID,PI_STATUS,LATEST_ROW_STATUS)VALUES(9988887,1,8888887,6,'0',1)", transaction);
                    // ORM_TASK_HEADER
                    connection.Execute("INSERT INTO ORM_TASK_HEADER(TASK_NUM_ID, TASK_NUMBER, TASK_MACHINE)VALUES(8888888, 8888888, 'СГП')", transaction);
                    connection.Execute("INSERT INTO ORM_TASK_HEADER(TASK_NUM_ID, TASK_NUMBER, TASK_MACHINE)VALUES(8888887, 8888887, 'АПР1')", transaction);
                    // ORM_TASK_LINE
                    connection.Execute("INSERT INTO ORM_TASK_LINE(LINE_NUM_ID, LINE_NUMBER, TASK_HEADER_ID, LINE_STATUS)VALUES(9988888, 1, 8888888, 1)", transaction);
                    connection.Execute("INSERT INTO ORM_TASK_LINE(LINE_NUM_ID, LINE_NUMBER, TASK_HEADER_ID, LINE_STATUS)VALUES(9988887, 1, 8888887, 1)", transaction);
                    transaction.Commit();
                }
            }
        }
        [TestMethod]
        public void TestMethod1()
        {
        }
        [ClassCleanup]
        public static void CleanupClass()
        {

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    // ORM_TASK_LINE
                    connection.Execute("DELETE FROM ORM_TASK_LINE WHERE LINE_NUM_ID IN (9988888, 9988887)");
                    // ORM_TASK_HEADER
                    connection.Execute("DELETE FROM ORM_TASK_HEADER WHERE TASK_NUM_ID IN (8888888, 8888887)");
                    // ORM_LINES
                    connection.Execute("DELETE FROM ORM_LINES WHERE OID IN (9988888, 9988887)");
                    // ORM_TASKS
                    connection.Execute("DELETE FROM ORM_TASKS WHERE OID IN (8888888, 8888887)");
                    transaction.Commit();
                }
            }
        }
    }
}
