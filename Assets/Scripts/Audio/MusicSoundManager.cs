using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -------------------- // MINEPIRE demo // -------------------- //
public class MusicSoundManager : MonoBehaviour
{
    public float breakDuration;
    new public AudioSource audio;
    public List<AudioClip> townSongs;

    void Start() 
    {
        StartPlaying();
    }

    public void StartPlaying()
    {
        audio.Stop();
        StopCoroutine(SongIteration());
        StartCoroutine(SongIteration());
    }

    public void StopPlaying()
    {
        audio.Stop();
        StopCoroutine(SongIteration());
    }

    IEnumerator SongIteration()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(breakDuration);

            int songInd = (int)Random.Range(0f, 100f) % townSongs.Count;

            audio.Stop();
            audio.PlayOneShot(townSongs[songInd]);

            yield return new WaitForSecondsRealtime(townSongs[songInd].length);
        }
    }
}
