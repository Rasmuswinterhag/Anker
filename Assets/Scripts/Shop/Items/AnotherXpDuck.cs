using UnityEngine;

public class AnotherXpDuck : ShopItem
{
    void Awake()
    {
        SetActualCost();
    }

    public int SetActualCost()
    {
        actualCost = Mathf.RoundToInt(itemData.cost * (Saving.Instance.shopData.xpDucksPurchased * itemData.costMultiplier));
        //cost is the original cost (itemdata.cost) times how many times its purchased multiplied by how much it should increase every purchase
        if(actualCost <= 0)
        {
            actualCost = itemData.cost;
        }

        return actualCost;
    }

    public override void Purchase()
    {
        Debug.Log("Another Xp Duck");
        GameManager.Instance.AddCoins(-actualCost);

        Saving.Instance.shopData.xpDucksPurchased++;
        SetActualCost();
        Shop.Instance.SetCostText(actualCost);

        GameManager.Instance.amountOfXpDucks++;
    }
}
