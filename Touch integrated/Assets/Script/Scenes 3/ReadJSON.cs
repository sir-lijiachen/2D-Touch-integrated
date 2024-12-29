using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class ReadJSON : MonoBehaviour
{
    public static List<ReadJSONAttribute> imgAtbArray;

    private void Awake()
    {
        //【Header("Scenes 3配置.json   需要改为自己的json文件名字")】
        string configPath = Path.Combine(Application.streamingAssetsPath, "Scenes 3配置.json");
        if (File.Exists(configPath))
        {
            string jsonData = File.ReadAllText(configPath);

            // 注册自定义导入器来处理类型转换
            RegisterCustomImporters();

            imgAtbArray = JsonMapper.ToObject<List<ReadJSONAttribute>>(jsonData);
        }
    }
    /// <summary>
    /// JSON中类型转换
    /// </summary>
    private void RegisterCustomImporters()
    {
        //【Header("需要根据情况加以改变")】
        JsonMapper.RegisterImporter<double, int>(input => Convert.ToInt32(input));//将 double 类型转换为 int 
        JsonMapper.RegisterImporter<double, float>(input => Convert.ToSingle(input));//从 double 类型转换为 float 
    }
}
