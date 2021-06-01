using System.Collections.Generic;
using UnityEngine;
using System;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class DataList : MonoBehaviour
{
    [Header("Array of buildings")]
    public GameObject[] buildings;

    [Header("Array of resources")]
    public GameObject itemObject;
    public ResourceData[] resources;
    public GameObject[] resourceModels;

    [Header("Array of natures")]
    public GameObject[] natureObjects;
    public NatureData[] natures;

    [Header("Array of creatures")]
    public GameObject[] creatureObjects;                     // In future add humans here

    static Dictionary<BuildingIndex, GameObject> intBuildingObjDict = new Dictionary<BuildingIndex, GameObject>();

    //static Dictionary<ResourceIndex, GameObject> intItemObjDict = new Dictionary<ResourceIndex, GameObject>();
    static GameObject emptyItem;

    static Dictionary<ResourceIndex, ResourceData> intResourceDict = new Dictionary<ResourceIndex, ResourceData>();

    static Dictionary<ResourceIndex, GameObject> intResourceModelDict = new Dictionary<ResourceIndex, GameObject>();

    static Dictionary<ResourceType, ResourceIndex[]> typeResourceDict = new Dictionary<ResourceType, ResourceIndex[]>();

    static Dictionary<NatureIndex, GameObject> intNatureObjDict = new Dictionary<NatureIndex, GameObject>();

    static Dictionary<NatureIndex, NatureData> intNatureDict = new Dictionary<NatureIndex, NatureData>();

    static Dictionary<CreatureIndex, GameObject> intCreatureObjDict = new Dictionary<CreatureIndex, GameObject>();

    public static Dictionary<Profession, string> profNameDict_rus = new Dictionary<Profession, string>()          
    {
        {Profession.NONE, "Нет"},
        {Profession.LABORER, "Разнорабочий"},
        {Profession.HUNTER, "Охотник"}
    };


    private void Awake()
    {
        GameObject bld;
        Building bldScript;
        //GameObject resObj;
        //ResourceData resData;
        List<ResourceIndex>[] resIndices = new List<ResourceIndex>[Enum.GetNames(typeof(ResourceType)).Length];
        GameObject natureObj;
        NatureData natureData;
        GameObject creatureObj;

        // Making dictionary of building
        for (int i = 0; i < buildings.Length; i++)
        {
            bld = buildings[i];
            bldScript = bld.GetComponent<Building>();
            intBuildingObjDict.Add(bldScript.BldData.Index, bld);
        }

        // Making dictionary of item prefabs (of game objects)
        //for (int i = 0; i < itemObjects.Length; i++)
        //{
        //    resObj = itemObjects[i];
        //    intItemObjDict.Add(resObj.GetComponent<Item>().data.index, resObj);
        //}

        //
        emptyItem = itemObject;

        // Making dictionary of scripts "Resource" (of resource data)
        for (int i = 0; i < resources.Length; i++)
        {
            intResourceDict.Add(resources[i].Index, resources[i]);
        }

        // Making dictionary of resource models
        for (int i = 0; i < resourceModels.Length; i++)
        {
            intResourceModelDict.Add(resources[i].Index, resourceModels[i]);
        }

        // Making dictionary, that assotiate resource types and resource indices (For example: ResourceType.EAT ---> { ResourceIndex.RAWVENISON, ResourceIndex.APPLES })
        for (int i = 0; i < resIndices.Length; i++)
        {
            resIndices[i] = new List<ResourceIndex>();
        }
        for (int i = 0; i < resources.Length; i++) 
        {
            for (int j = 0; j < resources[i].TypeLength; j++) 
            {
                resIndices[(int)resources[i].Type(j)].Add(resources[i].Index);
            }
        }
        for (int i = 0; i < resIndices.Length; i++)
        {
            typeResourceDict.Add((ResourceType)i, resIndices[i].ToArray());

            /*for (int k = 0; k < resIndices[i].Count; k++)
            {
                Debug.Log("DataList: " + (ResourceType)i + " -> " + resIndices[i][k]);
            }*/
        }

        // Making two dictionaries, for NatureData and for Nature prefabs
        for (int i = 0; i < natureObjects.Length; i++)
        {
            natureObj = natureObjects[i];
            natureData = natures[i];
            intNatureObjDict.Add(natureObj.GetComponent<Nature>().NtrData.Index, natureObj);
            intNatureDict.Add(natureData.Index, natureData);
        }

        // Making dictionary of creature prefabs
        for (int i = 0; i < creatureObjects.Length; i++)
        {
            creatureObj = creatureObjects[i];
            intCreatureObjDict.Add(creatureObj.GetComponent<Creature>().CrtData.Index, creatureObj);
        }
    }

    public static GameObject GetBuildingObj(BuildingIndex index) => intBuildingObjDict[index];
    //public static GameObject GetResourceObj(ResourceIndex index) => intResourceObjDict[index];
    public static GameObject GetItemObject() => emptyItem;
    public static ResourceData GetResource(ResourceIndex index) => intResourceDict[index];
    public static GameObject GetResourceModel(ResourceIndex index) => intResourceModelDict[index];
    public static ResourceIndex[] GetResourceIndices(ResourceType type) => typeResourceDict[type];
    public static GameObject GetNatureObj(NatureIndex index) => intNatureObjDict[index];
    public static NatureData GetNature(NatureIndex index) => intNatureDict[index];
    public static GameObject GetCreatureObj(CreatureIndex index) => intCreatureObjDict[index];
}

public enum ResourceIndex
{
    NONE,
    STICKS,
    COBBLESTONES,
    LOG,
    WILDBERRIES,
    APPLE,
    RAWVENISON,
    RAWDEERSKIN,
    DEERSKIN
}

public enum ResourceType
{
    NONE,
    FOOD,
    ANIMALDROP,
    BUILDMAT
}

public enum NatureIndex
{
    NONE,
    BRANCHHEAP,
    COBBLESTONEHEAP,
    POPLAR,
    BERRYBUSH,
    APPLETREE,
    BOULDER
}

public enum BuildingIndex
{
    PRIMHUT,
    STORTENT,
    HUNTHUT,
    BONFIRE,
    SKINDRYER,
    TRIBLEADER
}

public enum CreatureIndex
{
    NONE,
    VILLAGER,
    DEER
}

public delegate void SimpleEventHandler();
