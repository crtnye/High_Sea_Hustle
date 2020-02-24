﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public GameObject helpPanel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showHelpPanel()
    {
        helpPanel.SetActive(true);
        Debug.Log("make panel visible");
    }

    public void hideHelpPanel()
    {
        //Panel helpPanel = GameObject.Find("helpPanel").GetComponent<Panel>();
        helpPanel.SetActive(false);
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
