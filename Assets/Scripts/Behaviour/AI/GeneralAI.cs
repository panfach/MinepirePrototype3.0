using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSequenceSystem;
using XNode;

public class GeneralAI : MonoBehaviour
{
    public const float angleControlDelay = 0.1f;

    [Header("Entity")]
    public Entity entity;

    [Header("Settings")]
    [SerializeField] float maxRandomWalkDelay;
    [SerializeField] ActSequenceIndex generalActSequence;
    [SerializeField] bool debugLogActions;

    [Header("Destinations")]
    [SerializeReference] Recipe destRecipe;
    [SerializeField] ExtractedResourceLink destExtractedResource;
    [SerializeReference] InteractionSpot destInteractionSpot;
    [SerializeField] Entity destEntity;
    [SerializeField] Inventory destInv;

    [Header("Info")]
    [SerializeField] ActionType actionType;
    [SerializeField] ActionNode currentAction;
    [SerializeField] ActSequenceGraph currentSequence;
    bool deferredDefineBehaviour;
    int deferredBehaviourPriority;

    public ActionType ActionType { get => actionType; }
    public void ForgetExtractedResource() { destExtractedResource = null; }                // ? Remake extractedResourceLink. Merge occupation of extracted resources and interaction spots ???
    public void ForgetDestRecipe() { Debug.Log("ForgetDestRecipe!"); destRecipe = null; }
    public void ForgetInteractionSpot() { destInteractionSpot = null; }
    public void ForgetDestEntity() { destEntity = null; }
    public void ForgetDestInventory() { destInv = null; }
    public ExtractedResourceLink DestExtractedResource
    {
        get => destExtractedResource;
        set => destExtractedResource = value;
    }
    public Recipe DestRecipe
    {
        get => destRecipe;
        set { Debug.Log("DestRecipe set: " + value); destRecipe = value; }
    }
    public InteractionSpot DestInteractionSpot
    {
        get => destInteractionSpot;
        set => destInteractionSpot = value;
    }
    public Entity DestEntity
    {
        get => destEntity;
        set => destEntity = value;
    }
    public Inventory DestInv
    {
        get => destInv;
        set => destInv = value;
    }
    public ActionType CurrentAction { get => actionType; }

    Coroutine angleControlCoroutine;


    private void OnEnable()
    {
        destInteractionSpot = null;
        entity.Agent.speed = entity.CrtData.WalkSpeed;
        DefineBehaviour();
    }


    public void DefineBehaviour(int priority = 0)
    {
        if (currentAction != null && priority < currentAction.Priority)
        {
            deferredDefineBehaviour = true;
            if (priority > deferredBehaviourPriority)
                deferredBehaviourPriority = priority;
            return;
        }
        else
        {
            deferredDefineBehaviour = false;
            deferredBehaviourPriority = 0;
        }

        FreeOccupations();

        StopAllCoroutines();
        // Stop Interaction Coroutines
        actionType = ActionType.NONE;
        if (!gameObject.activeSelf) return;

        currentSequence = ActSequenceList.GetSequence(generalActSequence);
        currentAction = currentSequence.GetStart();
        angleControlCoroutine = StartCoroutine(AngleControl());
        StartCoroutine(Act());
    }

    public IEnumerator Act()
    {
        if (currentAction == null)
        {
            if (currentSequence == null)
                currentSequence = ActSequenceList.GetSequence(generalActSequence);
            currentAction = currentSequence.GetStart();
        }

        while(currentAction != null)
        {
            if (deferredDefineBehaviour) DefineBehaviour(deferredBehaviourPriority);

            actionType = currentAction.Type;
            SwitchToWalk();
            yield return StartCoroutine(currentAction.Algorithm(entity as Creature));
        }
    }

    public void SwitchCurrentAction(ActionNode action)
    {
        if (debugLogActions && action != null) Debug.Log(entity.CrtData.Name + " : Switching to action : " + action.Type);
        if (action == null)
            DefineBehaviour(100);
        else
            currentAction = action;
    }

    public void SwitchCurrentSequence(ActSequenceIndex sequenceIndex)
    {
        if (debugLogActions) Debug.Log(entity.CrtData.Name + " : Switching to sequemce : " + sequenceIndex);
        if (sequenceIndex == ActSequenceIndex.NONE)
            currentSequence = ActSequenceList.GetSequence(generalActSequence);
        else
            currentSequence = ActSequenceList.GetSequence(sequenceIndex);
        currentAction = currentSequence.GetStart();
    }

    public void FreeOccupations()
    {
        if (destRecipe != null) destRecipe.RemoveOccupation();
        if (destExtractedResource != null) destExtractedResource.RemoveOccupation(this);
        if (destInteractionSpot != null) destInteractionSpot.RemoveOccupation();
        if (destEntity != null) ForgetDestEntity();
        if (destInv != null) destInv.RemoveOccupation((Creature)entity);
    }

    public void SwitchToWalk()
    {
        entity.Agent.speed = entity.CrtData.WalkSpeed;
    }

    public void SwitchToRun()
    {
        entity.Agent.speed = entity.CrtData.RunSpeed;
    }


    public static float GetViewAngle(Vector3 viewPoint)
    {
        return -180f * Mathf.Atan(viewPoint.z / viewPoint.x) / Mathf.PI + ((viewPoint.x < 0f) ? 180f : 0f);
    }

    void DefineAngle()
    {
        //float angle = -180f * Mathf.Atan(entity.Agent.velocity.z / entity.Agent.velocity.x) / Mathf.PI + ((entity.Agent.velocity.x < 0f) ? 180f : 0f);
        float angle = GetViewAngle(entity.Agent.velocity);
        //if (entity.Appointer != null && entity.Appointer.Home != null) Debug.Log(entity.Agent.velocity);
        LeanTween.rotateY(gameObject, angle, angleControlDelay);
    }

    public void StopAngleControl()
    {
        if (angleControlCoroutine != null) StopCoroutine(angleControlCoroutine);
    }

    IEnumerator AngleControl()
    {
        while (true)
        {
            yield return new WaitForSeconds(angleControlDelay);
            if (entity.Agent.velocity.sqrMagnitude > 0.01f) DefineAngle();
            //if (entity.Appointer != null && entity.Appointer.Home != null) Debug.Log("Angle Control");
        }
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
