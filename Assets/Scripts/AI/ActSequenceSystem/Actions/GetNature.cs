using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class GetNature : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] public ActionType type = ActionType.GETNATURE;
        [NonSerialized] public int priority = 0;
        public GetNatureMode target;

        [Output] public Connection trueConnection;
        [Output] public Nature nature;
        [Output] public Connection falseConnection;


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.GetNature(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "nature") return nature;
            else return null;
        }
    }

    public enum GetNatureMode
    {
        DESTEXTRACTEDRES
    }
}