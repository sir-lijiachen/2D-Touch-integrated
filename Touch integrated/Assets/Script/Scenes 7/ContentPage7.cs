using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Networking;
using System.IO;
using DG.Tweening;
using System.Linq;

public class ContentPage7 : MonoBehaviour
{
    public Main7 main7;
    [SerializeField] private Button btn_left; // 左按钮，用于切换到前一个媒体
    [SerializeField] private Button btn_right; // 右按钮，用于切换到下一个媒体
    [SerializeField] private Button btn_back; // 返回按钮
    [SerializeField] private GameObject obj_media;
    [SerializeField] private Text text_mediaName; // 用于显示当前正在展示的媒体名称
    [SerializeField] private Text text_pageNumber; // 用于显示当前页码信息

    [SerializeField] private GameObject obj_Special;
    [SerializeField] private GameObject obj_Img;
    [SerializeField] private GameObject obj_Video;

    private GameObject currentPrefab; // 当前正在显示的预设，用于切换图片/视频内容
    private Transform contentParent; // 存放动态实例化媒体内容的父级对象
    private List<string> mediaPaths = new List<string>(); // 存储所有媒体文件路径（图片或视频）
    private int currentMediaIndex = -1; // 当前正在显示的媒体索引

    // 支持的图片扩展名列表
    private readonly string[] imageExtensions = { ".png", ".jpg", ".jpeg" };
    // 支持的视频扩展名列表
    private readonly string[] videoExtensions = { ".mp4", ".avi", ".mov" };

    private bool isImage = false; // 用于记录上次加载的媒体类型，是否为图片


    private List<Texture2D> pakistanImages;

    private float beginDragPos;//鼠标开始的位置
    private float endDragPos;//鼠标结束的位置
    /// <summary>
    /// 初始化按钮和绑定事件。
    /// </summary>
    private void Awake()
    {

        btn_left = transform.Find("Left").GetComponent<Button>(); // 获取左按钮引用
        btn_right = transform.Find("Right").GetComponent<Button>(); // 获取右按钮引用
        btn_back = transform.Find("Back").GetComponent<Button>(); // 获取返回按钮引用
        contentParent = transform.Find("Content"); // 获取存放内容的父级对象


        // 获取Text组件引用
        obj_media = transform.Find("MediaNameImg").gameObject;
        text_mediaName = transform.Find("MediaNameImg/MediaNameText").GetComponent<Text>();
        text_pageNumber = transform.Find("PageNumberText").GetComponent<Text>();

        // 为左右按钮绑定点击事件
        btn_left.onClick.AddListener(ShowPreviousMedia);
        btn_right.onClick.AddListener(ShowNextMedia);
        btn_back.onClick.AddListener(HandleBackButtonClick);
    }


    /// <summary>
    /// 点击按钮后，从网络路径加载特定媒体文件（图片或视频）。
    /// </summary>
    public void ClickButton_Path(string path)
    {
        // 调用主程序中的 GetImagesForCountry 方法，获取与指定国家相关的图片
        pakistanImages = main7.GetImagesForCountry(path);

        string mediaDir = Path.Combine(Application.streamingAssetsPath, $"{path}");
        mediaPaths = GetFilesByExtension(mediaDir, imageExtensions.Concat(videoExtensions).ToArray());

        if (mediaPaths.Count > 0)
        {
            Debug.Log($"找到 {mediaPaths.Count} 个媒体文件");
            currentMediaIndex = -1; // 重置索引
            ShowNextMedia(); // 显示第一个媒体内容
        }
        else
        {
            Debug.LogWarning("未找到任何图片或视频！");
        }
    }

