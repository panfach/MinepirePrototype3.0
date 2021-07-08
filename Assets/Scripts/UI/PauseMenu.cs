using Boo.Lang;
using UnityEngine;
using UnityEngine.SceneManagement;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
/// <summary>
/// Этот скрипт должен висеть на PauseMenu, который содержит pausemenuwindow и все разделы меню паузы.
/// Отвечает почти за все действия, которые происходят в меню паузы.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    static bool gameIsPaused = false;
    public static int activeLoadfileIndex = -1;
    static string saveFileName = "";
    public static List<string> loadFilenames = new List<string>();
    static PauseMenuSection activeSection = PauseMenuSection.None;

    [Header("Managed object")]
    public GameObject pauseMenuWindow;

    [Header("Sections")]
    public GameObject[] section;

    private void Start()
    {
        //Open(PauseMenuSection.Load);


        //SaveLoader.mapIsLoaded = true;
    }

    public bool GameIsPaused
    {
        get
        {
            return gameIsPaused;
        }
        set
        {
            if (value)
            {
                if (StateManager.SetPauseMode())
                {
                    pauseMenuWindow.SetActive(true);
                    gameIsPaused = true;
                }
            }
            else if (StateManager.GeneralState == GameState.PAUSE)
            {
                StateManager.SetOrdinaryMode();
                pauseMenuWindow.SetActive(false);
                gameIsPaused = false;
            }
        }
    }

    private void Awake()
    {
        //if (pauseMenuWindow.activeSelf)
        //{
        //    CloseAllSections();
        //    pauseMenuWindow.SetActive(false);
        //}

        Open(PauseMenuSection.Load);
    }

    public void Open(PauseMenuSection sect)
    {
        Debug.Log("SaveLoadState : " + SaveLoader.state);
        if (SaveLoader.state == SaveLoadState.START && sect != PauseMenuSection.Load) return;

        if (sect != activeSection)
        {
            GameIsPaused = true;
            CloseAllSections();
            activeSection = sect;
            section[(int)sect].SetActive(true);
        }
        else
        {
            CloseAllSections();
            activeSection = PauseMenuSection.None;
            GameIsPaused = false;
        }
    }

    public void Open(int sect)
    {
        Open((PauseMenuSection)sect);
    }

    void CloseAllSections()
    {
        foreach (GameObject item in section) item.SetActive(false);
    }

    public void SetSaveFileName(string s)
    {
        saveFileName = s;
    }

    public void Save()
    {
        Connector.saveLoader.Save(saveFileName);
        Open(PauseMenuSection.Main);
        saveFileName = "";
    }

    public void Load()
    {
        if (activeLoadfileIndex != -1)
        {
            Connector.saveLoader.Load(loadFilenames[activeLoadfileIndex]);
            Open(activeSection);
        }
        else
            Debug.Log("Choose the file to load");
    }

    public void LoadNewGame()
    {
        //Connector.saveLoader.Load("Assets/Saves/Gold_Test_1.map");
        Connector.saveLoader.LoadNewGame();
        Open(activeSection);
    }

    public void Quit()
    {
        Application.Quit();
    }

    /*
    public void Quit() {
        GameIsPaused = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }*/
}

public enum PauseMenuSection
{
    Main, Save, Load, Settings, None
}

/*public static class PauseMenuSectionExtensions
{
    public static implicit operator PauseMenuSection(this int x)
    {
        return (PauseMenuSection)x;
    }
}*/
