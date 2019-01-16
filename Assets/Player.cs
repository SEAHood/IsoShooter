using System;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [HideInInspector]
    public string PlayerName;

	void Start () {
        // todo move all isLocalPlayer stuff here?
		transform.Find("Camera").gameObject.SetActive(isLocalPlayer);
	    PlayerName = Guid.NewGuid().ToString().Substring(0, 5);

	    if (isServer)
	        GetComponent<PlayerScore>().CurrentScore = 0;
	}
}
