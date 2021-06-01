using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SmallCreatureInfo : SmallInfo
{
    [Header("Objects")]
    public GameObject infoBox;
    public GameObject iconBox;

    [Header("Values")]
    public TextMeshProUGUI _name;
    public Slider healthPoints;


    public void Init(Creature _creature)
    {
        instance = _creature;
        objectTransform = _creature.transform;
        healthPoints.maxValue = _creature.CrtData.MaxHealth;
        Refresh();
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

        _name.text = instance.CrtData.Name;
        healthPoints.value = instance.Health.Value;
    }
}
