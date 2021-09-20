//=====================================================
// - FileName:      EventCenter.cs
// - Created:       mahuibao
// - UserName:      2020-04-15
// - Email:         1023276156@qq.com
// - Description:   C#层事件管理中心
//======================================================
using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================
// - 事件管理层分C#层跟lua层
// - 通过字符串为Key注册事件
// -
// -
// - 重点注意：C#层，注册事件参数数量跟类型，必须对应派发事件时候参数数量跟类型
// -
// -
// -
// - 避免c#层跟lua层之间频繁调用，触发事件逻辑都添加了一个标记位参数，用于判断是否触发到另外一层。
// - 
// - c#层触发到lua层逻辑，使用的是在LuaManager初始化之后，通过LuaManager获取到LuaEventCenter管理层table，再调用里面的函数进行触发。
// - 
// - lua层触发到C#层逻辑，首先将此类(EventCenter)注册生成warp文件，使得lua层可以调用。
// - 因为lua不支持c#的泛型参数，使用的通过c#层事件管理层中，特定的函数进行触发。
// - 目前仅有四种特定函数，在下面封装好的模块之中，如有特殊需求，可以自行封装。函数名为：FireForLua
// - 温馨提示：为避免GC，尽量不要使用特别的数据结构在C#层跟lua层之间传输。
// - 
// - 使用例子：
// - C#层
// - 1.声明事件字符串
// -    目前增加了一个EventConst类，里面存储了所有事件类型，如下：
// -    public const string OPEN_VIEW = "OPEN_VIEW";
// -    public const string LUA_MGR_INIT_COMPLETE = "LUA_MGR_INIT_COMPLETE";
// -
// - 2.注册事件
// -    无参数：EventCenter.Instance.Register(EventConst.LUA_MGR_INIT_COMPLETE, this.tessss);
// -    回调：private void tessss(){}
// -        
// -    有参数：EventCenter.Instance.Register<int, string>(EventConst.OPEN_VIEW, this.testText);
// -    回调：private void testText(int can1, string can2){}
// - 
// - 3.移除事件
// -    无参数：EventCenter.Instance.UnRegister(EventConst.LUA_MGR_INIT_COMPLETE, this.tessss);
// -    回调：private void tessss(){}
// -        
// -    有参数：EventCenter.Instance.UnRegister<int, string>(EventConst.OPEN_VIEW, this.testText);
// -    回调：private void testText(int can1, string can2){}
// - 
// - 4.触发事件(触发事件中，最后带有一个默认参数，是否需要触发到Lua层事件，默认为false）
// -    无参数：EventCenter.Instance.Fire(EventConst.LUA_MGR_INIT_COMPLETE,true);
// -        
// -    有参数：EventCenter.Instance.Fire(EventConst.OPEN_VIEW, 111, "test",true);
// - 
//======================================================

public delegate void EventCallBack();
public delegate void EventCallBack<T>(T arg1);
public delegate void EventCallBack<T, U>(T arg1, U arg2);
public delegate void EventCallBack<T, U, V>(T arg1, U arg2, V arg3);
public delegate void EventCallBack<T, U, V, W>(T arg1, U arg2, V arg3, W arg4);
public delegate void EventCallBack<T, U, V, W, Z>(T arg1, U arg2, V arg3, W arg4, Z arg5);
public delegate void EventCallBack<T, U, V, W, Z, X>(T arg1, U arg2, V arg3, W arg4, Z arg5, X args6);

public class EventCenter
{
    private static EventCenter instance;

