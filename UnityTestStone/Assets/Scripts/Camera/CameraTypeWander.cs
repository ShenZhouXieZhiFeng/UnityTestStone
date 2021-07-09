using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 漫游式相机
/// </summary>
public class CameraTypeWander : CameraTypeBase
{
    float curPitch = 0;
    float curYaw = 0;

    float maxPitch = 80;
    float minPitch = -80;

    Vector3 moveVector = Vector3.zero;
    float moveSpeed = 0.1f;

    protected override void onEnter()
    {
        base.onEnter();

        Vector3 euler = mCameraTransform.localEulerAngles;
        curPitch = euler.x;
        curYaw = euler.y;
    }

    protected override void onUpdate()
    {
        base.onUpdate();

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Confined;
        }

        if (Cursor.visible)
        {
            return;
        }

        // 移动
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveVector = mCameraTransform.forward.normalized * vertical;
        moveVector += mCameraTransform.right.normalized * horizontal;
        if (Input.GetKey(KeyCode.Q))
        {
            moveVector += Vector3.up;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            moveVector += Vector3.down;
        }

        // 旋转
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        curPitch += mouseY;
        curYaw += mouseX;
        curPitch = Mathf.Clamp(curPitch, minPitch, maxPitch);
    }

    protected override void onLaterUpdate()
    {
        base.onLaterUpdate();

        // 应用位移
        mCameraTransform.position += (moveVector * moveSpeed);
        moveVector = Vector3.zero;

        // 应用旋转
        mCameraTransform.localEulerAngles = new Vector3(curPitch, curYaw, 0);
    }

    string guiLabel = "~键进入操控模式；WSAD上下左右；QE上升下降；";
    GUIStyle style = new GUIStyle();
    protected override void onGUI()
    {
        base.onGUI();

        style.fontSize = 30;

        GUI.Label(new Rect(100,50,60,100), guiLabel, style);
    }
}
