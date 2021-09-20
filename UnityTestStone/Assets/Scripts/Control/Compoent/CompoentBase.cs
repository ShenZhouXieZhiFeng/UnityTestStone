using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基础组件封装
/// </summary>
public abstract class CompoentBase
{
    #region 数据

    protected CompoentOwnerInterface mOwner;

    #endregion

    #region 外部调用

    public void CallStart(CompoentOwnerInterface owner)
    {
        mOwner = owner;
        onStart();
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

    public void CallDestory()
    {
        onDestory();
    }

    #endregion

    #region 子类重写生命周期

    protected virtual void onStart() { }

    protected virtual void onDestory() { }

    protected virtual void onUpdate() { }

    protected virtual void onFixedUpdate() { }

    protected virtual void onLateUpdate() { }

    #endregion
}
