using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashControl : MonoBehaviour
{
    public Image gameLogo;
    public Image teamLogo;
    public Image blackBackground;
    public AudioSource Waves;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(myStart());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Initiate.Fade("MainMenu", Color.black, 4.0f);
        }
    }

    IEnumerator myStart()
    {
        gameLogo.canvasRenderer.SetAlpha(0);
        teamLogo.canvasRenderer.SetAlpha(0);
        teamLogoFadeIn();
        yield return new WaitForSeconds(2);
        teamLogoFadeOut();
        yield return new WaitForSeconds(1);
        blackBackgroundFadeOut();
        yield return new WaitForSeconds(2);
        gameLogoFadeIn();
        yield return new WaitForSeconds(6);
        gameLogoFadeOut();
        yield return new WaitForSeconds(2);

        DontDestroyOnLoad(Waves);
        Initiate.Fade("MainMenu", Color.black, 4.0f);
    }

    void gameLogoFadeIn()
    {
        gameLogo.CrossFadeAlpha(1,2, false);
    }

    void gameLogoFadeOut()
    {
        gameLogo.CrossFadeAlpha(0,2, false);
    }

    void teamLogoFadeIn()
    {
        teamLogo.CrossFadeAlpha(1,2, false);
    }

    void teamLogoFadeOut()
    {
        teamLogo.CrossFadeAlpha(0,1, false);
    }

    void blackBackgroundFadeOut()
    {
        blackBackground.CrossFadeAlpha(0, 2, false);
    }
}
