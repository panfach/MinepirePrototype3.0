using UnityEngine;

public class SmallInfoController : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] bool reactToMouseEnter;
    [SerializeField] bool reactToMouseDrag;                                                        // react to villager dragging
    [SerializeField] bool reactToZeroAppointedPeople;
    [SerializeField] bool reactToResourceDepositStatus;
    [SerializeField] bool reactToHomeAbsence;
    public float height;

    [Header("Other")]
    [SerializeField] SmallInfo smallInfo;                                                                              // Generalize

    public bool ReactToMouseEnter { get => reactToMouseEnter; }
    public bool ReactToMouseDrag { get => reactToMouseDrag; }
    public bool ReactToZeroAppointedPeople { get => reactToZeroAppointedPeople; }
    public bool ReactToResourceDepositStatus { get => reactToResourceDepositStatus; }
    public bool ReactToHomeAbsence { get => reactToHomeAbsence; }
    public SmallInfo Info { get => smallInfo; }


    private void OnEnable()
    {
        smallInfo = Connector.dynamicGameCanvas.SpawnInfo(entity);                           
        if (reactToMouseEnter)
        {
            entity.ColliderHandler.mouseEnterEvent += Set;                                                    // must return spawned object (small info). 
            entity.ColliderHandler.mouseExitEvent += Set;
        }
        if (reactToMouseDrag)
        {
            entity.ColliderHandler.mouseDragEvent += SetAllBuilding;                        // Strange place
            entity.ColliderHandler.mouseUpEvent += SetAllBuilding;
        }
        if (reactToZeroAppointedPeople) entity.Appointer.appointmentChangedEvent += Set;
        if (reactToResourceDepositStatus) entity.ResourceDeposit.statusChangedEvent += Set;
        if (reactToHomeAbsence) entity.Appointer.appointmentChangedEvent += Set;
        Set();
    }


    //public void AppointSmallInfo(SmallInfo item)                                                              // Temporal. In future each "SpawnInfo" function in dynamic game canvas
    //{                                                                                                         // must return created object (small info). 
    //    smallInfo = (SmallBuildingInfo)item;
    //}

    public void Refresh()
    {
        if (smallInfo.gameObject.activeSelf) smallInfo.Refresh();
    }

    public static void SetAllBuilding()                                                                          // THIS MUST BE IN "DYNAMIC GAME CANVAS" // And generalize
    {                                                                                                       
        foreach (Building item in VillageData.Buildings)
        {
            item.SmallInfoController.Set();
        }
        foreach (Building item in VillageData.Constructions)
        {
            item.SmallInfoController.Set();
        }
    }

    public void Set()
    {
        if (!enabled || smallInfo == null) return;

        smallInfo.Set();                                                                       // In future make fuction "Set" in SmallInfo, which will be define criteria of activation
        //if (!enabled || smallInfo == null) return;

        //if (reactToMouseEnter && entity.ColliderHandler.MouseOver ||
        //    reactToZeroAppointedPeople && entity.PeopleAppointer.enabled && entity.PeopleAppointer.People == 0 ||
        //    StateManager.VillagerDragging)
        //{
        //    smallInfo.gameObject.SetActive(true);
        //    smallInfo.infoBox.SetActive(reactToMouseEnter && entity.ColliderHandler.MouseOver || StateManager.VillagerDragging);
        //    smallInfo.iconBox.SetActive(reactToZeroAppointedPeople && entity.PeopleAppointer.enabled &&  entity.PeopleAppointer.People == 0);
        //    smallInfo.Refresh();
        //}
        //else
        //{
        //    smallInfo.gameObject.SetActive(false);
        //}
    }

    //public override void SetSmallInfo()
    //{
    //    if (smallInfo == null) return;

    //    if (activeCollider.mouseOver)
    //    {
    //        smallInfo.gameObject.SetActive(true);
    //        smallInfo.infoBox.SetActive(activeCollider.mouseOver);
    //        smallInfo.iconBox.SetActive(false);
    //        smallInfo.Refresh();
    //    }
    //    else
    //    {
    //        smallInfo.gameObject.SetActive(false);
    //    }
    //}

    //public override void SetSmallInfo()
    //{
    //    if (smallInfo == null) return;

    //    if (activeCollider.mouseOver || StateManager.VillagerDragging || (people == 0 && GetMaxPeople() != 0))
    //    {
    //        smallInfo.gameObject.SetActive(true);
    //        smallInfo.infoBox.SetActive(activeCollider.mouseOver || StateManager.VillagerDragging);
    //        smallInfo.iconBox.SetActive(people == 0 && GetMaxPeople() != 0);
    //        smallInfo.Refresh();
    //    }
    //    else
    //    {
    //        smallInfo.gameObject.SetActive(false);
    //    }
    //}

    //public void SetSmallInfo()
    //{
    //    bool extractable = ExtractableResourceExists();
    //    if (mouseOver || extractable)
    //    {
    //        smallInfo.gameObject.SetActive(true);
    //        smallInfo.infoBox.SetActive(mouseOver);
    //        smallInfo.iconBox.SetActive(ExtractableResourceExists());
    //        smallInfo.Refresh();
    //    }
    //    else
    //    {
    //        smallInfo.gameObject.SetActive(false);
    //    }
    //}

    public static void SetSmallInfoByMouse()
    {
        Ray ray = Connector.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Entity entity;

        if (Physics.Raycast(ray, out hit, 1000f) && (entity = hit.collider.GetComponent<Entity>()) != null)
        {
            if (entity.SmallInfoController != null) entity.SmallInfoController.Set();
        }
    }


    private void OnDisable()
    {
        // Deleting smallInfo from list in dynamicGameCanvas
        Connector.dynamicGameCanvas.RemoveInfo(entity);               
        if (smallInfo != null) smallInfo.Delete();

        if (reactToMouseEnter)
        {
            entity.ColliderHandler.mouseEnterEvent -= Set;
            entity.ColliderHandler.mouseExitEvent -= Set;
        }
        if (reactToMouseDrag)
        {     
            entity.ColliderHandler.mouseDragEvent -= SetAllBuilding;
            entity.ColliderHandler.mouseUpEvent -= SetAllBuilding;
        }
        if (reactToZeroAppointedPeople) entity.Appointer.appointmentChangedEvent -= Set;
        if (reactToResourceDepositStatus) entity.ResourceDeposit.statusChangedEvent -= Set;
        if (reactToHomeAbsence) entity.Appointer.appointmentChangedEvent -= Set;
    }
}
