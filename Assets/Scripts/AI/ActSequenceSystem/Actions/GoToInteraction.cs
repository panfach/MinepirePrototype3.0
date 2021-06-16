using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class GoToInteraction : ActionNode
    {
        [Input] public Connection enter;
        [Input] public Building building;

        [NonSerialized] public ActionType type = ActionType.GOTOINTERACTION;
        [Range(0.0f, 5.0f)] public float initialDelay;
        [Range(0, 100)] public int priority;
        public GoToInteractionMode mode;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection trueConnection;
        [Output] public InteractionSpot spot;
        [Output] public Connection falseConnection;


        public Building Building { get => GetInputValue("building", building); }


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.GoToInteraction(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "spot") return spot;
            else return null;
        }
    }

    public enum GoToInteractionMode
    {
        NEAR,
        NEARRECIPE
    }
}