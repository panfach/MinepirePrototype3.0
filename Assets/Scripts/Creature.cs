using UnityEngine;
using UnityEngine.AI;

public class Creature : Entity
{
    [SerializeField] CreatureData creatureData;
    [SerializeField] CreatureProperties creatureProperties;
    [SerializeField] DisplayedItems displayedItems;
    [SerializeField] SmallInfoController smallInfoController;
    [SerializeField] ColliderHandler colliderHandler;
    [SerializeField] GeneralAI generalAI;
    [SerializeField] Health health;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] NavMeshAgent navMeshAgent;

    public override CreatureData CrtData { get => creatureData; }
    public override CreatureProperties CrtProp { get => creatureProperties; }
    public override DisplayedItems DisplayedItems { get => displayedItems; }
    public override SmallInfoController SmallInfoController { get => smallInfoController; }
    public override ColliderHandler ColliderHandler { get => colliderHandler; }
    public override GeneralAI GeneralAI { get => generalAI; }
    public override Health Health { get => health; }
    public override Rigidbody Rigidbody { get => _rigidbody; }
    public override NavMeshAgent Agent { get => navMeshAgent; }


    public override void Die()
    {
        CreatureManager.Creatures.Remove(this);

        Destroy(gameObject);
    }
}

public enum CreatureState
{
    NONE,
    RNDWALK,
    REST
}

public enum Profession
{
    NONE,
    LABORER,
    HUNTER
}
