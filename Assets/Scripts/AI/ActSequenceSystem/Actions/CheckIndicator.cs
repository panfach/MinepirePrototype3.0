using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
    public class CheckIndicator : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] ActionType type = ActionType.CHECKINDICATOR;
        [NonSerialized] public int priority = 0;
        public IndicatorType indicator;
        public ComparingOperator @operator;
        public float value;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.CheckIndicator(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }

    public enum IndicatorType
    {
        NONE,
        HEALTH,
        SATIETY
    }

    public enum ComparingOperator
    {
        EQUAL,
        GREATER,
        GREATEREQUAL,
        LESS,
        LESSEQUAL
    }
}