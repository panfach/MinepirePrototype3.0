using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class GoToBuilding: ActionNode
    {
        [Input] public Connection enter;
        [Input] public Building building;

        [NonSerialized] public ActionType type = ActionType.GOTOBUILDING;
        [Range(0.0f, 5.0f)] public float initialDelay;
        [Range(0, 100)] public int priority;
        [Range(0.0f, 5.0f)] public float finalDelay;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public Building Building { get => GetInputValue<Building>("building", null); }


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.GoToBuilding(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            //GetInputValue<Building>()
            return null;
        }
    }
}