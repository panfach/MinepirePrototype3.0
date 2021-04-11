using UnityEngine;

/// <summary>
/// Этот скрипт должен находится на родительском объекте всех canvas.
/// </summary>
public class PanelInvoker : MonoBehaviour
{
    [Header("UI elements")]
    public GameObject mapEditor;
    public BuildingInfo buildingInfo;
    public BuildPanelInfo buildPanelInfo;
    public ResourceSourceInfo resourceSourceInfo;
    public VillagerInfo villagerInfo;
    public PauseMenu pauseMenu;
    public GameObject buildingRotationTooltip;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Escape. BuildingInfo = " + StateManager.BuildingInfo + ", VillagerInfo = " + StateManager.VillagerInfo + "ResourceSourceInfo = " + StateManager.ResourceSourceInfo);
                if (StateManager.BuildingInfo || StateManager.VillagerInfo || StateManager.ResourceSourceInfo)
                {
                    CloseBuildingInfo();
                    CloseVillagerInfo();
                    CloseNatureInfo();
                }
                else
                {
                    pauseMenu.Open(PauseMenuSection.Main);
                }
            }
        }
    }

    public void OpenMapEditor()
    {
        bool state = mapEditor.activeSelf;
        if (!state)
        {
            mapEditor.SetActive(true);
            StateManager.SetMapEditingMode();
            StateManager.GodMode = true;
            //return true;
        }
        else
        {
            mapEditor.SetActive(false);
            StateManager.SetOrdinaryMode();
            StateManager.GodMode = false;
            //return false;
        }
    }

    public void BuildModeOn(int index)
    {
        if (StateManager.GeneralState == GameState.ORD)
            Connector.generalBuilder.BuildModeActivate((BuildingIndex)index);
    }

    public void BuildModeOn(BuildingIndex index)
    {
        Connector.generalBuilder.BuildModeActivate(index);
    }

    public void NextTurn()
    {
        if (Sunlight.theEndOfDay)
        {
            Connector.turnManager.TurnProcess();
        }
        else
        {
            // Вызвать ошибку
        }
    }

    // General function, that determines, what info should open
    public void OpenInfo(Entity entity)
    {
        if (entity is Building)
            OpenBuildingInfo(entity as Building);
        else if (entity is Nature)
            OpenNatureInfo(entity as Nature);
        // ...
    }

    // General function, that determines, what info should close
    public void CloseInfo(Entity entity)
    {
        if (entity is Building && BuildingInfo.activeBuilding == entity as Building)
            CloseBuildingInfo(entity as Building);
        if (entity is Nature && ResourceSourceInfo.activeNature == entity as Nature)
            CloseNatureInfo(entity as Nature);
        // ...
    }

    // General function, that updates information
    public void RefreshInfo(Entity entity)
    {
        if (entity is Building && BuildingInfo.activeBuilding == entity as Building)
            RefreshBuildingInfo();
        else if (entity is Nature && ResourceSourceInfo.activeNature == entity as Nature)
            RefreshNatureInfo();
        // ....
    }

    public void OpenBuildingInfo(Building building)
    {
        Connector.effectSoundManager.PlaySlideSound();

        BuildingInfo.activeBuilding = building;

        resourceSourceInfo.Open(false);
        buildingInfo.Open(true);
    }

    public void CloseBuildingInfo(Building building)
    {
        if (BuildingInfo.activeBuilding == building) CloseBuildingInfo();
    }

    public void CloseBuildingInfo()
    {
        Connector.effectSoundManager.PlaySlideSound();
        buildingInfo.Open(false);
    }

    public void OpenNatureInfo(Nature nature)
    {
        Connector.effectSoundManager.PlaySlideSound();

        ResourceSourceInfo.activeNature = nature;

        buildingInfo.Open(false);
        resourceSourceInfo.Open(true);
    }

    public void CloseNatureInfo(Nature nature)
    {
        if (ResourceSourceInfo.activeNature == nature) CloseNatureInfo();
    }

    public void CloseNatureInfo()
    {
        Connector.effectSoundManager.PlaySlideSound();    
        resourceSourceInfo.Open(false);                                                               // Change name. Bad name of function "Open", though we are closing
    }

    public void OpenVillagerInfo(Villager villager)
    {
        Connector.effectSoundManager.PlaySlideSound();

        VillagerInfo.activeVillager = villager;
        villagerInfo.Open(true);
    }

    public void CloseVillagerInfo()
    {
        Connector.effectSoundManager.PlaySlideSound();

        villagerInfo.Open(false);
    }

    public void RefreshBuildingInfo()
    {
        buildingInfo.Refresh();
    }

    public void RefreshNatureInfo()
    {
        resourceSourceInfo.Refresh();                                          // Change name
    }

    public void RefreshVillagerInfo()
    {
        villagerInfo.Refresh();
    }

    public void SetTimeScale(int speed)
    {
        Time.timeScale = speed;
    } 

    public void OpenBuildPanelInfo(int index)
    {
        buildPanelInfo.gameObject.SetActive(true);
        buildPanelInfo.Refresh((BuildingIndex)index);
    }

    public void CloseBuildPanelInfo()
    {
        buildPanelInfo.gameObject.SetActive(false);
    }
}
