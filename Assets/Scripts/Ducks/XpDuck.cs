using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpDuck : MonoBehaviour
{
    [SerializeField] float speedNeeded = 2f;
    [SerializeField] float xpGiven = 100f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Rigidbody2D otherRb = other.gameObject.GetComponent<Rigidbody2D>();
        float OtherSpeed = otherRb.velocity.magnitude;

        if (OtherSpeed <= 0)
        {
            OtherSpeed = -OtherSpeed;
        }

        if ((OtherSpeed >= speedNeeded || OtherSpeed <= -speedNeeded) && other.gameObject.CompareTag("Duck"))
        {
            GameManager.Instance.AddXp(xpGiven);
            ResetDuck();
        }
    }

    void ResetDuck()
    {
        transform.position = GameManager.Instance.GenerateRandomPosition();
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
