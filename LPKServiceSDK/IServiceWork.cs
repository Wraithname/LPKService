﻿using Repository.WorkModels;

namespace LPKServiceSDK
{
    public interface IServiceWork
    {
        void MngLoop();
        void GetAutoCloseOrder();
        void GetNewMessage();
        void GetNewMessageDelivery();
        void UpdateStatusMessage(int msgCounter,int status,string remark);
        bool ExistsBol(string bolId, bool selChild);
        bool ExistBolPosition(string bolId, string posId);
        void CreateBol(int msgCounter, string bolId, bool allBol);
        void UpdateBolPosition(int msgCounter, int posNumId);
        void DeleteBol(int msgCounter, string bolId, int posNumId);
        void UpdateMsgStatus(TL4MsgInfo l4MsgInfo);
        string GetBolId(int msgCounter);
        bool IsUpdate(int msgCounter, string bolId, int bolPosNumId);

    }
}
