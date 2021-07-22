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
    // --------------------------------------------------------------------------------------------------------------------- //
    static int homeless;
    static int[] workers = new int[Enum.GetNames(typeof(Profession)).Length];
    static float happiness;
    static float foodAmount;
    static float foodRatio;
    static float foodServing;
    static float[] resources = new float[Enum.GetNames(typeof(ResourceIndex)).Length];

    public static int Homeless { get => homeless; }
    public static void AddHomeless() { homeless++; }
    public static void RemoveHomeless() { homeless--; }
    public static int Workers(Profession profession) { return workers[(int)profession]; }
    public static void AddWorker(Profession profession) { workers[(int)profession] += 1; }
    public static void RemoveWorker(Profession profession) { workers[(int)profession] -= 1; }
    public static float Happiness { get => happiness; set => happiness = value; }
    public static float FoodAmount { get => foodAmount; set => foodAmount = value; }
    public static float FoodRatio { get => foodRatio; set => foodRatio = value; }
    public static float FoodServing { get => foodServing; set => foodServing = value; }
    public static float Resources(ResourceIndex index) { return resources[(int)index]; }
    public static void AddResource(ResourceIndex index, float amount) { resources[(int)index] += amount; }
    public static void RemoveResource(ResourceIndex index, float amount) { resources[(int)index] -= amount; }

    public static List<Building> Constructions { get; private set; } = new List<Building>();         
    public static List<Building> Buildings { get; private set; } = new List<Building>();
    public static List<Production> Productions { get; private set; } = new List<Production>();
    public static List<Inventory> Warehouses { get; private set; } = new List<Inventory>();
    public static Dictionary<int, Building> UniqIndexDict { get; private set; } = new Dictionary<int, Building>();
    public static Building Townhall = null;
    public static BuildingIndex TownhallIndex = BuildingIndex.TRIBLEADER;
    public static List<ExtractedResourceLink> ExtractionQueue = new List<ExtractedResourceLink>();

    public static List<GameObject> staticBatchingObjects = new List<GameObject>();

    // --------------------------------------------------------------------------------------------------------------------- //

    public static void AddConstruction(Building item)
    {
        Constructions.Add(item);
    }

    public static void RemoveConstruction(Building item)
    {
        Constructions.Remove(item);                                         
    }

    public static void AddBuilding(Building item)
    {
        Buildings.Add(item);
        if (Townhall == null && item.BldData.Index == TownhallIndex)
        {
            Townhall = item;
        }
    }

    public static void RemoveBuilding(Building item)
    {
        Buildings.Remove(item);
    }

    /// <summary>
    /// Checking the presence of all resources from resource query in all warehouses in village
    /// </summary>
    public static bool CheckResourceAvailability(ResourceQuery resQuery)
    {
        float value;

        for (int i = 0; i < resQuery.index.Length; i++)
        {
            value = resQuery.indexVal[i];

            foreach (Inventory item in Warehouses)
            {
                for (int j = 0; j < item.PacksAmount; j++)
                {
                    item.Look(j, out ResourceIndex outInd, out float outValue);
                    //Debug.Log(" : : : Checking warehouse: j=" + j + " resQuery.index[i] " + resQuery.index[i] + " outInd " + outInd + " outValue " + outValue);
                    if (outInd != resQuery.index[i]) continue;
                    value -= outValue;
                    if (value < 0.001) break;
                }
                if (value < 0.001) break;
            }

            if (value > 0.001f) { /*Debug.Log("Not enough resources! Need " + value + " " + resQuery.index[i] + " more");*/ return false; }
            //else Debug.Log("There is necessary amount of " + resQuery.index[i]);
        }

        return true;
    }

    /// <summary>
    /// Counting amount of specific resource in all warehouses in village
    /// </summary>
    public static float CheckWarehouseResourceAmount(ResourceIndex resInd)
    {
        float amount = 0;

        foreach (Inventory item in Warehouses)
        {
            for (int j = 0; j < item.PacksAmount; j++)
            {
                item.Look(j, out ResourceIndex outInd, out float outValue);
                if (outInd != resInd) continue;
                amount += outValue;
            }
        }

        return amount;
    }

    /// <summary>
    /// Returns free space in warehouses for specific resource
    /// </summary>
    public static float FreeSpaceForResource(ResourceIndex resInd)
    {
        float amount = 0f;

        foreach (Inventory item in Warehouses)
        {
            amount += item.FreeSpaceForResource(resInd);
        }

        return amount;
    }

    /// <summary>
    /// Deleting resources from resource query from warehouses in village
    /// </summary>
    public static void SpendResource(ResourceQuery resQuery)
    {
        float value;

        for (int i = 0; i < resQuery.index.Length; i++)
        {
            value = resQuery.indexVal[i];
            //resources[(int)resQuery.index[i]] -= value;

            foreach (Inventory item in Warehouses)
            {
                for (int j = 0; j < item.PacksAmount; j++)
                {
                    if (item.StoredRes[j] != resQuery.index[i]) continue;
                    value = item.ClearPack(j, value);
                    if (value <= 0) break;
                }
                if (value <= 0) break;
            }
        }

        InfoDisplay.Refresh();                                                                                   // Need to use UIController
    }

    public static void Recalculate()
    {
        homeless = 0;
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
            }
        }

        foodAmount = resources[(int)ResourceIndex.APPLE] + 
                     resources[(int)ResourceIndex.WILDBERRIES] + 
                     resources[(int)ResourceIndex.RAWVENISON] +
                     resources[(int)ResourceIndex.FISH];
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
                remainder = item.Inventory.CreateResource((ResourceIndex)i, remainder, true);
                if (remainder <= 0) break;
            }
        }
    }

    public static void AddResourceIntoFreeWarehouse(ResourceIndex index, float amount)
    {
        float remainder = amount;
        foreach (Inventory item in Warehouses)
        {
            remainder = item.CreateResource(index, remainder);
            if (remainder < 0.001f) return;
        }
    }


    public static void Save(BinaryWriter writer)
    {
        writer.Write((short)BuildingProperties.maxUniqueIndex);           //Debug.Log("VillageData.Save() Building.maxUniqueIndex = " + Building.maxUniqueIndex);
        writer.Write((short)Constructions.Count);                         //Debug.Log("VillageData.Save() Constructions.Count = " + Constructions.Count);
        foreach (Building building in Constructions)
        {
            //if (!building.isActiveAndEnabled) continue;                                                                                           // I don't know, when it is useful
            writer.Write((byte)building.BldData.Index);
            writer.Write((short)building.BldProp.UniqueIndex);
            writer.Write((short)building.GridObject.coordinates.X);
            writer.Write((short)building.GridObject.coordinates.Z);
            writer.Write((short)building.BuildSet.Process);
        }

        writer.Write((short)Buildings.Count);                             //Debug.Log("VillageData.Save() Buildings.Count = " + Buildings.Count);
        foreach (Building building in Buildings)
        {
            //if (!building.isActiveAndEnabled) continue;
            writer.Write((byte)building.BldData.Index);
            writer.Write((short)building.BldProp.UniqueIndex);            //Debug.Log("VillageData.Save() uniqueIndex = " + building.uniqueIndex);
            writer.Write((byte)building.GridObject.angle.Index);
            writer.Write((short)building.GridObject.coordinates.X);       //Debug.Log("VillageData.Load() (short)building.coordinates.X = " + (short)building.GridObject.coordinates.X);
            writer.Write((short)building.GridObject.coordinates.Z);       //Debug.Log("VillageData.Load() (short)building.coordinates.Z = " + (short)building.GridObject.coordinates.Z);
            //writer.Write((byte)((ReadySet)building).People);
        }

        writer.Write((short)CreatureManager.villagerPopulation);          //Debug.Log("VillageData.Load() (short)Population = " + (short)Population);
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

        for (int i = 0; i < 256; i++)
        {
            if (i < resources.Length) writer.Write(resources[i]);
            else writer.Write(0f);
        }

        for (int i = 0; i < 256; i++)
        {
            if (i < resources.Length) writer.Write(Connector.statistics.ReceivedResource((ResourceIndex)i));
            else writer.Write(0f);
        }

        for (int i = 0; i < 256; i++)
        {
            if (i < Enum.GetNames(typeof(TechnologySystem.TechIndex)).Length) writer.Write((byte)Connector.techManager.GetTechStatus((TechnologySystem.TechIndex)i));
            else writer.Write((byte)TechStatus.NOTRESEARCHED);
        }
    }

    // OLD LOAD FUNCTION
