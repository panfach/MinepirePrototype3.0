using System;
using UnityEngine;
using System.Collections.Generic;
using XNode;

namespace ActSequenceSystem
{
    public class EnterBuilding : Node
    {
        [Input] public Building building;

        [NonSerialized] public ActionType type = ActionType.ENTERBUILDING;

        [Range(0.0f, 5.0f)] public float initialDelay;
        [Range(0, 100)] public int priority;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection end;
        [Output] public Connection nullReaction;


        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
