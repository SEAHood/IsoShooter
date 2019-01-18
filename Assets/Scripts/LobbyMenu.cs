using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour {
    
    public LobbyManager Manager;
    public Button HostButton;
    public Button JoinButton;
    public RectTransform JoinPanel;


    void Start()
    {
        HostButton.onClick.AddListener(OnClickHost);
        JoinButton.onClick.AddListener(OnClickJoin);
    }

    public void OnClickHost()
    {
        Manager.StartHost();
    }

    public void OnClickJoin()
    {
        Manager.ChangeTo(JoinPanel);
    }
}
