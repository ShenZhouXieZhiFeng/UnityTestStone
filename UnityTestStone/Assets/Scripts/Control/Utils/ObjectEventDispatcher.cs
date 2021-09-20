using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEventDispatcher : MonoBehaviour
{
    private EventController _eventController = new EventController();

    #region 增加监听器

    /// <summary>
    /// 增加监听器，不带参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">不带参数事件回调</param>
    public void register(string eventType, EventCallBack handler)
    {
        _eventController.addEventListener(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，1个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void register<T>(string eventType, EventCallBack<T> handler)
    {
        _eventController.addEventListener(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，2个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void register<T, U>(string eventType, EventCallBack<T, U> handler)
    {
        _eventController.addEventListener(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，3个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <typeparam name="V">回调中第3个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void register<T, U, V>(string eventType, EventCallBack<T, U, V> handler)
    {
        _eventController.addEventListener(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，4个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <typeparam name="V">回调中第3个参数类型</typeparam>
    /// <typeparam name="W">回调中第4个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void register<T, U, V, W>(string eventType, EventCallBack<T, U, V, W> handler)
    {
        _eventController.addEventListener(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，1个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带1个object参数的回调</param>
    public void register(string eventType, EventCallBack<object> handler)
    {
        _eventController.addEventListener(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，2个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带2个object参数的回调</param>
    public void register(string eventType, EventCallBack<object, object> handler)
    {
        _eventController.addEventListener(eventType, handler);
    }
    #endregion

    #region 移除监听器
    /// <summary>
    /// 移除监听器，不带参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">不带参数回调</param>
    public void unRegister(string eventType, EventCallBack handler)
    {
        _eventController.removeEventListener(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，1个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void unRegister<T>(string eventType, EventCallBack<T> handler)
    {
        _eventController.removeEventListener(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，2个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void unRegister<T, U>(string eventType, EventCallBack<T, U> handler)
    {
        _eventController.removeEventListener(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，3个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <typeparam name="V">回调中第3个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void unRegister<T, U, V>(string eventType, EventCallBack<T, U, V> handler)
    {
        _eventController.removeEventListener(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，4个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <typeparam name="V">回调中第3个参数类型</typeparam>
    /// <typeparam name="W">回调中第4个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void unRegister<T, U, V, W>(string eventType, EventCallBack<T, U, V, W> handler)
    {
        _eventController.removeEventListener(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，1个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带1个object参数的回调</param>
    public void unRegister(string eventType, EventCallBack<object> handler)
    {
        _eventController.removeEventListener(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，2个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带2个object参数的回调</param>
    public void unRegister(string eventType, EventCallBack<object, object> handler)
    {
        _eventController.removeEventListener(eventType, handler);
    }
    #endregion

    #region 触发事件

    /// <summary>
    /// 触发事件，不带参数触发
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void fire(string eventType)
    {
        _eventController.triggerEvent(eventType);
    }

    /// <summary>
    /// 触发事件，带1个参数触发
    /// </summary>
    /// <typeparam name="T">第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void fire<T>(string eventType, T arg1)
    {
        _eventController.triggerEvent(eventType, arg1);
    }

    /// <summary>
    /// 触发事件，带2个参数触发
    /// </summary>
    /// <typeparam name="T">第1个参数类型</typeparam>
    /// <typeparam name="U">第2个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="arg2">第2个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void fire<T, U>(string eventType, T arg1, U arg2)
    {
        _eventController.triggerEvent(eventType, arg1, arg2);
    }

    /// <summary>
    /// 触发事件，带3个参数触发
    /// </summary>
    /// <typeparam name="T">第1个参数类型</typeparam>
    /// <typeparam name="U">第2个参数类型</typeparam>
    /// <typeparam name="V">第3个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="arg2">第2个参数</param>
    /// <param name="arg3">第3个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void fire<T, U, V>(string eventType, T arg1, U arg2, V arg3)
    {
        _eventController.triggerEvent(eventType, arg1, arg2, arg3);
    }

    /// <summary>
    ///  触发事件，带4个参数触发
    /// </summary>
    /// <typeparam name="T">第1个参数类型</typeparam>
    /// <typeparam name="U">第2个参数类型</typeparam>
    /// <typeparam name="V">第3个参数类型</typeparam>
    /// <typeparam name="W">第4个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="arg2">第2个参数</param>
    /// <param name="arg3">第3个参数</param>
    /// <param name="arg4">第4个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void fire<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4)
    {
        _eventController.triggerEvent(eventType, arg1, arg2, arg3, arg4);
    }

    /// <summary>
    /// 触发事件，带1个object参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void fire(string eventType, object arg1)
    {
        _eventController.triggerEvent(eventType, arg1);
    }

    /// <summary>
    /// 触发事件，带2个object参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="arg2">第2个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void fire(string eventType, object arg1, object arg2)
    {
        _eventController.triggerEvent(eventType, arg1, arg2);
    }
    #endregion

    public static ObjectEventDispatcher Get(GameObject obj)
    {
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null)
        {
            dispatcher = obj.AddComponent<ObjectEventDispatcher>();
        }

        return dispatcher;
    }
}
