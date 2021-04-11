using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class CreatureManager : MonoBehaviour
{
    public static bool animalsWereSpawned = false;
    public static int animalPopulation;
    public static List<Creature> Creatures { get; private set; } = new List<Creature>();

    //public GameObject animalPrefab;

    bool spawnBreak = false;
    float spawnDelay = 0.2f;

    /*const float raycastLength = 100f;
    Ray ray;
    RaycastHit hit;*/

    // -------------------------------------------------------------------------------------------------- //

/*    private void Update()
    {

        if (StateManager.VillagerDragging)
        {
            ray = Connector.mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, raycastLength) *//*&& hit.collider.tag == "TerrainMesh"*//*)
            {
                if (Villager.silhouette != null) Villager.silhouette.transform.position = hit.point;
            }
        }
    }*/

    // -------------------------------------------------------------------------------------------------- //
    public void SpawnAnimal(Vector3 position, CreatureIndex index, float healthPoints = -1)
    {
        if (!spawnBreak)
        {
            GameObject animal = Instantiate(DataList.GetCreatureObj(index), new Vector3(position.x, SCCoord.GetHeight(position) + 0.5f, position.z), Quaternion.identity);
            Creature animalScript = animal.GetComponent<Creature>();
            if (index != CreatureIndex.HUMAN) animalPopulation++;

            Creatures.Add(animalScript);
            animal.SetActive(true);
            animalScript.CrtProp.Init((Random.Range(0, 2) == 0) ? true : false, Random.Range(0, 9));
            //animalScript.GeneralAI.DefineBehaviour();
            if (healthPoints >= 0) animalScript.Health.Value = healthPoints;
            //Connector.dynamicGameCanvas.SpawnInfo(animalScript);

            spawnBreak = true;
            StartCoroutine(SpawnBreak(spawnDelay));
        }
    }

    public void InstantSpawnAnimal(Vector3 position, CreatureIndex index, float healthPoints = -1)
    {
        spawnBreak = false;
        SpawnAnimal(position, index, healthPoints);
    }

    /*public void SpawnAllAnimals()
    {
        if (VillageData.townhall != null)
        {
            foreach (Animal  in VillageData.Villagers)
            {
                SpawnAnimal(data); //////
            }
            animalsWereSpawned = true;
        }
    }*/

    public static void DefineAllBehaviours()
    {
        foreach (Creature item in Creatures)
        {
            item.GeneralAI.DefineBehaviour();
        }
    }

    /*public static void MorningBehaviourDefinition()
    {
        foreach (Villager item in Villagers)
        {
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
    }*/

    /*public static void DeleteAnimal(Animal animal) //////////////// А что, если животное умрет, когда другой охотник будет перебирать цикл foreach при поиске 
    {
        Animals.Remove(animal);
        DynamicGameCanvas.animalSmallInfoList.Remove(animal.smallInfo);
        Destroy(animal.smallInfo.gameObject);
        Destroy(animal.gameObject); Debug.Log("AnimalManager.DeleteAnimal()   Destroy...");
    }*/

    public void Clear()
    {
        foreach (Creature item in Creatures)
        {
            Destroy(item);
        }
        Creatures.Clear();
    }

    IEnumerator SpawnBreak(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        spawnBreak = false;
    }
}
