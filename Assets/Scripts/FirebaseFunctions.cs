using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using Firebase.Auth;

public class FirebaseFunctions : MonoBehaviour
{
    string displayNameInput;

    public void SignOutButton()
    {
        FirebaseStuff.SignOut();
    }

    public void UpdateDisplayName(string input)
    {
        displayNameInput = input;
    }

    public void SetDisplayname()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        FirebaseStuff.SetDisplayname(user, displayNameInput);
    }
}
