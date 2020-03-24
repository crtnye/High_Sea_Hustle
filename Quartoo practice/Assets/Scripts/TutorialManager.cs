using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=a1RFxtuTVsk
public class TutorialManager : MonoBehaviour
{
    public GameObject[] popups;
    private int popupIndex = 0;

    //these are temporary:
    public Text caption;
    private string[] captions = 
    {
        "Hello, I'm Peter Parrot and I'll be showing you how to play.",
        "The objective of this game is to create a line of four pieces which share at least one common characteristic.",
        "These characteristics are color, height, shape, and stamp. The line can be vertical, horizontal, or diagonal.",
        "The game begins with the first player sending a move to their opponent. Click the hilighted piece and then the" + 
            "check to confirm that's the piece you'd like to send.",
        "Your opponent will place the piece on the board, then send you a piece to place. Turns continue in this way for the rest of the game.",
        "Let's jump ahead a few turns.",
        "If you look at the board now, there is a place for a potential win if a piece is _____ or _____. Make sure you don't give your" +
           "opponent a piece with these characteristics!",
        "Your opponent has given you a winning piece! Place it in the hilighted board space to win by ______ (characteristic).",
        "You've just completed your first game of Quarto!"
    };

    public void StepCompleted()
    {
        popupIndex++;
        ShowNextStep();
    }

    private void ShowNextStep()
    {
        //
        caption.text = captions[popupIndex];
        //

        for (int i = 0; i < popups.Length; i++)
        {
            if (i == popupIndex)
            {
                popups[i].SetActive(true);
            }
            else
            {
                popups[i].SetActive(false);
            }
        }
    }

    void Start()
    {
        //Show the initial step

        //
        caption.text = captions[popupIndex];
        //
    }


    //Hello, I'm Peter Parrot and I'll be showing you how to play.
    //The objective of this game is to create a line of four pieces which share at least one common characteristic.
    //These characteristics are color, height, shape, and stamp. The line can be vertical, horizontal, or diagonal.
    //***user input begins***
    //The game begins with the first player sending a move to their opponent. Click the hilighted piece and then the check to
    //   confirm that's the piece you'd like to send.
    //Your opponent will place the piece on the board, then send you a piece to place. Turns continue in this way for the rest of the game.
    //Let's jump ahead a few turns.
    //If you look at the board now, there is a place for a potential win if a piece is _____ or _____. Make sure you don't give your
    //   opponent a piece with these characteristics!
    //Your opponent has given you a winning piece! Place it in the hilighted board space to win by ______ (characteristic).
    //You've just completed your first game of Quarto! 
}
