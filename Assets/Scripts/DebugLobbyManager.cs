using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugLobbyManager : NetworkLobbyManager
{
    private bool _lobbyConnected;
    private bool _hostStarted;

    public static DebugLobbyManager Instance;
    
    private LobbyHook _lobbyHook;

    //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
    //of players, so that even client know how many player there is.
    [HideInInspector]
    public int PlayerCount = 0;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        _lobbyHook = GetComponent<LobbyHook>();
        Instance = this;
        DontDestroyOnLoad(gameObject);
        HostGame();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void HostGame()
    {
        if (_hostStarted) return;
        // TODO Try..catch
        StartHost();
        // TODO Try..catch
        _hostStarted = true;


    }


    public override void OnStartHost()
    {
        Debug.Log("OnStartHost");
        base.OnStartHost();
        AddLocalPlayer();
    }
    
    public void AddLocalPlayer()
    {
        TryToAddPlayer();
    }
    
    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        if (_lobbyHook != null)
            _lobbyHook.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

        return true;
    }
    
    public void StartGame()
    {
        foreach (var t in lobbySlots)
        {
            var p = t as LobbyPlayer;
            if (p != null) p.RpcReady();

        }
    }


    /*public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        base.OnLobbyClientSceneChanged(conn);
        Debug.Log("OnLobbyClientSceneChanged");
        if (SceneManager.GetSceneAt(0).name == lobbyScene)
        {
            MenuUi.Instance.Show();
        }
        else
        {
            MenuUi.Instance.Hide();
        }
    }*/

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
        Debug.Log("OnLobbyServerCreateLobbyPlayer");
        //MenuUi.OnNewLobbyPlayer();
        var obj = Instantiate(lobbyPlayerPrefab.gameObject);


        return obj;
    }
}
