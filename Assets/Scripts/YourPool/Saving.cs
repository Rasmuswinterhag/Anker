using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tools;
using Random = UnityEngine.Random;
public class Saving : MonoBehaviour
{
    [SerializeField, Tooltip("Which ducks to save in addition to the ones in the GameManager array")] Duck[] additionalDucksToCount;
    List<Duck> allDucks = new();
    [HideInInspector] public int timeBetweenAutosaves = 30;
    float autosaveTimer;
    static public Saving Instance;
    PlayerData defaultData;
    FirebaseDatabase database;
    FirebaseUser user;
    [SerializeField] GameObject saveIcon;


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
        defaultData.displayName = "No Name";
        defaultData.coins = 0;
        defaultData.xp = 0;
        defaultData.xpNeeded = 1000;
        defaultData.level = 0;
        defaultData.ownedDucks.Add(GameManager.DuckTypes.DefaultDuck);
        defaultData.UID = "Default User Id";
        foreach (var item in GameManager.Instance.duckArray)
        {
            defaultData.availableDucks.Add(item.duckType);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        autosaveTimer += Time.deltaTime;
        if (autosaveTimer >= timeBetweenAutosaves)
        {
            autosaveTimer = 0;
            SaveGame();
        }
    }

    public void SaveGame()
    {
        saveIcon.SetActive(true);
        PlayerData data = new PlayerData();
        data.displayName = user.DisplayName;
        data.coins = GameManager.Instance.coins;
        data.xp = GameManager.Instance.xp;
        data.xpNeeded = GameManager.Instance.xpNeeded;
        data.level = GameManager.Instance.level;
        data.UID = user.UserId;

        List<Duck> duckObjects = FindObjectsOfType<Duck>().ToList();
        List<GameManager.DuckTypes> ownedDucks = new();
        foreach (Duck duckObject in duckObjects)
        {
            ownedDucks.Add(duckObject.duckType);
        }

        foreach (GameManager.DuckTypes duckType in ownedDucks) //adds all the owned ducks to save data
        {
            data.ownedDucks.Add(duckType);
        }

        foreach (var avilableDuck in GameManager.Instance.availableDucksList) //adds all the unowned ducks to save data
        {
            data.availableDucks.Add(avilableDuck.duckType);
        }

        database.RootReference.Child("users").Child(user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(data));

        SaveSettings();
        Debug.Log("Saved");
        saveIcon.SetActive(false);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("autosaveTime", timeBetweenAutosaves);
        PlayerPrefs.SetInt("zoomAllowed", TranslateValues.BoolToInt(Settings.Instance.allowZoom));
        Debug.Log("Saved settings");
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

            if (data == null)
            {
                data = defaultData; //Reference copy, not value copy
            }

            GameManager.Instance.SetCoins(data.coins);
            GameManager.Instance.SetXp(data.xp, data.xpNeeded, data.level);

            for (int i = 0; i < data.ownedDucks.Count; i++)
            {
                GameObject duckToSpawn = Duckstuff.GetDuckByDuckType(data.ownedDucks[i], allDucks.ToArray()).gameObject;
                Instantiate(duckToSpawn, MyRandom.RandomPosition(GameManager.Instance.minPos, GameManager.Instance.maxPos), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
            }

            if (data.availableDucks.Count > 0)
            {
                foreach (var duckType in data.availableDucks)
                {
                    GameManager.Instance.availableDucksList.Add(Duckstuff.GetDuckByDuckType(duckType, allDucks));
                }
            }
            else
            {
                foreach (var defaultDataDuckType in defaultData.availableDucks)
                {
                    GameManager.Instance.availableDucksList.Add(Duckstuff.GetDuckByDuckType(defaultDataDuckType, allDucks));
                }
            }

            LoadSettings();
            Debug.Log("Loaded Game");
        });
    }

    void LoadSettings()
    {
        timeBetweenAutosaves = PlayerPrefs.GetInt("autosaveTime", 60);
        Settings.Instance.allowZoom = TranslateValues.IntToBool(PlayerPrefs.GetInt("zoomAllowed", 1));

        Settings.Instance.UpdateUIToNewValues();
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
        PlayerPrefs.SetInt("zoomAllowed", TranslateValues.BoolToInt(true));
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
}


public class PlayerData
{
    public string UID;
    public int coins;
    public float xp;
    public int xpNeeded;
    public int level;
    public string displayName;
    public List<GameManager.DuckTypes> ownedDucks = new();
    public List<GameManager.DuckTypes> availableDucks = new();
}