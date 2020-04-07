using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryModeManager : MonoBehaviour
{
    public Text currentPirateDialogue;
    public Text currentSailorDialogue;

    bool isPiratesLine;

    string[] pirateDialogueLines =
    {
        "Finally! After all this time spent searching the treasure is ours! Hey, what is this navy sailor boy doing on my ship?!",
        "How do you figure that? We're the ones who tracked it down! Actually, I think I know how we can settle this...",
        "With a good honest game of Quarto! Whoever wins takes the gold.",
    };

    string[] sailorDialogueLines =
    {
        "I've boarded your ship on behalf of my Captain. That treasure there is ours, it was buried on Royally owned land and we intend to claim it",
        "How's that?",
        "Sounds fair enough to me, let's play.",
    };


    int i = 0; //which image/scene are we on?

    // Start is called before the first frame update
    void Start()
    {
        currentPirateDialogue.text = pirateDialogueLines[i];
        isPiratesLine = false;
    }

    public void nextButtonPressed()
    {
        if (i < pirateDialogueLines.Length) {
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
            //Load the game against the easy AI
            Initiate.Fade("GameScene", Color.black, 4.0f);
        }
    }

    public void returnToMainMenu()
    {
        Initiate.Fade("MainMenu", Color.black, 4.0f);
    }
}
