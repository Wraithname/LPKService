using Work.Models;
using SOM.Models;
using Dapper.Oracle;

namespace SOM.Repo
{
    interface ICCManagement
    {
        TCheckResult CustomerMng(TL4MsgInfo l4MsgInfo);
        bool FillAddressEngine(AddresEngine addrEngine,string pModUserId, OracleDynamicParameters odp = null);
        int GetCustIDFromDescr(string sCustomerDescrId);
        string CheckClassificationType(string strClassification, OracleDynamicParameters odp = null);
        bool CheckCustomerExists(string strCustomerDescrId, OracleDynamicParameters odp = null);
    }
}
