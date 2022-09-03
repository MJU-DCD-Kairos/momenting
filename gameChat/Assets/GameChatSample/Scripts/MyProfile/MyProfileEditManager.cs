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

        //���õ� Ű���� ���� ����
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

        //������ ���� �ε� �� ���� �ҷ�����
        public void Load_introduction()
        {
            txtPlaceholder.text = FirebaseManager.myintroduction;
            txtIntro_edit.text = FirebaseManager.myintroduction;
        }
        
        //������ ����
        public async void Edit_Save()
        {
            DocumentReference userRef = FirebaseManager.db.Collection("userInfo").Document(FirebaseManager.GCN);
            //Dictionary<string, object> addRQ = new Dictionary<string, object> 
            //        {
            //            { "Info" ,  }, //���ټҰ�
            //        };

            await userRef.UpdateAsync("Introduction", txtIntro_edit.text);
            Debug.Log("����Ϸ�!");
            EDIT_INTRO = txtIntro_edit.text;
            Debug.Log("edit �� == " + EDIT_INTRO);
        }


        //������ Ű������ ���ڸ� �޾ƿ� UI�� ����ִ� �ڵ�
        public void getKWnum()
        {
            IndicatorNum.text = getKeywordList.KWcheckCount.ToString();
        }

        
    }
}
