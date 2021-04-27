using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public static class ActionAlgorithms
{
    public static IEnumerator EnterBuilding(Creature creature, ActSequenceSystem.EnterBuilding action)
    {
        // Checking the possibility of execution
        if (action.building == null || action.building.CreatureContainer == null)
        {
            FalseReaction(creature, action);
            yield return null;
        }

        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        creature.Agent.enabled = false;
        LeanTween.move(creature.gameObject, action.building.GridObject.GetCenter() + creature.CrtProp.HeightVector, creature.CrtData.timeOfBuildingEntering);
        yield return new WaitForSeconds(creature.CrtData.timeOfBuildingEntering);

        if (action.building == null)
        {
            creature.Agent.enabled = true;
            FalseReaction(creature, action);
            yield return null;
        }

        creature.CrtProp.PlaceOfStay = action.building;

        TrueReaction(creature, action);
        creature.GeneralAI.Act();
    }

    public static IEnumerator RandomWalk(Creature creature, ActSequenceSystem.RandomWalk action)
    {
        Vector3 dest;
        while (true)
        {
            float delayTime = Random.Range(1f, creature.CrtProp.maxRandomWalkDelay);
            yield return new WaitForSeconds(delayTime);

            dest = creature.transform.position;
            dest += new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            dest.y = SCCoord.GetHeight(dest);
            creature.Agent.SetDestination(dest);
        }
    }


    static void TrueReaction(Creature creature, Node action)
    {
        creature.GeneralAI.SwitchCurrentAction(action.GetOutputPort("trueConnection").Connection?.node as ActionNode);
    }

    static void FalseReaction(Creature creature, Node action)
    {
        creature.GeneralAI.SwitchCurrentAction(action.GetOutputPort("falseConnection").Connection?.node as ActionNode);
    }
}
