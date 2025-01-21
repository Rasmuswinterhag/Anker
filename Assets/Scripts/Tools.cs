using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class Duckstuff
    {
        public static Duck GetDuckByDuckType(GameManager.DuckTypes DuckType, List<Duck> allDucks)
        {
            return GetDuckByDuckType(DuckType, allDucks.ToArray());
        }

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
        public static Vector2 RandomPosition(Vector2 minPos, Vector2 maxPos)
        {
            return new Vector2(UnityEngine.Random.Range(minPos.x, maxPos.x), UnityEngine.Random.Range(minPos.y, maxPos.y));
        }
    }
}