/*    public static void Load_old(BinaryReader reader)
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
                ((uniqueIndex = reader.ReadInt16()) == 0) ? null : UniqIndexDict[uniqueIndex].GetComponent<Building>(),                              // Dictionary Error (Maybe while construction doesn't refresh)
                ((uniqueIndex = reader.ReadInt16()) == 0) ? null : UniqIndexDict[uniqueIndex].GetComponent<Building>(),
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

        for (int i = 0; i < resources.Length - 5; i++)
        {
            resources[i] = reader.ReadSingle();
        }

        //resources[(int)ResourceIndex.STICKS] += 8.0f;
        //resources[(int)ResourceIndex.RAWDEERSKIN] += 8.0f;
        //resources[(int)ResourceIndex.RAWVENISON] += 8.0f;

        *//*
        Population = 0;
        Villagers.Clear();
        for (int i = 0; i < 6; i++)
        {
            NewRandomVillager();
        }
        Recalculate();
        *//*

        if (Connector.villageDataInit.startVillageInit)
        {
            Connector.villageDataInit.Start();
        }
        //Connector.villagerManager.SpawnAllVillagers();
        ExtractionQueue.Clear();
        ResourcesReassigning();
        //VillagerPlacesReassigning();
        Recalculate();
        SmallInfoController.SetAllBuilding();
    }*/

    // New LOAD FUNCTION
    public static void Load(BinaryReader reader)
    {
        BuildingIndex bldIndex;
        CreatureIndex animIndex;
        SCCoord scc;
        short uniqueIndex;
        byte angle;

        BuildingProperties.maxUniqueIndex = reader.ReadInt16();           //Debug.Log("VillageData.Load() Building.maxUniqueIndex = " + Building.maxUniqueIndex);
        short constructionsCount = reader.ReadInt16();                    //Debug.Log("VillageData.Load() constructionsCount = " + constructionsCount);
        for (int i = 0; i < constructionsCount; i++)
        {
            bldIndex = (BuildingIndex)reader.ReadByte();
            uniqueIndex = reader.ReadInt16();
            scc = new SCCoord(reader.ReadInt16(), reader.ReadInt16());
            Connector.generalBuilder.InstantConstruction(bldIndex, scc, uniqueIndex, reader.ReadInt16());
        }

        short readyCount = reader.ReadInt16();                            //Debug.Log("VillageData.Load() readyCount = " + readyCount);
        for (int i = 0; i < readyCount; i++)
        {
            bldIndex = (BuildingIndex)reader.ReadByte();                  //Debug.Log("VillageData.Load() trying to read bldIndex. bldIndex = " + bldIndex);
            uniqueIndex = reader.ReadInt16();                             //Debug.Log("VillageData.Load() trying to read uniqueIndex. uniqueIndex = " + uniqueIndex);
            angle = reader.ReadByte();
            scc = new SCCoord(reader.ReadInt16(), reader.ReadInt16());    //Debug.Log("VillageData.Load() scc of building = " + scc);
            Connector.generalBuilder.InstantBuilding(bldIndex, scc, uniqueIndex, angle);
        }

        int population = reader.ReadInt16();                              //Debug.Log("VillageData.Load() Population = " + Population);
        for (int i = 0; i < population; i++)
        {
            Connector.creatureManager.SpawnVillager
                (reader.ReadBoolean(),
                reader.ReadString(),
                reader.ReadByte(),
                ((uniqueIndex = reader.ReadInt16()) == 0) ? null : UniqIndexDict[uniqueIndex].GetComponent<Building>(), 
                ((uniqueIndex = reader.ReadInt16()) == 0) ? null : UniqIndexDict[uniqueIndex].GetComponent<Building>(),
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
                healthPoints: -1f);
            reader.ReadSingle();
        }

        for (int i = 0; i < 256; i++)
        {
            if (i < resources.Length) resources[i] = reader.ReadSingle();
            else reader.ReadSingle();
        }

        for (int i = 0; i < 256; i++)
        {
            if (i < resources.Length) Connector.statistics.ChangeReceivedResource((ResourceIndex)i, reader.ReadSingle());
            else reader.ReadSingle();
        }

        for (int i = 0; i < 256; i++)
        {
            if (i < Enum.GetNames(typeof(TechnologySystem.TechIndex)).Length) Connector.techManager.InitTechStatus((TechnologySystem.TechIndex)i, (TechStatus)reader.ReadByte());
            else reader.ReadByte();
        }

        if (Connector.villageDataInit.startVillageInit)
        {
            Connector.villageDataInit.Start();
        }
        ExtractionQueue.Clear();
        ResourcesReassigning();
        Recalculate();
        SmallInfoController.SetAllBuilding();
    }

    public static void Clear()
    {
        homeless = 0;
        workers = new int[Enum.GetNames(typeof(Profession)).Length];
        happiness = 0.75f;
        foodAmount = 0.0f;
        foodRatio = 1.0f;
        foodServing = 1.0f;

        Constructions = new List<Building>();

        resources = new float[Enum.GetNames(typeof(ResourceIndex)).Length];

        Buildings = new List<Building>();
        UniqIndexDict = new Dictionary<int, Building>();
        Townhall = null;
        ExtractionQueue.Clear();
        staticBatchingObjects.Clear();

        BuildingProperties.maxUniqueIndex = 1;
    }
}
