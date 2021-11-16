using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void playAnimator(string animName)
    {
        //animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        animator.Play(animName);
        //TimerManager.instance.Once(5000, () =>
        //{
        //    animator.cullingMode = AnimatorCullingMode.CullCompletely;
        //});
    }

    private void OnGUI()
    {
        if (GUILayout.Button("anim1"))
        {
            playAnimator("MVP_WhoElse");
        }
        if (GUILayout.Button("anim2"))
        {
            playAnimator("MVP_Intricacy");
        }
        if (GUILayout.Button("anim3"))
        {
            playAnimator("MVP_Briefness01");
        }
    }
}
