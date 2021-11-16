using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetMessageFactory
{
    public static NetMessageBase GetMessage(int messageType)
    {
        EmNetMessageType type = (EmNetMessageType)messageType;
        switch (type)
        {
            case EmNetMessageType.SC_SYNCHRONIZATE:
                return new NetSCSynchronizateMsg();
            case EmNetMessageType.CS_SYNCHRONIZATE:
                return new NetCSSynchronizateMsg();
        }
        return null;
    }
}

public enum EmNetMessageType
{
    // 消息同步 S2C
    SC_SYNCHRONIZATE = 1,

    // 消息同步 C2S
    CS_SYNCHRONIZATE = 2
}

public class FrameSyncMsg
{
    public List<FrameData> frameDtas = new List<FrameData>();
}

public class OperationData
{
    public int mNetId;

    public EmOperation mOperation;
}

public class FrameData
{
    public int mFrame;

    public List<OperationData> mOperationDatas = new List<OperationData>();

    public void Reset()
    {
        mFrame = 0;
        mOperationDatas.Clear();
    }
}

/// <summary>
/// S2C 消息体
/// </summary>
public class NetSCSynchronizateMsg : NetMessageBase
{
    public int CurFrame;

    public List<FrameData> FrameDatas = new List<FrameData>();

    public NetSCSynchronizateMsg()
    {
        MessageType = EmNetMessageType.SC_SYNCHRONIZATE;
    }

    public override void Read(NetBuffer buffer)
    {
        base.Read(buffer);
        CurFrame = buffer.ReadInt();
        int frameCount = buffer.ReadInt();
        for (int i = 0; i < frameCount; i++)
        {
            FrameData frame = new FrameData();
            frame.mFrame = buffer.ReadInt();
            FrameDatas.Add(frame);
            int operationCount = buffer.ReadInt();
            for (int j = 0; j < operationCount; j++)
            {
                OperationData operation = new OperationData();
                operation.mNetId = buffer.ReadInt();
                operation.mOperation = (EmOperation)buffer.ReadInt();
                frame.mOperationDatas.Add(operation);
            }
        }
    }

    public override void Write(NetBuffer buffer)
    {
        base.Write(buffer);
        buffer.WriteInt(CurFrame);
        buffer.WriteInt(FrameDatas.Count);
        for (int i = 0; i < FrameDatas.Count; i++)
        {
            var frame = FrameDatas[i];
            buffer.WriteInt(frame.mFrame);
            buffer.WriteInt(frame.mOperationDatas.Count);
            for (int j = 0; j < frame.mOperationDatas.Count; j++)
            {
                buffer.WriteInt(frame.mOperationDatas[j].mNetId);
                buffer.WriteInt((int)frame.mOperationDatas[j].mOperation);
            }
        }
    }
}

/// <summary>
/// C2S 消息体
/// </summary>
public class NetCSSynchronizateMsg : NetMessageBase
{
    public int mCurFrame = 0;

    public FrameData mFrameData = new FrameData();

    public NetCSSynchronizateMsg()
    {
        MessageType = EmNetMessageType.CS_SYNCHRONIZATE;
    }

    public override void Read(NetBuffer buffer)
    {
        base.Read(buffer);
        mCurFrame = buffer.ReadInt();
        mFrameData.mFrame = buffer.ReadInt();
        var opCount = buffer.ReadInt();
        for (int i = 0; i < opCount; i++)
        {
            OperationData opData = new OperationData();
            opData.mNetId = buffer.ReadInt();
            opData.mOperation = (EmOperation)buffer.ReadInt();
            mFrameData.mOperationDatas.Add(opData);
        }
    }

    public override void Write(NetBuffer buffer)
    {
        base.Write(buffer);
        buffer.WriteInt(mCurFrame);
        buffer.WriteInt(mFrameData.mFrame);
        var mOperationDatas = mFrameData.mOperationDatas;
        var opCount = mOperationDatas.Count;
        buffer.WriteInt(opCount);
        for (int i = 0; i < opCount; i++)
        {
            var opData = mOperationDatas[i];
            buffer.WriteInt(opData.mNetId);
            buffer.WriteInt((int)opData.mOperation);
        }
    }
}

public class NetMessageBase
{
    public EmNetMessageType MessageType;

    public virtual void Read(NetBuffer buffer)
    {
        int n = buffer.ReadInt();
        MessageType = (EmNetMessageType)n;
    }

    public virtual void Write(NetBuffer buffer)
    {
        int n = (int)MessageType;
        buffer.WriteInt(n);
    }

}