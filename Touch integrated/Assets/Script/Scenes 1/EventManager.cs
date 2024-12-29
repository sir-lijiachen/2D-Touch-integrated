using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager 
{
    //待机
    public delegate void StandbyHandler();
    public static event StandbyHandler OnStandbyHandler;
    // 动画
    public delegate void AnimationCompleted();
    public static event AnimationCompleted OnAnimationComplete;

    public static void TriggerAnimationComplete()
    {
        if (OnAnimationComplete != null)
        {
            OnAnimationComplete();
        }
    }

    /// <summary>
    /// 待机
    /// </summary>
    public static void TriggerStandbyHandler()
    {
        // 调用事件之前，检查是否有方法订阅了这个事件
        if (OnStandbyHandler != null)
        {
            OnStandbyHandler(); // 触发事件
        }
    }
}
