using System.Collections;
using UnityEngine;

// -------------------- // MINEPIRE demo // -------------------- //
public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 origPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3((origPosition.x + x), (origPosition.y+y), origPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = origPosition;
    }
}
