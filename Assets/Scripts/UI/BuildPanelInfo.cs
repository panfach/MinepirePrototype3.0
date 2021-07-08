using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TechnologySystem;

public class BuildPanelInfo : MonoBehaviour
{
    public TextMeshProUGUI buildingName, constructionCost;
    public TextMeshProUGUI techName;
    public GameObject[] resource;
    public TextMeshProUGUI[] resourceName;
    public TextMeshProUGUI[] resourceValue;
    public Color accessibleColor, inaccessibleColor;

    public void Refresh(BuildingIndex index)
    {
        if (!gameObject.activeSelf) return;

        Building bs = DataList.GetBuildingObj(index).GetComponent<Building>();
        buildingName.text = bs.BldData.Name_rus;
        constructionCost.text = bs.BldData.ConstrCost.ToString("F0");

        if (bs.BldData.RequiredTech == TechIndex.STARTTECH)
        {
            techName.text = "";
            techName.color = accessibleColor;
        }
        else
        {
            techName.text = DataList.GetTech(bs.BldData.RequiredTech).Name_rus;
            techName.color = (Connector.techManager.IsTechResearched(bs.BldData.RequiredTech)) ? accessibleColor : inaccessibleColor;
        }

        ResourceQuery resCost = bs.BldData.ResourceCost;
        float amount;
        for (int i = 0; i < resource.Length; i++)
        {
            if (i < resCost.index.Length)
            {
                resource[i].SetActive(true);
                resourceName[i].text = DataList.GetResource(resCost.index[i]).Name_rus;

                if ((amount = VillageData.CheckWarehouseResourceAmount(resCost.index[i])) >= resCost.indexVal[i])
                    resourceValue[i].color = accessibleColor;
                else
                    resourceValue[i].color = inaccessibleColor;
                resourceValue[i].text = $"{amount:F0}/{resCost.indexVal[i]}";
            }
            else resource[i].SetActive(false);
        }
    }
}
