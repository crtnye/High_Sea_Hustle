using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void quitGameClicked()
    {
        Application.Quit();
        Debug.Log("Game is quitting.");
    }

    //There will need to be cases for the game scene which handle quitting a networked game, etc.
    //Can be checked with SceneManager.GetActiveScene();
}
