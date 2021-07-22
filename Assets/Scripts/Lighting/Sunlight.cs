using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
/// <summary>
/// Этот скрипт должен висеть на основном источнике света.
/// </summary>
public class Sunlight : MonoBehaviour
{
    public static float timeOfDay = 0.84f;
    public static bool theEndOfDay = false;

    /* * * * * * * * * *
     * 8:00       0.84 *
     * 12:00      0.00 *
     * 20:00      0.33 *
     * 00:00      0.50 *
     * * * * * * * * * */

    [Header("Options")]
    public float rotationDurationOfChangingTurn, daySpeed;
    public Vector3 rotationAxis;
    public DayNightChangeLightingPreset lightingPreset;

    bool isTurnChanging = false;
    float timeStart, currentAngle, lastAngle, smallRotationAngle;


    private void Awake()
    {
        UpdateLighting(timeOfDay);

        // Начальная предустановка
        lastAngle = 360f * timeOfDay; 
        transform.Rotate(rotationAxis, -360f * (1 - timeOfDay), Space.World);
    }

    private void LateUpdate()
    {
        if (isTurnChanging)
        {
            UpdateLighting(timeOfDay);

            if (Time.time - timeStart > rotationDurationOfChangingTurn)
            {
                isTurnChanging = false;
                lastAngle = 360f * timeOfDay;

                TimeEvents.StartOfTheDay();
            }
        }
        else if (!theEndOfDay)
        {
            if (timeOfDay < 0.5f && timeOfDay > 0.33f)
            {
                theEndOfDay = true;
                TimeEvents.EndOfTheDay();
            }
            else
            {
                TimeUpdate();
                UpdateLighting(timeOfDay);
                DayRotate();
            }
        }
    }


    public void ChangeTurn()
    {
        if (!isTurnChanging && (StateManager.GeneralState == GameState.ORD))
        {
            theEndOfDay = false;

            // Переключение режимов
            isTurnChanging = true;

            // Запоминаем время начала
            timeStart = Time.time;

            // Изменение timeOfDay и угла поворота освещения во время смены хода
            LeanTween.value(gameObject, LeanTweenUpdateTimeOfDay, timeOfDay, 0.84f, rotationDurationOfChangingTurn).setEaseInOutQuad();
            LeanTween.rotateAround(gameObject, rotationAxis, 180f, rotationDurationOfChangingTurn).setEaseInOutQuad();
        }
    }

    private void UpdateLighting(float time)
    {
        RenderSettings.ambientLight = lightingPreset.ambientColor.Evaluate(time);
        RenderSettings.fogColor = lightingPreset.fogColor.Evaluate(time);
        GetComponent<Light>().color = lightingPreset.directionalColor.Evaluate(time);
        GetComponent<Light>().intensity = lightingPreset.sunlightIntensity.Evaluate(time);
    }

    void LeanTweenUpdateTimeOfDay(float time)
    {
        timeOfDay = time;
    }

    void TimeUpdate()
    {
        timeOfDay += daySpeed * Time.deltaTime;
        if (timeOfDay >= 1f) timeOfDay -= 1f;
    }

    void DayRotate()
    {
        currentAngle = 360f * timeOfDay;
        smallRotationAngle = currentAngle - lastAngle;
        if (smallRotationAngle < 0) smallRotationAngle = 360f - smallRotationAngle;
        transform.Rotate(rotationAxis, smallRotationAngle, Space.World);

        lastAngle = currentAngle;
    }
}
