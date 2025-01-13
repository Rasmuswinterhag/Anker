using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAfterSeconds : MonoBehaviour
{
    [SerializeField] float removeTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Remove", removeTime);
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
