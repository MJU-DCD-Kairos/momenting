using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardTipManager_old : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;

    public Text textObject;
    const int TextMaxCount = 4;
    string[] CardTipArr;
    int i = 0;
    public float time = 3;

    float[] pos = new float[TextMaxCount];
    float distance, curPos, targetPos;
    bool isDrag;
    int targetIndex;

    // Start is called before the first frame update
    void Start()
    {
        textObject.text = CardTipArr[i];
        //�� ���� �� 2�� ���� ��(ù �� ���� �ð�) 2�� �������� AutoCardTip�Լ� �ݺ� ����
        InvokeRepeating("AutoCardTip", time, time);

        // �Ÿ��� ���� 0~1�� pos����
        distance = 1f / (TextMaxCount - 1);
        for (int i = 0; i < TextMaxCount; i++) pos[i] = distance * i;
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

    float SetPos()
    {
        // ���ݰŸ��� �������� ����� ��ġ�� ��ȯ
        for (int i = 0; i < TextMaxCount; i++)
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        return 0;
    }

    public void OnBeginDrag(PointerEventData eventData) => curPos = SetPos();

    public void OnDrag(PointerEventData eventData) => isDrag = true;

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();

        // ���ݰŸ��� ���� �ʾƵ� ���콺�� ������ �̵��ϸ�
        if (curPos == targetPos)
        {
            // �� ���� ������ ��ǥ�� �ϳ� ����
            if (eventData.delta.x > 18 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
            }

            // �� ���� ������ ��ǥ�� �ϳ� ����
            else if (eventData.delta.x < -18 && curPos + distance <= 1.01f)
            {
                ++targetIndex;
                targetPos = curPos + distance;
            }
        }
    }

    void Update()
    {
        if (!isDrag) scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
    }

}
