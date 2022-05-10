using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum RegionsCodes
{
    AUTO,
    CAE,
    EU,
    US,
    USW
}


public class ConnectCtrl : MonoBehaviourPunCallbacks
{

    string gameVersion = "1";


    [SerializeField]
    private string regionCode = null;
    [SerializeField]
    private GameObject DropdownColors;
    [SerializeField]
    private GameObject PanelConnect;
    [SerializeField]
    private GameObject PanelRoom;

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 2;


    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Connect()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
            SetButton(false, "FINDING MATCH...");

        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            SetButton(false, "CONNECTING...");
        }
    }

    public void SetRegion(int index)
    {
        RegionsCodes region = (RegionsCodes)index;

        if(region == RegionsCodes.AUTO)
        {
            regionCode = null;
        }
        else
        {
            regionCode = region.ToString();
        }

        Debug.Log("Region: " + regionCode);
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = regionCode;
    }

    public void SetColor(int index)
    {
        string color = GameObject.Find("DropdownColors").GetComponent<Dropdown>().options[index].text;

        Debug.Log("color: " + color);

        var propsToSet = new ExitGames.Client.Photon.Hashtable() { { "color", color } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(propsToSet);

    }

    public void SetReady()
    {

        var propsToSet = new ExitGames.Client.Photon.Hashtable() { { "ready", true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(propsToSet);
    }
    void SetButton(bool state, string msg)
    {
        GameObject.Find("Button").GetComponentInChildren<Text>().text = msg;
        GameObject.Find("Button").GetComponent<Button>().enabled = state;
    }
    void ShowRoomPanel()
    {
        GameObject.Find("PanelConnect").SetActive(false);
        PanelRoom.SetActive(true);
    }



    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        GameObject.Find("DropdownRegions").SetActive(false);
        
        SetButton(true, "LETS BATTLE");
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName +" Now  is in a room.");
        SetButton(false, "WATING PLAYERS");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Room is full");
            ShowRoomPanel();
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Debug.Log(newPlayer.NickName + " Se Ha unido al cuarto, Players: " + PhotonNetwork.CurrentRoom.PlayerCount);


        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 )
        {
            Debug.Log("Room is full");
            ShowRoomPanel();
        }
       
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("ready"))
        {
            int playersReady = 0;

            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                bool ready = (bool)player.CustomProperties["ready"];
                Debug.Log(player.NickName + " is ready? ...." + ready);

                if (ready)
                    playersReady++;
            }

            if(playersReady == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.LoadLevel("Match");
            }
            
        }
        if (changedProps.ContainsKey("color"))
        {

        }

    }

    #endregion


}
