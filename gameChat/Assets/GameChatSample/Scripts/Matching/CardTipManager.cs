using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardTipManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;

    //ũ�Ⱑ 4(�̹��� ����)�� �迭 �����
    const int SIZE = 4;
    float[] pos = new float[SIZE];
    float distance, curPos, targetPos;
    bool isDrag;
    public int targetIndex;

    public float time = 3;

    // Start is called before the first frame update
    void Start()
    {
        // �Ÿ��� ���� 0~1�� pos����
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;

        //�� ���� �� 2�� ���� ��(ù �� ���� �ð�) 2�� �������� AutoCardTip�Լ� �ݺ� ����
        InvokeRepeating("AutoCardTip", time, time);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDrag) scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
    }

    float SetPos()
    {
        // ���ݰŸ��� �������� ����� ��ġ�� ��ȯ
        for (int i = 0; i < SIZE; i++)
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

    void AutoCardTip()
    {
        if (targetIndex == 3)
        {
            targetIndex = 0;
        }

        else if (targetIndex == 0)
        {
            targetIndex = 1;
        }

        else if (targetIndex == 1)
        {
            targetIndex = 2;
        }

        else if (targetIndex == 2)
        {
            targetIndex = 3;
        }

        else
        {
            targetIndex = 0;
        }

        targetPos = pos[targetIndex];
    }

    public void LeftBtn()
    {
        //�ڵ� �ݺ� ����
        CancelInvoke("AutoCardTip");

        if (targetIndex != 0)
        {
            --targetIndex;
            targetPos = pos[targetIndex];
        }

        else
        {
            targetIndex = 3;
            targetPos = pos[targetIndex];
        }

        //�ڵ� �ݺ� �����
        InvokeRepeating("AutoCardTip", time, time);

    }

    public void RightBtn()
    {
        //�ڵ� �ݺ� ����
        CancelInvoke("AutoCardTip");

        if (targetIndex != 3)
        {
            ++targetIndex;
            targetPos = pos[targetIndex];
        }

        else
        {
            targetIndex = 0;
            targetPos = pos[targetIndex];
        }

        //�ڵ� �ݺ� �����
        InvokeRepeating("AutoCardTip", time, time);
    }
}
