using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public abstract class Building_old : MonoBehaviour
{
    //public static int maxUniqueIndex = 1;                 
    //public static string[] tags = {"BuildSet", "ConstructionSet", "Building"};
    //public static ConstructionMode constructionMode = ConstructionMode.ORD;

    //public static BuildingType[] livingBuildings = {
    //    BuildingType.LIVING };
    //public static BuildingType[] workBuildings = {
    //    BuildingType.HUNT };

    //[Header("Main data")]
    //public GameObject currentModel;
    //public Vector3 currentModelPos;
    //public GameObject buildModel;
    //public GameObject constructionModel;
    //public GameObject readyModel;
    //public CellPointer[] cellPointer;
    //public SmallBuildingInfo smallInfo;
    //public BuildingEnterTrigger enter;
    //public int uniqueIndex;
    //public SCCoord coordinates;
    //public Transform[] itemSpot;
    //protected GameObject[] displayedItem;
    //public float smallInfoHeight;
    //[SerializeField] BuildingData data = null;
    //public bool deletionFlag;
    //public BuildingAngle angle = new BuildingAngle(0);

    //public string GetName() => data._name;
    //public string GetName_rus() => data._name_rus;
    //public BuildingIndex GetIndex() => data.index;
    //public int[] GetSize() => data.Size();
    //public int GetConstrCost() => data.constructionCost;
    //public ResourceQuery GetResourceCost() => data.resourceCost;
    //public int GetMaxPeople() => data.maxPeople;
    //public BuildingType GetBldType() => data.type;
    //public Profession GetProfession() => data.profession;

    //// -------------------------------------------------------------------------------------------------- //

    //public void OnEnable()
    //{
    //    cellPointer = GetComponentsInChildren<CellPointer>();
    //}

    //public Building() { }
    //public Building(Building building) { data = building.data; }

    //public Vector3 GetLocalCenter()
    //{
    //    //int[] size = GetSize();
    //    //return new Vector3((float)size[0] / 2f, 0f, (float)size[1] / 2f);

    //    Vector3 pos = currentModelPos;
    //    pos.y = transform.position.y;

    //    return transform.InverseTransformPoint(pos);
    //}

    //public Vector3 GetCenter()
    //{
    //    //return currentModelPos

    //    Vector3 pos = currentModelPos;
    //    pos.y = transform.position.y;

    //    return pos;
    //}

    //public void RefreshSmallInfo()
    //{
    //    smallInfo.Refresh();
    //}

    //public void AssignUniqueIndex(int index = 0)
    //{
    //    if (index == 0) uniqueIndex = ++maxUniqueIndex;
    //    else uniqueIndex = index;
    //}

    //// -------------------------------------------------------------------------------------------------- //

    //public static void SetSmallInfoByMouse()
    //{
    //    Ray ray = Connector.mainCamera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, 1000f) && hit.collider.tag == "BuildingModel")
    //    {
    //        Building building = hit.collider.GetComponent<BuildingCollider>().building;
    //        building.SetSmallInfo();
    //    }
    //}

    //public virtual void SetSmallInfo()
    //{

    //}


    //public static void SetAllSmallInfo()
    //{
    //    foreach (Building item in VillageData.Buildings)
    //    {
    //        item.SetSmallInfo();
    //    }
    //    foreach (Building item in VillageData.Constructions)
    //    {
    //        item.SetSmallInfo();
    //    }
    //}

    //public BuildingCollider GetCollider()
    //{
    //    if (this is ConstructionSet)
    //        return ((ConstructionSet)this).activeCollider;
    //    else if (this is ReadySet)
    //        return ((ConstructionSet)this).activeCollider;
    //    else return null;
    //}

    //public static void BuildingCopyComponents(Building b1, Building b2)
    //{
    //    b1.currentModelPos = b2.currentModelPos;
    //    b1.data = b2.data;
    //    b1.buildModel = b2.buildModel;
    //    b1.constructionModel = b2.constructionModel;
    //    b1.readyModel = b2.readyModel;
    //    b1.enter = b2.enter;
    //    b1.coordinates = b2.coordinates;
    //    b1.smallInfoHeight = b2.smallInfoHeight;

    //    b1.itemSpot = new Transform[b2.itemSpot.Length];
    //    for (int i = 0; i < b1.itemSpot.Length; i++) b1.itemSpot[i] = b2.itemSpot[i];
    //    b1.angle = b2.angle;
    //}

    //public virtual void Delete()
    //{
    //    deletionFlag = true;
    //    VillageData.deletedBuildingsQueue.Add(this);

    //    // Controlling UI things
    //    smallInfo.Delete();
    //    Connector.panelInvoker.CloseBuildingInfo();

    //    // Clear small grid cells from building
    //    foreach (CellPointer item in cellPointer)
    //    {
    //        item.Free();
    //    }

    //    // Hiding object (Now the object is waiting for its final deletion)
    //    transform.position = CellMetrics.hidedObjects;
    //    gameObject.SetActive(false);
    //}

    //// -------------------------------------------------------------------------------------------------- //

    //public abstract override string ToString();

    //public virtual void SelfDeletion() { Destroy(gameObject); } 
}

public enum ConstructionMode_old
{
    ORD,
    INSTBLD,
    INSTCONSTR
}

public enum BuildingType_old
{
    LIVING,
    TOWNHALL,
    WAREHOUSE,
    HUNT,
    SKIN,
    PUBLIC
}
