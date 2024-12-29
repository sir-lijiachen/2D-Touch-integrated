using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ContentPage4 : MonoBehaviour
{
    public Transform contentPageTran;

    private int totalAnimations; // �ƻ��еĶ�������
    private int completedAnimations; // ��ɵĶ�����

    private float beginDragPos;//��꿪ʼ��λ��
    private float endDragPos;//��������λ��

    private int lastPage;//�����ϴ�/��ǰҳ��
    private int page=0;//ҳ��


    private void Awake()
    {
        contentPageTran = transform.GetChild(0);
    }

    private void Update()
    {
        // �����갴���¼�Xxx
        if (Input.GetMouseButtonDown(0))
        {
            beginDragPos = Input.mousePosition.x;
        }

        // �������ɿ��¼�
        if (Input.GetMouseButtonUp(0))
        {
            endDragPos = Input.mousePosition.x;

            HandleSwipe();
        }
    }

    /// <summary>
    /// �ƶ��ж�����һҳ������һҳ
    /// </summary>
    private void HandleSwipe()
    {
        float dragDistance = endDragPos - beginDragPos;
        if (Mathf.Abs(dragDistance) > 50) // ������ֵ�Ա�����΢��������
        {
            UpdatePage(dragDistance > 0 ? -1 : 1);
        }
    }
    /// <summary>
    /// ִ���´�һ������ǰ����
    /// </summary>
    public void UpdatePage(int direction)
    {
        int newPageNum = Mathf.Clamp(page + direction, 0, FindMaxPageNumber());//+1��-1����һҳ��һҳ
        if (newPageNum != page)
        {
            lastPage = page;
            page = newPageNum;
            LoadContent(page);
        }
    }
  
    /// <summary>
    /// ���ҳ��
    /// </summary>
    private int FindMaxPageNumber()
    {
        return ReadJSON.imgAtbArray.Max(attr => attr.pageNumber);
    }












    /// <summary>
    /// ��������
    /// </summary>
    public void LoadContent(int _page)
    {
        //��ɾ�����ݵĸ��ࡿ
        RemoveAllChild(contentPageTran);

        // ���ص�ǰҳ��ͼƬ��Դ
        Sprite[] allContentResources = Resources.LoadAll<Sprite>($"{_page}");
        totalAnimations = allContentResources.Length;
        completedAnimations = 0;

        // ������������ȡ��Ӧ����Ϣ,��������Ҫ�ڶ�ȡ��JSON�еı�����ȡ�
        List<ReadJSONAttribute> currentAttributes = ReadJSON.imgAtbArray.FindAll(attr => attr.index == _page);
        //�������ص���Դ������ͼƬ������������
        for (int i = 0; i < totalAnimations; i++)
        {
            GameObject imgObject = new GameObject("img " + i);
            imgObject.transform.parent = contentPageTran;//��ͼƬ�ĸ���mask��
            imgObject.transform.localPosition = new Vector2(currentAttributes[i].startPosX, currentAttributes[i].startPosY);

            ImageExecute img = imgObject.AddComponent<ImageExecute>();
            img.sprite = allContentResources[i];
            img.color = new Color(1, 1, 1, 0);
            img.SetNativeSize();

            //ִ��
            img.Init(currentAttributes[i]);
        }
    }



    /// <summary>
    /// �Ƴ�ȫ����Ԫ��
    /// </summary>
    private void RemoveAllChild(Transform _partentTran)
    {
        foreach (Transform child in _partentTran)
        {
            // �ݻ��Ӷ������������
            Destroy(child.gameObject);
        }
    }

}
