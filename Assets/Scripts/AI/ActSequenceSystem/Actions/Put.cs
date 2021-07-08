using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class Put : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] ActionType type = ActionType.PUT;
        [Range(0, 100)] public int priority;
        public PutMode mode;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.Put(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }

    public enum PutMode
    {
        CLEARINV
    }
}