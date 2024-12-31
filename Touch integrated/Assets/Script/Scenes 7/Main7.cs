using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Main7 : MonoBehaviour
{
    // 用字典存储每个国家的图片列表
    public Dictionary<string, List<Texture2D>> countryImages = new Dictionary<string, List<Texture2D>>();
    // 需要加载的图片后缀
    private string[] supportedExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".tiff" };
    private void Awake()
    {
        // 加载图片
        LoadImages();
    }

    /// 加载所有图片到内存
    void LoadImages()
    {
        // 假设你有30个国家，每个国家的文件夹都在 StreamingAssets 下
        string[] countryNames =
            {
                "图片1", "图片2", "img/图片3", "img/图片4",
            };

        foreach (var country in countryNames)
        {
            LoadImagesForCountry(country);
        }
    }

    /// 加载指定国家的图片
    void LoadImagesForCountry(string country)
    {
        // 拼接国家的文件夹路径
        string folderPath = Path.Combine(Application.streamingAssetsPath, country);

        // 检查文件夹是否存在
        if (Directory.Exists(folderPath))
        {
            string[] allFiles = Directory.GetFiles(folderPath);  // 获取文件夹中的所有文件

            List<Texture2D> images = new List<Texture2D>();  // 每个国家的图片列表

            foreach (string filePath in allFiles)
            {
                // 获取文件的扩展名
                string extension = Path.GetExtension(filePath).ToLower();

                // 如果文件扩展名是支持的图片格式
                if (IsSupportedImageExtension(extension))
                {
                    LoadImage(filePath, images);
                }
            }

            // 将国家的图片列表存储到字典中
            if (images.Count > 0)
            {
                countryImages[country] = images;
                Debug.Log($"已加载 {country} 的 {images.Count} 张图片");
            }
            else
            {
                Debug.LogWarning($"未能加载 {country} 的图片");
            }
        }
        else
        {
            Debug.LogError($"文件夹 {folderPath} 不存在！");
        }
    }

    // 判断文件后缀是否为支持的图片格式
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
    // 加载图片文件到内存
    void LoadImage(string filePath, List<Texture2D> images)
    {
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);  // 读取文件为字节数组
            Texture2D texture = new Texture2D(2, 2);  // 创建一个临时的Texture2D对象

            if (texture.LoadImage(fileData))  // 加载字节数组并生成纹理
            {
                images.Add(texture);  // 将纹理存储到该国家的图片列表中
            }
            else
            {
                Debug.LogError($"加载图片失败: {filePath}");
            }
        }
        else
        {
            Debug.LogError($"文件未找到: {filePath}");
        }
    }


    // 示例：根据国家名称获取加载的图片列表
    //调用这个方法可以获取图片
    public List<Texture2D> GetImagesForCountry(string country)
    {
        if (countryImages.ContainsKey(country))
        {
            return countryImages[country];
        }
        else
        {
            Debug.LogError($"未找到国家 {country} 的图片！");
            return new List<Texture2D>();
        }
    }
}
