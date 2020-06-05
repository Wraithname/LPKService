using LPKService.Domain.Models.Work;
using Dapper.Oracle;
using LPKService.Domain.Models.CCM;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса CCManagement
    /// </summary>
    public interface ICCManagement
    {
        TCheckResult CustomerMng(TL4MsgInfo l4MsgInfo);
        bool FillAddressEngine(L4L3Customer customer,IAddressEngine addressEngine, string pModUserId, OracleDynamicParameters odp = null);
        int GetCustIDFromDescr(string sCustomerDescrId, OracleDynamicParameters odp = null);
        string CheckClassificationType(string strClassification, OracleDynamicParameters odp = null);
        bool CheckCustomerExists(string strCustomerDescrId, OracleDynamicParameters odp = null);
    }
}
