using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Finger : MonoBehaviour
{
    [SerializeField] float pushForce = 1f;
    Touch touch;

    bool isTriggerd;
    GameObject tappedDuck;
    float timer = 0;
    bool hitEmpty;
    //references
    FocusDuck focusDuck;
    SpriteRenderer sr;

    [SerializeField] List<Collider2D> colliders;
    [SerializeField] Collider2D trigger;
    [SerializeField] Collider2D myCollider;

    [SerializeField] int fingerNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        focusDuck = FindObjectOfType<FocusDuck>();

        //Get all colliders and add them to a list of colliders
        Collider2D[] collidersOnObject = GetComponents<Collider2D>();
        colliders.AddRange(collidersOnObject);

        //loop through the colliders and see if theyre a trigger or not
        for (int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].isTrigger)
                trigger = colliders[i];
            else
                myCollider = colliders[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > fingerNumber) //Held down finger
        {

            Vector3 touchposition = Input.GetTouch(fingerNumber).position;
            var touch = Camera.main.ScreenToWorldPoint(touchposition);
            touch.z = transform.position.z;

            transform.position = touch;
            sr.enabled = true;
        }
        else //let go of finger
        {
            if (timer < 10 && isTriggerd)
            {
                focusDuck.Focus(tappedDuck);
            }

            transform.position = Vector3.zero;
            sr.enabled = false;
            
            isTriggerd = false;
            timer = 0;
            tappedDuck = null;
            hitEmpty = false;
        }

        if (isTriggerd)
        {
            timer++;
        }

        if (touch.phase == TouchPhase.Began)
        {
            
        }
    }

    void LateUpdate()
    {
        // Track a single touch as a direction control.
        if (Input.touchCount > fingerNumber)
        {
            touch = Input.GetTouch(fingerNumber);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    myCollider.enabled = false;
                    break;
                
                case TouchPhase.Moved:
                    myCollider.enabled = true;
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it deos
                    break;
            }
        }
    }

    //push
    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 pushVector = other.transform.position - transform.position;
        other.gameObject.GetComponent<Rigidbody2D>().AddForce(pushVector.normalized * pushForce);
    }

    //focus
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Duck") && hitEmpty == false)
        {
            isTriggerd = true;
            tappedDuck = other.gameObject;
        }
        else if (other.gameObject.CompareTag("KeepTriggerOn"))
        {
            hitEmpty = true;
        }
        //dont run if started on nothing
    }
}
