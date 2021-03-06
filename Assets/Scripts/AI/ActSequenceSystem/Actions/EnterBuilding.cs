using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class EnterBuilding : ActionNode
    {
        [Input] public Connection enter;
        [Input] public Building building;

        [NonSerialized] ActionType type = ActionType.ENTERBUILDING;

        [Range(0.0f, 5.0f)] public float initialDelay;
        [NonSerialized] public int priority = 80;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public Building Building { get => GetInputValue<Building>("building", null); }


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.EnterBuilding(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
