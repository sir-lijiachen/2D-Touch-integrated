using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 这是一个选项页，用于控制点击按钮
/// </summary>
public class OptionPage : MonoBehaviour
{
    private Image bgImg;//当前选项页背景
    private Transform btnListTran;//按钮集的父类
    private List<Button> btnList;//内容的点击按钮
    private Button backButton;//返回按钮

    //页码和左右翻页
    private GameObject pagination;//是否显示页码
    private Button leftButton;
    private Button rightButton;
    private Text pageText;//页码文本

    private void Awake()
    {
        //挂载
    }
    private void Start()
    {
        
    }
    private void StorageButton()
    {
        //存放按钮List
        foreach(Transform child in btnListTran)
        {
            Button btn = child.GetComponent<Button>();
            btnList.Add(btn);//看是否需要存储按钮
            int num = btnList.IndexOf(btn);
            btn.onClick.AddListener(() =>  ClickButton(num));
        }

        //直接点击按钮
        for(int i = 0; i < btnListTran.childCount; i++)
        {
            Button btn = btnListTran.GetChild(i).GetComponent<Button>();
            int num = i;
            btn.onClick.AddListener(()=>  ClickButton(num));
        }
    }
    private void ClickButton(int index)
    {

    }
}
