using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 点击待机页，自动关闭待机画面
/// </summary>
public class StandbyPage : MonoBehaviour
{
    private Button standbyButton;

    private void Awake()
    {
        standbyButton = transform.GetComponent<Button>();
    }
    private void Start()
    {
        //点击待机页按钮
        standbyButton.onClick.AddListener(()=> ThisSetActive(false));
    }

    //控制当前活动状态
    public void ThisSetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
