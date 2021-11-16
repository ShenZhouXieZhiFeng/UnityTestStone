using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameNetMgr
{
    public static FrameNetMgr Instance = new FrameNetMgr();

    private NetBuffer readNetBuff = new NetBuffer();
    private NetBuffer writeNetBuff = new NetBuffer();
    //private byte[] analysisBuffer = new byte[1024];
    //private byte[] analysisTempBuffer = new byte[1024];
    //private int analysisBufferDataLength;

    public Queue<NetMessageBase> Messages = new Queue<NetMessageBase>();

    // 发送数据
    public void Send(NetCSSynchronizateMsg netMessage)
    {
        writeNetBuff.Reset();
        netMessage.Write(writeNetBuff);

        writeNetBuff.ResetPointer();

        //NetCSSynchronizateMsg tempMsg = new NetCSSynchronizateMsg();
        //tempMsg.Read(writeNetBuff);

        //Debug.LogError(netMessage.MessageType + " | " + netMessage.mCurFrame + " | " + netMessage.mFrameData.mOperationDatas.Count);
        //Debug.LogError(tempMsg.MessageType + " | " + tempMsg.mCurFrame + " | " + tempMsg.mFrameData.mOperationDatas.Count);

        // 模拟发送数据，这部分可以转接服务端
        ServerManager.Instance.OnReceiveMessage(writeNetBuff);
    }

    // 接收数据，中间省去了socket的过程
    public void OnReceive(NetBuffer dataBuffer)
    {
        readNetBuff = dataBuffer;
        analysisMsg();
        //int read = dataBuffer.DataLength;
        //if (read > 0)
        //{
        //    Array.Copy(dataBuffer.Buffer, 0, analysisBuffer, analysisBufferDataLength, read);
        //    analysisBufferDataLength += read;
        //    analysisMsg();
        //}
    }

    void analysisMsg()
    {
        readNetBuff.ResetPointer();

        NetSCSynchronizateMsg msg = new NetSCSynchronizateMsg();
        msg.Read(readNetBuff);

        //Debug.LogError(msg.MessageType + " | " + msg.FrameDatas.Count);

        Messages.Enqueue(msg);
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
        //            Messages.Enqueue(msg);
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
}

