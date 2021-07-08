using UnityEngine;

public class Building : Entity
{
    [SerializeField] BuildingData data;
    [SerializeField] BuildingProperties properties;
    [SerializeField] SmallInfoController smallInfoController;
    [SerializeField] GridObject gridObject;
    [SerializeField] Inventory inventory;
    [SerializeField] ColliderHandler colliderHandler;
    [SerializeField] BuildSet buildSet;
    [SerializeField] DisplayedItems displayedItems;
    [SerializeField] UIController uiController;
    [SerializeField] CreatureContainer creatureContainer;
    [SerializeField] Appointer appointer;
    [SerializeField] Interactive interactive;
    [SerializeField] Production production; 
    [SerializeField] Workplace workplace; 

    public override BuildingData BldData { get => data; }
    public override BuildingProperties BldProp { get => properties; }
    public override SmallInfoController SmallInfoController { get => smallInfoController; }
    public override GridObject GridObject { get => gridObject; }
    public override Inventory Inventory { get => inventory; }
    public override ColliderHandler ColliderHandler { get => colliderHandler; }
    public override BuildSet BuildSet { get => buildSet; }
    public override DisplayedItems DisplayedItems { get => displayedItems; }
    public override UIController UIController { get => uiController; }
    public override CreatureContainer CreatureContainer { get => creatureContainer; }
    public override Appointer Appointer { get => appointer; }
    public override Interactive Interactive { get => interactive; }
    public override Production Production { get => production; }
    public override Workplace Workplace { get => workplace; }


    public override void Die()
    {
        Debug.Log("Building.Die() start");
        if (buildSet.ConstrStatus == ConstructionStatus.CONSTR) VillageData.Constructions.Remove(this);
        else if (buildSet.ConstrStatus == ConstructionStatus.READY) { Debug.Log("Building.Die() : " + BldData.Name); VillageData.Buildings.Remove(this); }

        Destroy(gameObject);
    }
}


public enum ConstructionMode
{
    ORD,
    INSTBLD,
    INSTCONSTR
}

public enum BuildingType
{
    LIVING,
    TOWNHALL,
    WAREHOUSE,
    HUNT,
    SKIN,
    PUBLIC,
    COOKING,
    FISHING,
    TOOLS,
    CLOTHING
}