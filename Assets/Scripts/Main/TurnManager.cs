using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class TurnManager : MonoBehaviour
{
    // Эта функция должна активироваться кнопкой "следующий ход"
    public void TurnProcess()
    {
        foreach (Creature item in CreatureManager.Villagers)
        {
            if (item.Appointer.Home != null && (item.CrtProp.PlaceOfStay == null || item.CrtProp.PlaceOfStay.entity != item.Appointer.Home.entity))
            {
                Notification.Invoke(NotifType.EMPTYHOME);
                return;
            }
        }

        Connector.effectSoundManager.PlayNextTurnSound();

        Connector.sunlight.ChangeTurn();
        TimeEvents.TurnChanging();
        Connector.panelInvoker.SetTimeScale(1);

        // Recover resource deposits
        foreach (Nature item in NatureManager.natures)
        {
            item.ResourceDeposit.Recover();
        }

        //
        RandomEvents();

        //
        SpawnRandomAnimals();

        //
        SatietyRefresh();

        //
        VillageData.Recalculate();

        //
        SetHappiness();

        //
        Connector.panelInvoker.CloseBuildingInfo();
        Connector.panelInvoker.CloseNatureInfo();
        Connector.panelInvoker.CloseCreatureInfo();
        InfoDisplay.Refresh();
    }

    void RandomEvents()
    {
        // New villagers
        float chance = Mathf.Clamp((VillageData.Happiness - 0.3f) * 2f, 0f, 1f); // foodRatio = 2 => chance = 0; foodRatio = 6 => chance = 1;
        Debug.Log("Chance = " + chance);

        if (Random.Range(0f, 1f) < chance)
        {
            for (int i = 0; i < 2; i++)
            {
                Connector.creatureManager.SpawnRandomVillager();
            }
        }
    }

    void SpawnRandomAnimals()
    {
        int x, z;
        float y;
        Vector3 spawnPos;

        for (int i = 0; i < CreatureManager.villagerPopulation / 3; i++)
        {
            x = Random.Range(1, 40);
            z = Random.Range(5, 60);

            if (CCoord.GetCell(CCoord.FromPos(new Vector3(x, 0, z))).Elevation == 0 || SmallCellGrid.cellState[x, z].slope)
            {
                i--;
                continue;
            }

            y = SCCoord.GetHeight(new SCCoord(x, z));
            spawnPos = SCCoord.GetCenter(new SCCoord(x, z), y);
            Connector.creatureManager.SpawnRandomAnimal(spawnPos);
        }
    }

    void SatietyRefresh()
    {
        foreach (Creature item in CreatureManager.Villagers)
        {
            item.Satiety.Value -= 0.25f; 
        }
    }

    void SetHappiness()
    {
        float foodRatio = VillageData.FoodAmount / CreatureManager.villagerPopulation;
        VillageData.FoodRatio = foodRatio;

        if (foodRatio < 0.5f)
        {
            VillageData.FoodServing = 0.2f;
            VillageData.Happiness -= 0.10f;
        }
        else if (foodRatio < 1.0f)
        {
            VillageData.FoodServing = 0.5f;
            VillageData.Happiness -= 0.05f;
        }
        else if (foodRatio < 1.5f)
        {
            VillageData.FoodServing = 1.0f;
            VillageData.Happiness -= 0.0f;
        }
        else if (foodRatio < 2.0f)
        {
            VillageData.FoodServing = 1.3f;
            VillageData.Happiness += 0.03f;
        }
        else if (foodRatio < 3.0f)
        {
            VillageData.FoodServing = 1.6f;
            VillageData.Happiness += 0.06f;
        }
        else if (foodRatio > 3.0f)
        {
            VillageData.FoodServing = 2.0f;
            VillageData.Happiness += 0.10f;
        }

        VillageData.Happiness = Mathf.Clamp(VillageData.Happiness, 0.0f, 1.0f);
    }
}
