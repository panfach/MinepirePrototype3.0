using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{
    [Header("Panels")]
    public PopulationInfo populInfo;
    public ResourceInfo resInfo;
    public BuildingInfo bldInfo;
    public CreatureInfo crtInfo;

    [Header("Other elements")]
    public Slider happinessSlider;
    public TextMeshProUGUI happinessValue;
    public TextMeshProUGUI fpsCounter;

    float startFrameTime;
    float newFrameDuration = 0f;
    float oldFrameDuration = 0f;

    private void Awake()
    {
        happinessSlider.maxValue = 1.0f;
        //StartCoroutine(MeasureFrameDuration());
        StartCoroutine(DisplayFPS());
    }

    private void FixedUpdate()
    {

    }

    public void RefreshAllInfo()
    {
        populInfo.Refresh();
        resInfo.Refresh();
        bldInfo.Refresh();
        crtInfo.Refresh();
        RefreshOtherElements();
    }

    public static void Refresh()
    {
        Connector.infoDisplay.RefreshAllInfo();
    }

    void RefreshOtherElements()
    {
        happinessSlider.value = VillageData.happiness;
        happinessValue.text = (VillageData.happiness * 100.0f).ToString("F0");
    }


    IEnumerator MeasureFrameDuration()
    {

        while (true)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForFixedUpdate();
            if (startFrameTime == 0f) startFrameTime = Time.time;
            yield return new WaitForEndOfFrame();
            newFrameDuration = Time.time - startFrameTime;
            fpsCounter.text = ((newFrameDuration + oldFrameDuration) * 1000f / 2f).ToString("F2") + " ms";
            oldFrameDuration = newFrameDuration;
            startFrameTime = 0f;
            yield return new WaitForSecondsRealtime(1.0f);
        }
    }

    IEnumerator DisplayFPS()
    {
        WaitForSecondsRealtime fpsDisplayDelay = new WaitForSecondsRealtime(0.2f);

        while (true)
        {
            fpsCounter.text = ((int)(1f / Time.unscaledDeltaTime)).ToString();
            yield return fpsDisplayDelay;
        }
    }
}
