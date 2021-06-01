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
    public int MaxAppointments(AppointerType type) => maxAppointments[(int)type];
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


    /// <summary>
    /// Performs appointing to this appointer with all table changes
    /// </summary>
    /// <returns>Successful</returns>
    public bool Appoint(Appointer target)
    {
        if (!CheckAppointConditions(target)) return false;

        AddAppointment(target);
        target.AddAppointment(this);

        if (type == AppointerType.VILLAGER) VillagerAddHandler(target);                                 // Bad
        if (type == AppointerType.LIVINGPLACE) LivingPlaceAddHandler(target);
        if (type == AppointerType.WORKPLACE) WorkPlaceAddHandler(target);

        return true;
    }

    /// <summary>
    /// Performs appointing to this appointer with all table changes (Appointment is selected by mouse)
    /// </summary>
    /// <returns>Successful</returns>
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

    /// <summary>
    /// Performs removing of appointment from this appointer with all table changes
    /// </summary>
    public void Remove(AppointerType _type, int index)
    {
        if (appointment[(int)_type].Count < index) return;
        Remove(appointment[(int)_type][index]);
    }

    /// <summary>
    /// Performs removing of appointment from this appointer with all table changes
    /// </summary>
    public bool Remove(Appointer target)
    {
        if (!CheckRemoveConditions(target)) return false;

        RemoveAppointment(target);
        target.RemoveAppointment(this);

        if (type == AppointerType.VILLAGER) VillagerRemoveHandler(target);
        if (type == AppointerType.LIVINGPLACE) LivingPlaceRemoveHandler(target);
        if (type == AppointerType.WORKPLACE) WorkPlaceRemoveHandler(target);

        return true;
    }

    public bool CheckAppointment(Appointer target)
    {
        for (int i = 0; i < AppointerTypeSize; i++)
        {
            for (int j = 0; j < appointment[i].Count; j++)
            {
                if (appointment[i][j] == target) return true;
            }
        }

        return false;
    }


    bool CheckAppointConditions(Appointer target)
    {
        if (target == null) return false;
        if (maxAppointments[(int)target.type] != 1 && appointment[(int)target.type].Count >= maxAppointments[(int)target.type]) return false;
        if (target.maxAppointments[(int)type] != 1 && target.appointment[(int)type].Count >= target.maxAppointments[(int)type]) return false;

        return true;
    }

    bool CheckRemoveConditions(Appointer target)
    {
        if (target == null) return false;
        if (!CheckAppointment(target)) return false;

        return true;
    }

    /// <summary>
    /// Performs appointing to this appointer WITHOUT table changes
    /// </summary>
    void AddAppointment(Appointer target)
    {
        if (maxAppointments[(int)target.type] == 1)
        {
            Remove(target.type, 0);                                                                
            appointment[(int)target.type][0] = target;
        }
        else appointment[(int)target.type].Add(target);
        appointmentChangedEvent?.Invoke();                                                                      // Add special conditions
    }

    /// <summary>
    /// Performs removing appointment form this appointer WITHOUT table changes
    /// </summary>
    void RemoveAppointment(Appointer target)
    {
        appointment[(int)target.type].Remove(target);
        appointmentChangedEvent?.Invoke();
    }

    void VillagerAddHandler(Appointer target)
    {
        switch (target.type)
        {
            case AppointerType.WORKPLACE:
                VillageData.workers[(int)Profession.LABORER]--;                            
                VillageData.workers[(int)Profession]++;
                VillageData.workersCount++;                                                   // ? Add property in VillageData 
                if (Home == null) Remove(AppointerType.WORKPLACE, 0);
                break;
            case AppointerType.LIVINGPLACE:
                VillageData.homeless--;
                break;
        }

        // workSequence = ActSequenceList.GetWorkSequence(profession);                        ///////////////////////////

        entity.GeneralAI.DefineBehaviour();     
    }

    void VillagerRemoveHandler(Appointer target)
    {
        switch (target.type)
        {
            case AppointerType.WORKPLACE:
                VillageData.workersCount--;                                                   // ? Add property in VillageData
                VillageData.workers[(int)Profession]--;
                VillageData.workers[(int)Profession.LABORER]++;
                break;
            case AppointerType.LIVINGPLACE:
                if (Work != null) Remove(AppointerType.WORKPLACE, 0);
                VillageData.homeless++;
                break;
        }

        entity.GeneralAI.DefineBehaviour();
    }

    void LivingPlaceAddHandler(Appointer target)
    {
        switch (target.type)
        {
            case AppointerType.VILLAGER:
                target.VillagerAddHandler(this);
                break;
        }
    }

    void LivingPlaceRemoveHandler(Appointer target)
    {
        switch (target.type)
        {
            case AppointerType.VILLAGER:
                target.VillagerRemoveHandler(this);
                break;
        }
    }

    void WorkPlaceAddHandler(Appointer target)
    {
        switch (target.type)
        {
            case AppointerType.VILLAGER:
                target.VillagerAddHandler(this);
                break;
        }
    }

    void WorkPlaceRemoveHandler(Appointer target)
    {
        switch (target.type)
        {
            case AppointerType.VILLAGER:
                target.VillagerRemoveHandler(this);
                break;
        }
    }


    private void OnDisable()
    {
        for (int i = 0; i < AppointerTypeSize; i++)
        {
            for (int j = 0; j < appointment[i].Count; j++)
            {
                Remove((AppointerType)i, j);
            }
            appointment[i].Clear();
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
