using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class Duck : MonoBehaviour
{
    public GameManager.DuckTypes duckType;
    [SerializeField] float maxDistanceToZoom = 0.75f;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Finger"))
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                    FocusDuck.Instance.Focus(gameObject);
                }
            }
        }

    }

    void Update()
    {
        float sqrMouseDistance = Vector2.SqrMagnitude(TranslateValues.WorldMousePosition() - (Vector2)gameObject.transform.position);
        if (Input.GetMouseButtonDown(0) && sqrMouseDistance <= maxDistanceToZoom)
        {
            FocusDuck.Instance.Focus(gameObject);
        }
    }
}
