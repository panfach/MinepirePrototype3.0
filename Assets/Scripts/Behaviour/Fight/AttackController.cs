using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class AttackController : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] bool canAttackCreatures;
    [SerializeField] float attackPower;
    [SerializeField] float hitDuration;

    [Header("Info")]
    [SerializeField] float attackModifier = 1f;

    List<Item> dropItems = new List<Item>();

    public float Duration { get => hitDuration; }
    public List<Item> DropItems { get => dropItems; }
    public int DropItemsCount { get => dropItems.Count; }
    public float ImpactDamage
    {
        get => attackPower * attackModifier;
    }


    public bool Attack(Health target)
    {
        if (!canAttackCreatures && target.entity is Creature) return false;

        target.GetDamage(this, ImpactDamage);
        return true;
    }

    public void AssignDrop(List<Item> _dropItems)
    {
        dropItems = _dropItems;
    }

    public Item GetFirstDropItem()
    {
        RemoveNullItemsFromDrop();
        return dropItems.Count > 0 ? dropItems[0] : null;
    }

    void RemoveNullItemsFromDrop()
    {
        int count = dropItems.Count;
        for (int i = 0; i < count; i++)
        {
            if (dropItems[i] == null)
            {
                dropItems.RemoveAt(i);
                count--;
            }
        }
    }
}
