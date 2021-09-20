using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TimerItemModel
{
    public int delay;
    public int during;
    public bool isFrame;
    public bool isLoop;
    public CallBack callBack;
    public bool removeFlag;
}

/// <summary>
/// 计时器管理类
/// </summary>
public class TimerManager : MonoSingleton<TimerManager>
{
    List<TimerItemModel> timerList = new List<TimerItemModel>();

    void Update()
    {
        //每帧驱动
        STimer.UpdateAllTimer();

        int count = timerList.Count;
        if (count <= 0)
        {
            return;
        }
        for (int i = count - 1; i >= 0; i--)
        {
            TimerItemModel tData = timerList[i];
            if (tData.removeFlag)
            {
                timerList.Remove(tData);
            }
        }
        for (int i = timerList.Count - 1; i >= 0; i--)
        {
            var tData = timerList[i];
            if (tData.callBack == null || tData.removeFlag)
            {
                tData.removeFlag = true;
                continue;
            }
            if (tData.isFrame)
            {
                tData.during += 1;
                if (tData.isLoop)
                {
                    if (tData.during >= tData.delay)
                    {
                        tData.during = tData.during - tData.delay;
                        tData.callBack();
                    }
                }
                else
                {
                    if (tData.during >= tData.delay)
                    {
                        tData.removeFlag = true;
                        tData.callBack();
                    }
                }
            }
            else
            {
                int dt = (int)(Time.deltaTime * 1000);
                tData.during += dt;
                if (tData.isLoop)
                {
                    if (tData.during  >= tData.delay)
                    {
                        tData.during = tData.during - tData.delay;
                        tData.callBack();
                    }
                }
                else
                {
                    if (tData.during >= tData.delay)
                    {
                        tData.removeFlag = true;
                        tData.callBack();
                    }
                }
            }
        }
    }

    /// <summary>
    /// delay毫秒之后执行一次CallBack，不带参数
    /// </summary>
    public void Once(int delay,CallBack callBack, bool coverBefore = true)
    {
        addTime(delay, false, false, callBack, coverBefore);
    }

    /// <summary>
    /// 每n毫秒执行一次
    /// </summary>
    public void Loop(int delay, CallBack callBack, bool coverBefore = true)
    {
        addTime(delay, false, true, callBack, coverBefore);
    }

    /// <summary>
    /// n帧之后执行一次
    /// </summary>
    public void FrameOnce(int delay, CallBack callBack, bool coverBefore = true)
    {
        addTime(delay, true, false, callBack, coverBefore);
    }

    /// <summary>
    /// 每n帧执行一次
    /// </summary>
    public void FrameLoop(int delay, CallBack callBack, bool coverBefore = true)
    {
        addTime(delay, true, true, callBack, coverBefore);
    }

    /// <summary>
    /// 清除计时器，计时器一定要清理，非循环的计时器也要注意
    /// </summary>
    public void ClearTimer(CallBack callBack)
    {
        foreach (TimerItemModel tData in timerList)
        {
            if (tData.callBack == callBack)
            {
                tData.removeFlag = true;
            }
        }
    }

    /// <summary>
    /// 消除所有的计时器，除了管理类，都不准调用
    /// </summary>
    //public void ClearAllTimer()
    //{
    //    Log.log("清除所有定时器");
    //    //timerList.Clear();
    //    foreach (TimerItemModel tData in timerList)
    //    {
    //        tData.removeFlag = true;
    //    }
    //}

    void addTime(int delay, bool isFrame, bool isLoop, CallBack callBack, bool coverBefore)
    {
        if (callBack == null)
        {
            return;
        }
        if (coverBefore)
        {
            foreach (TimerItemModel tData in timerList)
            {
                if (tData.callBack == callBack)
                {
                    tData.removeFlag = true;
                }
            }
        }
        TimerItemModel item = new TimerItemModel();
        item.delay = delay;
        item.isFrame = isFrame;
        item.isLoop = isLoop;
        item.callBack = callBack;
        item.during = 0;
        item.removeFlag = false;
        timerList.Add(item);
    }

    TimerItemModel findTimeItem(CallBack callBack)
    {
        foreach (TimerItemModel tData in timerList)
        {
            return tData;
        }
        return null;
    }

}



public delegate void TimerUpdateHandler<T>(T obj);
public delegate void TimerEndHandler();
public class STimer
{
    /// <summary>
    /// 所有的计时器
    /// </summary>
    private static List<STimer> MyTimers = new List<STimer>();

