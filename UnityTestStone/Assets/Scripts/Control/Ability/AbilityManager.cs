using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色能力管理类
/// </summary>
public class AbilityManager
{
    #region 数据

    /// <summary>
    /// 能力持有者
    /// </summary>
    AbilityOwnerInterface mOwner;
    /// <summary>
    /// 能力集合
    /// </summary>
    Dictionary<EAbilityType, AbilitiesBase> mAbilities = new Dictionary<EAbilityType, AbilitiesBase>();
    /// <summary>
    /// 已激活的能力集合
    /// </summary>
    Dictionary<EAbilityType, AbilitiesBase> mActiveAbilities = new Dictionary<EAbilityType, AbilitiesBase>();
    /// <summary>
    /// 已激活的能力集合(数组，用于方便遍历)
    /// </summary>
    List<AbilitiesBase> mActiveAbilitiesList = new List<AbilitiesBase>();

    #endregion

    #region 生命周期

    public void CallCreate(AbilityOwnerInterface owner)
    {
        mOwner = owner;
    }

    public void CallUpdate()
    {
        for (int i = mActiveAbilitiesList.Count - 1; i >= 0; i--)
        {
            mActiveAbilitiesList[i].CallUpdate();
        }
    }

    public void CallLateUpdate()
    {
        for (int i = mActiveAbilitiesList.Count - 1; i >= 0; i--)
        {
            mActiveAbilitiesList[i].CallLateUpdate();
        }
    }

    public void CallFixedUpdate()
    {
        for (int i = mActiveAbilitiesList.Count - 1; i >= 0; i--)
        {
            mActiveAbilitiesList[i].CallFixedUpdate();
        }
    }

    public void CallDestory()
    {
        foreach (var ab in mAbilities.Values)
        {
            ab.CallRemove();
        }
        mAbilities.Clear();
        mActiveAbilities.Clear();
        mActiveAbilitiesList.Clear();
    }

    #endregion

    #region 对外接口

    /// <summary>
    /// 添加能力
    /// </summary>
    /// <param name="eType"></param>
    public void AddAbility(EAbilityType eType)
    {
        addAbility(eType);
    }

    /// <summary>
    /// 移除能力
    /// </summary>
    /// <param name="eType"></param>
    public void RemoveAbility(EAbilityType eType)
    {
        removeAbility(eType);
    }

    /// <summary>
    /// 激活能力
    /// </summary>
    /// <param name="eType"></param>
    public void ActiveAbility(EAbilityType eType)
    {
        activeAbility(eType);
    }

    /// <summary>
    /// 取消激活能力
    /// </summary>
    /// <param name="eType"></param>
    public void DeactiveAbility(EAbilityType eType)
    {
        deactiveAbility(eType);
    }

    /// <summary>
    /// 是否包含指定能力
    /// </summary>
    /// <param name="eType"></param>
    /// <returns></returns>
    public bool HasAbility(EAbilityType eType)
    {
        return mAbilities.ContainsKey(eType);
    }

    /// <summary>
    /// 是否包含指定能力被激活
    /// </summary>
    /// <param name="eType"></param>
    /// <returns></returns>
    public bool HasActiveAbility(EAbilityType eType)
    {
        return mActiveAbilities.ContainsKey(eType);
    }

    /// <summary>
    /// 优先级检测，如果有优先级低于目标的能力处于激活状态，会被中断激活
    /// </summary>
    /// <param name="eType"></param>
    public void CheckAbilityPriority(EAbilityType eType)
    {
        checkPriority(eType);
    }

    #endregion

    #region 内部逻辑

    void addAbility(EAbilityType eType)
    {
        if (mAbilities.ContainsKey(eType))
        {
            return;
        }
        AbilitiesBase newAb = AbilityHelper.GetNewAbility(eType);
        if (newAb != null)
        {
            //Debug.LogError("addAbility: " + eType);
            // 注入数据
            newAb.InjectData(mOwner, this);
            // 先添加能力
            newAb.CallAdd();
            mAbilities.Add(eType, newAb);
            // 自动激活能力
            if (newAb.AutoActive)
            {
                activeAbility(eType);
            }
        }
    }

    void removeAbility(EAbilityType eType)
    {
        if (!mAbilities.ContainsKey(eType))
        {
            return;
        }
        //Debug.LogError("removeAbility: " + eType);
        // 先取消激活
        activeAbility(eType);
        // 移除
        AbilitiesBase ab = mAbilities[eType];
        ab.CallRemove();
        mAbilities.Remove(eType);
    }

    void activeAbility(EAbilityType eType)
    {
        if (!mAbilities.ContainsKey(eType))
        {
            return;
        }
        if (mActiveAbilities.ContainsKey(eType))
        {
            return;
        }
        //Debug.LogError("activeAbility: " + eType);
        AbilitiesBase ab = mAbilities[eType];
        mActiveAbilities.Add(eType, ab);
        mActiveAbilitiesList.Add(ab);
        ab.CallActive();
        // 每次激活某个能力时，判断一下是否要取消激活其他能力
        checkPriority(eType);
    }

    void deactiveAbility(EAbilityType eType)
    {
        if (!mAbilities.ContainsKey(eType))
        {
            return;
        }
        if (!mActiveAbilities.ContainsKey(eType))
        {
            return;
        }
        //Debug.LogError("deactiveAbility: " + eType);
        AbilitiesBase ab = mActiveAbilities[eType];
        mActiveAbilities.Remove(eType);
        mActiveAbilitiesList.Remove(ab);
        ab.CallDeactive();
    }

    void checkPriority(EAbilityType eType)
    {
        if (!mAbilities.ContainsKey(eType))
        {
            return;
        }
        AbilitiesBase checkAb = mAbilities[eType];
        int checkPriority = checkAb.mConfig.priority;
        for (int i = mActiveAbilitiesList.Count - 1; i >= 0; i--)
        {
            var ab = mActiveAbilitiesList[i];
            if (ab.EType == eType)
            {
                continue;
            }
            // 优先级低的，要被取消激活
            if (ab.mConfig.priority < checkPriority)
            {
                deactiveAbility(ab.EType);
            }
        }
    }

    #endregion
}
