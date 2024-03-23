using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Utils.LoadScene();
    }
    public void QuitGame()
    {
        Utils.QuitGame();
    }
}
