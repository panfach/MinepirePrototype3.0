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
        [NonSerialized] public int priority;
        public PutMode mode;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public override ActionType Type { get => type; }

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