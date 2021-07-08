using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TechnologySystem;


public class TechManager : MonoBehaviour
{
    [SerializeField] TechGraph graph;
    [SerializeField] TechStatus[] techStatus;

    public event SimpleEventHandler changedEvent;

    public TechGraph Graph => graph;
    public TechStatus GetTechStatus(TechIndex techInd) { return techStatus[(int)techInd]; }
    public bool IsTechResearched(TechIndex techInd) { return techStatus[(int)techInd] == TechStatus.RESEARCHED; }
    public bool IsAvailableForResearch(TechIndex techIndex)
    {
        TechData data = DataList.GetTech(techIndex);
        if (!data.Node.CheckInputResearchings) return false;
        if (!Connector.statistics.CheckResources(data.RequiredStatRes)) return false;
        if (!VillageData.CheckResourceAvailability(data.RequiredRes)) return false;

        return true;
    }


    void Awake()
    {
        techStatus = new TechStatus[Enum.GetNames(typeof(TechIndex)).Length];
        techStatus[(int)TechIndex.STARTTECH] = TechStatus.RESEARCHED;
    }


    public bool TryToResearch(TechIndex techIndex, bool instant = false)
    {
        if (!IsAvailableForResearch(techIndex)) return false;

        VillageData.SpendResource(DataList.GetTech(techIndex).RequiredRes);
        techStatus[(int)techIndex] = TechStatus.RESEARCHED;

        changedEvent?.Invoke();
        return true;
    }

    public void InitTechStatus(TechIndex techInd, TechStatus status)
    {
        techStatus[(int)techInd] = status;
        changedEvent?.Invoke();
    }
}

public enum TechStatus
{
    NOTRESEARCHED,
    RESEARCHING,
    RESEARCHED
}


