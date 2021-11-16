using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizationManager
{
    public static SynchronizationManager Instance = new SynchronizationManager();

    public int FrameCounter = 1;

    public int ServerFrameCounter = 1;

    public bool CouldRunCurentFrame
    {
        get
        {
            return FrameCounter < ServerFrameCounter;
        }
    }

    // 本地帧数据
    private FrameData localFrameData = new FrameData();
    // 服务端过来的帧数据
    private List<FrameData> serverFrameDatas = new List<FrameData>();

    public void AddOperation(uint netId, EmOperation operation)
    {
        OperationData opData = new OperationData()
        { 
            mNetId = (int)netId,
            mOperation = operation
        };
        localFrameData.mFrame = FrameCounter;
        localFrameData.mOperationDatas.Add(opData);
    }

    /// <summary>
    /// 接收服务端传过来的数据
    /// </summary>
    public void AddFrameData(int serverFrame, List<FrameData> datas)
    {
        if (serverFrame == ServerFrameCounter)
        {
            ServerFrameCounter++;
            serverFrameDatas.AddRange(datas);
        }
    }

    public void RunFrameStart()
    {
        FrameCounter++;

        // 应用玩家操作
        if (serverFrameDatas.Count > 0)
        {
            var frame = serverFrameDatas[0];
            //if (frame.mFrame != FrameCounter)
            //{
            //    Debug.LogErrorFormat("帧和服务器消息对不上 当前{0}, 服务器{1}", FrameCounter, frame.mFrame);
            //}
            //else
            //{
                foreach (var operation in frame.mOperationDatas)
                {
                    var player = FrameGameMain.instance.mPawnMgr.getPawnCharacter((uint)operation.mNetId);
                    if (player != null)
                    {
                        player.DoOperation(operation);
                    }
                }
            //}
        }
    }

    public void RunFrameFinish()
    {
        // 上报玩家操作
        if (localFrameData.mOperationDatas.Count > 0)
        {
            NetCSSynchronizateMsg msg = new NetCSSynchronizateMsg();
            msg.mCurFrame = FrameCounter;
            msg.mFrameData = localFrameData;
            FrameNetMgr.Instance.Send(msg);
            clearOperationData();
        }

        if (serverFrameDatas.Count > 0)
        {
            serverFrameDatas.RemoveAt(0);
        }
    }

    void clearOperationData()
    {
        localFrameData.mOperationDatas.Clear();
    }

}
