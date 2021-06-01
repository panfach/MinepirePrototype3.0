using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class VillagerManager_old : MonoBehaviour
{
    public static bool villagersWereSpawned = false;
    public static List<Villager> Villagers { get; private set; } = new List<Villager>();

    public GameObject villagerPrefab;

    const float raycastLength = 100f;
    Ray ray;
    RaycastHit hit;

    // -------------------------------------------------------------------------------------------------- //

    private void Update()
    {

        if (StateManager.VillagerDragging)
        {
            ray = Connector.mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, raycastLength) /*&& hit.collider.tag == "TerrainMesh"*/)
            {
                if (Villager.silhouette != null) Villager.silhouette.transform.position = hit.point;
            }
        }
    }

    // -------------------------------------------------------------------------------------------------- //

    public void SpawnVillager(VillagerData data)
    {
        Vector3 spawnPos = VillageData.townhall.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f)); 
        GameObject villager = Instantiate(villagerPrefab, spawnPos, Quaternion.identity);
        Villager villagerScript = villager.GetComponent<Villager>();
        villagerScript.data = data;
        villagerScript.actions = null;
        //villagerScript.uiElement.text = data.Name;
        //villagerScript.DefineInventory(1, 2);

        Villagers.Add(villagerScript);
        villager.SetActive(true);

        Connector.dynamicGameCanvas.SpawnInfo(villagerScript);
        villagerScript.SetSmallInfo();
    }

    public void SpawnAllVillagers()
    {
        if (VillageData.townhall == null)
        {
            foreach (Building item in VillageData.Buildings)
            {
                if (item.BldData.Index == BuildingIndex.TRIBLEADER) VillageData.townhall = item;
            }
        }

        if (VillageData.townhall != null)
        {
            foreach (VillagerData data in VillageData.Villagers)
            {
                SpawnVillager(data);
            }
            villagersWereSpawned = true;
        }
    }

    public static void DefineAllBehaviours(int priority = 0)
    {
        foreach (Villager item in Villagers)
        {
            item.DefineBehaviour(priority);
        }
    }

    public static void DefineBehaviourOfFreeLaborers()
    {
        foreach (Villager item in Villagers)
        {
            if (item.data.profession == Profession.LABORER && item.state == VillagerState.RNDWALK)
            {
                item.DefineBehaviour();
            }
        }
    }

    public static void MorningBehaviourDefinition()
    {
        foreach (Villager item in Villagers)
        {
            item.MorningFlagRefresh();
            switch (item.state)
            {
                case VillagerState.INDOORS:
                    item.ExitBuilding();
                    break;
                case VillagerState.RNDWALK:
                    item.DefineBehaviour();
                    break;
            }
        }
    }

    public void Clear()
    {
        villagersWereSpawned = false;
        foreach (Villager item in Villagers)
        {
            item.Die();
        }
        Villagers = new List<Villager>();
    }

    /*public IEnumerator SpawnVillagersWithDelay(float delay) ////////////////////////////////////////////
    {
        yield return new WaitForSeconds(delay);
        SpawnAllVillagers();
    }*/
}

public enum BehDefType
{

}