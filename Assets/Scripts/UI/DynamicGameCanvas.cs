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

    public static List<SmallInfo> villagerSmallInfoList = new List<SmallInfo>();
    public static List<SmallInfo> buildingSmallInfoList = new List<SmallInfo>();
    public static List<SmallInfo> animalSmallInfoList = new List<SmallInfo>();
    public static List<SmallInfo> resourceSmallInfoList = new List<SmallInfo>();
    public static List<SmallInfo> resourceSourceSmallInfoList = new List<SmallInfo>();

    public GameObject buildingInfoPrefab;
    public GameObject creatureInfoPrefab;
    public GameObject resourceInfoPrefab;
    public GameObject resourceSourceInfoPrefab; 
    public float creatureInfoHeight;
    public float resourceInfoHeight;

    SmallInfo info;
    Vector3 worldPosition = new Vector3(), screenPosition = new Vector3();

    void Update()
    {
        UpdateBuildings();
        UpdateVillagers();
        UpdateAnimals();
        UpdateItems();
        UpdateResourceSources();
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
        else if (entity is Creature) animalSmallInfoList.Remove(info);
        else if (entity is Item) resourceSmallInfoList.Remove(info);
        else if (entity is Nature) resourceSourceSmallInfoList.Remove(info);
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
        SmallAnimalInfo script = obj.GetComponent<SmallAnimalInfo>();
        script.Init(creature);
        animalSmallInfoList.Add(script);

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

        obj = Instantiate(resourceSourceInfoPrefab, transform);
        SmallResourceSourceInfo script = obj.GetComponent<SmallResourceSourceInfo>();
        script.Init(nature);
        resourceSourceSmallInfoList.Add(script);

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

    void UpdateVillagers()
    {
        foreach (SmallVillagerInfo item in villagerSmallInfoList)
        {
            if (!item.deletionFlag && item.gameObject.activeSelf)
            {
                worldPosition = new Vector3(item.objectTransform.position.x, item.objectTransform.position.y + creatureInfoHeight, item.objectTransform.position.z);
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

    void UpdateAnimals()
    {
        /*foreach (SmallAnimalInfo item in animalSmallInfoList)
        {
            if (!item.deletionFlag && item.gameObject.activeSelf)
            {
                worldPosition = new Vector3(item.objectTransform.position.x, item.objectTransform.position.y + villagerInfoHeight, item.objectTransform.position.z);
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
        }*/

        for (int i = 0; i < animalSmallInfoList.Count; i++)
        {
            info = animalSmallInfoList[i];

            if (!info.deletionFlag && info.gameObject.activeSelf)
            {
                worldPosition.x = info.objectTransform.position.x;
                worldPosition.y = info.objectTransform.position.y + creatureInfoHeight;
                worldPosition.z = info.objectTransform.position.z;
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

    void UpdateResourceSources()
    {
        foreach (SmallResourceSourceInfo item in resourceSourceSmallInfoList)
        {
            if (!item.deletionFlag && item.gameObject.activeSelf)
            {
                Vector3 centerPos = item.instance.ResourceDeposit.ExtractPoint.position;
                worldPosition = new Vector3(centerPos.x, centerPos.y + item.instance.SmallInfoController.height, centerPos.z);
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
}


