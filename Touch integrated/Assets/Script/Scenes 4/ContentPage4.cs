using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ContentPage4 : MonoBehaviour
{
    public Transform contentPageTran;

    private int totalAnimations; // 计划中的动画总数
    private int completedAnimations; // 完成的动画数

    private float beginDragPos;//鼠标开始的位置
    private float endDragPos;//鼠标结束的位置

    private int lastPage;//保存上次/当前页码
    private int page=0;//页码


    private void Awake()
    {
        contentPageTran = transform.GetChild(0);
    }

    private void Update()
    {
        // 检测鼠标按下事件Xxx
        if (Input.GetMouseButtonDown(0))
        {
            beginDragPos = Input.mousePosition.x;
        }

        // 检测鼠标松开事件
        if (Input.GetMouseButtonUp(0))
        {
            endDragPos = Input.mousePosition.x;

            HandleSwipe();
        }
    }

    /// <summary>
    /// 移动判断是上一页还是下一页
    /// </summary>
    private void HandleSwipe()
    {
        float dragDistance = endDragPos - beginDragPos;
        if (Mathf.Abs(dragDistance) > 50) // 设置阈值以避免轻微触摸触发
        {
            UpdatePage(dragDistance > 0 ? -1 : 1);
        }
    }
    /// <summary>
    /// 执行下次一次内容前加载
    /// </summary>
    public void UpdatePage(int direction)
    {
        int newPageNum = Mathf.Clamp(page + direction, 0, FindMaxPageNumber());//+1、-1是下一页上一页
        if (newPageNum != page)
        {
            lastPage = page;
            page = newPageNum;
            LoadContent(page);
        }
    }
  
    /// <summary>
    /// 最大页数
    /// </summary>
    private int FindMaxPageNumber()
    {
        return ReadJSON.imgAtbArray.Max(attr => attr.pageNumber);
    }












    /// <summary>
    /// 加载内容
    /// </summary>
    public void LoadContent(int _page)
    {
        //【删除内容的父类】
        RemoveAllChild(contentPageTran);

        // 加载当前页的图片资源
        Sprite[] allContentResources = Resources.LoadAll<Sprite>($"{_page}");
        totalAnimations = allContentResources.Length;
        completedAnimations = 0;

        // 【根据条件获取对应的信息,各变量需要于读取的JSON中的变量相等】
        List<ReadJSONAttribute> currentAttributes = ReadJSON.imgAtbArray.FindAll(attr => attr.index == _page);
        //遍历加载的资源，创建图片对象并设置属性
        for (int i = 0; i < totalAnimations; i++)
        {
            GameObject imgObject = new GameObject("img " + i);
            imgObject.transform.parent = contentPageTran;//【图片的父类mask】
            imgObject.transform.localPosition = new Vector2(currentAttributes[i].startPosX, currentAttributes[i].startPosY);

            ImageExecute img = imgObject.AddComponent<ImageExecute>();
            img.sprite = allContentResources[i];
            img.color = new Color(1, 1, 1, 0);
            img.SetNativeSize();

            //执行
            img.Init(currentAttributes[i]);
        }
    }



    /// <summary>
    /// 移除全部子元素
    /// </summary>
    private void RemoveAllChild(Transform _partentTran)
    {
        foreach (Transform child in _partentTran)
        {
            // 摧毁子对象及其所有组件
            Destroy(child.gameObject);
        }
    }

}
