using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 运动能力组件，控制单位移动，旋转等基本能力
/// </summary>
public class MovementCompoent : CompoentBase
{
    CharacterController mCharacterController;

    protected override void onStart()
    {
        base.onStart();
        mCharacterController = mOwner.CharacterController;
    }

    protected override void onLateUpdate()
    {
        base.onLateUpdate();
        updateMove();
        groundedStateCheck();
    }

    #region 状态判定

    bool curIsGrounded = false;

    public bool IsGround
    {
        get { return mCharacterController.isGrounded; }
    }

    void groundedStateCheck()
    {
        var curState = mCharacterController.isGrounded;
        if (curIsGrounded != curState)
        {
            EventCenter.Instance.targetFire(mOwner.transform, AbilityEvent.ABEvent_GroundedChange, curState);
            curIsGrounded = curState;
        }
    }

    #endregion

    #region 旋转

    /// <summary>
    /// 旋转
    /// </summary>
    /// <param name="rotateVector"></param>
    public void Rotate(Vector3 rotateVector)
    {
        Quaternion rotDelta = Quaternion.Euler(rotateVector);
        Quaternion taretRotation = rotDelta * mCharacterController.transform.rotation;
        mCharacterController.transform.rotation = taretRotation;
    }

    #endregion

    #region 位移，掉落，跳跃

    Vector3 moveVector = Vector3.zero;
    Vector3 jumpVector = Vector3.zero;
    Vector3 fallVector = Vector3.zero;

    /// <summary>
    /// 位移
    /// </summary>
    /// <param name="moveVector"></param>
    public void ApplyMove(Vector3 vector)
    {
        moveVector += vector;
    }

    /// <summary>
    /// 下落
    /// </summary>
    /// <param name="gravity"></param>
    public void ApplyFall(float gravity)
    {
        fallVector.y = gravity;
    }

    public void ApplyJump(Vector3 vector)
    {
        jumpVector += vector;
    }

    void updateMove()
    {
        Vector3 finalMove = moveVector + fallVector + jumpVector;
        if (finalMove != Vector3.zero)
        {
            mCharacterController.Move(finalMove * Time.deltaTime);
        }
        moveVector = Vector3.zero;
        fallVector = Vector3.zero;
        jumpVector = Vector3.zero;
    }

    #endregion

}
