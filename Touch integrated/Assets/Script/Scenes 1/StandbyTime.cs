using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自定义事件
public class StandbyTime : MonoBehaviour
{
    private float timer = 0f; // 计时器
    private bool isTimerRunning = false; // 是否启动计时器

    void Update()
    {
        // 检查是否有左键点击，如果有，则重置计时器
        if (Input.GetMouseButtonDown(0))
        {
            ResetTimer();
        }

        // 如果计时器正在运行，则递增计时器
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            // 如果计时器超过设置的待机时间，则触发事件并停止计时器
            if (timer >= 300)//【读取UdpConfig.standbyTime】
            {
                isTimerRunning = false;
                EventManager.TriggerStandbyHandler();
            }
        }
    }
    /// <summary>
    /// 时间重置
    /// </summary>
    public void ResetTimer()
    {
        timer = 0f;
        isTimerRunning = true;
    }

    /// <summary>
    /// 停止计时
    /// </summary>
    /// 待机的地方写【standbyTime.SetIsInStandbyPage(false);//时间停止】
    public void SetIsInStandbyPage(bool _switch)
    {
        isTimerRunning = _switch;
    }
}
