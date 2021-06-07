using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class VillageDataInit : MonoBehaviour
{
    public bool startVillageInit;
    public int startPopulation;


    public void Start()
    {
        if (!startVillageInit) return;

        VillageData.Clear();
        DefaultInit();
        InfoDisplay.Refresh();
        //Connector.villagerManager.SpawnAllVillagers();
    }


    public void DefaultInit()
    {
        for (int i = 0; i < startPopulation; i++)
        {
            Connector.creatureManager.SpawnRandomVillager();
        }
        VillageData.Recalculate();
    }
}

public static class VillageData
{
    // --------------------------------------------------- Data ------------------------------------------------------------ //
    public static int homeless;
    public static int workersCount;
    public static int[] workers = new int[Enum.GetNames(typeof(Profession)).Length];
    public static float happiness;
    public static float foodAmount;
    public static float foodRatio;
    public static float foodServing;

    public static int Builders { get; private set; }

    public static float[] resources = new float[Enum.GetNames(typeof(ResourceIndex)).Length];

    public static List<Building> Constructions { get; private set; } = new List<Building>();                                         // Maybe GeneralBuilder (or BuildingManager) must contain it
    public static List<Building> Buildings { get; private set; } = new List<Building>();
    public static Dictionary<int, Building> uniqIndexDict = new Dictionary<int, Building>();
    public static Building townhall = null;
    public static BuildingIndex townhallIndex = BuildingIndex.TRIBLEADER;
    public static List<ExtractedResourceLink> extractionQueue = new List<ExtractedResourceLink>();
    // --------------------------------------------------------------------------------------------------------------------- //

    public static List<GameObject> staticBatchingObjects = new List<GameObject>();
    // --------------------------------------------------------------------------------------------------------------------- //

    /*public static Creature NewRandomVillager()
    {
        //Names _names = Connector.names ?? GameObject.Find("GameManager").GetComponent<Names>();
        return Connector.creatureManager.SpawnRandomVillager();
    }*/

    /*public static void RemoveVillager(Creature creature)
    {
        if (Villagers.Remove(data)) Population--;
    }*/

    //public static int GetBuildPoint() => Builders * 1;

    public static void AddConstruction(Building item)
    {
        Constructions.Add(item);
    }

    public static void RemoveConstruction(Building item)
    {
        Debug.Log("VillageData.RemoveConstruction() Construction was removed ... ");
        Constructions.Remove(item);                                         
    }

    public static void AddBuilding(Building item)
    {
        Buildings.Add(item);
        if (townhall == null && item.BldData.Index == townhallIndex)
        {
            townhall = item;
        }
    }

    public static void RemoveBuilding(Building item)
    {
        Buildings.Remove(item); // Возможно, это очень неэффективно (Можно попробовать искать по уникальному номеру постройки)
    }

    public static void Recalculate()
    {
        //Builders = 1;
        
        homeless = 0;
        workersCount = 0;
        for (int i = 0; i < workers.Length; i++)
        {
            workers[i] = 0;
        }

        foreach (Creature villager in CreatureManager.Villagers)
        {
            if (villager.Appointer.Home == null) homeless++;
            else
            {
                workers[(int)villager.Appointer.Profession]++;
                if (villager.Appointer.Profession != Profession.LABORER) workersCount++;
            }
        }

        foodAmount = resources[(int)ResourceIndex.APPLE] + resources[(int)ResourceIndex.WILDBERRIES] + resources[(int)ResourceIndex.RAWVENISON];
    }

    public static int CountReadyBuildings()
    {
        var seq = from item in Buildings
                  where item.BuildSet.ConstrStatus == ConstructionStatus.READY
                  select item;
        return seq.Count();
    }

    public static Building GetNearestBuilding(List<Building> buildings, Vector3 point)
    {
        if (buildings.Count == 0) return null;

        Building nearestBuilding = null;
        float distance, minDistance = float.MaxValue;
        foreach (Building building in buildings)
        {
            if ((distance = Vector3.SqrMagnitude(point - building.transform.position)) < minDistance)
            {
                nearestBuilding = building;
                minDistance = distance;
            }
        }

        return nearestBuilding;
    }

/*    public static void VillagerPlacesReassigning()
    {
        if (!VillagerManager.villagersWereSpawned) return;

        foreach (Building building in Buildings)
        {
            building.PeopleAppointer.ForgetPeopleAssignment();
        }
        foreach (VillagerData data in Villagers)
        {
            if (data.home != null)
            {
                data.Settle(data.home, AppointMode.REASSIGNING);
                Villager villager = data.villagerAgent;
                villager.agent.enabled = false;
                villager.transform.position = data.home.GridObject.GetCenter();
                villager.placeOfStay = data.home;
                villager.state = VillagerState.INDOORS;
                villager.DefineBehaviour();
            }
            else
            {
                data.villagerAgent.DefineBehaviour(-5);
            }
            if (data.work != null) data.AssignJob(data.work, AppointMode.REASSIGNING);
        }
    }*/

