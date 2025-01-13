using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    Rigidbody2D rb2d;
    PointEffector2D pEffector;
    CircleCollider2D pEffectorTrigger;
    ParticleSystem particleSys;
    [SerializeField] float spinTrorque = 5;
    [SerializeField] int minSuckSpeed = 360;
    [SerializeField] float suckRepeatTime = 30;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        pEffector = GetComponent<PointEffector2D>();
        pEffectorTrigger = GetComponent<CircleCollider2D>();
        particleSys = GetComponent<ParticleSystem>();

        InvokeRepeating(nameof(StartSuck), 0, suckRepeatTime);
    }

    void StartSuck()
    {
        StartCoroutine(nameof(Suck));
    }

    IEnumerator Suck()
    {
        while (rb2d.angularVelocity < minSuckSpeed)
        {
            rb2d.AddTorque(spinTrorque, ForceMode2D.Force);
            yield return new WaitForSeconds(0.1f);
        }
        SuckEnabled(true);
        yield return new WaitForSeconds(1f);
        while (rb2d.angularVelocity > minSuckSpeed / 3)
        {
            yield return new WaitForSeconds(0.1f);
        }
        SuckEnabled(false);
    }

    void SuckEnabled(bool value)
    {
        pEffector.enabled = value;
        pEffectorTrigger.enabled = value;
        if (value)
        {
            particleSys.Play();
        }
        else
        {
            particleSys.Stop();
        }
    }
}
