using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Tools;
using UnityEditor.Rendering;

public class GoToXpBar : MonoBehaviour
{
    Transform xpBar;
    Vector3 targetPosition;

    bool hasGoneOut = false;

    void Start()
    {
        xpBar = GameObject.FindGameObjectWithTag("Slider").transform;
        targetPosition = (Vector2)transform.position + Random.insideUnitCircle * 2.5f;
    }

    void Update()
    {
        float speed = Mathf.Clamp(Vector3.Distance(transform.position, targetPosition), 10, 20);
        Vector3 nextPosition;
        nextPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.position = nextPosition;

        if (transform.position == targetPosition)
        {
            if (hasGoneOut)
            {
                Destroy(gameObject); //TODO: Should use object pool instead
            }
            else
            {
                targetPosition = Camera.main.ScreenToWorldPoint(xpBar.position); //Id want this to go to the edge of the current Xp on the bar fill
                hasGoneOut = true;
            }
        }
    }
}
