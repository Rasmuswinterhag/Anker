using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjects : MonoBehaviour
{
    [SerializeField] float mousePushForce = 100f;
    [SerializeField] float mobilePushForce = 100f;

    //push
    void OnTriggerStay2D(Collider2D other)
    {
        Vector2 pushVector = other.transform.position - transform.position;
        Debug.Log(Input.touchCount, this);
        if (Input.touchCount == 0)
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(pushVector.normalized * mousePushForce);
        }
        else
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(pushVector.normalized * mobilePushForce);
        }
    }
}
