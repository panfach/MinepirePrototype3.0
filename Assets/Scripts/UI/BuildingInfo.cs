using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class BuildingInfo : MonoBehaviour
{
    public static Building activeBuilding;
    public static bool buildingInfoTurnedOn = false; 

    [Header("Managed object")]
    public GameObject buildingInfoWindow;
    public GameObject peopleSection;
    public GameObject constructionProcessSection;
    public GameObject warehouseSection;

    [Header("Components")]
    public TextMeshProUGUI buildingName, people;
    public GameObject[] peoplePlace;
    public TextMeshProUGUI[] peopleName;
    public TextMeshProUGUI[] peopleStatus;
    public GameObject[] peopleTrueSign;
    public Slider constructionProcessSlider;
    public Slider[] warehouseSlider;
    public TextMeshProUGUI[] warehouseResName;
    public TextMeshProUGUI[] warehouseValue;

    // -------------------------------------------------------------------------------------------------- //

    /*    private void LateUpdate()
        {
            if (cameraAngleDifference)
            {
                AdjustAllSmallInfoAngles();
                cameraAngleDifference = false;
            }
            if (cameraHeightDifference)
            {
                additionalSmallInfoScale = CellMetrics.XYdir * 10 * Mathf.Pow((1f - CameraScript.Height), 1f);
                AdjustAllSmallInfoScales();
                cameraHeightDifference = false;
            }
        }*/

    // -------------------------------------------------------------------------------------------------- //

    public bool BuildingInfoTurnedOn
    {
        get
        {
            return buildingInfoTurnedOn;
        }
        set
        {
            buildingInfoWindow.SetActive(value);
            StateManager.BuildingInfo = value;
            buildingInfoTurnedOn = value;
        }
    }

    public void Open(bool state)
    {
        BuildingInfoTurnedOn = state;
        StateManager.BuildingInfo = state;
        if (state)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        if (buildingInfoTurnedOn)
        {
            buildingName.text = activeBuilding.BldData.Name_rus;

            if (activeBuilding.BuildSet.ConstrStatus == ConstructionStatus.CONSTR)
            {
                warehouseSection.SetActive(false);

                peopleSection.SetActive(false);

                constructionProcessSection.SetActive(true);
                constructionProcessSlider.maxValue = activeBuilding.BldData.ConstrCost;
                constructionProcessSlider.value = activeBuilding.BuildSet.Process;
            }
            else 
            {
                Building rs = activeBuilding;

                constructionProcessSection.SetActive(false);

                if (rs.BldData.MaxPeople > 0)
                {
                    peopleSection.SetActive(true);
                    people.text = $"{rs.PeopleAppointer.People}/{rs.BldData.MaxPeople}";

                    for (int i = 0; i < peopleName.Length; i++)
                    {
                        bool res = i < activeBuilding.BldData.MaxPeople;
                        peoplePlace[i].SetActive(res);
                        if (res)
                        {
                            if (i < rs.PeopleAppointer.People)
                            {
                                peopleTrueSign[i].SetActive(true);
                                peopleName[i].text = rs.PeopleAppointer.GetPeople(i).Name;
                                peopleStatus[i].text = (rs.PeopleAppointer.GetPeople(i).villagerAgent.placeOfStay == rs) ? "внутри" : "на улице";
                            }
                            else
                            {
                                peopleTrueSign[i].SetActive(false);
                                peopleName[i].text = " ";
                                peopleStatus[i].text = "пусто";
                            }
                        }
                    }
                }
                else peopleSection.SetActive(false);

                if (rs.BldData.BldType == BuildingType.WAREHOUSE)
                {
                    warehouseSection.SetActive(true);
                    for (int i = 0; i < rs.Inventory.PacksAmount; i++)
                    {
                        rs.Inventory.Look(i, out ResourceIndex resInd, out float resVal);
                        Debug.Log(resInd);
                        warehouseSlider[i].maxValue = rs.Inventory.PackSize;
                        warehouseSlider[i].value = resVal;
                        if (resInd == ResourceIndex.NONE || resInd == ResourceIndex.DEERSKIN)
                            warehouseResName[i].text = "";
                        else
                            warehouseResName[i].text = DataList.GetResource(resInd).Name_rus;
                        warehouseValue[i].text = resVal.ToString("F1");
                    }
                }
                else warehouseSection.SetActive(false);
            }
        }
    }

    public void DeleteBuilding()
    {
        activeBuilding.Die();
    }
}