    /// <summary>
    /// 依赖的mono
    /// </summary>
    private MonoBehaviour attachMono;

    /// <summary>
    /// 是否依赖mono
    /// </summary>
    public bool hasAttachMono = false;

    private TimerUpdateHandler<float> UpdateEvent;

    private TimerEndHandler CompleteEvent;

    private float _time;

    private bool _loop;

    /// <summary>
    /// 计时器标志
    /// </summary>
    private string _flag;

    public static TimerManager driver = null;

    private float CurrentTime { get { return Time.time; } }

    /// <summary>
    /// 缓存时间,暂停使用
    /// </summary>
    private float cachedTime;

    float timePassed;

    private bool _isFinish = false;

    private bool _isPause = false;


    /// <summary>
    /// 当前计时器时间
    /// </summary>
    public float Duration { get { return _time; } }

    public bool IsPause
    {
        get { return _isPause; }
        set
        {
            if (value)
            {
                Pause();
            }
            else
            {
                Resum();
            }
        }

    }

    private STimer(float time, string flag, bool loop = false)
    {
        if (null == driver) driver = TimerManager.instance;
        _time = time * 0.001f;
        _loop = loop;

        cachedTime = CurrentTime;
        if (MyTimers.Exists((v) => { return flag != "" && v._flag == flag; }))
        {
            //Log.logWarning(string.Format("存在相同计时器：{0}", flag));
        }
        _flag = flag;
    }

    /// <summary>  
    /// 暂停
    /// </summary>  
    private void Pause()
    {
        if (_isFinish)
        {
            Log.logWarning("计时已经结束！");
        }
        else
        {
            _isPause = true;
        }
    }
    /// <summary>  
    /// 继续 
    /// </summary>  
    private void Resum()
    {
        if (_isFinish)
        {
            Log.logWarning("计时已经结束！");
        }
        else
        {
            if (_isPause)
            {
                cachedTime = CurrentTime - timePassed;
                _isPause = false;
            }
            else
            {
                Log.logWarning("当前计时非暂停状态！");
            }
        }
    }

    /// <summary>
    /// 刷新
    /// </summary>
    private void Update()
    {
        if (!_isFinish && !_isPause)
        {
            timePassed = CurrentTime - cachedTime;

            if (null != UpdateEvent)
            {
                if (hasAttachMono)
                {
                    if (attachMono != null)
                        UpdateEvent.Invoke(Mathf.Clamp01(timePassed / _time));
                    else
                        DeleteTimer(this);
                }
                else
                    UpdateEvent.Invoke(Mathf.Clamp01(timePassed / _time));

            }
            if (timePassed >= _time)
            {
                if (null != CompleteEvent)
                {
                    if(hasAttachMono)
                    {
                        if(attachMono != null)
                            CompleteEvent.Invoke();
                        else
                            DeleteTimer(this);
                    }
                    else
                        CompleteEvent.Invoke();
                }
                if (_loop)
                {
                    cachedTime = CurrentTime;
                }
                else
                {
                    Stop();
                }
            }
        }
    }

    /// <summary>
    /// 停止并初始化
    /// </summary>
    private void Stop()
    {
        if (MyTimers.Contains(this))
        {
            MyTimers.Remove(this);
        }
        _time = -1;
        _isFinish = true;
        _isPause = false;
        UpdateEvent = null;
        CompleteEvent = null;
        DetachMono();
    }


    /// <summary>
    /// 添加计时器
    /// </summary>
    /// <param name="time"></param>
    /// <param name="flag"></param>
    /// <param name="loop"></param>
    /// <returns></returns>
    public static STimer AddTimer(float time, string flag = "", bool loop = false)
    {
        STimer timer = new STimer(time, flag, loop);
        MyTimers.Add(timer);
        return timer;
    }

    /// <summary>
    /// 驱动计时器
    /// </summary>
    public static void UpdateAllTimer()
    {
        for (int i = 0; i < MyTimers.Count; i++)
        {
            if (null != MyTimers[i])
            {
                MyTimers[i].Update();
            }
        }
    }

    /// <summary>
    /// 是否存在计时器
    /// </summary>
    /// <param name="flag"></param>
    public static bool Exist(string flag)
    {
        return MyTimers.Exists((v) => { return v._flag == flag; });
    }
    /// <summary>
    /// 是否存在计时器
    /// </summary>
    /// <param name="flag"></param>
    public static bool Exist(STimer timer)
    {
        return MyTimers.Contains(timer);
    }


    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="flag"></param>
    public static STimer GetTimer(string flag)
    {
        return MyTimers.Find((v) => { return v._flag == flag; });
    }


