using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T mInstance = null;
    public static T instance
    {
        get
        {
            if (mInstance == null)
            {
                Type type = typeof(T);
                mInstance = (T)FindObjectOfType(type);

                if (mInstance == null)
                {
                    //Debug.LogWarning("An instance of " + typeof(T) + " is needed in the scene, but there is none.");

                    GameObject go = new GameObject(type.ToString());
                    DontDestroyOnLoad(go);
                    mInstance = go.AddComponent<T>();
                }
            }

            return mInstance;
        }
    }
}