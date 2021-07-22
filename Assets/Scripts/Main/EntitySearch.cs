using UnityEngine;


public static class EntitySearch
{
    static Vector3 a = new Vector3(0f, 0f, 0f);

/*    public static Item GetItem(bool nearest = false, Vector3 senderPos = new Vector3(), bool hasFreeSpaceInWarehouses = false)
    {
        Item result = null;
        float distance, minDistance = float.MaxValue;

        if (!nearest)
        {
            if (ItemManager.items.Count > 0) return ItemManager.items[0];
            else return null;
        }

        foreach (Item item in ItemManager.items)
        {
            if ((distance = Vector3.SqrMagnitude(senderPos - item.transform.position)) < minDistance)
            {
                nearest = item;
                minDistance = distance;
            }

            if (!nearest && result != null) break;
        }
    }*/


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
} 