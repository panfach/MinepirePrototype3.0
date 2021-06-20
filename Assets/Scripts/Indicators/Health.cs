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
            if (!enabled) return;

            health = value;
            health = Mathf.Clamp(health, 0f, entity.CrtData.MaxHealth);
            changedValueEvent?.Invoke();

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
}
