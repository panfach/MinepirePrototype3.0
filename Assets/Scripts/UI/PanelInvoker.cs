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
    public NatureInfo natureInfo;
    public CreatureInfo creatureInfo;
    public ResearchInfo researchInfo;
    public PauseMenu pauseMenu;
    public GameObject buildingRotationTooltip;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Debug.Log("Escape. BuildingInfo = " + StateManager.BuildingInfo + ", VillagerInfo = " + StateManager.VillagerInfo + "ResourceSourceInfo = " + StateManager.ResourceSourceInfo);
                if (StateManager.BuildingInfo || StateManager.CreatureInfo || StateManager.ResourceSourceInfo)
                {
                    CloseBuildingInfo();
                    CloseCreatureInfo();
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
        else if (entity is Creature)
            OpenCreatureInfo(entity as Creature);
        // ...
    }

    // General function, that determines, what info should close
    public void CloseInfo(Entity entity)
    {
        if (entity is Building && BuildingInfo.activeBuilding == entity as Building)
            CloseBuildingInfo(entity as Building);
        else if (entity is Nature && NatureInfo.activeNature == entity as Nature)
            CloseNatureInfo(entity as Nature);
        else if (entity is Creature)
            CloseCreatureInfo(entity as Creature);
        // ...
    }

    // General function, that updates information
    public void RefreshInfo(Entity entity)
    {
        if (entity is Building && BuildingInfo.activeBuilding == entity as Building)
            RefreshBuildingInfo();
        else if (entity is Nature && NatureInfo.activeNature == entity as Nature)
            RefreshNatureInfo();
        else if (entity is Creature)
            RefreshCreatureInfo();
        // ....
    }

    public void OpenBuildingInfo(Building building)
    {
        Connector.effectSoundManager.PlaySlideSound();

        BuildingInfo.activeBuilding = building;

        natureInfo.Set(false);
        buildingInfo.Set(true);
    }

    public void CloseBuildingInfo(Building building)
    {
        if (BuildingInfo.activeBuilding == building) CloseBuildingInfo();
    }

    public void CloseBuildingInfo()
    {
        Connector.effectSoundManager.PlaySlideSound();
        buildingInfo.Set(false);
    }

    public void OpenNatureInfo(Nature nature)
    {
        Connector.effectSoundManager.PlaySlideSound();

        NatureInfo.activeNature = nature;

        buildingInfo.Set(false);
        natureInfo.Set(true);
    }

    public void CloseNatureInfo(Nature nature)
    {
        if (NatureInfo.activeNature == nature) CloseNatureInfo();
    }

    public void CloseNatureInfo()
    {
        Connector.effectSoundManager.PlaySlideSound();    
        natureInfo.Set(false);                                                               // Change name. Bad name of function "Open", though we are closing
    }

    public void OpenCreatureInfo(Creature creature)
    {
        Connector.effectSoundManager.PlaySlideSound();

        CreatureInfo.activeCreature = creature;
        creatureInfo.Set(true);
    }

    public void CloseCreatureInfo(Creature creature)
    {
        if (CreatureInfo.activeCreature == creature) CloseCreatureInfo();
    }

    public void CloseCreatureInfo()
    {
        Connector.effectSoundManager.PlaySlideSound();
        creatureInfo.Set(false);
    }

    public void RefreshBuildingInfo()
    {
        buildingInfo.Refresh();
    }

    public void RefreshNatureInfo()
    {
        natureInfo.Refresh();                                          // Change name
    }

    public void RefreshCreatureInfo()
    {
        creatureInfo.Refresh();
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

    public void OpenResearchWindow()
    {
        researchInfo.Set(true);
    }

    public void CloseResearchWindow()
    {
        researchInfo.Set(false);
    }
}
