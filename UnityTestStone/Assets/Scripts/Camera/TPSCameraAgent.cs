using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三人称相机代理类，玩家通过这个类和相机进行交互
/// </summary>
public class TPSCameraAgent : CameraAgent
{
    CameraTypeTPS mTpsCamera;

    public float Height = 2.6f;
    public Vector3 TpsCameraOffset = new Vector3(0, 0, 6);
    [HideInInspector]
    public Vector3 RotateInputVector = Vector3.zero;

    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        debug();
    }

    public override void OnAttach(CameraTypeBase cameraType)
    {
        base.OnAttach(cameraType);
        mTpsCamera = cameraType as CameraTypeTPS;
    }

    public override void OnDeAttach()
    {
        base.OnDeAttach();
        mTpsCamera = null;
    }

    /// <summary>
    /// 切换相机当前的模式
    /// </summary>
    /// <param name="newMold"></param>
    public void ChangeCameraMold(ETPSCameraMold newMold)
    {
        if (mTpsCamera == null)
        {
            return;
        }
        mTpsCamera.ChangeCameraMold(newMold);
    }

    public void ApplyRotation()
    {
        if (mTpsCamera == null)
        {
            return;
        }
        if (mTpsCamera.CameraMold == ETPSCameraMold.Adventure)
        {
            rotateAgent();
        }

        mTpsCamera.ApplyRotation();

        ClearRoteteInputVector();
    }

    void rotateAgent()
    {
        float yaw = RotateInputVector.x;
        Quaternion rotDelta = Quaternion.Euler(0, yaw, 0);
        Quaternion taretRotation = rotDelta * transform.rotation;
        transform.rotation = taretRotation;
    }

    /// <summary>
    /// 更新旋转输入变量
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void UpdateRotateInputVector(float x, float y)
    {
        RotateInputVector.x = x;
        RotateInputVector.y = y;
    }

    /// <summary>
    /// 清空旋转输入变量
    /// </summary>
    public void ClearRoteteInputVector()
    {
        RotateInputVector = Vector3.zero;
    }

    /// <summary>
    /// 更新玩家角色渲染状态
    /// alpha：0-1，0表示不可见，1表示完全可见
    /// </summary>
    public void UpdatePlayerRender(float alpha)
    {
        player.UpdateRender(alpha);
    }

    #region debug

    private void debug()
    {
        if (mTpsCamera == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (mTpsCamera.CameraMold == ETPSCameraMold.Adventure)
            {
                ChangeCameraMold(ETPSCameraMold.Freedom);
            }
            else
            {
                ChangeCameraMold(ETPSCameraMold.Adventure);
            }
        }
    }

    #endregion
}
