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
    [HideInInspector] public int timeBetweenAutosaves = 60;
    float autosaveTimer;
    static public Saving Instance;
    PlayerData defaultData;
    FirebaseDatabase database;
    FirebaseUser user;
    [SerializeField] GameObject saveIcon;
    public ShopData shopData = new ShopData();
    bool startUpFocusHasHappend = false;

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
        defaultData.shopData.passiveXpBuffsPurchased = 0;
        defaultData.shopData.shorterPackageTimesPurchased = 0;
        defaultData.shopData.xpDucksPurchased = 0;
        defaultData.shopData.xpFromXpDuckPurchesed = 0;
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
        PlayerData playerData = new PlayerData();

        playerData.displayName = user.DisplayName;
        playerData.coins = GameManager.Instance.coins;
        playerData.xp = GameManager.Instance.xp;
        playerData.xpNeeded = GameManager.Instance.xpNeeded;
        playerData.level = GameManager.Instance.level;
        playerData.UID = user.UserId;
        playerData.shopData = shopData;

        List<Duck> duckObjects = FindObjectsOfType<Duck>().ToList();
        List<GameManager.DuckTypes> ownedDucks = new();
        foreach (Duck duckObject in duckObjects)
        {
            ownedDucks.Add(duckObject.duckType);
        }

        foreach (GameManager.DuckTypes duckType in ownedDucks) //adds all the owned ducks to save data
        {
            playerData.ownedDucks.Add(duckType);
        }

        foreach (var avilableDuck in GameManager.Instance.availableDucksList) //adds all the unowned ducks to save data
        {
            playerData.availableDucks.Add(avilableDuck.duckType);
        }

        database.RootReference.Child("users").Child(user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(playerData)).ContinueWithOnMainThread(task =>
        {
            saveIcon.SetActive(false);
            SaveSettings();
            Debug.Log("Saved");
        });

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
            PassiveXPGain.xpPerSecond = (PassiveXPGain.xpPerSecond * data.shopData.passiveXpBuffsPurchased) + PassiveXPGain.xpPerSecond;
            XpDuck.xpGiven = (XpDuck.xpGiven * data.shopData.xpFromXpDuckPurchesed) + XpDuck.xpGiven;
            GameManager.Instance.boxSpawnTimer -= data.shopData.shorterPackageTimesPurchased;
            GameManager.Instance.amountOfXpDucks = data.shopData.xpDucksPurchased + 1; //+1 because you start with one

            shopData.passiveXpBuffsPurchased = data.shopData.passiveXpBuffsPurchased;
            shopData.shorterPackageTimesPurchased = data.shopData.shorterPackageTimesPurchased;
            shopData.xpDucksPurchased = data.shopData.xpDucksPurchased;
            shopData.xpFromXpDuckPurchesed = data.shopData.xpFromXpDuckPurchesed;


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
        PassiveXPGain.xpPerSecond = 1; // The default 
        XpDuck.xpGiven = 100; // The default 
        database.RootReference.Child("users").Child(user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(defaultData));
        PlayerPrefs.DeleteAll();
        Debug.LogWarning("Save Deleted");
        ReloadScene();
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
        if (startUpFocusHasHappend)
        {
            SaveGame();
        }
        else
        {
            startUpFocusHasHappend = true;
        }
    }
}


public class PlayerData
{
    public string UID = "";
    public int coins = 0;
    public float xp = 0;
    public int xpNeeded = 0;
    public int level = 0;
    public string displayName = "";
    public List<GameManager.DuckTypes> ownedDucks = new();
    public List<GameManager.DuckTypes> availableDucks = new();
    public ShopData shopData = new(); //For some reason this doesnt get pushed to firebase i give up for today
}

[Serializable]
public class ShopData
{
    public int xpDucksPurchased = 0;
    public int xpFromXpDuckPurchesed = 0;
    public int passiveXpBuffsPurchased = 0;
    public int shorterPackageTimesPurchased = 0;
    public bool hasBoughtGoldenDuck = false;
}