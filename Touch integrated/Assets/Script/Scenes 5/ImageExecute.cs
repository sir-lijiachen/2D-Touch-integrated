using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class ImageExecute : Image
{
    public Text valueText; // 假设你有一个用于显示数值的UI文本

    internal void Init(ReadJSONAttribute _currentImgAttr)
    {
        DOVirtual.DelayedCall(_currentImgAttr.delay, () =>
        {
            Tween tween = null;
            switch (_currentImgAttr.action)
            {
                //【根据所需要的动画设置，ToWeen基本就这些】
                case "Move":
                    color = new Color(1, 1, 1, 1);
                    tween = transform.DOLocalMove(new Vector2(_currentImgAttr.endPosX, _currentImgAttr.endPosY), 0.6f).SetEase(Ease.InQuad);
                    break;
                case "Appear"://放大
                    color = new Color(1, 1, 1, 1);
                    transform.localScale = Vector2.zero;
                    tween = transform.DOScale(Vector2.one, 1f).SetEase(Ease.OutQuad);
                    break;
                case "Fade In"://淡入
                    color = new Color(1, 1, 1, 0);
                    tween = this.DOFade(1f, 2f).SetEase(Ease.OutQuad);
                    break;
            }
        });
    }
    //【text文本需求】
    public static void FadeIn(Graphic text)//可以被用于任何继承自 Graphic 的 UI 元素
    {
        text.color = new Color(1, 1, 1, 0);
        text.DOFade(1f, 1f).SetEase(Ease.OutQuad);
    }
}
