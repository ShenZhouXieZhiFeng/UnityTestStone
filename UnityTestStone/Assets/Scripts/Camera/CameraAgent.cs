using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 如果相机是要附着到某个物体身上，则需要在切换相机类型时，传入这个对象
/// </summary>
public class CameraAgent : MonoBehaviour
{
    protected CameraTypeBase mCameraType;

    public virtual void OnAttach(CameraTypeBase cameraType)
    {
        mCameraType = cameraType;
    }

    public virtual void OnDeAttach()
    {
        mCameraType = null;
    }

}
