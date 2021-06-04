using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三人称式相机
/// </summary>
public class CameraTypeTPS : CameraTypeBase
{
    protected TPSCameraAgent tpsAgent;
    protected Transform agentTransform;

    protected float curPitch = 0;
    protected float curYaw = 0;

    protected float curFreeViewPitch = 0;
    protected float curFieeViewYaw = 0;

    float maxPitch = 80;
    float minPitch = -80;

    protected override void onAgentAttach(CameraAgent agent)
    {
        base.onAgentAttach(agent);
        if (agent is TPSCameraAgent)
        {
            tpsAgent = agent as TPSCameraAgent;
            agentTransform = tpsAgent.transform;
        }
        else
        {
            Debug.LogError("Agent代理类型传入错误");
        }
    }

    protected override void onAgentDeAttach()
    {
        base.onAgentDeAttach();

    }

    //protected override void onLaterUpdate()
    //{
    //    base.onLaterUpdate();
    //    if (tpsAgent == null)
    //    {
    //        return;
    //    }
    //    // 刷新第三人称相机位置`
    //    updateCameraPostion();
    //    // 刷新第三人称相机旋转
    //    updateCameraRotation();
    //}

    public void ApplyRotation()
    {
        if (tpsAgent == null)
        {
            return;
        }
        if (tpsAgent.IsFreeView)
        {
            // 刷新第三人称相机位置
            updateCameraPostion();
            // 刷新第三人称相机旋转
            updateCameraRotation();
        }
        else
        {
            // 刷新第三人称相机旋转
            updateCameraRotation();
            // 刷新第三人称相机位置
            updateCameraPostion();
        }
    }

    void updateCameraPostion()
    {
        Vector3 right = agentTransform.right;
        Vector3 forward = mCameraTransform.forward;
        Vector3 up = Vector3.up;
        Vector3 agentPos = agentTransform.position;
        Vector3 tpsOffset = tpsAgent.TpsCameraOffset;

        if (tpsAgent.IsFreeView)
        {
            //forward
            forward = agentTransform.forward;
            Quaternion qua = Quaternion.Euler(curFreeViewPitch, curFieeViewYaw, 0);
            forward = qua * forward;
        }

        Vector3 wantedPos = agentPos + -forward * tpsOffset.z + right * tpsOffset.x + up * tpsOffset.y;
        mCameraTransform.position = wantedPos;
    }

    Quaternion cameraRotation;
    void updateCameraRotation()
    {
        Vector3 rotateVector = tpsAgent.RotateInputVector;
        if (tpsAgent.IsFreeView)
        {
            curFreeViewPitch += rotateVector.y;
            curFieeViewYaw += rotateVector.x;
            curFreeViewPitch = Mathf.Clamp(curFreeViewPitch, minPitch, maxPitch);
            Vector3 lookForward = tpsAgent.transform.position - mCameraTransform.position;
            cameraRotation = Quaternion.LookRotation(lookForward, Vector3.up);
        }
        else
        {
            curPitch += rotateVector.y;
            curYaw += rotateVector.x;
            curPitch = Mathf.Clamp(curPitch, minPitch, maxPitch);
            Quaternion inputRot = Quaternion.Euler(curPitch, curYaw, 0);
            cameraRotation = inputRot;
        }
        mCameraTransform.rotation = cameraRotation;

        tpsAgent.ClearRoteteInputVector();
    }
}
