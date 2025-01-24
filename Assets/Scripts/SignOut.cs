using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class SignOut : MonoBehaviour
{
    public void SignOutButton()
    {
        FirebaseStuff.SignOut();
    }
}
