﻿using System.Collections.Generic;
using UnityEngine;

public class CreatureContainer : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    public BuildingEnterTrigger[] enter;
    [SerializeField] bool hasCreatureLimit;
    [SerializeField] int creatureLimit;

    [Header("Info")]
    [SerializeField] int creatures;
    [SerializeField] List<Creature> creatureList = new List<Creature>();

    public int CreatureCount { get => creatures; }


    private void OnEnable()
    {
        // Add reaction of change. Changes must refresh ui info at least
    }


    public void Add(Creature creature)
    {
        creatureList.Add(creature);
        creatures++;
        // ChangeEvent
    }

    public void Remove(Creature creature)
    {
        if (creatureList.Remove(creature))
            creatures--;
        // ChangeEvent
    }


    private void OnDisable()
    {

    }
}
