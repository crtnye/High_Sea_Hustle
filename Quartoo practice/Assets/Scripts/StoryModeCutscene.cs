using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryModeCutscene : MonoBehaviour
{
    public Text currentPirateDialogue;
    public Text currentSailorDialogue;

    bool isPiratesLine;
    int i = 0;

    string[] pirateDialogueLines =
    {
        "Looks like I've bested you sir!"
    };

    string[] sailorDialogueLines =
    {
        "Not so fast, I demand a rematch. Let's see how you fare against my Captain, she's quite the competitor!"
    };

    void Start()
    {
        currentPirateDialogue.text = pirateDialogueLines[i];
        isPiratesLine = false;
    }

    public void nextButtonPressed()
    {
        Debug.Log("Next button has been pressed");
        if (i < pirateDialogueLines.Length)
        {
            if (isPiratesLine) //if its the pirate's turn to talk
            {
                currentPirateDialogue.text = pirateDialogueLines[i];
                isPiratesLine = false;
            }
            else //if its the sailor's turn to talk
            {
                currentSailorDialogue.text = sailorDialogueLines[i];
                i++; //only increment every other time so both arrays are accessed at every index
                isPiratesLine = true;
            }
        }
        else
        {
            //Load the game against the advanced AI
            Initiate.Fade("GameScene", Color.black, 4.0f);
        }
    }
}
