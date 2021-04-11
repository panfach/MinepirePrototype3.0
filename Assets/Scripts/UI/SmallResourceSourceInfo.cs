using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SmallResourceSourceInfo : SmallInfo                                                 // Change Name to SmallNatureInfo
{
    [Header("Objects")]
    public GameObject infoBox;
    public GameObject iconBox;

    [Header("Values")]
    public TextMeshProUGUI resourceSourceName;
    public RawImage iconImage;


    public void Init(Nature _nature)
    {
        instance = _nature;
        objectTransform = instance.transform;
        gameObject.SetActive(false);
    }

    public override void SetInfoBox(bool state)
    {
        infoBox.SetActive(state);
        if (state) needToActivate = true;
    }

    public override void SetIconBox(bool state)
    {
        iconBox.SetActive(state);
        if (state) needToActivate = true;
    }

    public override void Refresh()
    {
        if (!gameObject.activeSelf) return;

        if (infoBox.activeSelf)
            resourceSourceName.text = instance.NtrData.Name_rus;
        if (iconBox.activeSelf)
            { }
    }
}
