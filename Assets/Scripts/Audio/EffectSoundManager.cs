using UnityEngine;

// -------------------- // MINEPIRE demo // -------------------- //
public class EffectSoundManager : MonoBehaviour
{
    public AudioSource _as;
    public AudioClip buttonSound, smoothClick, cancelSound, slideSound, nextTurnSound, constructionSound;

    public void PlayButtonSound() {
        _as.PlayOneShot(buttonSound);
    }

    public void PlaySmoothClick() {
        _as.PlayOneShot(smoothClick);
    }

    public void PlayCancelSound() {
        _as.PlayOneShot(cancelSound);
    }

    public void PlaySlideSound() {
        _as.PlayOneShot(slideSound);
    }

    public void PlayNextTurnSound() {
        _as.PlayOneShot(nextTurnSound);
    }

    public void PlayConstructionSound() {
        _as.PlayOneShot(constructionSound);
    }
}
