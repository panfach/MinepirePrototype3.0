using UnityEngine;

public class Health : Indicator
{
    [Header("Info")]
    [SerializeField] float health;
    [SerializeField] AttackController lastAttacker;

    public AttackController LastAttacker { get => lastAttacker; }


    private void OnEnable()
    {
        Value = entity.CrtData.MaxHealth;
    }


    public override float Value
    {
        get => health;
        set
        {
            if (!enabled) return;

            health = value;
            health = Mathf.Clamp(health, 0f, entity.CrtData.MaxHealth);
            InvokeChangedEvent();

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
