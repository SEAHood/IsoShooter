using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayerList : MonoBehaviour
{

    public GameObject PlayerRowPrefab;
    
    /*public void AddPlayer(NetworkConnection conn)
    {
        var row = Instantiate(PlayerRowPrefab, transform.Find("PlayerList/PlayerListRows"));
        row.transform.Find("PlayerName").GetComponent<Text>().text = conn.connectionId.ToString();
    }*/

    public static LobbyPlayerList Instance = null;

    public RectTransform PlayerListContentTransform;
    //public GameObject WarningDirectPlayServer;
    //public Transform AddButtonRow;

    protected VerticalLayoutGroup Layout;
    protected List<LobbyPlayer> Players = new List<LobbyPlayer>();

    public void OnEnable()
    {
        Instance = this;
        Layout = PlayerListContentTransform.GetComponent<VerticalLayoutGroup>();
    }

    public void DisplayDirectServerWarning(bool enabled)
    {
        //if (WarningDirectPlayServer != null)
        //    WarningDirectPlayServer.SetActive(enabled);
    }

    void Update()
    {
        //this dirty the layout to force it to recompute evryframe (a sync problem between client/server
        //sometime to child being assigned before layout was enabled/init, leading to broken layouting)

        //if (Layout)
        //    Layout.childAlignment = Time.frameCount % 2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
    }

    public void AddPlayer(LobbyPlayer player)
    {
        Debug.Log("ADDING PLAYER");
        if (Players.Contains(player))
            return;

        Players.Add(player);
        player.PlayerName = Guid.NewGuid().ToString().Substring(0, 5);
        //player.transform.SetParent(PlayerListContentTransform, false);

        //AddButtonRow.transform.SetAsLastSibling();
        var row = Instantiate(PlayerRowPrefab, PlayerListContentTransform);
        row.transform.Find("PlayerName").GetComponent<Text>().text = player.PlayerName;
        row.transform.Find("PlayerReady").GetComponent<Image>().color = player.readyToBegin ? Color.green : Color.red;

        PlayerListModified();
    }

    public void RemovePlayer(LobbyPlayer player)
    {
        Players.Remove(player);
        PlayerListModified();
    }

    public void PlayerListModified()
    {
        int i = 0;
        foreach (LobbyPlayer p in Players)
        {
            p.OnPlayerListChanged(i);
            ++i;
        }
    }
}
