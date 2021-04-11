using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAI : MonoBehaviour
{
    public const float angleControlDelay = 0.1f;

    [Header("Entity")]
    [SerializeField] Entity entity;

    [Header("Settings")]
    [SerializeField] float maxRandomWalkDelay;

    [Header("Info")]
    [SerializeField] CreatureState state;

    Vector3 dest;

    public CreatureState State { get => state; }


    private void OnEnable()
    {
        DefineBehaviour();
    }


    public void DefineBehaviour()
    {
        StopAllCoroutines();
        state = CreatureState.NONE;

        StartCoroutine(RandomWalk());
    }

    private void DefineAngle()
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

    IEnumerator RandomWalk()
    {
        StartCoroutine(AngleControl());
        state = CreatureState.RNDWALK;

        Vector3 dest;
        while (true)
        {
            float delayTime = Random.Range(1f, maxRandomWalkDelay);
            yield return new WaitForSeconds(delayTime);

            dest = transform.position;
            dest += new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            dest.y = SCCoord.GetHeight(dest);
            entity.Agent.SetDestination(dest);
        }
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
