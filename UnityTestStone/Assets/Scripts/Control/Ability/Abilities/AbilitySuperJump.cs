using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 二段跳能力
/// </summary>
public class AbilitySuperJump : AbilitiesBase
{
    bool isJump = false;
    float beginJumpTime = 0;
    float jumpPower = 20f;
    float jumpDuringTime = 0.8f;
    Vector3 jumpVector = Vector3.zero;
    float curJumpPow = 0;

    protected override void onAdd()
    {
        base.onAdd();
    }

    protected override void onActive()
    {
        base.onActive();
        beginJump();
    }

    protected override void onDeactive()
    {
        base.onDeactive();
        stopJump();
    }

    protected override void onUpdate()
    {
        base.onUpdate();
        if (!isJump)
        {
            if (mOwner.MoveComp.IsGround)
            {
                jumpEnd();
            }
            return;
        }
        if (Time.time - beginJumpTime > jumpDuringTime)
        {
            stopJump();
        }
        else
        {
            jumpVector.y = curJumpPow;
            mOwner.MoveComp.ApplyJump(jumpVector);
            if (curJumpPow > 0)
            {
                float delta = Time.deltaTime;
                float jumpDelta = jumpPower * delta;
                curJumpPow -= jumpDelta;
                curJumpPow = Mathf.Max(0, curJumpPow);
            }
        }
    }

    void beginJump()
    {
        curJumpPow = jumpPower;
        beginJumpTime = Time.time;
        isJump = true;
    }

    void stopJump()
    {
        beginJumpTime = 0;
        jumpVector = Vector3.zero;
        isJump = false;
    }

    void jumpEnd()
    {
        deActiveSelf();
    }

}
