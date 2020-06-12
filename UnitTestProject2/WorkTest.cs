using System;
using Dapper;
using LPKService.Domain.Models.CCM;
using LPKService.Domain.Models.Work;
using LPKService.Infrastructure.Builders;
using LPKService.Infrastructure.CCM;
using LPKService.Infrastructure.Material;
using LPKService.Infrastructure.Repository;
using LPKService.Infrastructure.Shipping;
using LPKService.Infrastructure.SOM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oracle.ManagedDataAccess.Client;

namespace UnitTestProject2
{
    [TestClass]
    public class WorkTest
    {
        private const string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA= (SERVICE_NAME=XEPDB1)));User Id=OMK;Password=oracle;";
        private NewMessageBuilder build = new NewMessageBuilder(new L4L3InterfaceServiceGlobalCheck(), new CCManagement(), new SOManagment(), new L4L3ServiceShipping(), new Material());
        private CCManagement cmm = new CCManagement();
        private SOManagment som = new SOManagment();
        private L4L3CustomerRepo custrep = new L4L3CustomerRepo();
        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    connection.Execute("INSERT INTO L4_L3_CUSTOMER(MSG_COUNTER,CUSTOMER_ID,CUSTOMER_NAME,INTERNAL_CUSTOMER_FLAG,CUSTOMER_CLASSIFICATION_TYPE,CUSTOMER_CURRENCY_CODE,ZIP_CODE,ADDRESS_1,ADDRESS_2,ADDRESS_3,CITY,STATE,COUNTRY,CONTACT_NAME,CONTACT_PHONE,CONTACT_FAX,CONTACT_MOBILE,CONTACT_EMAIL,VALIDITY_FLAG,INN,KPP,RWSTATION_CODE,REGION)VALUES(1,1,'ВлГУ','N','type','31231','606107','123412','12334','123444','Муром','Владимирская','Россия','Кульков','1221334','1232324','1231234','1234','N','1231241244','1235154','1212334','12314144')", transaction);
                    transaction.Commit();
                }
            }
        }
        [TestMethod]
        public void TestConnection()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                string res = conn.ExecuteScalar<string>("SELECT user_id FROM all_users where username='OMK'", null);
                Assert.AreEqual("103", res);
            }
        }
        [TestMethod]
        public void TestBlockProcess()
        {
            Assert.AreEqual(false, build.IsBlocked(1));
        }
        [TestMethod]
        public void TestGetCustID()
        {
            Assert.AreEqual(-1, cmm.GetCustIDFromDescr("0"));
            Assert.AreEqual(-1, cmm.GetCustIDFromDescr("1"));
        }
        [TestMethod]
        public void TestCheckClassificationType()
        {
            Assert.IsNull(cmm.CheckClassificationType("12"));
            Assert.IsNull(cmm.CheckClassificationType("14"));
            Assert.IsNull(cmm.CheckClassificationType("65"));
        }
        [TestMethod]
        public void TestOrderExist()
        {
            Assert.AreEqual(false, som.OrderExist("12"));
        }
        [TestMethod]
        public void TestOrderCanBeProcess()
        {
            Assert.AreEqual(true, som.OrderCanBeProcess("12", 1, ""));
        }
        [ClassCleanup]
        public static void CleanupClass()
        {

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    // L4_L3_CUSTOMER
                    connection.Execute("DELETE FROM L4_L3_CUSTOMER WHERE MSG_COUNTER = 1");
                    transaction.Commit();
                }
            }
        }
    }
}
