using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageResize : MonoBehaviour
{
    //������Ʈ�� ���� �ν����ͻ󿡼� ������ ���� �����ϵ��� ����
    public float size = 384;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }


    // Update is called once per frame
    void Update()
    {
        
        //�ҽ��̹��� ���� ������� ����
        GetComponent<Image>().SetNativeSize();

        var RectTransform = transform as RectTransform;

        //���̰� �� ª���� ���̸� �ν����ͻ󿡼� �Է��� size�� �ٲ�
        if (RectTransform.sizeDelta.x < RectTransform.sizeDelta.y)
        {
            RectTransform.sizeDelta = new Vector2(size, RectTransform.sizeDelta.y);
        }

        //���̰� �� ª���� ���̸� �ν����ͻ󿡼� �Է��� size�� �ٲ�
        else
        {
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, size);
        }

    }

    
}
