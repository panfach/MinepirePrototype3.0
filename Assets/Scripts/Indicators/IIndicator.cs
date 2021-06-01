using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIndicator
{
    event SimpleEventHandler changedValueEvent;
    float Value { get; set; }
}
