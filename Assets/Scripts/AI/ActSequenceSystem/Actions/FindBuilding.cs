using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class FindBuilding : ActionNode
    {
        [Input] public Connection enter;
        [Input] public ResourceQuery query;

        [NonSerialized] public ActionType type = ActionType.FINDBUILDING;
        [Range(0, 100)] public int priority;
        public BuildingType bldType;
        public FindBuildingMode mode;

        [Output] public Connection trueConnection;
        [Output] public Building building;
        [Output] public Connection falseConnection;


        public ResourceQuery ResourceQuery { get => GetInputValue("query", query); } 


        public override ActionType Type { get => type; }
        public override int Priority { get => priority; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.FindBuilding(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "building") return building;
            else return null;
        }
    }

    public enum FindBuildingMode
    {
        NEAR,
        NEARQUERY,
        NEARINTERACT,
        NEARFREEPLACEFORQUERY,
        NEARFREEPLACEFORINV
    }
}