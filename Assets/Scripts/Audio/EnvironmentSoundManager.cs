using UnityEngine;

// -------------------- // MINEPIRE demo // -------------------- //
public class EnvironmentSoundManager : MonoBehaviour
{
    [Header("Main")]
    public AudioSource envAudio;
    public AudioSource windAudio;

    [Header("Other settings")]
    [Range(0f, 1f)] public float minWindHeight;
    [Range(0f, 1f)] public float maxWindVolume;

    void Start()
    {
        StartPlaying();
    }

    public void StartPlaying()
    {
        envAudio.Stop();
        windAudio.Stop();
        envAudio.Play();
        windAudio.Play();
    }

    public void StopPlaying()
    {
        envAudio.Stop();
        windAudio.Stop();
    }

    void Update()
    {
        if (CameraScript.Height < minWindHeight)
            windAudio.volume = 0f;
        else
            windAudio.volume = (CameraScript.Height - minWindHeight) / (1f - minWindHeight) * maxWindVolume;

        // ! CameraScript.Height is between 0f and 1f !
        // Height = minWindHeight has volume = 0
        // Height = 1f has volume = 1f * maxWindVolume
    }
}
