using System;
using UnityEngine;

public class PassiveXp : ShopItem
{
    void Awake()
    {
        SetActualCost();
    }

    public int SetActualCost()
    {
        actualCost = Mathf.RoundToInt(itemData.cost * (Saving.Instance.shopData.passiveXpBuffsPurchased * itemData.costMultiplier));
        //cost is the original cost (itemdata.cost) times how many times its purchased multiplied by how much it should increase every purchase
        if(actualCost <= 0)
        {
            actualCost = itemData.cost;
        }
        return actualCost;
    }
    public override void Purchase()
    {
        Debug.Log("Passive XP");
        GameManager.Instance.AddCoins(-actualCost);

        Saving.Instance.shopData.passiveXpBuffsPurchased++;
        SetActualCost();

        Shop.Instance.SetCostText(actualCost);

        PassiveXPGain.xpPerSecond++;
    }
}
