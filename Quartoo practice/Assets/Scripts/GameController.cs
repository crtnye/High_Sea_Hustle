﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Variables and Startup
    // Controllers
    private static NetworkController networkController = new NetworkController();
    private AIEasy easyAIController = new AIEasy();
    private AIHard hardAIController = new AIHard();
    private GameCore gameCore = new GameCore();
    private TutorialManager tutorialManager;
    private Tooltips tooltips;

    // Unity Objects
    public List<GamePiece> gamePieces;
    public Button[] buttonList;
    public GamePiece selectedPiece;
    public Button recentMove;
    public GameObject gameSceneManagerObject;
    public Text ParrotCaption;
    public Vector3 oldPosition;
    public GameObject networkChat;
    public GameObject openChat;

    public GameObject ParretPopup;
    public Text TurnMessage;
    public GameObject ButtonClickSound;

    // GameController specific variables
    private int playerTurn;
    private bool placingPiece = false;
    private int tutorialPieceIndex = 1;
    private int tutorialBoardSpaceIndex = 3;
    private Coroutine hideParrot;
    private bool isNetworkMove = true;

    void Awake()
    {
        GameInfo.isGameOver = false;
        SetGameControllerReferenceOnGamePieces();
        SetGameControllerReferenceOnNetwork();
        playerTurn = GameInfo.selectPieceAtStart;
        recentMove = buttonList[0];
    }

    void Start()
    {
        if (GameInfo.firstGame)
        {
            tooltips = ParretPopup.GetComponent<Tooltips>();
            StartCoroutine("FirstGameTooltip");
        }

        if (GameInfo.gameType == 'E' || GameInfo.gameType == 'H' || GameInfo.gameType == 'S')
            StartAIGame();
        else if (GameInfo.gameType == 'N')
        {
            if (GameInfo.selectPieceAtStart == 2)
            {
                isNetworkMove = false;
            }
            StartNetworkingGame();
        }
        else if (GameInfo.gameType == 'T')
            StartTutorialModeGame();
        else
            Debug.Log("Houston we have a problem");

        if (GameInfo.gameType != 'T')
        {
            if (GameInfo.selectPieceAtStart == 1)
                UpdateTurnMessage(3);
            else
                UpdateTurnMessage(4);
        }
    }
    #endregion

    #region Networking functions
    void StartNetworkingGame()
    {
        placingPiece = (GameInfo.selectPieceAtStart == 1) ? false : true;
        networkChat.SetActive(true);
        openChat.SetActive(true);
        NetworkGame();
    }

    void NetworkGame()
    {
        // Disable everything at the start. They will be enabled later in the function as needed
        DisableEverything();
        Debug.Log(playerTurn);
        // Host's turn
        if (playerTurn == 1)
        {
            Debug.Log("Hosts turn");
            // Host is placing a piece selected by the opponent
            if (placingPiece == true)
            {
                Debug.Log("Host placing a piece");
                UpdateTurnMessage(1);
                EnableAvailableBoardSpaces();
            }
            // Host is choosing a piece for the opponent to place
            else
            {
                UpdateTurnMessage(3);
                Debug.Log("Host choosing opponents piece");
                EnableAvailablePieces();
            }
        }

        // Opponent's turn
        else if (playerTurn == 2)
        {
            Debug.Log("Opponents turn");
            if (isNetworkMove)
            {
                UpdateTurnMessage(2);
                isNetworkMove = false;
            }
            else
            {
                UpdateTurnMessage(4);
                isNetworkMove = true;
            }
            StartCoroutine(networkController.WaitForTurn());
        }
    }

    public void NetworkMessageReceived()
    {
        char messageType = networkController.GetNetworkMessage();

        Debug.Log("Message type = " + messageType);
        if (messageType == 'M')
            ReceiveMoveFromNetwork();
        else if (messageType == 'P')
        {
            ReceivePieceFromNetwork();
        }
        else
            Debug.Log("Yall broke something in network");

        NetworkGame();
    }

    public void ReceivePieceFromNetwork()
    {
        GamePiece pieceSelected = new GamePiece();

        foreach (GamePiece piece in gamePieces)
        {
            if (piece.id == networkController.GetMovePiece())
                pieceSelected = piece;
        }

        selectedPiece = null;
        SetSelectedPieceFromNetwork(pieceSelected);
    }

    public void SetSelectedPieceFromNetwork(GamePiece gamePiece)
    {
        selectedPiece = gamePiece;
        selectedPiece.transform.GetChild(0).gameObject.SetActive(false);
        SelectNetworkOpponentsPiece();
    }

    public void SelectNetworkOpponentsPiece()
    {
        StagePiece();
        ChangeSides();
        placingPiece = true;
        NetworkGame();
    }

    public void ReceiveMoveFromNetwork()
    {
        Button networkButton = buttonList[0];

        foreach (GamePiece piece in gamePieces)
        {
            if (piece.id == networkController.GetMovePiece())
                selectedPiece = piece;
        }

        foreach (Button button in buttonList)
        {
            if (button.name == networkController.GetMoveLocation())
                networkButton = button;
        }

        RemoveHighlightBoardspace();
        selectedPiece.GetComponent<BoxCollider2D>().enabled = false;
        GameObject gamePiece = GameObject.Find("GamePiece " + selectedPiece.id);
        Vector3 newPosition = networkButton.transform.position;
        recentMove = networkButton;
        networkButton.interactable = false;

        Hashtable pieceAnimationArgs = new Hashtable()
            {
                {"position", newPosition},
                {"time", .9f},
                {"oncomplete", "boardSpaceSoundAndHilight" },
                {"oncompletetarget", this.gameObject}
            };

        iTween.MoveTo(gamePiece, pieceAnimationArgs);

        // Returns a specific char if game is over 
        char gameState = gameCore.SetPiece(selectedPiece.id, networkButton.name);

        if (gameState == 'W' || gameState == 'T')
            GameOver(gameState);
    }

    public void PlayerLeft()
    {
        gameSceneManagerObject.GetComponent<GameSceneManager>().showPlayerLeft();
    }

    public void PlayerDisconnected()
    {
        gameSceneManagerObject.GetComponent<GameSceneManager>().showPlayerDisconnected();
    }

    #endregion

    #region Story Mode Functions
    void StoryModeGame()
    {
        // do stuff
    }
    #endregion

    #region AI Functions
    // NOTE: Do we want to add a short (three - five seconds) opening at start of an ai gamescreen?
    private void StartAIGame()
    {
        if (GameInfo.storyModeType == 'E' || GameInfo.gameType == 'E')
            Debug.Log("easy ai game");
        else if (GameInfo.storyModeType == 'H' || GameInfo.gameType == 'H')
            Debug.Log("hard ai game");

        Debug.Log("Start ai game");

        // Player 1 (human) selects first piece
        if (playerTurn == 1)
        {
            Debug.Log("player started");
            if (GameInfo.gameType == 'E' || GameInfo.storyModeType == 'E')
                EasyAIGame();
            else
                HardAIGame();
            // NOTE: Include some UI to inform user to select a piece
        }
        // Player 2 (ai) selects first piece
        else
        {
            Debug.Log("Ai started");
            if (GameInfo.gameType == 'E' || GameInfo.storyModeType == 'E')
                EasyAIGame();
            else
                HardAIGame();
            // NOTE: Include some UI to inform user that the ai has already selected a piece
        }
    }

    void EasyAIGame()
    {
        // Player's turn
        if (playerTurn == 1)
        {
            // Player is placing a piece selected by the AI
            if (placingPiece == true)
            {
                Debug.Log("User placing a piece");
                UpdateTurnMessage(1);
                //Make gameboard interactable, gamepieces not interactable
                EnableAvailableBoardSpaces();
                DisableAllPieces();
            }
            // Player is choosing a piece for the AI to place
            else
            {
                Debug.Log("User choosing opponents piece");
                UpdateTurnMessage(3);
                DisableAllBoardSpaces();
                EnableAvailablePieces();
            }
        }
        // AI's turn
        else
        {
            DisableAllPieces();

            // AI is placing a piece by the user
            if (placingPiece == true)
            {
                Debug.Log("Easy AI placing a piece");
                UpdateTurnMessage(2);
                string aiBoardSpaceChosen = easyAIController.ChooseLocation(gameCore.GetGameBoard(), gameCore.availableBoardSpaces, gameCore.usedBoardSpaces, gameCore.availablePieces, selectedPiece.id, recentMove.name);
                Button boardSpace = ConvertAIBoardSpace(aiBoardSpaceChosen);
                StartCoroutine("DelayAIMove", boardSpace);
            }
            // AI is choosing a piece for the Player to place
            else
            {
                Debug.Log("Easy AI choosing opponents piece");
                UpdateTurnMessage(4);
                // Have ai pick piece
                string aiPieceChosen = easyAIController.ChooseGamePiece(gameCore.availablePieces);
                ConvertAIPiece(aiPieceChosen);
                StartCoroutine("DelayAIGivePiece");
            }
        }
    }

    //WARNING!! THIS IS NOT COMPLETE
    void HardAIGame()
    {
        // Player's turn
        if (playerTurn == 1)
        {
            // Player is placing a piece selected by the AI
            if (placingPiece == true)
            {
                Debug.Log("User placing a piece");
                UpdateTurnMessage(1);
                //Make gameboard interactable, gamepieces not interactable
                EnableAvailableBoardSpaces();
                DisableAllPieces();
            }
            // Player is choosing a piece for the AI to place
            else
            {
                UpdateTurnMessage(3);
                Debug.Log("User choosing opponents piece");
                DisableAllBoardSpaces();
                EnableAvailablePieces();
            }
        }
        // AI's turn
        else
        {
            DisableAllPieces();

            // AI is placing a piece by the user
            if (placingPiece == true)
            {
                Debug.Log("Hard AI placing a piece");
                UpdateTurnMessage(2);
                string aiBoardSpaceChosen = hardAIController.ChooseLocation(gameCore.GetGameBoard(), gameCore.availableBoardSpaces, gameCore.usedBoardSpaces, gameCore.availablePieces, selectedPiece.id, recentMove.name);
                Button boardSpace = ConvertAIBoardSpace(aiBoardSpaceChosen);
                StartCoroutine("DelayAIMove", boardSpace);
            }
            // AI is choosing a piece for the Player to place
            else
            {
                Debug.Log("Hard AI choosing opponents piece");
                UpdateTurnMessage(4);
                // Have ai pick piece
                string aiPieceChosen = hardAIController.ChooseGamePiece(gameCore.availablePieces);
                ConvertAIPiece(aiPieceChosen);
                StartCoroutine("DelayAIGivePiece");
            }
        }
    }

    // NOTE: Remove this delay after Levi gets a legit AI integrated
    IEnumerator DelayAIMove(Button boardSpace)
    {
        yield return new WaitForSeconds(GameInfo.aiDelayBoardSpace);
        PlacePieceOnBoard(boardSpace);
    }

    IEnumerator DelayAIGivePiece()
    {
        yield return new WaitForSeconds(GameInfo.aiDelayPiece);
        StagePiece();
        EndTurn();
    }

    public void ConvertAIPiece(string aiPieceChosen)
    {
        string gamePieceString = "GamePiece " + aiPieceChosen;
        selectedPiece = GameObject.Find(gamePieceString).GetComponent<GamePiece>();
    }

    public Button ConvertAIBoardSpace(string aiBoardSpaceChosen)
    {
        string boardSpaceString = "Board Space " + aiBoardSpaceChosen;
        return GameObject.Find(boardSpaceString).GetComponent<Button>();
    }
    #endregion

    #region Tutorial Functions
    void StartTutorialModeGame()
    {
        tutorialManager = ParretPopup.GetComponent<TutorialManager>();

        // Get the first caption from the array in tutorial manager
        ParrotCaption.text = tutorialManager.getCurrentCaption();

        // Enable Peter Parrot
        StartCoroutine("TutorialShowParrot");

        // Player should not be able to click on any gamepiece or boardspace
        DisableEverything();

        UpdateTurnMessage(3);
    }

    public void TutorialModeGame()
    {
        Button boardSpace;
        GamePiece gamePiece;
        int popupIndex = tutorialManager.GetPopupIndex();
        Button nextArrow = GameObject.Find("NextButton").GetComponent<Button>();

        DisableTutorialNextArrow(nextArrow);
        switch (popupIndex)
        {
            case 3:
                // Have player select piece for opponent
                EnableTutorialPiece();
                break;
            case 4:
                // Inform player opponent will now place piece
                UpdateTurnMessage(2);
                DisableTutorialPiece();
                EnableTutorialNextArrow(nextArrow);
                tutorialPieceIndex = 14;
                break;
            case 5:
                // Have opponent place piece
                UpdateTurnMessage(4);
                boardSpace = GameObject.Find("Board Space C2").GetComponent<Button>();
                PlacePieceOnBoard(boardSpace);
                EnableTutorialNextArrow(nextArrow);
                break;
            case 6:
                // Have opponent give piece and player place piece
                gamePiece = gamePieces[tutorialPieceIndex];
                TutorialSetPiece(gamePiece);
                UpdateTurnMessage(1);
                EnableTutorialBoardSpace();
                break;
            case 7:
                // Jump ahead a few turns
                UpdateTurnMessage(3);
                EnableTutorialNextArrow(nextArrow);
                break;
            case 8:
                // Update gameboard so their is a win condition
                UpdateGameBoard();
                EnableTutorialNextArrow(nextArrow);
                break;
            case 9:
                // Have user select piece to send opponent
                tutorialPieceIndex = 9;
                EnableTutorialPiece();
                break;
            case 10:
                // Have opponent place piece and select player piece
                UpdateTurnMessage(4);
                DisableTutorialPiece();
                boardSpace = GameObject.Find("Board Space A2").GetComponent<Button>();
                PlacePieceOnBoard(boardSpace);
                tutorialPieceIndex = 3;
                EnableTutorialNextArrow(nextArrow);
                break;
            case 11:
                // Have player win
                UpdateTurnMessage(1);
                tutorialBoardSpaceIndex = 5;
                gamePiece = gamePieces[tutorialPieceIndex];
                TutorialSetPiece(gamePiece);
                EnableTutorialBoardSpace();
                break;
            case 12:
                // Maybe include popup or something, for now clicking the next arrow causes an error so dont enable it
                //EnableTutorialNextArrow(nextArrow);
                break;
            default:
                // Enable arrow to go next
                Debug.Log("enable arrow");
                EnableTutorialNextArrow(nextArrow);
                break;
        }
    }

    public void StepCompleted()
    {
        // If its tutorial, show next tutorial text; else its a tooltip
        if (GameInfo.gameType == 'T')
        {
            ParrotCaption.text = tutorialManager.ShowNextStep();
            TutorialModeGame();
        }
        else
        {
            StopCoroutine(hideParrot);
            gameSceneManagerObject.GetComponent<GameSceneManager>().showParrot();
        }
    }

    public void TutorialSetPiece(GamePiece gamePiece)
    {
        Button StagePiece = GameObject.Find("StagePiece").GetComponent<Button>();
        selectedPiece = gamePiece;

        Vector3 newPosition = StagePiece.transform.position;
        selectedPiece.transform.position = newPosition;
    }

    private void UpdateGameBoard()
    {
        selectedPiece = gamePieces[15];
        PlacePieceOnBoard(buttonList[0]);

        selectedPiece = gamePieces[7];
        PlacePieceOnBoard(buttonList[2]);

        selectedPiece = gamePieces[0];
        PlacePieceOnBoard(buttonList[11]);

        selectedPiece = gamePieces[13];
        PlacePieceOnBoard(buttonList[13]);

        selectedPiece = gamePieces[10];
        PlacePieceOnBoard(buttonList[14]);
    }

    #endregion

    #region Turn-Based Functions

    public void IsTutorial(Button button)
    {
        if (GameInfo.gameType != 'T')
            PlacePieceOnBoard(button);
        else
        {
            DisableTutorialBoardSpace();
            PlacePieceOnBoard(button);
            StepCompleted();
        }
    }

    public void PlacePieceOnBoard(Button button)
    {
        string debug = (playerTurn == 1) ? "Player 1 placed a piece" : "Player 2 placed a piece";
        Debug.Log(debug);

        if (selectedPiece != null)
        {
            RemoveHighlightBoardspace();
            selectedPiece.GetComponent<BoxCollider2D>().enabled = false;
            Vector3 newPosition = button.transform.position;
            GameObject gamePiece = GameObject.Find("GamePiece " + selectedPiece.id);
            recentMove = button;
            button.interactable = false;

            Hashtable pieceAnimationArgs = new Hashtable()
            {
                {"position", newPosition},
                {"time", .9f},
                {"oncomplete", "boardSpaceSoundAndHilight" },
                {"oncompletetarget", this.gameObject}
            };

            iTween.MoveTo(gamePiece, pieceAnimationArgs);

            if (GameInfo.gameType == 'N')
            {
                // Send move to other machine on network
                networkController.SetMovePiece(selectedPiece.id);
                networkController.SetMoveLocation(button.name);
                networkController.SendMove();
            }

            // if this is true, game is over
            char gameState = gameCore.SetPiece(selectedPiece.id, button.name);

            if (gameState == 'W' || gameState == 'T')
                GameOver(gameState);
            else
                PiecePlaced();
        }
    }

    public void StagePiece()
    {
        Button StagePiece = GameObject.Find("StagePiece").GetComponent<Button>();
        Button opponentStagePiece = GameObject.Find("OpponentStagePiece").GetComponent<Button>();
        
        if (GameInfo.doubleClickConfirm == true)
            selectedPiece.transform.GetChild(0).gameObject.SetActive(false);

        if (playerTurn == 2)
        {
            Vector3 newPosition = StagePiece.transform.position;
            GameObject gamePiece = GameObject.Find("GamePiece " + selectedPiece.id);
            Hashtable pieceAnimationArgs = new Hashtable()
            {
                {"position", newPosition},
                {"time", 1},
                {"oncomplete", "stagePieceSound"},
                {"oncompletetarget", this.gameObject}
            };

            iTween.MoveTo(gamePiece, pieceAnimationArgs);

        }
        else
        {
            Vector3 newPosition = opponentStagePiece.transform.position;
            GameObject gamePiece = GameObject.Find("GamePiece " + selectedPiece.id);
            Hashtable pieceAnimationArgs = new Hashtable()
            {
                {"position", newPosition},
                {"time", 1},
                {"oncomplete", "stagePieceSound"},
                {"oncompletetarget", this.gameObject}
            };

            iTween.MoveTo(gamePiece, pieceAnimationArgs);
        }
    }

    public void SelectOpponentsPiece()
    {
        StagePiece();

        if (GameInfo.gameType != 'T')
        {
            EndTurn();
        }
        else
        {
            StepCompleted();
        }
    }

    private void PiecePlaced()
    {
        placingPiece = false;
        selectedPiece = null;

        if (GameInfo.gameType == 'E' || GameInfo.storyModeType == 'E')
            EasyAIGame();
        else if (GameInfo.gameType == 'H' || GameInfo.storyModeType == 'H')
            HardAIGame();
        else if (GameInfo.gameType == 'N')
            NetworkGame();
        else
            StoryModeGame();
    }

    public void SetSelectedPiece(GamePiece gamePiece)
    {
        Debug.Log("got into set selected piece");
        // This is always == true unless user changes it in settings
        if (GameInfo.doubleClickConfirm == true)
        {
            if (selectedPiece == gamePiece)
            {
                SelectOpponentsPiece();
            }
            else
            {
                if (selectedPiece != null)
                    selectedPiece.transform.GetChild(0).gameObject.SetActive(false);

                selectedPiece = gamePiece;

                gamePiece.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            if (selectedPiece != null)
                selectedPiece.transform.GetChild(0).gameObject.SetActive(false);

            selectedPiece = gamePiece;
            SelectOpponentsPiece();
        }
    }

    public void ChooseAnotherPiece()
    {
        if (selectedPiece)
        {
            selectedPiece.transform.position = oldPosition;
            selectedPiece = null;
        }
    }

    public List<GameCore.Piece> GetAvailablePieces()
    {
        return gameCore.availablePieces;
    }

    public void EndTurn()
    {
        ChangeSides();
        placingPiece = true;

        if (GameInfo.gameType == 'E' || GameInfo.storyModeType == 'E')
            EasyAIGame();
        else if (GameInfo.gameType == 'H' || GameInfo.storyModeType == 'H')
            HardAIGame();
        else if (GameInfo.gameType == 'N')
        {
            // Send piece to other machine on network
            networkController.SetMovePiece(selectedPiece.id);
            networkController.SendPiece();
            NetworkGame();
        }
        else
            StoryModeGame();
    }

    void GameOver(char endGame)
    {
        // Prevent the user(s) from clicking any boardspace or gamepieces
        DisableEverything();

        GameInfo.isGameOver = true;

        // By default, assume the player lost
        char playerWinStatus = 'L';

        // The player won or tied
        if (playerTurn == 1)
        {
            // Win Condition was met
            if (endGame == 'W')
                playerWinStatus = 'W';
            // Tie 
            else
                playerWinStatus = 'T';
        }

        if (GameInfo.gameType == 'S')
        {
            if (playerWinStatus == 'W')
            {
                if (GameInfo.storyModeType == 'E')
                    gameSceneManagerObject.GetComponent<GameSceneManager>().showStoryModeWinPanel();
                else
                    gameSceneManagerObject.GetComponent<GameSceneManager>().showStoryModeWin2Panel();
            }
            else
                gameSceneManagerObject.GetComponent<GameSceneManager>().showStoryModeLosePanel();
        }
        else if (GameInfo.gameType == 'N')
            gameSceneManagerObject.GetComponent<GameSceneManager>().showNetworkGameOverPanel(playerWinStatus);
        else if (GameInfo.gameType == 'T')
        {
            // Do nothing since they will click button on top to exit (ask tristan if you dont understand)
        }
        else
            gameSceneManagerObject.GetComponent<GameSceneManager>().showGameOverPanel(playerWinStatus);

        if (GameInfo.gameType != 'T')
            DisableTooltips();
    }


    private void ChangeSides()
    {
        playerTurn = (playerTurn == 1) ? 2 : 1;
    }
    #endregion

    #region Enabling/Disabling GameObjects
    private void EnableAvailablePieces()
    {
        foreach (GameCore.Piece availablePiece in gameCore.availablePieces)
            foreach (GamePiece piece in gamePieces)
                if (availablePiece.id == piece.name.Substring(10))
                {
                    piece.GetComponent<BoxCollider2D>().enabled = true;
                    break;
                }
    }

    private void EnableAvailableBoardSpaces()
    {
        foreach (GameCore.BoardSpace availableButton in gameCore.availableBoardSpaces)
            foreach (Button button in buttonList)
                if (availableButton.id == button.name.Substring(12))
                {
                    button.interactable = true;
                    break;
                }
    }

    public void EnablePiecesIfTurn()
    {
        if (playerTurn == 1 && placingPiece == false)
            EnableAvailablePieces();
    }

    private void EnableTutorialNextArrow(Button nextArrow)
    {
        nextArrow.interactable = true;
    }

    private void EnableTutorialBoardSpace()
    {
        buttonList[tutorialBoardSpaceIndex].GetComponent<Button>().interactable = true;
        buttonList[tutorialBoardSpaceIndex].transform.GetChild(0).gameObject.SetActive(true);
    }

    private void EnableTutorialPiece()
    {
        gamePieces[tutorialPieceIndex].GetComponent<BoxCollider2D>().enabled = true;
        gamePieces[tutorialPieceIndex].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DisableAllPieces()
    {
        foreach (GamePiece piece in gamePieces)
        {
            piece.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void DisableAllBoardSpaces()
    {
        foreach (Button button in buttonList)
            button.interactable = false;
    }

    private void DisableEverything()
    {
        DisableAllBoardSpaces();
        DisableAllPieces();
    }

    private void DisableTutorialNextArrow(Button nextArrow)
    {
        nextArrow.interactable = false;
    }

    private void DisableTutorialBoardSpace()
    {
        buttonList[tutorialBoardSpaceIndex].GetComponent<Button>().interactable = false;
        buttonList[tutorialBoardSpaceIndex].transform.GetChild(0).gameObject.SetActive(false);
    }

    private void DisableTutorialPiece()
    {
        Debug.Log("piece disabled");
        gamePieces[tutorialPieceIndex].GetComponent<BoxCollider2D>().enabled = false;
        gamePieces[tutorialPieceIndex].transform.GetChild(0).gameObject.SetActive(false);
    }
    #endregion

    #region Miscellaneous Functions
    void SetGameControllerReferenceOnGamePieces()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            gamePieces[i].GetComponent<GamePiece>().SetGameControllerReference(this);
        }
    }

    void SetGameControllerReferenceOnNetwork()
    {
        networkController.SetGameControllerReference(this);
    }

    IEnumerator FirstGameTooltip()
    {
        yield return new WaitForSeconds(20);
        ParrotCaption.text = tooltips.ShowTooltip();

        gameSceneManagerObject.GetComponent<GameSceneManager>().showParrot();

        if (tooltips.getUsedTooltipLength() != tooltips.tooltips.Length)
            StartCoroutine("FirstGameTooltip");

        hideParrot = StartCoroutine("HideParrot");
    }

    IEnumerator HideParrot()
    {
        yield return new WaitForSeconds(10);

        gameSceneManagerObject.GetComponent<GameSceneManager>().showParrot();
    }

    IEnumerator TutorialShowParrot()
    {
        yield return new WaitForSeconds(1);

        gameSceneManagerObject.GetComponent<GameSceneManager>().showParrot();
    }

    private void DisableTooltips()
    {
        if (GameInfo.firstGame == true)
        {
            // Disable tooltips for next game and from popping up for current game
            GameInfo.firstGame = false;
            StopAllCoroutines();
        }
    }

    private void UpdateTurnMessage(int message)
    {
        switch (message)
        {
            case 1:
                TurnMessage.text = "Your Turn:            Placing Piece";
                break;
            case 2:
                TurnMessage.text = "Opponent's Turn: Placing Piece";
                break;
            case 3:
                TurnMessage.text = "Your Turn: Selecting Oppenents Piece";
                break;
            case 4:
                TurnMessage.text = "Opponent's Turn: Selecting Your Piece";
                break;
        }
    }

    private void HighlightBoardspace()
    {
        recentMove.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void RemoveHighlightBoardspace()
    {
        recentMove.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void boardSpaceSoundAndHilight()
    {
        ButtonClickSound.GetComponent<ButtonClick>().PlaySoundOneShot();

        if (playerTurn == 2)
            HighlightBoardspace();
    }
    private void stagePieceSound()
    {
        ButtonClickSound.GetComponent<ButtonClick>().PlaySoundOneShot();
    }
    #endregion
}