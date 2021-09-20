using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, AbilityOwnerInterface, CompoentOwnerInterface
{
    MeshRenderer[] renders;
    Animator mAnimator;
    CharacterController mCharacterController;

    TPSCameraAgent mCameraAgent;

    private void Awake()
    {
        mAnimator = transform.Find("avatar").GetComponent<Animator>();
        mCharacterController = transform.GetComponent<CharacterController>();
        mCameraAgent = GetComponent<TPSCameraAgent>();
        renders = GetComponentsInChildren<MeshRenderer>();
        // 初始化组件管理逻辑
        initComp();
        // 初始化能力
        initAbility();
    }

    void Start()
    {
        CameraManager.Instance.ChangeCameraType(ECameraType.TPS, mCameraAgent);
    }

    void Update()
    {
        testControl();
        mAbilityMgr.CallUpdate();
        callCompUpdate();
    }


    private void LateUpdate()
    {
        mCameraAgent?.ApplyRotation();
        mAbilityMgr.CallLateUpdate();
        callCompLateUpdate();
    }

    private void FixedUpdate()
    {
        mAbilityMgr.CallFixedUpdate();
        callCompFixedUpdate();
    }

    private void OnDestroy()
    {
        mAbilityMgr.CallDestory();
        callCompDestory();
    }

    #region 能力

    AbilityManager mAbilityMgr;

    public MovementCompoent MoveComp
    {
        get
        {
            return mMovementComp;
        }
    }

    public AnimatorCompoent AnimatorComp
    {
        get
        {
            return mAnimatorComp;
        }
    }

    void initAbility()
    {
        mAbilityMgr = new AbilityManager();
        mAbilityMgr.CallCreate(this);
        // 初始能力
        mAbilityMgr.AddAbility(EAbilityType.move);
        mAbilityMgr.AddAbility(EAbilityType.jump);
        mAbilityMgr.AddAbility(EAbilityType.superJump);
        mAbilityMgr.AddAbility(EAbilityType.fall);
        mAbilityMgr.AddAbility(EAbilityType.useItem);
    }

    #endregion

    #region 客户端组件

    MovementCompoent mMovementComp;
    AnimatorCompoent mAnimatorComp;

    List<CompoentBase> compoentList = new List<CompoentBase>();

    public Animator Aniamtor
    {
        get
        {
            return mAnimator;
        }
    }

    public CharacterController CharacterController
    {
        get
        {
            return mCharacterController;
        }
    }

    void initComp()
    {
        mMovementComp = AddCompoent<MovementCompoent>();
        mAnimatorComp = AddCompoent<AnimatorCompoent>();
    }

    T AddCompoent<T>() where T : CompoentBase, new()
    {
        T res = new T();
        res.CallStart(this);
        compoentList.Add(res);
        return res;
    }

    void callCompUpdate()
    {
        foreach (var comp in compoentList)
        {
            comp.CallUpdate();
        }
    }

    void callCompLateUpdate()
    {
        foreach (var comp in compoentList)
        {
            comp.CallLateUpdate();
        }
    }

    void callCompFixedUpdate()
    {
        foreach (var comp in compoentList)
        {
            comp.CallFixedUpdate();
        }
    }

    void callCompDestory()
    {
        foreach (var comp in compoentList)
        {
            comp.CallDestory();
        }
    }

    #endregion

    #region  render todo 抽离出render组件

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

    #endregion

    #region test

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

    void testControl()
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
        if (horizontal != 0 || vertical != 0)
        {
            EventCenter.Instance.fire(AbilityEvent.ABEvent_Move, new Vector3(horizontal, 0, vertical));
        }

        // 旋转
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        mCameraAgent?.UpdateRotateInputVector(mouseX, mouseY);

        // 跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventCenter.Instance.fire(AbilityEvent.ABEvent_Jump);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            EventCenter.Instance.fire(AbilityEvent.ABEvent_UseItem, 1001);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            EventCenter.Instance.fire(AbilityEvent.ABEvent_UseItem, 1001);
        }
    }

    #endregion

}
