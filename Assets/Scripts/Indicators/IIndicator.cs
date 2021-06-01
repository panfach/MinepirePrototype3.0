using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIndicator
{
    public event SimpleEventHandler changedValueEvent;
    public float Value { get; set; }
}
