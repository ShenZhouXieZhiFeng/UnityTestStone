using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动能力
/// </summary>
public class AbilityMove : AbilitiesBase
{
    float moveSpeed = 8f;
    Vector3 moveVector;

    protected override void onAdd()
    {
        base.onAdd();
    }

    protected override void onAddListener()
    {
        base.onAddListener();
        EventCenter.Instance.register<Vector3>(AbilityEvent.ABEvent_Move, onMoveEvent);
    }

    protected override void onRemove()
    {
        base.onRemove();
        EventCenter.Instance.unRegister<Vector3>(AbilityEvent.ABEvent_Move, onMoveEvent);
    }

    void onMoveEvent(Vector3 input)
    {
        float vertical = input.z;
        float horizontal = input.x;
        Transform mTransform = mOwner.transform;
        moveVector = mTransform.forward.normalized * vertical;
        moveVector += mTransform.right.normalized * horizontal;
        moveVector *= moveSpeed;
    }

    protected override void onUpdate()
    {
        base.onUpdate();
        float speed = moveSpeed;
        if (moveVector == Vector3.zero)
        {
            speed = 0;
        }
        mOwner.MoveComp.ApplyMove(moveVector);
        mOwner.AnimatorComp.RefreshMovePars(speed, moveVector.x, moveVector.z);
        moveVector = Vector3.zero;
    }

}
