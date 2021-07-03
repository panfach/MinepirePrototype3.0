using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class ExitBuilding : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] ActionType type = ActionType.EXITBUILDING;
        [Range(0.0f, 5.0f)] public float initialDelay;
        [NonSerialized] public int priority = 80;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection trueConnection;


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.ExitBuilding(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}