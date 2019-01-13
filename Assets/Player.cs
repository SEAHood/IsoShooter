using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	void Start () {
        // todo move all isLocalPlayer stuff here?
		transform.Find("Camera").gameObject.SetActive(isLocalPlayer);
	}
}
