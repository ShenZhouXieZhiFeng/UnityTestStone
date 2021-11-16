using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager
{
    public static ServerManager Instance = new ServerManager();

    public int ServerFrameCount = 0;

    private NetBuffer readNetBuff = new NetBuffer();
    private NetBuffer writeNetBuff = new NetBuffer();
    //private byte[] sendBuffer = new byte[1024];
    //private byte[] analysisBuffer = new byte[1024];
    //private byte[] analysisTempBuffer = new byte[1024];
    //private int analysisBufferDataLength;

    private Dictionary<EmNetMessageType, List<NetMessageBase>> messages = new Dictionary<EmNetMessageType, List<NetMessageBase>>();

    public void OnReceiveMessage(NetBuffer buffer)
    {
        readNetBuff = buffer;
        readNetBuff.ResetPointer();
        analysisMsg();
        //int read = buffer.DataLength;
        //if (read > 0)
        //{
        //    Array.Copy(buffer.Buffer, 0, analysisBuffer, analysisBufferDataLength, read);
        //    analysisBufferDataLength += read;
        //    analysisMsg();
        //}
    }

    public void SendToClient(NetMessageBase msg)
    {
        writeNetBuff.Reset();
        msg.Write(writeNetBuff);
        // 模拟发送给客户端
        FrameNetMgr.Instance.OnReceive(writeNetBuff);
    }

    public void Tick(float deltaTime)
    {
        NetSCSynchronizateMsg scMsg = new NetSCSynchronizateMsg();
        scMsg.CurFrame = ServerFrameCount;
        if (messages.ContainsKey(EmNetMessageType.CS_SYNCHRONIZATE))
        {
            var csMsgs = messages[EmNetMessageType.CS_SYNCHRONIZATE];
            for (int i = 0; i < csMsgs.Count; i++)
            {
                NetCSSynchronizateMsg csMsg = csMsgs[i] as NetCSSynchronizateMsg;
                scMsg.FrameDatas.Add(csMsg.mFrameData);
            }
        }
        SendToClient(scMsg);

        ServerFrameCount++;

        messages.Clear();
    }

    void analysisMsg()
    {
        NetCSSynchronizateMsg msg = new NetCSSynchronizateMsg();
        msg.Read(readNetBuff);
        addMessage(msg);
        //while (analysisBufferDataLength > 0)
        //{
        //    int msgLength = BitConverter.ToInt32(analysisBuffer, 0);
        //    if (msgLength <= analysisBufferDataLength - 4)
        //    {
        //        readNetBuff.Set(analysisBuffer, 4, msgLength);
        //        int messageType = BitConverter.ToInt32(analysisBuffer, 4);
        //        var msg = NetMessageFactory.GetMessage(messageType);
        //        if (msg != null)
        //        {
        //            msg.Read(readNetBuff);
        //            addMessage(msg);
        //        }
        //        else
        //        {
        //            Debug.LogError("wrong message type " + messageType);
        //        }
        //        int remain = analysisBufferDataLength - msgLength - 4;
        //        Array.Copy(analysisBuffer, 4 + msgLength, analysisTempBuffer, 0, remain);
        //        Array.Copy(analysisTempBuffer, 0, analysisBuffer, 0, remain);
        //        analysisBufferDataLength = remain;
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}
    }

    void addMessage(NetMessageBase msg)
    {
        var type = msg.MessageType;
        if (messages.ContainsKey(type))
        {
            messages[type].Add(msg);
        }
        else
        {
            List<NetMessageBase> msgList = new List<NetMessageBase>();
            msgList.Add(msg);
            messages.Add(type, msgList);
        }
    }

}
