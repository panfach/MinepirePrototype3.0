using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class RandomWalk : ActionNode
    {
        [Input] public Building building;

        [NonSerialized] ActionType type = ActionType.RNDWALK;

        [Range(0, 100)] public int priority;


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.RandomWalk(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