    public static EventCenter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventCenter();
            }
            return instance;
        }
    }

    /// <summary>
    /// 封装好的事件管理类
    /// </summary>
    private static EventController _eventController = new EventController();

    /// <summary>
    /// 事件字典（string：delegate）
    /// </summary>
    public static Dictionary<string, Delegate> theRouter
    {
        get { return _eventController.theRouter; }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void init()
    {
        //Debug.Log("C#层事件管理中心初始化");
        //register(EventConst.LUA_MGR_INIT_COMPLETE, this.afterInit);
    }

    /// <summary>
    /// 在LuaManager层初始化完成后，获取到需要LuaEventCenter的Table
    /// </summary>
    public void afterInit()
    {
        //Debug.Log("C#获取lua层的事件管理层");
        //_LEventTable = Lua.LuaManager.instance.lua.GetTable("LuaEventCenter");
    }

    /// <summary>
    /// 标记为永久注册事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    public void markAsPermanent(string eventType)
    {
        _eventController.markAsPermanent(eventType);
    }

    /// <summary>
    /// 清除非永久性注册事件
    /// </summary>
    public void cleanUp()
    {
        _eventController.cleanUp();
    }

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
    public void fire(string eventType, bool canIsTriggerLuaBoo=false)
    {
        _eventController.triggerEvent(eventType);
        //if (canIsTriggerLuaBoo && _LEventTable != null)
        //{
        //    _LEventTable.Call("fire", eventType);
        //}
    }

    /// <summary>
    /// 触发事件，带1个参数触发
    /// </summary>
    /// <typeparam name="T">第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void fire<T>(string eventType, T arg1, bool canIsTriggerLuaBoo = false)
    {
        _eventController.triggerEvent(eventType, arg1);
        //if (canIsTriggerLuaBoo && _LEventTable != null)
        //{
        //    _LEventTable.Call("fire", eventType, arg1);
        //}
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
    public void fire<T, U>(string eventType, T arg1, U arg2, bool canIsTriggerLuaBoo = false)
    {
        _eventController.triggerEvent(eventType, arg1, arg2);
        //if (canIsTriggerLuaBoo && _LEventTable != null)
        //{
        //    _LEventTable.Call("fire", eventType, arg1, arg2);
        //}
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
    public void fire<T, U, V>(string eventType, T arg1, U arg2, V arg3, bool canIsTriggerLuaBoo = false)
    {
        _eventController.triggerEvent(eventType, arg1, arg2, arg3);
        //if (canIsTriggerLuaBoo && _LEventTable != null)
        //{
        //    _LEventTable.Call("fire", eventType, arg1, arg2, arg3);
        //}
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
    public void fire<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4, bool canIsTriggerLuaBoo = false)
    {
        _eventController.triggerEvent(eventType, arg1, arg2, arg3, arg4);
        //if (canIsTriggerLuaBoo && _LEventTable != null)
        //{
        //    _LEventTable.Call("fire", eventType, arg1, arg2, arg3, arg4);
        //}
    }

    /// <summary>
    /// 触发事件，带1个object参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void fire(string eventType, object arg1, bool canIsTriggerLuaBoo = false)
    {
        _eventController.triggerEvent(eventType, arg1);
        //if (canIsTriggerLuaBoo && _LEventTable != null)
        //{
        //    _LEventTable.Call("fire", eventType, arg1);
        //}
    }

    /// <summary>
    /// 触发事件，带2个object参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="arg2">第2个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void fire(string eventType, object arg1, object arg2, bool canIsTriggerLuaBoo = false)
    {
        _eventController.triggerEvent(eventType, arg1, arg2);
        //if (canIsTriggerLuaBoo && _LEventTable != null)
        //{
        //    _LEventTable.Call("fire", eventType, arg1, arg2);
        //}
    }
    #endregion

    #region 静态 GameObject 增加监听器

    /// <summary>
    /// 增加监听器，不带参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">不带参数事件回调</param>
    public void targetRegister(GameObject obj, string eventType, EventCallBack handler)
    {
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(obj);
        dispatcher.register(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，1个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetRegister<T>(GameObject obj, string eventType, EventCallBack<T> handler)
    {
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(obj);
        dispatcher.register(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，2个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetRegister<T, U>(GameObject obj, string eventType, EventCallBack<T, U> handler)
    {
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(obj);
        dispatcher.register(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，3个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <typeparam name="V">回调中第3个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetRegister<T, U, V>(GameObject obj, string eventType, EventCallBack<T, U, V> handler)
    {
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(obj);
        dispatcher.register(eventType, handler);
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
    public void targetRegister<T, U, V, W>(GameObject obj, string eventType, EventCallBack<T, U, V, W> handler)
    {
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(obj);
        dispatcher.register(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，1个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带1个object参数的回调</param>
    public void targetRegister(GameObject obj, string eventType, EventCallBack<object> handler)
    {
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(obj);
        dispatcher.register(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，2个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带2个object参数的回调</param>
    public void targetRegister(GameObject obj, string eventType, EventCallBack<object, object> handler)
    {
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(obj);
        dispatcher.register(eventType, handler);
    }
    #endregion

    #region  静态 GameObject 移除监听器
    /// <summary>
    /// 移除监听器，不带参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">不带参数回调</param>
    public void targetUnregister(GameObject obj, string eventType, EventCallBack handler)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，1个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetUnregister<T>(GameObject obj, string eventType, EventCallBack<T> handler)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，2个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetUnregister<T, U>(GameObject obj, string eventType, EventCallBack<T, U> handler)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，3个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <typeparam name="V">回调中第3个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetUnregister<T, U, V>(GameObject obj, string eventType, EventCallBack<T, U, V> handler)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
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
    public void targetUnregister<T, U, V, W>(GameObject obj, string eventType, EventCallBack<T, U, V, W> handler)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，1个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带1个object参数的回调</param>
    public void targetUnregister(GameObject obj, string eventType, EventCallBack<object> handler)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，2个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带2个object参数的回调</param>
    public void targetUnregister(GameObject obj, string eventType, EventCallBack<object, object> handler)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }
    #endregion

    #region 静态 GameObject 触发事件

    /// <summary>
    /// 触发事件，不带参数触发
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void targetFire(GameObject obj, string eventType)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType);
    }

    /// <summary>
    /// 触发事件，带1个参数触发
    /// </summary>
    /// <typeparam name="T">第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void targetFire<T>(GameObject obj, string eventType, T arg1)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1);
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
    public void targetFire<T, U>(GameObject obj, string eventType, T arg1, U arg2)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1, arg2);
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
    public void targetFire<T, U, V>(GameObject obj, string eventType, T arg1, U arg2, V arg3)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1, arg2, arg3);
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
    public void targetFire<T, U, V, W>(GameObject obj, string eventType, T arg1, U arg2, V arg3, W arg4)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1, arg2, arg3, arg4);
    }

    /// <summary>
    /// 触发事件，带1个object参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void targetFire(GameObject obj, string eventType, object arg1)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1);
    }

    /// <summary>
    /// 触发事件，带2个object参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="arg2">第2个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void targetFire(GameObject obj, string eventType, object arg1, object arg2)
    {
        if (obj == null) return;
        ObjectEventDispatcher dispatcher = obj.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1, arg2);
    }
    #endregion

    #region 静态 Component 增加监听器

    /// <summary>
    /// 增加监听器，不带参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">不带参数事件回调</param>
    public void targetRegister(Component comp, string eventType, EventCallBack handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(comp.gameObject);
        dispatcher.register(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，1个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetRegister<T>(Component comp, string eventType, EventCallBack<T> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(comp.gameObject);
        dispatcher.register(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，2个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetRegister<T, U>(Component comp, string eventType, EventCallBack<T, U> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(comp.gameObject);
        dispatcher.register(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，3个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <typeparam name="V">回调中第3个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetRegister<T, U, V>(Component comp, string eventType, EventCallBack<T, U, V> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(comp.gameObject);
        dispatcher.register(eventType, handler);
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
    public void targetRegister<T, U, V, W>(Component comp, string eventType, EventCallBack<T, U, V, W> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(comp.gameObject);
        dispatcher.register(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，1个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带1个object参数的回调</param>
    public void targetRegister(Component comp, string eventType, EventCallBack<object> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(comp.gameObject);
        dispatcher.register(eventType, handler);
    }

    /// <summary>
    /// 增加监听器，2个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带2个object参数的回调</param>
    public void targetRegister(Component comp, string eventType, EventCallBack<object, object> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = ObjectEventDispatcher.Get(comp.gameObject);
        dispatcher.register(eventType, handler);
    }
    #endregion

    #region  静态 Component 移除监听器
    /// <summary>
    /// 移除监听器，不带参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">不带参数回调</param>
    public void targetUnregister(Component comp, string eventType, EventCallBack handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，1个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetUnregister<T>(Component comp, string eventType, EventCallBack<T> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，2个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetUnregister<T, U>(Component comp, string eventType, EventCallBack<T, U> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，3个参数
    /// </summary>
    /// <typeparam name="T">回调中第1个参数类型</typeparam>
    /// <typeparam name="U">回调中第2个参数类型</typeparam>
    /// <typeparam name="V">回调中第3个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void targetUnregister<T, U, V>(Component comp, string eventType, EventCallBack<T, U, V> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
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
    public void targetUnregister<T, U, V, W>(Component comp, string eventType, EventCallBack<T, U, V, W> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，1个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带1个object参数的回调</param>
    public void targetUnregister(Component comp, string eventType, EventCallBack<object> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }

    /// <summary>
    /// 移除监听器，2个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带2个object参数的回调</param>
    public void targetUnregister(Component comp, string eventType, EventCallBack<object, object> handler)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.unRegister(eventType, handler);
    }
    #endregion

    #region 静态 Component 触发事件

    /// <summary>
    /// 触发事件，不带参数触发
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void targetFire(Component comp, string eventType)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType);
    }

    /// <summary>
    /// 触发事件，带1个参数触发
    /// </summary>
    /// <typeparam name="T">第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void targetFire<T>(Component comp, string eventType, T arg1)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1);
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
    public void targetFire<T, U>(Component comp, string eventType, T arg1, U arg2)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1, arg2);
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
    public void targetFire<T, U, V>(Component comp, string eventType, T arg1, U arg2, V arg3)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1, arg2, arg3);
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
    public void targetFire<T, U, V, W>(Component comp, string eventType, T arg1, U arg2, V arg3, W arg4)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1, arg2, arg3, arg4);
    }

    /// <summary>
    /// 触发事件，带1个object参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void targetFire(Component comp, string eventType, object arg1)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1);
    }

    /// <summary>
    /// 触发事件，带2个object参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="arg2">第2个参数</param>
    /// <param name="canIsTriggerLuaBoo">是否触发到Lua层的事件层，默认为False</param>
    public void targetFire(Component comp, string eventType, object arg1, object arg2)
    {
        if (comp == null) return;
        ObjectEventDispatcher dispatcher = comp.gameObject.GetComponent<ObjectEventDispatcher>();
        if (dispatcher == null) return;
        dispatcher.fire(eventType, arg1, arg2);
    }
    #endregion

    #region For lua层的触发事件调用
    /// <summary>
    /// 触发事件，不带参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    public void fireForLua(string eventType)
    {
        fire(eventType);
    }
    /// <summary>
    /// 触发事件，固定带1个int参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">int数值</param>
    public void fireForLua(string eventType, int arg1)
    {
        fire<int>(eventType, arg1);
    }
    /// <summary>
    /// 触发事件，固定带1个string参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">string数值</param>
    public void fireForLua(string eventType, string arg1)
    {
        fire<string>(eventType, arg1);
    }
    /// <summary>
    /// 触发事件，固定带1个int参数，1个string参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">int数值</param>
    /// <param name="arg2">string数值</param>
    public void fireForLua(string eventType, int arg1, string arg2)
    {
        fire<int, string>(eventType, arg1, arg2);
    }

	/// <summary>
	/// 触发事件，固定带1个int参数，1个string参数
	/// </summary>
	/// <param name="eventType">事件类型</param>
	/// <param name="arg1">string</param>
	/// <param name="arg2">string数值</param>
	public void fireForLua(string eventType, string arg1, string arg2)
	{
		fire<string, string>(eventType, arg1, arg2);
	}

	/// <summary>
	/// 触发事件，固定带3个string参数
	/// </summary>
	/// <param name="eventType">事件类型</param>
	/// <param name="arg1">int数值</param>
	/// <param name="arg2">string数值</param>
	public void fireForLua(string eventType, string arg1, string arg2, string arg3)
    {
        fire<string, string, string>(eventType, arg1, arg2, arg3);
    }
    
    /// <summary>
    /// 触发事件，固定带3个int参数
    /// </summary>
    public void fireForLua(string eventType, int arg1, int arg2, int arg3)
    {
        fire<int, int, int>(eventType, arg1, arg2, arg3);
    }
    
    /// <summary>
    /// 触发事件，固定带2个int参数，2个string参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">int数值</param>
    /// <param name="arg2">int数值</param>
    /// <param name="arg3">string数值</param>
    /// <param name="arg4">string数值</param>
    public void fireForLua(string eventType, int arg1, int arg2, string arg3, string arg4)
    {
        fire<int, int, string, string>(eventType, arg1, arg2, arg3, arg4);
    }

	/// <summary>
	/// 触发事件，固定带2个int参数，2个string参数
	/// </summary>
	/// <param name="eventType">事件类型</param>
	/// <param name="table">LuaTable数值</param>
	//public void fireForLua(string eventType, LuaTable table)
	//{
		//fire<LuaTable>(eventType, table);
	//}

	#endregion
}

/// <summary>
/// 事件处理类
/// </summary>
public class EventController
{
    /// <summary>
    /// 事件字典
    /// </summary>
    private Dictionary<string, Delegate> _theRouter = new Dictionary<string, Delegate>();

    public Dictionary<string, Delegate> theRouter
    {
        get { return _theRouter; }
    }

    /// <summary>
    /// 永久注册的事件列表
    /// </summary>
    private List<string> _PermanentEvents = new List<string>();

    /// <summary>
    /// 标记为永久注册事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    public void markAsPermanent(string eventType)
    {
        _PermanentEvents.Add(eventType);
    }

    /// <summary>
    /// 判断是否已经包含事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <returns></returns>
    public bool containsEvent(string eventType)
    {
        return _theRouter.ContainsKey(eventType);
    }

    /// <summary>
    /// 清除非永久性注册的事件
    /// </summary>
    public void cleanUp()
    {
        List<string> eventToRemove = new List<string>();
        var iter = _theRouter.GetEnumerator();
        while (iter.MoveNext())
        {
            bool wasFound = false;
            int permanentEvnetsCount = _PermanentEvents.Count;
            for (int i = 0; i < permanentEvnetsCount; i++)
            {
                if (iter.Current.Key == _PermanentEvents[i])
                {
                    wasFound = true;
                    break;
                }
            }

            if (!wasFound) eventToRemove.Add(iter.Current.Key);
        }
        int eventToRemoveCount = eventToRemove.Count;
        for (int i = 0; i < eventToRemoveCount; i++)
        {
            _theRouter.Remove(eventToRemove[i]);
        }
    }

    /// <summary>
    /// 处理增加监听前的事项，检测参数等等
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="listenerBeingAdded">增加的监听回调</param>
    private void _OnListenerAdding(string eventType, Delegate listenerBeingAdded)
    {
        if (!_theRouter.ContainsKey(eventType))
        {
            _theRouter.Add(eventType, null);
        }

        Delegate d = _theRouter[eventType];
        if (d != null && d.GetType() != listenerBeingAdded.GetType())
        {
            throw new Exception(string.Format("Try to add not correct event {0} ，Current type is{1}, adding type is {2} "
                , eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
        }
    }

    /// <summary>
    /// 处理移除监听前的事项，检测参数等等
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="listenerBeingAdded">移除的监听回调</param>
    private bool _OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
    {
        if (!_theRouter.ContainsKey(eventType))
        {
            return false;
        }
        Delegate d = _theRouter[eventType];
        if ((d != null) && (d.GetType() != listenerBeingRemoved.GetType()))
        {
            throw new Exception(string.Format("Remove listener {0}\" failed, Current type is {1}, removing type is{2}"
                , eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
        }
        else
            return true;
    }

    /// <summary>
    /// 移除监听之后的处理，检测此事件类型中，监听回调是否为空，然后在字典中移除事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    private void _OnListenerRemoved(string eventType)
    {
        if (_theRouter.ContainsKey(eventType) && _theRouter[eventType] == null)
        {
            _theRouter.Remove(eventType);
        }
    }

    #region 增加监听器
    /// <summary>
    /// 增加监听器，不带参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">不带参数事件回调</param>
    public void addEventListener(string eventType, EventCallBack handler)
    {
        _OnListenerAdding(eventType, handler);
        if (_theRouter[eventType] != null)
        {
            foreach (EventCallBack tempCallBack in _theRouter[eventType].GetInvocationList())
            {
                if (handler == tempCallBack)
                {
                    Debug.LogWarning(string.Format("重复注册事件监听，事件类型{0}，注册的事件{1}", eventType, handler.GetType().Name));
                    return;
                }
            }
        }
        _theRouter[eventType] = (EventCallBack)_theRouter[eventType] + handler;
    }

    /// <summary>
    /// 增加监听器，1个参数
    /// </summary>
    /// <typeparam name="T">回调中的第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void addEventListener<T>(string eventType, EventCallBack<T> handler)
    {
        _OnListenerAdding(eventType, handler); if (_theRouter[eventType] != null)
        {
            foreach (EventCallBack<T> tempCallBack in ((EventCallBack<T>)_theRouter[eventType]).GetInvocationList())
            {
                if (handler == tempCallBack)
                {
                    Debug.LogWarning(string.Format("重复注册事件监听，事件类型{0}，注册的事件{1}", eventType, handler.GetType().Name));
                    return;
                }
            }
        }
        _theRouter[eventType] = (EventCallBack<T>)_theRouter[eventType] + handler;
    }

    /// <summary>
    /// 增加监听器，2个参数
    /// </summary>
    /// <typeparam name="T">回调中的第1个参数类型</typeparam>
    /// <typeparam name="U">回调中的第2个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void addEventListener<T, U>(string eventType, EventCallBack<T, U> handler)
    {
        _OnListenerAdding(eventType, handler);
        if (_theRouter[eventType] != null)
        {
            foreach (EventCallBack<T, U> tempCallBack in ((EventCallBack<T, U>)_theRouter[eventType]).GetInvocationList())
            {
                if (handler == tempCallBack)
                {
                    Debug.LogWarning(string.Format("重复注册事件监听，事件类型{0}，注册的事件{1}", eventType, handler.GetType().Name));
                    return;
                }
            }
        }
        _theRouter[eventType] = (EventCallBack<T, U>)_theRouter[eventType] + handler;
    }

    /// <summary>
    /// 增加监听器，3个参数
    /// </summary>
    /// <typeparam name="T">回调中的第1个参数类型</typeparam>
    /// <typeparam name="U">回调中的第2个参数类型</typeparam>
    /// <typeparam name="V">回调中的第3个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void addEventListener<T, U, V>(string eventType, EventCallBack<T, U, V> handler)
    {
        _OnListenerAdding(eventType, handler);
        if (_theRouter[eventType] != null)
        {
            foreach (EventCallBack<T, U, V> tempCallBack in ((EventCallBack<T, U, V>)_theRouter[eventType]).GetInvocationList())
            {
                if (handler == tempCallBack)
                {
                    Debug.LogWarning(string.Format("重复注册事件监听，事件类型{0}，注册的事件{1}", eventType, handler.GetType().Name));
                    return;
                }
            }
        }
        _theRouter[eventType] = (EventCallBack<T, U, V>)_theRouter[eventType] + handler;
    }

    /// <summary>
    /// 增加监听器，4个参数
    /// </summary>
    /// <typeparam name="T">回调中的第1个参数类型</typeparam>
    /// <typeparam name="U">回调中的第2个参数类型</typeparam>
    /// <typeparam name="V">回调中的第3个参数类型</typeparam>
    /// <typeparam name="W">回调中的第4个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void addEventListener<T, U, V, W>(string eventType, EventCallBack<T, U, V, W> handler)
    {
        _OnListenerAdding(eventType, handler);
        if (_theRouter[eventType] != null)
        {
            foreach (EventCallBack<T, U, V, W> tempCallBack in ((EventCallBack<T, U, V, W>)_theRouter[eventType]).GetInvocationList())
            {
                if (handler == tempCallBack)
                {
                    Debug.LogWarning(string.Format("重复注册事件监听，事件类型{0}，注册的事件{1}", eventType, handler.GetType().Name));
                    return;
                }
            }
        }
        _theRouter[eventType] = (EventCallBack<T, U, V, W>)_theRouter[eventType] + handler;
    }

    /// <summary>
    /// 增加监听器，1个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带1个object参数的回调</param>
    public void addEventListener(string eventType, EventCallBack<object> handler)
    {
        _OnListenerAdding(eventType, handler);
        if (_theRouter[eventType] != null)
        {
            foreach (EventCallBack<object> tempCallBack in ((EventCallBack<object>)_theRouter[eventType]).GetInvocationList())
            {
                if (handler == tempCallBack)
                {
                    Debug.LogWarning(string.Format("重复注册事件监听，事件类型{0}，注册的事件{1}", eventType, handler.GetType().Name));
                    return;
                }
            }
        }
        _theRouter[eventType] = (EventCallBack<object>)_theRouter[eventType] + handler;
    }

    /// <summary>
    /// 增加监听器，2个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带2个object参数的回调</param>
    public void addEventListener(string eventType, EventCallBack<object, object> handler)
    {
        _OnListenerAdding(eventType, handler);
        if (_theRouter[eventType] != null)
        {
            foreach (EventCallBack<object, object> tempCallBack in ((EventCallBack<object, object>)_theRouter[eventType]).GetInvocationList())
            {
                if (handler == tempCallBack)
                {
                    Debug.LogWarning(string.Format("重复注册事件监听，事件类型{0}，注册的事件{1}", eventType, handler.GetType().Name));
                    return;
                }
            }
        }
        _theRouter[eventType] = (EventCallBack<object, object>)_theRouter[eventType] + handler;
    }

    #endregion

    #region 移除监听器
    /// <summary>
    /// 移除监听器，不带参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">不带参数回调</param>
    public void removeEventListener(string eventType, EventCallBack handler)
    {
        if (_OnListenerRemoving(eventType, handler))
        {
            _theRouter[eventType] = (EventCallBack)_theRouter[eventType] - handler;
            _OnListenerRemoved(eventType);
        }
    }

    /// <summary>
    /// 移除监听器，1个参数
    /// </summary>
    /// <typeparam name="T">回调中的第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void removeEventListener<T>(string eventType, EventCallBack<T> handler)
    {
        if (_OnListenerRemoving(eventType, handler))
        {
            _theRouter[eventType] = (EventCallBack<T>)_theRouter[eventType] - handler;
            _OnListenerRemoved(eventType);
        }
    }

    /// <summary>
    /// 移除监听器，2个参数
    /// </summary>
    /// <typeparam name="T">回调中的第1个参数类型</typeparam>
    /// <typeparam name="U">回调中的第2个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void removeEventListener<T, U>(string eventType, EventCallBack<T, U> handler)
    {
        if (_OnListenerRemoving(eventType, handler))
        {
            _theRouter[eventType] = (EventCallBack<T, U>)_theRouter[eventType] - handler;
            _OnListenerRemoved(eventType);
        }
    }

    /// <summary>
    /// 移除监听器，3个参数
    /// </summary>
    /// <typeparam name="T">回调中的第1个参数类型</typeparam>
    /// <typeparam name="U">回调中的第2个参数类型</typeparam>
    /// <typeparam name="V">回调中的第3个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void removeEventListener<T, U, V>(string eventType, EventCallBack<T, U, V> handler)
    {
        if (_OnListenerRemoving(eventType, handler))
        {
            _theRouter[eventType] = (EventCallBack<T, U, V>)_theRouter[eventType] - handler;
            _OnListenerRemoved(eventType);
        }
    }

    /// <summary>
    /// 移除监听器，4个参数
    /// </summary>
    /// <typeparam name="T">回调中的第1个参数类型</typeparam>
    /// <typeparam name="U">回调中的第2个参数类型</typeparam>
    /// <typeparam name="V">回调中的第3个参数类型</typeparam>
    /// <typeparam name="W">回调中的第4个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">事件回调</param>
    public void removeEventListener<T, U, V, W>(string eventType, EventCallBack<T, U, V, W> handler)
    {
        if (_OnListenerRemoving(eventType, handler))
        {
            _theRouter[eventType] = (EventCallBack<T, U, V, W>)_theRouter[eventType] - handler;
            _OnListenerRemoved(eventType);
        }
    }

    /// <summary>
    /// 移除监听器，1个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带1个object参数的回调</param>
    public void removeEventListener(string eventType, EventCallBack<object> handler)
    {
        if (_OnListenerRemoving(eventType, handler))
        {
            _theRouter[eventType] = (EventCallBack<object>)_theRouter[eventType] - handler;
            _OnListenerRemoved(eventType);
        }
    }

    /// <summary>
    /// 移除监听器，2个object参数的回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">带2个object参数的回调</param>
    public void removeEventListener(string eventType, EventCallBack<object, object> handler)
    {
        if (_OnListenerRemoving(eventType, handler))
        {
            _theRouter[eventType] = (EventCallBack<object, object>)_theRouter[eventType] - handler;
            _OnListenerRemoved(eventType);
        }
    }
    #endregion

    #region 触发事件
    /// <summary>
    /// 触发事件，不带参数触发
    /// </summary>
    /// <param name="eventType">事件类型</param>
    public void triggerEvent(string eventType)
    {
        Delegate d;
        if (!_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }
        var callBacks = d.GetInvocationList();
        for (int i = 0; i < callBacks.Length; i++)
        {
            EventCallBack callBack = callBacks[i] as EventCallBack;
            if (null == callBack.Target)
            {
                continue;
            }
            if (callBack == null)
            {
                throw new Exception(string.Format("TriggerEvent {0} error:types of parameters not match", eventType));
            }
            try
            {
                callBack();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    /// <summary>
    /// 触发事件，带1个参数触发
    /// </summary>
    /// <typeparam name="T">第1个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    public void triggerEvent<T>(string eventType, T arg1)
    {
        Delegate d;
        if (!_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }
        var callBacks = d.GetInvocationList();
        for (int i = 0; i < callBacks.Length; i++)
        {
            EventCallBack<T> callBack = callBacks[i] as EventCallBack<T>;
            //if (null == callBack.Target)
            //{
            //    continue;
            //}
            if (callBack == null || callBack.Target == null)
            {
                continue;
                //throw new Exception(string.Format("TriggerEvent {0} error:types of parameters not match", eventType));
            }
            try
            {
                callBack(arg1);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
    /// <summary>
    /// 触发事件，带2个参数触发
    /// </summary>
    /// <typeparam name="T">第1个参数类型</typeparam>
    /// <typeparam name="U">第2个参数类型</typeparam>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="arg2">第2个参数</param>
    public void triggerEvent<T, U>(string eventType, T arg1, U arg2)
    {
        Delegate d;
        if (!_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }
        var callBacks = d.GetInvocationList();
        for (int i = 0; i < callBacks.Length; i++)
        {
            EventCallBack<T, U> callBack = callBacks[i] as EventCallBack<T, U>;
            //if (null == callBack.Target)
            //{
            //    continue;
            //}
            if (callBack == null || callBack.Target == null)
            {
                continue;
                //throw new Exception(string.Format("TriggerEvent {0} error:types of parameters not match", eventType));
            }
            try
            {
                callBack(arg1, arg2);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
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
    public void triggerEvent<T, U, V>(string eventType, T arg1, U arg2, V arg3)
    {
        Delegate d;
        if (!_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }
        var callBacks = d.GetInvocationList();
        for (int i = 0; i < callBacks.Length; i++)
        {
            EventCallBack<T, U, V> callBack = callBacks[i] as EventCallBack<T, U, V>;
            if (null == callBack.Target)
            {
                continue;
            }
            if (callBack == null)
            {
                throw new Exception(string.Format("TriggerEvent {0} error:types of parameters not match", eventType));
            }
            try
            {
                callBack(arg1, arg2, arg3);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    /// <summary>
    /// 触发事件，带4个参数触发
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
    public void triggerEvent<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4)
    {
        Delegate d;
        if (!_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }
        var callBacks = d.GetInvocationList();
        for (int i = 0; i < callBacks.Length; i++)
        {
            EventCallBack<T, U, V, W> callBack = callBacks[i] as EventCallBack<T, U, V, W>;
            if (null == callBack.Target)
            {
                continue;
            }
            if (callBack == null)
            {
                throw new Exception(string.Format("TriggerEvent {0} error:types of parameters not match", eventType));
            }
            try
            {
                callBack(arg1, arg2, arg3, arg4);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    /// <summary>
    /// 触发事件，带1个object参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    public void triggerEvent(string eventType, object arg1)
    {
        Delegate d;
        if (!_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }
        var callBacks = d.GetInvocationList();
        for (int i = 0; i < callBacks.Length; i++)
        {
            EventCallBack<object> callBack = callBacks[i] as EventCallBack<object>;
            if (null == callBack.Target)
            {
                continue;
            }
            if (callBack == null)
            {
                throw new Exception(string.Format("TriggerEvent {0} error:types of parameters not match", eventType));
            }
            try
            {
                callBack(arg1);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    /// <summary>
    /// 触发事件，带2个object参数
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="arg1">第1个参数</param>
    /// <param name="arg2">第2个参数</param>
    public void triggerEvent(string eventType, object arg1, object arg2)
    {
        Delegate d;
        if (!_theRouter.TryGetValue(eventType, out d))
        {
            return;
        }
        var callBacks = d.GetInvocationList();
        for (int i = 0; i < callBacks.Length; i++)
        {
            EventCallBack<object, object> callBack = callBacks[i] as EventCallBack<object, object>;
            if (null == callBack.Target)
            {
                continue;
            }
            if (callBack == null)
            {
                throw new Exception(string.Format("TriggerEvent {0} error:types of parameters not match", eventType));
            }
            try
            {
                callBack(arg1, arg2);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    #endregion
}