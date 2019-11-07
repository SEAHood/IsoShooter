using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.UI;

public class MenuUi : MonoBehaviour
{
    private GameObject _bottomPanel;
    private GameObject _lobbyPanel;
    private GameObject _joinPanel;
    private GameObject _hostPanel;
    private Button _joinMenuButton;
    private Button _hostMenuButton;
    private LobbyListContent _lobbyList;

    private Color _selectedColor = new Color(1f, 1f, 1f);
    private Color _unselectedColor = new Color(140f / 255f, 140f / 255f, 140f / 255f);
    private ColorBlock _selectedMenuItemColors;
    private ColorBlock _unselectedMenuItemColors;
    private GameObject _activePanel;

    public static MenuUi Instance;

    public void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        _joinMenuButton = transform.Find("Container/TopPanel/Join").gameObject.GetComponent<Button>();
        _hostMenuButton = transform.Find("Container/TopPanel/Host").gameObject.GetComponent<Button>();

        _selectedMenuItemColors = _joinMenuButton.colors;
        _selectedMenuItemColors.normalColor = _selectedColor;
        _unselectedMenuItemColors = _joinMenuButton.colors;
        _unselectedMenuItemColors.normalColor = _unselectedColor;

        _bottomPanel = transform.Find("Container/BottomPanel").gameObject;
        _lobbyPanel = transform.Find("Container/BottomPanel/LobbyUiGroup").gameObject;
        _joinPanel = transform.Find("Container/BottomPanel/JoinUiGroup").gameObject;
        _hostPanel = transform.Find("Container/BottomPanel/HostUiGroup").gameObject;
        _lobbyList = _joinPanel.transform.Find("JoinPanel/JoinScrollList/JoinListViewPort/JoinListContent").GetComponent<LobbyListContent>();
        SwitchToJoin();
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

    public void ReadyClicked()
    {

    }

    public void StartGameClicked()
    {
        LobbyManager.Instance.StartGame();
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
        _activePanel = _lobbyPanel;
    }

    public void SwitchToJoin()
    {
        DisableAllPanels();
        
        _joinMenuButton.colors = _selectedMenuItemColors;
        _hostMenuButton.colors = _unselectedMenuItemColors;

        _joinPanel.SetActive(true);
        _activePanel = _joinPanel;
    }

    public void SwitchToHost()
    {
        DisableAllPanels();

        _hostMenuButton.colors = _selectedMenuItemColors;
        _joinMenuButton.colors = _unselectedMenuItemColors;

        _hostPanel.SetActive(true);
        _activePanel = _hostPanel;
    }

    public void Show()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void Hide()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
