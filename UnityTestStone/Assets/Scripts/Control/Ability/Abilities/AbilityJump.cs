using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一段跳跃能力
/// </summary>
public class AbilityJump : AbilitiesBase
{
    bool canJump = true;
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

    protected override void onAddListener()
    {
        base.onAddListener();
        EventCenter.Instance.targetRegister<bool>(mOwner.gameObject, AbilityEvent.ABEvent_GroundedChange, onGroundStateChange);
        EventCenter.Instance.register(AbilityEvent.ABEvent_Jump, onJumpEvent);
    }

    protected override void onRemoveListener()
    {
        base.onRemoveListener();
        EventCenter.Instance.targetUnregister<bool>(mOwner.gameObject, AbilityEvent.ABEvent_GroundedChange, onGroundStateChange);
        EventCenter.Instance.unRegister(AbilityEvent.ABEvent_Jump, onJumpEvent);
    }

    protected override void onUpdate()
    {
        base.onUpdate();
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

    void onGroundStateChange(bool isGround)
    {
        if (isGround)
        {
            canJump = true;
        } 
    }

    void onJumpEvent()
    {
        //Debug.LogError("onJumpEvent:" + mOwner.MoveComp.IsGround);
        if (canJump)
        {
            beginJump();
        }
        else
        {
            if (mAbilityMgr.HasActiveAbility(EAbilityType.superJump))
            {
                return;
            }
            else
            {
                if (isJump)
                {
                    stopJump();
                    mAbilityMgr.ActiveAbility(EAbilityType.superJump);
                }
            }
        }
    }

    void beginJump()
    {
        //Debug.LogError("beginJump");
        curJumpPow = jumpPower;
        beginJumpTime = Time.time;
        canJump = false;
        isJump = true;
        activeSelf();
    }

    void stopJump()
    {
        //Debug.LogError("endJump");
        beginJumpTime = 0;
        jumpVector = Vector3.zero;
        isJump = false;
        deActiveSelf();
    }
}
