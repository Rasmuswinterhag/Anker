using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class GetUserList : MonoBehaviour
{
    [SerializeField] GameObject button;
    FirebaseDatabase database;



    void Start()
    {
        AccountButtonData data;
        //TODO: Get all users Displaynames, UID's and levels and display them on on each button, one button/user
        database = FirebaseDatabase.DefaultInstance;
        database.RootReference.Child("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            //TODO: understand how to get each child seperatly so you can set each button's values seperatly
            //TODO: Keep the buttons values (displayname, uid and level) in a component
            //TODO: Create one button for each user
            //TODO: When you click the button put the UID in the "visit input field" and update the value in the script so you just have to press visit
            DataSnapshot snap = task.Result;
            data = JsonUtility.FromJson<AccountButtonData>(snap.GetRawJsonValue());
        });
    }
}

public class AccountButtonData
{
    public string displayName;
    public string UID;
    public int level;
}
