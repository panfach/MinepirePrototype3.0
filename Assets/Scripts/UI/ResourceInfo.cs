using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceInfo : MonoBehaviour
{
    public TextMeshProUGUI rawVenison, rawDeerSkins, deerSkins, apples, wildberries, sticks, cobblestones;
    public TextMeshProUGUI stoneSpear, roughClothesFromSkins, fish, jerky;

    public void Refresh()
    {
        rawVenison.text = VillageData.Resources(ResourceIndex.RAWVENISON).ToString("F0");
        rawDeerSkins.text = VillageData.Resources(ResourceIndex.RAWDEERSKIN).ToString("F0");
        apples.text = VillageData.Resources(ResourceIndex.APPLE).ToString("F0");
        wildberries.text = VillageData.Resources(ResourceIndex.WILDBERRIES).ToString("F0");
        sticks.text = VillageData.Resources(ResourceIndex.STICKS).ToString("F0");
        cobblestones.text = VillageData.Resources(ResourceIndex.COBBLESTONES).ToString("F0");
        deerSkins.text = VillageData.Resources(ResourceIndex.DEERSKIN).ToString("F0");
        stoneSpear.text = VillageData.Resources(ResourceIndex.STONESPEAR).ToString("F0");
        roughClothesFromSkins.text = VillageData.Resources(ResourceIndex.ROUGHCLOTHINGOFSKINS).ToString("F0");
        fish.text = VillageData.Resources(ResourceIndex.FISH).ToString("F0");
        jerky.text = VillageData.Resources(ResourceIndex.JERKY).ToString("F0");
    }
}
