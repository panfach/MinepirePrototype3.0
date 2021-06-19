using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class Construct : ActionNode
    {
        [Input] public Connection enter;
        [Input] public Building building;

        [NonSerialized] ActionType type = ActionType.CONSTRUCT;

        [Range(0.0f, 5.0f)] public float initialDelay;
        [Range(0, 100)] public int priority;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public Building Building { get => GetInputValue("building", building); }


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.Construct(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}