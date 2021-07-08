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
        float[] res = VillageData.resources;

        rawVenison.text = res[(int)ResourceIndex.RAWVENISON].ToString("F0");
        rawDeerSkins.text = res[(int)ResourceIndex.RAWDEERSKIN].ToString("F0");
        apples.text = res[(int)ResourceIndex.APPLE].ToString("F0");
        wildberries.text = res[(int)ResourceIndex.WILDBERRIES].ToString("F0");
        sticks.text = res[(int)ResourceIndex.STICKS].ToString("F0");
        cobblestones.text = res[(int)ResourceIndex.COBBLESTONES].ToString("F0");
        deerSkins.text = res[(int)ResourceIndex.DEERSKIN].ToString("F0");
        stoneSpear.text = res[(int)ResourceIndex.STONESPEAR].ToString("F0");
        roughClothesFromSkins.text = res[(int)ResourceIndex.ROUGHCLOTHINGOFSKINS].ToString("F0");
        fish.text = res[(int)ResourceIndex.FISH].ToString("F0");
        jerky.text = res[(int)ResourceIndex.JERKY].ToString("F0");

        //Debug.Log("ResourceInfo cobblestones now: " + res[(int)ResourceIndex.COBBLESTONES].ToString("F6"));
    }
}
