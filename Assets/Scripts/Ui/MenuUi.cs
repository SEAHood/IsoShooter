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

    }





    // UI Hooks
    // ReSharper disable UnusedMember.Global
    public void HostGameClicked()
    {
        var lobby = new LobbyDto
        {
            name = "Test lobby",
            maxPlayers = 8,
            players = 1,
            ip = "localhost",
            map = "test_map"
        };
        LobbyManager.Instance.HostGame();
        SwitchToLobby(lobby);
    }

    public void JoinGameClicked()
    {
        var lobby = _lobbyList.GetSelectedLobby();
        LobbyManager.Instance.JoinGame(lobby);
        SwitchToLobby(lobby);
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
    
    public void SwitchToLobby(LobbyDto lobby)
    {
        DisableAllPanels();
        _lobbyPanel.SetActive(true);
        _lobbyPanel.GetComponent<LobbyUi>().PopulateDetails(lobby);
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
