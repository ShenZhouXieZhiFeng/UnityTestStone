using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 掉落能力
/// </summary>
public class AbilityFall : AbilitiesBase
{
    float gravity = -9.8f;

    protected override void onAdd()
    {
        base.onAdd();
    }

    protected override void onUpdate()
    {
        base.onUpdate();
        mOwner.MoveComp.ApplyFall(gravity);
    }

}
