using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayer : NetworkLobbyPlayer
{
    static Color[] _colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
    //used on server to avoid assigning the same color to two player
    static List<int> _colorInUse = new List<int>();

    //public Button ColorButton;
    //public Text NameText;
    //public Button ReadyButton;
    //public Button WaitingPlayerButton;
    //public Button RemovePlayerButton;

    //public GameObject LocalIcone;
    //public GameObject RemoteIcone;

    //OnMyName function will be invoked on clients when server change the value of playerName
    //[SyncVar(hook = "OnMyName")]
    //public string PlayerName = "";
    //[SyncVar(hook = "OnMyColor")]
    //public Color PlayerColor = Color.white;

    public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
    public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

    static Color _joinColor = new Color(255.0f / 255.0f, 0.0f, 101.0f / 255.0f, 1.0f);
    static Color _notReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
    static Color _readyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
    static Color _transparentColor = new Color(0, 0, 0, 0);

    //static Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
    //static Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();


        if (LobbyManager.Instance != null)
        {
            //LobbyManager.Instance.SetServerInfo("Connected", LobbyManager.Instance.networkAddress);
            LobbyManager.Instance.OnPlayersNumberModified(1);
        }
        LobbyPlayerList.Instance.AddPlayer(this);
        LobbyPlayerList.Instance.DisplayDirectServerWarning(isServer && LobbyManager.Instance.matchMaker == null);

        if (isLocalPlayer)
        {
            SetupLocalPlayer();
        }
        else
        {
            SetupOtherPlayer();
        }

        //setup the player data on UI. The value are SyncVar so the player
        //will be created with the right value currently on server
       // OnMyName(PlayerName);
        //OnMyColor(PlayerColor);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        //if we return from a game, color of text can still be the one for "Ready"
        //ReadyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

        SetupLocalPlayer();
    }

    /*void ChangeReadyButtonColor(Color c)
    {
        ColorBlock b = ReadyButton.colors;
        b.normalColor = c;
        b.pressedColor = c;
        b.highlightedColor = c;
        b.disabledColor = c;
        ReadyButton.colors = b;
    }*/

    void SetupOtherPlayer()
    {
        //NameInput.interactable = false;
        //RemovePlayerButton.interactable = NetworkServer.active;

        //ChangeReadyButtonColor(_transparentColor);

        //ReadyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
        //ReadyButton.interactable = false;

        OnClientReady(false);
    }

    void SetupLocalPlayer()
    {
        //NameInput.interactable = true;
        //RemoteIcone.gameObject.SetActive(false);
        //LocalIcone.gameObject.SetActive(true);

        //CheckRemoveButton();

        //if (PlayerColor == Color.white)
        //    CmdColorChange();

        //ChangeReadyButtonColor(_joinColor);
        //ChangeReadyButtonColor(_notReadyColor);

        //ReadyButton.transform.GetChild(0).GetComponent<Text>().text = "JOIN";
        //ReadyButton.interactable = true;

        //have to use child count of player prefab already setup as "this.slot" is not set yet
        //if (PlayerName == "")
        //    CmdNameChanged("Player" + (LobbyPlayerList.Instance.PlayerListContentTransform.childCount - 1));

        //we switch from simple name display to name input
        //ColorButton.interactable = true;
        //NameInput.interactable = true;

        //NameInput.onEndEdit.RemoveAllListeners();
        //NameInput.onEndEdit.AddListener(OnNameChanged);

        //ColorButton.onClick.RemoveAllListeners();
        //ColorButton.onClick.AddListener(OnColorClicked);

        //CmdNameChanged(CrossScene.PlayerName);

        //ReadyButton.onClick.RemoveAllListeners();
        //ReadyButton.onClick.AddListener(OnReadyClicked);

        //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
        //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
        if (LobbyManager.Instance != null) LobbyManager.Instance.OnPlayersNumberModified(0);
    }

    //This enable/disable the remove button depending on if that is the only local player or not
    public void CheckRemoveButton()
    {
        if (!isLocalPlayer)
            return;

        int localPlayerCount = 0;
        foreach (PlayerController p in ClientScene.localPlayers)
            localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

        //RemovePlayerButton.interactable = localPlayerCount > 1;
    }

    public override void OnClientReady(bool readyState)
    {
        if (readyState)
        {
            //ChangeReadyButtonColor(_transparentColor);
            //ChangeReadyButtonColor(_readyColor);

            //Text textComponent = ReadyButton.transform.GetChild(0).GetComponent<Text>();
            //textComponent.text = "READY";
           // textComponent.color = _readyColor;
            //ReadyButton.interactable = false;
            //ColorButton.interactable = false;
            //NameInput.interactable = false;
        }
        else
        {
            //ChangeReadyButtonColor(isLocalPlayer ? _notReadyColor : _transparentColor);
            //ChangeReadyButtonColor(_notReadyColor);

            //Text textComponent = ReadyButton.transform.GetChild(0).GetComponent<Text>();
            //textComponent.text = isLocalPlayer ? "JOIN" : "...";
            //textComponent.color = Color.white;
            //ReadyButton.interactable = isLocalPlayer;
            //ColorButton.interactable = isLocalPlayer;
            //NameInput.interactable = isLocalPlayer;
        }
    }

    public void OnPlayerListChanged(int idx)
    {
        //GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
    }

    ///===== callback from sync var

    /*public void OnMyName(string newName)
    {
        //PlayerName = newName;
        //NameText.text = PlayerName;
    }

    public void OnMyColor(Color newColor)
    {
        PlayerColor = newColor;
        NameText.color = newColor;
        //ColorButton.GetComponent<Image>().color = newColor;
    }*/

    //===== UI Handler

    //Note that those handler use Command function, as we need to change the value on the server not locally
    //so that all client get the new value throught syncvar
    public void OnColorClicked()
    {
        //CmdColorChange();
    }

    public void OnReadyClicked()
    {
        SendReadyToBeginMessage();
    }

    public void OnNameChanged(string str)
    {
        //CmdNameChanged(str);
    }

    /*public void OnRemovePlayerClick()
    {
        if (isLocalPlayer)
        {
            RemovePlayer();
        }
        else if (isServer)
            LobbyManager.Instance.KickPlayer(connectionToClient);

    }*/

    public void ToggleJoinButton(bool enabled)
    {
        //ReadyButton.gameObject.SetActive(enabled);
        //WaitingPlayerButton.gameObject.SetActive(!enabled);
    }

    [ClientRpc]
    public void RpcUpdateCountdown(int countdown)
    {
        //LobbyManager.Instance.countdownPanel.UIText.text = "Match Starting in " + countdown;
        //LobbyManager.Instance.countdownPanel.gameObject.SetActive(countdown != 0);
    }

    [ClientRpc]
    public void RpcUpdateRemoveButton()
    {
        CheckRemoveButton();
    }

    //====== Server Command

    /*[Command]
    public void CmdColorChange()
    {
        int idx = System.Array.IndexOf(_colors, PlayerColor);

        int inUseIdx = _colorInUse.IndexOf(idx);

        if (idx < 0) idx = 0;

        idx = (idx + 1) % _colors.Length;

        bool alreadyInUse = false;

        do
        {
            alreadyInUse = false;
            for (int i = 0; i < _colorInUse.Count; ++i)
            {
                if (_colorInUse[i] == idx)
                {//that color is already in use
                    alreadyInUse = true;
                    idx = (idx + 1) % _colors.Length;
                }
            }
        }
        while (alreadyInUse);

        if (inUseIdx >= 0)
        {//if we already add an entry in the colorTabs, we change it
            _colorInUse[inUseIdx] = idx;
        }
        else
        {//else we add it
            _colorInUse.Add(idx);
        }

        PlayerColor = _colors[idx];
    }*/
/*

    [Command]
    public void CmdNameChanged(string name)
    {
        PlayerName = name;
    }
*/

    //Cleanup thing when get destroy (which happen when client kick or disconnect)
    public void OnDestroy()
    {
        // remove player from list of players


        LobbyPlayerList.Instance.RemovePlayer(this);
        if (LobbyManager.Instance != null) LobbyManager.Instance.OnPlayersNumberModified(-1);

        /*int idx = System.Array.IndexOf(_colors, PlayerColor);

        if (idx < 0)
            return;

        for (int i = 0; i < _colorInUse.Count; ++i)
        {
            if (_colorInUse[i] == idx)
            {//that color is already in use
                _colorInUse.RemoveAt(i);
                break;
            }
        }*/
    }
}