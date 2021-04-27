using System;
using UnityEngine;
using System.Collections.Generic;
using XNode;

namespace ActSequenceSystem
{
    public class StartAction : Node
    {
        [NonSerialized] public ActionType type = ActionType.START;

        [NonSerialized] public int priority = 100;

        [Output] public Connection trueConnection;


        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
