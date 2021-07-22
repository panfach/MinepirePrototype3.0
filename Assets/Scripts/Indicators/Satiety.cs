using UnityEngine;

public class Satiety : Indicator
{
    [Header("Info")]
    [SerializeField] float satiety;


    private void OnEnable()
    {
        Value = 1.0f;
    }


    public override float Value
    {
        get => satiety;
        set
        {
            if (!enabled) return;

            satiety = value;
            satiety = Mathf.Clamp(satiety, 0f, 1f);
            InvokeChangedEvent();

            if (satiety <= 0f) entity.Die();
        }
    }
}