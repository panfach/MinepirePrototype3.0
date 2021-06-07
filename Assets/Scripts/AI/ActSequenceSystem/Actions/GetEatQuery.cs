using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class GetEatQuery : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] ActionType type = ActionType.GETEATQUERY;
        [NonSerialized] public int priority = 0;

        [Output] public Connection trueConnection;
        [Output] public ResourceQuery query;


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.GetEatQuery(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "query") return query;
            return null;
        }
    }
}
