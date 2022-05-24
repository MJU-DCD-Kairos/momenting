using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using FireStoreScript;

public class GoogleSignInDemo : MonoBehaviour
{
    //public static FirebaseFirestore db;
    public string GAA = null;
    public Text infoText;
    public string webClientId = "793745035944-glhfup1hj1am1qk1f9cql7i05mtg573t.apps.googleusercontent.com";
    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
    public bool LCheck = true;
    public string GmailAddress;
    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckFirebaseDependencies();
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);



        //Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //{
        //    //var dependencyStatus = task.Result;
        //    if (task.Result == DependencyStatus.Available)
        //    {
        //        Debug.Log("파이어스토어 DB 연결 성공");
               // db = FirebaseFirestore.DefaultInstance; //Cloud Firestore 인스턴스 초기화

        //    }
        //    else
        //    {
        //        Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
        //    }
        //});

        //FireStoreScript.FirebaseManager.db = null;
    }

    private void CheckFirebaseDependencies()
    {
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        if (task.Result == DependencyStatus.Available)
                    auth = FirebaseAuth.DefaultInstance;
        //        else
        //            AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
        //    }
        //    else
        //    {
        //        AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
        //    }
        //});
    }

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        GAA = PlayerPrefs.GetString("UTken");
        Debug.LogError("#### 구글아이디: " + GAA);
        if (GAA.Equals(""))
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            AddToInformation("Calling SignIn");

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        }
        else
        {
            AddToInformation("이미 로그인 중입니다.");
            SceneManager.LoadScene("Home");
        }
    }


    private void OnSignOut()
    {
        //AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
        LCheck = true;
        SceneManager.LoadScene("Title");
    }

    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
        }
        else
        {
            
            GmailAddress = task.Result.Email.Replace(".","");
            
            PlayerPrefs.SetString("GAddress", GmailAddress);
            AddToInformation(GmailAddress);
            AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            AddToInformation("Email = " + task.Result.Email);
            AddToInformation("" + task.Result.IdToken.Length);
            //AddToInformation("Google ID Token = " + task.Result.IdToken);
            AddToInformation("Email = " + task.Result.Email);
            SignInWithGoogleOnFirebase(task.Result.IdToken);

            
        }
    }
    //public void testdb()
    //{
    //    Dictionary<string, object> Gml = new Dictionary<string, object>
    //    {
    //        {"token","testdb"}, //토큰
    //    };

    //    FireStoreScript.FirebaseManager.db.Collection("userToken").Document("testdb").SetAsync(Gml);
    //}

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                AddToInformation("Sign In Successful.");

        //        Dictionary<string, object> Gml = new Dictionary<string, object>
        //{

        //    {"token", GmailAddress } //토큰
            
        //};
        //        AddToInformation("아무거나");
        //        db.Collection("userInfo").Document(GmailAddress).SetAsync(Gml);
                LCheck = false;
                AddToInformation("실행후");
                SceneManager.LoadScene("SignUp");
            }
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }
    public void save()
    {
        PlayerPrefs.SetString("UTken", GmailAddress);


    }
    private void AddToInformation(string str) { infoText.text += "\n" + str; }
}
