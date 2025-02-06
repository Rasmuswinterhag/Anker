using UnityEngine;

public class CoinsToXp : ShopItem
{
    [Header("Item")]
    [SerializeField] int xpTogive = 250;

    void Awake()
    {
        SetActualCost();
    }

    public int SetActualCost()
    {
        actualCost = itemData.cost;

        return actualCost;
    }
    public override void Purchase()
    {
        SetActualCost();
        GameManager.Instance.AddCoins(-actualCost);
        Debug.Log("coins to xp");
        GameManager.Instance.AddXp(xpTogive);
        Shop.Instance.SetCostText(actualCost);
    }
}