    public static void ResourcesReassigning()
    {
        List<Building> warehouses = new List<Building>();

        foreach (Building item in Buildings)
        {
            if (item.BldData.BldType == BuildingType.WAREHOUSE && item.BuildSet.IsStatusReady())
                warehouses.Add(item);
        }

        for (int i = 1; i < resources.Length; i++)
        {
            //if ((ResourceIndex)i == ResourceIndex.DEERSKIN || (ResourceIndex)i == ResourceIndex.LOG || (ResourceIndex)i == ResourceIndex.NONE) break;
            float remainder = resources[i];

            foreach (Building item in warehouses)
            {
                remainder = item.Inventory.CreateResource((ResourceIndex)i, remainder);
                if (remainder <= 0) break;
            }
        }
    }

/*    public static void CleanDeletedObjects()
    {
        foreach (Villager item in deletedVillagersQueue)
        {
            Villagers.Remove(item.data);
            item.SelfDeletion();
        }
        deletedVillagersQueue.Clear();

        foreach (SmallInfo item in deletedSmallInfoQueue)
        {
            if (item is SmallBuildingInfo)
                DynamicGameCanvas.buildingSmallInfoList.Remove(item);
            else if (item is SmallVillagerInfo)
                DynamicGameCanvas.villagerSmallInfoList.Remove(item);
            else if (item is SmallAnimalInfo)
                DynamicGameCanvas.animalSmallInfoList.Remove(item);
            else if (item is SmallResourceInfo)
                DynamicGameCanvas.resourceSmallInfoList.Remove(item);
            else if (item is SmallResourceSourceInfo)
                DynamicGameCanvas.resourceSourceSmallInfoList.Remove(item);


            //if (item is SmallVillagerInfo) item.Delete();
        }
        deletedSmallInfoQueue.Clear();
    }*/

    public static void Save(BinaryWriter writer)
    {
        writer.Write((short)BuildingProperties.maxUniqueIndex); //Debug.Log("VillageData.Save() Building.maxUniqueIndex = " + Building.maxUniqueIndex);
        writer.Write((short)Constructions.Count); //Debug.Log("VillageData.Save() Constructions.Count = " + Constructions.Count);
        foreach (Building building in Constructions)
        {
            if (!building.isActiveAndEnabled) continue;                                                                                           // I don't know, when it is useful
            writer.Write((byte)building.BldData.Index);
            writer.Write((short)building.BldProp.UniqueIndex);
            writer.Write((short)building.GridObject.coordinates.X);
            writer.Write((short)building.GridObject.coordinates.Z);
            writer.Write((short)building.BuildSet.Process);
        }

        writer.Write((short)Buildings.Count); //Debug.Log("VillageData.Save() Buildings.Count = " + Buildings.Count);
        foreach (Building building in Buildings)
        {
            if (!building.isActiveAndEnabled) continue;
            writer.Write((byte)building.BldData.Index);
            writer.Write((short)building.BldProp.UniqueIndex); //Debug.Log("VillageData.Save() uniqueIndex = " + building.uniqueIndex);
            writer.Write((byte)building.GridObject.angle.Index);
            writer.Write((short)building.GridObject.coordinates.X); //Debug.Log("VillageData.Load() (short)building.coordinates.X = " + (short)building.coordinates.X);
            writer.Write((short)building.GridObject.coordinates.Z); //Debug.Log("VillageData.Load() (short)building.coordinates.Z = " + (short)building.coordinates.Z);
            //writer.Write((byte)((ReadySet)building).People);
        }

        writer.Write((short)CreatureManager.villagerPopulation); //Debug.Log("VillageData.Load() (short)Population = " + (short)Population);
        foreach (Creature data in CreatureManager.Villagers)
        {
            writer.Write(data.CrtProp.Gender);
            writer.Write(data.CrtProp.Name);
            writer.Write((byte)data.CrtProp.Age);
            if (data.Appointer.Home == null) writer.Write((short)0);
            else writer.Write((short)data.Appointer.Home.entity.BldProp.UniqueIndex);
            if (data.Appointer.Work == null) writer.Write((short)0);  
            else writer.Write((short)data.Appointer.Work.entity.BldProp.UniqueIndex);
            writer.Write((float)data.Satiety.Value);
        }

        writer.Write((short)CreatureManager.animalPopulation);
        foreach (Creature animal in CreatureManager.Animals)
        {
            SCCoord animalPos = SCCoord.FromPos(animal.transform.position);
            writer.Write((byte)animal.CrtData.Index);
            writer.Write((short)animalPos.X);
            writer.Write((short)animalPos.Z);
            writer.Write((float)animal.Health.Value);
        }

        for (int i = 0; i < resources.Length; i++)
        {
            writer.Write((float)resources[i]);
        }
    }

