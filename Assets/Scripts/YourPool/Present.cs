using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Finger"))
        {
            GameManager.Instance.SpawnDuckFromPresent(this);
        }
    }

    public void PlaceDuck(Duck duck)
    {
        Instantiate(duck, transform.position, transform.rotation);
        Saving.Instance.SaveGame();
        Destroy(gameObject);
    }
}
