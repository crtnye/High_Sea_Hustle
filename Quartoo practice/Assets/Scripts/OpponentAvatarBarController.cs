﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpponentAvatarBarController : MonoBehaviour
{
    public GameObject opponentAvatarBar;
    public Image avatarImage;
    public Text usernametext;
    private static NetworkController networkController = new NetworkController();

    public Sprite[] avatarImageOptions;

    //THESE NEED TO BE SET DYNAMICALLY
    private string opponentsAvatar;
    private string opponentsUsername;
    void Start()
    {
        //If its a Tutorial
        if (GameInfo.gameType == 'T')
            opponentAvatarBar.SetActive(false); //There is no opponent, hide the avatar bar
        else
        {
            opponentAvatarBar.SetActive(true); //Make sure the avatar bar is visible

            //If its a Quick game hard
            if (GameInfo.gameType == 'H')
            {
                //Make the opponent the available captain avatar
                if (GameInfo.avatar == "NavyCaptain")
                {
                    //Make opponent pirate captain
                    avatarImage.sprite = avatarImageOptions[0];
                    usernametext.text = "Pirate Captain";
                }
                else
                {
                    //Default to navy captain
                    avatarImage.sprite = avatarImageOptions[2];
                    usernametext.text = "Navy Captain";
                }
            }
            else if (GameInfo.gameType == 'E') //Quick game easy
            {
                if (GameInfo.avatar == "NavySailor")
                {
                    //make opponent pirate sailor
                    avatarImage.sprite = avatarImageOptions[1];
                    usernametext.text = "Pirate Sailor";
                }
                else
                {
                    //default to navy sailor
                    avatarImage.sprite = avatarImageOptions[3];
                    usernametext.text = "Navy Sailor";
                }
            }
            else if (GameInfo.gameType == 'N') //Networked game
            {
                //Access the networked opponent's avatar and username
                Debug.Log("networkController: " + networkController);
                networkController.SendPlayerInfo(opponentsAvatar, opponentsUsername);

                usernametext.text = opponentsUsername;
                ShowAvatar();

                Debug.Log("Avatar: " + opponentsAvatar);
                Debug.Log("Name: " + opponentsUsername);
            }
            else if (GameInfo.gameType == 'S') //Story Mode
            {
                if (GameInfo.storyModeType == 'E')
                {
                    opponentsAvatar = "NavySailor";
                    usernametext.text = "Navy Sailor";
                }
                else if (GameInfo.storyModeType == 'H')
                {
                    opponentsAvatar = "NavyCaptain";
                    usernametext.text = "Navy Captain";
                }
                else
                {
                    return;
                }
                ShowAvatar();
            }
            else
            {
                //Give the opponent a default avatar and username 
                opponentsAvatar = "PirateCaptain";
                usernametext.text = "Opponent Player";
                ShowAvatar();
            }
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
