using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void CallBack();
public delegate void CallBack<T>(T t);
public delegate void CallBack<T, T1>(T t, T1 t1);
public delegate void CallBack<T, T1, T2>(T t, T1 t1, T2 t2);
public delegate void CallBack<T, T1, T2, T3>(T t, T1 t1, T2 t2, T3 t3);

public delegate R CallBackR<R>();
public delegate R CallBackR<R, T>(T t);
public delegate R CallBackR<R, T, T1>(T t, T1 t1);
public delegate R CallBackR<R, T, T1, T2>(T t, T1 t1, T2 t2);
public delegate R CallBackR<R, T, T1, T2, T3>(T t, T1 t1, T2 t2, T3 t3);

public static class MathUtils
{
    public static bool SimilarVector(Vector3 a, Vector3 b, float deviation = 0.001f)
    {
        return Mathf.Abs(a.x - b.x) <= deviation &&
            Mathf.Abs(a.y - b.y) <= deviation &&
            Mathf.Abs(a.z - b.z) <= deviation;
    }
}
