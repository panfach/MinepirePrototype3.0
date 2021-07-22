using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public static class StateManager
{
    public static GameState GeneralState { get; private set; }
    public static TimeScaleIndex TimeScale { get; set; }

    static bool buildingInfo = false;
    static bool resourceSourceInfo = false;

    public static bool BuildingInfo 
    {
        get => buildingInfo; 
        set { if (value) ResourceSourceInfo = false; buildingInfo = value; }
    }
    public static bool ResourceSourceInfo 
    { 
        get => resourceSourceInfo; 
        set { if (value) BuildingInfo = false; resourceSourceInfo = value; } 
    }
    public static bool GodMode { get; set; } = false;
    public static bool CreatureInfo { get; set; } = false;
    public static bool VillagerDragging { get; set; } = false;                                                         
    public static bool TimeIsFreezed { get; set; } = false;
    public static bool CameraIsFreezed { get; set; } = false;

    // -------------------------------------------------------------------------------------------------- //

    // -------------------------------------------------------------------------------------------------- //

    static float oldTimeScale;

    public static void SetOrdinaryMode() 
    {
        FreezeTime(false);
        CameraIsFreezed = false;
        GeneralState = GameState.ORD;
    }

    public static bool SetBuildMode() 
    {
        if (GeneralState == GameState.ORD || GeneralState == GameState.MAPEDIT)
        {
            GeneralState = GameState.BUILD;
            return true;
        }
        return false;
    }

    public static bool SetPauseMode() 
    {
        if (GeneralState == GameState.ORD && GeneralState != GameState.PAUSE)
        {
            FreezeTime(true);
            CameraIsFreezed = true;
            GeneralState = GameState.PAUSE;
            return true;
        }
        return false;
    }

    public static bool SetMapEditingMode()
    {
        if (GeneralState == GameState.ORD && GeneralState != GameState.MAPEDIT)
        {
            //FreezeTime(true);
            GeneralState = GameState.MAPEDIT;
            return true;
        }
        return false;
    }

    public static bool SetChangeTurnMode() 
    {
        if (GeneralState == GameState.ORD && GeneralState != GameState.CHANGETURN)
        {
            GeneralState = GameState.CHANGETURN;
            return true;
        }
        return false;
    }

    public static void SetFightMode()
    {
        GeneralState = GameState.FIGHT;
    }

    static void FreezeTime(bool state)
    {
        if (state)
        {
            oldTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            TimeIsFreezed = true;
        }
        else
        {
            if (TimeIsFreezed)
            {
                Time.timeScale = oldTimeScale;
                TimeIsFreezed = false;
            }
        }
    }
}

public enum GameState
{
    ORD,
    BUILD,
    PAUSE,
    MAPEDIT,
    CHANGETURN,
    FIGHT
}

public enum TimeScaleIndex
{
    FREEZED,
    SCALE1,
    SCALE3,
    SCALE6
}
