using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CreatureProperties))]
[RequireComponent(typeof(GeneralAI))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Satiety))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Creature : Entity
{
    [SerializeField] CreatureData creatureData;
    [SerializeField] CreatureProperties creatureProperties;
    [SerializeField] DisplayedItems displayedItems;
    [SerializeField] SmallInfoController smallInfoController;
    [SerializeField] ColliderHandler colliderHandler;
    [SerializeField] GeneralAI generalAI;
    [SerializeField] Health health;
    [SerializeField] Satiety satiety;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Appointer appointer;
    [SerializeField] AttackController attackController;
    [SerializeField] Inventory inventory;

    public override CreatureData CrtData { get => creatureData; }
    public override CreatureProperties CrtProp { get => creatureProperties; }
    public override DisplayedItems DisplayedItems { get => displayedItems; }
    public override SmallInfoController SmallInfoController { get => smallInfoController; }
    public override ColliderHandler ColliderHandler { get => colliderHandler; }
    public override GeneralAI GeneralAI { get => generalAI; }
    public override Health Health { get => health; }
    public override Satiety Satiety { get => satiety; }
    public override Rigidbody Rigidbody { get => _rigidbody; }
    public override NavMeshAgent Agent { get => navMeshAgent; }
    public override Appointer Appointer { get => appointer; }
    public override AttackController AttackController { get => attackController; }
    public override Inventory Inventory { get => inventory; }


    public override void Die()
    {
        Connector.itemManager.DropItems(this, "Animal");                                            // What is "tag"?
        Connector.creatureManager.Remove(this);

        Destroy(gameObject);
    }
}

public enum CreatureState
{
    NONE,
    RNDWALK,
    REST
}

public enum CreatureType
{
    NONE,
    HUMAN,
    ANIMAL
}

public enum Profession
{
    NONE,
    LABORER,
    HUNTER
}
