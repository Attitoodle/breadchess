using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button loadButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resignButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button quitButton;
    public GameObject pauseMenu;
    public static bool isPaused = false;
    private Game game;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void OnSceneUnloaded(Scene current)
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                saveButton.interactable = !game.IsPristine();
                loadButton.interactable = DataPersistenceManager.instance.SaveExists();
                PauseGame();
            }
        }
    }

    public void GoToMainMenu()
    {
        DisableAllButtons();
        Utils.LoadScene("Scenes/MainMenu");
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Resign()
    {
        resignButton.interactable = false;
        saveButton.interactable = false;
        Game game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        game.Winner(game.GetCurrentPlayer() == "white" ? "black" : "white");
        ResumeGame();
    }

    public void SaveGame()
    {
        saveButton.interactable = false;
        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadGame()
    {
        DisableAllButtons();
        Utils.LoadGame();
        Utils.ReloadCurrentScene();
    }

    public void QuitGame()
    {
        DisableAllButtons();
        Utils.QuitGame();
    }
    private void DisableAllButtons()
    {
        loadButton.interactable = false;
        mainMenuButton.interactable = false;
        resignButton.interactable = false;
        resumeButton.interactable = false;
        saveButton.interactable = false;
        quitButton.interactable = false;
    }
}
