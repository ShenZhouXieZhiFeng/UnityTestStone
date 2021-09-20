using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCompoent: CompoentBase
{
    public bool IsGrounded = true;
    public bool IsJumping = false;
    public bool Running = false;

    Animator mAnimator;

    protected override void onStart()
    {
        base.onStart();
        mAnimator = mOwner.Aniamtor;
    }

    protected override void onUpdate()
    {
        base.onUpdate();
        refreshAnimatorPars();
    }

    void refreshAnimatorPars()
    {
        mAnimator.SetBool(AnimHelper.isGroundHashId, IsGrounded);
        mAnimator.SetBool(AnimHelper.isJumpAnimHashId, IsJumping);
        mAnimator.SetBool(AnimHelper.runningHashId, Running);
    }

    public void RefreshMovePars(float moveSpeed, float moveX, float moveZ)
    {
        mAnimator.SetFloat(AnimHelper.speedAnimHashId, moveSpeed);
        mAnimator.SetFloat(AnimHelper.moveXAnimHashId, moveX);
        mAnimator.SetFloat(AnimHelper.moveZAnimHashId, moveZ);
    }

    public void PlayAnimation(string animName, int layer = 0, float normalizedTime = 0.0f)
    {
        if (mAnimator != null)
        {
            mAnimator.Play(animName, layer, normalizedTime);
        }
    }
}

class AnimHelper
{
    public static int moveXAnimHashId = Animator.StringToHash("moveX");
    public static int moveZAnimHashId = Animator.StringToHash("moveZ");
    public static int speedAnimHashId = Animator.StringToHash("Speed");
    public static int isJumpAnimHashId = Animator.StringToHash("IsJump");
    public static int isFallAnimHashId = Animator.StringToHash("IsFall");
    //public static int isFallOrJumpAnimHashId = Animator.StringToHash("IsFallOrJump");
    public static int isProneAnimHashId = Animator.StringToHash("IsProne");
    public static int isSquatAnimHashId = Animator.StringToHash("IsSquat");
    public static int isGroundHashId = Animator.StringToHash("IsGround");
    public static int poseStateAnimHashId = Animator.StringToHash("poseState");

    public static int weaponTypeFloatHashId = Animator.StringToHash("weaponTypeFloat");
    public static int meleeTypeFloatHashId = Animator.StringToHash("meleeTypeFloat");
    public static int superJumpHashId = Animator.StringToHash("SuperJump");
    public static int superJumpFloatHashId = Animator.StringToHash("SuperJumpFloat");
    public static int barrierHitHashId = Animator.StringToHash("BarrierHit");

    public static int reloadSpeedHashId = Animator.StringToHash("ReloadSpeed");
    //public static int drinkSpeedHashId = Animator.StringToHash("DrinkSpeed");
    public static int attackSpeedHashId = Animator.StringToHash("MeleeAttackSpeed");

    public static int startChargeHashId = Animator.StringToHash("StartCharge");
    public static int chargeTypeHashId = Animator.StringToHash("ChargeType");
    public static int chargeSpeedHashId = Animator.StringToHash("ChargeSpeed");
    public static int weaponTypeHashId = Animator.StringToHash("weaponType");
    public static int rescueHashId = Animator.StringToHash("Rescuing");
    public static int runningHashId = Animator.StringToHash("IsRun");
    public static int reloadTypeHashId = Animator.StringToHash("ReloadType");
    public static int isOnMountHashId = Animator.StringToHash("IsOnMount");
    public static int isDeadHashId = Animator.StringToHash("Died");
    //public static int singleReloadHashId = Animator.StringToHash("SingleReload");
    public static int isEatEndHashId = Animator.StringToHash("IsEatEnd");
    public static int eatTypeHashId = Animator.StringToHash("EatType");
}