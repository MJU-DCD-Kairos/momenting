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

public class GoogleSignInDemo : MonoBehaviour
{
    public string GAA;
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
        GAA = PlayerPrefs.GetString("GoogleID");
    }
  
    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    auth = FirebaseAuth.DefaultInstance;
                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        if (GAA == null) {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            AddToInformation("Calling SignIn");

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        }
        else if ( GAA != null)
        {
            AddToInformation("이미 로그인 중입니다.");
            SceneManager.LoadScene("Home");
        }
    }


    private void OnSignOut()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
        LCheck = true;
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
            GmailAddress = task.Result.Email;
            AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            AddToInformation("Email = " + task.Result.Email);
            AddToInformation("Google ID Token = " + task.Result.IdToken);
            AddToInformation("Email = " + task.Result.Email);
            SignInWithGoogleOnFirebase(task.Result.IdToken);
            LCheck = false;
            SceneManager.LoadScene("SignUp");
        }
    }
    
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
        PlayerPrefs.SetString("GoogleID", GmailAddress);


    }
    private void AddToInformation(string str) { infoText.text += "\n" + str; }
}
