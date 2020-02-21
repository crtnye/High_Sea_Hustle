using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour
{
    public Sprite[] images;
    public Image currentImage;
    public Button nextImage;
    public Button previousImage;
    public int i = 0;

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
    }
}
