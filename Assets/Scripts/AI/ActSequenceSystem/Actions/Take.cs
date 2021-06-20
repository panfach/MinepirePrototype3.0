using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class Take : ActionNode
    {
        [Input] public Connection enter;
        [Input] public ResourceQuery inputQuery;
        [Input] public Item item;

        [NonSerialized] ActionType type = ActionType.TAKE;
        [Range(0.0f, 5.0f)] public float initialDelay;
        [Range(0, 100)] public int priority;
        [Range(0.0f, 5.0f)] public float finalDelay;
        public TakeMode mode;

        [Output] public Connection trueConnection;
        [Output] public ResourceQuery outputQuery;
        [Output] public Connection falseConnection;


        public ResourceQuery ResourceQuery { get => GetInputValue("inputQuery", inputQuery); }
        public Item Item { get => GetInputValue("item", item); }


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.Take(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }

    public enum TakeMode
    {
        QUERY,
        ITEM
    }
}
