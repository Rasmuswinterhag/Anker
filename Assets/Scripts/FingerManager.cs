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
                GameObject finger = fingers[i];
                Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                worldTouchPosition.z = 0;
                // if (!finger.activeSelf)
                // {
                //     finger.SetActive(true);
                // }
                fingers[i].transform.position = worldTouchPosition;
            }
        }

    }
}
