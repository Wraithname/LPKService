using Dapper;
using LPKService.Infrastructure.Builders;
using LPKService.Infrastructure.CCM;
using LPKService.Infrastructure.Material;
using LPKService.Infrastructure.Repository;
using LPKService.Infrastructure.Shipping;
using LPKService.Infrastructure.SOM;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        private const string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA= (SERVICE_NAME=XEPDB1)));User Id=OMK;Password=oracle;";
        static void Main(string[] args)
        {
            //InitClass();
            INewMessageBuilder mng = new NewMessageBuilder(new L4L3InterfaceServiceGlobalCheck(), new CCManagement(), new SOManagment(), new L4L3ServiceShipping(), new Material());
            mng.NewMessage();
            CleanupClass();
        }
        public static void InitClass()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    // L4_L3_EVENT
                    connection.Execute("INSERT INTO L4_L3_EVENT(MSG_COUNTER,MSG_ID,MSG_DATETIME,OP_CODE,MSG_STATUS,BLOCK_FOR_PROCESS)VALUES(1,4301,SYSDATE,1,1,1)", transaction);
                    // L4_L3_SO_HEADER
                    connection.Execute("INSERT INTO L4_L3_SO_HEADER(MSG_COUNTER,SO_ID,INSERT_DATE,CUSTOMER_ID,CUSTOMER_PO,CUSTOMER_PO_DATE,SO_NOTES,INQUIRY_REF_NUMBER,INQUIRY_REF_DATE,STATUS,HEADER_NOTE)" +
                    "VALUES(1,1,SYSDATE,1,1,SYSDATE,'31231','606107',SYSDATE,1,'Муром')", transaction);
                    // AUX_CONSTANT
                    connection.Execute("INSERT INTO AUX_CONSTANT(CONSTANT_ID,DESCRIPTION,INTEGER_VALUE,CHAR_VALUE,FLOAT_VALUE,INFO,MOD_USER_ID,VALUE_MODIFIABLE_FLAG)VALUES('ONE_TO_SEVERAL_ORDER_FROM_SAP','Принятие заказа',0,'N',0,'lelelo',103,'N')", transaction);
                    connection.Execute("INSERT INTO AUX_CONSTANT(CONSTANT_ID,DESCRIPTION,INTEGER_VALUE,CHAR_VALUE,FLOAT_VALUE,INFO,MOD_USER_ID,VALUE_MODIFIABLE_FLAG)VALUES('SO_NUMERATION','Принятие заказа',0,'N',0,'lelelo',103,'N')", transaction);
                    transaction.Commit();
                }
            }
        }
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
                    connection.Execute("DELETE FROM L4_L3_SO_HEADER WHERE MSG_COUNTER = 1");
                    // AUX_CONSTANT
                    connection.Execute("DELETE FROM AUX_CONSTANT WHERE CONSTANT_ID = 'ONE_TO_SEVERAL_ORDER_FROM_SAP'");
                    connection.Execute("DELETE FROM AUX_CONSTANT WHERE CONSTANT_ID = 'SO_NUMERATION'");
                    transaction.Commit();
                }
            }
        }
    }
}
