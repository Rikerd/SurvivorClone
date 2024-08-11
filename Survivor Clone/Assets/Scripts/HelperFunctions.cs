using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    public static void ShuffleList<T>(ref List<T> list)
    {
        int count = list.Count;
        int lastIndex = count - 1;
        for (int i = 0; i < lastIndex; i++)
        {
            int randomIndex = Random.Range(i, count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
