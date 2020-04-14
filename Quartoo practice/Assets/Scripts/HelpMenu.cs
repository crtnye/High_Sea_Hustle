using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour
{
    public Sprite[] images;
    public Image currentImage;
    public Text currentText;
    public Button nextImage;
    public Button previousImage;
    public int i;

    private string[] text =
        {
            "High Sea Hustle is an adaptation of the modern abstract strategy game Quarto. The game consists of 16 pieces and a board with 16 locations. " +
            "The pieces have 4 characteristics: gold or silver, tall or short, round or angular, and plain or embossed.",
            "Players take turns giving their opponent a piece to place, then placing the piece given to them. Double click on a piece to give it to your opponent.",
            "Your opponent will place the piece on the board, then give you a piece to place. If its your first game, a parrot will occasionally appear to give you tips and tricks.",
            "The game continues in this pattern, placing the piece you've been given and then selecting a piece to give to your opponent. " +
            "The object of the game is to be the first person to establish a line of 4 pieces which share a common characteristic.",
            "The line can be formed in any direction: vertical, horizontal, or diagonal. The challenge is keeping track of all the possible winning conditions on the board. Good luck!"
        };

    void Start()
    {
        i = 0;

        previousImage.interactable = false;
        nextImage.interactable = true;
        updatePanel();
    }

    public void next()
    {
        previousImage.interactable = true;

        if (i < images.Length - 1)
        {
            i++;

            if (i == images.Length - 1)
                nextImage.interactable = false;

            updatePanel();
        }
        else
        {
            Debug.Log("currently viewing the last image");
        }
    }

    public void previous()
    {
        nextImage.interactable = true;

        if (i - 1 >= 0)
        {
            i--;

            if (i == 0)
                previousImage.interactable = false;

            updatePanel();
        }
    }

    private void updatePanel()
    {
        if (images.Length > 0)
        {
            currentImage.sprite = images[i];
            currentText.text = text[i];
        }

    }
}
