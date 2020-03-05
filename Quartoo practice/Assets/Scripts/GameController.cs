using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public List<GamePiece> gamePieces;
    private GameCore gameCore = new GameCore();
    private static NetworkController networkController = new NetworkController();
    AIv1 aiController = new AIv1();
    public Button[] buttonList;
    public GamePiece selectedPiece;
    public Vector3 oldPosition;
    public Button recentMove;
    private int playerTurn;
    private bool placingPiece = false;
    private GameInfo.NetworkGameState networkGameState = GameInfo.NetworkGameState.start;
    public GameObject GameSceneManagerObject;

    void Awake()
    {
        //WARNING!! THESE ARE SET ONLY FOR TESTING!! DELETE THESE LATER!! ONLY TRISTAN
        //CAN DELETE THEM! DONT DELETE WITHOUT ASKING HIM FIRST PUNKS
        //GameInfo.gameType = 'N';
        //GameInfo.selectPieceAtStart = 2;

        //DisableAllBoardSpaces();
        SetGameControllerReferenceOnGamePieces();
        SetGameControllerReferenceOnNetwork();
        playerTurn = GameInfo.selectPieceAtStart;
    }

    void Start()
    {
        if (GameInfo.gameType == 'E' || GameInfo.gameType == 'H')
            StartAIGame();
        else if (GameInfo.gameType == 'N')
            StartNetworkingGame();
        else
            StartStoryModeGame();
    }

    #region Networking functions
    void StartNetworkingGame()
    {
        selectedPiece = GameObject.Find("GamePiece A1").GetComponent<GamePiece>();
        networkGameState = (GameInfo.selectPieceAtStart == 1) ? GameInfo.NetworkGameState.myTurn : GameInfo.NetworkGameState.opponentsTurn;
        placingPiece = (GameInfo.selectPieceAtStart == 1) ? false : true;
        NetworkGame();
    }

    void NetworkGame()
    {
        // Disable everything at the start. They will be enabled later in this function as needed
        DisableAllBoardSpaces();
        DisableAllPieces();
        DisableChooseOptions();

        // Host's turn
        if (networkGameState == GameInfo.NetworkGameState.myTurn)
        {
            Debug.Log("Hosts turn");
            // Host is placing a piece selected by the opponent
            if (placingPiece == true)
            {
                Debug.Log("Host placing a piece");
                EnableAvailableBoardSpaces();

                // ***Var here declaring which board space
                //Make gameboard interactable, gamepieces not interactable

            }
            // Host is choosing a piece for the opponent to place
            else
            {
                Debug.Log("Host choosing opponents piece");
                EnableAvailablePieces();
                EnableChooseOptions();
            }
        }

        // Opponent's turn
        else if (networkGameState == GameInfo.NetworkGameState.opponentsTurn)
        {
            Debug.Log("Opponents turn");

            // recieve move
            // recieve piece to place()
            StartCoroutine(networkController.WaitForTurn());
        }
    }

    public void NetworkMessageReceived()
    {
        char messageType = networkController.GetNetworkMessage();
        Debug.Log("Message type = " + messageType);
        if (messageType == 'M')
        {
            ReceiveMoveFromNetwork();
        }
        else if (messageType == 'P')
        {
            ReceivePieceFromNetwork();
        }

        NetworkGame();
    }

    public void ReceivePieceFromNetwork()
    {
        Debug.Log("piece received from network is " + networkController.GetMovePiece());
        GamePiece pieceSelected = new GamePiece();
        foreach (GamePiece piece in gamePieces)
        {
            if (piece.id == networkController.GetMovePiece())
                pieceSelected = piece;
        }
        selectedPiece = null;
        SetSelectedPiece(pieceSelected);
        ChangeGameStateTurn();
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

        selectedPiece.GetComponent<BoxCollider2D>().enabled = false;
        Vector3 newPosition = networkButton.transform.position;
        selectedPiece.transform.position = newPosition;
        recentMove = networkButton;
        networkButton.interactable = false;


        // if this is true, game is over
        char gameState = gameCore.SetPiece(selectedPiece.id, networkButton.name);

        if (gameState == 'W' || gameState == 'T')
            GameOver(gameState);
    }
    #endregion

    #region Story Mode Functions
    void StartStoryModeGame()
    {

    }

    void StoryModeGame()
    {
        // do stuff
    }
    #endregion

    #region AI Functions
    // NOTE: Do we want to add a short (three - five seconds) opening at start of an ai gamescreen?
    private void StartAIGame()
    {
        Debug.Log("Start ai game");

        // Player 1 (human) selects first piece
        if (playerTurn == 1)
        {
            Debug.Log("player started");
            EasyAIGame();
            // NOTE: Include some UI to inform user to select a piece
        }
        // Player 2 (ai) selects first piece
        else
        {
            Debug.Log("Ai started");
            EasyAIGame();
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

                //Make gameboard interactable, gamepieces not interactable
                DisableChooseOptions();
                EnableAvailableBoardSpaces();
                DisableAllPieces();
            }
            // Player is choosing a piece for the AI to place
            else
            {
                Debug.Log("User choosing opponents piece");

                DisableAllBoardSpaces();
                EnableAvailablePieces();
                EnableChooseOptions();
            }
        }
        // AI's turn
        else
        {
            DisableChooseOptions();
            DisableAllPieces();

            // AI is placing a piece by the user
            if (placingPiece == true)
            {
                Debug.Log("AI placing a piece");

                string aiBoardSpaceChosen = aiController.choosePosition(gameCore.availableBoardSpaces);
                Button boardSpace = ConvertAIBoardSpace(aiBoardSpaceChosen);
                StartCoroutine("DelayAIMove", boardSpace);
            }
            // AI is choosing a piece for the Player to place
            else
            {
                Debug.Log("AI choosing opponents piece");

                // Have ai pick piece
                string aiPieceChosen = aiController.chooseGamePiece(gameCore.availablePieces);
                ConvertAIPiece(aiPieceChosen);
                EndTurn();
            }
        }
    }

    //WARNING!! THIS IS NOT COMPLETE
    void HardAIGame()
    {
        // Player's turn
        if (playerTurn == 1)
        {
            // Have ai pick piece
            EnableAvailableBoardSpaces();
            //Make everything interactable
        }
        // AI's turn
        else
        {
            DisableAllBoardSpaces();
            string aiPieceChosen = aiController.chooseGamePiece(gameCore.availablePieces);
            ConvertAIPiece(aiPieceChosen);
            string aiBoardSpaceChosen = aiController.choosePosition(gameCore.availableBoardSpaces);
            Button boardSpace = ConvertAIBoardSpace(aiBoardSpaceChosen);
            StartCoroutine("DelayAIMove", boardSpace);
        }

    }

    // NOTE: Remove this delay after Levi gets a legit AI integrated
    IEnumerator DelayAIMove(Button boardSpace)
    {
        yield return new WaitForSeconds(2);
        PlacePieceOnBoard(boardSpace);
    }

    public void ConvertAIPiece(string aiPieceChosen)
    {
        string gamePieceString = "GamePiece " + aiPieceChosen;
        SetSelectedPiece(GameObject.Find(gamePieceString).GetComponent<GamePiece>());
    }

    public Button ConvertAIBoardSpace(string aiBoardSpaceChosen)
    {
        string boardSpaceString = "Board Space " + aiBoardSpaceChosen;
        return GameObject.Find(boardSpaceString).GetComponent<Button>();
    }
    #endregion

    #region Turn-Based Functions
    public void PlacePieceOnBoard(Button button)
    {
        string debug = (playerTurn == 1) ? "Player 1 placed a piece" : "Player 2 placed a piece";
        Debug.Log(debug);

        if (selectedPiece != null)
        {
            Vector3 newPosition = button.transform.position;
            selectedPiece.transform.position = newPosition;
            recentMove = button;
            button.interactable = false;
            selectedPiece.GetComponent<BoxCollider2D>().enabled = false;

            if (GameInfo.gameType == 'N')
            {
                networkController.SetMovePiece(selectedPiece.id);
                networkController.SetMoveLocation(button.name);
                networkController.SendMove();
                //Send move to network
                //networkSendMove(selectedPiece.id, button.name)
            }
            // if this is true, game is over
            char gameState = gameCore.SetPiece(selectedPiece.id, button.name);

            if (gameState == 'W' || gameState == 'T')
                GameOver(gameState);
            else
                PiecePlaced();
        }
    }

    public void SelectOpponentsPiece()
    {
        if (selectedPiece)
            EndTurn();
    }

    private void PiecePlaced()
    {
        placingPiece = false;
        selectedPiece = null;

        if (GameInfo.gameType == 'E')
            EasyAIGame();
        else if (GameInfo.gameType == 'H')
            HardAIGame();
        else if (GameInfo.gameType == 'N')
            NetworkGame();
        else
            StoryModeGame();
    }

    public void SetSelectedPiece(GamePiece gamePiece)
    {
        Debug.Log("called set selected piece");
        Button StagePiece = GameObject.Find("StagePiece").GetComponent<Button>();

        if (selectedPiece)
        {
            selectedPiece.transform.position = oldPosition;
            selectedPiece = gamePiece;
            oldPosition = gamePiece.transform.position;
            Vector3 newPosition = StagePiece.transform.position;
            selectedPiece.transform.position = newPosition;
        }
        else
        {
            selectedPiece = gamePiece;
            oldPosition = gamePiece.transform.position;
            Vector3 newPosition = StagePiece.transform.position;
            selectedPiece.transform.position = newPosition;
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

        if (GameInfo.gameType == 'E')
            EasyAIGame();
        else if (GameInfo.gameType == 'H')
            HardAIGame();
        else if (GameInfo.gameType == 'N')
        {
            // send piece to opponent
            // sendPiece(selectedPiece.id)
            networkController.SetMovePiece(selectedPiece.id);
            networkController.SendPiece();
            ChangeGameStateTurn();
            NetworkGame();
        }
        else
            StoryModeGame();
    }

    void GameOver(char endGame)
    {
        Debug.Log("GameOver");

        //by default, assume the player lost
        char playerWinStatus = 'L';

        // May or may not need network game over function
        if (GameInfo.gameType == 'N')
        {
            // tell opponent gameover
        }

        if (playerTurn == 1) //the player won or tied
        {
            // Win Condition was met
            if (endGame == 'W')
            {
                Debug.Log("Game Over: Winner");
                playerWinStatus = 'W';
            }
            // Tie 
            else
            {
                Debug.Log("Game Over: Tie");
                playerWinStatus = 'T';
            }
        }

        if (GameSceneManagerObject != null)
        { 
            GameSceneManagerObject.GetComponent<GameSceneManager>().showGameOverPanel(playerWinStatus);
        }
        else
            Debug.Log("GameSceneManagerObject is null");
    }

    void ChangeSides()
    {
        playerTurn = (playerTurn == 1) ? 2 : 1;
    }

    void ChangeGameStateTurn()
    {
        networkGameState = (networkGameState == GameInfo.NetworkGameState.myTurn) ? GameInfo.NetworkGameState.opponentsTurn : GameInfo.NetworkGameState.myTurn;
        Debug.Log("networkGameState = " + networkGameState);
    }



    #endregion

    #region 
    public void EnableAvailablePieces()
    {
        foreach (GameCore.Piece availablePiece in gameCore.availablePieces)
            foreach (GamePiece piece in gamePieces)
                if (availablePiece.id == piece.name.Substring(10))
                {
                    piece.GetComponent<BoxCollider2D>().enabled = true;
                    break;
                }
    }

    public void EnableChooseOptions()
    {
        Button ChoosePiece = GameObject.Find("ChoosePiece").GetComponent<Button>();
        Button ChooseAnother = GameObject.Find("ChooseAnother").GetComponent<Button>();

        ChooseAnother.interactable = true;
        ChoosePiece.interactable = true;

    }


    public void EnableAvailableBoardSpaces()
    {
        foreach (GameCore.BoardSpace availableButton in gameCore.availableBoardSpaces)
            foreach (Button button in buttonList)
                if (availableButton.id == button.name.Substring(12))
                {
                    button.interactable = true;
                    break;
                }
    }

    public void DisableAllPieces()
    {
        foreach (GamePiece piece in gamePieces)
        {
            piece.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void DisableChooseOptions()
    {
        Button ChoosePiece = GameObject.Find("ChoosePiece").GetComponent<Button>();
        Button ChooseAnother = GameObject.Find("ChooseAnother").GetComponent<Button>();

        ChooseAnother.interactable = false;
        ChoosePiece.interactable = false;

    }

    public void DisableAllBoardSpaces()
    {
        foreach (Button button in buttonList)
            button.interactable = false;
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
    #endregion
}