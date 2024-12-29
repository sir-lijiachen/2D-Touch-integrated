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
        //��Header("Scenes 3����.json   ��Ҫ��Ϊ�Լ���json�ļ�����")��
        string configPath = Path.Combine(Application.streamingAssetsPath, "Scenes 3����.json");
        if (File.Exists(configPath))
        {
            string jsonData = File.ReadAllText(configPath);

            // ע���Զ��嵼��������������ת��
            RegisterCustomImporters();

            imgAtbArray = JsonMapper.ToObject<List<ReadJSONAttribute>>(jsonData);
        }
    }
    /// <summary>
    /// JSON������ת��
    /// </summary>
    private void RegisterCustomImporters()
    {
        //��Header("��Ҫ����������Ըı�")��
        JsonMapper.RegisterImporter<double, int>(input => Convert.ToInt32(input));//�� double ����ת��Ϊ int 
        JsonMapper.RegisterImporter<double, float>(input => Convert.ToSingle(input));//�� double ����ת��Ϊ float 
    }
}
