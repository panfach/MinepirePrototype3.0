using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceInfo : MonoBehaviour
{
    public TextMeshProUGUI rawVenison, rawDeerSkins, apples, wildberries, sticks, cobblestones;

    public void Refresh()
    {
        float[] res = VillageData.resources;

        rawVenison.text = res[(int)ResourceIndex.RAWVENISON].ToString("F0");
        rawDeerSkins.text = res[(int)ResourceIndex.RAWDEERSKIN].ToString("F0");
        apples.text = res[(int)ResourceIndex.APPLE].ToString("F0");
        wildberries.text = res[(int)ResourceIndex.WILDBERRIES].ToString("F0");
        sticks.text = res[(int)ResourceIndex.STICKS].ToString("F0");
        cobblestones.text = res[(int)ResourceIndex.COBBLESTONES].ToString("F0");
    }
}
