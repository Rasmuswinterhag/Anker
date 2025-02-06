using System;
using UnityEngine;
using Tools;

public class BoughtGoldenDuck : ShopItem
{
    [Header("Item")]
    [SerializeField] GameObject Duck;
    void Awake()
    {
        SetActualCost();
        gameObject.SetActive(!CheckIfMaxed());
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
        Debug.Log("Bought Golden Duck");
        Instantiate(Duck, MyRandom.RandomPosition(GameManager.Instance.minPos, GameManager.Instance.maxPos), Quaternion.identity);
        Saving.Instance.shopData.hasBoughtGoldenDuck = true;
        //throw new NotImplementedException();
        Shop.Instance.SetCostText(actualCost);
        gameObject.SetActive(!CheckIfMaxed());

    }

    public bool CheckIfMaxed()
    {
        return Saving.Instance.shopData.hasBoughtGoldenDuck;
    }
}