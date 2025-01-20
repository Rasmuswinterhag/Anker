using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public enum DuckTypes
    {
        DefaultDuck,
        NoTextureDuck,
        DevilDuck,
        DemonEggplantDuck,
        AngelApplePieDuck,
        ChonkerDuck,
        Protoduck,
        RocketDuck,
        SunDuck,
        MoonDuck,
        BlackHoleDuck,
        YrgoDuck,
        ClownDuck,
        AngelDuck,
        FemaleDuck,
        CaptainSauceDuck,
    }

    //xp Stuff
    [HideInInspector] public float xp;
    public int xpNeeded = 1000;
    [HideInInspector] public int level;

    [HideInInspector] public int coins;

    //other stuff
    Vector3 midPoint = new Vector3(10.5f, 5f, 0f);
    public static GameManager Instance;

    [Header("References")]
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text coinText;

    [Header("Spawn Positions")]
    [SerializeField] float xMaxCorner;
    [SerializeField] float xMinCorner;

    [SerializeField] float yMaxCorner;
    [SerializeField] float yMinCorner;

    [Header("Box Spawn Time")]
    [SerializeField] int BoxSpawnMax = 30;
    [SerializeField] int BoxSpawnMin = 1;

    [Header("Prefabs")]
    [SerializeField] GameObject xpDuck;
    [SerializeField] GameObject present;
    [SerializeField] GameObject coinPackage;
    public List<Duck> duckArray;
    public List<Duck> avalibleDucksList = new();
    public Duck extraDuck;
    

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

        FindObjectOfType<Settings>().Startup();
        FindObjectOfType<Saving>().Startup();
    }
    private void Start()
    {
        UpdateSlider();
        UpdateSliderMax();

        InvokeRepeating(nameof(SpawnPackage), Random.Range(BoxSpawnMin, BoxSpawnMax), Random.Range(BoxSpawnMin, BoxSpawnMax));

        UpdateCoinText();

        Instantiate(xpDuck, GenerateRandomPosition(), quaternion.identity);
    }

    public void AddXp(float amount)
    {
        xp += amount;

        if (xp >= xpNeeded)
        {
            LevelUp();
        }

        UpdateSlider();
    }

    public void SetXp(float newXp, int newXpNeeded, int newLevel)
    {
        level = newLevel;
        xpNeeded = newXpNeeded;
        xp = newXp;
        UpdateSlider();
        UpdateSliderMax();
    }

    void UpdateSlider()
    {
        slider.value = xp;
    }

    void UpdateSliderMax()
    {
        slider.maxValue = xpNeeded;
    }

    //TODO: Maybe change the amount needed after each level
    void LevelUp()
    {
        level++;
        xp = 0f;
        xpNeeded = level * 1000 + 1;

        UpdateSlider();
        UpdateSliderMax();

        Instantiate(present, midPoint, Quaternion.identity);
    }

    public void SpawnDuckFromPresent(Present tappedPresent)
    {
        int listLegth = avalibleDucksList.Count;


        if (listLegth > 0)
        {
            int randomListIndex = Random.Range(0, listLegth);
            tappedPresent.PlaceDuck(avalibleDucksList[randomListIndex]);
            avalibleDucksList.RemoveAt(randomListIndex);
        }
        else
        {
            tappedPresent.PlaceDuck(extraDuck);
        }

    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinText();
    }

    public void SetCoins(int amount)
    {
        coins = amount;
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        coinText.text = "X " + coins.ToString();
    }

    void SpawnPackage()
    {
        if (FindObjectOfType<CoinPackage>() == null)
        {
            Instantiate(coinPackage, GenerateRandomPosition(), Quaternion.identity);
        }
    }

    public Vector2 GenerateRandomPosition()
    {
        return new Vector2(Random.Range(xMinCorner, xMaxCorner), Random.Range(yMinCorner, yMaxCorner));
    }
}
