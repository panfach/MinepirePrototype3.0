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

    public static Health ChooseNearestTarget(List<Health> targets, Vector3 point)
    {
        if (targets.Count == 0) return null;

        Health nearest = null;
        float distance, minDistance = float.MaxValue;

        foreach (Health item in targets)
        {
            if ((distance = Vector3.SqrMagnitude(point - item.transform.position)) < minDistance)
            {
                nearest = item;
                minDistance = distance;
            }
        }

        return nearest;
    }

    public static Item ChooseNearestItem(List<Item> items, Vector3 point)
    {
        if (items.Count == 0) return null;

        Item nearest = null;
        float distance, minDistance = float.MaxValue;

        foreach (Item item in items)
        {
            if ((distance = Vector3.SqrMagnitude(point - item.transform.position)) < minDistance)
            {
                nearest = item;
                minDistance = distance;
            }
        }

        return nearest;
    }

    public static Item ChooseNearestItemWithFreeSpaceInWarehouse(List<Item> items, Vector3 point)
    {
        if (items.Count == 0) return null;

        Item nearest = null;
        float distance, minDistance = float.MaxValue;

        foreach (Item item in items)
        {
            if ((distance = Vector3.SqrMagnitude(point - item.transform.position)) < minDistance && HasFreeSpaceInWarehouse(item))
            {
                nearest = item;
                minDistance = distance;
            }
        }

        return nearest;
    }

    static bool HasFreeSpaceInWarehouse(Item item)
    {
        for (int i = 0; i < item.Inventory.PacksAmount; i++)
        {
            if (VillageData.FreeSpaceForResource(item.Inventory.StoredRes[i]) > 0.001f)
                return true;
        }
        return false;
    }



    /*public static Entity ChooseNearest<T>(List<T> comparableObjects, Vector3 targetPoint)
    {
        if (comparableObjects.Count == 0) return null;

        T nearestObj = default(T);
        float distance, minDistance = float.MaxValue;

        if (comparableObjects[0] is MonoBehaviour)
        {
            foreach (T item in comparableObjects)
            {
                if ((distance = Vector3.SqrMagnitude(targetPoint - building.transform.position)) < minDistance)
                {
                    nearestBuilding = building;
                    minDistance = distance;
                }
            }
        }
        else return null;

        return nearestBuilding;
    }*/
}
