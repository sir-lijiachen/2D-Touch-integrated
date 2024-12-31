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
    [SerializeField] private Button btn_left; // ��ť�������л���ǰһ��ý��
    [SerializeField] private Button btn_right; // �Ұ�ť�������л�����һ��ý��
    [SerializeField] private Button btn_back; // ���ذ�ť
    [SerializeField] private GameObject obj_media;
    [SerializeField] private Text text_mediaName; // ������ʾ��ǰ����չʾ��ý������
    [SerializeField] private Text text_pageNumber; // ������ʾ��ǰҳ����Ϣ

    [SerializeField] private GameObject obj_Special;
    [SerializeField] private GameObject obj_Img;
    [SerializeField] private GameObject obj_Video;

    private GameObject currentPrefab; // ��ǰ������ʾ��Ԥ�裬�����л�ͼƬ/��Ƶ����
    private Transform contentParent; // ��Ŷ�̬ʵ����ý�����ݵĸ�������
    private List<string> mediaPaths = new List<string>(); // �洢����ý���ļ�·����ͼƬ����Ƶ��
    private int currentMediaIndex = -1; // ��ǰ������ʾ��ý������

    // ֧�ֵ�ͼƬ��չ���б�
    private readonly string[] imageExtensions = { ".png", ".jpg", ".jpeg" };
    // ֧�ֵ���Ƶ��չ���б�
    private readonly string[] videoExtensions = { ".mp4", ".avi", ".mov" };

    private bool isImage = false; // ���ڼ�¼�ϴμ��ص�ý�����ͣ��Ƿ�ΪͼƬ


    private List<Texture2D> pakistanImages;

    private float beginDragPos;//��꿪ʼ��λ��
    private float endDragPos;//��������λ��
    /// <summary>
    /// ��ʼ����ť�Ͱ��¼���
    /// </summary>
    private void Awake()
    {

        btn_left = transform.Find("Left").GetComponent<Button>(); // ��ȡ��ť����
        btn_right = transform.Find("Right").GetComponent<Button>(); // ��ȡ�Ұ�ť����
        btn_back = transform.Find("Back").GetComponent<Button>(); // ��ȡ���ذ�ť����
        contentParent = transform.Find("Content"); // ��ȡ������ݵĸ�������


        // ��ȡText�������
        obj_media = transform.Find("MediaNameImg").gameObject;
        text_mediaName = transform.Find("MediaNameImg/MediaNameText").GetComponent<Text>();
        text_pageNumber = transform.Find("PageNumberText").GetComponent<Text>();

        // Ϊ���Ұ�ť�󶨵���¼�
        btn_left.onClick.AddListener(ShowPreviousMedia);
        btn_right.onClick.AddListener(ShowNextMedia);
        btn_back.onClick.AddListener(HandleBackButtonClick);
    }


    /// <summary>
    /// �����ť�󣬴�����·�������ض�ý���ļ���ͼƬ����Ƶ����
    /// </summary>
    public void ClickButton_Path(string path)
    {
        // �����������е� GetImagesForCountry ��������ȡ��ָ��������ص�ͼƬ
        pakistanImages = main7.GetImagesForCountry(path);

        string mediaDir = Path.Combine(Application.streamingAssetsPath, $"{path}");
        mediaPaths = GetFilesByExtension(mediaDir, imageExtensions.Concat(videoExtensions).ToArray());

        if (mediaPaths.Count > 0)
        {
            Debug.Log($"�ҵ� {mediaPaths.Count} ��ý���ļ�");
            currentMediaIndex = -1; // ��������
            ShowNextMedia(); // ��ʾ��һ��ý������
        }
        else
        {
            Debug.LogWarning("δ�ҵ��κ�ͼƬ����Ƶ��");
        }
    }

    /// <summary>
    /// ��ȡָ��Ŀ¼�·���ָ����չ��������ý���ļ�·����
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
    /// ��ʾ��һ��ý�����ݡ�
    /// </summary>
    private void ShowNextMedia()
    {
        if (mediaPaths.Count == 0) return; // ���ý��·��Ϊ�գ�����

        currentMediaIndex = (currentMediaIndex + 1) % mediaPaths.Count; // ѭ���л�����һ��ý��
        DisplayMedia(mediaPaths[currentMediaIndex]); // ���ص�ǰý������
    }

    /// <summary>
    /// ��ʾ��һ��ý�����ݡ�
    /// </summary>
    private void ShowPreviousMedia()
    {
        if (mediaPaths.Count == 0) return; // ���ý��·��Ϊ�գ�����

        currentMediaIndex = (currentMediaIndex - 1 + mediaPaths.Count) % mediaPaths.Count; // ѭ���л�����һ��ý��
        DisplayMedia(mediaPaths[currentMediaIndex]); // ���ص�ǰý������
    }

    /// <summary>
    /// ��ʾ��ǰѡ�е�ý�����ݣ�ͼƬ����Ƶ����
    /// </summary>
    private void DisplayMedia(string mediaPath)
    {
        string extension = Path.GetExtension(mediaPath).ToLower(); // ��ȡý���ļ���չ��

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(mediaPath); // ��ȡ�ļ�����ȥ����չ����

        // �����ǰ��ͼƬ������һ��Ҳ��ͼƬ����ֱ���滻ͼƬ
        if (Array.Exists(imageExtensions, ext => ext == extension) && isImage && fileNameWithoutExtension != "00")
        {
            LoadImage(mediaPath, currentPrefab.GetComponentInChildren<RawImage>(), false);
        }
        else
        {
            if (currentPrefab != null)
            {
                Destroy(currentPrefab); // ���֮ǰ�����ݴ��ڣ�������
            }

            if (Array.Exists(imageExtensions, ext => ext == extension)) // �����ͼƬ����
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
                    rectTransform.anchoredPosition = new Vector2(0, 20); // ����Ļ���� (0, 100) �ƶ�

                    LoadImage(mediaPath, currentPrefab.GetComponentInChildren<RawImage>(), false);
                    isImage = true; // ���õ�ǰý������ΪͼƬ
                }
            }
            else if (Array.Exists(videoExtensions, ext => ext == extension)) // �������Ƶ����
            {
                GameObject videoPrefab = Resources.Load<GameObject>("Prefab_Video");
                currentPrefab = Instantiate(videoPrefab, contentParent);

                obj_media.SetActive(true);
                RectTransform rectTransform = text_mediaName.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, 110); // ����Ļ���� (0, 100) �ƶ�

                PlayVideo(mediaPath, currentPrefab.GetComponent<VideoPlayer>());
                isImage = false; // ���õ�ǰý������Ϊ��Ƶ
            }
        }
        UpdateUI(mediaPath); // ÿ�μ���ý������UI
    }


    /// <summary>
    /// �첽����ͼƬ���ݡ�
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
    /// ������Ƶ���ݡ�
    /// </summary>
    private void PlayVideo(string videoPath, VideoPlayer videoPlayer)
    {
        videoPlayer.url = videoPath; // ������Ƶ·��
        videoPlayer.Pause(); // ������Ƶ
    }

    /// <summary>
    /// ����UIԪ����Ϣ��������ǰý�����ƺ�ҳ����Ϣ��
    /// </summary>
    private void UpdateUI(string mediaPath)
    {
        // ��ȡý�����ƣ�ȥ��·����ǰ��������
        string fileName = Path.GetFileName(mediaPath);
        if (fileName.Length > 3)
        {
            fileName = fileName.Substring(3); // ȥ��ǰ3���ַ�
        }

        // ȥ���ļ�����չ��
        fileName = Path.GetFileNameWithoutExtension(fileName);

        // ����UI�е�ý������
        text_mediaName.text = fileName;

        btn_left.gameObject.SetActive(true);
        btn_right.gameObject.SetActive(true);
        // ����ҳ����Ϣ
        text_pageNumber.text = $"��{currentMediaIndex + 1}ҳ/��{mediaPaths.Count}ҳ";

        if (mediaPaths.Count == 1)
        {
            text_pageNumber.text = "";
            btn_left.gameObject.SetActive(false);
            btn_right.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ���ذ�ť����¼������߼���
    /// </summary>
    public void HandleBackButtonClick()
    {
        Debug.Log("���ذ�ť�����");

        ThisSetActive(false);
        // ���ٵ�ǰý������չʾ
        if (currentPrefab)
        {
            Destroy(currentPrefab);
            currentPrefab = null;
        }

        // ���ý��·��������
        mediaPaths.Clear();
        currentMediaIndex = -1;
        isImage = false;

        // ����UIΪ��ʼ״̬
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
