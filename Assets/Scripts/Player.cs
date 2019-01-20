using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [HideInInspector]
    [SyncVar]
    public string PlayerName;

    public Text PlayerNameText;

	void Start () {
        // todo move all isLocalPlayer stuff here?
		transform.Find("Camera").gameObject.SetActive(isLocalPlayer);
	    //PlayerName = Guid.NewGuid().ToString().Substring(0, 5);
	    PlayerNameText.text = PlayerName;

	    if (isServer)
	        GetComponent<PlayerScore>().CurrentScore = 0;
	}
}
