using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    Camera camera;
    Transform cameraTransform;
    
    public ECameraType curCameraType = ECameraType.None;
    private CameraTypeBase curCameraTypeIns;

    #region 生命周期

    private void Awake()
    {
        Instance = this;

        cameraTransform = transform.Find("Main Camera");
        camera = cameraTransform.GetComponent<Camera>();

        ChangeCameraType(ECameraType.Wander);
    }

    private void Update()
    {
        if (curCameraTypeIns != null)
        {
            curCameraTypeIns.CallUpdate();
        }
    }

    private void LateUpdate()
    {
        if (curCameraTypeIns != null)
        {
            curCameraTypeIns.CallLaterUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (curCameraTypeIns != null)
        {
            curCameraTypeIns.CallFixedUpdate();
        }
    }

    private void OnGUI()
    {
        if (curCameraTypeIns != null)
        {
            curCameraTypeIns.CallGUI();
        }
    }

    #endregion

    public void ChangeCameraType(ECameraType newType, CameraAgent agent = null)
    {
        if (curCameraType == newType)
        {
            return;
        }
        var oldCameraIns = curCameraTypeIns;
        CameraTypeBase newCameraIns = null;
        curCameraType = newType;
        switch (newType)
        {
            case ECameraType.Fixed:
                newCameraIns = new CameraTypeFixed();
                break;
            case ECameraType.Wander:
                newCameraIns = new CameraTypeWander();
                break;
            case ECameraType.TPS:
                newCameraIns = new CameraTypeTPS();
                break;
        }
        // todo 是否要有切换过程? 
        // todo 要有
        if (oldCameraIns != null)
        {
            oldCameraIns.DeAttachAgent();
            oldCameraIns.ExitType();
        }
        if (newCameraIns != null)
        {
            newCameraIns.Initialize(camera, newType);
            newCameraIns.AttachAgent(agent);
            newCameraIns.EnterType();
        }
        curCameraTypeIns = newCameraIns;
    }

    #region debug

    [ContextMenu("切换到TPS相机")]
    void changeToTPS()
    {
        GameObject player = GameObject.Find("Player");
        TPSCameraAgent agent = player.GetComponent<TPSCameraAgent>();
        ChangeCameraType(ECameraType.TPS, agent);
    }

    [ContextMenu("切换到漫游相机")]
    void changeToWander()
    {
        ChangeCameraType(ECameraType.Wander);
    }

    [ContextMenu("切换到固定相机")]
    void changeToFixed()
    {
        ChangeCameraType(ECameraType.Fixed);
    }

    #endregion
}
