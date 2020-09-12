using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
using UnityEditor;
using TMPro;
using Photon.Realtime;
using System.Linq;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] Transform hunterListContent;
    [SerializeField] Transform survivorListContent;
    [SerializeField] Player player; 
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    public Button chooseHunterButton;
    public Button choosePlayerButton;
    public Button startAfterChoosingRolesButton;
    PhotonView PV;

    public RoomManager rm;
    int survivorCount = 0;
    int selectedCount = 0;
    private void Awake()
    {
        Instance = this;
        player = PhotonNetwork.LocalPlayer;
        PV = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
        
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        
       
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        

       
        MenuManager.Instance.OpenMenu("title");

    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return; 
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < players.Count(); i++)
        {
           
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);


    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: "+message;
        MenuManager.Instance.OpenMenu("error");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");

      


    }
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
       foreach(RoomInfo r in roomList)
        {
            if (r.RemovedFromList) 
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<Roomlistitem>().SetUp(r);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player Entered room");
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void StartGameHunter()
    {
        rm.launchHunter = true;
        PV.RPC("IncrementSelectedCount", RpcTarget.AllBuffered);
        Debug.Log(selectedCount);
        Debug.Log(PhotonNetwork.PlayerList.Length);
        if (selectedCount == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("ActivateStartButton", RpcTarget.AllBuffered);
        }

        Player[] players = PhotonNetwork.PlayerList;

  
        for (int i = 0; i < players.Count(); i++)
        {
            if (players[i].IsLocal)
            {
                choosePlayerButton.interactable = false;
                PV.RPC("addToHunterList", RpcTarget.AllBuffered,players[i]);
                //addToHunterList(players[i]);
            }
        }
        //PhotonNetwork.LoadLevel(1);
    }

    [PunRPC]
    public void addToHunterList(Player player)
    {

        Instantiate(playerListItemPrefab, hunterListContent).GetComponent<PlayerListItem>().SetUp(player);
        chooseHunterButton.interactable = false;
        
    }
    [PunRPC]
    public void addToPlayerList(Player player)
    {
        Debug.Log(player.NickName);
        Instantiate(playerListItemPrefab, survivorListContent).GetComponent<PlayerListItem>().SetUp(player);
        

    }
    public void StartGameSurvivor()
    {
        Player[] players = PhotonNetwork.PlayerList;
        PV.RPC("IncrementSelectedCount", RpcTarget.AllBuffered);
        PV.RPC("IncrementSurvivorCount", RpcTarget.AllBuffered);

        if (selectedCount == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("ActivateStartButton", RpcTarget.AllBuffered);
        }
        if (survivorCount == PhotonNetwork.PlayerList.Length-1)
        {
            PV.RPC("DeactivatePlayerButton", RpcTarget.AllBuffered);
        }


        for (int i = 0; i < players.Count(); i++)
        {
            if (players[i].IsLocal)
            {
                
                choosePlayerButton.interactable = false;
                chooseHunterButton.interactable = false;
                PV.RPC("addToPlayerList", RpcTarget.AllBuffered, players[i]);
            }
        }
        

    }
    [PunRPC]
    public void OpenSelectMenu()
    {
        MenuManager.Instance.OpenMenu("select");
        startAfterChoosingRolesButton.interactable = false;
    }
    [PunRPC]
    public void ActivateStartButton()
    {
        startAfterChoosingRolesButton.interactable = true;
    }
    [PunRPC]
    public void DeactivatePlayerButton()
    {
        choosePlayerButton.interactable = false;
    }
    [PunRPC]
    public void IncrementSurvivorCount()
    {
        survivorCount++; 
    }
    [PunRPC]
    public void IncrementSelectedCount()
    {
        selectedCount++;
    }
    public void OpenSelect()
    {
      
        PV.RPC("OpenSelectMenu", RpcTarget.AllBuffered);
    }

    public void startGameFunction()
    {
        PV.RPC("StartGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void StartGame()
    {
        MenuManager.Instance.OpenMenu("loading");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(2);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
