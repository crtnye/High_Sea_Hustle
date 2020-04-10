using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatarBarController : MonoBehaviour
{
    public Image avatarImage;
    public Text usernametext;

    public Sprite[] avatarImageOptions;


    // Start is called before the first frame update
    void Start()
    {
        //If playing story mode, you're always the pirate
        if (GameInfo.gameType == 'S')
        {
            avatarImage.sprite = avatarImageOptions[0];
            usernametext.text = "Pirate Captain";
        }
        else if (GameInfo.gameType == 'T') //Tutorial
        {
            avatarImage.sprite = avatarImageOptions[0];
            usernametext.text = "New Player";
        }
        else
        {
            usernametext.text = GameInfo.username;
            ShowAvatar();
        }
    }

    private void ShowAvatar()
    {
        if (GameInfo.avatar == "PirateCaptain")
        {
            avatarImage.sprite = avatarImageOptions[0];
        }
        else if (GameInfo.avatar == "PirateSailor")
        {
            avatarImage.sprite = avatarImageOptions[1];
        }
        else if (GameInfo.avatar == "NavyCaptain")
        {
            avatarImage.sprite = avatarImageOptions[2];
        }
        else if (GameInfo.avatar == "NavySailor")
        {
            avatarImage.sprite = avatarImageOptions[3];
        }
        else
        {
            Debug.Log("Player avatar name was invalid");
            return;
        }
    }
}
