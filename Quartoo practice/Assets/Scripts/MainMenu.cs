using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject helpPanel;
    public GameObject settingsPanel;
    public GameObject darkBackground;

    //These are for the scene transition
    public Image greyFade;
    public Animator fadeAnimation;

    public void Start()
    {
        //StartCoroutine(FadeIn());
    }

    public void multiplayerGame()
    {
        GameInfo.gameType = 'N';

        Initiate.Fade("GameLobby", Color.black, 0.75f);
        //StartCoroutine(LoadSceneAsync("GameLobby"));
    }

    public void storyModeGame()
    {
        // Human Player will always go first
        GameInfo.selectPieceAtStart = 1;

        GameInfo.gameType = 'S';
        GameInfo.storyModeType = 'E';

        Initiate.Fade("StoryMode", Color.black, 0.75f);
        //StartCoroutine(LoadSceneAsync("StoryMode"));
    }

    public void quickGame()
    {
        // Set to Easy so correct player prefs screen shows (ask Tristan if you dont understand) 
        GameInfo.gameType = 'E';

        GameInfo.storyModeType = 'T';

        Initiate.Fade("UserPreferences", Color.black, 0.5f);
        //StartCoroutine(LoadSceneAsync("UserPreferences"));
    }

    public void tutorial()
    {
        GameInfo.gameType = 'T';

        Initiate.Fade("GameScene", Color.black, 0.5f);
        //StartCoroutine(LoadSceneAsync("GameScene"));
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

    //These functions are for the fade transitions between scenes
    //private IEnumerator LoadSceneAsync(string sceneName)
    //{
    //    yield return FadeOut();
    //    SceneManager.LoadScene(sceneName);
    //}

    //// Reference: https://youtu.be/iV-igTT5yE4
    //IEnumerator FadeIn()
    //{
    //    fadeAnimation.SetBool("Fade", false);
    //    yield return new WaitUntil(() => greyFade.color.a == 0);
    //}

    //IEnumerator FadeOut()
    //{
    //    fadeAnimation.SetBool("Fade", true);
    //    yield return new WaitUntil(() => greyFade.color.a == 1);
    //}
}
