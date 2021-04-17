using System.Collections.Generic;
using UnityEngine;

public class CreatureContainer : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    public BuildingEnterTrigger[] enter;
    [SerializeField] bool hasPeopleLimit;
    [SerializeField] int peopleLimit;

    [Header("Info")]
    [SerializeField] int people;
    [SerializeField] List<VillagerData> peopleList = new List<VillagerData>();


    private void OnEnable()
    {

    }


    private void OnDisable()
    {

    }
}
