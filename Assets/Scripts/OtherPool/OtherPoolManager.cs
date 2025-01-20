using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class OtherPoolManager : MonoBehaviour
{
    FirebaseDatabase database;
    [SerializeField] Duck[] allDucks;
    string idInput;

    [Header("Spawn Positions")]
    [SerializeField] float xMaxCorner;
    [SerializeField] float xMinCorner;

    [SerializeField] float yMaxCorner;
    [SerializeField] float yMinCorner;
    FirebaseUser visitedUser;

    void Start()
    {
        database = FirebaseDatabase.DefaultInstance;
    }

    public void UpdateIdInput(string input)
    {
        idInput = input;
    }

    public void LoadFromInput()
    {
        LoadPool(idInput);
        //TODO: Get visited users displayname and show it on screen
        //TODO: Hide visit screen when sucsessfully visited someone
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

            for (int i = 0; i < data.ownedDucks.Count; i++)
            {
                GameObject duckToSpawn = GetDuckByDuckType(data.ownedDucks[i]).gameObject;
                GameObject spawnedDuck = Instantiate(duckToSpawn, GenerateRandomPosition(), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
                Destroy(spawnedDuck.GetComponent<PassiveXPGain>());

            }
            Debug.Log("Loaded " + userId);
        });
    }

    public Duck GetDuckByDuckType(GameManager.DuckTypes DuckType)
    {
        //return allDucks.Find(duck => duck.duckType == DuckType);
        foreach (var duck in allDucks)
        {
            if (duck.duckType == DuckType)
            {
                return duck;
            }
        }
        return null;
    }

    public Vector2 GenerateRandomPosition()
    {
        return new Vector2(Random.Range(xMinCorner, xMaxCorner), Random.Range(yMinCorner, yMaxCorner));
    }
}