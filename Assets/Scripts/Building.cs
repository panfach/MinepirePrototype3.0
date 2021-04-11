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
    [SerializeField] PeopleContainer peopleContainer;
    [SerializeField] PeopleAppointer peopleAppointer;

    public override BuildingData BldData { get => data; }
    public override BuildingProperties BldProp { get => properties; }
    public override SmallInfoController SmallInfoController { get => smallInfoController; }
    public override GridObject GridObject { get => gridObject; }
    public override Inventory Inventory { get => inventory; }
    public override ColliderHandler ColliderHandler { get => colliderHandler; }
    public override BuildSet BuildSet { get => buildSet; }
    public override DisplayedItems DisplayedItems { get => displayedItems; }
    public override UIController UIController { get => uiController; }
    public override PeopleContainer PeopleContainer { get => peopleContainer; }
    public override PeopleAppointer PeopleAppointer { get => peopleAppointer; }


    public override void Die()
    {
        if (buildSet.ConstrStatus == ConstructionStatus.CONSTR) VillageData.Constructions.Remove(this);
        else if (buildSet.ConstrStatus == ConstructionStatus.READY) VillageData.Buildings.Remove(this);

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
    PUBLIC
}