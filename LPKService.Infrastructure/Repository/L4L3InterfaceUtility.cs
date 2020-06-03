using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using NLog;
using LPKService.Repository;

namespace LPKService.Infrastructure.Repository
{
    public class L4L3InterfaceUtility
    {
        private Logger logger = LogManager.GetLogger(nameof(Repository));
        private string[] coloursAttributes = { "PAINT_FIN_BOTTOM_COLOUR", "PAINT_FIN_TOP_COLOUR", "PAINT_PRIME_TOP_TYPE", "PAINT_PRIME_BOTTOM_TYPE" };
        /// <summary>
        /// Проверка выделения аттрибута
        /// </summary>
        /// <param name="SAttrbCode">Код аттрибута</param>
        /// <returns>
        /// true - аттрибут выделен
        /// false - аттрибут не выделен
        /// </returns>
        public bool IsAColorAttribute(string SAttrbCode)
        {
            foreach (string attr in coloursAttributes)
            {
                if (SAttrbCode == attr)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Получение ИД заказа
        /// </summary>
        /// <param name="sSoDescrID">Текстовый ИД заказа</param>
        /// <param name="odp"></param>
        /// <returns>ИД заказа</returns>
        public int GetSoIdFromDescr(string sSoDescrID, OracleDynamicParameters odp = null)
        {
            string stm = @"SELECT SO_ID FROM   SALES_ORDER_HEADER WHERE  SO_DESCR_ID = " + sSoDescrID + "";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                return connection.ExecuteScalar<int>(stm, odp);
            }
        }
        /// <summary>
        /// Получение строкого значения списка значений
        /// </summary>
        /// <param name="SAttrbCode">Код аттрибута</param>
        /// <param name="SAttrbValue">Значение аттрибута</param>
        /// <param name="odp"></param>
        /// <returns>Строковое значение списка значений</returns>
        public string DecodeL4L3AttribValueAlpha(string SAttrbCode, string SAttrbValue, OracleDynamicParameters odp = null)
        {
            if (!IsAColorAttribute(SAttrbCode))
                return SAttrbValue;
            string stm = @"SELECT ACR.AN_CONTROL_VALUE AS AN_CONTROL_VALUE FROM ATTRB_CONTROL_RULES ACR, ATTRB_CONTROL_VERSION ACV WHERE TRIM(SUBSTR(" + SAttrbValue + ", 1, length(" + SAttrbValue + "))) = " +
"TRIM(SUBSTR(ACR.AN_CONTROL_VALUE, 1, length(" + SAttrbValue + ")))AND ACR.ATTRB_CODE =" + SAttrbCode + "AND ACR.DUMMY_KEY = ACV.DUMMY_KEY " +
"AND TO_CHAR(ACV.EXPIRATION_DATE, 'DD/MM/YYYY') = '01/01/1970' ";
            using (OracleConnection connection = BaseRepo.GetDBConnection())
            {
                return connection.ExecuteScalar<string>(stm, odp);
            }
        }
    }
}
