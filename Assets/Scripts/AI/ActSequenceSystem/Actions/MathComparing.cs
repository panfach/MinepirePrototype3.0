using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class MathComparing : ActionNode
    {
        [Input] public Connection enter;
        [Input] public float value1;
        [Input] public float value2;

        [NonSerialized] ActionType type = ActionType.MATHCOMPARING;
        [NonSerialized] public int priority = 0;
        public MathComparingOperator @operator;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public float Value1 { get => GetInputValue("value1", value1); }
        public float Value2 { get => GetInputValue("value2", value2); }


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.MathComparing(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }

    public enum MathComparingOperator
    {
        EQUAL,
        GREATER,
        GREATEREQUAL,
        LESS,
        LESSEQUAL
    }
}
