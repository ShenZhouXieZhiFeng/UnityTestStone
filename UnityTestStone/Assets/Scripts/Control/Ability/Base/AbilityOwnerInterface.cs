using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 能力框架拥有这必须实现的接口
/// </summary>
public interface AbilityOwnerInterface
{
    GameObject gameObject { get; }
    Transform transform { get; }

    MovementCompoent MoveComp { get; }

    AnimatorCompoent AnimatorComp { get; }
}
