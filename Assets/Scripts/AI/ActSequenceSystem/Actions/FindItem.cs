using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class FindItem : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] public ActionType type = ActionType.FINDITEM;
        [NonSerialized] public int priority = 0;
        public FindItemMode mode;

        [Output] public Connection trueConnection;
        [Output] public Item item;
        [Output] public Connection falseConnection;


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.FindItem(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "item") return item;
            else return null;
        }
    }

    public enum FindItemMode
    {
        WORKITEM,
        DROP
    }
}