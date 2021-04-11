using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static List<Item> items = new List<Item>();

    public void DropItems(Creature creature, string tag = null)
    {
        List<Item> dropItems = new List<Item>();
        Vector3 pos = creature.transform.position;
        Vector3 dropPos = new Vector3(pos.x, SCCoord.GetHeight(pos), pos.z);

        // Calculates chances, amounts, and spawns required resource instances
        foreach (DroppedItem drop in creature.CrtData.Drop())
        {
            Vector3 randDropPos = dropPos + new Vector3(Random.Range(-0.05f, 0.05f), 0f, Random.Range(-0.05f, 0.05f));
            if (Random.Range(0f, 1f) < drop.chance)
                dropItems.Add(CreateItem(randDropPos, drop.resourceIndex, Random.Range(drop.minAmount, drop.maxAmount)));
        }

        creature.CrtProp.droppedItems = dropItems.ToArray();
    }

    public void ExecuteExtracting(ResourceDeposit resourceDeposit, int ind, float amount)
    {
        Vector3 pos = resourceDeposit.ExtractPoint.position;
        Vector3 dropPos = new Vector3(pos.x + Random.Range(-0.05f, 0.05f), SCCoord.GetHeight(pos), pos.z + Random.Range(-0.05f, 0.05f));
        CreateItem(dropPos, resourceDeposit.Index(ind), amount);
    }

    public Item CreateEmptyItem(Vector3 position)
    {
        GameObject itemObj = Instantiate(DataList.GetItemObject(), position, Quaternion.identity);
        Item item = itemObj.GetComponent<Item>();
        items.Add(item);

        return item;
    }

    public Item CreateItem(Vector3 position, ResourceIndex resInd, float size)
    {
        GameObject itemObj = Instantiate(DataList.GetItemObject(), position, Quaternion.identity);
        Item item = itemObj.GetComponent<Item>();
        item.Inventory.ItemInit(resInd, size);
        items.Add(item);

        return item;
    }
}

[System.Serializable]
public struct DroppedItem
{
    public ResourceIndex resourceIndex;
    public float minAmount;
    public float maxAmount;
    [Range(0.0f, 1.0f)] public float chance;
}
