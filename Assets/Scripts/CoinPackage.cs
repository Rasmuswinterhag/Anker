using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoinPackage : MonoBehaviour
{

    [SerializeField] float speedNeeded = 1f;
    
    [Header("Prefabs")]
    [SerializeField] GameObject coin;

    [Header("randomzie coin amount")]
    [SerializeField] int minCoins = 1;
    [SerializeField] int maxCoins = 5;

    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Rigidbody2D otherRb = other.gameObject.GetComponent<Rigidbody2D>();
        float otherSpeed = otherRb.velocity.magnitude;

        if (otherSpeed <= 0)
        {
            otherSpeed = -otherSpeed;
        }   

        if (otherSpeed >= speedNeeded || otherSpeed <= -speedNeeded)
        {
            int coinAmount = Random.Range(minCoins, maxCoins);

            for (int i = 0; i < coinAmount; i++)
            {
                GameObject spawnedCoin = Instantiate(coin, transform.position, Quaternion.identity);
                Rigidbody2D coinRb = spawnedCoin.GetComponent<Rigidbody2D>();

                //coinRb.AddForce(new Vector2(Random.Range(-1,1), Random.Range(-1,1))); //For random coin direction
                coinRb.AddForce(rb.velocity); //For coins that follow the speed on the box
            }
            Destroy(gameObject);
        }
    }
}
