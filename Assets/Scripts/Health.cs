using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] Entity entity;

    [Header("Info")]
    [SerializeField] float health;
    public GameObject lastAttacker;                                                                  // temporal ?

    public event SimpleEventHandler changeHealthEvent;


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
            if (health <= 0f)
            {
                Die(lastAttacker);
            }
            health = Mathf.Clamp(health, 0f, entity.CrtData.MaxHealth);
            changeHealthEvent?.Invoke();

            //if (smallInfo != null && smallInfo.enabled) smallInfo.Refresh();                     // Implement this in future
        }
    }

    public void GetDamage(GameObject sender, float damage)
    {
        if (sender.GetComponent<Villager>() != null) lastAttacker = sender;                        // Rewrite it to "if (sender.GetComponent<AttackController>() != null) ...
        Value -= damage;
    }


    public void Die(GameObject killer = null)                                                              
    {
        CreatureManager.Creatures.Remove(entity as Creature);                                          // In future merge VillagerManager and AnimalManager to new CreatureManager
        CreatureManager.animalPopulation--;                                                                // In future CreatureProperties should contain CreatureManager.Remove(entity) in OnDisable()

        Connector.itemManager.DropItems(entity as Creature, "Animal");
        killer?.GetComponent<Villager>()?.SetFutureDestObj(entity.CrtProp.DroppedItemsAsGameObjects);            // ~ Curve drop system

        Destroy(gameObject);
    }
}
