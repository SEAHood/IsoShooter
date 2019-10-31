using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinMenu : MonoBehaviour {

    public LobbyManager Manager;
    public Button JoinButton;
    public Button BackButton;
    public RectTransform MainMenuPanel;
    public RectTransform LobbyPanel;
    public InputField IpInput;

    void Start()
    {
        JoinButton.onClick.AddListener(OnClickJoin);
        BackButton.onClick.AddListener(OnClickBack);
    }

    public void OnClickBack()
    {
        Manager.ChangeTo(MainMenuPanel);
    }

    public void OnClickJoin()
    {
        Manager.ChangeTo(LobbyPanel);
        Manager.networkAddress = IpInput.text;
        Manager.StartClient();
        //Manager.SetServerInfo("Connecting...", Manager.networkAddress);
    }
}
