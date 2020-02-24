using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryModeManager : MonoBehaviour
{
    public Image currentImage;
    public Text currentCaption;
    public Button nextButton;
    
    public Sprite[] storyModeScenes;
    string[] storyModeCaptions = 
    {
        "Shipmate: Captain, they’ve beat us again! Those scoundrels. We always arrive just too late...",
        "Captain: All is not yet lost sailor, prepare to board their ship immediately!"
    };
    int i = 0; //which image/scene are we on?

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void nextScreen()
    {
        if (i == 0)
        {
            Debug.Log("enabling image and caption");

            currentImage.transform.SetAsLastSibling();
            currentCaption.transform.SetAsLastSibling();
            nextButton.transform.SetAsLastSibling();

        }

        if (i < storyModeScenes.Length)
        {
            currentImage.sprite = storyModeScenes[i];
            currentCaption.text = storyModeCaptions[i];
            i++;
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
