using System.Collections.Generic;
using UnityEngine;

public class CreatureContainer : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] Transform[] enter;                                    
    [SerializeField] bool hasCreatureLimit;
    [SerializeField] int creatureLimit;

    [Header("Info")]
    [SerializeField] int creatures;
    [SerializeField] List<Creature> creatureList = new List<Creature>();

    public Transform Enter { get => enter[0]; }
    public Transform GetEnter(int i) { return enter[i]; }
    public int CreatureCount { get => creatures; }


    private void OnEnable()
    {
        // Add reaction of change. Changes must refresh ui info at least
    }


    public bool Add(Creature creature)
    {
        if (hasCreatureLimit && CreatureCount >= creatureLimit) return false;

        creatureList.Add(creature);
        creatures++;
        creature.CrtProp.PlaceOfStay = this;

        // ChangeEvent

        return true;
    }

    public bool Remove(Creature creature)
    {
        if (!creatureList.Remove(creature))
            return false;
        creatures--;
        creature.CrtProp.PlaceOfStay = null;

        // ChangeEvent

        return true;
    }


    private void OnDisable()
    {

    }
}
