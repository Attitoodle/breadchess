using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utils
{
    public static void NewGame()
    {
        DataPersistenceManager.instance.NewGame();
    }
    public static void LoadGame()
    {
        DataPersistenceManager.instance.LoadGame();
    }
    public static void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public static void ReloadCurrentScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public static void LoadScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }
    public static void LoadScene(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
    public static void QuitGame()
    {
        Application.Quit();
    }
}