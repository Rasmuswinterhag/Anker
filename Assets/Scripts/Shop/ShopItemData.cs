using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop/Item", order = -1)]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    public Sprite image;
    [TextArea(3, 5)] public string description;
    public int cost;
    public float costMultiplier;
    public int maxAmount = 1;
}