using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "ScriptableObjects/Creature")]
public class CreatureData : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] CreatureIndex index;
    [SerializeField] float maxHealth;
    [SerializeField] DroppedItem[] drop;

    public string Name { get => _name; }
    public CreatureIndex Index { get => index; }
    public float MaxHealth { get => maxHealth; }
    public IEnumerable Drop() => drop;
    public DroppedItem Drop(int index) => drop[index];
}
