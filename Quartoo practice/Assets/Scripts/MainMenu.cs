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
        StartCoroutine(FadeIn());
    }

    public void multiplayerGame()
    {
        GameInfo.gameType = 'N';

        StartCoroutine(LoadSceneAsync("GameLobby"));
    }

    public void storyModeGame()
    {
        // Human Player will always go first
        GameInfo.selectPieceAtStart = 1;

        GameInfo.gameType = 'S';
        GameInfo.storyModeType = 'E';

        StartCoroutine(LoadSceneAsync("StoryMode"));
    }

    public void quickGame()
    {
        // NOTE: Sets every quickplay to a hard game. Change this to be either easy or hard
        // whenever that functionality is set in unity
        GameInfo.gameType = 'H';

        GameInfo.storyModeType = 'T';

        StartCoroutine(LoadSceneAsync("UserPreferences"));
    }

    public void tutorial()
    {
        GameInfo.gameType = 'T';

        StartCoroutine(LoadSceneAsync("GameScene"));
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
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return FadeOut();
        SceneManager.LoadScene(sceneName);
    }

    // Reference: https://youtu.be/iV-igTT5yE4
    IEnumerator FadeIn()
    {
        fadeAnimation.SetBool("Fade", false);
        yield return new WaitUntil(() => greyFade.color.a == 0);
    }

    IEnumerator FadeOut()
    {
        fadeAnimation.SetBool("Fade", true);
        yield return new WaitUntil(() => greyFade.color.a == 1);
    }
}
