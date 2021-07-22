using UnityEngine;

[System.Serializable]
public class InteractionSpot
{
    [SerializeField] Transform spot;
    [SerializeField] Transform interactionTarget;
    [SerializeField] bool eatType;
    [SerializeField] bool produceType;
    [SerializeField] bool reapType;
    [SerializeField] bool extractType;
    [SerializeField] float duration;

    Interactive interactive;
    GeneralAI actor;
    bool interactionProcess;
    float progress;
    int recipeIndex;

    public Interactive Interactive { get => interactive; }
    public Transform Spot { get => spot; }
    public Transform InteractionTarget { get => interactionTarget; }
    public GeneralAI Actor { get => actor; set { actor = value; } }
    public bool InteractionProcess { get => interactionProcess; set { interactionProcess = value; } }
    public float Progress { get => progress; set { progress = value; } }
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
        if (_actor.DestInteractionSpot != null && _actor.DestInteractionSpot.Actor != null) _actor.DestInteractionSpot.RemoveOccupation();
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
            case InteractionType.EXTRACT:
                if (extractType) interactive.StartExtractInteraction(creature, this);
                break;
        }
    }


    // public void StopInteract(Creature creature) { interactive.StopInteract(creature, this); }

    public void RemoveOccupation()
    {
        if (Actor != null && Actor.DestInteractionSpot != null) Actor.ForgetInteractionSpot();
        Actor = null;
        interactionProcess = false;
    }
}