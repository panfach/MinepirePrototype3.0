using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class GetBuilding : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] public ActionType type = ActionType.GETBUILDING;
        [NonSerialized] public int priority = 0;
        public GetBuildingMode target;

        [Output] public Connection trueConnection;
        [Output] public Building building;
        [Output] public Connection falseConnection;


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.GetBuilding(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "building") return building;
            else return null;
        }
    }

    public enum GetBuildingMode
    {
        PLACEOFSTAY,
        HOME,
        WORK,
        DESTBUILDING
    }
}