    // OLD LOAD FUNCTION
    //public static void Load_old1(BinaryReader reader)
    //{
    //    BuildingIndex bldIndex;
    //    AnimalIndex animIndex;
    //    SCCoord scc;
    //    short uniqueIndex;

    //    BuildingProperties.maxUniqueIndex = reader.ReadInt16(); //Debug.Log("VillageData.Load() Building.maxUniqueIndex = " + Building.maxUniqueIndex);
    //    short constructionsCount = reader.ReadInt16(); //Debug.Log("VillageData.Load() constructionsCount = " + constructionsCount);
    //    for (int i = 0; i < constructionsCount; i++)
    //    {
    //        bldIndex = (BuildingIndex)reader.ReadByte();
    //        uniqueIndex = reader.ReadInt16();
    //        scc = new SCCoord(reader.ReadInt16(), reader.ReadInt16());
    //        Connector.generalBuilder.InstantConstruction(bldIndex, scc, uniqueIndex, reader.ReadInt16());
    //        //cs.uniqueIndex = uniqueIndex;
    //        //cs.IncreaseProcess(reader.ReadInt16());
    //    }
    //    CleanDeletedObjects();

    //    short readyCount = reader.ReadInt16(); //Debug.Log("VillageData.Load() readyCount = " + readyCount);
    //    for (int i = 0; i < readyCount; i++)
    //    {
    //        bldIndex = (BuildingIndex)reader.ReadByte(); //Debug.Log("VillageData.Load() trying to read bldIndex. bldIndex = " + bldIndex);
    //        uniqueIndex = reader.ReadInt16(); //Debug.Log("VillageData.Load() trying to read uniqueIndex. uniqueIndex = " + uniqueIndex);
    //        scc = new SCCoord(reader.ReadInt16(), reader.ReadInt16()); //Debug.Log("VillageData.Load() scc of building = " + scc);
    //        Connector.generalBuilder.InstantBuilding(bldIndex, scc, uniqueIndex);
    //        //rs = obj.GetComponent<ReadySet>();                          
    //        //rs.uniqueIndex = uniqueIndex;
    //    }

    //    Population = reader.ReadInt16(); //Debug.Log("VillageData.Load() Population = " + Population);
    //    for (int i = 0; i < Population; i++)
    //    {
    //        Villagers.Add(new VillagerData(
    //            reader.ReadBoolean(),
    //            reader.ReadString(),
    //            reader.ReadByte(),
    //            ((uniqueIndex = reader.ReadInt16()) == 0) ? null : uniqIndexDict[uniqueIndex].GetComponent<Building>(),
    //            ((uniqueIndex = reader.ReadInt16()) == 0) ? null : uniqIndexDict[uniqueIndex].GetComponent<Building>()
    //        ));
    //    }

    //    int animalPopul = reader.ReadInt16();
    //    Debug.Log("animalPopul = " + animalPopul);
    //    for (int i = 0; i < animalPopul; i++)
    //    {
    //        animIndex = (AnimalIndex)reader.ReadByte();
    //        scc = new SCCoord(reader.ReadInt16(), reader.ReadInt16());
    //        Connector.animalManager.InstantSpawnAnimal(SCCoord.GetCenter(scc), animIndex, reader.ReadSingle());
    //    }

    //    /*
    //    Population = 0;
    //    Villagers.Clear();
    //    for (int i = 0; i < 6; i++)
    //    {
    //        NewRandomVillager();
    //    }
    //    Recalculate();
    //    */

