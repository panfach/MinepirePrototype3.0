using UnityEngine;
using UnityEngine.AI;

public abstract class Entity : MonoBehaviour
{
    public virtual BuildingData BldData { get => null; }
    public virtual CreatureData CrtData { get => null; }
    public virtual NatureData NtrData { get => null; }
    public virtual BuildingProperties BldProp { get => null; }
    public virtual CreatureProperties CrtProp { get => null; }
    public virtual NatureProperties NtrProp { get => null; }
    public virtual SmallInfoController SmallInfoController { get => null; }
    public virtual GridObject GridObject { get => null; }
    public virtual Inventory Inventory { get => null; }
    public virtual ColliderHandler ColliderHandler { get => null; }
    public virtual BuildSet BuildSet { get => null; }
    public virtual DisplayedItems DisplayedItems { get => null; }
    public virtual UIController UIController { get => null; }
    public virtual CreatureContainer CreatureContainer { get => null; }
    public virtual Appointer Appointer { get => null; }
    public virtual GeneralAI GeneralAI { get => null; }
    public virtual Health Health { get => null; }
    public virtual Satiety Satiety { get => null; }
    public virtual ResourceDeposit ResourceDeposit { get => null; }
    public virtual Rigidbody Rigidbody { get => null; }
    public virtual NavMeshAgent Agent { get => null; }
    public virtual AttackController AttackController { get => null; }
    public virtual Interactive Interactive { get => null; }
    public virtual Production Production { get => null; }
    public virtual Workplace Workplace { get => null; }

    public abstract void Die();
}
