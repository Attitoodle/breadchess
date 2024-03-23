using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utils
{
    public static void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public static void QuitGame()
    {
        Application.Quit();
    }
}
