using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject shop;
    public void OpenCloseShop()
    {
        shop.SetActive(!shop.activeSelf);
    }
}
