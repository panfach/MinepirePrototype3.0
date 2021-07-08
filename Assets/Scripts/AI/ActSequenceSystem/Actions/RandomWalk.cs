using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class RandomWalk : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] ActionType type = ActionType.RNDWALK;

        [Range(0, 100)] public int priority;
        public RandomWalkMode mode;

        [Output] public Connection trueConnection;


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.RandomWalk(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }

    public enum RandomWalkMode
    {
        ENDLESS,
        ONETIME
    }
}
