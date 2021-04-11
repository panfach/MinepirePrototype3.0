using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SmallBuildingInfo : SmallInfo
{
    [Header("Objects")]
    //public Building building;
    public GameObject infoBox;
    public GameObject iconBox;

    [Header("Values")]
    public TextMeshProUGUI buildingName, people;
    public RawImage iconImage;


    public void Init(Building _building)
    {
        instance = _building;
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

        buildingName.text = instance.BldData.Name_rus;

        if (instance.BuildSet.IsStatusConstr())
        {
            people.text = "---";
        }
        else
        {
            Building rs = instance as Building; 
            people.text = $"{rs.PeopleAppointer.People}/{rs.BldData.MaxPeople}";
        }
    }
}
