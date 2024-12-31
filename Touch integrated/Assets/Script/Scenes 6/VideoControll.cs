using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoControll : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer; // 视频播放器组件
    [SerializeField] private RectTransform rect_navigation; // 导航栏
    [SerializeField] private Button btn_play; // 播放按钮
    [SerializeField] private Button btn_pause; // 暂停按钮
    [SerializeField] private Button btn_replay; // 重播按钮
    [SerializeField] private Text text_videoTime; // 视频时间显示
    [SerializeField] private Slider slider_video; // 视频进度条
    [SerializeField] private Slider slider_volume; // 音量控制滑动条
    [SerializeField] private Text text_volume; // 音量百分比显示
    [SerializeField] private Button btn_back; // 返回按钮

    private float float_videoLength; // 视频总时长
    private string string_videoLength;

    private float float_currentTime;
    private string string_currentTime;

    private bool isDragging = false; // 用于标记是否正在拖动进度条

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

        // 设置Slider监听视频时间
        slider_video.onValueChanged.AddListener(OnSliderValueChanged);

        VideoTime();

        // 视频播放时，更新进度
        videoPlayer.frameReady += OnFrameReady;
    }
    private void Update()
    {

    }

    // 每帧更新时调用
    private void OnFrameReady(VideoPlayer source, long frameIndex)
    {
        // 获取当前播放时间
        float currentTime = (float)source.time;

        // 更新Slider的进度
        slider_video.value = currentTime;

        // 更新时间显示
        text_videoTime.text = TurnTimeToString(currentTime) + " / " + string_videoLength; 
    }

    // 视频播放
    private void OnReplayButtonDown()
    {
        videoPlayer.Play();
    }

    // 视频暂停
    private void OnPauseButtonDown()
    {
        videoPlayer.Pause();
    }

    // 视频重播
    private void OnPlayButtonDown()
    {
        // 暂停重置
        videoPlayer.Stop();
        videoPlayer.time = 0;
        slider_video.SetValueWithoutNotify(0);
        text_videoTime.text = TurnTimeToString(0) + "/" + string_videoLength;

        videoPlayer.Play();
    }

    // 获取视频时长
    private void VideoTime()
    {
        float_videoLength = (float)videoPlayer.length;
        string_videoLength = TurnTimeToString(float_videoLength);
    }

    // 转换时间（秒）为分钟:秒的格式
    private string TurnTimeToString(float time)
    {
        int minutes = (int)Mathf.Floor(time / 60);
        int remainingSeconds = (int)Mathf.Round(time % 60);
        string formatted = minutes.ToString("D2") + ":" + remainingSeconds.ToString("D2");
        return formatted;
    }


    // Slider的值发生变化时，更新视频进度
    private void OnSliderValueChanged(float value)
    {
        // 当Slider的值改变时，设置视频到相应的帧
        if (!isDragging)
        {
            videoPlayer.time = value * float_videoLength;
            float_currentTime = (float)videoPlayer.time;
            string_currentTime = TurnTimeToString(float_currentTime);
            text_videoTime.text = string_currentTime + "/" + string_videoLength;
        }
    }
}
