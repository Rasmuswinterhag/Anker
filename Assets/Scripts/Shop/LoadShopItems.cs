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
        Shop.Instance.SetCost(shopItem.cost);
        //TODO: Keep track how many of the items youve bought, and adjust cost according to that and shopItem.costMultiplier
        //TODO: Make the item selected
    }
}
