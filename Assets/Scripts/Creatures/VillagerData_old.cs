using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VillagerData
{
    public static Dictionary<Profession, string> profNameDict_rus = new Dictionary<Profession, string>()                       // This thing moved to DataList
    {
        {Profession.NONE, "Нет"},
        {Profession.LABORER, "Разнорабочий"},
        {Profession.HUNTER, "Охотник"}
    };

    public bool Gender { get; } // true - male ; false - female
    public string Name { get; }
    public float Age { get; }
    public Building home;
    public Building work;
    public Profession profession;
    public ActSequence workSequence;
    public Villager villagerAgent;
    public float defaultDamage;
    public float satiety;

    public VillagerData(bool gender, string name, float age, Building _home = null, Building _work = null, float _satiety = 2.0f)
    {
        Gender = gender;
        Name = name;
        Age = age;
        home = _home;
        work = _work;
        satiety = _satiety;

        profession = Profession.NONE;
        workSequence = null;
        villagerAgent = null;
        defaultDamage = 1.0f;
    }

    public bool TryToSettleByDragging()
    {
        Ray ray = Connector.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Building building;

        if (Physics.Raycast(ray, out hit, 1000f) && (building = hit.collider.GetComponent<CollideListener>()?.colliderHandler.GetComponent<Building>()) != null)
        {
            if (building.BuildSet.ConstrStatus == ConstructionStatus.READY)
            {
                if (building.BldData.Index == BuildingIndex.PRIMHUT)
                {
                    if (profession == Profession.NONE)
                        return Settle(building);
                    else
                        return Settle(building, AppointMode.SAVINGWORK);
                }
                else if (building.BldData.Index == BuildingIndex.HUNTHUT)
                    return AssignJob(building);
            }
        }
        return false;
    }

    public bool TryToSettle(Building building)
    {
        if (building.BldData.Index == BuildingIndex.PRIMHUT && building.BuildSet.ConstrStatus == ConstructionStatus.READY)
        {
            return Settle(building);
        }

        return false;
    }

    public bool Settle(Building building, AppointMode mode = AppointMode.NONE)
    {
        bool defineBehaviour;

        if (building.PeopleAppointer.AddPeople(this))
        {
            if (home != null)
            {
                if (mode == AppointMode.SAVINGWORK)
                {
                    Evict(AppointMode.SAVINGWORK);
                }
                else if (mode != AppointMode.REASSIGNING)
                {
                    Evict();
                }
            }

            if (profession == Profession.NONE)
            {
                VillageData.workers[(int)Profession.LABORER]++;
                profession = Profession.LABORER;
            }

            if (mode != AppointMode.SAVINGWORK || (mode != AppointMode.SAVINGWORK && villagerAgent.state == VillagerState.GOTOHOUSE && villagerAgent.destinationBuilding == home))
            {
                defineBehaviour = true;
            }
            else
            {
                defineBehaviour = false;
            }

            home = building;
            VillageData.homeless--;
            workSequence = ActSequenceList.GetWorkSequence(profession);
            //villagerAgent.workAction = workSequence.actions;

            if (defineBehaviour && villagerAgent != null) villagerAgent.DefineBehaviour();
            InfoDisplay.Refresh();
            return true;
        }
        return false;
    }

    public bool Evict(AppointMode mode = AppointMode.NONE)
    {
        Debug.Log("VillagerData.Evict() TRY : " + Name);
        if (home.PeopleAppointer.RemovePeople(this))
        {
            Debug.Log("VillagerData.Evict() " + Name);
            home = null;
            VillageData.homeless++;
            if (mode != AppointMode.SAVINGWORK)
            {
                if (work != null)
                {
                    work = null;
                    VillageData.workers[(int)profession]--;
                    VillageData.workersCount--;
                }
                else if (profession == Profession.LABORER)
                {
                    VillageData.workers[(int)profession]--;
                }
                profession = Profession.NONE;
                workSequence = null;
                villagerAgent.actions = null;
                if (villagerAgent != null) villagerAgent.DefineBehaviour();
            }
            villagerAgent.SetSmallInfo();
            InfoDisplay.Refresh();
            return true;
        }
        return false;
    }

    public bool AssignJob(Building building, AppointMode mode = AppointMode.NONE)
    {
        if (home != null && building.PeopleAppointer.AddPeople(this))
        {
            if (work != null && mode != AppointMode.REASSIGNING)
            {
                Dismiss();
            }

            work = building;
            VillageData.workersCount++;
            VillageData.workers[(int)Profession.LABORER]--;
            profession = building.BldData.Profession;
            workSequence = ActSequenceList.GetWorkSequence(profession);
            //villagerAgent.actions = workSequence.actions;
            VillageData.workers[(int)profession]++;

            if (villagerAgent != null)
            {
                Debug.Log("DEFINE BEHAVIOUR FROM REASSIGNING JOB");
                villagerAgent.workInd = 0;
                villagerAgent.DefineBehaviour();
            }
            InfoDisplay.Refresh();
            return true;
        }
        return false;
    }

    public bool Dismiss(AppointMode mode = AppointMode.NONE)
    {
        if (work.PeopleAppointer.RemovePeople(this))
        {
            if (mode != AppointMode.SAVINGWORK)
            {
                work = null;
                VillageData.workers[(int)profession]--;
                VillageData.workersCount--;
                profession = Profession.LABORER;
                workSequence = ActSequenceList.GetWorkSequence(profession);
                //villagerAgent.actions = workSequence.actions;
                VillageData.workers[(int)Profession.LABORER]++;

                if (villagerAgent != null)
                {
                    villagerAgent.DefineBehaviour();
                    villagerAgent.workInd = 0;
                }
                InfoDisplay.Refresh();
            }
            return true;
        }
        return false;
    }
}
