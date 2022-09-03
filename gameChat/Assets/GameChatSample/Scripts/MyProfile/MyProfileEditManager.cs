using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FireStoreScript;
using Firebase.Firestore;
using System.Threading.Tasks;
using myprofile;

namespace editprofile
{
    public class MyProfileEditManager : MonoBehaviour
    {
        
        public Text txtIntro_edit;
        public Text txtPlaceholder;
        public static string EDIT_INTRO;

        //선택된 키워드 개수 노출
        public Text IndicatorNum;
        public GameObject Save;
        public Button SaveBtn;

        private void Update()
        {
            getKWnum();

        }

        private void Start()
        {
            //SaveBtn.onClick.AddListener(KWedit);
            
        }

        //페이지 최초 로드 시 정보 불러오기
        public void Load_introduction()
        {
            txtPlaceholder.text = FirebaseManager.myintroduction;
            txtIntro_edit.text = FirebaseManager.myintroduction;
        }
        
        //프로필 편집
        public async void Edit_Save()
        {
            DocumentReference userRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN);
            //Dictionary<string, object> addRQ = new Dictionary<string, object> 
            //        {
            //            { "Info" ,  }, //한줄소개
            //        };

            await userRef.UpdateAsync("Introduction", txtIntro_edit.text);
            Debug.Log("저장완료!");
            EDIT_INTRO = txtIntro_edit.text;
            Debug.Log("edit 씬 == " + EDIT_INTRO);
        }


        //선택한 키워드의 숫자를 받아와 UI에 띄워주는 코드
        public void getKWnum()
        {
            IndicatorNum.text = getKeywordList.KWcheckCount.ToString();
        }

        
    }
}
