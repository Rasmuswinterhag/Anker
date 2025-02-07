using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FingerManager : MonoBehaviour
{
    [SerializeField] GameObject fingerObject;
    List<GameObject> fingers = new();
    [SerializeField] int minFingers = 5;

    void Start()
    {
        for (int i = 0; i < minFingers; i++)
        {
            NewFinger();
        }
    }

    void NewFinger()
    {
        GameObject spawnedFinger = Instantiate(fingerObject, gameObject.transform);
        spawnedFinger.SetActive(false);
        fingers.Add(spawnedFinger);
    }


    void Update()
    {
        if (Shop.Instance != null && Shop.Instance.gameObject.activeSelf) { return; }
        while (fingers.Count < Input.touchCount)
        {
            NewFinger();
        }

        for (int i = 0; i < fingers.Count; i++)
        {
            fingers[i].SetActive(i < Input.touchCount);
        }

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);

                worldTouchPosition.z = 0;
                fingers[i].transform.position = worldTouchPosition;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldTouchPosition.z = 0;
            fingers[0].SetActive(true);
            fingers[0].transform.position = worldTouchPosition;
        }
    }
}