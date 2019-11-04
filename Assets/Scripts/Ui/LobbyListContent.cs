using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class LoginDto
{
    public string username;
    public string password;
}


public class LobbyListContent : MonoBehaviour
{
    public GameObject LobbyListRow;
    public GameObject EmptyContent;
    public GameObject LoadingContent;
    public JoinLobbyDetails JoinLobbyDetails;

    private LobbyDto _selectedLobby;

    // Start is called before the first frame update
    void Start()
    {
        LoadLobbies();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLobbies()
    {
        StopAllCoroutines();
        ClearLobbies();
        LoadingContent.SetActive(true);
        StartCoroutine(Api.GetLobbies(
            lobbies => {
                LoadingContent.SetActive(false);
                EmptyContent.SetActive(lobbies.Count == 0);

                foreach (var lobby in lobbies)
                {
                    var lobbyListRow = Instantiate(LobbyListRow, transform);
                    lobbyListRow.GetComponent<LobbyListRow>().SetData(lobby);
                }
            },
            () => {
                LoadingContent.SetActive(false);
                Debug.Log("Failed");
            })
        );
    }

    private void ClearLobbies()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void LobbySelected(LobbyDto lobby)
    {
        _selectedLobby = lobby;
        JoinLobbyDetails.Populate(lobby);
    }

    public LobbyDto GetSelectedLobby()
    {
        return _selectedLobby;
    }
}
