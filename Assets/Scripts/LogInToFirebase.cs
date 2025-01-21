using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class LogInToFirebase : MonoBehaviour
{
    FirebaseAuth auth;
    string emailInput;
    string passwordInput;
    string displayNameInput;
    [SerializeField] TMP_Text userInfo;
    [SerializeField] GameObject displayNameThings;
    FirebaseUser user;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void UpdateEmail(string input)
    {
        emailInput = input;
    }

    public void UpdatePassword(string input)
    {
        passwordInput = input;
    }

    public void UpdateDisplayName(string input)
    {
        displayNameInput = input;
    }

    public void Register()
    {
        RegisterNewUser(emailInput, passwordInput);
    }

    private void RegisterNewUser(string email, string password)
    {
        Debug.Log("Starting Registration");

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.EmailAlreadyInUse:
                        message = "Attempting Log In";
                        SignIn(email, password);
                        break;
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    default:
                        Debug.LogWarning(task.Exception);
                        break;

                }
                userInfo.text = message;
            }
            else
            {
                user = task.Result.User;
                userInfo.text = "Registration succsessfull";
                Debug.LogFormat("User Registerd: {0}", user.UserId);
                SignInOrSetName();
            }
        });
    }

    void SignInOrSetName()
    {
        if (string.IsNullOrWhiteSpace(user.DisplayName))
        {
            showDisplayNameThings();
        }
        else
        {
            SignIn(emailInput, passwordInput);
        }
    }

    void showDisplayNameThings()
    {
        displayNameThings.SetActive(true);
        userInfo.text = "Choose Displayname";
    }

    public void SetDisplayname()
    {
        if (user != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = displayNameInput
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
                SignInOrSetName();
            });
        }
    }

    public void LogInButton()
    {
        RegisterNewUser(emailInput, passwordInput);
    }


    private void SignIn(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Sign In Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        break;
                    case AuthError.UserDisabled:
                        message = "User Disabled";
                        break;
                    case AuthError.Failure:
                        message = "Failure (Wrong Password)";
                        break;
                    default:
                        Debug.LogWarning(task.Exception);
                        break;
                }
                userInfo.text = message;
            }
            else
            {
                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
                userInfo.text = "user logged in: " + newUser.DisplayName;
                SceneManager.LoadScene(1); //Switch to game scene
            }
        });
    }

    public void SignOut()
    {
        auth.SignOut();
        SceneManager.LoadScene(0);
        Debug.Log("User signed out");
    }
}