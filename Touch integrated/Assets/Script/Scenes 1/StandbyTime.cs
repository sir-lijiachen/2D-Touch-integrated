using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Զ����¼�
public class StandbyTime : MonoBehaviour
{
    private float timer = 0f; // ��ʱ��
    private bool isTimerRunning = false; // �Ƿ�������ʱ��

    void Update()
    {
        // ����Ƿ���������������У������ü�ʱ��
        if (Input.GetMouseButtonDown(0))
        {
            ResetTimer();
        }

        // �����ʱ���������У��������ʱ��
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            // �����ʱ���������õĴ���ʱ�䣬�򴥷��¼���ֹͣ��ʱ��
            if (timer >= 300)//����ȡUdpConfig.standbyTime��
            {
                isTimerRunning = false;
                EventManager.TriggerStandbyHandler();
            }
        }
    }
    /// <summary>
    /// ʱ������
    /// </summary>
    public void ResetTimer()
    {
        timer = 0f;
        isTimerRunning = true;
    }

    /// <summary>
    /// ֹͣ��ʱ
    /// </summary>
    /// �����ĵط�д��standbyTime.SetIsInStandbyPage(false);//ʱ��ֹͣ��
    public void SetIsInStandbyPage(bool _switch)
    {
        isTimerRunning = _switch;
    }
}
