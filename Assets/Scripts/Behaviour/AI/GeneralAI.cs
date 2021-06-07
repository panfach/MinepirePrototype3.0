﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSequenceSystem;
using XNode;

public class GeneralAI : MonoBehaviour
{
    public const float angleControlDelay = 0.1f;

    [Header("Entity")]
    [SerializeField] Entity entity;

    [Header("Settings")]
    [SerializeField] float maxRandomWalkDelay;
    [SerializeField] ActSequenceIndex generalActSequence;

    [Header("Info")]
    [SerializeField] ActionType actionType;
    [SerializeField] ActionNode currentAction;
    [SerializeField] ActSequenceGraph currentSequence;
    [SerializeField] ExtractedResourceLink destExtractedResource;
    [SerializeField] InteractionSpot destInteractionSpot;
    //[SerializeField] ResourceQuery resourceQuery;

    public ActionType ActionType { get => actionType; }
    public void ForgetExtractedResource() { destExtractedResource = null; }                // ? Remake extractedResourceLink. Merge occupation of extracted resources and interaction spots ???
    /*public ExtractedResourceLink DestExtractedResource
    {
        get => destInteractionSpot;
        set => destInteractionSpot = value;
    }*/
    public InteractionSpot DestInteractionSpot
    {
        get => destInteractionSpot;
        set
        {
            // destInteractionSpot.RemoveOccupation ...
            destInteractionSpot = value;
        }
    }
    //public ResourceQuery ResourceQuery { get => resourceQuery; }
    //public void AssignResourceQuery(ResourceQuery query) { resourceQuery = query; }


    private void OnEnable()
    {
        destInteractionSpot = null;
        DefineBehaviour();
    }


    public void DefineBehaviour(int priority = 0)
    {
        // Deffered definebehaviour
        // Nullify all links (destInteractionSpot, destExtractedResource ...)

        StopAllCoroutines();
        // Stop Interaction Coroutines
        actionType = ActionType.NONE;
        if (!gameObject.activeSelf) return;

        currentSequence = ActSequenceList.GetSequence(generalActSequence);
        currentAction = currentSequence.GetStart();
        StartCoroutine(AngleControl());
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

        // Deferred DefineBehaviour                                                                                  // Add deferred define behaviour

        while(currentAction != null)
        {
            actionType = currentAction.Type;
            yield return StartCoroutine(currentAction.Algorithm(entity as Creature));
        }
    }

    public void SwitchCurrentAction(ActionNode action)
    {
        if (action != null) Debug.Log("Switching to action : " + action.Type);
        if (action == null)
            DefineBehaviour(100);
        else
            currentAction = action;
    }

    public void SwitchCurrentSequence(ActSequenceIndex sequenceIndex)
    {
        Debug.Log("Switching to sequemce : " + sequenceIndex);
        if (sequenceIndex == ActSequenceIndex.NONE)
            currentSequence = ActSequenceList.GetSequence(generalActSequence);
        else
            currentSequence = ActSequenceList.GetSequence(sequenceIndex);
        currentAction = currentSequence.GetStart();
    }


    public static float GetViewAngle(Vector3 viewPoint)
    {
        return -180f * Mathf.Atan(viewPoint.z / viewPoint.x) / Mathf.PI + ((viewPoint.x < 0f) ? 180f : 0f);
    }

    void DefineAngle()
    {
        //float angle = -180f * Mathf.Atan(entity.Agent.velocity.z / entity.Agent.velocity.x) / Mathf.PI + ((entity.Agent.velocity.x < 0f) ? 180f : 0f);
        float angle = GetViewAngle(entity.Agent.velocity);
        LeanTween.rotateY(gameObject, angle, angleControlDelay);
    }

    IEnumerator AngleControl()
    {
        while (true)
        {
            if (entity.Agent.velocity.sqrMagnitude > 0.01f) DefineAngle();
            yield return new WaitForSeconds(angleControlDelay);
        }
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
