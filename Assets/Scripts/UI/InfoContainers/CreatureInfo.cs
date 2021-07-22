using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreatureInfo : InfoContainer
{
    public static Creature activeCreature;
    static bool turnedOn = false;

    [Header("Windows and sections")]
    public GameObject creatureInfoWindow;

    [Header("Components")]
    public TextMeshProUGUI creatureName, gender, age, profession;
    public Slider healthSlider;
    public Slider satietySlider;
    public GameObject inventory;
    public Slider[] warehouseSlider;
    public TextMeshProUGUI[] warehouseResName;
    public TextMeshProUGUI[] warehouseValue;

    public bool TurnedOn { get => turnedOn; }


    public override void Set(bool state)
    {
        if (SaveLoader.state == SaveLoadState.EXITING) return;

        creatureInfoWindow.SetActive(state);
        StateManager.CreatureInfo = state;
        turnedOn = state;
        if (state) Refresh();
    }

    public override void Refresh()
    {
        if (!turnedOn || SaveLoader.state == SaveLoadState.EXITING) return;

        creatureName.text = activeCreature.CrtProp.Name;
        healthSlider.value = activeCreature.Health.Value;
        satietySlider.value = activeCreature.Satiety.Value;
        gender.text = activeCreature.CrtProp.Gender ? "мужчина" : "женщина";
        age.text = activeCreature.CrtProp.Age.ToString();
        if (activeCreature.Appointer != null)
            profession.text = DataList.profNameDict_rus[activeCreature.Appointer.Profession];
        else
            profession.text = DataList.profNameDict_rus[Profession.NONE];

        if (activeCreature.UIController.ReactToInventoryChanges)
        {
            inventory.SetActive(true);
            for (int i = 0; i < activeCreature.Inventory.PacksAmount; i++)
            {
                activeCreature.Inventory.Look(i, out ResourceIndex resInd, out float resVal);
                warehouseSlider[i].maxValue = activeCreature.Inventory.PackSize;
                warehouseSlider[i].value = resVal;
                if (resInd == ResourceIndex.NONE)
                    warehouseResName[i].text = "";
                else
                    warehouseResName[i].text = DataList.GetResource(resInd).Name_rus;
                warehouseValue[i].text = resVal.ToString("F1");
            }
        }
        else inventory.SetActive(false);
    }

    public void Refresh(Creature creature)
    {
        if (activeCreature == creature) Refresh();
    }
}
