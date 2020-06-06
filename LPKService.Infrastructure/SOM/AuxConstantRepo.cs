using Dapper;
using LPKService.Domain.Interfaces;
using LPKService.Domain.Models.SOM;
using LPKService.Domain.BaseRepository;
using Oracle.ManagedDataAccess.Client;

namespace LPKService.Infrastructure.SOM
{
    public class AuxConstantRepo : SOMRepoBase,IAuxConstant
    {
        private AuxConstant auxConstant;
        /// <summary>
        /// Получение значения константы с плавающей запятой
        /// </summary>
        /// <param name="constId">ИД константы</param>
        /// <returns>Значение с плавающей запятой</returns>
        public float GetFloatAuxConstant(string constId)
        {
            AuxConGetData(constId);
            return auxConstant.floatVal;
        }
        /// <summary>
        /// Получение целочисленного значения константы
        /// </summary>
        /// <param name="constId">ИД константы</param>
        /// <returns>Целочисленное значение</returns>
        public int GetIntAuxConstant(string constId)
        {
            AuxConGetData(constId);
            return auxConstant.integerVal;
        }
        /// <summary>
        /// Получение строкового значения константы
        /// </summary>
        /// <param name="constId">ИД константы</param>
        /// <returns>Строковое значение</returns>
        public string GetStringAuxConstant(string constId)
        {
            AuxConGetData(constId);
            return auxConstant.charVal;
        }
        /// <summary>
        /// Получение значений константы
        /// </summary>
        /// <param name="constId">ИД константы</param>
        private void AuxConGetData(string constId)
        {
            string sqlstr = $"SELECT INTEFER_VALUE,CHAR_VALUE,FLOAT_VALUE FROM AUX_CONTSTANT WHERE CONSTANT_ID= {constId}";
            using (OracleConnection conn = GetConnection())
            {
                conn.Open();
                auxConstant = conn.ExecuteScalar<AuxConstant>(sqlstr,null);
                conn.Clone();
                conn.Dispose();
            }
        }
    }
}
