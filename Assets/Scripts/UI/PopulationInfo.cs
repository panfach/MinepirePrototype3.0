using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulationInfo : MonoBehaviour
{
    public TextMeshProUGUI population, homeless, laborers, hunters, artisans, tailors, fishermen;

    public void Refresh()
    {
        population.text = CreatureManager.villagerPopulation.ToString();
        homeless.text = VillageData.Homeless.ToString();
        laborers.text = VillageData.Workers(Profession.LABORER).ToString();
        hunters.text = VillageData.Workers(Profession.HUNTER).ToString();
        artisans.text = VillageData.Workers(Profession.ARTISAN).ToString();
        tailors.text = VillageData.Workers(Profession.TAILOR).ToString();
        fishermen.text = VillageData.Workers(Profession.FISHERMAN).ToString();
    }
}
