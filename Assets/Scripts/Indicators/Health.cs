using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IIndicator
{
    [Header("Entity")]
    public Entity entity;

    [Header("Info")]
    [SerializeField] float health;
    [SerializeField] AttackController lastAttacker;                                                                  // temporal ?

    public event SimpleEventHandler changedValueEvent;

    public AttackController LastAttacker { get => lastAttacker; }


    private void OnEnable()
    {
        Value = entity.CrtData.MaxHealth;
    }


    public float Value
    {
        get => health;
        set
        {
            health = value;
            health = Mathf.Clamp(health, 0f, entity.CrtData.MaxHealth);
            changedValueEvent?.Invoke();

            //if (smallInfo != null && smallInfo.enabled) smallInfo.Refresh();                     // Implement this in future

            if (health <= 0f)
            {
                entity.Die();
            }
        }
    }

    public void GetDamage(AttackController sender, float damage)
    {
        lastAttacker = sender;                                            
        Value -= damage;
    }


    /*public void Die(GameObject killer = null)                                                              
    {
        CreatureManager.Creatures.Remove(entity as Creature);                                          // In future merge VillagerManager and AnimalManager to new CreatureManager
        CreatureManager.animalPopulation--;                                                                // In future CreatureProperties should contain CreatureManager.Remove(entity) in OnDisable()

        Connector.itemManager.DropItems(entity as Creature, "Animal");
        killer?.GetComponent<Villager>()?.SetFutureDestObj(entity.CrtProp.DroppedItemsAsGameObjects);            // ~ Curve drop system

        Destroy(gameObject);
    }*/
}
