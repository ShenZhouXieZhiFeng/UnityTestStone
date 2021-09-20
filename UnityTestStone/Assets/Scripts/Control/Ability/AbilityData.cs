using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色能力枚举
/// </summary>
public enum EAbilityType
{ 
    none = 0,
    move,
    jump,
    superJump,
    fall,
    useItem,
}

public class AbilityEvent
{
    #region 状态变更

    public static string ABEvent_GroundedChange = "ABEvent_GroundedChange";

    #endregion

    public static string ABEvent_Move = "ABEvent_Move";
    public static string ABEvent_Jump = "ABEvent_Jump";
    public static string ABEvent_UseItem = "ABEvent_UseItem";
}

public class AbilityHelper
{
    /// <summary>
    /// 能力配置数据
    /// </summary>
    static Dictionary<EAbilityType, AbilityConfig> abilityConfigs = new Dictionary<EAbilityType, AbilityConfig>()
    {
        {EAbilityType.move,         new AbilityConfig(EAbilityType.move, 10, true)},
        {EAbilityType.jump,         new AbilityConfig(EAbilityType.move, 10, false)},
        {EAbilityType.superJump,    new AbilityConfig(EAbilityType.move, 10, false)},
        {EAbilityType.fall,         new AbilityConfig(EAbilityType.move, 10, true)},
        {EAbilityType.useItem,      new AbilityConfig(EAbilityType.move, 9, false)},
    };

    public static AbilitiesBase GetNewAbility(EAbilityType eType)
    {
        AbilitiesBase res = null;
        switch (eType)
        {
            case EAbilityType.move:
                res = new AbilityMove();
                break;
            case EAbilityType.jump:
                res = new AbilityJump();
                break;
            case EAbilityType.superJump:
                res = new AbilitySuperJump();
                break;
            case EAbilityType.fall:
                res = new AbilityFall();
                break;
            case EAbilityType.useItem:
                res = new AbilityUseItem();
                break;
        }
        res.EType = eType;
        if (abilityConfigs.ContainsKey(eType))
        {
            res.mConfig = abilityConfigs[eType];
        }
        else
        {
            Debug.LogError("缺少能力配置:" + eType);
        }
        return res;
    }
}

/// <summary>
/// 能力配置数据
/// </summary>
public struct AbilityConfig
{
    /// <summary>
    ///  类型
    /// </summary>
    public EAbilityType eType;
    /// <summary>
    /// 优先级
    /// </summary>
    public int priority;
    /// <summary>
    /// 自动激活
    /// </summary>
    public bool autoActive;

    public AbilityConfig(EAbilityType eType, int priority, bool autoActive)
    {
        this.eType = eType;
        this.priority = priority;
        this.autoActive = autoActive;
    }
}

/// <summary>
/// 能力激活检测标志位
/// </summary>
public enum EAbilityFlag
{
    jump,
}
