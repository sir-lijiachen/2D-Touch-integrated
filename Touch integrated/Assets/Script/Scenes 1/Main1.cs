using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main1 : MonoBehaviour
{
    public StandbyTime standbyTime;
    private void Awake()
    {
        standbyTime = gameObject.AddComponent<StandbyTime>();
        EventManager.OnStandbyHandler += EventManager_OnStandbyHandler;//待机事件   
    }


    /// <summary>
    /// 待机事件
    /// </summary>
    private void EventManager_OnStandbyHandler()
    {
        //【根据需要打开待机，关闭其他】
/*        standbyPage.ThisSetActive(true);
        homePage.ThisSetActive(true);
        contentPage.RemoveAllChild(contentPage.transform);*/
    }
}
