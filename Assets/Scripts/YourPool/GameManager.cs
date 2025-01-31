using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Tools;

using Random = UnityEngine.Random;
using System.Security.Cryptography;
using UnityEngine.Tilemaps;

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

    [HideInInspector] public float xp;
    [HideInInspector] public int xpNeeded;
    [HideInInspector] public int level;
    [HideInInspector] public int coins;

    //other stuff
    Vector3 midPoint = new Vector3(10.5f, 5f, 0f);
    public static GameManager Instance;
    [SerializeField] float boxSpawnTimer = 30;
    float boxTimer;

    [Header("References")]
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text coinText;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile borderTile;
    [SerializeField] Transform backround;


    [Header("Map Size")]
    [SerializeField] float paddingLeft;
    [SerializeField] float paddingRight;
    [SerializeField] float paddingTop;
    [SerializeField] float paddingBottom;
    public Vector2 minPos;
    public Vector2 maxPos;


    [Header("Prefabs")]
    [SerializeField] GameObject xpDuck;
    [SerializeField] GameObject present;
    [SerializeField] GameObject coinPackage;
    public List<Duck> duckArray;
    [HideInInspector] public List<Duck> availableDucksList = new();


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
        UpdateCoinText();
        CalculateBounds();

        Instantiate(xpDuck, MyRandom.RandomPosition(minPos, maxPos), quaternion.identity);
        xpNeeded = (level + 1) * 1000;
    }

    void Update()
    {
        boxTimer += Time.deltaTime;
        if (boxTimer >= boxSpawnTimer)
        {
            SpawnPackage();
            boxTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            AddXp(xpNeeded - xp);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            CalculateBounds(); //TODO: Update only X amount of time after screen size updated
        }
    }

    void CalculateBounds()
    {
        minPos = TranslateValues.CalculateMinCameraBounds(paddingLeft, paddingBottom);
        maxPos = TranslateValues.CalculateMaxCameraBounds(paddingRight, paddingTop);
        Mathf.RoundToInt(minPos.x);
        Mathf.RoundToInt(minPos.y);
        Mathf.RoundToInt(maxPos.x);
        Mathf.RoundToInt(maxPos.y);

        tilemap.ClearAllTiles();
        for (int x = (int)minPos.x - 1; x <= maxPos.x + 1; x++)
        {
            tilemap.SetTile(new Vector3Int(x, (int)minPos.y - 1), borderTile);
            tilemap.SetTile(new Vector3Int(x, (int)minPos.y - 2), borderTile);
        }

        for (int y = (int)minPos.y; y <= maxPos.y; y++)
        {
            tilemap.SetTile(new Vector3Int((int)maxPos.x + 1, y), borderTile);
            tilemap.SetTile(new Vector3Int((int)maxPos.x + 2, y), borderTile);
        }

        for (int x = (int)maxPos.x + 1; x >= minPos.x - 1; x--)
        {
            tilemap.SetTile(new Vector3Int(x, (int)maxPos.y), borderTile);
            tilemap.SetTile(new Vector3Int(x, (int)maxPos.y + 1), borderTile);
        }

        for (int y = (int)maxPos.y; y >= minPos.y; y--)
        {
            tilemap.SetTile(new Vector3Int((int)minPos.x - 1, y), borderTile);
            tilemap.SetTile(new Vector3Int((int)minPos.x - 2, y), borderTile);
        }

        backround.localScale = new(Camera.main.orthographicSize * Camera.main.aspect * 2, Camera.main.orthographicSize * 2, 1);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector3)minPos, 0.5f);
        Gizmos.DrawWireSphere((Vector3)maxPos, 0.5f);
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
        xpNeeded = (level + 1) * 1000;

        UpdateSlider();
        UpdateSliderMax();

        Instantiate(present, midPoint, Quaternion.identity);
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
            Instantiate(coinPackage, MyRandom.RandomPosition(minPos, maxPos), Quaternion.identity);
        }
    }
}