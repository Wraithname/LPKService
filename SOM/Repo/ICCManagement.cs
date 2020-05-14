using Work.Models;
using SOM.Models;
using Dapper.Oracle;

namespace SOM.Repo
{
    interface ICCManagement
    {
        TCheckResult CustomerMng(L4L3Customer customer,TL4MsgInfo l4MsgInfo);
        bool FillAddressEngine(L4L3Customer customer, string pModUserId, OracleDynamicParameters odp = null);
        int GetCustIDFromDescr(string sCustomerDescrId, OracleDynamicParameters odp = null);
        string CheckClassificationType(string strClassification, OracleDynamicParameters odp = null);
        bool CheckCustomerExists(string strCustomerDescrId, OracleDynamicParameters odp = null);
    }
}
