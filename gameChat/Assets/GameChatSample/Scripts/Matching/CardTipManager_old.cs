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
        //씬 실행 후 2초 지연 후(첫 팁 노출 시간) 2초 간격으로 AutoCardTip함수 반복 실행
        InvokeRepeating("AutoCardTip", time, time);

        // 거리에 따라 0~1인 pos대입
        distance = 1f / (TextMaxCount - 1);
        for (int i = 0; i < TextMaxCount; i++) pos[i] = distance * i;
    }

   
    void Awake()
    {
        //서비스팁 텍스트가 담긴 배열 생성
        CardTipArr = new string[TextMaxCount];
        CardTipArr[0] = "단체 채팅 시간이 다 끝나면" + "\n" + "1:1 대화 상대를 선택할 수 있어요!" + "\n" + "종료된 이후 단체방은 폭파되니" + "\n" + "연락하기 위해서는 서로 선택해야 해요.";
        CardTipArr[1] = "1:1 대화 신청을 수락하면 상대의" + "\n" + "다른 프로필 사진들을 확인할 수 있어요.";
        CardTipArr[2] = "1:1 대화 24시간 경과 시" + "\n" + "매너 평가를 할 수 있어요." + "\n" + "남겨주신 평가는 상대의" + "\n" + "매너 등급에 반영됩니다!";
        CardTipArr[3] = "1:1 대화방을 삭제하면 대화가 종료되고" + "\n" + "7일 후에 대화방이 자동 삭제돼요.";

        //Tip 게임오브젝트 찾고 연결하기
        GameObject Tip = GameObject.Find("Tip");
        textObject = GetComponent<Text>();
    }




    //카드팁을 다음 팁 노출하는 함수(자동 반복을 위해 필요)
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
    
    //chevron_left 버튼 눌렀을 때 이전 팁 노출하는 함수
    public void OnLeftBtnClick()
    {
        //자동 반복 중지
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

        //자동 반복 재시작
        InvokeRepeating("AutoCardTip", time, time);

    }


    //chevron_right 버튼 눌렀을 때 다음 팁 노출하는 함수
    public void OnRightBtnClick()
    {
        //자동 반복 중지
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

        //자동 반복 재시작
        InvokeRepeating("AutoCardTip", time, time);

    }

    float SetPos()
    {
        // 절반거리를 기준으로 가까운 위치를 반환
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

    void Update()
    {
        if (!isDrag) scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
    }

}
