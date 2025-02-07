using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Tools;
using TMPro;
using UnityEngine.SceneManagement;

public class OtherPoolManager : MonoBehaviour
{
    FirebaseDatabase database;
    string idInput;

    [Header("Spawn Positions")]
    [SerializeField] Vector2 minPos;
    [SerializeField] Vector2 maxPos;

    [Header("References")]
    [SerializeField] Duck[] allDucks;
    [SerializeField] TMP_Text poolUsersName;
    [SerializeField] GameObject joinMenu;

    void Start()
    {
        joinMenu.SetActive(true);
        poolUsersName.gameObject.SetActive(false);
        database = FirebaseDatabase.DefaultInstance;
    }

    public void UpdateIdInput(string input)
    {
        idInput = input;
    }

    public void LoadFromInput()
    {
        LoadPool(idInput);
    }

    public void LoadPool(FirebaseUser user)
    {
        LoadPool(user.UserId);
    }

    public void LoadPool(string userId)
    {
        PlayerData data = new();

        //Set data from firebase
        database.RootReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {

            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
            }

            DataSnapshot snap = task.Result;

            data = JsonUtility.FromJson<PlayerData>(snap.GetRawJsonValue());

            if (data == null)
            {
                Debug.LogError("No Data To Load"); //Reference copy, not value copy
                return;
            }

            poolUsersName.text = data.displayName;

            for (int i = 0; i < data.ownedDucks.Count; i++)
            {
                GameObject duckToSpawn = Duckstuff.GetDuckByDuckType(data.ownedDucks[i], allDucks).gameObject;
                GameObject spawnedDuck = Instantiate(duckToSpawn, MyRandom.RandomPosition(minPos, maxPos), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
                Destroy(spawnedDuck.GetComponent<PassiveXPGain>());
            }
            //Debug.Log("Loaded " + data.displayName + "'s pool (" + userId + ")");
            joinMenu.SetActive(false);
            poolUsersName.gameObject.SetActive(true);
            Debug.LogFormat("Loaded {0}'s pool ({1})", data.displayName, userId);
        });
    } 

    public void GoHome()
    {
        SceneManager.LoadScene(1);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}