using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FireStoreManager : MonoBehaviour
{

    public static FireStoreManager instance;
    FirebaseFirestore firestore;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        firestore = FirebaseFirestore.DefaultInstance;
    }

    /*public async Task<string> getDocumentAsync()
    {
        object emailGot = "";

        Query getEmailQuery = firestore.Collection("users").WhereEqualTo("username", _username);
        await getEmailQuery.GetSnapshotAsync().ContinueWith(task =>
        {
            QuerySnapshot querySnaphot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in querySnaphot.Documents)
            {
                Dictionary<string, object> user = documentSnapshot.ToDictionary();
                user.TryGetValue("email", out emailGot);
            }
        });
        return (string)emailGot;
    }*/

    public async Task<string> getMailFromUsernameAsync(string _username)
    {
        object emailGot = "";
        DocumentReference docRef = firestore.Collection("usermailinfo").Document(_username);
        await docRef.GetSnapshotAsync().ContinueWith(task => {
            DocumentSnapshot docSnapshot = task.Result;
            if (!docSnapshot.Exists) return;

            Dictionary<string, object> user = docSnapshot.ToDictionary();
            user.TryGetValue("email", out emailGot);
        });
        return (string)emailGot;
    }

    public async Task setMailWithUsernameAsync(string _username, string _mail)
    {
        Debug.Log("ıkodsa");
        DocumentReference docRef = firestore.Collection("usermailinfo").Document(_username);
        await docRef.SetAsync(new Dictionary<string, object>() { { "email", _mail } }).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the LA document in the cities collection.");
        });
        Debug.Log("lşidlsaf");
    }
}
