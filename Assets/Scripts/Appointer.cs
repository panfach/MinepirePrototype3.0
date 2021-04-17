using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class Appointer : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] AppointerType type;
    [Tooltip("00: Villager \n01: Workplace \n02: Livingplace \n03: Warehouse")]
    [SerializeField] int[] maxAppointments = new int[AppointerTypeSize];

    [Header("Info")]
    [Tooltip("00: Villager \n01: Workplace \n02: Livingplace \n03: Warehouse")]
    [SerializeField] List<Appointer>[] appointment;

    public event SimpleEventHandler appointmentChangedEvent;                                                                     // does it work? -> InfoDisplay.Refresh();

    public static int AppointerTypeSize { get => Enum.GetNames(typeof(AppointerType)).Length; }
    public int People { get => appointment[(int)AppointerType.VILLAGER].Count; }
    public IEnumerable GetPeople() => appointment[(int)AppointerType.VILLAGER];
    public Appointer GetPeople(int i) => appointment[(int)AppointerType.VILLAGER][i];
    public IEnumerable GetWorkPlace() => appointment[(int)AppointerType.WORKPLACE];
    public Appointer GetWorkPlace(int i) => appointment[(int)AppointerType.WORKPLACE][i];
    public Appointer Work { get => (appointment[(int)AppointerType.WORKPLACE].Count > 0) ? GetWorkPlace(0) : null; }
    public IEnumerable GetLivingPlace() => appointment[(int)AppointerType.LIVINGPLACE];
    public Appointer GetLivingPlace(int i) => appointment[(int)AppointerType.LIVINGPLACE][i];
    public Appointer Home { get => (appointment[(int)AppointerType.LIVINGPLACE].Count > 0) ? GetLivingPlace(0) : null; }

    public Profession Profession
    {
        get
        {
            if (Home == null) return Profession.NONE;
            else if (Work == null) return Profession.LABORER;
            else return Work.entity.BldData.Profession;
        }
    }


    private void OnEnable()
    {                                                                   
        appointment = new List<Appointer>[AppointerTypeSize];
    }


    public bool Appoint(Appointer target)
    {
        if (!CheckAppointConditions(target)) return false;

        AddAppointment(target);
        target.AddAppointment(this);

        if (type == AppointerType.VILLAGER) VillagerHandler(target);

        return true;
    }

    public bool AppointByDragging()
    {
        Ray ray = Connector.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Appointer target;

        if (Physics.Raycast(ray, out hit, 1000f) &&
           ((target = hit.collider.GetComponent<CollideListener>()?.colliderHandler.GetComponent<Appointer>()) != null) ||                                  // Bad . Maybe create class (RayCaster)
           (target = hit.collider.GetComponent<Appointer>()) != null)
        {
            return Appoint(target);
        }
        return false;
    }

    public void Remove(AppointerType _type, int index)
    {
        if (appointment[(int)_type].Count < index) return;
        Remove(appointment[(int)_type][index]);
    }

    public void Remove(Appointer target)
    {
        AppointerType _type = target.type;
        //int index = appointment[(int)_type].IndexOf(target);

        RemoveAppointment(target);
        target.RemoveAppointment(this);
    }


    bool CheckAppointConditions(Appointer target)
    {
        if (maxAppointments[(int)target.type] != 1 && appointment[(int)target.type].Count >= maxAppointments[(int)target.type]) return false;
        if (target.maxAppointments[(int)type] != 1 && target.appointment[(int)type].Count >= target.maxAppointments[(int)type]) return false;

        return true;
    }

    void AddAppointment(Appointer target)
    {
        if (maxAppointments[(int)target.type] == 1)
        {
            Remove(target.type, 0);                                                                             // Must contain homeless++ and etc
            appointment[(int)target.type][0] = target;
        }
        else appointment[(int)target.type].Add(target);
        appointmentChangedEvent?.Invoke();                                                                      // Add special conditions
    }

    void RemoveAppointment(Appointer target)
    {
        appointment[(int)target.type].Remove(target);
        appointmentChangedEvent?.Invoke();
    }

    void VillagerHandler(Appointer target)
    {
        switch (target.type)
        {
            case AppointerType.WORKPLACE:
                VillageData.workers[(int)Profession.LABORER]--;                               // Remove(work) must laborer++
                VillageData.workers[(int)Profession]++;
                VillageData.workersCount++;                                                   // ? Add property in VillageData 
                if (Home == null) Remove(target.type, 0);
                break;
            case AppointerType.LIVINGPLACE:
                VillageData.homeless--;
                break;
        }

        // workSequence = ActSequenceList.GetWorkSequence(profession);                                             ///////////////////////////

        entity.GeneralAI.DefineBehaviour();
        
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

    //public void ForgetPeopleAssignment()               // It doesn't need, because OnDisable exists
    //{
    //    people = 0;
    //    peopleList.Clear();

    //    for (int i = 0; i < AppointerTypeSize; i++)
    //    {

    //    }
    //}

    //public void SetDefaultPeople()
    //{
    //    people = 0;
    //}


    private void OnDisable()
    {
        for (int i = 0; i < AppointerTypeSize; i++)
        {
            for (int j = 0; j < appointment[i].Count; j++)
            {
                // Remove from Appointer
            }
            appointment[i].Clear();

            //if (entity.BldData.BldType == BuildingType.LIVING)
            //    peopleList[0].Evict();                                    // Maybe use "RemovePeople()". Then its need to add some code in that function
            //else if (entity.BldData.BldType == BuildingType.HUNT)
            //    peopleList[0].Dismiss();
        }
    }
}

public enum AppointerType
{
    VILLAGER,
    LIVINGPLACE,
    WORKPLACE,
    WAREHOUSE
}

public enum AppointMode
{
    NONE,
    REASSIGNING,
    SAVINGWORK
}
