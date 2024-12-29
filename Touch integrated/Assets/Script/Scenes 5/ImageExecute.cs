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
    public Text valueText; // ��������һ��������ʾ��ֵ��UI�ı�

    internal void Init(ReadJSONAttribute _currentImgAttr)
    {
        DOVirtual.DelayedCall(_currentImgAttr.delay, () =>
        {
            Tween tween = null;
            switch (_currentImgAttr.action)
            {
                //����������Ҫ�Ķ������ã�ToWeen��������Щ��
                case "Move":
                    color = new Color(1, 1, 1, 1);
                    tween = transform.DOLocalMove(new Vector2(_currentImgAttr.endPosX, _currentImgAttr.endPosY), 0.6f).SetEase(Ease.InQuad);
                    break;
                case "Appear"://�Ŵ�
                    color = new Color(1, 1, 1, 1);
                    transform.localScale = Vector2.zero;
                    tween = transform.DOScale(Vector2.one, 1f).SetEase(Ease.OutQuad);
                    break;
                case "Fade In"://����
                    color = new Color(1, 1, 1, 0);
                    tween = this.DOFade(1f, 2f).SetEase(Ease.OutQuad);
                    break;
            }
        });
    }
    //��text�ı�����
    public static void FadeIn(Graphic text)//���Ա������κμ̳��� Graphic �� UI Ԫ��
    {
        text.color = new Color(1, 1, 1, 0);
        text.DOFade(1f, 1f).SetEase(Ease.OutQuad);
    }
}
