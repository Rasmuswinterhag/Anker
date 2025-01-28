using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField, TextArea(3, 5)] string[] greetings;
    [SerializeField] TMP_Text shopKeeperField;

    public static Shop Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    void OnEnable()
    {
        ShopkeeperSay(greetings[Random.Range(0, greetings.Length)]);
    }

    public void ShopkeeperSay(string whatToSay)
    {
        shopKeeperField.text = whatToSay;
    }
}

[CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop/Item", order = -1)]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public Sprite image;
    [TextArea(3, 5)] public string description;
    public int cost;
    public float costMultiplier;
    public int maxAmount = 1;
}