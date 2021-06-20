using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class Attack : ActionNode
    {
        [Input] public Connection enter;
        [Input] public Health target;

        [NonSerialized] public ActionType type = ActionType.ATTACK;
        [Range(0.0f, 5.0f)] public float initialDelay;
        [Range(0, 100)] public int priority;
        public AttackMode mode;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection trueConnection;


        public Health Target { get => GetInputValue("target", target); }


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.Attack(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }

    public enum AttackMode
    {
        DEFAULT,
        WORKGROUP
    }
}