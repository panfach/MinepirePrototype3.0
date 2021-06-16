using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class GetQuery : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] ActionType type = ActionType.GETQUERY;
        [NonSerialized] public int priority = 0;
        public GetQueryMode mode;

        [Output] public Connection trueConnection;
        [Output] public ResourceQuery query;


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.GetQuery(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "query") return query;
            return null;
        }
    }

    public enum GetQueryMode
    {
        EAT,
        RECIPE
    }
}
