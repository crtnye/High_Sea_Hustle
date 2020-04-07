using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartRoom : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    #region Variables

    public static StartRoom room;

    public GameObject CreateGameButton;
    public GameObject JoinGameButton;
    public GameObject BackButton;
    public GameObject StartGameButton;
    public GameObject FindGamesButton;
    public GameObject roomListingPrefab;
    public GameObject roomListingPanel;

    public Canvas CreateOrJoinCanvas;
    public Canvas RoomLobbyCanvas;
    public Canvas WaitingLoadingCanvas;
    public Canvas LoadingCanvas;

    public GameObject StatusText;
    public GameObject StartButton;
    
    public Transform roomsPanel;

    public string roomName;

    #endregion

    #region AwakeStartUpdate

    private void Awake()
    {
        room = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("F: StartRoom.cs/private void Start - Connected to photon servers");
        CreateOrJoinCanvas.gameObject.SetActive(true);
        Debug.Log("CreateOrJoinCanvas showing");
    }

    #endregion

    #region PunCallbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("F: StartRoom.cs/public override void OnConnectedToMaster - Connected to photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("AutSyncScene turned on");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("F: StartRoom.cs/public ovveride void OnRoomListUpdate - Change in the rooms available");
        base.OnRoomListUpdate(roomList);
        RemoveRoomListings();

        foreach (RoomInfo room in roomList)
        {
            ListRoom(room);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("F: StartRoom.cs/public override void OnCreateRoomFailed - Room with same name exists");

        // Users with the same name???
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            Debug.Log("Two players connected, ready to start");
        }
    }

    #endregion

    #region Functions

    public void CreateRoom()
    {
        Debug.Log("F: StartRoom.cs/void CreateRoom - Creating new room");
        RoomOptions roomOps = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2
        };

        // Room name needs to be player name???
        roomName = Random.Range(0, 10000).ToString();
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public void RemoveRoomListings()
    {
        Debug.Log("F: StartRoom.cs/void RemoveRoomListing");
        while (roomsPanel.childCount != 0)
        {
            Destroy(roomsPanel.GetChild(0).gameObject);
        }
    }

    public void ListRoom(RoomInfo room)
    {
        Debug.Log("F: StartRoom.cs/void ListRoom");
        if (room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsPanel);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.roomName = room.Name;
            tempButton.SetRoom();
        }
    }


    #endregion

    #region Buttons

    public void OnCreateGameButtonClicked()
    {
        Debug.Log("F: StartRoom.cs/ public void OnCreateGameButtonClicked - Create button clicked");

        while (!PhotonNetwork.IsConnectedAndReady)
            LoadingCanvas.gameObject.SetActive(true);

        LoadingCanvas.gameObject.SetActive(false);

        CreateRoom();

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            Debug.Log("You are master client");

        CreateOrJoinCanvas.gameObject.SetActive(false);
        WaitingLoadingCanvas.gameObject.SetActive(true);
        StartButton.gameObject.SetActive(true);

        GameInfo.selectPieceAtStart = 1;
    }

    public void OnJoinGameButtonClicked()
    {
        Debug.Log("F: StartRoom.cs/ OnJoinGameButtonClicked");

        CreateOrJoinCanvas.gameObject.SetActive(false);
        RoomLobbyCanvas.gameObject.SetActive(true);

        FindGamesButton.gameObject.SetActive(true);
        roomListingPanel.gameObject.SetActive(true);
  
        GameInfo.selectPieceAtStart = 2;
    }

    //public void OnCancelButtonClicked()
    //{
    //    Debug.Log("F: StartRoom.cs/public void OnCancelButtonClicked - Cancel button was clicked");
    //    PhotonNetwork.LeaveRoom();
    //}

    public void JoinLobbyOnClick()
    {
        Debug.Log("F: StartRoom.cs/public void JoinLobbyOnClick");
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    // debug here
    public void OnBackButtonClicked()
    {
        if (CreateOrJoinCanvas.isActiveAndEnabled)
        {
            PhotonNetwork.Disconnect();
            Initiate.Fade("MainMenu", Color.black, 4.0f);
        }

        if (RoomLobbyCanvas.isActiveAndEnabled || WaitingLoadingCanvas.isActiveAndEnabled)
        {
            if (PhotonNetwork.InRoom)
            {
                //if (PhotonNetwork.IsMasterClient)
                //{
                //    PhotonNetwork.CurrentRoom.IsOpen = false;
                //    PhotonNetwork.CurrentRoom.IsVisible = false;
                //}
                
                PhotonNetwork.LeaveRoom();
            }
            CreateOrJoinCanvas.gameObject.SetActive(true);
        }
    }

    public void OnStartGameButtonClicked()
    {
        RoomLobbyCanvas.gameObject.SetActive(false);
        WaitingLoadingCanvas.gameObject.SetActive(true);

        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            StartButton.gameObject.SetActive(false);
        }
        else
        { 
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                StartButton.gameObject.SetActive(true);
            }
        }
    }

    public void OnStartButtonClicked()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    #endregion
}
