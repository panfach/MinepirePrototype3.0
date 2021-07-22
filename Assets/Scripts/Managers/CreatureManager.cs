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


    public Creature Spawn
        (Vector3 position,
        CreatureIndex index = CreatureIndex.DEER,
        bool gender = false,
        string _name = "Олень",
        int age = 0,
        Building home = null,
        Building work = null,
        float satiety = 2.0f,
        float healthPoints = -1)
    {
        GameObject creature = Instantiate(DataList.GetCreatureObj(index), new Vector3(position.x, SCCoord.GetHeight(position) + 0.5f, position.z), Quaternion.identity);
        Creature entity = creature.GetComponent<Creature>();
        Add(entity);
        entity.CrtProp.Init(gender, _name, age, home, work, satiety, healthPoints);
        //entity.Health.Value = (healthPoints >= 0) ? healthPoints : entity.CrtData.MaxHealth;

        return entity;
    }

    public Creature SpawnSingle
        (Vector3 _position,
        CreatureIndex _index = CreatureIndex.DEER,
        bool _gender = false,
        string _name = "Олень",
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
        CreatureData data = DataList.GetCreatureObj(CreatureIndex.DEER).GetComponent<Creature>().CrtData;
        int _age = Random.Range(data.MinRandomAge, data.MaxRandomAge);

        return Spawn(pos, _index, _gender, age: _age);
    }

    public Creature SpawnVillager
        (bool _gender = false,
        string _name = "Олень",
        int _age = 0,
        Building _home = null,
        Building _work = null,
        float _satiety = 2.0f,
        float _healthPoints = -1)
    {
        Vector3 _position = VillageData.Townhall.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
        return Spawn(_position, CreatureIndex.VILLAGER, _gender, _name, _age, _home, _work, _satiety, _healthPoints);
    }

    public Creature SpawnRandomVillager()
    {
        CreatureData data = DataList.GetCreatureObj(CreatureIndex.VILLAGER).GetComponent<Creature>().CrtData;
        Creature creature = SpawnVillager(
            _gender: Random.Range(0, 2) == 0 ? true : false,
            _name: Connector.names.GetRndName(true),
            _age: Random.Range(data.MinRandomAge, data.MaxRandomAge)
            );
        return creature;
    }

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
            if (item.Appointer.Profession == Profession.LABORER && item.GeneralAI.ActionType == ActionType.RNDWALK)
            {
                item.GeneralAI.DefineBehaviour();
            }
        }
    }

    public void Clear()
    {
        int count = Animals.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(Animals[i].gameObject);
        }
        count = Villagers.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(Villagers[i].gameObject);
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
