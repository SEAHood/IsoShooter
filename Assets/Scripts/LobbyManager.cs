using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : NetworkLobbyManager
{
    public GameObject LobbyUi;

    private bool _lobbyConnected;
    private bool _hostStarted;

    public static LobbyManager Singleton;

    public Text StatusInfo;
    public Text HostInfo;
    public RectTransform MainMenuPanel;
    public RectTransform JoinPanel;
    public RectTransform LobbyPanel;
    protected RectTransform CurrentPanel;

    //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
    //of players, so that even client know how many player there is.
    [HideInInspector]
    public int PlayerNumber = 0;


    // Use this for initialization
    void Start()
    {
        Singleton = this;
        CurrentPanel = MainMenuPanel;
        DontDestroyOnLoad(gameObject);
        SetServerInfo("Offline", "None");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Network Client: " + NetworkClient.active);
        //Debug.Log("Network Server: " + NetworkServer.active);
        //Debug.Log("HostingGame: " + CrossScene.HostingGame);
        /*if (!_hostStarted && CrossScene.HostingGame)
        {
            HostGame();
        }*/
    }

    public void JoinGame()
    {

    }

    public void HostGame()
    {
        if (_hostStarted) return;
        StartHost();
        _hostStarted = true;
    }


    /*public override void OnLobbyStartHost()
    {
        LobbyUI.SetActive(true);
        Debug.Log("OnLobbyStartHost");
        //ServerChangeScene(lobbyScene);
        base.OnLobbyStartHost();
    }*/

    public override void OnStartHost()
    {
        base.OnStartHost();
        Debug.Log("OnStartHost");

        ChangeTo(LobbyPanel);
        AddLocalPlayer();
        //backDelegate = StopHostClbk;
        SetServerInfo("Hosting", networkAddress);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        //LobbyUi.GetComponent<PlayerListManager>().AddPlayer(conn);
        base.OnClientConnect(conn);
    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        //LobbyUi.GetComponent<PlayerListManager>().AddPlayer(conn);
        base.OnLobbyClientConnect(conn);
    }

    /*public override void OnLobbyServerConnect(NetworkConnection conn)
    {
        LobbyUi.GetComponent<PlayerListManager>().AddPlayer(conn);
        base.OnLobbyServerConnect(conn);
    }*/


    public void AddLocalPlayer()
    {
        TryToAddPlayer();
    }

    //allow to handle the (+) button to add/remove player
    public void OnPlayersNumberModified(int count)
    {
        PlayerNumber += count;

        int localPlayerCount = 0;
        foreach (PlayerController p in ClientScene.localPlayers)
            localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

        //addPlayerButton.SetActive(localPlayerCount < maxPlayersPerConnection && PlayerNumber < maxPlayers);
    }

    public void SetServerInfo(string status, string host)
    {
        StatusInfo.text = status;
        HostInfo.text = host;
    }

    public void ChangeTo(RectTransform newPanel)
    {
        if (CurrentPanel != null)
        {
            CurrentPanel.gameObject.SetActive(false);
        }

        if (newPanel != null)
        {
            newPanel.gameObject.SetActive(true);
        }

        CurrentPanel = newPanel;

        /*if (currentPanel != mainMenuPanel)
        {
            backButton.gameObject.SetActive(true);
        }
        else
        {
            backButton.gameObject.SetActive(false);
            SetServerInfo("Offline", "None");
            _isMatchmaking = false;
        }*/
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

        LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();
        newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers);


        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

            if (p != null)
            {
                p.RpcUpdateRemoveButton();
                p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
            }
        }

        return obj;
    }
}
