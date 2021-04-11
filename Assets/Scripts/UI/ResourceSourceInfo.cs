using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceSourceInfo : MonoBehaviour
{
    public static Nature activeNature;
    public static bool natureInfoTurnedOn = false;

    [Header("Managed object")]
    public GameObject natureInfoWindow;
    public GameObject[] resourceField;

    [Header("Components")]
    public TextMeshProUGUI _name;
    public Slider[] resourceSlider;
    public TextMeshProUGUI[] resourceName;
    public TextMeshProUGUI[] resourceValue;
    public GameObject[] interactButton;
    public TextMeshProUGUI[] interactButtonText;

    // -------------------------------------------------------------------------------------------------- //


    // -------------------------------------------------------------------------------------------------- //

    public bool NatureInfoTurnedOn
    {
        get
        {
            return natureInfoTurnedOn;
        }
        set
        {
            natureInfoWindow.SetActive(value);
            StateManager.ResourceSourceInfo = value;
            natureInfoTurnedOn = value;
        }
    }

    public void Open(bool state)
    {
        NatureInfoTurnedOn = state;
        StateManager.ResourceSourceInfo = state;
        if (state)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        if (natureInfoTurnedOn)
        {
            _name.text = activeNature.NtrData.Name_rus;

            for (int i = 0; i < interactButton.Length; i++)
            {
                interactButton[i].SetActive(false);
            }

            int j = 0;
            for (int i = 0; i < activeNature.ResourceDeposit.Size; i++)
            {
                resourceField[i].SetActive(true);
                resourceName[i].text = DataList.GetResource(activeNature.ResourceDeposit.Index(i)).Name_rus;
                resourceSlider[i].maxValue = activeNature.NtrData.Amount(i);
                resourceSlider[i].value = activeNature.ResourceDeposit.Amount(i);
                resourceValue[i].text = activeNature.ResourceDeposit.Amount(i).ToString("F2");

                if (i == 0 || i == 1)
                {
                    interactButton[i].SetActive(true);
                    interactButtonText[i].text = "Собрать\n" + DataList.GetResource(activeNature.ResourceDeposit.Index(i)).Name_rus;
                }

                j++;
            }
            for (int i = j; i < resourceField.Length; i++)
            {
                resourceField[i].SetActive(false);
            }
        }
    }

    public void TurnOnExtractable(int index)
    {
        activeNature.ResourceDeposit.AddToExtractionQueue(index);
    }

    public void TurnOffExtractable(int index)
    {
        activeNature.ResourceDeposit.ForceRemoveFromExtractionQueue(index);
    }
}
