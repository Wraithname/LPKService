using LPKService.Domain.Models.Work;
using System;

namespace LPKService.Domain.Interfaces
{
    public interface IMaterial
    {
        bool InsertNewMovement(string matcode,string mt_reconciliation, DateTime movement_datetime, float movement_qty,int userID,int num,string title, string mes="");
        TCheckResult L4L3MaterialMovement(TL4MsgInfo l4MsgInfo);
    }
}
