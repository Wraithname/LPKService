using System;
using Dapper;
using LPKService.Infrastructure.Builders;
using LPKService.Infrastructure.CCM;
using LPKService.Infrastructure.Material;
using LPKService.Infrastructure.Repository;
using LPKService.Infrastructure.Shipping;
using LPKService.Infrastructure.SOM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oracle.ManagedDataAccess.Client;

namespace UnitTestProject2.CCMTest
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
                    // L4_L3_EVENT
                    connection.Execute("INSERT INTO L4_L3_EVENT(MSG_COUNTER,MSG_ID,MSG_DATETIME,OP_CODE,MSG_STATUS,BLOCK_FOR_PROCESS)VALUES(1,4303,SYSDATE,1,1,1)", transaction);
                    // L4_L3_CUSTOMER
                    connection.Execute("INSERT INTO L4_L3_CUSTOMER(MSG_COUNTER,CUSTOMER_ID,CUSTOMER_NAME,INTERNAL_CUSTOMER_FLAG,CUSTOMER_CLASSIFICATION_TYPE,ZIP_CODE,CITY,COUNTRY,VALIDITY_FLAG)VALUES(1,1,'ВлГУ','N','type','606107','Муром','Россия','N')", transaction);
                    // AUX_CONSTANT
                    connection.Execute("INSERT INTO AUX_CONSTANT(CONSTANT_ID,DESCRIPTION,INTEGER_VALUE,CHAR_VALUE,FLOAT_VALUE,INFO,MOD_USER_ID,VALUE_MODIFIABLE_FLAG)VALUES('ACCEPT_ORDER_IN_SRV','Принятие заказа',0,'Y',0,'lelelo',103,'Y')", transaction);
                    transaction.Commit();
                }
            }
        }
        [TestMethod]
        public void CCMTest()
        {
            INewMessageBuilder mng = new NewMessageBuilder(new L4L3InterfaceServiceGlobalCheck(), new CCManagement(), new SOManagment(), new L4L3ServiceShipping(), new Material());
            mng.NewMessage();
        }
        [ClassCleanup]
        public static void CleanupClass()
        {

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    // L4_L3_EVENT
                    connection.Execute("DELETE FROM L4_L3_EVENT WHERE MSG_COUNTER = 1");
                    // L4_L3_CUSTOMER
                    connection.Execute("DELETE FROM L4_L3_CUSTOMER WHERE MSG_COUNTER = 1");
                    // AUX_CONSTANT
                    connection.Execute("DELETE FROM AUX_CONSTANT WHERE CONSTANT_ID = 'ACCEPT_ORDER_IN_SRV'");
                    transaction.Commit();
                }
            }
        }
    }
}
