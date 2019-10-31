﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkLobbyManager
{
    private bool _lobbyConnected;
    private bool _hostStarted;

    public static LobbyManager Instance;

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
        Instance = this;
        CurrentPanel = MainMenuPanel;
        DontDestroyOnLoad(gameObject);
        //SetServerInfo("Offline", "None");
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

    public void JoinGame(LobbyDto lobby)
    {
        networkAddress = lobby.ip;
        networkAddress = "localhost";
        Debug.Log("Attempting to connect to " + networkAddress);
        StartClient();
    }

    public void HostGame()
    {
        if (_hostStarted) return;
        // TODO Try..catch
        StartHost();
        // TODO Try..catch
        StartCoroutine(Api.CreateLobby($"somelobby{Guid.NewGuid().ToString().Substring(0, 4)}", lobby => { }, error => { }));
        _hostStarted = true;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("Connected to " + conn.address);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
        Debug.Log("Error on " + conn.address + ": " + errorCode);
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

        MenuUi.Instance.SwitchToLobby();

        AddLocalPlayer();
        //backDelegate = StopHostClbk;
        //SetServerInfo("Hosting", networkAddress);
    }
    
    public void AddLocalPlayer()
    {
        TryToAddPlayer();
    }

    //allow to handle the (+) button to add/remove player
    public void OnPlayersNumberModified(int count)
    {
        Debug.Log("OnPlayersNumberModified");
        PlayerNumber += count;

        var localPlayerCount = 0;
        foreach (PlayerController p in ClientScene.localPlayers)
            localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

        //addPlayerButton.SetActive(localPlayerCount < maxPlayersPerConnection && PlayerNumber < maxPlayers);
    }


    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        Debug.Log("OnLobbyServerSceneLoadedForPlayer");
        //This hook allows you to apply state data from the lobby-player to the game-player
        //just subclass "LobbyHook" and add it to the lobby object.

        if (_lobbyHook != null)
            _lobbyHook.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

        return true;
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
        Debug.Log("OnLobbyClientSceneChanged");
        /*if (SceneManager.GetSceneAt(0).name == lobbyScene)
        {
            ChangeTo(MainMenuPanel);
        }
        else
        {
            ChangeTo(null);

            //Destroy(GameObject.Find("MainMenuUI(Clone)"));
        }*/
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("OnLobbyServerCreateLobbyPlayer");
        //MenuUi.OnNewLobbyPlayer();
        var obj = Instantiate(lobbyPlayerPrefab.gameObject);

        /*var newPlayer = obj.GetComponent<LobbyPlayer>();
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
        */

        return obj;
    }



    static bool WantsToQuit()
    {
        if (Instance == null) return true;

        Instance.StartCoroutine(Api.ClearLobby(Application.Quit));
        return false;
    }

    [RuntimeInitializeOnLoadMethod]
    static void RunOnStart()
    {
        Application.wantsToQuit += WantsToQuit;
        EditorApplication.wantsToQuit += WantsToQuit;
    }
}
