using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三人称相机类型
/// </summary>
public enum ETPSCameraMold
{ 
    None,
    // 冒险模式
    Adventure,
    // 自由模式
    Freedom
}

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

    float freeTransitionVal = 5f;

    float mCameraCheckRadius = 0.2f;
    int mCameraCheckLayer = 0;

    Quaternion mCameraRotation;
    Vector3 mFreeEulerCache;

    protected override void onCreate()
    {
        base.onCreate();

        mCameraCheckLayer = 1 << 0;
    }

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

    #region 相机视角处理

    // 正在过渡中的模式
    private ETPSCameraMold inTransitionMold = ETPSCameraMold.None;

    public ETPSCameraMold CameraMold
    {
        get
        {
            return mCameraMold;
        }
    }
    private ETPSCameraMold mCameraMold = ETPSCameraMold.Adventure;

    public void ChangeCameraMold
       (ETPSCameraMold newMold)
    {
        // 正在切换中，不处理
        if (isInTransition())
        {
            return;
        }
        if (mCameraMold == newMold)
        {
            return;
        }
        Debug.Log("切换相机模式:" + newMold);
        inTransitionMold = newMold;
        if (newMold == ETPSCameraMold.Freedom)
        {
            mFreeEulerCache = mCameraTransform.eulerAngles;
            curFreeViewPitch = 0;
            curFieeViewYaw = 0;
            // 切换到freedom是瞬间切换完成的
            onCameraMoldChangeSuccess();
        }
        else if (newMold == ETPSCameraMold.Adventure)
        {
            // 等待切换完成
        }
    }

    void onCameraMoldChangeSuccess()
    {
        if (inTransitionMold == ETPSCameraMold.None)
        {
            return;
        }
        Debug.Log("相机模式切换完成:" + inTransitionMold);
        mCameraMold = inTransitionMold;
        inTransitionMold = ETPSCameraMold.None;
    }

    bool isInTransition()
    {
        return inTransitionMold != ETPSCameraMold.None;
    }

    bool checkIsFreedomMold()
    {
        if (mCameraMold == ETPSCameraMold.Freedom)
        {
            return true;
        }
        return false;
    }

    #endregion


    #region 相机位置旋转更新操作


    public void ApplyRotation()
    {
        if (tpsAgent == null)
        {
            return;
        }

        // 刷新过渡
        updateForTransition();
        // 刷新旋转
        updateCameraRotation();
        // 刷新位置
        updateCameraPostion();
        // 刷新角色渲染状态
        updateAgentRender();
    }

    void updateForTransition()
    {
        if (inTransitionMold == ETPSCameraMold.None)
        {
            return;
        }
        if (mCameraMold == ETPSCameraMold.Freedom)
        {
            // 由自由模式切换到其他模式
            curFreeViewPitch = Mathf.MoveTowards(curFreeViewPitch, 0, freeTransitionVal);
            curFieeViewYaw = Mathf.MoveTowards(curFieeViewYaw, 0, freeTransitionVal);
            if (curFreeViewPitch == 0 && curFieeViewYaw == 0)
            {
                onCameraMoldChangeSuccess();
            }
        }
    }

    void updateCameraPostion()
    {
        Vector3 right = agentTransform.right;
        Vector3 forward = mCameraTransform.forward;
        Vector3 up = Vector3.up;
        Vector3 agentPos = agentTransform.position + new Vector3(0, tpsAgent.Height, 0);
        Vector3 tpsOffset = tpsAgent.TpsCameraOffset;

        Vector3 wantedPos = agentPos + -forward * tpsOffset.z + right * tpsOffset.x + up * tpsOffset.y;

        var dir = wantedPos - agentPos;
        if (dir.sqrMagnitude > 0.0001f)
        {
            Debug.DrawLine(agentPos, mCameraTransform.position, Color.red);
            if (Physics.SphereCast(agentPos, mCameraCheckRadius, dir, out RaycastHit sphereHitInfo, dir.magnitude, mCameraCheckLayer))
            {
                Vector3 d1 = sphereHitInfo.point - agentPos;
                float endDist = Vector3.Dot(dir, d1) / dir.magnitude;
                float vdist = (agentPos + dir.normalized * endDist - sphereHitInfo.point).magnitude;
                float dist = endDist - Mathf.Sqrt(Mathf.Max(0.000001f, mCameraCheckRadius * mCameraCheckRadius - vdist * vdist));
                wantedPos = agentPos + dir.normalized * dist;
            }
        }

        mCameraTransform.position = wantedPos;
    }

    void updateCameraRotation()
    {
        Vector3 rotateVector = tpsAgent.RotateInputVector;

        if (checkIsFreedomMold())
        {
            if (!isInTransition())
            {
                curFreeViewPitch += rotateVector.y;
                curFieeViewYaw += rotateVector.x;
            }
            curFreeViewPitch = processAngle(curFreeViewPitch);
            curFieeViewYaw = processAngle(curFieeViewYaw);
            float cachePitch = Mathf.Abs(mFreeEulerCache.x);
            curFreeViewPitch = Mathf.Clamp(curFreeViewPitch, minPitch - cachePitch, maxPitch - cachePitch);

            Vector3 euler = mFreeEulerCache + new Vector3(curFreeViewPitch, curFieeViewYaw, 0);
            Quaternion rot = Quaternion.Euler(euler);
            mCameraRotation = rot;
        }
        else
        {
            curPitch += rotateVector.y;
            curYaw += rotateVector.x;
            curPitch = processAngle(curPitch);
            curYaw = processAngle(curYaw);
            curPitch = Mathf.Clamp(curPitch, minPitch, maxPitch);

            Quaternion inputRot = Quaternion.Euler(curPitch, curYaw, 0);
            mCameraRotation = inputRot;
        }

        mCameraTransform.rotation = mCameraRotation;
    }

    float processAngle(float angle)
    {
        angle %= 360;
        return angle;
    }

    #endregion

    #region 角色透明处理

    float disFadeout = 0.5f;
    float alphaDisMax = 4f;
    void updateAgentRender()
    {
        Vector3 headPos = tpsAgent.transform.position + new Vector3(0, tpsAgent.Height, 0);
        float camAgentDis = Vector3.Distance(headPos, mCameraTransform.position);

        if (camAgentDis < disFadeout)
        {
            tpsAgent.UpdatePlayerRender(0);
        }
        else
        {
            float alpha = (camAgentDis - disFadeout) / (alphaDisMax - disFadeout);
            alpha = Mathf.Min(alpha, 1);
            tpsAgent.UpdatePlayerRender(alpha);
        }
    }

    #endregion

}
