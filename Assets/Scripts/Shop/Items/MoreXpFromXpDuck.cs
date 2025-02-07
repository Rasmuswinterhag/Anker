using System;
using UnityEngine;

public class MoreXpFromXpDuck : ShopItem
{
    void Awake()
    {
        SetActualCost();
        gameObject.SetActive(!CheckIfMaxed());
    }

    public int SetActualCost()
    {
        actualCost = Mathf.RoundToInt(itemData.cost * (Saving.Instance.shopData.xpFromXpDuckPurchesed * itemData.costMultiplier));
        //cost is the original cost (itemdata.cost) times how many times its purchased multiplied by how much it should increase every purchase
        if(actualCost <= 0)
        {
            actualCost = itemData.cost;
        }
        return actualCost;
    }
    public override void Purchase()
    {
        Debug.Log("More Xp From Xp Duck");
        GameManager.Instance.AddCoins(-actualCost);

        Saving.Instance.shopData.xpFromXpDuckPurchesed++;
        SetActualCost();

        Shop.Instance.SetCostText(actualCost);

        XpDuck.xpGiven += Saving.Instance.shopData.xpFromXpDuckPurchesed * 100;
        gameObject.SetActive(!CheckIfMaxed());
    }

    public bool CheckIfMaxed()
    {
        if (itemData.maxAmount <= -1) { return false; }
        if (itemData.maxAmount > Saving.Instance.shopData.xpFromXpDuckPurchesed)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}