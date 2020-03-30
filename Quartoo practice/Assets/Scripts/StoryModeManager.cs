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
        "",
        "Looks like I've bested you sir!"

    };

    string[] sailorDialogueLines =
    {
        "I've boarded your ship on behalf of my Captain. That treasure there is ours, it was buried on Royally owned land and we intend to claim it",
        "How's that?",
        "Sounds fair enough to me, let's play.",
        "",
        "Not so fast, I demand a rematch. I'm going to call my Captain to play you a round. She's quite the competitor."
    };


    int i = 0; //which image/scene are we on?

    // Start is called before the first frame update
    void Start()
    {
        //If this is the first story mode scene
        if(i == 0)
        {
            currentPirateDialogue.text = pirateDialogueLines[i];
            isPiratesLine = false;
        }
        else
        {
            i = 4;
        }
    }

    //Used in the initial scene
    public void nextButtonPressed()
    {
        if (i < pirateDialogueLines.Length) {
            if (i == 3)
            {
                //Load the game against the novice AI
                SceneManager.LoadScene("GameScene");
            }
            else if (isPiratesLine) //if its the pirate's turn to talk
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
            SceneManager.LoadScene("GameScene");
        }
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
