using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class SwitchSequence : ActionNode
    {
        [Input] public Connection enter;
        [Input] public ActSequenceIndex sequence;

        [NonSerialized] ActionType type = ActionType.SWITCHSEQ;

        [NonSerialized] public int priority = 100;


        public ActSequenceIndex Sequence { get => GetInputValue("sequence", sequence); }


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.SwitchSequence(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}


