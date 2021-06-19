using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class InteractionSpot
{
    [SerializeField] Transform spot;
    [SerializeField] Transform interactionTarget;
    [SerializeField] bool eatType;
    [SerializeField] bool produceType;
    [SerializeField] bool reapType;
    [SerializeField] float duration;

    Interactive interactive;
    GeneralAI actor;
    bool interactionProcess;
    int recipeIndex;

    public Interactive Interactive { get => interactive; }
    public Transform Spot { get => spot; }
    public Transform InteractionTarget { get => interactionTarget; }
    public GeneralAI Actor { get => actor; set { actor = value; } }
    public bool InteractionProcess { get => interactionProcess; set { interactionProcess = value; } }
    public bool EatType { get => eatType; }
    public bool ProduceType { get => produceType; }
    public bool ReapType { get => reapType; }
    public float Duration { get => duration; }
    public void AssignRecipe(int recipe) { recipeIndex = recipe; }
    public Recipe Recipe { get => interactive.entity.Production.Recipe(recipeIndex); }
    public bool IsOccupied { get => (actor != null); }


    public void Init(Interactive _interactive)
    {
        interactive = _interactive;
    }

    public void Occupy(GeneralAI _actor)
    {
        if (actor != null) return;
        if (_actor.DestInteractionSpot != null && _actor.DestInteractionSpot.Actor != null) actor.DestInteractionSpot.RemoveOccupation();
        _actor.DestInteractionSpot = this;
        actor = _actor;
    }

    public void Interact(Creature creature, InteractionType type)
    {
        switch (type)
        {
            case InteractionType.EAT:
                if (eatType) interactive.StartEatInteraction(creature, this);
                break;
            case InteractionType.PRODUCE:
                if (produceType) interactive.StartProduceInteraction(creature, this);
                break;
            case InteractionType.REAP:
                if (reapType) interactive.StartReapInteraction(creature, this);
                break;
        }
    }


    // public void StopInteract(Creature creature) { interactive.StopInteract(creature, this); }

    public void RemoveOccupation()
    {
        Actor.DestInteractionSpot = null;
        Actor = null;
    }
}