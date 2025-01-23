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
    }
}