    //    Connector.villagerManager.SpawnAllVillagers();
    //    extractionQueue = new List<ResourceDeposit>();
    //    VillagerPlacesReassigning();
    //    Recalculate();
    //    SmallInfoController.SetAllBuilding();
    //}

    // OLD LOAD FUNCTION 2
    //public static void Load_old2(BinaryReader reader)
    //{
    //    BuildingIndex bldIndex;
    //    AnimalIndex animIndex;
    //    SCCoord scc;
    //    short uniqueIndex;

    //    Building.maxUniqueIndex = reader.ReadInt16(); //Debug.Log("VillageData.Load() Building.maxUniqueIndex = " + Building.maxUniqueIndex);
    //    short constructionsCount = reader.ReadInt16(); //Debug.Log("VillageData.Load() constructionsCount = " + constructionsCount);
    //    for (int i = 0; i < constructionsCount; i++)
    //    {
    //        bldIndex = (BuildingIndex)reader.ReadByte();
    //        uniqueIndex = reader.ReadInt16();
    //        scc = new SCCoord(reader.ReadInt16(), reader.ReadInt16());
    //        Connector.generalBuilder.InstantConstruction(bldIndex, scc, uniqueIndex, reader.ReadInt16());
    //        //cs.uniqueIndex = uniqueIndex;
    //        //cs.IncreaseProcess(reader.ReadInt16());
    //    }
    //    CleanDeletedObjects();

    //    short readyCount = reader.ReadInt16(); //Debug.Log("VillageData.Load() readyCount = " + readyCount);
    //    for (int i = 0; i < readyCount; i++)
    //    {
    //        bldIndex = (BuildingIndex)reader.ReadByte(); //Debug.Log("VillageData.Load() trying to read bldIndex. bldIndex = " + bldIndex);
    //        uniqueIndex = reader.ReadInt16(); //Debug.Log("VillageData.Load() trying to read uniqueIndex. uniqueIndex = " + uniqueIndex);
    //        scc = new SCCoord(reader.ReadInt16(), reader.ReadInt16()); //Debug.Log("VillageData.Load() scc of building = " + scc);
    //        Connector.generalBuilder.InstantBuilding(bldIndex, scc, uniqueIndex);
    //        //rs = obj.GetComponent<ReadySet>();                          
    //        //rs.uniqueIndex = uniqueIndex;
    //    }

    //    Population = reader.ReadInt16(); //Debug.Log("VillageData.Load() Population = " + Population);
    //    for (int i = 0; i < Population; i++)
    //    {
    //        Villagers.Add(new VillagerData(
    //            reader.ReadBoolean(),
    //            reader.ReadString(),
    //            reader.ReadByte(),
    //            ((uniqueIndex = reader.ReadInt16()) == 0) ? null : uniqIndexDict[uniqueIndex].GetComponent<ReadySet>(),
    //            ((uniqueIndex = reader.ReadInt16()) == 0) ? null : uniqIndexDict[uniqueIndex].GetComponent<ReadySet>(),
    //            reader.ReadSingle()
    //        ));
    //    }

    //    int animalPopul = reader.ReadInt16();
    //    for (int i = 0; i < animalPopul; i++)
    //    {
    //        animIndex = (AnimalIndex)reader.ReadByte();
    //        scc = new SCCoord(reader.ReadInt16(), reader.ReadInt16());
    //        Connector.animalManager.InstantSpawnAnimal(SCCoord.GetCenter(scc), animIndex, reader.ReadSingle());
    //    }

    //    for (int i = 0; i < resources.Length; i++)
    //    {
    //        resources[i] = reader.ReadSingle();
    //    }

    //    /*
    //    Population = 0;
    //    Villagers.Clear();
    //    for (int i = 0; i < 6; i++)
    //    {
    //        NewRandomVillager();
    //    }
    //    Recalculate();
    //    */
        
    //    if (Connector.villageDataInit.startVillageInit)
    //    {
    //        Connector.villageDataInit.Start();
    //    }
    //    Connector.villagerManager.SpawnAllVillagers();
    //    extractionQueue = new List<ResourceDeposit>();
    //    ResourcesReassigning();
    //    VillagerPlacesReassigning();
    //    Recalculate();
    //    Building.SetAllSmallInfo();
    //}

