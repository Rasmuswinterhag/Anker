using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketDuck : MonoBehaviour
{
    [SerializeField] float rocketSpeed = 1.5f;
    [SerializeField] float rocketPower = 0.5f;
    [SerializeField] float rocketCooldown = 1f;
    float speed;
    Animator anim;
    Rigidbody2D rb;
    ParticleSystem pSystem;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pSystem = GetComponent<ParticleSystem>();
        timer = rocketCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer = timer + Time.fixedDeltaTime;

        speed = rb.velocity.magnitude;
        if (timer > rocketCooldown)
        {
            if (speed < rocketSpeed)
            {
                rb.AddForce(transform.up * rocketPower);
                anim.Play("RocketDuckRocketing");
                pSystem.Play();
            }
            else
            {
                timer = 0f;
                anim.Play("RocketDuckIdle");
                pSystem.Stop();
            }
        }
    }
}
