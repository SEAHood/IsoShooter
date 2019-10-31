using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUi : MonoBehaviour
{
    private GameObject _bottomPanel;
    private GameObject _lobbyPanel;
    private GameObject _joinPanel;
    private GameObject _hostPanel;
    private LobbyListContent _lobbyList;

    public static MenuUi Instance;

    public void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        _bottomPanel = transform.Find("Container/BottomPanel").gameObject;
        _lobbyPanel = transform.Find("Container/BottomPanel/LobbyUiGroup").gameObject;
        _joinPanel = transform.Find("Container/BottomPanel/JoinUiGroup").gameObject;
        _hostPanel = transform.Find("Container/BottomPanel/HostUiGroup").gameObject;
        _lobbyList = _joinPanel.transform.Find("JoinPanel/JoinScrollList/JoinListViewPort/JoinListContent").GetComponent<LobbyListContent>();
        SwitchToJoin();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchToJoin();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchToHost();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchToLobby();
    }

    // UI Hooks
    // ReSharper disable UnusedMember.Global
    public void HostGameClicked()
    {
        LobbyManager.Instance.HostGame();
        SwitchToLobby();
    }

    public void JoinGameClicked()
    {
        LobbyManager.Instance.JoinGame(_lobbyList.GetSelectedLobby());
        SwitchToLobby();
    }
    // ReSharper enable UnusedMember.Global
    // UI Hooks

    public void OnNewLobbyPlayer()
    {
        Debug.Log("OnNewLobbyPlayer");
    }

    private void DisableAllPanels()
    {
        foreach (Transform childPanel in _bottomPanel.transform)
        {
            childPanel.gameObject.SetActive(false);
        }
    }
    
    public void SwitchToLobby()
    {
        DisableAllPanels();
        _lobbyPanel.SetActive(true);
    }

    public void SwitchToJoin()
    {
        DisableAllPanels();
        _joinPanel.SetActive(true);
    }

    public void SwitchToHost()
    {
        DisableAllPanels();
        _hostPanel.SetActive(true);
    }
}
