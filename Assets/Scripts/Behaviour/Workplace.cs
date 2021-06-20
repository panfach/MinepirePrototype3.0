using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Info")]
    public bool attackPermission;                                                // Temporal
    [SerializeField] List<Item> items = new List<Item>();



    public void AddItem(Item _item)
    {
        items.Add(_item);
    }

    public void AssignItems(List<Item> _items)
    {
        items = _items;
    }

    public Item GetFirstItem()
    {
        RemoveNullItems();
        return items.Count > 0 ? items[0] : null;
    }


    void RemoveNullItems()
    {
        int count = items.Count;
        for (int i = 0; i < count; i++)
        {
            if (items[i] == null)
            {
                items.RemoveAt(i);
                count--;
                i--;
            }
        }
    }
}
