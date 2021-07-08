using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechnologySystem;
using XNode;
using TMPro;

public class ResearchInfo : MonoBehaviour
{
    public static bool turnedOn = false;
    public TechIndex selectedTech;

    public GameObject researchWindow;
    public Image[] techButtonImages;
    public GameObject descriptionWindow;
    public TextMeshProUGUI techName;
    public GameObject[] reqRes;
    public GameObject[] reqStatRes;
    public TextMeshProUGUI[] reqResName;
    public TextMeshProUGUI[] reqResValue;
    public TextMeshProUGUI[] reqStatResName;
    public TextMeshProUGUI[] reqStatResValue;
    public TextMeshProUGUI descriptionText;
    public Image researchButton;
    public Color researchedColor;
    public Color availableColor;
    public Color unavailableColor;
    public Color standardTextColor;
    public Color lackTextColor;

    public bool TurnedOn
    {
        get
        {
            return turnedOn;
        }
        set
        {
            researchWindow.SetActive(value);
            turnedOn = value;
        }
    }


    void OnEnable()
    {
        Connector.statistics.changedEvent += RefreshDescription;
        Connector.statistics.changedEvent += RefreshGraph;
        Connector.techManager.changedEvent += RefreshDescription;
        Connector.techManager.changedEvent += RefreshGraph;
    }

    public void OnClickTechButton(int techIndex)
    {
        TurnOnDescription((TechIndex)techIndex);
    }

    public void OnClickResearch()
    {
        bool result = Connector.techManager.TryToResearch(selectedTech);

        if (!result)
        {
            Notification.Invoke(NotifType.RESRESEARCH);
        }
    }


    public void Set(bool state)
    {
        TurnedOn = state;
        if (state)
        {
            TurnOffDescription();
            RefreshGraph();
        }
    }

    public void TurnOnDescription(TechIndex techIndex)
    {
        descriptionWindow.SetActive(true);
        selectedTech = techIndex;

        TechData data = DataList.GetTech(selectedTech);
        techName.text = data.Name;
        float amount;
        ResourceIndex resInd;
        for (int i = 0; i < reqRes.Length; i++)
        {
            if (i < data.RequiredRes.index.Length)
            {
                resInd = data.RequiredRes.index[i];
                reqRes[i].SetActive(true);
                reqResName[i].text = DataList.GetResource(data.RequiredRes.index[i]).Name_rus;
                if ((amount = VillageData.CheckWarehouseResourceAmount(resInd)) >= data.RequiredRes.indexVal[i])
                    reqResValue[i].color = standardTextColor;
                else
                    reqResValue[i].color = lackTextColor;
                reqResValue[i].text = $"{amount:F0}/{data.RequiredRes.indexVal[i]}";
            }
            else reqRes[i].SetActive(false);
        }
        for (int i = 0; i < reqStatRes.Length; i++)
        {
            if (i < data.RequiredStatRes.index.Length)
            {
                resInd = data.RequiredStatRes.index[i];
                reqStatRes[i].SetActive(true);
                reqStatResName[i].text = DataList.GetResource(resInd).Name_rus;
                if (Connector.statistics.CheckResources(resInd, data.RequiredStatRes.indexVal[i]))
                    reqStatResValue[i].color = standardTextColor;
                else
                    reqStatResValue[i].color = lackTextColor;
                reqStatResValue[i].text = $"{Connector.statistics.ReceivedResource(resInd):F0}/{data.RequiredStatRes.indexVal[i]}";
            }
            else reqStatRes[i].SetActive(false);
        }
        descriptionText.text = data.Description;
        switch (Connector.techManager.GetTechStatus(selectedTech))
        {
            case TechStatus.RESEARCHED:
                researchButton.gameObject.SetActive(false);
                break;
            case TechStatus.NOTRESEARCHED:
                researchButton.gameObject.SetActive(true);
                researchButton.color = Connector.techManager.IsAvailableForResearch(selectedTech) ? availableColor : unavailableColor;
                break;
        }
    }

    public void TurnOffDescription()
    {
        descriptionWindow.SetActive(false);
    }

    public void RefreshGraph()
    {
        for (int i = 0; i < techButtonImages.Length; i++)
        {
            TechData data = DataList.GetTech((TechIndex)i);
            switch (Connector.techManager.GetTechStatus(data.Index))
            {
                case TechStatus.RESEARCHED:
                    techButtonImages[(int)data.Index].color = researchedColor;
                    break;
                case TechStatus.NOTRESEARCHED:
                    techButtonImages[(int)data.Index].color = Connector.techManager.IsAvailableForResearch((TechIndex)i) ? availableColor : unavailableColor;
                    break;
            }
        }
    }

    public void RefreshDescription()
    {
        if (!descriptionWindow.activeSelf) return;

        TurnOnDescription(selectedTech);
    }


    public void OnDisable()
    {
        Connector.statistics.changedEvent -= RefreshDescription;
        Connector.statistics.changedEvent -= RefreshGraph;
        Connector.techManager.changedEvent -= RefreshDescription;
        Connector.techManager.changedEvent -= RefreshGraph;
    }
}
