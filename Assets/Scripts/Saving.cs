using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class Saving : MonoBehaviour
{
    int amountOfThisDuck;
    [SerializeField, Tooltip("Which ducks to save in addition to the ones in the GameManager array")] Duck[] additionalDucksToCount;
    List<Duck> allDucks = new();
    [HideInInspector] public int timeBetweenAutosaves = 30;
    float autosaveTimer;
    static public Saving instance;
    PlayerData defaultData;
    FirebaseDatabase database;
    FirebaseUser user;


    public void Startup() //Called when GameManager is setup
    {
        SetDefaultData();
        foreach (var item in additionalDucksToCount)
        {
            allDucks.Add(item);
        }
        foreach (var item in GameManager.Instance.duckArray)
        {
            allDucks.Add(item);
        }

        database = FirebaseDatabase.DefaultInstance;
        user = FirebaseAuth.DefaultInstance.CurrentUser;

        LoadGame();
    }

    void SetDefaultData()
    {
        defaultData = new PlayerData();
        defaultData.coins = GameManager.Instance.coins;
        defaultData.xp = GameManager.Instance.xp;
        defaultData.xpNeeded = GameManager.Instance.xpNeeded;
        defaultData.level = GameManager.Instance.level;
        defaultData.ownedDucks.Add(GameManager.DuckTypes.DefaultDuck);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    //void Update()
    //{
    //    autosaveTimer += Time.deltaTime;
    //    if (autosaveTimer >= timeBetweenAutosaves)
    //    {
    //        autosaveTimer = 0;
    //        SaveGame();
    //    }
    //}

    public void SaveGame()
    {
        PlayerData data = new PlayerData();
        data.coins = GameManager.Instance.coins;
        data.xp = GameManager.Instance.xp;
        data.xpNeeded = GameManager.Instance.xpNeeded;
        data.level = GameManager.Instance.level;

        List<Duck> duckObjects = FindObjectsOfType<Duck>().ToList();
        List<GameManager.DuckTypes> ownedDucks = new();
        for (int i = 0; i < duckObjects.Count; i++)
        {
            Duck duckObject = duckObjects[i];
            ownedDucks.Add(duckObject.duckType);
        }

        for (int i = 0; i < ownedDucks.Count; i++) //adds all the owned ducks to save data
        {
            data.ownedDucks.Add(ownedDucks[i]);
        }

        foreach (var duckGameObject in allDucks) //adds all the unowned ducks to save data
        {
            Duck duck = duckGameObject.GetComponent<Duck>();
            if (!ownedDucks.Contains(duck.duckType) && duck.duckType != GameManager.Instance.extraDuck.duckType)
            {
                data.avalibleDucks.Add(duck.duckType);
            }
        }

        //PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(data));
        foreach (var item in data.ownedDucks)
        {
            Debug.Log("Owned ducks: " + item);
        }
        database.RootReference.Child("users").Child(user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(data));

        SaveSettings();
        Debug.Log("Saved");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("autosaveTime", timeBetweenAutosaves);
        PlayerPrefs.SetInt("zoomAllowed", BoolToInt(Settings.instance.allowZoom));
    }

    public void ReloadSceneThenLoadGame()
    {
        ReloadScene();
        LoadGame();
    }

    public void LoadGame()
    {
        PlayerData data = new();
        //Set data from firebase
        database.RootReference.Child("users").Child(user.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
            }

            DataSnapshot snap = task.Result;

            data = JsonUtility.FromJson<PlayerData>(snap.GetRawJsonValue());

            //string defaultDataJson = JsonUtility.ToJson(defaultData);
            //PlayerData data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("SaveData", defaultDataJson));

            if (data == null)
            {
                data = defaultData; //Reference copy, not value copy
            }

            GameManager.Instance.SetCoins(data.coins);
            GameManager.Instance.SetXp(data.xp, data.xpNeeded, data.level);

            for (int i = 0; i < data.ownedDucks.Count; i++)
            {
                GameObject duckToSpawn = GetDuckByDuckType(data.ownedDucks[i]).gameObject;
                Instantiate(duckToSpawn, GameManager.Instance.GenerateRandomPosition(), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
            }

            foreach (var duckType in data.avalibleDucks)
            {
                GameManager.Instance.avalibleDucksList.Add(GetDuckByDuckType(duckType));
            }

            LoadSettings();
            Debug.Log("Loaded Game");
        });
    }

    void LoadSettings()
    {
        timeBetweenAutosaves = PlayerPrefs.GetInt("autosaveTime", 30);
        Settings.instance.allowZoom = IntToBool(PlayerPrefs.GetInt("zoomAllowed", 1));

        Settings.instance.UpdateUIToNewValues();
    }

    public void RemoveAllData()
    {
        SetDefaultData();
        database.RootReference.Child("users").Child(user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(defaultData));
        PlayerPrefs.DeleteAll();
        Debug.LogWarning("Save Deleted");
        ReloadScene();
    }

    void ResetSettings()
    {
        PlayerPrefs.SetInt("autosaveTime", 30);
        PlayerPrefs.SetInt("zoomAllowed", BoolToInt(true));
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    void OnApplicationFocus()
    {
        SaveGame();
    }

    bool IntToBool(int input)
    {
        if (input == 1)
        {
            return true;
        }
        else if (input == 0)
        {
            return false;
        }

        Debug.LogWarning("input was " + input + " expected 1 or 0, returning false");
        return false;
    }

    int BoolToInt(bool input)
    {
        if (input)
        {
            return 1;
        }
        else if (!input)
        {
            return 0;
        }

        Debug.LogWarning("input was " + input + " expected true or false, returning 0");
        return 0;
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
}

public class PlayerData
{
    public int coins;
    public float xp;
    public int xpNeeded;
    public int level;
    public List<GameManager.DuckTypes> ownedDucks = new();
    public List<GameManager.DuckTypes> avalibleDucks = new();
}