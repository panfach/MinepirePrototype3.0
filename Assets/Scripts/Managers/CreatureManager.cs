using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class CreatureManager : MonoBehaviour
{
    public static bool creaturesWereSpawned = false;                                                                        // Why is it needed ?
    public static bool villagersWereSpawned = false;
    public static int animalPopulation;
    public static int villagerPopulation;
    public static List<Creature> Animals { get; private set; } = new List<Creature>();
    public static List<Creature> Villagers { get; private set; } = new List<Creature>();

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

    public Creature Spawn
        (Vector3 position,
        CreatureIndex index = CreatureIndex.NONE,
        bool gender = false,
        string _name = "Potato",
        int age = 0,
        Building home = null,
        Building work = null,
        float satiety = 2.0f,
        float healthPoints = -1)
    {
        GameObject creature = Instantiate(DataList.GetCreatureObj(index), new Vector3(position.x, SCCoord.GetHeight(position) + 0.5f, position.z), Quaternion.identity);
        Creature entity = creature.GetComponent<Creature>();
        Add(entity);
        entity.CrtProp.Init(gender, _name, age, home, work, satiety);
        entity.Health.Value = (healthPoints >= 0) ? healthPoints : entity.CrtData.MaxHealth;

        return entity;
    }

    public Creature SpawnSingle
        (Vector3 _position,
        CreatureIndex _index = CreatureIndex.NONE,
        bool _gender = false,
        string _name = "Potato",
        int _age = 0,
        Building _home = null,
        Building _work = null,
        float _satiety = 2.0f,
        float _healthPoints = -1)
    {
        if (spawnBreak) return null;

        spawnBreak = true;
        StartCoroutine(SpawnBreak(spawnDelay));

        return Spawn(_position, _index, _gender, _name, _age, _home, _work, _satiety, _healthPoints);
    }

    public Creature SpawnRandomAnimal(Vector3 pos)                                                                   // In future remake this function. Now it is only for deer
    {
        CreatureIndex _index = CreatureIndex.DEER;
        bool _gender = (Random.Range(0, 2) == 0) ? true : false;
        CreatureData data = DataList.GetCreatureObj(CreatureIndex.DEER).GetComponent<CreatureData>();
        int _age = Random.Range(data.MinRandomAge, data.MaxRandomAge);

        return Spawn(pos, _index, _gender, age: _age);
    }

    public Creature SpawnVillager
        (bool _gender = false,
        string _name = "Potato",
        int _age = 0,
        Building _home = null,
        Building _work = null,
        float _satiety = 2.0f,
        float _healthPoints = -1)
    {
        Vector3 _position = VillageData.townhall.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
        return Spawn(_position, CreatureIndex.VILLAGER, _gender, _name, _age, _home, _work, _satiety, _healthPoints);
    }

    public Creature SpawnRandomVillager()
    {
        //Names _names = Connector.names ?? GameObject.Find("GameManager").GetComponent<Names>();
        return null;                                                                                                           // Finish up
    }

/*    public void SpawnVillager(VillagerData data)
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
    }*/

    public void Add(Creature creature)
    {
        if (creature.CrtData.Index == CreatureIndex.VILLAGER)
        {
            Villagers.Add(creature);
            villagerPopulation++;
        }
        else
        {
            Animals.Add(creature);
            animalPopulation++;
        }
    }

    public bool Remove(Creature creature)
    {
        if (creature.CrtData.Index == CreatureIndex.VILLAGER)
        {
            if (!Villagers.Remove(creature)) 
                return false;
            villagerPopulation--;
        }
        else
        {
            if (!Animals.Remove(creature))
                return false;
            animalPopulation--;
        }

        return true;
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

    public static void DefineVillagerBehaviours(int priority = 0)
    {
        foreach (Creature item in Villagers)
        {
            item.GeneralAI.DefineBehaviour(priority);
        }
    }

    public static void DefineBehaviourOfFreeLaborers()
    {
        foreach (Creature item in Villagers)
        {
            if (item.Appointer.Profession == Profession.LABORER && item.GeneralAI.GetActionType == ActionType.RNDWALK)
            {
                item.GeneralAI.DefineBehaviour();
            }
        }
    }

    public static void MorningBehaviourDefinition()
    {
        /*foreach (Creature item in Villagers)
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
        }*/
    }

    /*public static void DeleteAnimal(Animal animal) //////////////// А что, если животное умрет, когда другой охотник будет перебирать цикл foreach при поиске 
    {
        Animals.Remove(animal);
        DynamicGameCanvas.animalSmallInfoList.Remove(animal.smallInfo);
        Destroy(animal.smallInfo.gameObject);
        Destroy(animal.gameObject); Debug.Log("AnimalManager.DeleteAnimal()   Destroy...");
    }*/

    public void Clear()
    {
        foreach (Creature item in Animals)
        {
            Destroy(item);
        }
        foreach (Creature item in Villagers)
        {
            Destroy(item);
        }
        Animals.Clear();
        Villagers.Clear();
    }

    IEnumerator SpawnBreak(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        spawnBreak = false;
    }
}
