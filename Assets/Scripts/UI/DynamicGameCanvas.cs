using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGameCanvas : MonoBehaviour
{
    /*public static List<SmallVillagerInfo> villagerSmallInfoList = new List<SmallVillagerInfo>();
    public static List<SmallBuildingInfo> buildingSmallInfoList = new List<SmallBuildingInfo>();
    public static List<SmallAnimalInfo> animalSmallInfoList = new List<SmallAnimalInfo>();
    public static List<SmallResourceInfo> resourceSmallInfoList = new List<SmallResourceInfo>();
    public static List<SmallResourceSourceInfo> resourceSourceSmallInfoList = new List<SmallResourceSourceInfo>();*/

    public static List<SmallInfo> creatureSmallInfoList = new List<SmallInfo>();
    public static List<SmallInfo> buildingSmallInfoList = new List<SmallInfo>();
    //public static List<SmallInfo> animalSmallInfoList = new List<SmallInfo>();
    public static List<SmallInfo> resourceSmallInfoList = new List<SmallInfo>();
    public static List<SmallInfo> natureSmallInfoList = new List<SmallInfo>();

    public GameObject buildingInfoPrefab;
    public GameObject creatureInfoPrefab;
    public GameObject resourceInfoPrefab;
    public GameObject natureInfoPrefab; 
    public float creatureInfoHeight;
    public float itemInfoHeight;

    SmallInfo info;
    Vector3 worldPosition = new Vector3(), screenPosition = new Vector3();

    void Update()
    {
        UpdateBuildings();
        UpdateCreatures();
        UpdateItems();
        UpdateNatures();
    }

    public SmallInfo SpawnInfo(Entity entity)
    {
        if (entity is Building) return SpawnInfo(entity as Building);
        else if (entity is Creature) return SpawnInfo(entity as Creature);
        else if (entity is Item) return SpawnInfo(entity as Item);
        else if (entity is Nature) return SpawnInfo(entity as Nature);
        else return null;
    }

    public void RemoveInfo(Entity entity)
    {
        SmallInfo info = entity.SmallInfoController.Info;

        if (entity is Building) buildingSmallInfoList.Remove(info);
        else if (entity is Creature) creatureSmallInfoList.Remove(info);
        else if (entity is Item) resourceSmallInfoList.Remove(info);
        else if (entity is Nature) natureSmallInfoList.Remove(info);
    }

    public SmallInfo SpawnInfo(Building building)
    {
        GameObject obj;

        obj = Instantiate(buildingInfoPrefab, transform);
        SmallBuildingInfo script = obj.GetComponent<SmallBuildingInfo>();
        script.Init(building);
        buildingSmallInfoList.Add(script);

        return script;
    }

    public SmallInfo SpawnInfo(Creature creature)
    {
        GameObject obj;

        obj = Instantiate(creatureInfoPrefab, transform);
        SmallCreatureInfo script = obj.GetComponent<SmallCreatureInfo>();
        script.Init(creature);
        creatureSmallInfoList.Add(script);

        return script;
    }

    public SmallInfo SpawnInfo(Item item)
    {
        GameObject obj;

        obj = Instantiate(resourceInfoPrefab, transform);
        SmallResourceInfo script = obj.GetComponent<SmallResourceInfo>();
        script.Init(item);
        resourceSmallInfoList.Add(script);

        return script;
    }

    public SmallInfo SpawnInfo(Nature nature)
    {
        GameObject obj;

        obj = Instantiate(natureInfoPrefab, transform);
        SmallNatureInfo script = obj.GetComponent<SmallNatureInfo>();
        script.Init(nature);
        natureSmallInfoList.Add(script);

        return script;
    }

    void UpdateBuildings()
    {
        foreach (SmallBuildingInfo item in buildingSmallInfoList)
        {
            if (!item.deletionFlag && item.gameObject.activeSelf)
            {
                //worldPosition = new Vector3(item.objectTransform.position.x, item.objectTransform.position.y + item.building.smallInfoHeight, item.objectTransform.position.z);
                //worldPosition += item.building.GetLocalCenter();
                worldPosition = item.instance.GridObject.GetCenter();
                worldPosition.y += item.instance.SmallInfoController.height;
                screenPosition = Connector.mainCamera.WorldToScreenPoint(worldPosition);

                if (screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height)
                {
                    item._transform.position = screenPosition;
                }
                else
                {
                    item._transform.position = CellMetrics.hidedObjectsUI;
                }
            }
        }
    }

    void UpdateCreatures()
    {
        for (int i = 0; i < creatureSmallInfoList.Count; i++)
        {
            info = creatureSmallInfoList[i];

            if (info.gameObject.activeSelf)
            {
                worldPosition = info.objectTransform.position;
                worldPosition.y += creatureInfoHeight;
                screenPosition = Connector.mainCamera.WorldToScreenPoint(worldPosition);

                if (screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height)
                {
                    info._transform.position = screenPosition;
                }
                else
                {
                    info._transform.position = CellMetrics.hidedObjectsUI;
                }
            }
        }
    }

    void UpdateItems()                                                                               
    {
        foreach (SmallResourceInfo item in resourceSmallInfoList)
        {
            if (!item.deletionFlag && item.gameObject.activeSelf)
            {
                worldPosition = new Vector3(item.objectTransform.position.x, item.objectTransform.position.y + 0.5f, item.objectTransform.position.z);           // 0.5f = itemInfoHeight <= add this variable
                screenPosition = Connector.mainCamera.WorldToScreenPoint(worldPosition);

                if (screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height)
                {
                    item._transform.position = screenPosition;
                }
                else
                {
                    item._transform.position = CellMetrics.hidedObjectsUI;
                }
            }
        }
    }

    void UpdateNatures()
    {
        for (int i = 0; i < natureSmallInfoList.Count; i++)
        {
            info = natureSmallInfoList[i];

            if (info.gameObject.activeSelf)
            {
                Vector3 centerPos = info.instance.ResourceDeposit.ExtractPoint.position;
                worldPosition = new Vector3(centerPos.x, centerPos.y + info.instance.SmallInfoController.height, centerPos.z);
                screenPosition = Connector.mainCamera.WorldToScreenPoint(worldPosition);

                if (screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height)
                {
                    info._transform.position = screenPosition;
                }
                else
                {
                    info._transform.position = CellMetrics.hidedObjectsUI;
                }
            }
        }
    }
}


