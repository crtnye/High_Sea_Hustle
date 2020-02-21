using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenu : MonoBehaviour
{
    public GameObject helpPanel;
    public void exitButton()
    {
        helpPanel.SetActive(false);
    }
}
