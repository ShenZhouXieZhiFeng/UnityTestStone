using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机类型基类
/// </summary>
public class CameraTypeBase
{
    protected ECameraType mCameraType;
    protected Camera mCamera;
    protected Transform mCameraTransform;
    protected CameraAgent mAgent;

    #region 由CameraManager管理调用

    public void Initialize(Camera camera, ECameraType cameraType)
    {
        mCameraType = cameraType;
        mCamera = camera;
        mCameraTransform = camera.transform;
    }

    public void AttachAgent(CameraAgent agent)
    {
        if (agent != null)
        {
            mAgent = agent;
            mAgent.OnAttach(this);
            onAgentAttach(agent);
        }
    }

    public void DeAttachAgent()
    {
        if (mAgent != null)
        {
            onAgentDeAttach();
            mAgent.OnDeAttach();
            mAgent = null;
        }
    }

    public void EnterType()
    {
        onEnter();
        onAddListener();
    }

    public void ExitType()
    {
        onRemoveListener();
        onExit();
    }

    public void CallUpdate()
    {
        onUpdate();
    }

    public void CallGUI()
    {
        onGUI();
    }

    public void CallLaterUpdate()
    {
        onLaterUpdate();
    }

    public void CallFixedUpdate()
    {
        onFixedUpdate();
    }

    #endregion

    #region 子类重写生命周期

    protected virtual void onEnter()
    {

    }

    protected virtual void onAgentAttach(CameraAgent agent)
    { 
    
    }

    protected virtual void onAgentDeAttach()
    { 
    
    }

    protected virtual void onAddListener()
    { 
    
    }

    protected virtual void onGUI()
    { 
    
    }

    protected virtual void onUpdate()
    {

    }

    protected virtual void onLaterUpdate()
    {

    }

    protected virtual void onFixedUpdate()
    {

    }

    protected virtual void onRemoveListener()
    { 
    
    }

    protected virtual void onExit()
    {
        
    }

    #endregion

    #region 工具方法

    /// <summary>
    /// 提供统一的deltaTime，用于后续可能的慢镜头效果
    /// </summary>
    protected float deltaTime
    {
        get 
        {
            return Time.deltaTime;
        }
    }

    /// <summary>
    /// 更新相机的FOV使用moveToward逼近目标FOV
    /// </summary>
    /// <param name="targetFOV"></param>
    /// <param name="delta"></param>
    protected void moveTowardFOV(float targetFOV, float delta)
    {
        if (targetFOV != mCamera.fieldOfView)
        {
            mCamera.fieldOfView = Mathf.MoveTowards(mCamera.fieldOfView, targetFOV, delta);
        }
    }

    #endregion

}
