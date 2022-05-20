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
    public int count;
    public int f_count;
    public int m_count;
    //public bool female = true;
    public string female;
    public bool male;
    public string member;
    public string docID;
    //public List<string> member;

    void Start()
    {
        //username = "나영";
        //sex = "여";
        Debug.Log("현재 로그인 유저 닉네임 : " + username);
        Debug.Log("현재 로그인 유저 성별 : " + sex);
    }
    public void OnclickMatching() //매칭버튼 눌렀을 때 호출할 함수
    {
        matchingOn();
    }
    public async void matchingOn()
    {
        CollectionReference mrRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoom");
        if (sex == "여")
        {
            //Debug.Log(sex);

            Query femaleRef = mrRef.WhereEqualTo("female", false).Limit(1); //여성 유저 2명 이하인 방 중에 1개만 반환
            QuerySnapshot snapthot = await femaleRef.GetSnapshotAsync();
            foreach (DocumentSnapshot doc in snapthot.Documents)
            {
                Debug.Log(doc.Id);
                Dictionary<string, object> docDictionary = doc.ToDictionary();
               
                count = int.Parse(docDictionary["count"].ToString());
                f_count = int.Parse(docDictionary["f_count"].ToString());
                female = docDictionary["female"].ToString();
                //member = docDictionary["member"].ToString();

                //Debug.Log("멤버type: " + docDictionary["member"].GetType());

            }
            Debug.Log("현재 여성유저 3명이상인지: " + female);
            //Debug.Log(member);
            if (female == "False")
            {
                Debug.Log("기존 방에 join함");
                
                count = count + 1; //전체유저수 +1 올려주기
                f_count = f_count + 1; //여성유저수 +1 올려주기
                /*
                if (f_count == 3) //여성유저수가 3명이면
                { 
                    Dictionary<string, object> joinName = new Dictionary<string, object>
                    {
                        {"female", true } //여성유저가 더이상 못들어오게 true로 바꿔줌
                    };
                    await mrRef.Document(docID).UpdateAsync(joinName);
                }

                Dictionary<string, object> newCount = new Dictionary<string, object>
                {
                    {"count" , count },
                    {"f_count" , f_count },
                    {"member", member }
                };
                await mrRef.Document(docID).UpdateAsync(newCount);

                Debug.Log("업데이트된 전체유저수: " + count);
                Debug.Log("업데이트된 여성유저수: " + f_count);
                Debug.Log("업데이트된 여성유저 다 찼는지 여부: " + female);*/
            }
            else
            {
                Debug.Log("새로운 방 만듦");
                
                count = 1; //전체유저수 1
                f_count = 1; //여성유저수 1
                makeNewRoom(); //새로운 방 만들기

                Debug.Log("업데이트된 전체유저수: " + count);
                Debug.Log("업데이트된 여성유저수: " + f_count);
                Debug.Log("업데이트된 여성유저 다 찼는지 여부: " + female);
            }

        }
    }
    public void makeNewRoom() //채팅방 생성
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

    /*
    void matching() //MatchingRoom DB
    {
        if (sex == "여") //유저가 여성이라면
        {
            CollectionReference matchingRoomRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoomRef");
            //if (matchingRoomRef != null)
            //{
            //    Debug.Log("콜렉션 참조 성공");
            //}
            Query femaleRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoomRef").WhereEqualTo("female", false); //여성 유저 2명 이하인 방 중에 1개만 반환 }
            //if (matchingRoomRef != null)
            //{
            //    Debug.Log("쿼리 참조 성공");
            //}

            //Debug.Log("쿼리 반환됨");
            /*
            femaleRef.GetSnapshotAsync().ContinueWithOnMainThread((task) =>
            {
                QuerySnapshot snapshot = task.Result;
                //Debug.Log(snapshot);
                foreach(DocumentSnapshot doc in snapshot.Documents)
                {
                    Debug.Log(doc);
                    Debug.Log("join 가능한 방 : " + doc.Id);

                    //doc.GetValue < "count" > //count의 value 받아와야함 
                    count = count + 1; //현재 전체 유저 수 + 1  }
                    f_count = f_count + 1;
                    member.Add(username);
                    DocumentReference docRef = matchingRoomRef.Document(doc.Id);
                    Dictionary<string, object> newUser = new Dictionary<string, object>
                    {
                        {"count", count}, //전체 유저 수 1 올려주기
                        { "count_f" , f_count}, //여성 유저 수 1 올려주기
                        {"member" , member } //유저리스트에 유저닉네임 추가
                    };
                    docRef.UpdateAsync(newUser); //count 정보 업데이트

                    if (f_count == 3)
                    {
                        Dictionary<string, object> femaleCount = new Dictionary<string, object>
                        {
                            { "female" , true } //여성 유저수가 3명이면 female필드의 value를 true로 바꿔줌
                        };
                        docRef.UpdateAsync(femaleCount); //count 정보 업데이트
                        if (female == true)
                        {
                            count = 1;
                            f_count = 1;
                            m_count = 0;
                            female = false;
                            male = false;

                            makeRoom(); //새로운 방 생성
                        }
                    }
                }

            });
            
            ListenerRegistration listener = femaleRef.Listen(snapshot =>
            {
                Debug.Log(snapshot);
                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    Debug.Log("join 가능한 방 : " + doc.Id);

                    //doc.GetValue < "count" > //count의 value 받아와야함 
                    count = count + 1; //현재 전체 유저 수 + 1  }
                    f_count = f_count + 1;
                    member.Add(username);
                    DocumentReference docRef = matchingRoomRef.Document(doc.Id);
                    Dictionary<string, object> newUser = new Dictionary<string, object>
                    {
                        {"count", count}, //전체 유저 수 1 올려주기
                        { "count_f" , f_count}, //여성 유저 수 1 올려주기
                        {"member" , member } //유저리스트에 유저닉네임 추가
                    };
                    docRef.UpdateAsync(newUser); //count 정보 업데이트

                    if (f_count == 3)
                    {
                        Dictionary<string, object> femaleCount = new Dictionary<string, object>
                        {
                            { "female" , true } //여성 유저수가 3명이면 female필드의 value를 true로 바꿔줌
                        };
                        docRef.UpdateAsync(femaleCount); //count 정보 업데이트
                        if (female == true)
                        {
                            count = 1;
                            f_count = 1;
                            m_count = 0;
                            female = false;
                            male = false;

                            makeRoom(); //새로운 방 생성
                        }
                    }
                    
                }

            });

            listener.Stop();
        }
        else if (sex == "남") //유저가 남성이라면
        {
            CollectionReference matchingRoomRef = FireStoreScript.FirebaseManager.db.Collection("matchingRoomRef");
            Query maleRef = matchingRoomRef.WhereEqualTo("male", false).OrderBy("name").Limit(1); //남성 유저 2명 이하인 방 중에 1개만 반환 }

            ListenerRegistration listener = maleRef.Listen(snapshot =>
            {
                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    Debug.Log("join 가능한 방 : " + doc.Id);

                    //doc.GetValue < "count" > //count의 value 받아와야함 
                    count = count + 1; //현재 전체 유저 수 + 1  }
                    m_count = m_count + 1;
                    member.Add(username);
                    DocumentReference docRef = matchingRoomRef.Document(doc.Id);
                    Dictionary<string, object> newUser = new Dictionary<string, object>
                    {
                        {"count", count}, //전체 유저 수 1 올려주기
                        { "m_count" , m_count}, //남성 유저 수 1 올려주기
                        { "member" , member }
                    };
                    docRef.UpdateAsync(newUser); //count 정보 업데이트

                    if (m_count == 3)
                    {
                        Dictionary<string, object> maleCount = new Dictionary<string, object>
                        {
                            { "male" , true } //남성 유저수가 3명이면 female필드의 value를 true로 바꿔줌
                        };
                        docRef.UpdateAsync(maleCount); //count 정보 업데이트
                    }
                    count = 1;
                    f_count = 0;
                    m_count = 1;
                    female = false;
                    male = false;

                    makeRoom(); //새로운 방 생성

                }

            });

            listener.Stop();
        }
    
        
    }
    */



}
