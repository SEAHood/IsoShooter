﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
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

    private LobbyHook _lobbyHook;

    //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
    //of players, so that even client know how many player there is.
    [HideInInspector]
    public int PlayerNumber = 0;


    // Use this for initialization
    void Start()
    {
        _lobbyHook = GetComponent<LobbyHook>();
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
    
    public void AddLocalPlayer()
    {
        TryToAddPlayer();
    }

    //allow to handle the (+) button to add/remove player
    public void OnPlayersNumberModified(int count)
    {
        PlayerNumber += count;

        var localPlayerCount = 0;
        foreach (PlayerController p in ClientScene.localPlayers)
            localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

        //addPlayerButton.SetActive(localPlayerCount < maxPlayersPerConnection && PlayerNumber < maxPlayers);
    }


    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        //This hook allows you to apply state data from the lobby-player to the game-player
        //just subclass "LobbyHook" and add it to the lobby object.

        if (_lobbyHook != null)
            _lobbyHook.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

        return true;
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


    public override void OnLobbyServerPlayersReady()
    {
        //ChangeTo(null);
        //transform.Find("UI").gameObject.SetActive(false);
        Debug.Log("Everyone ready!");
        base.OnLobbyServerPlayersReady();

    }

    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        if (SceneManager.GetSceneAt(0).name == lobbyScene)
        {
            ChangeTo(MainMenuPanel);
        }
        else
        {
            ChangeTo(null);

            //Destroy(GameObject.Find("MainMenuUI(Clone)"));
        }
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        var obj = Instantiate(lobbyPlayerPrefab.gameObject);

        var newPlayer = obj.GetComponent<LobbyPlayer>();
        //newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers);
        newPlayer.ToggleJoinButton(true);


        for (var i = 0; i < lobbySlots.Length; ++i)
        {
            var p = lobbySlots[i] as LobbyPlayer;

            if (p != null)
            {
                p.RpcUpdateRemoveButton();
                p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
            }
        }

        return obj;
    }
}