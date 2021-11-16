using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager
{
    public static NetManager Instance = new NetManager();

    public void Tick(float deltaTime)
    {
        var msgs = FrameNetMgr.Instance.Messages;
        while (msgs.Count > 0)
        {
            var msg = msgs.Dequeue();
            ProcessMessage(msg);
        }
    }

    void ProcessMessage(NetMessageBase msg)
    {
        switch (msg.MessageType)
        {
            case EmNetMessageType.SC_SYNCHRONIZATE:
                onReceiverSyncData(msg as NetSCSynchronizateMsg);
                break;
            default:
                break;
        }
    }

    void onReceiverSyncData(NetSCSynchronizateMsg msg)
    {
        SynchronizationManager.Instance.AddFrameData(msg.CurFrame, msg.FrameDatas);
    }
}
