using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveXPGain : MonoBehaviour
{
    [SerializeField] float xpPerSecond = 1;

    void Start()
    {
        InvokeRepeating(nameof(GainXp), 0.0f, 0.1f);
    }

    void GainXp()
    {
        GameManager.Instance.AddXp(xpPerSecond/10);
    }
}
