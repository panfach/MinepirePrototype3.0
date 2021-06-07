using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satiety : MonoBehaviour, IIndicator
{
    [Header("Entity")]
    public Entity entity;

    [Header("Info")]
    [SerializeField] float satiety;
    public GameObject lastAttacker;                                                                  // temporal ?

    public event SimpleEventHandler changedValueEvent;


    private void OnEnable()
    {
        Value = 1.0f;
    }


    public float Value
    {
        get => satiety;
        set
        {
            satiety = value;
            satiety = Mathf.Clamp(satiety, 0f, 1f);
            changedValueEvent?.Invoke();
            //if (smallInfo != null && smallInfo.enabled) smallInfo.Refresh();                     // Implement this in future

            if (satiety <= 0f) entity.Die();
        }
    }
}