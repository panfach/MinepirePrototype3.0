using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class IsEndOfDay : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] ActionType type = ActionType.ISENDOFDAY;

        [Range(0.0f, 5.0f)] public float initialDelay;
        [Range(0, 100)] public int priority;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.IsEndOfDay(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
