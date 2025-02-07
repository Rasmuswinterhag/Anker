using System;
using UnityEngine;

public class ShorterPackageTime : ShopItem
{
    void Awake()
    {
        SetActualCost();
        gameObject.SetActive(!CheckIfMaxed());
    }

    public int SetActualCost()
    {
        actualCost = Mathf.RoundToInt(itemData.cost * (Saving.Instance.shopData.shorterPackageTimesPurchased * itemData.costMultiplier));
        //cost is the original cost (itemdata.cost) times how many times its purchased multiplied by how much it should increase every purchase
        if (actualCost <= 0)
        {
            actualCost = itemData.cost;
        }
        return actualCost;
    }

    public override void Purchase()
    {
        Debug.Log("Shorter Package Time");
        GameManager.Instance.AddCoins(-actualCost);

        Saving.Instance.shopData.shorterPackageTimesPurchased++;
        SetActualCost();

        Shop.Instance.SetCostText(actualCost);

        GameManager.Instance.boxSpawnTimer -= 1;
        gameObject.SetActive(!CheckIfMaxed());
    }

    public bool CheckIfMaxed()
    {
        if (itemData.maxAmount <= -1) { return false; }
        if (itemData.maxAmount > Saving.Instance.shopData.shorterPackageTimesPurchased)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
