using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main1 : MonoBehaviour
{
    public StandbyTime standbyTime;
    private void Awake()
    {
        standbyTime = gameObject.AddComponent<StandbyTime>();
        EventManager.OnStandbyHandler += EventManager_OnStandbyHandler;//�����¼�   
    }


    /// <summary>
    /// �����¼�
    /// </summary>
    private void EventManager_OnStandbyHandler()
    {
        //��������Ҫ�򿪴������ر�������
/*        standbyPage.ThisSetActive(true);
        homePage.ThisSetActive(true);
        contentPage.RemoveAllChild(contentPage.transform);*/
    }
}
