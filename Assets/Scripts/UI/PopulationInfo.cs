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
        homeless.text = VillageData.homeless.ToString();
        laborers.text = VillageData.workers[(int)Profession.LABORER].ToString();
        //workers.text = VillageData.workersCount.ToString();
        hunters.text = VillageData.workers[(int)Profession.HUNTER].ToString();
        artisans.text = VillageData.workers[(int)Profession.ARTISAN].ToString();
        tailors.text = VillageData.workers[(int)Profession.TAILOR].ToString();
        fishermen.text = VillageData.workers[(int)Profession.FISHERMAN].ToString();
    }
}
