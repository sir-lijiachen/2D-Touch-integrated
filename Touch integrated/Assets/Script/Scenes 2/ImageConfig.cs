using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEngine.UI;
public class ImageConfig : MonoBehaviour
{
    public string move = "Move";//移动
    public string appear = "Appear";//尺寸放大
    public string fadeIn = "Fade In";//淡入


    public static List<ImageAttribute>[,] imgAtbArray;//图片属性

    private void Awake()
    {
        imgAtbArray = new List<ImageAttribute>[8,4];

        //0-0
        imgAtbArray[0, 0] = new List<ImageAttribute>();
        imgAtbArray[0, 0].Add(Create( move, new Vector2(-2400, 50), new Vector2(-700, 50), 0f));
        imgAtbArray[0, 0].Add(Create( move, new Vector2(2400, 440), new Vector2(730, 440), 0.5f));
        imgAtbArray[0, 0].Add(Create( move, new Vector2(2400, 50), new Vector2(730, 50), 0.8f));
        imgAtbArray[0, 0].Add(Create( move, new Vector2(2400, -410), new Vector2(730, -410), 1.2f));

        //0-1
        imgAtbArray[0, 1] = new List<ImageAttribute>();
        imgAtbArray[0, 1].Add(Create( fadeIn, new Vector2(0, 610), new Vector2(0, 610), 0f));
        imgAtbArray[0, 1].Add(Create( appear, new Vector2(-790, 150), new Vector2(-790, 150), 0.5f));
        imgAtbArray[0, 1].Add(Create( appear, new Vector2(-810, -430), new Vector2(-810, -430), 1f));
        imgAtbArray[0, 1].Add(Create( fadeIn, new Vector2(0, -210), new Vector2(0, -210), 1f));
        imgAtbArray[0, 1].Add(Create( appear, new Vector2(850, -210), new Vector2(850, -120), 1.5f));


    }

    public ImageAttribute Create(string action, Vector2 startPos, Vector2 endPos, float delay)  //可以加一个图片位置
    {
        // 创建 ImageAttribute 对象并返回
        ImageAttribute imgAttr = new ImageAttribute();
        imgAttr.action = action;
        imgAttr.startPos = startPos;
        imgAttr.endPos = endPos;
        imgAttr.delay = delay;

        return imgAttr;
    }
}
