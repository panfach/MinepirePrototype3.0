using UnityEngine;
using UnityEngine.AI;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
[DefaultExecutionOrder(-5)]
public class ConnectorInit : MonoBehaviour
{
    public VillageDataInit villageDataInit;
    public GameObject environmentSpawnedObjects;
    public SaveLoader saveLoader;
    public CameraScript cameraScript;
    public GeneralBuilder generalBuilder;
    public CellGrid cellGrid;
    public SmallCellGrid smallCellGrid;
    public Sunlight sunlight;
    public TurnManager turnManager;
    public NavMeshSurface navMeshSurface;
    public Names names;
    public NatureManager natureManager;
    public ItemManager itemManager;
    public PanelInvoker panelInvoker;
    public Camera mainCamera;
    public InfoDisplay infoDisplay;
    public CreatureManager creatureManager;
    public DynamicGameCanvas dynamicGameCanvas;
    public PhysicMaterial terrainPhysMat;
    public Notification notification;
    public EffectSoundManager effectSoundManager;
    public Statistics statistics;
    public TechManager techManager;

    public GameObject buildingSelectionPlane, villagerSelectionPlane, resourceSourceSelectionPlane,
                      villagerSilhouette;

    private void Awake()
    {
        Connector.villageDataInit = villageDataInit;
        Connector.environmentSpawnedObjects = environmentSpawnedObjects;
        Connector.saveLoader = saveLoader;
        Connector.cameraScript = cameraScript;
        Connector.generalBuilder = generalBuilder;
        Connector.cellGrid = cellGrid;
        Connector.smallCellGrid = smallCellGrid;
        Connector.sunlight = sunlight;
        Connector.turnManager = turnManager;
        Connector.navMeshSurface = navMeshSurface;
        Connector.names = names;
        Connector.natureManager = natureManager;
        Connector.itemManager = itemManager;
        Connector.panelInvoker = panelInvoker;
        Connector.mainCamera = mainCamera;
        Connector.infoDisplay = infoDisplay;
        Connector.creatureManager = creatureManager;
        Connector.dynamicGameCanvas = dynamicGameCanvas;
        Connector.notification = notification;
        Connector.effectSoundManager = effectSoundManager;
        Connector.statistics = statistics;
        Connector.techManager = techManager;

        //Villager.selectionPlanePrefab = villagerSelectionPlane;
        //Villager.silhouettePrefab = villagerSilhouette;
        //BuildingInfo.smallBuildingInfoPrefab = smallBuildingInfo;
        //ResourceSourceInstance.selectionPlanePrefab = resourceSourceSelectionPlane;
        //ResourceInstance.selectionPlanePrefab = resourceSourceSelectionPlane;
        //Animal.selectionPlanePrefab = villagerSelectionPlane;
        CellGrid.physMat = terrainPhysMat;
}
}

public static class Connector
{
    public static VillageDataInit villageDataInit;
    public static GameObject environmentSpawnedObjects;
    public static SaveLoader saveLoader;
    public static CameraScript cameraScript;
    public static GeneralBuilder generalBuilder;
    public static CellGrid cellGrid;
    public static SmallCellGrid smallCellGrid;
    public static Sunlight sunlight;
    public static TurnManager turnManager;
    public static NavMeshSurface navMeshSurface;
    public static Names names;
    public static NatureManager natureManager;
    public static ItemManager itemManager;
    public static PanelInvoker panelInvoker;
    public static Camera mainCamera;
    public static InfoDisplay infoDisplay;
    public static CreatureManager creatureManager;
    public static DynamicGameCanvas dynamicGameCanvas;
    public static Notification notification;
    public static EffectSoundManager effectSoundManager;
    public static Statistics statistics;
    public static TechManager techManager;
}
