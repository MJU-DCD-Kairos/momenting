using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FireStoreScript;
using UnityEngine.UI;
using System;
using System.Text;


namespace ElsePrefab { 
public class ElseProfileManager : MonoBehaviour
{
        public Text txtName;
        public Text txtAge;
        public Text txtIntro;
        public Text txtSex;
        public Text txtMbti;

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Text userName;
    public static string userName2;
    public void ElseProfile()
    {
        //userName.text = userName2;
        GameObject.Find("PrCanvas").transform.Find("GC_Chat_PrCanvas").gameObject.SetActive(true);
            //
        FirebaseManager.ElseData(userName.text);
        Debug.Log(userName.text + "유저네임");
            txtName.text = userName.text;
            txtAge.text = FirebaseManager.ElseAge;
            txtMbti.text = FirebaseManager.ElseMbti;
            txtIntro.text = FirebaseManager.Elseintroduction;
            if (FirebaseManager.ElseSex == 1)
            {
                txtSex.text = "남";
            }
            else
            {
                txtSex.text = "여";
            }
            Debug.Log(txtName.text + txtAge.text + txtMbti.text+ txtIntro.text+ txtSex.text);
        }
        

    }
}