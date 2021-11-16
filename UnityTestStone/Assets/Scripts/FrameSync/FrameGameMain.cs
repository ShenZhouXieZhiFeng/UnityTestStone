using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameGameMain : MonoBehaviour
{
    public static FrameGameMain instance;

    public uint FrameVirtualDelay = 1;
    public int DebugFrameCount;
    public int DebugServerFrameCount;

    [HideInInspector]
    public PawnManager mPawnMgr;

    bool gameIsStart = false;

    float frameTime;

    private void Awake()
    {
        instance = this;
        mPawnMgr = GetComponent<PawnManager>();
    }

    private void Start()
    {
        GameInit();
    }

    private void Update()
    {
        ServerManager.Instance.Tick(0);
        NetManager.Instance.Tick(0);

        float deltaTime = 0.025f;

        frameTime += Time.deltaTime;
        if (!(frameTime >= deltaTime))
        {
            return;
        }
        frameTime -= deltaTime;

        DebugFrameCount = SynchronizationManager.Instance.FrameCounter;
        DebugServerFrameCount = SynchronizationManager.Instance.ServerFrameCounter;

        if (!SynchronizationManager.Instance.CouldRunCurentFrame)
        {
            return;
        }

        SynchronizationManager.Instance.RunFrameStart();

        GameTick(deltaTime);

        SynchronizationManager.Instance.RunFrameFinish();

    }

    void GameInit()
    {
        mPawnMgr.CallGameInit();
    }

    void GameStart()
    {
        gameIsStart = true;
        mPawnMgr.CallGameStart();
    }

    void GameTick(float deltaTime)
    {
        if (!gameIsStart)
        {
            return;
        }
        mPawnMgr.CallGameTick(deltaTime);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("开始"))
        {
            GameStart();
        }
    }

}
