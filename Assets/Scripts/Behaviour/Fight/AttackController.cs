using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public class AttackController : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] bool canAttackCreatures;
    [SerializeField] float attackPower;

    [Header("Info")]
    [SerializeField] float attackModifier = 1f;

    Item[] dropItems;

    public float ImpactDamage
    {
        get => attackPower * attackModifier;
    }


    public bool Attack(Health target)
    {
        if (!canAttackCreatures && target.entity is Creature) return false;

        target.GetDamage(this, ImpactDamage);
        return true;
    }

    public void AssignDrop(Item[] _dropItems)
    {
        dropItems = _dropItems;
    }
}
