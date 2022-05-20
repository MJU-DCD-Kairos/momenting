using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using FireStoreScript;
using System.Threading;
using System.Threading.Tasks;



public class MatchingManager : MonoBehaviour
{
    public string username; 
    public string sex;

    //public CollectionReference matchingRoomRef;//매칭룸 컬렉션 참조할 변수
    public int count; //전체유저수 저장하기 위해 필요
    public int f_count; //여성유저수 저장하기 위해 필요
    public int m_count; //남성유저수 저장하기 위해 필요
    public string female; //여성유저 들어갈 수 있는지 확인하기 위해 필요 (db에서는 bool값인걸 string으로 변환해줌)
    public string male; //남성유저 들어갈 수 있는지 확인하기 위해 필요 (db에서는 bool값인걸 string으로 변환해줌)
    public string docID; //도큐먼트 고유ID 참조하기 위해 필요
    //public List<string> member;

    void Start()
    {
        username = PlayerPrefs.GetString("name");
        sex = PlayerPrefs.GetString("여");
        Debug.Log("현재 로그인 유저 닉네임 : " + username);
        Debug.Log("현재 로그인 유저 성별 : " + sex);
    }
    public void OnclickMatching() //매칭버튼 눌렀을 때 호출할 함수
    {
        matchingOn();
        //await checkMembers();
    }
    public async void matchingOn()
    {
        CollectionReference mrRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoom"); //매칭룸 컬렉션 참조

        if (sex == "여")
        {
            Query femaleRef = mrRef.WhereEqualTo("female", false).Limit(1); //여성 유저 3명 미만인 방 중에 1개만 반환
            QuerySnapshot snapthot = await femaleRef.GetSnapshotAsync();
            foreach (DocumentSnapshot doc in snapthot.Documents)
            {
                Debug.Log(doc.Id);
                docID = doc.Id;

                Dictionary<string, object> docDictionary = doc.ToDictionary();
               
                count = int.Parse(docDictionary["count"].ToString());
                f_count = int.Parse(docDictionary["f_count"].ToString());
                female = docDictionary["female"].ToString(); //bool 값을 string으로 변환

            }
            Debug.Log("현재 여성유저 3명이상인지: " + female);

            if (female == "False")
            {
                Debug.Log("기존 방 "+ docID + "에 join함");
                
                count = count + 1; //전체유저수 +1 올려주기
                f_count = f_count + 1; //여성유저수 +1 올려주기
                
                if (f_count == 3) //여성유저수가 3명이면
                { 
                    Dictionary<string, object> joinName = new Dictionary<string, object>
                    {
                        {"female", true } //여성유저가 더이상 못들어오게 true로 바꿔줌
                    };
                    await mrRef.Document(docID).UpdateAsync(joinName); //이제 해당방에 여성유저는 더이상 못들어옴
                }

                Dictionary<string, object> newCount = new Dictionary<string, object>
                {
                    {"count" , count },
                    {"f_count" , f_count }
                };
                await mrRef.Document(docID).UpdateAsync(newCount); //전체유저수, 여성유저수 카운트 올려주기
                await mrRef.Document(docID).UpdateAsync("member", FieldValue.ArrayUnion(username)); //member 배열에 유저닉네임 추가
                Debug.Log("업데이트된 전체유저수: " + count + ", 업데이트된 여성유저수" + f_count);
            }
            else
            {
                Debug.Log("새로운 방 만듦");
                
                count = 1; //전체유저수 1
                f_count = 1; //여성유저수 1
                m_count = 0; //남성유저수 0
                makeNewRoom(); //새로운 방 만들기

                Debug.Log("업데이트된 전체유저수: " + count + ", 업데이트된 여성유저수" + f_count);
            }

        }
        else if (sex == "남")
        {
            Query maleRef = mrRef.WhereEqualTo("male", false).Limit(1); //남성 유저 3명 미만인 방 중에 1개만 반환
            QuerySnapshot snapthot = await maleRef.GetSnapshotAsync();
            foreach (DocumentSnapshot doc in snapthot.Documents)
            {
                docID = doc.Id;

                Dictionary<string, object> docDictionary = doc.ToDictionary();

                count = int.Parse(docDictionary["count"].ToString());
                m_count = int.Parse(docDictionary["m_count"].ToString());
                male = docDictionary["male"].ToString(); //bool 값을 string으로 변환

            }
            Debug.Log("현재 남성유저 3명이상인지: " + male);

            if (male == "False")
            {
                Debug.Log("기존 방 " + docID + "에 join함");

                count = count + 1; //전체유저수 +1 올려주기
                m_count = m_count + 1; //남성유저수 +1 올려주기

                if (m_count == 3) //남성유저수가 3명이면
                {
                    Dictionary<string, object> joinName = new Dictionary<string, object>
                    {
                        {"male", true } //남성유저가 더이상 못들어오게 true로 바꿔줌
                    };
                    await mrRef.Document(docID).UpdateAsync(joinName); //이제 해당방에 남성유저는 더이상 못들어옴
                }

                Dictionary<string, object> newCount = new Dictionary<string, object>
                {
                    {"count" , count },
                    {"m_count" , m_count }
                };
                await mrRef.Document(docID).UpdateAsync(newCount); //전체유저수, 남성유저수 카운트 올려주기
                await mrRef.Document(docID).UpdateAsync("member", FieldValue.ArrayUnion(username)); //member 배열에 유저닉네임 추가
                Debug.Log("업데이트된 전체유저수: " + count + ", 업데이트된 남성유저수" + m_count);
            }
            else
            {
                Debug.Log("새로운 방 만듦");

                count = 1; //전체유저수 1
                f_count = 0; //여성유저수 0
                m_count = 1; //남성유저수 1
                makeNewRoom(); //새로운 방 만들기

                Debug.Log("업데이트된 전체유저수: " + count + ", 업데이트된 남성유저수" + m_count);
            }
        }

        //문서의 데이터가 변경될 때마다 전체유저수 불러와서 전체유저수가 6이 되면 매칭종료
        ListenerRegistration listener = mrRef.Document(docID).Listen(task =>
        {
            if (task.Exists)
            {
                Dictionary<string, object> users = task.ToDictionary();
                count = int.Parse(users["count"].ToString());
            }
            else
            {
                Debug.Log(string.Format("문서의 값이 존재하지 않습니다!", task.Id));
            }

            Debug.Log("전체유저수: " + count);
        });
        if (count == 6)
        {
            listener.Stop();
            Debug.Log("매칭종료됨");
        }
    }

    public void checkMembers() //채팅방 6명인지 확인하는 함수
    {
        Debug.Log("checkMembers 함수 실행됨");
        DocumentReference mrDocRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoom").Document(docID);
        //await mrDocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        
        ListenerRegistration listener = mrDocRef.Listen(task =>
        {
            //DocumentSnapshot docSnapshot = task.Result;
            if (task.Exists)
            {
                Dictionary<string, object> users = task.ToDictionary();
                count = int.Parse(users["count"].ToString()); 
            }
            else
            {
                Debug.Log(string.Format("문서의 값이 존재하지 않습니다!", task.Id));
            }
        });
        
        Debug.Log("전체유저수: " + count); 
    }

    void makeNewRoom() //채팅방 생성
    {
        Dictionary<string, object> room = new Dictionary<string, object>
        {
            {"count", count },
            {"f_count", f_count },
            { "m_count", m_count },
            {"female" , false },
            {"male" , false },
            {"member" , new List<object>() { username } }
        };

        FireStoreScript.FirebaseManager.db.Collection("matchingRoom").AddAsync(room); //문서 새로 생성
        //matchingRoomRef.AddAsync(room);
        Debug.Log("채팅방 문서 생성됨");
    }

}
