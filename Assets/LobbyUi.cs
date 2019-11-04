using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUi : MonoBehaviour
{
    private LobbyDetails _lobbyDetails;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Finding lobby details");
        var test = transform.Find("LobbySidePanel/LobbyDetailsPanel/LobbyDetails");
        _lobbyDetails = transform.Find("LobbySidePanel/LobbyDetailsPanel/LobbyDetails").GetComponent<LobbyDetails>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateDetails(LobbyDto lobby)
    {
        Debug.Log("Populating details");
        _lobbyDetails.Populate(lobby);
    }
}
