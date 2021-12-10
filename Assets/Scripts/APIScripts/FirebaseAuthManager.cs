using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuthManager instance;

    FirebaseAuth auth;
    FirebaseUser user;

    Dictionary<string, string> UserDataToUpload = new Dictionary<string, string>
    {
        {"Bio", "Hey NekoShare!" },
    };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        DontDestroyOnLoad(gameObject);

        //INITIALIZE FIREBASE
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChangedAsync;
        //AuthStateChangedAsync(this, null);
    }

    private async void AuthStateChangedAsync(object sender, System.EventArgs e)
    {
        bool signedin = auth.CurrentUser != user && auth.CurrentUser != null;

        if (!signedin && user != null)
        {
            Debug.Log("Signed out.");
        }

        user = auth.CurrentUser;
        if (auth.CurrentUser == null) {
            PopupManager.instance.HideLoadingPopup();
            return;
        }

        if (user != null)
        {
            if (!user.IsEmailVerified && user.ProviderId == EmailAuthProvider.ProviderId)
            {
                Debug.Log("Email not verified.");
                await SendVerificationEmail();
                auth.SignOut();
                return;
            }
            Debug.Log($"170 Signed in as {user.DisplayName} and User ID is {user.UserId}.");
            //loginSuccessful(user.UserId, user.DisplayName, user.Email);
        }

        PopupManager.instance.HideLoadingPopup();
    }

    private async Task SendVerificationEmail()
    {
        FirebaseUser _user = auth.CurrentUser;
        print(auth.CurrentUser.Email);
        if (_user != null)
        {
            await _user.ReloadAsync();
            await _user.SendEmailVerificationAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendEmailVerificationAsync was canceled.");
                    Debug.Log("Email Verification Cancelled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                    Debug.Log("Email Verification failed.");
                    return;
                }

                Debug.Log("Email sent successfully.");
            });
        }
    }

    public async void LoginWEmail(string _email, string _password)
    {
        await auth.SignInWithEmailAndPasswordAsync(_email, _password).ContinueWithOnMainThread(task =>
        {

            if (task.IsCanceled)
            {
                Debug.Log("Signing in canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                FirebaseException firebaseException = (FirebaseException)task.Exception.GetBaseException();
                AuthError authError = (AuthError)firebaseException.ErrorCode;
                Debug.Log("285: " + authError);
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        Debug.Log("Invalid Email");
                        break;
                    case AuthError.WrongPassword:
                        Debug.Log("Wrong Password");
                        break;
                    case AuthError.MissingEmail:
                        Debug.Log("Please enter your email.");
                        break;
                    case AuthError.MissingPassword:
                        Debug.Log("Please enter your password.");
                        break;
                    case AuthError.UserNotFound:
                        Debug.Log("User not found");
                        break;
                    case AuthError.UserDisabled:
                        Debug.Log("Your account is disabled.");
                        break;
                    case AuthError.NetworkRequestFailed:
                        Debug.Log("Check your connection.");
                        break;
                    default:
                        Debug.Log("Error code is: " + authError);
                        break;
                }
                return;
            }

            PlayerPrefs.SetString("password", _password);
            PlayerPrefs.Save();
            Debug.Log("Login Succesfull");
        });
    }

    public async void Register(string _fullname, string _email, string _username, string _password)
    {
        await auth.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWithOnMainThread(async task =>
        {

            if (task.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)task.Exception.GetBaseException();
                AuthError authError = (AuthError)firebaseException.ErrorCode;
                Debug.Log("Unknown Error line:312");

                switch (authError)
                {
                    case AuthError.EmailAlreadyInUse:
                        Debug.Log("This email is already in use.");
                        break;
                    case AuthError.InvalidEmail:
                        Debug.Log("Invalid Email.");
                        break;
                    case AuthError.WeakPassword:
                        Debug.Log("Weak Password.");
                        break;
                    case AuthError.MissingEmail:
                        Debug.Log("Missing Email.");
                        break;
                    case AuthError.MissingPassword:
                        Debug.Log("Missing Password.");
                        break;
                    case AuthError.NetworkRequestFailed:
                        Debug.Log("Please check your connection.");
                        break;
                    default:
                        Debug.Log("Error is: " + authError.GetType());
                        break;
                }
            } else
            {
                Debug.Log("Register Successful.");
                await FireStoreManager.instance.setMailWithUsernameAsync(_username, _email);
                Debug.Log("asd");
            }
            PopupManager.instance.HideLoadingPopup();
        });
    } 
        
    void OnDestroy()
    {
        auth.SignOut();
        auth.StateChanged -= AuthStateChangedAsync;
        auth = null;
    }
}