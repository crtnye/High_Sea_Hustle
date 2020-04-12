using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=a1RFxtuTVsk
public class TutorialManager : MonoBehaviour
{
    public GameObject[] popups;
    private int popupIndex = 0;

    private string[] captions =
    {
        /*0 -*/ "Hello! I'm Peter Parrot and I'll be showing you how to play Quarto. Click the arrow on the bottom right to continue.",
        /*1 -*/ "The objective of this game is to create a line of four pieces which share at least one common characteristic.",
        /*2 -*/ "These characteristics are color, height, shape, and stamp. The line can be vertical, horizontal, or diagonal.",
        /*3 HS*/ "The game begins with the first player selecting a piece for their opponent to place. Click the hilighted piece to select it, then click it again to confirm that's the piece you want.",
        /*4 CP*/ "Great! Now, each turn will have two parts: placing a piece and selecting your opponents piece. I will start my turn by first placing the piece you gave me.",
        /*5 CS*/ "Now that I've placed the piece you gave to me, I will give you a piece to place.",
        /*6 HP*/ "Here you go! Now, place the piece on the highlighted board space.",
        /*7 -*/ "Good Job! Normally you would now select a piece to give to me as you did before, but instead let's jump ahead a few turns.",
        /*8 -*/ "If you look at the board now, there is a potential win for a piece that is either tall or triangular.",
        /*9 HS*/ "It's your turn to select a piece for me, but you want to make sure not to give me a piece that will cause me to win. Click the highlighted piece.",
        /*10 CP*/ "I've blocked the winning move, but in doing so I have made a new one for a piece that has a stamp. Now I will select a piece for you to place.",
        /*11 CS HP*/ "You have just been given a winning piece! Place it in the hilighted board space to win by matching four pieces with a stamp.",
        /*12*/ "Congratulations, you've just completed your first game of Quarto! Click on the arrow icon at the top of the screen to exit to the Main Menu. Goodbye!"
    };

    public int GetPopupIndex()
    {
        return popupIndex;
    }

    public string ShowNextStep()
    {
        popupIndex++;
        return getCurrentCaption();
    }

    public string getCurrentCaption()
    {
        return captions[popupIndex];
    }
}