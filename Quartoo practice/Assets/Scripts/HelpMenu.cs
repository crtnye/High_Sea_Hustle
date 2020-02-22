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
    public int i = 0;

    private string[] text =
        {
            "High Sea Hustle is an adaptation of the modern abstract strategy game Quarto. The game consists of 16 pieces and a board with 16 locations. " +
            "The pieces have 4 characteristics: gold or silver, tall or short, round or angular, and plain or embossed.",
            "Players take turns giving their opponent a piece to place, then placing the piece given to them.",
            "The object of the game is to be the first person to establish a line of 4 pieces which share a common characteristic. " +
            "The line can be vertical, horizontal, or diagonal.",
        };

    void Start()
    {
        updatePanel();
    }

    public void next()
    {
        if (i + 1 < images.Length)
        {
            i++;
            updatePanel();
        }

    }

    public void previous()
    {
        if (i - 1 >= 0)
        {
            i--;
            updatePanel();
        }
    }

    private void updatePanel()
    {
        currentImage.sprite = images[i];
        currentText.text = text[i];
    }
}
