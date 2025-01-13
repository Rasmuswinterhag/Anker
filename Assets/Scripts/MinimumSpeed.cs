using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimumSpeed : MonoBehaviour
{
    [SerializeField] float minSpeed = 0.1f;
    float speed;

    Rigidbody2D rb;
    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.magnitude;

        if (speed <= 0) { speed = -speed; } //speed only positive
        
        if (speed == 0)
        {
            rb.AddForce(new Vector2 (
                Random.Range(-minSpeed, minSpeed),
                Random.Range(-minSpeed, minSpeed)));
        }
        else if (speed <= minSpeed)
        {
            //Debug.Log(speed);
            rb.velocity = rb.velocity * 1.1f;
        }
    }
}
