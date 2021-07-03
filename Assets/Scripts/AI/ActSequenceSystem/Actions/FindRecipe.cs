using System;
using UnityEngine;
using System.Collections;
using XNode;

namespace ActSequenceSystem
{
    public class FindRecipe : ActionNode
    {
        [Input] public Connection enter;

        [NonSerialized] public ActionType type = ActionType.FINDRECIPE;
        [NonSerialized] public int priority = 0;
        public FindRecipeMode mode;

        [Output] public Connection trueConnection;
        [Output] public Connection falseConnection;


        public override ActionType Type { get => type; }

        public override IEnumerator Algorithm(Creature creature)
        {
            return ActionAlgorithms.FindRecipe(creature, this);
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }

    public enum FindRecipeMode
    {
        PROFPRODUCEWORK,
        PROFREAPWORK
    }
}
