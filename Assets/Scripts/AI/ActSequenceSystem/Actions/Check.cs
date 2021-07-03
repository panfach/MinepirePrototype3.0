using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class Check : ActionNode
    {
        [Input] public Connection enter;
        [Input] public ResourceQuery query;
        [Input] public Item item;

        [NonSerialized] ActionType type = ActionType.CHECK;
        [NonSerialized] public int priority = 0;
        public CheckMode mode;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public ResourceQuery ResourceQuery { get => GetInputValue("query", query); }
        public Item Item { get => GetInputValue("item", item); }


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.Check(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }

    public enum CheckMode
    {
        EMPTYINV,
        EMPTYQUERY,
        AMILEADER,
        PLACEFORITEM
    }
}
