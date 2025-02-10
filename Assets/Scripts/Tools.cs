using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;

namespace Tools
{
    public class Duckstuff
    {
        /// <summary>
        /// Gets the duck component from a duck type
        /// </summary>
        /// <returns>
        /// The duck compontent
        /// </returns>
        public static Duck GetDuckByDuckType(GameManager.DuckTypes DuckType, List<Duck> allDucks)
        {
            return GetDuckByDuckType(DuckType, allDucks.ToArray());
        }

        /// <summary>
        /// Gets the duck component from a duck type
        /// </summary>
        /// <returns>
        /// The duck compontent
        /// </returns>
        public static Duck GetDuckByDuckType(GameManager.DuckTypes DuckType, Duck[] allDucks)
        {
            //return allDucks.Find(duck => duck.duckType == DuckType);
            foreach (var duck in allDucks)
            {
                if (duck.duckType == DuckType)
                {
                    return duck;
                }
            }
            return null;
        }
    }

    public class TranslateValues
    {
        /// <summary>
        /// Makes an int into a bool (accepts 1 or 0)
        /// </summary>
        /// <returns>
        /// 1 = true,
        /// 0 = false
        /// </returns>
        public static bool IntToBool(int input)
        {
            if (input == 1)
            {
                return true;
            }
            else if (input == 0)
            {
                return false;
            }

            Debug.LogWarning("input was " + input + " expected 1 or 0, returning false");
            return false;
        }

        /// <summary>
        /// Makes a bool into an int
        /// </summary>
        /// <returns>
        /// true = 1,
        /// false = 0
        /// </returns>
        public static int BoolToInt(bool input)
        {
            if (input)
            {
                return 1;
            }
            else if (!input)
            {
                return 0;
            }

            Debug.LogWarning("input was " + input + " expected true or false, returning 0");
            return 0;
        }

        /// <summary>
        /// Multiplies each of a Vector3's values with each of another Vector3's values
        /// </summary>
        /// <returns>
        /// A Vector3 with its values multiplied
        /// </returns>
        public static Vector3 MultiplyVector3(Vector3 input, Vector3 multipliedBy)
        {
            Vector3 output = new();
            output.x = input.x * multipliedBy.x;
            output.y = input.y * multipliedBy.y;
            output.z = input.z * multipliedBy.z;

            return output;
        }

        /// <summary>
        /// Multiplies each of a Vector2's values with each of another Vector2's values
        /// </summary>
        /// <returns>
        /// A Vector2 with its values multiplied
        /// </returns>
        public static Vector2 MultiplyVector2(Vector2 input, Vector2 multipliedBy)
        {
            Vector2 output = new();
            output.x = input.x * multipliedBy.x;
            output.y = input.y * multipliedBy.y;

            return output;
        }

        /// <returns>
        /// The mouse position translated from ScreenToWorldPoint from the main camera
        /// </returns>
        public static Vector2 WorldMousePosition()
        {
            return WorldMousePosition(Camera.main);
        }

        /// <returns>
        /// The mouse position translated from ScreenToWorldPoint according to a specific camera
        /// </returns>
        public static Vector2 WorldMousePosition(Camera camera)
        {
            return camera.ScreenToWorldPoint(Input.mousePosition);
        }

        /// <returns>
        /// The bottom left point of the camera plus padding
        /// </returns>
        public static Vector2 CalculateMinCameraBounds(float paddingLeft, float paddingBottom)
        {
            Camera cam = Camera.main;

            Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));

            bottomLeft.x += paddingLeft;
            bottomLeft.y += paddingBottom;

            return bottomLeft;
        }

        /// <returns>
        /// The top right point of the camera plus padding
        /// </returns>
        public static Vector2 CalculateMaxCameraBounds(float paddingRight, float paddingTop)
        {
            Camera cam = Camera.main;

            Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.nearClipPlane));

            topRight.x -= paddingRight;
            topRight.y -= paddingTop;

            return topRight;
        }
    }

    public class MyRandom
    {
        /// <returns>
        /// A random position between two vectors
        /// </returns>
        public static Vector2 RandomPosition(Vector2 minPos, Vector2 maxPos)
        {
            return new Vector2(UnityEngine.Random.Range(minPos.x, maxPos.x), UnityEngine.Random.Range(minPos.y, maxPos.y));
        }
    }

    public class FirebaseStuff
    {
        public static void SignOut()
        {
            FirebaseAuth.DefaultInstance.SignOut();
            SceneManager.LoadScene(0);
            Debug.Log("User signed out");
        }

        public static void SetDisplayname(FirebaseUser user, string displayNameInput)
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
                });
            }
        }

        public static bool IsLoggedIn()
        {
            if (FirebaseAuth.DefaultInstance.CurrentUser == null)
            {
                Debug.LogWarning("Player is NOT logged in");
                return false;
            }
            else
            {
                Debug.LogWarning("Player is logged in");
                return true;
            }
        }
    }

    public class Other
    {
        /// <summary>
        /// Cets all children of a gameobject
        /// </summary>
        /// <returns>
        // An array of Transforms
        // </returns>
        public static Transform[] GetAllChildren(GameObject parent)
        {
            return GetAllChildren(parent.transform);
        }

        /// <summary>
        /// Cets all children of a transform
        /// </summary>
        /// <returns>
        // An array of Transforms
        // </returns>
        public static Transform[] GetAllChildren(Transform parent)
        {
            List<Transform> children = new();
            for (int i = 0; i < parent.childCount; i++)
            {
                children.Add(parent.GetChild(i));
            }

            return children.ToArray();
        }

        /// <returns>
        /// Camera Width of Camera.main
        /// </returns>
        public static float CameraWidth()
        {
            return CameraWidth(Camera.main);
        }

        /// <param name="camera"></param>
        /// <returns>
        /// Camera Width of <paramref name="camera"/>
        /// </returns>
        public static float CameraWidth(Camera camera)
        {
            return camera.orthographicSize * camera.aspect * 2;
        }
    }
}