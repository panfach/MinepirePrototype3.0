using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class Interact : ActionNode
    {
        [Input] public Connection enter;
        [Input] public InteractionSpot spot;

        [NonSerialized] ActionType type = ActionType.INTERACT;

        [Range(0.0f, 5.0f)] public float initialDelay;
        [Range(0, 100)] public int priority;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public InteractionSpot Spot { get => GetInputValue<InteractionSpot>("spot", null); }


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.Interact(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}