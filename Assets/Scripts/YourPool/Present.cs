using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{
    [SerializeField] float speedNeeded = 0.5f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Rigidbody2D otherRb = other.gameObject.GetComponent<Rigidbody2D>();
        float OtherSpeed = otherRb.velocity.magnitude;

        if (OtherSpeed <= 0)
        {
            OtherSpeed = -OtherSpeed;
        }

        if ((OtherSpeed >= speedNeeded || OtherSpeed <= -speedNeeded) && other.gameObject.CompareTag("Duck"))
        {
            Pop();
        }
    }

    void Pop()
    {
        SpawnDuckFromPresent();
        //TODO: confetti
    }

    void SpawnDuckFromPresent()
    {
        int listLegth = GameManager.Instance.availableDucksList.Count;


        if (listLegth > 0)
        {
            int randomListIndex = Random.Range(0, listLegth);
            PlaceDuck(GameManager.Instance.availableDucksList[randomListIndex]);
            GameManager.Instance.availableDucksList.RemoveAt(randomListIndex);
        }
        else
        {
            GameManager.Instance.availableDucksList = new List<Duck>(GameManager.Instance.duckArray);
            SpawnDuckFromPresent();
        }
    }

    public void PlaceDuck(Duck duck)
    {
        Instantiate(duck, transform.position, transform.rotation, GameManager.Instance.transform);
        //Saving.Instance.SaveGame(); //opening too many presents at once makes firebase not load sometimes. probobly to stop bot attacks
        Destroy(gameObject);
    }
}
