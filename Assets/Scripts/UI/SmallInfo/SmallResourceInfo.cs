using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SmallResourceInfo : SmallInfo
{
    [Header("Objects")]
    public GameObject infoBox;

    [Header("Values")]
    public TextMeshProUGUI resourceName, size;


    public void Init(Item _item)
    {
        instance = _item;
        objectTransform = _item.transform;
        gameObject.SetActive(false);
    }

    public override void SetInfoBox(bool state)
    {
        infoBox.SetActive(state);
        if (state) needToActivate = true;
    }

    //public override void SetIconBox(bool state)
    //{
    //    iconBox.SetActive(state);
    //    if (state) needToActivate = true;
    //}

    public override void Refresh()
    {
        if (!gameObject.activeSelf) return;

        if (instance.Inventory.PacksAmount > 0) resourceName.text = DataList.GetResource(instance.Inventory.StoredRes[0]).Name;
        size.text = instance.Inventory.StoredVal[0].ToString("F1");
    }
}
