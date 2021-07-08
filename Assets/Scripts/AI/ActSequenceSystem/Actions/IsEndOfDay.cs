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
        [NonSerialized] public int priority = 0;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

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
