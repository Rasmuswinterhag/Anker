using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjects : MonoBehaviour
{
    [SerializeField] float pushForce = 100f;

    //push
    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 pushVector = other.transform.position - transform.position;
        other.gameObject.GetComponent<Rigidbody2D>().AddForce(pushVector.normalized * pushForce);
    }
}
