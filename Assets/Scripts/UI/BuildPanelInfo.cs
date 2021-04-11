using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildPanelInfo : MonoBehaviour
{
    public TextMeshProUGUI buildingName, constructionCost;
    public GameObject[] resource;
    public TextMeshProUGUI[] resourceName;
    public TextMeshProUGUI[] resourceValue;

    public void Refresh(BuildingIndex index)
    {
        if (!gameObject.activeSelf) return;

        Building bs = DataList.GetBuildingObj(index).GetComponent<Building>();
        buildingName.text = bs.BldData.Name_rus;
        constructionCost.text = bs.BldData.ConstrCost.ToString("F0");

        ResourceQuery resCost = bs.BldData.ResourceCost;
        for (int i = 0; i < resource.Length; i++)
        {
            if (i < resCost.index.Length)
            {
                resource[i].SetActive(true);
                resourceName[i].text = DataList.GetResource(resCost.index[i]).Name_rus;
                resourceValue[i].text = resCost.indexVal[i].ToString();
            }
            else resource[i].SetActive(false);
        }
    }
}
