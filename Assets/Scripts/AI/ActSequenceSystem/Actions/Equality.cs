using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class Equality : ActionNode
    {
        [Input] public Connection enter;
        [Input] public Entity object1;
        [Input] public Entity object2;

        [NonSerialized] ActionType type = ActionType.EQUALITY;
        [NonSerialized] public int priority = 0;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public Entity Object1 { get => GetInputValue<Entity>("object1", null); }
        public Entity Object2 { get => GetInputValue<Entity>("object2", null); }


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.Equality(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}