using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Diagnostics.CodeAnalysis;
using TMPro;

public class GetUserList : MonoBehaviour
{
    [SerializeField] TMP_InputField visitInputField;
    [SerializeField] GameObject accountButtonPrefab;
    FirebaseDatabase database;

    void Start()
    {
        database = FirebaseDatabase.DefaultInstance;
        database.RootReference.Child("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {

            List<PlayerData> pData = new();
            Debug.Log(task.Result.ChildrenCount + " users");
            foreach (var item in task.Result.Children)
            {
                pData.Add(JsonUtility.FromJson<PlayerData>(item.GetRawJsonValue()));
            }
            foreach (var item in pData)
            {
                GameObject spawnedButton = Instantiate(accountButtonPrefab, gameObject.transform);
                AccountButton spawnedAccountData = spawnedButton.GetComponent<AccountButton>();
                spawnedAccountData.SetData(item);
                spawnedAccountData.visitInputField = visitInputField;
            }
        });
    }
}