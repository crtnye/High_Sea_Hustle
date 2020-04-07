using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject helpPanel;
    public GameObject settingsPanel;
    public GameObject darkBackground;

    public void multiplayerGame()
    {
        GameInfo.gameType = 'N';

        Initiate.Fade("GameLobby", Color.black, 4.0f);
    }

    public void storyModeGame()
    {
        // Human Player will always go first
        GameInfo.selectPieceAtStart = 1;

        GameInfo.gameType = 'S';
        GameInfo.storyModeType = 'E';

        Initiate.Fade("StoryMode", Color.black, 4.0f);
    }

    public void quickGame()
    {
        // Set to Easy so correct player prefs screen shows (ask Tristan if you dont understand) 
        GameInfo.gameType = 'E';

        GameInfo.storyModeType = 'T';

        Initiate.Fade("UserPreferences", Color.black, 4.0f);
    }

    public void tutorial()
    {
        GameInfo.gameType = 'T';

        Initiate.Fade("GameScene", Color.black, 4.0f);
    }
       
    public void showHelpPanel()
    {
        darkBackground.SetActive(true);
        helpPanel.SetActive(true);
    }

    public void hideHelpPanel()
    {
        helpPanel.SetActive(false);
        darkBackground.SetActive(false);
    }

    public void showSettingsPanel()
    {
        darkBackground.SetActive(true);
        settingsPanel.SetActive(true);
    }

    public void hideSettingsPanel()
    {
        settingsPanel.SetActive(false);
        darkBackground.SetActive(false);
    }

    public void closeCurrentPanel()
    {
        if (settingsPanel.activeSelf)
            hideSettingsPanel();
        else
            hideHelpPanel();
    }
}