    // NEW LOAD FUNCTION
    public static void Load(BinaryReader reader)
    {
        BuildingIndex bldIndex;
        CreatureIndex animIndex;
        SCCoord scc;
        short uniqueIndex;
        byte angle;

        BuildingProperties.maxUniqueIndex = reader.ReadInt16(); //Debug.Log("VillageData.Load() Building.maxUniqueIndex = " + Building.maxUniqueIndex);
        short constructionsCount = reader.ReadInt16(); //Debug.Log("VillageData.Load() constructionsCount = " + constructionsCount);
        for (int i = 0; i < constructionsCount; i++)
        {
            bldIndex = (BuildingIndex)reader.ReadByte();
            uniqueIndex = reader.ReadInt16();
            scc = new SCCoord(reader.ReadInt16(), reader.ReadInt16());
            Connector.generalBuilder.InstantConstruction(bldIndex, scc, uniqueIndex, reader.ReadInt16());
            //cs.uniqueIndex = uniqueIndex;
            //cs.IncreaseProcess(reader.ReadInt16());
        }

        short readyCount = reader.ReadInt16(); //Debug.Log("VillageData.Load() readyCount = " + readyCount);
        for (int i = 0; i < readyCount; i++)
        {
            bldIndex = (BuildingIndex)reader.ReadByte(); //Debug.Log("VillageData.Load() trying to read bldIndex. bldIndex = " + bldIndex);
            uniqueIndex = reader.ReadInt16(); //Debug.Log("VillageData.Load() trying to read uniqueIndex. uniqueIndex = " + uniqueIndex);
            angle = reader.ReadByte();
            scc = new SCCoord(reader.ReadInt16(), reader.ReadInt16()); //Debug.Log("VillageData.Load() scc of building = " + scc);
            Connector.generalBuilder.InstantBuilding(bldIndex, scc, uniqueIndex, angle);
            //rs = obj.GetComponent<ReadySet>();                          
            //rs.uniqueIndex = uniqueIndex;
        }

        int population = reader.ReadInt16(); //Debug.Log("VillageData.Load() Population = " + Population);
        for (int i = 0; i < population; i++)
        {
            Connector.creatureManager.SpawnVillager
                (reader.ReadBoolean(),
                reader.ReadString(),
                reader.ReadByte(),
                ((uniqueIndex = reader.ReadInt16()) == 0) ? null : uniqIndexDict[uniqueIndex].GetComponent<Building>(),                              // Dictionary Error (Maybe while construction doesn't refresh)
                ((uniqueIndex = reader.ReadInt16()) == 0) ? null : uniqIndexDict[uniqueIndex].GetComponent<Building>(),
                _satiety: reader.ReadSingle());
        }

        int animalPopul = reader.ReadInt16();
        for (int i = 0; i < animalPopul; i++)
        {
            reader.ReadByte();
            animIndex = CreatureIndex.DEER;

            //animIndex = (CreatureIndex)reader.ReadByte();
            scc = new SCCoord(reader.ReadInt16(), reader.ReadInt16());
            Connector.creatureManager.Spawn
                (SCCoord.GetCenter(scc), 
                animIndex, 
                healthPoints: reader.ReadSingle());
        }

        for (int i = 0; i < resources.Length - 1; i++)
        {
            resources[i] = reader.ReadSingle();
        }

        //resources[(int)ResourceIndex.STICKS] += 8.0f;
        //resources[(int)ResourceIndex.RAWDEERSKIN] += 8.0f;
        //resources[(int)ResourceIndex.RAWVENISON] += 8.0f;

        /*
        Population = 0;
        Villagers.Clear();
        for (int i = 0; i < 6; i++)
        {
            NewRandomVillager();
        }
        Recalculate();
        */

        if (Connector.villageDataInit.startVillageInit)
        {
            Connector.villageDataInit.Start();
        }
        //Connector.villagerManager.SpawnAllVillagers();
        extractionQueue.Clear();
        ResourcesReassigning();
        //VillagerPlacesReassigning();
        Recalculate();
        SmallInfoController.SetAllBuilding();
    }

    public static void Clear()
    {
        homeless = 0;
        workersCount = 0;
        workers = new int[Enum.GetNames(typeof(Profession)).Length];
        happiness = 0.75f;
        foodAmount = 0.0f;
        foodRatio = 1.0f;
        foodServing = 1.0f;

        Constructions = new List<Building>();

        resources = new float[Enum.GetNames(typeof(ResourceIndex)).Length];

        Buildings = new List<Building>();
        uniqIndexDict = new Dictionary<int, Building>();
        townhall = null;
        extractionQueue.Clear();
        staticBatchingObjects.Clear();

        BuildingProperties.maxUniqueIndex = 1;
    }
}
