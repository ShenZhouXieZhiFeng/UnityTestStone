using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特殊相机类型，用于在两个不同的相机类型之间进行切换
/// </summary>
public class CameraTypeTransition : CameraTypeBase
{
    private bool mInTransition = false;
    public bool InTransition
    {
        get { return mInTransition; }
    }

    public void StartTransition()
    { 
    
    }

    public void StopTransition()
    { 
    
    }
}
