using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerSelectionPanelController : MonoBehaviour
{
    public EventSystem eventSystem;
    public InputField usernameInput;
    public Button[] avatarOptions;
    public GameObject[] avatarHighlights;

    private string username = "Player1";
    private bool userGoesFirst = true;
    private bool easyAI = true;
    private string selectedAvatar = "PirateCaptain";

    public void playButton()
    {
        //Not sure if this is the right way, but need some kind of check to provide a default name
        if (usernameInput.text != "")
            username = usernameInput.text;

        //Save the username and other relevant data somewhere

        SceneManager.LoadScene("GameScene");
    }

    public void backButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void firstMoveToggled()
    {
        userGoesFirst = !userGoesFirst;
    }

    public void AIDifficultyToggled()
    {
        easyAI = !easyAI;
    }

    public void avatarButton()
    {
        Debug.Log("The name of the button clicked is " + eventSystem.currentSelectedGameObject.name);

        selectedAvatar = eventSystem.currentSelectedGameObject.name;

        activateHighlight(eventSystem.currentSelectedGameObject.name);
    }

    private void activateHighlight(string avatarName)
    {
        if (avatarName == "PirateCaptain")
            showHilight(0);
        else if (avatarName == "PirateSailor")
            showHilight(1);
        else if (avatarName == "NavyCaptain")
            showHilight(2);
        else if (avatarName == "NavySailor")
            showHilight(3);
        else
            return;
    }

    private void showHilight(int index)
    {
        for (int i = 0; i < avatarHighlights.Length; i++)
        {
            if (i == index)
            {
                avatarHighlights[i].SetActive(true);
            }
            else
            {
                avatarHighlights[i].SetActive(false);
            }
        }
    }

}
