using System.Collections;
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
    [SerializeField] ExtractedResourceLink destExtractedResource;

    ActionNode currentAction;
    ActSequenceGraph currentSequence;

    public ActionType GetActionType { get => actionType; }
    public void ForgetExtractedResource() { destExtractedResource = null; }


    private void OnEnable()
    {
        StartCoroutine(AngleControl());
        DefineBehaviour();
    }


    public void DefineBehaviour(int priority = 0)
    {
        StopAllCoroutines();
        actionType = ActionType.NONE;

        currentSequence = ActSequenceList.GetSequence(generalActSequence);
        currentAction = currentSequence.GetStart();
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
        if (action == null)
            currentAction = currentSequence.GetStart();
        else
            currentAction = action;
    }

    public void SwitchCurrentSequence(ActSequenceIndex sequenceIndex)
    {
        if (sequenceIndex == ActSequenceIndex.NONE)
            currentSequence = ActSequenceList.GetSequence(generalActSequence);
        else
            currentSequence = ActSequenceList.GetSequence(sequenceIndex);
        currentAction = currentSequence.GetStart();
    }


    void DefineAngle()
    {
        float angle = -180f * Mathf.Atan(entity.Agent.velocity.z / entity.Agent.velocity.x) / Mathf.PI + ((entity.Agent.velocity.x < 0f) ? 180f : 0f);
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
