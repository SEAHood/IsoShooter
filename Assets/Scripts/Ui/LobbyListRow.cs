using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyListRow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Text _lobbyName;
    private Text _lobbyPlayers;
    private Text _lobbyMap;
    private LobbyDto _lobby;
    private LobbyListContent _parentContent;

    // Start is called before the first frame update
    void Awake()
    {
        _lobbyName = transform.Find("LobbyName").GetComponent<Text>();
        _lobbyPlayers = transform.Find("LobbyPlayers").GetComponent<Text>();
        _lobbyMap = transform.Find("LobbyMap").GetComponent<Text>();
        _parentContent = gameObject.transform.parent.GetComponent<LobbyListContent>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetData(LobbyDto lobby)
    {
        _lobby = lobby;
        _lobbyName.text = lobby.name;
        _lobbyPlayers.text = $"{lobby.players}/{lobby.maxPlayers}";
        _lobbyMap.text = lobby.map;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.Find("Selected").gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.Find("Selected").gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _parentContent.LobbySelected(_lobby);
    }
}
