using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三人称相机代理类
/// </summary>
public class TPSCameraAgent : CameraAgent
{
    CameraTypeTPS mTpsCamera;

    public Vector3 TpsCameraOffset = new Vector3(0, 2.6f, 6);
    public bool IsFreeView = true;

    [HideInInspector]
    public Vector3 RotateInputVector = Vector3.zero;

    public override void OnAttach(CameraTypeBase cameraType)
    {
        base.OnAttach(cameraType);
        mTpsCamera = cameraType as CameraTypeTPS;
    }

    public override void OnDeAttach()
    {
        base.OnDeAttach();

    }

    public void ApplyRotation()
    {
        if (!IsFreeView)
        {
            float yaw = RotateInputVector.x;
            Quaternion rotDelta = Quaternion.Euler(0, yaw, 0);
            Quaternion taretRotation = rotDelta * transform.rotation;
            transform.rotation = taretRotation;
        }

        mTpsCamera.ApplyRotation();
    }

    public void UpdateRotateInputVector(float x, float y)
    {
        RotateInputVector.x = x;
        RotateInputVector.y = y;
    }

    public void ClearRoteteInputVector()
    {
        RotateInputVector = Vector3.zero;
    }
}
