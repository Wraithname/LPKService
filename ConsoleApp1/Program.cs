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
            INewMessageBuilder mng = new NewMessageBuilder(new L4L3InterfaceServiceGlobalCheck(), new CCManagement(), new SOManagment(), new L4L3ServiceShipping(), new Material());
            mng.NewMessage();
        }
        public static void InitClass()
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
    }
}
