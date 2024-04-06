using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Utils.LoadScene();
    }

    public void LoadGame()
    {
        Utils.LoadScene();
        Utils.LoadGame();
    }
    public void QuitGame()
    {
        Utils.QuitGame();
    }
}
