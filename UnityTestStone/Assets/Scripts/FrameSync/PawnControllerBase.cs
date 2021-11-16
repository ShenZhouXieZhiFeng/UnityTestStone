using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 棋子控制器基类
/// </summary>
public class PawnControllerBase
{
    public Pawn mPawn;

    public uint mNetID => mPawn.mNetID;

    public void SetPawnController(Pawn pawn)
    {
        mPawn = pawn;
    }

    public void CallInit()
    {
        onInit();
    }

    public void CallDestory()
    {
        onDestory();
    }

    public void CallTick(float deltaTime)
    {
        onTick(deltaTime);
    }

    protected virtual void onInit()
    { 
    
    }

    protected virtual void onTick(float deltaTime)
    { 
    
    }

    protected virtual void onDestory()
    { 
    
    }

}
