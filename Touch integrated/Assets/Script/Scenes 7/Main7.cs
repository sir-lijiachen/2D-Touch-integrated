using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Main7 : MonoBehaviour
{
    // ���ֵ�洢ÿ�����ҵ�ͼƬ�б�
    public Dictionary<string, List<Texture2D>> countryImages = new Dictionary<string, List<Texture2D>>();
    // ��Ҫ���ص�ͼƬ��׺
    private string[] supportedExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".tiff" };
    private void Awake()
    {
        // ����ͼƬ
        LoadImages();
    }

    /// ��������ͼƬ���ڴ�
    void LoadImages()
    {
        // ��������30�����ң�ÿ�����ҵ��ļ��ж��� StreamingAssets ��
        string[] countryNames =
            {
                "ͼƬ1", "ͼƬ2", "img/ͼƬ3", "img/ͼƬ4",
            };

        foreach (var country in countryNames)
        {
            LoadImagesForCountry(country);
        }
    }

    /// ����ָ�����ҵ�ͼƬ
    void LoadImagesForCountry(string country)
    {
        // ƴ�ӹ��ҵ��ļ���·��
        string folderPath = Path.Combine(Application.streamingAssetsPath, country);

        // ����ļ����Ƿ����
        if (Directory.Exists(folderPath))
        {
            string[] allFiles = Directory.GetFiles(folderPath);  // ��ȡ�ļ����е������ļ�

            List<Texture2D> images = new List<Texture2D>();  // ÿ�����ҵ�ͼƬ�б�

            foreach (string filePath in allFiles)
            {
                // ��ȡ�ļ�����չ��
                string extension = Path.GetExtension(filePath).ToLower();

                // ����ļ���չ����֧�ֵ�ͼƬ��ʽ
                if (IsSupportedImageExtension(extension))
                {
                    LoadImage(filePath, images);
                }
            }

            // �����ҵ�ͼƬ�б�洢���ֵ���
            if (images.Count > 0)
            {
                countryImages[country] = images;
                Debug.Log($"�Ѽ��� {country} �� {images.Count} ��ͼƬ");
            }
            else
            {
                Debug.LogWarning($"δ�ܼ��� {country} ��ͼƬ");
            }
        }
        else
        {
            Debug.LogError($"�ļ��� {folderPath} �����ڣ�");
        }
    }

    // �ж��ļ���׺�Ƿ�Ϊ֧�ֵ�ͼƬ��ʽ
    bool IsSupportedImageExtension(string extension)
    {
        foreach (string ext in supportedExtensions)
        {
            if (extension == ext)
            {
                return true;
            }
        }
        return false;
    }
    // ����ͼƬ�ļ����ڴ�
    void LoadImage(string filePath, List<Texture2D> images)
    {
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);  // ��ȡ�ļ�Ϊ�ֽ�����
            Texture2D texture = new Texture2D(2, 2);  // ����һ����ʱ��Texture2D����

            if (texture.LoadImage(fileData))  // �����ֽ����鲢��������
            {
                images.Add(texture);  // ������洢���ù��ҵ�ͼƬ�б���
            }
            else
            {
                Debug.LogError($"����ͼƬʧ��: {filePath}");
            }
        }
        else
        {
            Debug.LogError($"�ļ�δ�ҵ�: {filePath}");
        }
    }


    // ʾ�������ݹ������ƻ�ȡ���ص�ͼƬ�б�
    //��������������Ի�ȡͼƬ
    public List<Texture2D> GetImagesForCountry(string country)
    {
        if (countryImages.ContainsKey(country))
        {
            return countryImages[country];
        }
        else
        {
            Debug.LogError($"δ�ҵ����� {country} ��ͼƬ��");
            return new List<Texture2D>();
        }
    }
}
