using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager 
{
    //����
    public delegate void StandbyHandler();
    public static event StandbyHandler OnStandbyHandler;
    // ����
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
    /// ����
    /// </summary>
    public static void TriggerStandbyHandler()
    {
        // �����¼�֮ǰ������Ƿ��з�������������¼�
        if (OnStandbyHandler != null)
        {
            OnStandbyHandler(); // �����¼�
        }
    }
}
