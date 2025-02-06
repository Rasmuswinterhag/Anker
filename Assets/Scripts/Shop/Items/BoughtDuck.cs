using System;
using UnityEngine;
using Tools;

public class BoughtDuck : ShopItem
{
    [Header("Item")]
    [SerializeField] GameObject Duck;
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
        Debug.Log("Bought Duck");
        Instantiate(Duck, MyRandom.RandomPosition(GameManager.Instance.minPos, GameManager.Instance.maxPos), Quaternion.identity);
        //throw new NotImplementedException();
        Shop.Instance.SetCostText(actualCost);

    }
}