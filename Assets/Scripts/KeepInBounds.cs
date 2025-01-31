using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepInBounds : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (WithinBounds())
        {
            Vector2 nextPos;
            nextPos.x = Mathf.Clamp(transform.position.x, GameManager.Instance.minPos.x, GameManager.Instance.maxPos.x);
            nextPos.y = Mathf.Clamp(transform.position.y, GameManager.Instance.minPos.y, GameManager.Instance.maxPos.y);

            transform.position = nextPos;

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    bool WithinBounds()
    {
        return transform.position.x > GameManager.Instance.maxPos.x ||
        transform.position.y > GameManager.Instance.maxPos.y ||
        transform.position.x < GameManager.Instance.minPos.x ||
        transform.position.y < GameManager.Instance.minPos.y;
    }
}
