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
        [Range(0, 100)] public int priority;
        public GetQueryMode mode;

        [Output] public Connection trueConnection;
        [Output] public ResourceQuery query;


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

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
