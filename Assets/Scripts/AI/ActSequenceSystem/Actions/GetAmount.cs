using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class GetAmount : ActionNode
    {
        [Input] public Connection enter;
        [Input] public Building building;

        [NonSerialized] public ActionType type = ActionType.GETAMOUNT;
        [NonSerialized] public int priority = 0;
        public GetAmountTarget target;
        public ResourceIndex resIndex;
        public ResourceType resType;
        public bool countInsideBuilding;

        [Output] public Connection trueConnection;
        [Output] public float amount;


        public Building Building { get => GetInputValue("building", building); }


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.GetAmount(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "amount") return amount;
            else return null;
        }
    }

    public enum GetAmountTarget
    {
        NONE,
        RESINDEX,
        RESTYPE
    }
}