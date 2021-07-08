using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -------------------- // MINEPIRE demo // -------------------- //
public class EffectSoundManager : MonoBehaviour
{
    public AudioSource _as;
    public float playDelayDuration;
    public AudioClip buttonSound, smoothClick, cancelSound, slideSound, nextTurnSound, constructionSound;

    WaitForSeconds playDelay;


    void Awake()
    {
        playDelay = new WaitForSeconds(playDelayDuration);
    }


    public void PlayButtonSound() {
        StopAllCoroutines();
        StartCoroutine(PlayWithDelay(buttonSound));
    }

    public void PlaySmoothClick() {
        _as.PlayOneShot(smoothClick);
    }

    public void PlayCancelSound() {
        StopAllCoroutines();
        StartCoroutine(PlayWithDelay(cancelSound));
    }

    public void PlaySlideSound() {
        StopAllCoroutines();
        StartCoroutine(PlayWithDelay(slideSound));
    }

    public void PlayNextTurnSound() {
        _as.PlayOneShot(nextTurnSound);
    }

    public void PlayConstructionSound() {
        _as.PlayOneShot(constructionSound);
    }


    IEnumerator PlayWithDelay(AudioClip clip)
    {
        yield return playDelay;
        _as.PlayOneShot(clip);
    }
}
