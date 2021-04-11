using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "ScriptableObjects/DNChange Lighting Preset")]
public class DayNightChangeLightingPreset : ScriptableObject
{
    public Gradient ambientColor;
    public Gradient directionalColor;
    public Gradient fogColor;
    public AnimationCurve sunlightIntensity;
}
