using System.Collections.Generic;
using UnityEngine;

public class PeopleAppointer : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Info")]
    [SerializeField] int people;
    [SerializeField] int maxPeople;
    [SerializeField] List<VillagerData> peopleList = new List<VillagerData>();

    public int People { get => people; }
    public VillagerData GetPeople(int i) => peopleList[i];

    public event SimpleEventHandler peopleChangedEvent;

    private void OnEnable()
    {                                                                   
        maxPeople = entity.BldData.MaxPeople;
    }


    public bool AddPeople(VillagerData villager)
    {
        if (People < maxPeople)
        {
            peopleList.Add(villager);
            people++;
            peopleChangedEvent?.Invoke();
            return true;
        }
        return false;
    }

    public bool RemovePeople(VillagerData villager)
    {
        /*Debug.Log($"ReadySet.RemovePeople() Trying to remove people {villager.Name} {villager.Age}");
        foreach(VillagerData item in peopleList)
        {
            Debug.Log($"peopleList: {item.Name} {item.Age}");
        }*/

        if (peopleList.Remove(villager))
        {
            people--;
            peopleChangedEvent?.Invoke();
            return true;
        }
        return false;
    }

    public void ForgetPeopleAssignment()
    {
        people = 0;
        peopleList.Clear();
    }

    public void SetDefaultPeople()
    {
        people = 0;
    }


    private void OnDisable()
    {
        for (int i = 0; i < people; i++)
        {
            if (entity.BldData.BldType == BuildingType.LIVING)
                peopleList[0].Evict();                                    // Maybe use "RemovePeople()". Then its need to add some code in that function
            else if (entity.BldData.BldType == BuildingType.HUNT)
                peopleList[0].Dismiss();
        }

        ForgetPeopleAssignment();
        maxPeople = 0;
    }
}
