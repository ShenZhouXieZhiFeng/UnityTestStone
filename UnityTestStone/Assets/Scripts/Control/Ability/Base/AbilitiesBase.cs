using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 能力基类
/// </summary>
public abstract class AbilitiesBase
{
    protected AbilityManager mAbilityMgr;

    protected AbilityOwnerInterface mOwner;

    public EAbilityType EType;

    public AbilityConfig mConfig;

    public bool AutoActive { get { return mConfig.autoActive; } }

    /// <summary>
    /// 该能力是否处于激活状态，未激活的能力，其刷新函数不会被执行
    /// </summary>
    public bool IsActive { get; private set; }

    #region 供管理类调用

    public void InjectData(AbilityOwnerInterface owner, AbilityManager abMgr)
    {
        mOwner = owner;
        mAbilityMgr = abMgr;
    }

    public void CallAdd()
    {
        onAdd();
        onAddListener();
    }

    public void CallActive()
    {
        IsActive = true;
        onActive();
    }

    public void CallDeactive()
    {
        IsActive = false;
        onDeactive();
    }

    public void CallUpdate()
    {
        onUpdate();
    }

    public void CallLateUpdate()
    {
        onLateUpdate();
    }

    public void CallFixedUpdate()
    {
        onFixedUpdate();
    }

    public void CallRemove()
    {
        onRemoveListener();
        onRemove();
    }

    #endregion

    #region 子类重写

    protected virtual void onAdd() { }

    protected virtual void onAddListener() { }

    protected virtual void onRemoveListener() { }

    protected virtual void onActive() { }

    protected virtual void onDeactive() { }

    protected virtual void onUpdate() { }

    protected virtual void onLateUpdate() { }

    protected virtual void onFixedUpdate() { }

    protected virtual void onRemove() { }

    #endregion

    #region 能力激活检测

    /// <summary>
    /// 前置能力
    /// </summary>
    public EAbilityType PrepositionAbility { get; private set; }
    /// <summary>
    /// 能力激活检测标志位，且的关系
    /// </summary>
    List<EAbilityFlag> andFlags;

    public List<EAbilityFlag> GetActiveAndFlags()
    {
        return andFlags;
    }

    protected void setPrepositionAbility(EAbilityType eType)
    {
        PrepositionAbility = eType;
    }

    protected void setAndFlags(params EAbilityFlag[] arr)
    {
        if (andFlags == null)
        {
            andFlags = new List<EAbilityFlag>();
        }
        andFlags.AddRange(arr);
    }

    #endregion

    #region 能力停止检测



    #endregion

    #region 子类工具

    /// <summary>
    /// 激活自身
    /// </summary>
    protected void activeSelf()
    {
        mAbilityMgr.ActiveAbility(EType);
    }

    /// <summary>
    /// 取消激活自身
    /// </summary>
    protected void deActiveSelf()
    {
        mAbilityMgr.DeactiveAbility(EType);
    }

    /// <summary>
    /// 主动优先级检测，如果有优先级低于自身的能力激活，会被中断激活
    /// </summary>
    protected void checkPriority()
    {
        mAbilityMgr.CheckAbilityPriority(EType);
    }

    #endregion
}
