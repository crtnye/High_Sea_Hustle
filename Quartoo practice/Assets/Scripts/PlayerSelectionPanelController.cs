using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectionPanelController : MonoBehaviour
{

    public InputField usernameInput;


    private string username = "Player1";
    private bool userGoesFirst = true;
    private bool easyAI = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playButton()
    {
        //Not sure if this is the right way, but need some kind of check to provide a default name
        if (usernameInput.text != "")
            username = usernameInput.text;

        //Save the username and other relevant data somewhere

        SceneManager.LoadScene("GameScene");
    }

    public void firstMoveToggled()
    {
        userGoesFirst = !userGoesFirst;
    }

    public void AIDifficultyToggled()
    {
        easyAI = !easyAI;
    }

}
