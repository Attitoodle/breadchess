using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    public void PlayGame()
    {
        DisableAllButtons();
        Utils.LoadScene();
    }

    public void LoadGame()
    {
        DisableAllButtons();
        Utils.LoadScene();
        Utils.LoadGame();
    }
    public void QuitGame()
    {
        Utils.QuitGame();
    }

    private void DisableAllButtons()
    {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
    }
}
