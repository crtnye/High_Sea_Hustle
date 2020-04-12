using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconBarController : MonoBehaviour
{
    public GameObject helpPanel;
    public GameObject settingsPanel;
    public GameObject darkenBackground;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void showSettingsPanel()
    {
        darkenBackground.SetActive(true);
        settingsPanel.SetActive(true);
    }

    public void hideSettingsPanel()
    {
        settingsPanel.SetActive(false);
        darkenBackground.SetActive(false);
    }

    public void showHelpPanel()
    {
        darkenBackground.SetActive(true);
        helpPanel.SetActive(true);
        Debug.Log("make panel visible");
    }

    public void hideHelpPanel()
    {
        helpPanel.SetActive(false);
        darkenBackground.SetActive(false);
    }

    public void closeCurrentPanel()
    {
        if (settingsPanel.activeSelf)
            hideSettingsPanel();
        else
            hideHelpPanel();
    }

    public void returnToMainMenu()
    {
        Initiate.Fade("MainMenu", Color.black, 4.0f);
    }
}
