using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameChatUnity;using System.Collections.Generic;
using UnityEngine;



namespace GameChatSample
{
    public class SampleLoginManager : MonoBehaviour
    {
        [SerializeField]
        GameObject PopupRoot;

        [SerializeField]
        GameObject LoadingPanel;

        [SerializeField]
        string initProjectID;




        PopupManager p_manager;
        public Text username;
        public string GCN;
        #region LifeCycle

        void Awake()
        {
            p_manager = new PopupManager();

            string init_project = "";
            init_project = initProjectID.Trim();


            GameChat.initialize(init_project);

            
        }
        void Start()
        {
            
            GCN = "";
            GCN = PlayerPrefs.GetString("GCName");
            
            if (GCN != "")
            {
                Debug.Log("로됨로됨");
                goHomerightaway();
            }
            else
            {
                Debug.Log("로그인안돼있어용");
            }
        }

        public void setsds()
        {
          // PlayerPrefs.SetString("GCName","please");
        }
        public void gogohome()
        {
            SceneManager.LoadScene("Title");
        }



        private void OnEnable()
        {
            Debug.Log("[OnEnable] Login Scene");
            GameChat.dispatcher.onConnected += onConnected;
            GameChat.dispatcher.onErrorReceived += onErrorReceived;
        }

        private void OnDisable()
        {
            Debug.Log("[OnDisable] Login Scene");
            GameChat.dispatcher.onConnected -= onConnected;
            GameChat.dispatcher.onErrorReceived -= onErrorReceived;
        }
        #endregion


        #region EventListener

        void onConnected(string messge)
        {
            SampleGlobalData.G_isSocketConnected = true;

            CustomizedPopup.PopupButtonInfo[] btn_info = new CustomizedPopup.PopupButtonInfo[1];
            btn_info[0].callback = () =>
            {
                LoadingPanel.SetActive(false);
                SceneManager.LoadScene("Home");
            };
            p_manager.ShowCustomPopup(PopupRoot, "GameChatPopup_BtnOne", "Connect Success !", "로그인이 완료되었습니다.", btn_info);
            Debug.LogError("##### 로그인 완료");

        }

        void onErrorReceived(string message, GameChatException exception)
        {
            CustomizedPopup.PopupButtonInfo[] b_info = new CustomizedPopup.PopupButtonInfo[1];
            b_info[0].callback = () =>
            {
                LoadingPanel.SetActive(false);
            };
            p_manager.ShowCustomPopup(PopupRoot, "GameChatPopup_BtnOne", "Login Failed!", "Error - " + exception.ToJson(), b_info);
        }

        #endregion



        #region UI

        public void ClickLoginButton(Text user_id)
        {
            string opt_user_id = user_id.text.Trim();

            if (string.IsNullOrEmpty(opt_user_id))
            {
                p_manager.ShowCustomPopup(PopupRoot, "GameChatPopup_BtnOne", "Login Failed", "로그인 아이디를 입력해주세요", new CustomizedPopup.PopupButtonInfo[0]);
                return;
            }

            LoadingPanel.SetActive(true);

            GameChat.connect(opt_user_id, (Member user, GameChatException exception) =>
            {
                if (exception != null)
                {
                    CustomizedPopup.PopupButtonInfo[] b_info = new CustomizedPopup.PopupButtonInfo[1];
                    b_info[0].callback = () =>
                    {
                        LoadingPanel.SetActive(false);
                    };
                    p_manager.ShowCustomPopup(PopupRoot, "GameChatPopup_BtnOne", "Login Failed!", "Error - " + exception.ToJson(), b_info);
                    return;
                }
                SampleGlobalData.G_isConnected = true;
                SampleGlobalData.G_User = user;
            });
        }

        public void goHomerightaway()
        {
            string opt_user_id = GCN.Trim();



            GameChat.connect(opt_user_id, (Member user, GameChatException exception) =>
            {
                
                SampleGlobalData.G_isConnected = true;
                SampleGlobalData.G_User = user;

                SceneManager.LoadScene("Home");
            });           
        }
        #endregion


        
    }
}
