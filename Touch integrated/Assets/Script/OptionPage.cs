using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����һ��ѡ��ҳ�����ڿ��Ƶ����ť
/// </summary>
public class OptionPage : MonoBehaviour
{
    private Image bgImg;//��ǰѡ��ҳ����
    private Transform btnListTran;//��ť���ĸ���
    private List<Button> btnList;//���ݵĵ����ť
    private Button backButton;//���ذ�ť

    //ҳ������ҷ�ҳ
    private GameObject pagination;//�Ƿ���ʾҳ��
    private Button leftButton;
    private Button rightButton;
    private Text pageText;//ҳ���ı�

    private void Awake()
    {
        //����
    }
    private void Start()
    {
        
    }
    private void StorageButton()
    {
        //��Ű�ťList
        foreach(Transform child in btnListTran)
        {
            Button btn = child.GetComponent<Button>();
            btnList.Add(btn);//���Ƿ���Ҫ�洢��ť
            int num = btnList.IndexOf(btn);
            btn.onClick.AddListener(() =>  ClickButton(num));
        }

        //ֱ�ӵ����ť
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
