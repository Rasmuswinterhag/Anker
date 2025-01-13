using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Duck"))
        {
            GameManager.Instance.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }
}