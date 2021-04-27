using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public abstract class ActionNode : Node
{
    abstract public ActionType Type { get; }
    abstract public IEnumerator Algorithm(Creature creature);
}
