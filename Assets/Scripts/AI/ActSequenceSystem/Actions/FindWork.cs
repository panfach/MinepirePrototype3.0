using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class FindWork : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] public ActionType type = ActionType.FINDWORK;
        [NonSerialized] public int priority = 0;

        [Output] public Connection trueConnection;
        [Output] public ActSequenceIndex sequence;
        [Output] public Connection falseConnection;


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.FindWork(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "sequence") return sequence;
            else return null;
        }
    }
}
