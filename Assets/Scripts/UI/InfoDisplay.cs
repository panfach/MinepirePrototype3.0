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
    public Image[] timePanelButtonImage;
    public Slider timePanelSlider;
    public Color currentTimeScaleColor;
    public Color defaultTimeScaleColor;

    float startFrameTime;
    float newFrameDuration = 0f;
    float oldFrameDuration = 0f;

    private void Awake()
    {
        happinessSlider.maxValue = 1.0f;
        //StartCoroutine(MeasureFrameDuration());
        StartCoroutine(DisplayFPS());
        StartCoroutine(DisplayTimeOfDay());
        StateManager.TimeScale = TimeScaleIndex.SCALE1;
        RefreshTimePanelButtons();
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

    public void RefreshTimePanelButtons()
    {
        foreach (Image item in timePanelButtonImage)
        {
            item.color = defaultTimeScaleColor;
        }

        if (StateManager.TimeScale == TimeScaleIndex.FREEZED) timePanelButtonImage[0].color = currentTimeScaleColor;
        if (StateManager.TimeScale == TimeScaleIndex.SCALE1) timePanelButtonImage[1].color = currentTimeScaleColor;
        if (StateManager.TimeScale == TimeScaleIndex.SCALE3) timePanelButtonImage[2].color = currentTimeScaleColor;
        if (StateManager.TimeScale == TimeScaleIndex.SCALE6) timePanelButtonImage[3].color = currentTimeScaleColor;
    }

    public void RefreshTimePanelSlider()
    {

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

    IEnumerator DisplayTimeOfDay()
    {
        float value;
        WaitForSeconds delay = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return delay;
            if (Sunlight.timeOfDay >= 0.84f && Sunlight.timeOfDay <= 1.00f)
            {
                value = Sunlight.timeOfDay - 0.84f;
                value /= 0.49f;
            }
            else if (Sunlight.timeOfDay >= 0.00f && Sunlight.timeOfDay <= 0.33f)
            {
                value = 0.16f + Sunlight.timeOfDay;
                value /= 0.49f;
            }
            else value = 1.0f;
            timePanelSlider.value = value;
        }
    }
}
