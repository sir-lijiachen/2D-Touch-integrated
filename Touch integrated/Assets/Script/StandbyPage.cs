using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �������ҳ���Զ��رմ�������
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
        //�������ҳ��ť
        standbyButton.onClick.AddListener(()=> ThisSetActive(false));
    }

    //���Ƶ�ǰ�״̬
    public void ThisSetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
