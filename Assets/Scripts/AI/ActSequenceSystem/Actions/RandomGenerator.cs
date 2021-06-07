using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class RandomGenerator : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] public ActionType type = ActionType.RANDOMGENERATOR;
        [NonSerialized] public int priority = 0;
        public RandomGeneratorMode mode;
        public float minRndNumber;
        public float maxRndNumber;

        [Output] public Connection trueConnection;
        [Output] public float rndNumber;


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.RandomGenerator(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "rndNumber") return rndNumber;
            else return null;
        }
    }

    public enum RandomGeneratorMode
    {
        NUM
    }
}
