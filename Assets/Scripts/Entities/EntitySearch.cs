using System.Collections.Generic;
using UnityEngine;


public static class EntitySearch
{
    public static Item ChooseNearestItem(Vector3 point)
    {
        Item nearest = null;
        float distance, minDistance = float.MaxValue;

        foreach (Item item in ItemManager.items)
        {
            if ((distance = Vector3.SqrMagnitude(point - item.transform.position)) < minDistance)
            {
                nearest = item;
                minDistance = distance;
            }
        }

        return nearest;
    }

    public static Item ChooseNearestItemWithFreeSpaceInWarehouse(Vector3 point)
    {
        Item nearest = null;
        float distance, minDistance = float.MaxValue;

        foreach (Item item in ItemManager.items)
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
} 