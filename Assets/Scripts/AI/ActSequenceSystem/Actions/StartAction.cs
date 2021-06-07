using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class StartAction : ActionNode
    {
        [NonSerialized] public ActionType type = ActionType.START;
        [NonSerialized] public int priority = 100;
        public ActSequenceIndex Index;

        [Output] public Connection trueConnection;


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.StartAction(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}