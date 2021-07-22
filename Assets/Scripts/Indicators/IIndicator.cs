using UnityEngine;

public class Indicator : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    public event SimpleEventHandler changedValueEvent;


    public virtual float Value { get; set; }

    public void InvokeChangedEvent() { changedValueEvent?.Invoke(); }
}
