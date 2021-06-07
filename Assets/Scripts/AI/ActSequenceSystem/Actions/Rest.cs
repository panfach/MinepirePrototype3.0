using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class Rest : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] ActionType type = ActionType.REST;
        [Range(0, 100)] public int priority;


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.Rest(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
