using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class Saving : MonoBehaviour
{
    int amountOfThisDuck;
    [SerializeField, Tooltip("Which ducks to save in addition to the ones in the GameManager array")] GameObject[] additionalDucksToCount;
    List<GameObject> allDucks = new();
    [HideInInspector] public int timeBetweenAutosaves = 30;
    float autosaveTimer;
    static public Saving instance;

    void Start()
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

    public void Startup() //Called when GameManager is setup
    {
        foreach (var item in additionalDucksToCount)
        {
            allDucks.Add(item);
        }
        foreach (var item in GameManager.Instance.duckArray)
        {
            allDucks.Add(item);
        }

        LoadGame();
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
        //Save Ducks
        for (int i = 0; i < allDucks.Count; i++)
        {
            foreach (var duck in FindObjectsOfType<Duck>())
            {
                if (duck.duckType == allDucks[i].GetComponent<Duck>().duckType)
                {
                    amountOfThisDuck++;
                }
            }
            PlayerPrefs.SetInt("AmountOfDuck#" + i, amountOfThisDuck);
            //Debug.Log("Saved " + amountOfThisDuck + " ducks of type #" + i);
            amountOfThisDuck = 0;
        }
        PlayerPrefs.SetInt("Coins", GameManager.Instance.coins);
        PlayerPrefs.SetFloat("Xp", GameManager.Instance.xp);
        PlayerPrefs.SetInt("XpNeeded", GameManager.Instance.xpNeeded);
        PlayerPrefs.SetInt("Level", GameManager.Instance.level);

        SaveSettings();
        Debug.Log("Saved");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("autosaveTime", timeBetweenAutosaves);
        PlayerPrefs.SetInt("zoomAllowed", BoolToInt(Settings.instance.allowZoom));
    }

    public void LoadGame()
    {
        //Load Values
        GameManager.Instance.AddCoins(PlayerPrefs.GetInt("Coins", 0));
        GameManager.Instance.SetXp(PlayerPrefs.GetFloat("Xp", 0), PlayerPrefs.GetInt("XpNeeded", GameManager.Instance.xpNeeded), PlayerPrefs.GetInt("Level", 0));

        //Load Ducks
        for (int i = 0; i < allDucks.Count; i++)
        {
            int amountToSpawn = PlayerPrefs.GetInt("AmountOfDuck#" + i, 0);
            //Debug.Log("there are " + amountToSpawn + " ducks to load of type #" + i);
            for (int j = 0; j < amountToSpawn; j++) //To spawn multiple of the same duck
            {
                Instantiate(allDucks[i], GameManager.Instance.GenerateRandomPosition(), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
            }

            if (amountToSpawn <= 0 && allDucks[i] != GameManager.Instance.extraDuck)
            {
                GameManager.Instance.avalibleDucksList.Add(allDucks[i]);
            }
        }

        for (int i = 0; i < GameManager.Instance.avalibleDucksList.Count; i++) //Removes nulls from avalibleDucksList
        {
            if (GameManager.Instance.avalibleDucksList[i] == null)
            {
                GameManager.Instance.avalibleDucksList.RemoveAt(i);
            }
        }

        LoadSettings();

        Debug.Log("Loaded Game");

    }

    void LoadSettings()
    {
        timeBetweenAutosaves = PlayerPrefs.GetInt("autosaveTime", 30);
        Settings.instance.allowZoom = IntToBool(PlayerPrefs.GetInt("zoomAllowed", 1));

        Settings.instance.UpdateUIToNewValues();
    }

    public void RemoveAllData()
    {
        PlayerPrefs.DeleteAll();
        Debug.LogWarning("Save Deleted");
        SetDefaultData();
        ReloadScene();
    }

    void SetDefaultData()
    {
        PlayerPrefs.SetInt("AmountOfDuck#" + 0, 1); //Always start with one default duck
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
}
