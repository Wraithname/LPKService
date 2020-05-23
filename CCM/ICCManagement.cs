using Repository.WorkModels;
using CCM.Models;
using Dapper.Oracle;
using CCM.Infostraction;

namespace CCM
{
    public interface ICCManagement
    {
        TCheckResult CustomerMng(TL4MsgInfo l4MsgInfo);
        bool FillAddressEngine(L4L3Customer customer,AddressEngine addressEngine, string pModUserId, OracleDynamicParameters odp = null);
        int GetCustIDFromDescr(string sCustomerDescrId, OracleDynamicParameters odp = null);
        string CheckClassificationType(string strClassification, OracleDynamicParameters odp = null);
        bool CheckCustomerExists(string strCustomerDescrId, OracleDynamicParameters odp = null);
    }
}
