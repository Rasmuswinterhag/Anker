using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField, TextArea(3, 5)] string[] greetings;
    [SerializeField, TextArea(3, 5)] string[] boughtLines;
    [SerializeField, TextArea(3, 5)] string[] YourePoorText;
    [SerializeField] AudioClip[] quacks;
    [SerializeField] TMP_Text shopKeeperField;
    [SerializeField] TMP_Text costField;
    [SerializeField] GameObject shopItemsContent;
    List<ShopItem> shopItems = new();
    int selectedItem = -1;

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
        selectedItem = -1;
        ShopkeeperSay(greetings[Random.Range(0, greetings.Length)]);
        foreach (Transform child in Other.GetAllChildren(shopItemsContent))
        {
            ShopItem childComponent = child.GetComponent<ShopItem>();
            if (!shopItems.Contains(childComponent))
            {
                shopItems.Add(childComponent);
            }
        }
    }

    public void ShopkeeperSay(string whatToSay)
    {
        shopKeeperField.text = whatToSay;
        AudioSource.PlayClipAtPoint(quacks[Random.Range(0, quacks.Length)], Camera.main.transform.position);
    }

    public void SetCostText(int cost)
    {
        costField.text = cost + " dc";
    }

    public void SelectItem(ShopItem shopItem)
    {
        if (shopItem == null)
        {
            OnEnable();
        }
        else
        {
            selectedItem = shopItems.IndexOf(shopItem);
            ShopkeeperSay(shopItem.itemData.description);
            SetCostText(shopItem.actualCost);
        }

    }

    public void Buy()
    {
        if (selectedItem == -1)
        {
            ShopkeeperSay("What do you want to buy? You have to select it so i know.");
        }

        ShopItemData item = shopItems[selectedItem].itemData;
        ShopItem shopItem = shopItems[selectedItem];

        if (GameManager.Instance.coins >= shopItem.actualCost)
        {
            shopItem.Purchase();
            ShopkeeperSay(boughtLines[Random.Range(0, boughtLines.Length)]);
        }
        else
        {
            ShopkeeperSay(YourePoorText[Random.Range(0, YourePoorText.Length)]);
        }
    }
}