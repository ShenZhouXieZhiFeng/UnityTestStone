using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 0.1f;
    [SerializeField]
    float jumpSpeed = 5f;

    CharacterController mCharacterController;
    TPSCameraAgent mCameraAgent;

    Vector3 moveVector = Vector3.zero;

    MeshRenderer[] renders;

    private void Awake()
    {
        mCharacterController = GetComponent<CharacterController>();
        mCameraAgent = GetComponent<TPSCameraAgent>();

        renders = GetComponentsInChildren<MeshRenderer>();
    }

    void Start()
    {
        CameraManager.Instance.ChangeCameraType(ECameraType.TPS, mCameraAgent);
    }

    void Update()
    {
        if (CameraManager.Instance.CameraType != ECameraType.TPS)
        {
            return;
        }

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
        moveVector = transform.forward.normalized * vertical;
        moveVector += transform.right.normalized * horizontal;
        moveVector *= moveSpeed;

        // 旋转
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        mCameraAgent.UpdateRotateInputVector(mouseX, mouseY);
    }

    string guiLabel = "~键进入操控模式；WSAD前后左右";
    GUIStyle style = new GUIStyle();
    void OnGUI()
    {
        if (CameraManager.Instance.CameraType != ECameraType.TPS)
        {
            return;
        }

        style.fontSize = 30;

        GUI.Label(new Rect(100, 50, 60, 100), guiLabel, style);
    }

    void LateUpdate()
    {
        applyMove();
        applyRotation();
    }

    void applyMove()
    {
        Vector3 finalMove = moveVector;
        if (finalMove != Vector3.zero)
        {
            mCharacterController.Move(finalMove);
        }
        moveVector = Vector3.zero;
    }

    void applyRotation()
    {
        mCameraAgent.ApplyRotation();
    }

    float alphaCache = 1;
    MaterialPropertyBlock block = null;
    public void UpdateRender(float alpha)
    {
        if (alphaCache == alpha)
        {
            return;
        }
        alphaCache = alpha;
        if (alpha <= 0)
        {
            foreachRenders((MeshRenderer render) =>
            {
                render.enabled = false;
            });
        }
        else if (alpha >= 1)
        {
            foreachRenders((MeshRenderer render) =>
            {
                render.enabled = true;
                setRenderAlpha(render, 1);
            });
        }
        else
        {
            foreachRenders((MeshRenderer render) =>
            {
                render.enabled = true;
                setRenderAlpha(render, alpha);
            });
        }
    }

    void setRenderAlpha(MeshRenderer render, float alpha)
    {
        Color temp = render.material.color;
        if (temp.a != alpha)
        {
            temp.a = alpha;
            render.material.color = temp;
        }
    }

    void foreachRenders(CallBack<MeshRenderer> callback)
    {
        foreach (var render in renders)
        {
            callback(render);
        }
    }
}
