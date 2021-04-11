using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SmallVillagerInfo : SmallInfo
{
    [Header("Objects")]
    public Villager villager;
    public GameObject infoBox;
    public GameObject iconBox;

    [Header("Values")]
    public TextMeshProUGUI villagerName, age;
    public RawImage iconImage;

    public void Init(Villager _villager)
    {
        villager = _villager;
        objectTransform = villager.transform;
        villager.smallInfo = this;
        gameObject.SetActive(false);
    }

    public override void Refresh()
    {
        if (!gameObject.activeSelf) return;

        if (infoBox.activeSelf)
        {
            villagerName.text = villager.data.Name;
            age.text = villager.data.Age.ToString();
        }
        if (iconBox.activeSelf)
        { 

        }
    }
}
