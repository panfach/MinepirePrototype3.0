using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.IO;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
/// <summary>
/// Этот скрипт должен висеть на content of scroll view.
/// Отвечает за запросы на получение всех имен файлов сохранения, отображение этих файлов в scroll view в виде кнопок,
/// отображение этих файлов в scroll view в виде кнопок и за реакцию нажатия на эти кнопки
/// </summary>
public class PauseMenuLoad : MonoBehaviour
{
    public GameObject loadPrefab;

    int i;

    private void OnEnable()
    {
        Refresh();
    }

    void Refresh()
    {
        i = 0;
        GameObject loadFile;

        ClearAll();
        PauseMenu.loadFilenames.Clear();

        foreach (string item in SaveLoader.GetLoadFilenames())
        {
            if (Path.GetExtension(item) != ".map") continue;

            PauseMenu.loadFilenames.Add(item);
            loadFile = Instantiate(loadPrefab, transform);

            loadFile.name = i.ToString();
            loadFile.GetComponent<Button>().onClick.AddListener(
                () => OnClickReaction(EventSystem.current.currentSelectedGameObject.name) 
            );
            string _item = item.Substring(Application.persistentDataPath.Length + 1);
            loadFile.GetComponentInChildren<Text>().text = _item.Substring(0, _item.Length - 4);
            i++;

            PauseMenu.activeLoadfileIndex = -1;
        }
    }

    void ClearAll()
    {
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
        }
    }

    void OnClickReaction(string name)
    {
        PauseMenu.activeLoadfileIndex = Convert.ToInt32(name);
    }
}
