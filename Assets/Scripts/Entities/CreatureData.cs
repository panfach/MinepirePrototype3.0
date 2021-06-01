using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "ScriptableObjects/Creature")]
public class CreatureData : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] CreatureIndex index;
    [SerializeField] CreatureType type;
    [SerializeField] float maxHealth;
    [SerializeField] DroppedItem[] drop;
    [SerializeField] int minRandomAge;
    [SerializeField] int maxRandomAge;

    [Header("Time settings")]
    public float timeOfBuildingEntering = 1f;
    public float angleControlDelay = 0.18f;
    public float checkEventsDelay = 0.11f;
    public float defaultAttackDistance = 0.2f;
    public float attackDelay = 0.9f;
    public float constructionDistance = 1f;
    public float constructionDelay = 1f;
    public float defaultActionDistance = 0.2f;
    public float takingDelay = 0.98f;
    public float maxDistFromHome = 5f;
    public float maxRandomWalkDelay = 5f;


    //const float timeOfBuildingEntering = 1f, angleControlDelay = 0.18f, checkEventsDelay = 0.11f;
    //const float defaultAttackDistance = 0.2f, attackDelay = 0.9f, constructionDistance = 1f;
    //const float constructionDelay = 1f, defaultActionDistance = 0.2f, takingDelay = 0.0f;

    public string Name { get => _name; }
    public CreatureIndex Index { get => index; }
    public CreatureType Type { get => type; }
    public float MaxHealth { get => maxHealth; }
    public IEnumerable Drop() => drop;
    public DroppedItem Drop(int index) => drop[index];
    public int MinRandomAge { get => minRandomAge; }
    public int MaxRandomAge { get => maxRandomAge; }
}
