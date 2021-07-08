using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class Extract : ActionNode
    {
        [Input] public Connection enter;
        [Input] public ResourceQuery query;

        [NonSerialized] ActionType type = ActionType.EXTRACT;

        [Range(0, 100)] public int priority;
        public float duration;

        [Output] public Connection trueConnection;


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.Extract(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
