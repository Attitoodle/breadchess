using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newButton;
    [SerializeField] private Button loadButton;

    public void Start()
    {
        loadButton.interactable = DataPersistenceManager.instance.SaveExists();
    }
    public void PlayGame()
    {
        DisableAllButtons();
        Utils.NewGame();
        Utils.LoadNextScene();
    }

    public void LoadGame()
    {
        DisableAllButtons();
        Utils.LoadGame();
        Utils.LoadNextScene();
    }
    public void QuitGame()
    {
        Utils.QuitGame();
    }

    private void DisableAllButtons()
    {
        newButton.interactable = false;
        loadButton.interactable = false;
    }
}
