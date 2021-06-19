using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] InteractionSpot[] interactionSpots;

    public InteractionSpot Spot() { return interactionSpots[0]; }
    public InteractionSpot Spot(int i) { return interactionSpots[i]; }
    public Transform SpotPos() { return interactionSpots[0].Spot; }
    public Transform SpotPos(int i) { return interactionSpots[i].Spot; }
    public bool IsOccupied
    {
        get
        {
            foreach (InteractionSpot spot in interactionSpots)
            {
                if (!spot.IsOccupied) return false;
            }
            return true;
        }
    }


    private void OnEnable()
    {
        foreach (InteractionSpot spot in interactionSpots)
        {
            spot.Init(this);
        }
    }


    public bool OccupyNearest(GeneralAI actor)
    {
        List<InteractionSpot> spots = new List<InteractionSpot>();

        foreach (InteractionSpot spot in interactionSpots)
        {
            if (!spot.IsOccupied) spots.Add(spot);
        }

        if (spots.Count == 0) return false;

        // Choosing the closest                                                                                // Make separate function for this
        Vector3 actorPos = actor.transform.position;
        InteractionSpot nearestSpot = null;
        float distance, minDistance = float.MaxValue;
        foreach (InteractionSpot spot in spots)
        {
            if ((distance = Vector3.SqrMagnitude(actorPos - spot.Spot.position)) < minDistance)
            {
                nearestSpot = spot;
                minDistance = distance;
            }
        }

        nearestSpot.Occupy(actor);

        return true;
    }

    public bool OccupyNearestRecipe(GeneralAI actor)
    {
        List<InteractionSpot> spots = new List<InteractionSpot>();

        foreach (int ind in actor.DestRecipe.interactionSpotIndex)
        {
            if (!interactionSpots[ind].IsOccupied) spots.Add(interactionSpots[ind]);
        }

        if (spots.Count == 0) return false;

        // Choosing the closest
        Vector3 actorPos = actor.transform.position;
        InteractionSpot nearestSpot = null;
        float distance, minDistance = float.MaxValue;
        foreach (InteractionSpot spot in spots)
        {
            if ((distance = Vector3.SqrMagnitude(actorPos - spot.Spot.position)) < minDistance)
            {
                nearestSpot = spot;
                minDistance = distance;
            }
        }

        nearestSpot.Occupy(actor);

        return true;
    }


    public void StartEatInteraction(Creature creature, InteractionSpot spot)
    {
        StartCoroutine(EatAlgorithm(creature, spot));
    }

/*    public void StopIndicatorInteraction(Creature creature, InteractionSpot spot)
    {
        StopCoroutine(entity.IndicatorInteraction.Interact(creature, spot));
    }*/

    public void StartProduceInteraction(Creature creature, InteractionSpot spot)
    {
        StartCoroutine(ProduceAlgorithm(creature, spot));
    }

    public void StartReapInteraction(Creature creature, InteractionSpot spot)
    {
        StartCoroutine(ReapAlgorithm(creature, spot));
    }


    public IEnumerator EatAlgorithm(Creature creature, InteractionSpot spot)
    {
        float amount, amountPerSecond;

        spot.InteractionProcess = true;
        LeanTween.move(creature.gameObject, spot.Spot.position, creature.CrtData.checkEventsDelay);
        LeanTween.rotate(creature.gameObject, new Vector3(0f, GeneralAI.GetViewAngle(spot.InteractionTarget.position), 0f), creature.CrtData.checkEventsDelay);
        yield return new WaitForSeconds(creature.CrtData.checkEventsDelay);

        amount = (creature.Inventory.StoredVal[0] + creature.Inventory.StoredVal[1]) / 2f;
        amountPerSecond = amount / spot.Duration;

        for (int i = 0; i < (int)spot.Duration; i++)
        {
            creature.Satiety.Value += amountPerSecond;
            yield return new WaitForSeconds(1f);
        }
        creature.Satiety.Value += amountPerSecond * (spot.Duration % 1f);
        creature.Inventory.ClearInventory();

        spot.RemoveOccupation();
    }

    public IEnumerator ProduceAlgorithm(Creature creature, InteractionSpot spot)
    {
        yield return new WaitForSeconds(spot.Duration);
        creature.Inventory.Give(entity.Inventory, new ResourceQuery(spot.Recipe.requiredRes));
        spot.Recipe.RemoveOccupation();
        spot.RemoveOccupation();
    }

    public IEnumerator ReapAlgorithm(Creature creature, InteractionSpot spot)
    {
        yield return new WaitForSeconds(spot.Duration);
        spot.RemoveOccupation();
        entity.Production.Reap(spot.Recipe, creature.GeneralAI);
    }


    private void OnDisable()
    {

    }
}

public enum InteractionType
{
    EAT,
    PRODUCE,
    REAP
}