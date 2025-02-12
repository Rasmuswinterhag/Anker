using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class XpDuck : MonoBehaviour
{
    XpOrbObjectPool xpOrbObjectPool;
    [SerializeField] float speedNeeded = 2f;
    [SerializeField] GameObject particle;
    public static float xpGiven = 100f;

    void Start()
    {
        xpOrbObjectPool = FindObjectOfType<XpOrbObjectPool>();
    }

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
            Pop();
        }
    }

    void Pop()
    {
        for (int i = 0; i < xpGiven; i += 20)
        {
            xpOrbObjectPool.RequestXpOrb(transform);
            //TODO: Theese particles will go over to the xp bar to show that popping theese ducks gives you xp
        }

        //GameManager.Instance.AddXp(xpGiven); ///TODO: Maybe give xp when the "particles" get to the xp bar
        ResetDuck();
    }

    void ResetDuck()
    {
        transform.position = MyRandom.RandomPosition(GameManager.Instance.minPos, GameManager.Instance.maxPos);
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}