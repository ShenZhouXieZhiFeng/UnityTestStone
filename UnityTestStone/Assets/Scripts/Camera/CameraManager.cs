using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    Camera mCamera;
    Transform mCameraTransform;
    
    ECameraType mCameraType = ECameraType.None;
    public ECameraType CameraType
    {
        get { return mCameraType; }
    }

    private CameraTypeBase mCameraTypeIns;
    private CameraTypeTransition mCameraTransition;

    #region 生命周期

    private void Awake()
    {
        Instance = this;

        mCameraTransform = transform.Find("Main Camera");
        mCamera = mCameraTransform.GetComponent<Camera>();

        createTransition();

        ChangeCameraType(ECameraType.Wander);
    }

    void createTransition()
    {
        mCameraTransition = new CameraTypeTransition();
        mCameraTransition.Initialize(mCamera, ECameraType.Transition);
    }

    private void Update()
    {
        if (mCameraTransition.InTransition)
        {
            mCameraTransition.CallUpdate();
            return;
        }
        if (mCameraTypeIns != null)
        {
            mCameraTypeIns.CallUpdate();
        }
    }

    private void LateUpdate()
    {
        if (mCameraTransition.InTransition)
        {
            mCameraTransition.CallLaterUpdate();
            return;
        }
        if (mCameraTypeIns != null)
        {
            mCameraTypeIns.CallLaterUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (mCameraTransition.InTransition)
        {
            mCameraTransition.CallFixedUpdate();
            return;
        }
        if (mCameraTypeIns != null)
        {
            mCameraTypeIns.CallFixedUpdate();
        }
    }

    private void OnGUI()
    {
        if (mCameraTransition.InTransition)
        {
            mCameraTransition.CallGUI();
            return;
        }
        if (mCameraTypeIns != null)
        {
            mCameraTypeIns.CallGUI();
        }
    }

    #endregion

    public void ChangeCameraType(ECameraType newType, CameraAgent agent = null)
    {
        if (mCameraType == newType)
        {
            return;
        }
        Debug.Log("ChangeCameraType: " + newType);

        var oldCameraIns = mCameraTypeIns;
        CameraTypeBase newCameraIns = null;
        mCameraType = newType;
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
            newCameraIns.Initialize(mCamera, newType);
            newCameraIns.AttachAgent(agent);
            newCameraIns.EnterType();
        }
        mCameraTypeIns = newCameraIns;
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
