using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 固定式相机，负责把相机各属性都重置到初始值
/// </summary>
public class CameraTypeFixed : CameraTypeBase
{
    protected float defaultFov = 60;

    protected override void onEnter()
    {
        base.onEnter();
    }

    protected override void onExit()
    {
        base.onExit();
    }

    protected override void onAddListener()
    {
        base.onAddListener();
    }

    protected override void onRemoveListener()
    {
        base.onRemoveListener();
    }

    protected override void onUpdate()
    {
        base.onUpdate();

        moveTowardFOV(defaultFov, deltaTime);
    }

    protected override void onLaterUpdate()
    {
        base.onLaterUpdate();
    }

    protected override void onFixedUpdate()
    {
        base.onFixedUpdate();
    }

}
