using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLightCycle : MonoBehaviour
{
    [SerializeField] float daySpeed = 1f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, daySpeed * -1 * Time.deltaTime, 0f, Space.Self);
    }
}
