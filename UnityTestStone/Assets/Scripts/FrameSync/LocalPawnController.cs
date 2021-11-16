using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmOperation
{ 
    None,
    MOVE_FORWARD,
    MOVE_BACKGROUND,
    MOVE_LEFT,
    MOVE_RIGHT
}

/// <summary>
/// 本地棋子控制器
/// </summary>
public class LocalPawnController : PawnControllerBase
{
    protected override void onInit()
    {

    }

    protected override void onTick(float deltaTime)
    {
        // 前后
        if (Input.GetKey(KeyCode.W))
        {
            addOperation(EmOperation.MOVE_FORWARD);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            addOperation(EmOperation.MOVE_BACKGROUND);
        }
        // 左右
        else if (Input.GetKey(KeyCode.A))
        {
            addOperation(EmOperation.MOVE_LEFT);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            addOperation(EmOperation.MOVE_RIGHT);
        }
    }

    void addOperation(EmOperation emOperation)
    {
        SynchronizationManager.Instance.AddOperation(mNetID, emOperation);
    }

}
