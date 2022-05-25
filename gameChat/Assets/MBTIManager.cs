using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public class MBTIManager : MonoBehaviour
{
    public Text header;
    public Text desc;
    public string MbtiType;
    public GameObject noTypeHead;
    public GameObject MyType;

    // Start is called before the first frame update
    void Start()
    {
        MbtiType = PlayerPrefs.GetString("MBTIResert");
        mbtiresult();
    }
    public void mbtiresult() { 
   if (MbtiType == "INTP")
        {
            header.text = "퍼즐 모래알";
            desc.text = "새로운 지식을 갈구하는 아이디어 뱅크예요. 관심 있는 분야에 대한 대화는 언제든 환영!";
        }
        else if (MbtiType == "INTJ")
{
            header.text = "톱니바퀴 모래알";
            desc.text = "상상력이 풍부한 전략가예요. 항상 철두철미한 계획이 있답니다.";
        }
else if (MbtiType == "INFP")
{
            header.text = "햇살반짝 모래알";
            desc.text = "감수성이 풍부한 평화주의자예요. 부끄러움도 많이 타는 타입이니 조심조심 다가와주세요!";
        }
else if (MbtiType == "INFJ")
{
            header.text = "보드라운 모래알";
            desc.text = "화합을 좋아하는 이상주의자예요. 낯을 가리지만 친해지면 재밌는 상상들을 나눠줄게요.";
        }
else if (MbtiType == "ISTP")
{
            header.text = "맥가이버 모래알";
            desc.text = "과묵한 백과사전이에요. 조용하지만 눈치가 빨라 주변 상황 파악을 잘 해요.";
        }
else if (MbtiType == "ISTJ")
{
            header.text = "굳건한 모래알";
            desc.text = "현실 감각이 좋은 나무예요. 굳건하고 용의단정한 모범적인 타입입니다.";
        }
else if (MbtiType == "ISFP")
{
            header.text = "내일 모래알";
            desc.text = "호기심이 많은 예술가예요. 인간관계에 힐링하고 인간관계에 상처받을 수 있어요.";
        }
else if (MbtiType == "ISFJ")
{
            header.text = "용감무쌍 모래알";
            desc.text = "부드럽지만 단호한 방패예요. 진솔하고 가볍지 않기 때문에 관계 맺기 가장 믿음직스러운 타입이에요.";
        }
else if (MbtiType == "ENTP")
{
            header.text = "뜨거운 감자 모래알";
            desc.text = "수줍은 성격 안에 뜨거운 열정이 있어요. 좋아하는 것이 생기면 거기에 초집중!";
        }
else if (MbtiType == "ENTJ")
{
            header.text = "나를 따르라 모래알";
            desc.text = "강한 의지의 리더형이에요. 항상 최선과 차선이 준비돼 있답니다.";
        }
else if (MbtiType == "ENFP")
{
            header.text = "방울방울 모래알";
            desc.text = "활기 넘치는 톡톡이에요. 온정적이고 통찰력이 좋아 사람들을 잘 도와줘요.";
        }
else if (MbtiType == "ENFJ")
{
            header.text = "카리스마 모래알";
            desc.text = "능수능란한 슬라임이에요. 어떤 상황이든 융통성있게 녹아든답니다.";
        }
else if (MbtiType == "ESTP")
{
            header.text = "모험가 모래알";
            desc.text = "모험을 즐기는 활동가예요. 관대하고 선입견이 없어 삶의 모든 걸 즐기는 타입이에요.";
        }
else if (MbtiType == "ESTJ")
{
            header.text = "팩트체크 모래알";
            desc.text = "엄격한 관리자예요. 무척 철저하고 엄격해 보일 수도 있지만 주변 사람에게 의리가 넘친답니다.";
        }
else if (MbtiType == "ESFP")
{
            header.text = "바람따라 모래알";
            desc.text = "자유로운 영혼의 소유자예요.낙천적이고 센스와 유머가 좋아 밝고 재미있는 타입이에요.";
        }
else if (MbtiType == "ESFJ")
{
            header.text = "어깨동무 모래알";
            desc.text = "인화를 중시하는 타고난 협력자예요. 다른 사람들을 잘 돕고 정이 많아 사람을 좋아해요.";
        }
        else if (MbtiType == "")
        {
            noTypeHead.SetActive(true);
            MyType.SetActive(false);
        }
    }
}
