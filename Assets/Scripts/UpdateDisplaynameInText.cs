using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using TMPro;
using UnityEngine;

public class UpdateDisplaynameInText : MonoBehaviour
{
    FirebaseUser user;
    [SerializeField] TMP_Text textField;

    void OnEnable()
    {
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        textField.text = user.DisplayName;
    }
}
