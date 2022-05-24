using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameChatSample;
using UnityEngine.UI;
using FireStoreScript;
using Firebase.Firestore;
using groupchatManager;

public enum Toggles { user0, user1, user2, user3, user4 }
public enum Reasons { reason1 , reason2, reason3, reason4 }

public class Report : MonoBehaviour
{
    public Toggles currentToggle;
    public Reasons selectReason;

    public Text user1; //라디오버튼 옆에 유저 닉네임 넣기
    public Text user2; //라디오버튼 옆에 유저 닉네임 넣기
    public Text user3; //라디오버튼 옆에 유저 닉네임 넣기
    public Text user4; //라디오버튼 옆에 유저 닉네임 넣기
    public Text user5; //라디오버튼 옆에 유저 닉네임 넣기
    public int userIdx; //리스트에서 인덱스 찾기 위해 참조

    public Text SelectedUser; //다이얼로그에서 선택한 유저닉네임 들어갈 텍스트 컴포넌트
    public Text reasonEtc;

    public string reason;

    private string reason1 = "욕설,비매너발언";
    private string reason2 = "도배, 광고성메시지";
    private string reason3 = "도용, 허위프로필";
        
    public static Report instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else if (instance != null) return;
        getUserList();
    }

    public void getUserList()
    {
        user1.text = groupchatSceneManager.chatRoom[2];
        user2.text = groupchatSceneManager.chatRoom[3];
        user3.text = groupchatSceneManager.chatRoom[4];
        user4.text = groupchatSceneManager.chatRoom[5];
        user5.text = groupchatSceneManager.chatRoom[6];

    }
    
    public void Report_SelectedUser()
    {
        if(currentToggle == Toggles.user0) 
        {   userIdx = 0; 
            Debug.Log(userIdx + "번 유저 선택");
            SelectedUser.text = user1.text;
        }
        else if (currentToggle == Toggles.user1) 
        {   userIdx = 1; 
            Debug.Log(userIdx + "번 유저 선택");
            SelectedUser.text = user2.text;
        }
        else if (currentToggle == Toggles.user2) 
        {   userIdx = 2; 
            Debug.Log(userIdx + "번 유저 선택");
            SelectedUser.text = user3.text;
        }
        else if (currentToggle == Toggles.user3) 
        {   userIdx = 3; 
            Debug.Log(userIdx + "번 유저 선택");
            SelectedUser.text = user4.text;
        }
        else if (currentToggle == Toggles.user4) 
        {   userIdx = 4; 
            Debug.Log(userIdx + "번 유저 선택");
            SelectedUser.text = user5.text;
        }

        
    }

    public void SelectReason()
    {
        if (selectReason == Reasons.reason1) 
        {
            reason = reason1;
        }
        else if (selectReason == Reasons.reason2)
        {
            reason = reason2;
        }
        else if (selectReason == Reasons.reason3)
        {
            reason = reason3;
        }
        else if (selectReason == Reasons.reason4)
        {
            reason = reasonEtc.text;
        }

    }
    public async void SaveReportData() //선택한 유저닉네임, 사유 db에 저장 
    {
        SelectReason();

        Dictionary<string, object> reportData = new Dictionary<string, object>
        {
            { "name", SelectedUser.text },
            { "reason", reason },
            { "처리여부" , false },
            { "RoomName" , gameSceneManager.chatRname}
        };
        await FirebaseManager.db.Collection("report").Document(NewChatManager.username).UpdateAsync("report", FieldValue.ArrayUnion(reportData));
        
    }
}



