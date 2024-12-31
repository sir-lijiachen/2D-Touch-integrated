using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoControll : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer; // ��Ƶ���������
    [SerializeField] private RectTransform rect_navigation; // ������
    [SerializeField] private Button btn_play; // ���Ű�ť
    [SerializeField] private Button btn_pause; // ��ͣ��ť
    [SerializeField] private Button btn_replay; // �ز���ť
    [SerializeField] private Text text_videoTime; // ��Ƶʱ����ʾ
    [SerializeField] private Slider slider_video; // ��Ƶ������
    [SerializeField] private Slider slider_volume; // �������ƻ�����
    [SerializeField] private Text text_volume; // �����ٷֱ���ʾ
    [SerializeField] private Button btn_back; // ���ذ�ť

    private float float_videoLength; // ��Ƶ��ʱ��
    private string string_videoLength;

    private float float_currentTime;
    private string string_currentTime;

    private bool isDragging = false; // ���ڱ���Ƿ������϶�������

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        rect_navigation = transform.GetChild(0).GetComponent<RectTransform>();
        btn_play = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        btn_pause = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        btn_replay = transform.GetChild(0).GetChild(2).GetComponent<Button>();
        text_videoTime = transform.GetChild(0).GetChild(3).GetComponent<Text>();
        slider_video = transform.GetChild(0).GetChild(4).GetComponent<Slider>();
        slider_volume = transform.GetChild(0).GetChild(6).GetComponent<Slider>();
        text_volume = transform.GetChild(0).GetChild(7).GetComponent<Text>();
        btn_back = transform.GetChild(0).GetChild(8).GetComponent<Button>();
    }

    private void Start()
    {
        btn_play.onClick.AddListener(OnPlayButtonDown);
        btn_pause.onClick.AddListener(OnPauseButtonDown);
        btn_replay.onClick.AddListener(OnReplayButtonDown);

        // ����Slider������Ƶʱ��
        slider_video.onValueChanged.AddListener(OnSliderValueChanged);

        VideoTime();

        // ��Ƶ����ʱ�����½���
        videoPlayer.frameReady += OnFrameReady;
    }
    private void Update()
    {

    }

    // ÿ֡����ʱ����
    private void OnFrameReady(VideoPlayer source, long frameIndex)
    {
        // ��ȡ��ǰ����ʱ��
        float currentTime = (float)source.time;

        // ����Slider�Ľ���
        slider_video.value = currentTime;

        // ����ʱ����ʾ
        text_videoTime.text = TurnTimeToString(currentTime) + " / " + string_videoLength; 
    }

    // ��Ƶ����
    private void OnReplayButtonDown()
    {
        videoPlayer.Play();
    }

    // ��Ƶ��ͣ
    private void OnPauseButtonDown()
    {
        videoPlayer.Pause();
    }

    // ��Ƶ�ز�
    private void OnPlayButtonDown()
    {
        // ��ͣ����
        videoPlayer.Stop();
        videoPlayer.time = 0;
        slider_video.SetValueWithoutNotify(0);
        text_videoTime.text = TurnTimeToString(0) + "/" + string_videoLength;

        videoPlayer.Play();
    }

    // ��ȡ��Ƶʱ��
    private void VideoTime()
    {
        float_videoLength = (float)videoPlayer.length;
        string_videoLength = TurnTimeToString(float_videoLength);
    }

    // ת��ʱ�䣨�룩Ϊ����:��ĸ�ʽ
    private string TurnTimeToString(float time)
    {
        int minutes = (int)Mathf.Floor(time / 60);
        int remainingSeconds = (int)Mathf.Round(time % 60);
        string formatted = minutes.ToString("D2") + ":" + remainingSeconds.ToString("D2");
        return formatted;
    }


    // Slider��ֵ�����仯ʱ��������Ƶ����
    private void OnSliderValueChanged(float value)
    {
        // ��Slider��ֵ�ı�ʱ��������Ƶ����Ӧ��֡
        if (!isDragging)
        {
            videoPlayer.time = value * float_videoLength;
            float_currentTime = (float)videoPlayer.time;
            string_currentTime = TurnTimeToString(float_currentTime);
            text_videoTime.text = string_currentTime + "/" + string_videoLength;
        }
    }
}
