using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class GoToCreature : ActionNode
    {
        [Input] public Connection enter;
        [Input] public Health creature;                                                    // Temporal

        [NonSerialized] public ActionType type = ActionType.GOTOCREATURE;
        [Range(0.0f, 5.0f)] public float initialDelay;
        [Range(0, 100)] public int priority;
        public GoToCreatureMode mode;
        public float requiredDistance;
        public float groupDistance;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public Health Creature { get => GetInputValue("creature", creature); }


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.GoToCreature(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }

    public enum GoToCreatureMode
    {
        DEFAULT,
        WORKGROUP
    }
}