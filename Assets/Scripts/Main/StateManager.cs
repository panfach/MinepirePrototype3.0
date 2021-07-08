using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
public static class StateManager
{
    /*public static bool OrdinaryMode { get; private set; }
    public static bool BuildMode { get; private set; }
    public static bool PauseMode { get; private set; }
    public static bool MapEditingMode { get; private set; }
    public static bool ChangeTurnMpde { get; private set; }
    public static bool FightMode { get; private set; }*/

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
    public static bool VillagerDragging { get; set; } = false;                                                          // ??
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

    /*public static bool OrdinaryMode
    {
        get => ordinaryMode;
        set
        {
            if (value)
            {
                buildMode = false;
                pauseMode = false;
                mapEditingMode = false;
                changeTurnMode = false;
                fightMode = false;
                if (Time.timeScale == 0f) Time.timeScale = prevTimeScale; ///////////////////////
            }
            ordinaryMode = value;
        }
    }

    public static bool BuildMode
    {
        get => buildMode;
        set
        {
            if (!mapEditingMode)
            {
                if (value)
                {
                    ordinaryMode = false;
                }
                buildMode = value;
            }
        }
    }

    public static bool PauseMode
    {
        get => pauseMode;
        set
        {
            if (value)
            {
                if (OrdinaryMode) prevTimeScale = Time.timeScale; ///////////////
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = prevTimeScale;
            }
            ordinaryMode = !value;
            pauseMode = value;
        }
    }

    public static bool MapEditingMode
    {
        get => mapEditingMode;
        set
        {
            if (!buildMode)
            {
                OrdinaryMode = !value;
                mapEditingMode = value;
            }
        }
    }

    public static bool ChangeTurnMode
    {
        get => changeTurnMode;
        set
        {
            if (!pauseMode)
            {
                if (value)
                {
                    ordinaryMode = false;
                    buildMode = false;
                }
                changeTurnMode = value;
            }
        }
    }

    public static bool BuildingInfo
    {
        get => buildingInfo;
        set
        {
            if (!pauseMode)
            {
                buildingInfo = value;
            }
        }
    }

    public static bool VillagerInfo
    {
        get => villagerInfo;
        set
        {
            if (!pauseMode)
            {
                villagerInfo = value;
            }
        }
    }

    public static bool VillagerDragging
    {
        get => villagerDragging;
        set
        {
            if (!pauseMode)
            {
                villagerDragging = value;
            }
        }
    }*/

    /*public void setBuildMode(bool state)
    {
        if (state)
        {
            buildMode = true;
            ordinaryMode = false;
            rayCaster.roadBuildMode = false;
            rayCaster.wrongPlaceCounter = 0;
            rayCaster.settlingBuildManager = false;
        }
        else
        {
            buildMode = false;
        }
    }

    public void setPauseMode(bool state)
    {
        pauseMode = state;
    }

    public void OnChangeTurnMode()
    {
        buildMode = false;
        fightMode = false;
        showInfoMode = false;
        ordinaryMode = false;
        changeTurnMode = true;
    }

    public void OnOrdinaryMode()
    {
        buildMode = false;
        pauseMode = false;
        changeTurnMode = false;
        fightMode = false;
        showInfoMode = false;
        cameraFreeze = false;
        ordinaryMode = true;
    }

    public void OnFightMode()
    {
        buildMode = false;
        changeTurnMode = false;
        showInfoMode = false;
        cameraFreeze = false;
        ordinaryMode = false;
        fightMode = true;
    }

    public void setShowInfoMode(bool state)
    {
        if (state)
        {
            showInfoMode = true;
            ordinaryMode = false;
        }
        else
        {
            showInfoMode = false;
            if (!changeTurnMode && !fightMode) ordinaryMode = true;
        }
    }

    public void setCameraFreezeMode(bool state)
    {
        cameraFreeze = state;
    }

    public void setGuideMode(bool state)
    {
        guideMode = state;
    }

    // Update is called once per frame
    void Update()
    {

    }*/
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
