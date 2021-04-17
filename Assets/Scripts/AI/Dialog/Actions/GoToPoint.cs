using System;
using UnityEngine;
using System.Collections.Generic;
using XNode;

namespace ActSequenceSystem
{
    public class GoToPoint: Node
    {
        [Input] public Vector3 destPoint;

        [NonSerialized] public ActionType type = ActionType.GOTOPOINT;

        [Range(0.0f, 5.0f)] public float initialDelay;
        [Range(0, 100)] public int priority;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection end;


        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}