using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardTipManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;

    //크기가 4(이미지 개수)인 배열 만들기
    const int SIZE = 4;
    float[] pos = new float[SIZE];
    float distance, curPos, targetPos;
    bool isDrag;
    public int targetIndex;

    public float time = 3;

    // Start is called before the first frame update
    void Start()
    {
        // 거리에 따라 0~1인 pos대입
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;

        //씬 실행 후 2초 지연 후(첫 팁 노출 시간) 2초 간격으로 AutoCardTip함수 반복 실행
        InvokeRepeating("AutoCardTip", time, time);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDrag) scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
    }

    float SetPos()
    {
        // 절반거리를 기준으로 가까운 위치를 반환
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

        // 절반거리를 넘지 않아도 마우스를 빠르게 이동하면
        if (curPos == targetPos)
        {
            // ← 으로 가려면 목표가 하나 감소
            if (eventData.delta.x > 18 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
            }

            // → 으로 가려면 목표가 하나 증가
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
        //자동 반복 중지
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

        //자동 반복 재시작
        InvokeRepeating("AutoCardTip", time, time);

    }

    public void RightBtn()
    {
        //자동 반복 중지
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

        //자동 반복 재시작
        InvokeRepeating("AutoCardTip", time, time);
    }
}
