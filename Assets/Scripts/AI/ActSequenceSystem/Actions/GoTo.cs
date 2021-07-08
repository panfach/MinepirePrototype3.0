using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class GoTo : ActionNode
    {
        [Input] public Connection enter;
        [Input] public Vector3 targetPos;

        [NonSerialized] public ActionType type = ActionType.GOTO;
        [Range(0.0f, 5.0f)] public float initialDelay;
        [Range(0, 100)] public int priority;
        public GoToMode mode;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public Vector3 TargetPos { get => GetInputValue("targetPos", targetPos); }


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.GoTo(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            //GetInputValue<Building>()
            return null;
        }
    }

    public enum GoToMode
    {
        NEARCOAST
    }
}