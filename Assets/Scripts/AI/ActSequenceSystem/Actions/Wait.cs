using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class Wait : ActionNode
    {
        [Input] public Connection enter;
        [Input] public float delay;

        [NonSerialized] ActionType type = ActionType.WAIT;
        [Range(0, 100)] public int priority;

        [Output] public Connection trueConnection;


        public float Delay { get => GetInputValue("delay", delay); }


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.Wait(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
