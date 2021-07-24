using System.Collections.Generic;
using UnityEngine;


public static class Distance
{
    public static float AvgSqrDistance(List<Transform> obj1, Transform obj2)
    {
        int count = obj1.Count;
        float sum = 0;
        for (int i = 0; i < count; i++)
        {
            sum += Vector3.SqrMagnitude(obj1[i].position - obj2.position);
        }
        return sum / count;
    }
}
