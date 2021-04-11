using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// -------------------- // MINEPIRE demo // -------------------- //
public class MainMenu : MonoBehaviour
{
    /*public GameObject mainMenu, options, loadingScreen, tooltip;
    public Slider loadSlider;
    public TextMeshProUGUI tooltipText;

    private void Start()
    {
        SmallTooltipScript.tooltip = tooltip;
        SmallTooltipScript.textObject = tooltipText;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsynchronously());
    }

    public void SetOptionsMenu() 
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
    }

    public void SetMainMenu() 
    {
        options.SetActive(false);
        mainMenu.SetActive(true);
    }

    IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadSlider.value = progress;
            yield return null;
        }
    }*/
}