using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BuildSet : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] bool hasResourceCost;
    [SerializeField] bool hasGridVolume;
    public GameObject buildModel;
    public GameObject constructionModel;
    public GameObject readyModel;

    [Header("Construction status")]
    [SerializeField] ConstructionStatus constructionStatus;
    [SerializeField] float constructionProcess;

    private int desiredUniqueIndex;
    private Vector3 currentModelPos;
    private GameObject currentModel;

    public float Process
    {
        get => constructionProcess;
        set
        {
            if (entity.BldProp.deletionFlag == true) return;

            constructionProcess = value;
            if (BuildingProperties.constructionMode == ConstructionMode.INSTBLD || constructionProcess >= entity.BldData.ConstrCost)
            {
                TransformIntoReady();
            }
            constructionProcess = Mathf.Clamp(constructionProcess, 0.0f, entity.BldData.ConstrCost);
            
            Connector.panelInvoker.RefreshBuildingInfo();
        }
    }

    public Vector3 ModelPos
    {
        get
        {
            currentModelPos = currentModel.transform.position;
            return currentModelPos;
        }
        set
        {
            currentModel.transform.position = value;
            currentModelPos = value;
        }
    }

    public ConstructionStatus ConstrStatus { get => constructionStatus; }
    public bool IsStatusChoosePlace() => constructionStatus == ConstructionStatus.CHOOSEPLACE;
    public bool IsStatusConstr() => constructionStatus == ConstructionStatus.CONSTR;
    public bool IsStatusReady() => constructionStatus == ConstructionStatus.READY;

    private void Awake()
    {
        constructionStatus = ConstructionStatus.CHOOSEPLACE;
        if (currentModel == null) currentModel = buildModel.transform.GetChild(0).gameObject;
        currentModelPos = currentModel.transform.position;
    }


    public bool TryToBuild(int _uniqueIndex = 0, int _process = 0)
    {
        if (!CheckStartConstructionConditions()) return false;

        if (hasGridVolume && entity.GridObject != null) entity.GridObject.OccupyPlace();
        if (hasResourceCost && BuildingProperties.constructionMode != ConstructionMode.INSTBLD) VillageData.SpendResource(entity.BldData.ResourceCost);
        entity.BldProp?.AssignUniqueIndex(_uniqueIndex);
        TransformIntoConstruction(_process);

        return true;
    }

    public bool CheckStartConstructionConditions()
    {
        if (BuildingProperties.constructionMode == ConstructionMode.INSTBLD) return true;
        if (hasGridVolume && entity.GridObject != null && !entity.GridObject.CheckPlaceAvailability())
        {
            Notification.Invoke(NotifType.PLACEBUILD);
            return false;
        }
        if (hasResourceCost && !VillageData.CheckResourceAvailability(entity.BldData.ResourceCost))
        {
            Notification.Invoke(NotifType.RESBUILD);
            return false;
        }
        if (!Connector.techManager.IsTechResearched(entity.BldData.RequiredTech))
        {
            Notification.Invoke(NotifType.REQTECH);
            return false;
        }

        return true;
    }

    Entity TransformIntoConstruction(int _process = 0)
    {
        // Switching displayed models
        Destroy(buildModel);
        constructionModel.SetActive(true);

        // List managing
        VillageData.AddConstruction(entity as Building);                                           // Generalize ? or not ?

        // Model position adjustment
        currentModel = constructionModel.transform.GetChild(0).gameObject;                         // Perhaps not good
        ModelPos = new Vector3(currentModelPos.x, currentModel.transform.position.y, currentModelPos.z);

        // Turning on components
        if (entity.SmallInfoController != null) entity.SmallInfoController.enabled = true;
        if (entity.ColliderHandler != null) entity.ColliderHandler.enabled = true;
        if (entity.DisplayedItems != null) entity.DisplayedItems.enabled = true;
        if (entity.SmallInfoController != null) entity.SmallInfoController.enabled = true;
        if (entity.UIController != null) entity.UIController.enabled = true;

        // Hiding cell pointers
        entity.GridObject.HideCellPointers();

        constructionStatus = ConstructionStatus.CONSTR;
        Process = _process;

        CreatureManager.DefineBehaviourOfFreeLaborers();

        return entity;
    }

    Entity TransformIntoReady()
    {
        // Switching displayed models
        Destroy(constructionModel);
        readyModel.SetActive(true);

        // List managing
        VillageData.RemoveConstruction(entity as Building);
        VillageData.AddBuilding(entity as Building);                                               // Generalize ? or not ?

        // Model position adjustment
        currentModel = readyModel.transform.GetChild(0).gameObject;                                // Perhaps not good
        ModelPos = new Vector3(currentModelPos.x, currentModel.transform.position.y, currentModelPos.z);

        // Recreating small info
        if (entity.SmallInfoController != null)
        {
            entity.SmallInfoController.enabled = false;
            entity.SmallInfoController.enabled = true;
        }

        // Turning on components
        if (entity.Inventory != null) entity.Inventory.enabled = true;
        if (entity.Appointer != null) entity.Appointer.enabled = true;
        if (entity.CreatureContainer != null) entity.CreatureContainer.enabled = true;
        if (entity.Interactive != null) entity.Interactive.enabled = true;
        if (entity.Production != null) entity.Production.enabled = true;

        // Handling selecton plane
        if (entity.DisplayedItems != null && entity.DisplayedItems.HasSelectionPlane) 
            entity.DisplayedItems.SetParentOfSelectionPlane(readyModel.transform);

        constructionStatus = ConstructionStatus.READY;

        if (BuildingInfo.activeBuilding == this) Connector.panelInvoker.CloseBuildingInfo();

        return entity;
    }
}

public enum ConstructionStatus
{
    CHOOSEPLACE,
    CONSTR,
    READY
}
