using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadShopItems : MonoBehaviour
{
    [SerializeField] ShopItem shopItem;

    [Header("References")]
    [SerializeField] Image image;
    [SerializeField] TMP_Text nameField;

    void Awake()
    {
        image.sprite = shopItem.image;
        nameField.text = shopItem.itemName;
    }

    public void OnClick()
    {
        Shop.Instance.ShopkeeperSay(shopItem.description);
        //TODO: Make the item selected
    }
}