    /// <summary>
    /// 获取指定目录下符合指定扩展名的所有媒体文件路径。
    /// </summary>
    private List<string> GetFilesByExtension(string directory, string[] extensions)
    {
        List<string> matchingFiles = new List<string>();

        if (Directory.Exists(directory))
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                foreach (string ext in extensions)
                {
                    if (file.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingFiles.Add(file);
                    }
                }
            }
        }
        return matchingFiles;
    }

    /// <summary>
    /// 显示下一个媒体内容。
    /// </summary>
    private void ShowNextMedia()
    {
        if (mediaPaths.Count == 0) return; // 如果媒体路径为空，返回

        currentMediaIndex = (currentMediaIndex + 1) % mediaPaths.Count; // 循环切换到下一个媒体
        DisplayMedia(mediaPaths[currentMediaIndex]); // 加载当前媒体内容
    }

    /// <summary>
    /// 显示上一个媒体内容。
    /// </summary>
    private void ShowPreviousMedia()
    {
        if (mediaPaths.Count == 0) return; // 如果媒体路径为空，返回

        currentMediaIndex = (currentMediaIndex - 1 + mediaPaths.Count) % mediaPaths.Count; // 循环切换到上一个媒体
        DisplayMedia(mediaPaths[currentMediaIndex]); // 加载当前媒体内容
    }

    /// <summary>
    /// 显示当前选中的媒体内容（图片或视频）。
    /// </summary>
    private void DisplayMedia(string mediaPath)
    {
        string extension = Path.GetExtension(mediaPath).ToLower(); // 获取媒体文件扩展名

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(mediaPath); // 获取文件名（去掉扩展名）

        // 如果当前是图片并且上一次也是图片，则直接替换图片
        if (Array.Exists(imageExtensions, ext => ext == extension) && isImage && fileNameWithoutExtension != "00")
        {
            LoadImage(mediaPath, currentPrefab.GetComponentInChildren<RawImage>(), false);
        }
        else
        {
            if (currentPrefab != null)
            {
                Destroy(currentPrefab); // 如果之前的内容存在，先销毁
            }

            if (Array.Exists(imageExtensions, ext => ext == extension)) // 如果是图片类型
            {
                if (fileNameWithoutExtension == "00")
                {
                    GameObject imagePrefab = Resources.Load<GameObject>("Prefab_Special");
                    currentPrefab = Instantiate(imagePrefab, contentParent);
                    LoadImage(mediaPath, currentPrefab.GetComponent<RawImage>(), true);
                    currentPrefab.transform.localPosition = new Vector3(0, -115, 0);
                    isImage = false;
                    //return;
                }
                else
                {
                    GameObject imagePrefab = Resources.Load<GameObject>("Prefab_Img");
                    currentPrefab = Instantiate(imagePrefab, contentParent);

                    obj_media.SetActive(true);
                    RectTransform rectTransform = text_mediaName.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, 20); // 以屏幕坐标 (0, 100) 移动

                    LoadImage(mediaPath, currentPrefab.GetComponentInChildren<RawImage>(), false);
                    isImage = true; // 设置当前媒体类型为图片
                }
            }
            else if (Array.Exists(videoExtensions, ext => ext == extension)) // 如果是视频类型
            {
                GameObject videoPrefab = Resources.Load<GameObject>("Prefab_Video");
                currentPrefab = Instantiate(videoPrefab, contentParent);

                obj_media.SetActive(true);
                RectTransform rectTransform = text_mediaName.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, 110); // 以屏幕坐标 (0, 100) 移动

                PlayVideo(mediaPath, currentPrefab.GetComponent<VideoPlayer>());
                isImage = false; // 设置当前媒体类型为视频
            }
        }
        UpdateUI(mediaPath); // 每次加载媒体后更新UI
    }


    /// <summary>
    /// 异步加载图片内容。
    /// </summary>
    private void LoadImage(string imagePath, RawImage rawImage, bool isSpecial)
    {

        rawImage.texture = pakistanImages[currentMediaIndex];
        if (isSpecial)
        {
            obj_media.SetActive(false);
            rawImage.SetNativeSize();
        }
    }

    /// <summary>
    /// 播放视频内容。
    /// </summary>
    private void PlayVideo(string videoPath, VideoPlayer videoPlayer)
    {
        videoPlayer.url = videoPath; // 设置视频路径
        videoPlayer.Pause(); // 播放视频
    }

    /// <summary>
    /// 更新UI元素信息，包括当前媒体名称和页码信息。
    /// </summary>
    private void UpdateUI(string mediaPath)
    {
        // 提取媒体名称，去掉路径和前两个数字
        string fileName = Path.GetFileName(mediaPath);
        if (fileName.Length > 3)
        {
            fileName = fileName.Substring(3); // 去掉前3个字符
        }

        // 去掉文件的扩展名
        fileName = Path.GetFileNameWithoutExtension(fileName);

        // 更新UI中的媒体名称
        text_mediaName.text = fileName;

        btn_left.gameObject.SetActive(true);
        btn_right.gameObject.SetActive(true);
        // 更新页码信息
        text_pageNumber.text = $"第{currentMediaIndex + 1}页/共{mediaPaths.Count}页";

        if (mediaPaths.Count == 1)
        {
            text_pageNumber.text = "";
            btn_left.gameObject.SetActive(false);
            btn_right.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 返回按钮点击事件处理逻辑。
    /// </summary>
    public void HandleBackButtonClick()
    {
        Debug.Log("返回按钮点击！");

        ThisSetActive(false);
        // 销毁当前媒体内容展示
        if (currentPrefab)
        {
            Destroy(currentPrefab);
            currentPrefab = null;
        }

        // 清空媒体路径和索引
        mediaPaths.Clear();
        currentMediaIndex = -1;
        isImage = false;

        // 更新UI为初始状态
        text_mediaName.text = string.Empty;
        text_pageNumber.text = "";
    }

    public void ThisBg(string path)
    {
        Debug.Log(path);
        Image imgBg = transform.GetComponent<Image>();
        Sprite bg = Resources.Load<Sprite>(path);
        imgBg.sprite = bg;
    }

    public void ThisSetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
