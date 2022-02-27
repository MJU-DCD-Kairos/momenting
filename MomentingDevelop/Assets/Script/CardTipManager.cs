using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardTipManager : MonoBehaviour
{
    public Text textObject;
    const int TextMaxCount = 4;
    string[] CardTipArr;
    int i = 0;
    public float time = 3;

    // Start is called before the first frame update
    void Start()
    {
        textObject.text = CardTipArr[i];
        //�� ���� �� 2�� ���� ��(ù �� ���� �ð�) 2�� �������� AutoCardTip�Լ� �ݺ� ����
        InvokeRepeating("AutoCardTip", time, time); 
    }

   
    void Awake()
    {
        //������ �ؽ�Ʈ�� ��� �迭 ����
        CardTipArr = new string[TextMaxCount];
        CardTipArr[0] = "��ü ä�� �ð��� �� ������" + "\n" + "1:1 ��ȭ ��븦 ������ �� �־��!" + "\n" + "����� ���� ��ü���� ���ĵǴ�" + "\n" + "�����ϱ� ���ؼ��� ���� �����ؾ� �ؿ�.";
        CardTipArr[1] = "1:1 ��ȭ ��û�� �����ϸ� �����" + "\n" + "�ٸ� ������ �������� Ȯ���� �� �־��.";
        CardTipArr[2] = "1:1 ��ȭ 24�ð� ��� ��" + "\n" + "�ų� �򰡸� �� �� �־��." + "\n" + "�����ֽ� �򰡴� �����" + "\n" + "�ų� ��޿� �ݿ��˴ϴ�!";
        CardTipArr[3] = "1:1 ��ȭ���� �����ϸ� ��ȭ�� ����ǰ�" + "\n" + "7�� �Ŀ� ��ȭ���� �ڵ� �����ſ�.";

        //Tip ���ӿ�����Ʈ ã�� �����ϱ�
        GameObject Tip = GameObject.Find("Tip");
        textObject = GetComponent<Text>();
    }




    //ī������ ���� �� �����ϴ� �Լ�(�ڵ� �ݺ��� ���� �ʿ�)
    void AutoCardTip()
    {
        if (i == 3)
        {
            i = 0;
        }

        else if (i == 0)
        {
            i = 1;
        }

        else if (i == 1)
        {
            i = 2;
        }

        else if (i == 2)
        {
            i = 3;
        }

        else
        {
            i = 0;
        }

        textObject.text = CardTipArr[i];
    }
    
    //chevron_left ��ư ������ �� ���� �� �����ϴ� �Լ�
    public void OnLeftBtnClick()
    {
        //�ڵ� �ݺ� ����
        CancelInvoke("AutoCardTip");

        if (i == 0)
        {
            i = 3;
        }

        else if(i == 1)
        {
            i = 0;
        }

        else if(i == 2)
        {
            i = 1;
        }

        else
        {
            i = 2;
        }

        textObject.text = CardTipArr[i];

        //�ڵ� �ݺ� �����
        InvokeRepeating("AutoCardTip", time, time);

    }


    //chevron_right ��ư ������ �� ���� �� �����ϴ� �Լ�
    public void OnRightBtnClick()
    {
        //�ڵ� �ݺ� ����
        CancelInvoke("AutoCardTip");

        if (i == 3)
        {
            i = 0;
        }

        else if(i == 0)
        {
            i = 1;
        }

        else if(i == 1)
        {
            i = 2;
        }

        else if(i == 2)
        {
            i = 3;
        }

        else
        {
            i = 0;
        }

        textObject.text = CardTipArr[i];

        //�ڵ� �ݺ� �����
        InvokeRepeating("AutoCardTip", time, time);

    }

}
