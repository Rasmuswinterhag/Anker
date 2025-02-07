using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public ShopItemData itemData;

    [Header("References")]
    [SerializeField] Image image;
    [SerializeField] TMP_Text nameField;
    public int actualCost;

    void Start()
    {
        image.sprite = itemData.image;
        nameField.text = itemData.itemName;
    }

    public void OnClick()
    {
        Shop.Instance.SelectItem(this);
    }

    public virtual void Purchase()
    {
        throw new NotImplementedException();
    }
}