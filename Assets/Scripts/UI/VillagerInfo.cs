using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VillagerInfo : MonoBehaviour
{
    public static Villager activeVillager;
    public static GameObject smallVillagerInfoPrefab;

    static bool villagerInfoTurnedOn = false;

    [Header("Managed object")]
    public GameObject villagerInfoWindow;

    [Header("Components")]
    public TextMeshProUGUI villagerName, gender, age, profession;
    public Slider satietySlider;
    public Slider[] warehouseSlider;
    public TextMeshProUGUI[] warehouseResName;
    public TextMeshProUGUI[] warehouseValue;

    public bool VillagerInfoTurnedOn
    {
        get
        {
            return villagerInfoTurnedOn;
        }
        set
        {
            villagerInfoWindow.SetActive(value);
            StateManager.VillagerInfo = value;
            villagerInfoTurnedOn = value;
        }
    }

    public void Open(bool state)
    {
        VillagerInfoTurnedOn = state;
        StateManager.VillagerInfo = state;
        if (state)
        {
            Refresh();
        }
    }
        
    public void Refresh()
    {
        if (villagerInfoTurnedOn)
        {
            villagerName.text = activeVillager.data.Name;
            satietySlider.value = activeVillager.data.satiety;
            gender.text = activeVillager.data.Gender ? "мужчина" : "женщина";
            age.text = activeVillager.data.Age.ToString();
            profession.text = VillagerData.profNameDict_rus[activeVillager.data.profession];

            for (int i = 0; i < activeVillager.inventory.PacksAmount; i++)
            {
                activeVillager.inventory.Look(i, out ResourceIndex resInd, out float resVal);
                warehouseSlider[i].maxValue = activeVillager.inventory.PackSize;
                warehouseSlider[i].value = resVal;
                if (resInd == ResourceIndex.NONE)
                    warehouseResName[i].text = "";
                else
                    warehouseResName[i].text = DataList.GetResource(resInd).Name_rus;
                warehouseValue[i].text = resVal.ToString("F1");
            }
        }
    }
}
