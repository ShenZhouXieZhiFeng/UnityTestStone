using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 0.1f;

    CharacterController mCharacterController;
    TPSCameraAgent mCameraAgent;

    Vector3 moveVector = Vector3.zero;

    private void Awake()
    {
        mCharacterController = GetComponent<CharacterController>();
        mCameraAgent = GetComponent<TPSCameraAgent>();
    }

    void Start()
    {
        CameraManager.Instance.ChangeCameraType(ECameraType.TPS, mCameraAgent);
    }

    void Update()
    {
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
}
