using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class FindTarget : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] public ActionType type = ActionType.FINDTARGET;
        [NonSerialized] public int priority = 0;
        public BuildingIndex[] indices;
        public FindTargetMode mode;

        [Output] public Connection trueConnection;
        [Output] public Health target;
        [Output] public Connection falseConnection;


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.FindTarget(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "target") return target;
            else return null;
        }
    }

    public enum FindTargetMode
    {
        NEARANIMAL,
        DESTENTITY
    }
}