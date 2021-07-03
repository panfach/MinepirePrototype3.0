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
    }


    public bool TryToResearch(TechIndex techIndex)
    {
        if (!IsAvailableForResearch(techIndex)) return false;

        VillageData.SpendResource(DataList.GetTech(techIndex).RequiredRes);
        techStatus[(int)techIndex] = TechStatus.RESEARCHED;

        changedEvent?.Invoke();
        return true;
    }
}

public enum TechStatus
{
    NOTRESEARCHED,
    RESEARCHING,
    RESEARCHED
}


