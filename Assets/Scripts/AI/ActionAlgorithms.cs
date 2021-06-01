using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public static class ActionAlgorithms
{
    /// <summary>
    /// Each act sequence graph must contain StartAction node in the beginning
    /// </summary>
    public static IEnumerator StartAction(Creature creature, ActSequenceSystem.StartAction action)
    {
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator IsEndOfDay(Creature creature, ActSequenceSystem.IsEndOfDay action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
        if (Sunlight.theEndOfDay)
            TrueReaction(creature, action);
        else
            FalseReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator SwitchSequence(Creature creature, ActSequenceSystem.SwitchSequence action)
    {
        // Body
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
        creature.GeneralAI.SwitchCurrentSequence(action.sequence);
        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
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

        // Body
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

        yield return new WaitForSeconds(action.finalDelay);
        TrueReaction(creature, action);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator RandomWalk(Creature creature, ActSequenceSystem.RandomWalk action)
    {
        // Body
        Vector3 dest;
        while (true)
        {
            float delayTime = Random.Range(1f, creature.CrtData.maxRandomWalkDelay);
            yield return new WaitForSeconds(delayTime);

            dest = creature.transform.position;
            dest += new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            dest.y = SCCoord.GetHeight(dest);
            creature.Agent.SetDestination(dest);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerator GoToBuilding(Creature creature, ActSequenceSystem.GoToBuilding action)
    {
        // Checking the possibility of execution
        if (action.building == null)
        {
            FalseReaction(creature, action);
            yield return null;
        }

        // Initial delay
        yield return new WaitForSeconds(action.initialDelay);

        // Body
        Building destBuilding = action.building;
        Vector3 destPoint = destBuilding.CreatureContainer.Enter.position;
        creature.Agent.SetDestination(destPoint);
        while (true)
        {
            if (destBuilding == null)
            {
                FalseReaction(creature, action);
                yield return null;
            }

            if (Vector3.SqrMagnitude(destPoint - creature.transform.position) < creature.CrtData.defaultActionDistance)
            {
                yield return new WaitForSeconds(action.finalDelay);
                TrueReaction(creature, action);
                yield return null;
            }

            yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);
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

// TEMPLATE OF AN ACTION ALGORITHM

/*
public static IEnumerator ActionName(Creature creature, ActSequenceSystem.ActionName action)
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
*/