using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;

public class LogInToFirebase : MonoBehaviour
{
    FirebaseAuth auth;
    string emailInput;
    string passwordInput;
    string displayName;
    [SerializeField] TMP_Text userInfo;
    [SerializeField] TMP_InputField displayNameInputField;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        FirebaseApp.LogLevel = LogLevel.Debug;
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
        displayName = input;
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
                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("User Registerd: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
            }
        });
    }


    public void LogIn()
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
                        message = "Failure";
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

    private void SignOut()
    {
        auth.SignOut();
        Debug.Log("User signed out");
    }
}