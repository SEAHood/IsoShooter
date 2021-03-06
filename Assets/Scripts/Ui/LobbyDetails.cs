﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyDetails : MonoBehaviour
{
    public GameObject EmptyContent;

    private Text _lobbyName;
    private Text _lobbyPlayers;
    private Text _lobbyMap;

    private bool _isHost;

    void Awake()
    {
        _lobbyName = transform.Find("LobbyName").GetComponent<Text>();
        _lobbyPlayers = transform.Find("Detail/Details/LobbyPlayers").GetComponent<Text>();
        _lobbyMap = transform.Find("Detail/Details/LobbyMap").GetComponent<Text>();

        ResetPanel();
    }

    public void SetHost(bool host)
    {
        _isHost = host;
        //_lobbyName.text += host ? " (host)" : " (client)"; 
    }

    public void UpdatePlayerCount(int localPlayerCount)
    {
        _lobbyName = transform.Find("LobbyName").GetComponent<Text>();
        _lobbyName.text = _isHost ? "HOST (" : "CLIENT (";
        _lobbyName.text += localPlayerCount + ")";
    }

    public void Populate(LobbyDto lobby)
    {
        gameObject.SetActive(true);
        EmptyContent.SetActive(false);

        _lobbyName.text = lobby.name;
        _lobbyPlayers.text = $"{lobby.players}/{lobby.maxPlayers}";
        _lobbyMap.text = lobby.map;

    }

    public void ResetPanel()
    {
        gameObject.SetActive(true);
        //EmptyContent.SetActive(true);
    }
}
