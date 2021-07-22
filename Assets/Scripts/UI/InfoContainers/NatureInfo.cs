using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NatureInfo : InfoContainer
{
    public static Nature activeNature;
    static bool turnedOn = false;

    [Header("Windows and sections")]
    public GameObject natureInfoWindow;
    public GameObject[] resourceField;

    [Header("Components")]
    public TextMeshProUGUI _name;
    public Slider[] resourceSlider;
    public TextMeshProUGUI[] resourceName;
    public TextMeshProUGUI[] resourceValue;
    public GameObject[] interactButton;
    public TextMeshProUGUI[] interactButtonText;

    public bool TurnedOn { get => turnedOn; }


    public void OnClickExtractButton(int index)
    {
        if (activeNature.ResourceDeposit.Extractable(index))
            TurnOffExtractable(index);
        else
            TurnOnExtractable(index);
    }


    public override void Set(bool state)
    {
        if (SaveLoader.state == SaveLoadState.EXITING) return;

        natureInfoWindow.SetActive(state);
        StateManager.ResourceSourceInfo = state;
        turnedOn = state;
        if (state) Refresh();
    }

    public override void Refresh()
    {
        if (!turnedOn || SaveLoader.state == SaveLoadState.EXITING) return;

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

    public void TurnOnExtractable(int index)
    {
        activeNature.ResourceDeposit.AddToExtractionQueue(index);
    }

    public void TurnOffExtractable(int index)
    {
        activeNature.ResourceDeposit.ForceRemoveFromExtractionQueue(index);
    }
}
