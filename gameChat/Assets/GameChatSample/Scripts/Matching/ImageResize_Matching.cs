using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageResize_Matching : MonoBehaviour
{
    //������Ʈ�� ���� �ν����ͻ󿡼� ������ ���� �����ϵ��� ����
    public float size = 384;
    public RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {

       
    }


    // Update is called once per frame
    public void Update()
    {
        //���� ���ϱ� ����
        //�ҽ��̹��� ���� ������� ����
        GetComponent<Image>().SetNativeSize();

        var RectTransform = transform as RectTransform;

        //���̰� �� ª���� ���̸� �ν����ͻ󿡼� �Է��� size�� �ٲ�
        if (RectTransform.sizeDelta.x < RectTransform.sizeDelta.y)
        {
            float aspect = RectTransform.sizeDelta.y / RectTransform.sizeDelta.x; //�̹��� ���� ���ϱ�
            
        }

        //���̰� �� ª���� ���̸� �ν����ͻ󿡼� �Է��� size�� �ٲ�
        else
        {
            float aspect = RectTransform.sizeDelta.x / RectTransform.sizeDelta.y; //�̹��� ���� ���ϱ�
            RectTransform.sizeDelta = new Vector2(size * aspect, size);
        }
        
    }

    
}