    /// <summary>
    /// 暂停
    /// </summary>
    /// <param name="flag"></param>
    public static void Pause(string flag)
    {
        STimer timer = GetTimer(flag);
        if (null != timer)
        {
            timer.Pause();
        }
        else
        {
            Log.log("检查此计时器：{0}是否存在！或者已经完成计时!" + flag);
        }
    }
    /// <summary>
    /// 暂停
    /// </summary>
    /// <param name="timer"></param>
    public static void Pause(STimer timer)
    {
        if (Exist(timer))
        {
            timer.Pause();
        }
        else
        {
            Log.log("检查此计时器是否存在！或者已经完成计时!");
        }
    }
    /// <summary>
    /// 恢复
    /// </summary>
    /// <param name="flag"></param>
    public static void Resum(string flag)
    {
        STimer timer = GetTimer(flag);
        if (null != timer)
        {
            timer.Resum();
        }
        else
        {
            Log.log("检查此计时器：{0}是否存在！或者已经完成计时!", flag);
        }
    }
    /// <summary>
    /// 恢复
    /// </summary>
    /// <param name="timer"></param>
    public static void Resum(STimer timer)
    {
        if (Exist(timer))
        {
            timer.Resum();
        }
        else
        {
            Log.log("检查此计时器是否存在！或者已经完成计时!");
        }
    }


    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="flag"></param>
    public static void DelTimer(string flag)
    {
        STimer timer = GetTimer(flag);
        if (null != timer)
        {
            timer.Stop();
        }
        else
        {
            //Log.log("检查此计时器是否存在！或者已经完成计时!");
        }
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="flag"></param>
    public static void DeleteTimer(STimer timer)
    {
        if (Exist(timer))
        {
            timer.Stop();
        }
        else
        {
            //Log.log("检查此计时器是否存在！或者已经完成计时!");
        }
    }

    public static void TryDeleteTimer(STimer timer)
    {
        if(timer != null)
        {
            DeleteTimer(timer);
            timer = null;
        }
    }

    public static void TryDeleteTimer(string timer)
    {
        DelTimer(timer);
    }


    /// <summary>
    /// 删除所有计时器
    /// </summary>
    public static void RemoveAll()
    {
        MyTimers.ForEach((v) => { v.Stop(); });
        MyTimers.Clear();
    }

    /// <summary>
    /// 绑定Mono，随Mono生命周期销毁
    /// </summary>
    /// <param name="mono"></param>
    public void AttachMono(MonoBehaviour mono)
    {
        if(mono != null)
        {
            attachMono = mono;
            hasAttachMono = true;
        }
    }

    public void DetachMono()
    {
        if(attachMono != null)
        {
            attachMono = null;
            hasAttachMono = false;
        }
    }

    /// <summary>
    /// 添加结束事件
    /// </summary>
    /// <param name="completedEvent"></param>
    public void AddEvent(TimerEndHandler completedEvent)
    {
        if (null == CompleteEvent)
        {
            CompleteEvent = completedEvent;
        }
        else
        {
            //防止多次注册同一事件
            Delegate[] delegates = CompleteEvent.GetInvocationList();
            if (!Array.Exists(delegates, (v) => { return v == (Delegate)completedEvent; }))
            {
                CompleteEvent += completedEvent;
            }
        }
    }
    public void AddEvent(TimerUpdateHandler<float> updateEvent)
    {
        if (null == UpdateEvent)
        {
            UpdateEvent = updateEvent;
        }
        else
        {
            Delegate[] delegates = UpdateEvent.GetInvocationList();

            if (!Array.Exists(delegates, (v) => { return v == (Delegate)updateEvent; }))
            {
                UpdateEvent += updateEvent;
            }
        }
    }

}



public static class TimerExtension
{
    /// <summary>
    /// 结束事件 
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="completedEvent"></param>
    /// <returns></returns>
    public static STimer OnComplete(this STimer timer, TimerEndHandler completedEvent)
    {
        if (null == timer)
        {
            return null;
        }
        timer.AddEvent(completedEvent);
        return timer;
    }
    /// <summary>
    /// 帧事件
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="updateEvent">形参为计时器进度:0到1</param>
    /// <returns></returns>
    public static STimer OnUpdate(this STimer timer, TimerUpdateHandler<float> updateEvent)
    {
        if (null == timer)
        {
            return null;
        }
        timer.AddEvent(updateEvent);
        return timer;
    }
}