using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirebaseAuthUIManager : MonoBehaviour
{
    [Header("Sign In Components")]
    public TMP_InputField l_usernameText;
    public TMP_InputField l_passwordText;

    [Space(20f)]
    [Header("Sign Up Components")]
    public TMP_InputField r_usernameText;
    public TMP_InputField r_passwordText;
    public TMP_InputField r_passwordAgainText;
    public TMP_InputField r_emailText;
    public TMP_InputField r_fullNameText;

    void Start()
    {

    }

    void Update()
    {

    }

    public async void Login()
    {
        PopupManager.instance.ShowLoadingPopup();

        if (l_usernameText.text.Contains("@"))
            FirebaseAuthManager.instance.LoginWEmail(l_usernameText.text, l_passwordText.text);
        else
        {
            string email = await FireStoreManager.instance.getMailFromUsernameAsync(l_usernameText.text);
            FirebaseAuthManager.instance.LoginWEmail(email, l_passwordText.text);
        }
    }

    public void Register()
    {
        PopupManager.instance.ShowLoadingPopup();

        if (r_passwordAgainText.text != r_passwordText.text) { Debug.Log("Passwords does not match."); return; }

        FirebaseAuthManager.instance.Register(r_fullNameText.text, r_emailText.text, r_usernameText.text, r_passwordText.text);

    }
}
