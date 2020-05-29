using LPKService.Domain.Models.Work;
using NLog;

namespace LPKService.Infrastructure.Repository
{
    public interface IGlobalCheck
    {
        TCheckResult InitResultWithFalse();
        void MngSuccesed(TL4MsgInfo l4MsgInfo, TCheckResult result);
        void SetMsgResult(TL4MsgInfo l4MsgInfo, int newStatus, string newRemark, string logMessage = "");
    }
    public class L4L3InterfaceServiceGlobalCheck:IGlobalCheck
    {
        private Logger logger = LogManager.GetLogger(nameof(Repository));

        public TCheckResult InitResultWithFalse()
        {
            TCheckResult result = new TCheckResult();
            result.isOK = false;
            result.rejType = L4L3InterfaceServiceConst.REJECT_GENERAL;
            result.data = "";
            return result;
        }
        public void MngSuccesed(TL4MsgInfo l4MsgInfo, TCheckResult result)
        {
            SetMsgResult(l4MsgInfo, L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS, "");
            result.isOK = true;
            result.data = l4MsgInfo.msgReport.remark;
        }
        public void SetMsgResult(TL4MsgInfo l4MsgInfo, int newStatus,string newRemark, string logMessage="")
        {
            switch(newStatus)
            {
                case L4L3InterfaceServiceConst.MSG_STATUS_ERROR:
                    l4MsgInfo.msgReport.status = newStatus;
                    l4MsgInfo.msgReport.remark = "ERROR: " + newRemark;
                    if (logMessage.Length > 0)
                        logger.Error("MSG_COUNTER "+l4MsgInfo.msgCounter+"MSG ID"+ l4MsgInfo.msgId+ "ERROR: " + logMessage);
                    break;
                case L4L3InterfaceServiceConst.MSG_STATUS_WARNING:
                    l4MsgInfo.msgReport.status = newStatus;
                    l4MsgInfo.msgReport.remark = "WARNING: " + newRemark;
                    if (logMessage.Length > 0)
                        logger.Error("MSG_COUNTER " + l4MsgInfo.msgCounter + "MSG ID" + l4MsgInfo.msgId + "WARNING: " + logMessage);
                    break;
                case L4L3InterfaceServiceConst.MSG_STATUS_SUCCESS:
                    l4MsgInfo.msgReport.status = newStatus;
                    l4MsgInfo.msgReport.remark = newRemark;
                    break;
            }
        }
    }
}
