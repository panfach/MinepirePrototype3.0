using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] InteractionSpot[] interactionSpots;

    public Transform Spot() { return interactionSpots[0].Spot; }
    public Transform Spot(int i) { return interactionSpots[i].Spot; }


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

        if (actor.DestInteractionSpot != null && actor.DestInteractionSpot.Actor != null) actor.DestInteractionSpot.RemoveOccupation();
        actor.DestInteractionSpot = nearestSpot;
        nearestSpot.Actor = actor;

        return true;
    }

    public void Interact(Creature creature, InteractionSpot spot)
    {
        if (spot.IndicatorInteraction) StartCoroutine(entity.IndicatorInteraction.Interact(creature, spot));
    }

    public void StopInteract(Creature creature, InteractionSpot spot)
    {
        if (spot.IndicatorInteraction) StopCoroutine(entity.IndicatorInteraction.Interact(creature, spot));
        spot.RemoveOccupation();
    }

    public void RemoveOccupation(InteractionSpot spot)
    {
        if (spot.Interactive != this) return;
        spot.Actor = null;
        spot.InteractionProcess = false;
    }

    public bool IsOccupied()
    {
        foreach (InteractionSpot spot in interactionSpots)
        {
            if (!spot.IsOccupied) return false;
        }
        return true;
    }


    private void OnDisable()
    {

    }
}

[System.Serializable]
public class InteractionSpot
{
    Interactive interactive;
    [SerializeField] Transform spot;
    [SerializeField] Transform interactionTarget;
    [SerializeField] bool indicatorInteraction;

    GeneralAI actor;
    bool interactionProcess;

    public Interactive Interactive { get => interactive; }
    public Transform Spot { get => spot; }
    public Transform InteractionTarget { get => interactionTarget; }
    public GeneralAI Actor { get => actor; set { actor = value; } }
    public bool InteractionProcess { get => interactionProcess; set { interactionProcess = value; } }
    public bool IndicatorInteraction { get => indicatorInteraction; }
    public bool IsOccupied { get => (actor != null); }


    public void Init(Interactive _interactive) 
    { 
        interactive = _interactive; 
    }

    public void Interact(Creature creature) { interactive.Interact(creature, this); }
    public void StopInteract(Creature creature) { interactive.StopInteract(creature, this); }

    public void RemoveOccupation()
    {
        interactive.RemoveOccupation(this);
    }
}