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
    public GameObject productionSection;
    public GameObject constructionProcessSection;
    public GameObject warehouseSection;

    [Header("Components")]
    public TextMeshProUGUI buildingName, people;
    public GameObject[] peoplePlace;
    public TextMeshProUGUI[] peopleName;
    public TextMeshProUGUI[] peopleStatus;
    public GameObject[] peopleTrueSign;
    public GameObject[] productionRecipe;
    public TextMeshProUGUI[] recipeName;
    public TextMeshProUGUI[] recipeQueue;
    public Slider[] productionProcessSlider;
    public Slider constructionProcessSlider;
    public GameObject[] warehousePack;
    public Slider[] warehouseSlider;
    public TextMeshProUGUI[] warehouseResName;
    public TextMeshProUGUI[] warehouseValue;

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


    public void Set(bool state)
    {
        if (SaveLoader.state == SaveLoadState.EXITING) return;

        BuildingInfoTurnedOn = state;
        StateManager.BuildingInfo = state;
        if (state)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        if (!buildingInfoTurnedOn || SaveLoader.state == SaveLoadState.EXITING) return;

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

            if (rs.Appointer.MaxAppointments(AppointerType.VILLAGER) > 0)
            {
                peopleSection.SetActive(true);
                people.text = $"{rs.Appointer.People}/{rs.Appointer.MaxAppointments(AppointerType.VILLAGER)}";

                for (int i = 0; i < peopleName.Length; i++)
                {
                    bool res = i < rs.Appointer.MaxAppointments(AppointerType.VILLAGER);
                    peoplePlace[i].SetActive(res);
                    if (res)
                    {
                        if (i < rs.Appointer.People)
                        {
                            peopleTrueSign[i].SetActive(true);
                            peopleName[i].text = rs.Appointer.GetPeople(i).entity.CrtProp.Name;
                            peopleStatus[i].text = (rs.Appointer.GetPeople(i).entity.CrtProp.PlaceOfStay == rs) ? "внутри" : "на улице";
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

            if (rs.Production != null)
            {
                productionSection.SetActive(true);

                for (int i = 0; i < productionRecipe.Length; i++)
                {
                    bool res = i < rs.Production.RecipeCount;
                    productionRecipe[i].SetActive(res);
                    if (res)
                    {
                        recipeName[i].text = activeBuilding.Production.Recipe(i).recipeName;       
                        recipeQueue[i].text = activeBuilding.Production.Recipe(i).Queue.ToString();
                        productionProcessSlider[i].value = activeBuilding.Production.Recipe(i).Progress;
                    }
                }
            }
            else productionSection.SetActive(false);

            if (rs.Inventory != null)
            {
                warehouseSection.SetActive(true);
                for (int i = 0; i < warehousePack.Length; i++)
                {
                    bool res = i < rs.Inventory.PacksAmount;
                    warehousePack[i].SetActive(res);
                    if (!res) continue;

                    rs.Inventory.Look(i, out ResourceIndex resInd, out float resVal);
                    //Debug.Log(resInd);
                    warehouseSlider[i].maxValue = rs.Inventory.PackSize;
                    warehouseSlider[i].value = resVal;
                    if (resInd == ResourceIndex.NONE)
                        warehouseResName[i].text = "";
                    else
                        warehouseResName[i].text = DataList.GetResource(resInd).Name_rus;
                    warehouseValue[i].text = resVal.ToString("F1");
                }
            }
            else warehouseSection.SetActive(false);
        }
    }

    public void Dismiss(int ind)                                                                      // Theese 4 methods should be called "OnClick..."
    {
        activeBuilding.Appointer.Remove(AppointerType.VILLAGER, ind);
    }

    public void AddItemToQueue(int recipeInd)
    {
        activeBuilding.Production.ChangeQueue(recipeInd, 1);
    }

    public void RemoveItemFromQueue(int recipeInd)
    {
        activeBuilding.Production.ChangeQueue(recipeInd, -1);
    }

    public void DeleteBuilding()
    {
        activeBuilding.Die();
    }
}
