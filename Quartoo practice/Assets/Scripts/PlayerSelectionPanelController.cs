using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerSelectionPanelController : MonoBehaviour
{
    public EventSystem eventSystem;
    public PlayerPrefsManager playerPrefsManager;
    public InputField usernameInput;
    public Button[] avatarOptions;
    public GameObject[] avatarHighlights;

    private bool userGoesFirst = true;
    private bool easyAI = true;
    private string selectedAvatar = "PirateCaptain";

    public void playButton()
    {
        if (usernameInput.text.Trim() != "" && !playerPrefsManager.isToastActive() && usernameInput.text.Trim().Length <= GameInfo.usernameLength)
        {
            GameInfo.username = usernameInput.text.Trim();

            // Save the selected avater
            GameInfo.avatar = selectedAvatar;

            // Set who goes first
            if (userGoesFirst)
                GameInfo.selectPieceAtStart = 1;
            else
                GameInfo.selectPieceAtStart = 2;

            // Set ai
            if (easyAI)
                GameInfo.gameType = 'E';
            else
                GameInfo.gameType = 'H';

            Initiate.Fade("GameScene", Color.black, 4.0f);
        }
        else if (usernameInput.text.Trim() == "")
            playerPrefsManager.ShowToast("Please provide a username", 3);
        else if (usernameInput.text.Trim().Length > GameInfo.usernameLength && !playerPrefsManager.isToastActive())
            playerPrefsManager.ShowToast("Username must be " + GameInfo.usernameLength + " letters or less", 3);
    }

    public void backButton()
    {
        Initiate.Fade("MainMenu", Color.black, 4.0f);
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
